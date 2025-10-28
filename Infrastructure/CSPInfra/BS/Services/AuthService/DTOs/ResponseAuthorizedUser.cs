using Helpers.CommonModels;

namespace BS.Services.AuthService.DTOs
{
    public class ResponseAuthorizedUser
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public IEnumerable<ResponsePolicyByRoleId> RoleAndActions { get; set; } = [];
    }

    public class ResponseListPolicyByRoleId
    {
        public int TotalCount { get; set; }
        public IEnumerable<ResponsePolicyByRoleId> userRoles { get; set; } = [];
    }

    public class ResponsePolicyByRoleId : ResponseGetRole
    {
        public IEnumerable<ResponseGetActionWithDetails> Actions { get; set; } = [];
    }

    public class ResponseRole
    {
        public string Id { get; set; } = "78f4b56a-3fa3-4067-b641-7adb0a7a2ca7";
        public string Name { get; set; } = "SuperAdmin";
        public string Tag { get; set; } = "DefaultAppUserTag";
    }

    public class ResponseGetRole : ResponseRole
    {
        public string CreatedBy { get; set; } = "72990663-2edc-4c10-b331-cd1c65e477e0";
        public string UpdatedBy { get; set; } = "72990663-2edc-4c10-b331-cd1c65e477e0";
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class ResponseGetActionWithDetails : ActivityTrackersInResponse
    {
        public string? PolicyId { get; set; }
        public string? ActionName { get; set; }
        public string? Tags { get; set; }
    }
}
