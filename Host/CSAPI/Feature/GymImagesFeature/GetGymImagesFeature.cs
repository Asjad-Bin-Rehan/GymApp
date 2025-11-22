using BS.Services.GymImagesService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.GymImagesFeature
{
    public class GetGymImagesByGymId : IGymImagesFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("/CSAPI/IGymImagesFeature/GetGymImages/{gymId}", Handle)
               .WithSummary("Get all images for a specific gym")
               .Produces(200)
               .Produces(400)
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            [FromRoute] int gymId,
            [FromServices] IGymImagesService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (gymId <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid Gym ID", 400, null);

                var images = await svc.GetGymImages(gymId, ct);

                return ApiResponseHelper.Convert(true, true, "Images retrieved successfully", 200, images);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                if (ex.Message == "INVALID_GYM_ID")
                    return ApiResponseHelper.Convert(false, false, "Gym ID must be greater than zero", 400, null);

                if (ex.Message == "GYM_NOT_FOUND")
                    return ApiResponseHelper.Convert(false, false, "Gym does not exist", 400, null);

                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
