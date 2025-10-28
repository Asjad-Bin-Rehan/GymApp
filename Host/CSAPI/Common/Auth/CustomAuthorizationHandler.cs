using CustomHTTP;
using DA.Common.CommonRoles;
using Helpers.Auth.Models;
using Helpers.Strings;
using Microsoft.AspNetCore.Authorization;

using CSAPI.Common.Auth.Requirements;


namespace CSAPI.Common.Auth
{
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {
            // Optionally, check the request path

            var httpContext = context.Resource as HttpContext;
            if (httpContext != null)
            {
                var path = httpContext.Request.Path;
                if(!context?.User?.Identity?.IsAuthenticated ?? false)
                {
                    int statusCode = HTTPStatusCode400.Unauthorized;
                    _httpContextAccessor.HttpContext.Response.StatusCode = statusCode;
                    _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
                    await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(ApiResponseHelper.Convert(true, false, $"Invalid jwt or token is expired", statusCode, null));
                    await _httpContextAccessor.HttpContext.Response.CompleteAsync();
                }
                // Optionally, check the request path
                else if (IsUserValidated(httpContext))
                {

                    context.Succeed(requirement);
                }
                else
                {
                    //TODO:loging
                    int statusCode = HTTPStatusCode400.Forbidden;
                    _httpContextAccessor.HttpContext.Response.StatusCode = statusCode;
                    _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
                    await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(ApiResponseHelper.Convert(true, false, $"User is not authorized to access this resource: {path}", statusCode, null));
                    await _httpContextAccessor.HttpContext.Response.CompleteAsync();

                    context.Fail();

                }
            }
            else
            {
                //TODO:loging
                int statusCode = HTTPStatusCode500.ServiceUnavailable;
                _httpContextAccessor.HttpContext.Response.StatusCode = statusCode;
                _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
                await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(ApiResponseHelper.Convert(true, false, "", statusCode, null));
                await _httpContextAccessor.HttpContext.Response.CompleteAsync();

                context.Fail();
            }

            
          
        }

        private static bool IsUserValidated(HttpContext context)
        {

            var userTypeClaim = context.User.Claims.FirstOrDefault(x => x.Type == KAuthClaimTypes.UserType)?.Value;
            if (string.IsNullOrEmpty(userTypeClaim))
            {
                return false;
            }

            if (userTypeClaim == KDefinedRoles.SuperAdmin)
            {
                return true;
            }

            var resourceClaim = context.User.Claims.FirstOrDefault(x => x.Type == KAuthClaimTypes.Resources)?.Value;
            if (string.IsNullOrEmpty(resourceClaim))
            {
                return false;
            }

            var actions = resourceClaim.Split(KConstantToken.Separator);
            if (actions.Length == 0)
            {
                return false;
            }

            var currentUrl = context.Request.Path.Value.ToLower().ToShortenUrl();
            return actions.Any(action => action.ToLower().Equals(currentUrl));



        }
    }
}
