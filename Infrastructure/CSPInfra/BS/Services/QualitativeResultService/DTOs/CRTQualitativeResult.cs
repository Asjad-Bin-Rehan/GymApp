using DM.DomainModels;

namespace BS.Services.QualitativeResultService.DTOs;

public static class CRTQualitativeResult
{
    #region AddQualitativeResult
    public static Qualitative_Result ToDomain(this AddQualitativeResultDTO request, string userId)
    {
        return new Qualitative_Result()
        {
            ResultDescription = request.ResultDescription,
            IsActive = request.IsActive,
            Id = Guid.NewGuid().ToString(),
            CreatedBy = userId,
            CreatedDate = DateTime.UtcNow,
            UpdatedBy = userId,
            UpdatedDate = DateTime.UtcNow,
            IsArchived = false,
        };
    }
    #endregion AddQualitativeResult
    
    #region ListAllQualitativeResults
    private static ResponseQualitativeResult ToResponse(this Qualitative_Result row)
    {
        return new ResponseQualitativeResult()
        {
            ResultDescription = row.ResultDescription,

            Id = row.Id,
            IntCode = row.IntCode,
            IsActive = row.IsActive,
            CreatedDate = row.CreatedDate,
            CreatedBy = row.CreatedBy,
            UpdatedBy = row.UpdatedBy,
            UpdatedDate = row.UpdatedDate,
            IsArchived = row.IsArchived
        };

    }

    public static List<ResponseQualitativeResult> ToResponseList(this IEnumerable<Qualitative_Result> rows)
    {
        return rows.Select(x => x.ToResponse()).ToList();
    }
    #endregion ListAllQualitativeResults
}