using BS.Services.ItemSampleService.DTO;
using DA;
using DM.DomainModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.ItemSampleService;

public class ItemSampleService : IItemSampleService
{
    private IUnitOfWork _uow;
    public ItemSampleService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<ResponseItemSample> GetItemSampleByItemId(string ItemId, CancellationToken ct)
    {
        var query = _uow.item_sample.GetQueryable();
        var result = await query.Data
                                .Include(x => x.Item)
                                .OrderByDescending(x => x.CreatedDate)
                                .Where(x => x.ItemId == ItemId)
                                .FirstOrDefaultAsync(ct);
        ArgumentFalseException.ThrowIfFalse(result != null, "No records found");
        var data = result?.ToResponse();
        return data;
    }

    public async Task<bool> AddSampleWithRanges(AddItemSampleWithRangesDTO request, string userId, CancellationToken ct)
    {
        var itemSample = request.ToDomain(userId);
        await _uow.item_sample.AddAsync(itemSample, userId, ct);
        
        foreach (var sampleRange in request.SamplingRangeObjects)
        {
            var sample = request.ToDomain(userId, sampleRange.LotSizeMin, sampleRange.LotSizeMax, sampleRange.SampleQty, sampleRange.CriticalDefects, sampleRange.MajorDefects, sampleRange.MinorDefects, itemSample.Id);
            await _uow.sampling_range.AddAsync(sample, userId, ct);
        }

        await _uow.CommitAsync();
        return true;
    }
    
    public async Task<List<ResponseItemSample>> ListAllItemSamplesWithItems(CancellationToken ct, int lastCount, int skipRecords)
    {
        var query = _uow.item_sample.GetQueryable();
        var result = await query.Data
            .Include(x => x.Item)
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);
        ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
        var data = result.ToResponseList().ToList();
        return data;
    }

    public async Task<List<ResponseItemSample>> GetItemSampleById(string itemSampleId, CancellationToken ct)
    {
        var query = _uow.item_sample.GetQueryable();
        var result = await query.Data
            .Include(x => x.Item)
            .OrderByDescending(x => x.CreatedDate)
            .Where(x =>  x.Id == itemSampleId)
            .ToListAsync(ct);
        ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
        var data = result.ToResponseList().ToList();
        return data;
    }

    public async Task<List<ResponseItemSampleWithSamplingRanges>> ListRangesBySampleId(string sampleId, CancellationToken ct, int lastCount, int skipRecords)
    {
        var query = _uow.item_sample.GetQueryable();
        var result = await query.Data
                                .Include(x => x.Sampling_Ranges)
                                .Where(x => x.Id == sampleId)
                                .OrderByDescending(x => x.CreatedDate)
                                .Skip(skipRecords)
                                .Take(lastCount)
                                .ToListAsync(ct);
        ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
        var data = result.Select(x => x.ToResponse(true)).ToList();
        return data;
    }

    public async Task<bool> UpdateSampleWithRanges(UpdateItemSampleDTO request, string userId, CancellationToken ct)
    {
        var result = await _uow.item_sample.GetQueryable().Data.Include(x => x.Sampling_Ranges).Where(x => x.Id==request.Id).SingleOrDefaultAsync(ct) ?? throw new ArgumentFalseException("No active record found to update");

        foreach (var samplingRangeResult in result.Sampling_Ranges)
        {
            var samplingRangeLog = samplingRangeResult.ToDomain(result, userId);
            await _uow.log_sampling_range.AddAsync(samplingRangeLog, userId, ct);
        }

        result.Flexibility = request.Flexibility;
        await _uow.item_sample.UpdateAsync(result, userId, ct);

        foreach (var samplingResultRangeObject in request.SamplingRangeObjects)
        {
            if (!string.IsNullOrEmpty(samplingResultRangeObject.Id)) await UpdateExistingRange(result, samplingResultRangeObject, userId, ct);
            else await AddNewRange(result, samplingResultRangeObject, userId, ct);
        }

        ValidateOverallRanges(result);
        await _uow.CommitAsync();
        return true;
    }

    #region UpdateSampleWithRanges Helper Functions
    private async Task UpdateExistingRange(Item_Sample itemSample, AttachSamplingRangeObject samplingRange, string userId, CancellationToken ct)
    {
        var existingRange = itemSample.Sampling_Ranges.FirstOrDefault(r => r.Id == samplingRange.Id);
        if (existingRange != null)
        {
            if (!samplingRange.IsActive) existingRange.IsActive = false;
            else
            {
                existingRange.LotSizeMin = samplingRange.LotSizeMin;
                existingRange.LotSizeMax = samplingRange.LotSizeMax;
                existingRange.SampleQty = samplingRange.SampleQty;
                existingRange.CriticalDefects = samplingRange.CriticalDefects;
                existingRange.MajorDefects = samplingRange.MajorDefects;
                existingRange.MinorDefects = samplingRange.MinorDefects;
                existingRange.IsActive = true;
            }
            await _uow.sampling_range.UpdateAsync(existingRange, userId, ct);
        }
    }

    private async Task AddNewRange(Item_Sample itemSample, AttachSamplingRangeObject samplingRange, string userId, CancellationToken ct)
    {
        var entity = samplingRange.ToDomain(itemSample, userId);
        await _uow.sampling_range.AddAsync(entity, userId, ct);
    }

    private void ValidateOverallRanges(Item_Sample itemSample)
    {
        var allRanges = itemSample.Sampling_Ranges.Where(r=>r.IsActive).Select(r=>new{r.LotSizeMin,r.LotSizeMax}).OrderBy(r=>r.LotSizeMin).ToList();
        for (int i = 0; i < allRanges.Count - 1; i++)
        {
            if (allRanges[i+1].LotSizeMin <= allRanges[i].LotSizeMax) throw new ArgumentException("Ranges cannot overlap with existing");
        }
    }
    #endregion UpdateSampleWithRanges Helper Functions

    #region Custom FluentValidations
    public async Task<bool> IsItemIdAvailable(string? itemId, CancellationToken ct)
    {
        var result = await _uow.item.AnyAsync(ct, x => x.Id == itemId);
        return result.Data;
    }

    public async Task<bool> IsSampleIdAvailable(string? Id, CancellationToken ct)
    {
        var result = await _uow.item_sample.AnyAsync(ct, x => x.Id == Id);
        return result.Data;
    }

    public async Task<bool> IsItemSampleUsed(string Id, CancellationToken ct)
    {
        var itemSample = await _uow.item_sample.GetQueryable().Data.Where(x => x.Id == Id).FirstOrDefaultAsync();
        var query = _uow.item.GetQueryable();
        var result = await query.Data.Where(x => itemSample != null && x.Id == itemSample.ItemId).Include(x => x.Production_QC).Include(x => x.Purchase_QC).FirstOrDefaultAsync();
        return (result?.Purchase_QC?.Any() ?? false) || (result?.Production_QC?.Any() ?? false);
    }

    public async Task<bool> IsSampleItemExists(string? itemId, CancellationToken ct)
    {
        var result = await _uow.item_sample.AnyAsync(ct, x => x.ItemId == itemId);
        return result.Data;
    }
    #endregion Custom FluentValidations
}