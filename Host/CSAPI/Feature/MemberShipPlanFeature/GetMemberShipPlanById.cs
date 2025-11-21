using BS.Services.MembershipPlanService;
using BS.Services.MembershipPlanService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.MemberShipPlanFeature
{
    public class GetMembershipPlanById : IMembershipPlanFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetMembershipPlanById)}/{{planId:int}}", Handle)
            .WithSummary("Get membership plan by plan_id")
            .Produces<ResponseMembershipPlanDTO>()
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromRoute] int planId,
            IMembershipPlanService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            int statusCode = 200;
            string message = "Membership plan retrieved successfully";

            try
            {
                // --- VALIDATION ---
                if (planId <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid plan_id", 400, null);

                // --- SERVICE CALL ---
                var plan = await svc.GetMembershipPlanByIdRaw(planId, ct);

                if (plan == null)
                    return ApiResponseHelper.Convert(false, false, "Membership plan not found", 404, null);

                return ApiResponseHelper.Convert(true, true, message, statusCode, plan);
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
