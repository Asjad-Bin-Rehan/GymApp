using BS.Services.MembershipPlanService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.MemberShipPlanFeature
{
    public class DeleteMembershipPlan : IMembershipPlanFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapDelete($"/{nameof(DeleteMembershipPlan)}/{{planId:int}}", Handle)
            .WithSummary("Delete a membership plan by plan_id")
            .Produces<bool>()
            .Produces(200)
            .Produces(404)
            .Produces(400)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromRoute] int planId,
            IMembershipPlanService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            if (planId <= 0)
                return ApiResponseHelper.Convert(false, false, "Invalid plan_id", 400, null);

            try
            {
                var result = await svc.DeleteMembershipPlanRaw(planId, ct);

                if (!result)
                    return ApiResponseHelper.Convert(false, false, "plan_id not found", 404, null);

                return ApiResponseHelper.Convert(true, true, "Membership plan deleted successfully", 200, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
