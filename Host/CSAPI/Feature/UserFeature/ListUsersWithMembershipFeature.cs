using BS.Services.UserService;
using BS.Services.UserService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.UserFeature
{
    public class ListUsersWithMembership : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListUsersWithMembership)}", Handle)
            .WithSummary("Lists users with membership & payment details from a VIEW")
            .Produces<List<ViewUserMembershipDTO>>()
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromQuery] int limit,
            [FromQuery] int offset,
            IUserService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            int statusCode = 200;
            string message = "Success";

            try
            {
                var users = await svc.ListUsersWithMembershipRaw(limit, offset, ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, users);
            }
            catch (Exception ex)
            {
                statusCode = 500;
                message = "Something went wrong";
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
