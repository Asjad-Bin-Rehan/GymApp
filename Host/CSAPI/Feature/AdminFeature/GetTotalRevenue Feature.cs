using BS.Services.AdminService;
using BS.Services.AdminService.DTOs;
using CSAPI.Common;
using CSAPI.Feature.AdminFeature;
using CustomHTTP;
using Logger;

namespace CSAPI.Feature.RevenueFeature
{
    public class GetTotalRevenue : IAdminFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetTotalRevenue)}", Handle)
            .WithSummary("Get total revenue from all paid subscriptions")
            .Produces<ResponseTotalRevenueDTO>()
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle(
            IAdminService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var result = await svc.GetTotalRevenueRaw(ct);

                return ApiResponseHelper.Convert(true, true, "Success", 200, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
