using DM.DomainModels;

namespace BS.Services.ItemSampleService.DTO;

public static class CRTItemSample
{
    #region Update Item Sample With Ranges
    public static Log_Sampling_Range ToDomain(this Sampling_Range samplingRangeResultObject, Item_Sample resultObject, string userId)
    {
        return new Log_Sampling_Range()
        {
            Id = Guid.NewGuid().ToString(),
            ItemSampleId = resultObject.Id,
            ItemSampleFlexibility = resultObject.Flexibility,
            SamplingRangeId = samplingRangeResultObject.Id,
            LotSizeMin = samplingRangeResultObject.LotSizeMin,
            LotSizeMax = samplingRangeResultObject.LotSizeMax,
            SampleQty = samplingRangeResultObject.SampleQty,
            MajorDefects = samplingRangeResultObject.MajorDefects,
            MinorDefects = samplingRangeResultObject.MinorDefects,
            CriticalDefects = samplingRangeResultObject.CriticalDefects,

            CreatedBy = userId,
            CreatedDate = DateTime.UtcNow,
            UpdatedBy = userId,
            UpdatedDate = DateTime.UtcNow,
            IsActive = true,
            IsArchived = false
        };
    }

    public static Sampling_Range ToDomain(this AttachSamplingRangeObject samplingRange, Item_Sample itemSample, string userId)
    {
        return new Sampling_Range()
        {
            ItemSampleId = itemSample.Id,
            LotSizeMin = samplingRange.LotSizeMin,
            LotSizeMax = samplingRange.LotSizeMax,
            SampleQty = samplingRange.SampleQty,
            CriticalDefects = samplingRange.CriticalDefects,
            MajorDefects = samplingRange.MajorDefects,
            MinorDefects = samplingRange.MinorDefects,

            Id = Guid.NewGuid().ToString(),
            CreatedBy = userId,
            CreatedDate = DateTime.UtcNow,
            UpdatedBy = userId,
            UpdatedDate = DateTime.UtcNow,
            IsActive = samplingRange.IsActive,
            IsArchived = false,
        };
    }
    #endregion Update Item Sample With Ranges

    #region Add Item Sample With Ranges
    public static Item_Sample ToDomain(this AddItemSampleWithRangesDTO request, string userId)
    {
        return new Item_Sample()
        {
            ItemId = request.ItemId,
            ItemDescription = request.ItemDescription,
            Flexibility = request.Flexibility,

            Id = Guid.NewGuid().ToString(),
            CreatedBy = userId,
            CreatedDate = DateTime.UtcNow,
            UpdatedBy = userId,
            UpdatedDate = DateTime.UtcNow,
            IsActive = request.IsActive,
            IsArchived = false
        };
    }

    public static Sampling_Range ToDomain(this AddItemSampleWithRangesDTO request, string userId, int? pLotSizeMin, int? pLotSizeMax, int? pSampleQty, int? pCriticalDefects, int? pMajorDefects, int? pMinorDefects, string? pItemSampleId)
    { 
        return new Sampling_Range()
        {
            ItemSampleId = pItemSampleId,            
            LotSizeMin = pLotSizeMin,
            LotSizeMax = pLotSizeMax,
            SampleQty = pSampleQty,
            CriticalDefects = pCriticalDefects,
            MajorDefects = pMajorDefects,
            MinorDefects = pMinorDefects,
            
            Id = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedBy = userId,
            CreatedDate = DateTime.UtcNow,
            UpdatedBy = userId,
            UpdatedDate = DateTime.UtcNow,
            IsArchived = false
        };
    }
    #endregion Add Item Sample With Ranges

    #region List Ranges By Sample ID
    public static ResponseItemSampleWithSamplingRanges ToResponse(this Item_Sample itemSample, bool IsOverloaded)
    {
        return new ResponseItemSampleWithSamplingRanges
        {
            Id = itemSample.Id,
            IntCode = itemSample.IntCode,
            ItemDescription = itemSample.ItemDescription,
            Flexibility = itemSample.Flexibility,
            ItemId = itemSample.ItemId,

            CreatedDate = itemSample.CreatedDate,
            CreatedBy = itemSample.CreatedBy,
            UpdatedDate = itemSample.UpdatedDate,
            UpdatedBy = itemSample.UpdatedBy,
            IsActive = itemSample.IsActive,
            SamplingRangeObjects = itemSample.Sampling_Ranges
            .OrderBy(sr => sr.LotSizeMin)
            .Select(samplingRange => new ResponseItemSampleWithSamplingRanges.SamplingRangeObject
            {
                Id = samplingRange.Id,
                LotSizeMin = samplingRange.LotSizeMin,
                LotSizeMax = samplingRange.LotSizeMax,
                SampleQty = samplingRange.SampleQty,
                CriticalDefects = samplingRange.CriticalDefects,
                MajorDefects = samplingRange.MajorDefects,
                MinorDefects = samplingRange.MinorDefects,
                IsActive = samplingRange.IsActive
            }).ToList()
        };
    }
    #endregion List Ranges By Sample ID

    #region List All Item Sample
    public static ResponseItemSample ToResponse(this Item_Sample row)
    {
        return new ResponseItemSample()
        {
            // Item Sample

            Id = row.Id,
            IntCode = row.IntCode,
            ItemDescription = row.ItemDescription,
            Flexibility = row.Flexibility,
            ItemId = row.ItemId,

            CreatedBy = row.CreatedBy,
            CreatedDate = row.CreatedDate,
            UpdatedBy = row.UpdatedBy,
            UpdatedDate = row.UpdatedDate,
            IsActive = row.IsActive,
            IsArchived = row.IsArchived,

            // Item
            ItemCode = row?.Item?.ItemCode,
            Name = row?.Item?.Name,
            GroupCode = row?.Item?.GroupCode,
            Type = row?.Item?.Type,
            IsEnabledForQA = row?.Item?.IsEnabledForQA,
            UoMGroupEntry = row?.Item?.UoMGroupEntry,
            U_QACard = row?.Item?.U_QACard,
            IsBatch = row?.Item?.IsBatch,
            GroupName = row?.Item?.GroupName,
            ManageBatchNumbers = row?.Item?.ManageBatchNumbers,
            PackSize = row?.Item?.PackSize,
        };
    }
    public static List<ResponseItemSample> ToResponseList(this IEnumerable<Item_Sample> rows)
    {
        return rows.Select(x => x.ToResponse()).ToList();
    }
    #endregion List All Item Cards
}