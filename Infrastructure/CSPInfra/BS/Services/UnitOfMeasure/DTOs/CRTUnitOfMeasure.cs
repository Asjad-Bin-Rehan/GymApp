using DM.DomainModels;

namespace BS.Services.UnitOfMeasure.DTOs
{
    public static class CRTUnitOfMeasure
    {
        #region Add Unit Of Measure
        public static Unit_Of_Measure ToDomain(this AddUnitOfMeasureDTO request, string userId)
        {
            return new Unit_Of_Measure()
            {
                Description = request.Description,
                UoMcode = request.UoMCode,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = request.IsActive,
                IsArchived = false
            };
        }
        #endregion Add Unit Of Measure

        #region List All Unit Of Measures
        public static ResponseUnitOfMeasure ToResponse(this Unit_Of_Measure row)
        {
            return new ResponseUnitOfMeasure()
            {
                Description = row.Description,
                IntCode = row.IntCode,
                UoMCode = row.UoMcode,

                Id = row.Id,
                CreatedDate = row.CreatedDate,
                CreatedBy = row.CreatedBy,
                UpdatedBy = row.UpdatedBy,
                UpdatedDate = row.UpdatedDate,
                IsActive = row.IsActive,
                IsArchived = row.IsArchived
            };
        }
        public static List<ResponseUnitOfMeasure> ToResponseList(this IEnumerable<Unit_Of_Measure> rows)
        {
            return rows.Select(x => x.ToResponse()).ToList();
        }
        #endregion List All Unit Of Measures
    }
}
