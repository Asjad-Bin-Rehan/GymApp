using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ItemSampleService;
using BS.Services.ItemSampleService.DTO;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using static BS.Services.ItemSampleService.DTO.AddItemSampleWithRangesDTO;

namespace CSAPI.Feature.ItemSampleFeature
{
    public class UpdateSampleWithRanges : IItemSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                               .MapPut($"/{nameof(UpdateSampleWithRanges)}", Handle)
                               .WithSummary("Update or Soft-Delete Item Sample with Ranges from DB")
                               .Produces(200)
                               .Produces(400)
                               .Produces(402)
                               .Produces(500)
                               .WithRequestValidation<UpdateItemSampleDTO>()
                               .Produces<bool>();

        public class RequestValidator : AbstractValidator<UpdateItemSampleDTO>
        {
            IItemSampleService _svc;
            public RequestValidator(IItemSampleService svc)
            {
                _svc = svc;

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("ID is required.")
                    .Must(x => x.Trim() == x).WithMessage("ID can not contain leading or trailing spaces.")
                    //.Must(IsUnUsed).WithMessage("Already being used so can't update.")
                    ;

                RuleFor(x => x.Flexibility).NotEmpty().WithMessage("Flexibility is required.");

                RuleForEach(x => x.SamplingRangeObjects).SetValidator(new SamplingRangeValidator());

                RuleFor(x => x.SamplingRangeObjects).Must(IsNonOverlapping).WithMessage("The sampling ranges cannot overlap.");
            }

            #region Custom FluentValidations
            private bool IsNonOverlapping(List<AttachSamplingRangeObject> ranges)
            {
                if (ranges == null || ranges.Count == 0) return true;
                var sortedRanges = ranges.OrderBy(x => x.LotSizeMin).ToList();
                for (int i = 0; i < sortedRanges.Count - 1; i++)
                {
                    if (sortedRanges[i + 1].LotSizeMin <= sortedRanges[i].LotSizeMax) return false;
                }
                return true;
            }

            //async Task<bool> IsUnUsed(string Id, CancellationToken ct)
            //{
            //    return !await _svc.IsItemSampleUsed(Id, ct);
            //}
            #endregion Custom FluentValidations
        }

        public class SamplingRangeValidator : AbstractValidator<AttachSamplingRangeObject>
        {
            public SamplingRangeValidator()
            {
                RuleFor(x => x.LotSizeMin).NotNull().WithMessage("LotSizeMin is required.");
                RuleFor(x => x.LotSizeMax).NotNull().WithMessage("LotSizeMax is required.");
                RuleFor(x => x.SampleQty).NotNull().WithMessage("SampleQty is required.");

                RuleFor(x => x)
                    .Must(x => x.LotSizeMin!.Value < x.LotSizeMax!.Value)
                    .WithMessage("LotSizeMin must be less than LotSizeMax.")
                    .When(x => x.LotSizeMin.HasValue && x.LotSizeMax.HasValue);
            }
        }

        private static async Task<IResult> Handle([FromBody] UpdateItemSampleDTO req, IUserContext user, ICustomLogger logger, IItemSampleService svc, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.UpdateSampleWithRanges(req, user.Data.UserId ?? "Anonymous", cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                return ApiResponseHelper.Convert(true, false, message, statusCode, false);
            }
            catch (Exception e)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                logger.LogError(e.Message);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}