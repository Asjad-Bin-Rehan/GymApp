using BS.Services.UserService;
using CSAPI.Common;
using CSAPI.Feature.UserFeature;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace BS.Features.UserFeature
{
    public class GetUserCount : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("/Users/Count", Handle)
               .WithName("GetUserCount")
               .WithSummary("Returns total number of registered users");
        }

        private static async Task<IResult> Handle(
            [FromServices] IUserService userService,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                // 1️⃣ Validate DI service
                if (userService == null)
                {
                    return ApiResponseHelper.Convert(false, false,
                        "User service not available", 500, null);
                }

                // 2️⃣ Fetch count
                var countDto = await userService.GetUserCountRaw(ct);

                // 3️⃣ Validate result
                if (countDto == null)
                {
                    return ApiResponseHelper.Convert(false, false,
                        "Failed to fetch user count", 500, null);
                }

                if (countDto.total_users < 0)
                {
                    return ApiResponseHelper.Convert(false, false,
                        "Invalid user count value returned", 500, null);
                }

                // 4️⃣ Success response
                return ApiResponseHelper.Convert(true, true,
                    "User count fetched successfully", 200, countDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                return ApiResponseHelper.Convert(false, false,
                    "Something went wrong", 500, null);
            }
        }
    }
}
