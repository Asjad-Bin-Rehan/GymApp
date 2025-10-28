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
    public class AddSampleWithRanges : IItemSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddSampleWithRanges)}", Handle)
            .WithSummary("Add an Item Sample with Sampling Ranges to DB")
            .Produces(201)
            .Produces(402)
            .Produces(500)
            .WithRequestValidation<AddItemSampleWithRangesDTO>()
            .Produces<string>();


        public class RequestValidator : AbstractValidator<AddItemSampleWithRangesDTO>
        {
            IItemSampleService _svc;
            public RequestValidator(IItemSampleService svc)
            {
                _svc = svc;

                RuleFor(x => x.ItemId).NotEmpty().WithMessage("Item ID is required")
                    .MustAsync(IsNotExists).WithMessage("Item ID cannot be duplicate.")
                    .MustAsync(IsItemIdAvailable).WithMessage("Item ID not found.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Item ID cannot be only whitespaces.")
                    .Must(x => x?.Trim() == x).WithMessage("Item ID can not contain leading or trailing spaces.")
                    ;

                RuleFor(x => x.ItemDescription).NotEmpty().WithMessage("Item Description is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Item Description cannot be only whitespaces.")
                    .Must(x => x?.Trim() == x).WithMessage("Item Description can not contain leading or trailing spaces.")
                    ;

                RuleFor(x => x.Flexibility).NotEmpty().WithMessage("Flexibility is required.");

                //RuleFor(x => x.SamplingRangeObjects).NotEmpty().WithMessage("At least one sampling range must be provided.");

                RuleForEach(x => x.SamplingRangeObjects).SetValidator(new SamplingRangeValidator());

                RuleFor(x => x.SamplingRangeObjects).Must(IsNonOverlapping).WithMessage("The defined sampling ranges must not overlap.");
            }

            #region Custom FluentValidations
            async Task<bool> IsItemIdAvailable(string? id, CancellationToken ct)
            {
                return await _svc.IsItemIdAvailable(id, ct);
            }

            async Task<bool> IsNotExists(string? ItemId, CancellationToken ct)
            {
                return !await _svc.IsSampleItemExists(ItemId, ct);
            }

            private bool IsNonOverlapping(List<SamplingRangeContract> ranges)
            {
                var sortedRanges = ranges.OrderBy(x => x.LotSizeMin).ToList();
                for (int i = 0; i < sortedRanges.Count - 1; i++)
                {
                    if (sortedRanges[i+1].LotSizeMin <= sortedRanges[i].LotSizeMax)
                    {
                        return false;
                    }
                }
                return true;
            }
            #endregion Custom FluentValidations
        }

        public class SamplingRangeValidator : AbstractValidator<SamplingRangeContract>
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

        private static async Task<IResult> Handle([FromBody] AddItemSampleWithRangesDTO req, IUserContext user, ICustomLogger logger, IItemSampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddSampleWithRanges(req, user.Data.UserId ?? "Anonymous", ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
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