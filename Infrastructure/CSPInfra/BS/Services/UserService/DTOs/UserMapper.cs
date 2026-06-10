using DM.DomainModels;

namespace BS.Services.UserService.DTOs
{
    public static class UserMapper
    {
        public static User ToInsert(this UpsertUserObject request, string userId)
        {
            return new User
            {
                Name = request.Name,
                Email = request.Email,
                UserType = request.UserType,
                OrganizationId = request.OrganizationId,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }

        public static User ToUpdate(this User existing, UpsertUserObject obj)
        {
            existing.Name = obj.Name;
            existing.Email = obj.Email;
            existing.UserType = obj.UserType;
            existing.OrganizationId = obj.OrganizationId;
            existing.IsActive = obj.IsActive;
            return existing;
        }

        public static GetUserResponse ToResponse(this User row)
        {
            return new GetUserResponse
            {
                Id = row.Id,
                Name = row.Name,
                Email = row.Email,
                UserType = row.UserType,
                OrganizationId = row.OrganizationId,
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
