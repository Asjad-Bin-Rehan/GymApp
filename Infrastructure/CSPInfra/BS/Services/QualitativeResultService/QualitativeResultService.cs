using BS.Services.QualitativeResultService.DTOs;
using DA;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.QualitativeResultService;

public class QualitativeResultService : IQualitativeResultService
{
    private IUnitOfWork _uow;
    public QualitativeResultService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }
    
    public async Task<bool> AddQualitativeResult(AddQualitativeResultDTO request, string userId, CancellationToken ct)
    {
        var qualitativeResult = request.ToDomain(userId);
        await _uow.qualitative_result.AddAsync(qualitativeResult, userId, ct);
        await _uow.CommitAsync();
        return true;
    }
    
    public async Task<List<ResponseQualitativeResult>> ListAllQualitativeResults(CancellationToken ct, int lastCount, int skipRecords)
    {
        var query = _uow.qualitative_result.GetQueryable();
        var result = await query.Data
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);
        ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
        var data = result.ToResponseList().ToList();
        return data;
    }

    public async Task<List<ResponseQualitativeResult>> GetQualitativeResultById(string id, CancellationToken ct)
    {
        var query = _uow.qualitative_result.GetQueryable();
        var result = await query.Data
            .OrderByDescending(x => x.CreatedDate)
            .Where(x => x.Id == id)
            .ToListAsync(ct);
        ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
        var data = result.ToResponseList().ToList();
        return data;
    }

    public async Task<bool> UpdateQualitativeResult(UpdateQualitativeResultDTO request, string userId, CancellationToken ct)
    {
        var query = _uow.qualitative_result.GetQueryable();

        var alreadyExists = await query.Data.AnyAsync(x => x.Id != request.Id && x.ResultDescription!=null && request.ResultDescription!=null && x.ResultDescription.ToLower() == request.ResultDescription.ToLower());
        ArgumentFalseException.ThrowIfFalse(!alreadyExists, "Details already exist for another record");

        var result = await query.Data.Where(x => x.Id == request.Id).SingleOrDefaultAsync(ct) ?? throw new ArgumentFalseException("No active record found to update");
        result.ResultDescription = request.ResultDescription;
        result.IsActive = request.IsActive;
        await _uow.qualitative_result.UpdateAsync(result, userId, ct);
        await _uow.CommitAsync();
        return true;
    }

    #region Custom FluentValidations
    public async Task<bool> IsQualitativeResultUsed(string Id, CancellationToken ct)
    {
        var query = _uow.qualitative_result.GetQueryable();
        var result = await query.Data.Where(x => x.Id == Id).Include(x => x.Qualitative_Result_Pass_Statuses).FirstOrDefaultAsync();
        return result?.Qualitative_Result_Pass_Statuses.Any() ?? false;
    }

    public async Task<bool> IsQualitativeResultDescriptionExists(string? resultDescription, CancellationToken ct)
    {
        var query = _uow.qualitative_result.GetQueryable();
        return await query.Data.Where(x => x.ResultDescription!=null && resultDescription!=null && x.ResultDescription.ToLower() == resultDescription.ToLower()).AnyAsync();
    }
    #endregion Custom FluentValidations
}