using DM.DomainModels;

namespace BS.Services.ProductionQACavityService.DTOs
{
    public static class CRTProductionQACavity
    {
        public static Production_QA_Cavity ToDomain(this AddProductionQACavityDTO request, string userId, int i)
        {
            return new Production_QA_Cavity
            {
                Id = Guid.NewGuid().ToString(),
                QaId = request.QaId,
                Name = request.Name + i.ToString(),
                InspectionBy = request.InspectionBy,
                InspectionDateTime = request.InspectionDateTime,
                MouldNo = request.MouldNo,
                IsToggledOn = request.IsToggledOn,
                IsCavityPassed = request.IsCavityPassed,

                IsActive = request.IsActive,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false
            };
        }

        public static ResponseProductionQACavity ToResponse(this Production_QA_Cavity entity)
        {
            return new ResponseProductionQACavity
            {
                Id = entity.Id,
                QaId = entity.QaId,
                Name = entity.Name,
                InspectionBy = entity.InspectionBy,
                InspectionDateTime = entity.InspectionDateTime,
                MouldNo = entity.MouldNo,
                IsToggledOn = entity.IsToggledOn,
                IsCavityPassed = entity.IsCavityPassed,

                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedDate = entity.CreatedDate,
                UpdatedBy = entity.UpdatedBy,
                UpdatedDate = entity.UpdatedDate,
                IsArchived = entity.IsArchived
            };
        }

        public static List<ResponseProductionQACavity> ToResponseList(this IEnumerable<Production_QA_Cavity> entities)
        {
            return entities.Select(x => x.ToResponse()).ToList();
        }

        public static Log_Production_QA_Cavity ToDomain(this Production_QA_Cavity entity, string userId)
        {
            return new Log_Production_QA_Cavity()
            {
                Name = entity.Name,
                InspectionBy = entity.InspectionBy,
                InspectionDateTime = entity.InspectionDateTime,
                MouldNo = entity.MouldNo,
                IsToggledOn = entity.IsToggledOn,
                IsCavityPassed = entity.IsCavityPassed,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = entity.IsActive,
                IsArchived = false
            };
        }
    }
}