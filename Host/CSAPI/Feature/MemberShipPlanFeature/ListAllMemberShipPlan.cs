using BS.Services.MembershipPlanService;
using BS.Services.MembershipPlanService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.MemberShipPlanFeature
{
    public class ListAllMembershipPlans : IMembershipPlanFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllMembershipPlans)}", Handle)
            .WithSummary("List all membership plans")
            .Produces<List<ResponseMembershipPlanDTO>>()
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle(IMembershipPlanService svc, ICustomLogger logger, CancellationToken ct)
        {
            int statusCode = 200;
            string message = "Success";

            try
            {
                var plans = await svc.ListAllMembershipPlansRaw(ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, plans);
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
