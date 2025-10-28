using BS.Services.NextIntCodeService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using BS.CustomExceptions.CustomExceptionMessage;

namespace CSAPI.Feature.NextIntCodeFeature
{
    public class GetDeploymentInfo : INextIntCodeFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app.MapGet($"/{nameof(GetDeploymentInfo)}", Handle).WithSummary("Get current branch & migration info").Produces(200).Produces(500).Produces<List<string>>();
        private static async Task<IResult> Handle(ICustomLogger logger, INextIntCodeService svc, CancellationToken ct)
        {
            try
            {
                List<string> deploymentInfo = [
                    "BRANCH : Usaid/staging",
                    "COMMIT : 13-4-2025 | Migration - LogPostSap & GetDeploymentInfo",
                    "MIGRATION : 13-4-2025 | LogPostSap",
                ];
                return ApiResponseHelper.Convert(true, true, "Success", HTTPStatusCode200.Ok, deploymentInfo);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return ApiResponseHelper.Convert(false, false, ExceptionMessage.SWW, HTTPStatusCode500.InternalServerError, null);
            }
        }
    }
}