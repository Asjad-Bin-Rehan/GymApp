using DM.DomainModels;

namespace BS.Services.CustomerProfileService.DTOs
{
    public static class CustomerProfileMapper
    {
        public static CustomerProfile ToInsert(this UpsertCustomerProfileObject request, string userId)
        {
            return new CustomerProfile
            {
                UserId = request.UserId,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }

        public static CustomerProfile ToUpdate(this CustomerProfile existing, UpsertCustomerProfileObject obj)
        {
            existing.UserId = obj.UserId;
            existing.IsActive = obj.IsActive;
            return existing;
        }

        public static GetCustomerProfileResponse ToResponse(this CustomerProfile row)
        {
            return new GetCustomerProfileResponse
            {
                Id = row.Id,
                UserId = row.UserId,
                CreatedDate = row.CreatedDate,
                CreatedBy = row.CreatedBy,
                UpdatedBy = row.UpdatedBy,
                UpdatedDate = row.UpdatedDate,
                IsActive = row.IsActive,
                IsArchived = row.IsArchived
            };
        }
    }
}
