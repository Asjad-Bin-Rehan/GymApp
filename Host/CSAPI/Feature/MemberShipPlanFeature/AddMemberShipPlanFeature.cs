using BS.Services.MembershipPlanService;
using BS.Services.MembershipPlanService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.MemberShipPlanFeature
{
    public class AddMembershipPlan : IMembershipPlanFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddMembershipPlan)}", Handle)
            .WithSummary("Add a new membership plan")
            .Produces<bool>()
            .Produces(200)
            .Produces(400)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromBody] AddMembershipPlanDTO request,
            IMembershipPlanService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            int statusCode = 200;
            string message = "Membership plan added successfully";

            try
            {
                // --- VALIDATIONS ---
                if (string.IsNullOrWhiteSpace(request.plan_name))
                    return ApiResponseHelper.Convert(false, false, "plan_name is required", 400, null);

                if (request.plan_name.Length > 100)
                    return ApiResponseHelper.Convert(false, false, "plan_name cannot exceed 100 characters", 400, null);

                if (request.duration_months <= 0)
                    return ApiResponseHelper.Convert(false, false, "duration_months must be greater than 0", 400, null);

                if (request.price < 0)
                    return ApiResponseHelper.Convert(false, false, "price cannot be negative", 400, null);

                if (!string.IsNullOrEmpty(request.description) && request.description.Length > 500)
                    return ApiResponseHelper.Convert(false, false, "description cannot exceed 500 characters", 400, null);

                // --- SERVICE CALL ---
                var result = await svc.AddMembershipPlanRaw(request, ct);

                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
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
