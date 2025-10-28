using DM.DomainModels;

namespace BS.Services.InspectionAttributeService.DTOs
{
    public static class CRTInspectionAttribute
    {
        #region Add
        public static Inspection_Attribute ToDomain(this AddInspectionAttributeDTO dto, string userId)
        {
            return new Inspection_Attribute
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Description = dto.Description,
                Tag = dto.Tag,

                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }
        #endregion Add

        #region ListAll
        public static ResponseInspectionAttribute ToResponse(this Inspection_Attribute row)
        {
            return new ResponseInspectionAttribute
            {
                Id = row.Id,
                IntCode = row.IntCode,
                Name = row.Name,
                Description = row.Description,
                Tag = row.Tag,

                CreatedBy = row.CreatedBy,
                CreatedDate = row.CreatedDate,
                UpdatedBy = row.UpdatedBy,
                UpdatedDate = row.UpdatedDate,
                IsActive = row.IsActive,
                IsArchived = row.IsArchived
            };
        }

        public static IEnumerable<ResponseInspectionAttribute> ToResponseList(this IEnumerable<Inspection_Attribute> rows)
        {
            return rows.Select(x => x.ToResponse());
        }
        #endregion ListAll
    }
}
