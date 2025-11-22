using BS.Services.GymImagesService;
using BS.Services.GymImagesService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.GymImagesFeature
{
    public class AddGymImage : IGymImagesFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("/CSAPI/IGymImagesFeature/AddGymImage", Handle)
               .WithSummary("Add a new image for a gym")
               .Produces(200)
               .Produces(400)
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            [FromBody] AddGymImageDTO request,
            [FromServices] IGymImagesService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                // Input validation
                if (request.gym_id <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid Gym ID", 400, null);

                if (string.IsNullOrWhiteSpace(request.image_url))
                    return ApiResponseHelper.Convert(false, false, "Image URL is required", 400, null);

                // Call service
                var imageId = await svc.AddGymImage(request, ct);

                return ApiResponseHelper.Convert(true, true, "Image added successfully", 200, new
                {
                    image_id = imageId,
                    gym_id = request.gym_id,
                    image_url = request.image_url,
                    created_at = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                if (ex.Message == "INVALID_GYM_ID")
                    return ApiResponseHelper.Convert(false, false, "Gym ID must be greater than zero", 400, null);

                if (ex.Message == "INVALID_IMAGE_URL")
                    return ApiResponseHelper.Convert(false, false, "Invalid image URL", 400, null);

                if (ex.Message == "GYM_NOT_FOUND")
                    return ApiResponseHelper.Convert(false, false, "Gym does not exist", 400, null);

                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
