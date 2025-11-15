using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CSAPI.Common.Constant;
using CSAPI.Common.Filters;
using CSAPI.Common.Auth;
using CSAPI.Common;
using CSAPI.Feature.QualitativeResultFeature;
using CSAPI.Feature.UnitOfMeasureFeature;
using CSAPI.Feature.InspectionCharacteristicFeature;
using CSAPI.Feature.NextIntCodeFeature;
using CSAPI.Feature.ItemCardFeature;
using CSAPI.Feature.InspectionCardFeature;
using CSAPI.Feature.ItemSampleFeature;
using CSAPI.Feature.ItemInspectionCardFeature;
using CSAPI.Feature.PurchaseQCFeature;
using CSAPI.Feature.ProductionQCFeature;
using CSAPI.Feature.ProductionQAFeature;
using CSAPI.Feature.PurchaseQCSampleFeature;
using CSAPI.Feature.ProductionQCSampleFeature;
using CSAPI.Feature.ProductionQACavityFeature;
using CSAPI.Feature.ProductionQACavitySampleFeature;
using CSAPI.Feature.AuthMonolithicFeature;
using CSAPI.Feature.InspectionAttributeFeature;

namespace CSAPI;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup(KConstant.ApiName).AddEndpointFilter<RequestLoggingFilter>().WithOpenApi();

        // Auto-Increment Code & Paginate
        endpoints.MapAuthEndpoints();
        endpoints.MapNextCountEndpoints();

        // Master (Data) Stepper
        endpoints.MapUnitOfMeasureEndpoints();
        endpoints.MapQualitativeResultEndpoints();
        endpoints.MapInspectionAttributeEndpoints();
        endpoints.MapInspectionCharacteristicsEndpoints();
        endpoints.MapItemEndpoints();
        endpoints.MapInspectionCardEndpoints();
        endpoints.MapItemSampleEndpoints();
        endpoints.MapItemInspectionCardEndpoints();

        // Evaluation (Plan) Stepper
        endpoints.MapPurchaseQCEndpoints();
        endpoints.MapProductionQCEndpoints();
        endpoints.MapProductionQAEndpoints();
        endpoints.MapPurchaseQCSampleEndpoints();
        endpoints.MapProductionQCSampleEndpoints();
        endpoints.MapProductionQACavityEndpoints();
        endpoints.MapProductionQACavitySampleEndpoints();
    }

    private static void MapProductionQACavitySampleEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IProductionQACavitySampleFeature)}").WithTags("IProductionQACavitySampleFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllProductionQACavitySamplesByCavityId>()
            .MapEndpoint<AddProductionQACavitySample>()
            .MapEndpoint<GetProductionQACavitySampleById>()
            .MapEndpoint<UpdateProductionQACavitySample>()
        ;
    }

    private static void MapProductionQACavityEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IProductionQACavityFeature)}").WithTags("IProductionQACavityFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllProductionQACavityByQaId>()
            .MapEndpoint<AddProductionQACavity>()
            .MapEndpoint<GetProductionQACavityById>()
            .MapEndpoint<UpdateProductionQACavity>()
        ;
    }

    private static void MapProductionQCSampleEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IProductionQCSampleFeature)}").WithTags("IProductionQCSampleFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllProductionQCSamplesByQcId>()
            .MapEndpoint<AddProductionQCSample>()
            .MapEndpoint<GetProductionQCSampleById>()
            .MapEndpoint<UpdateProductionQCSample>()
        ;
    }

    private static void MapPurchaseQCSampleEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IPurchaseQCSampleFeature)}").WithTags("IPurchaseQCSampleFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllPurchaseQCSamplesByQcId>()
            .MapEndpoint<AddPurchaseQCSample>()
            .MapEndpoint<GetPurchaseQcSampleById>()
            .MapEndpoint<UpdatePurchaseQcSample>()
        ;
    }

    private static void MapProductionQAEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IProductionQAFeature)}").WithTags("IProductionQAFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllProductionQAs>()
            .MapEndpoint<ListAllProductionQAsWithItem>()
            .MapEndpoint<AddProductionQA>()
            .MapEndpoint<GetProductionQAId>()
            .MapEndpoint<GetProductionQAWithItemById>()
            .MapEndpoint<UpdateProductionQA>()
            .MapEndpoint<GetProductionQaBMRByQaId>()
            .MapEndpoint<GetProductionQaReportCavityWiseById>()
        ;
    }

    private static void MapProductionQCEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IProductionQCFeature)}").WithTags("IProductionQCFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllProductionQCs>()
            .MapEndpoint<ListAllProductionQCsWithItem>()
            .MapEndpoint<AddProductionQC>()
            .MapEndpoint<GetProductionQCWithItemById>()
            .MapEndpoint<GetProductionQCId>()
            .MapEndpoint<UpdateProductionQC>()
            .MapEndpoint<GetProductionQcBMRByQcId>()
        ;
    }

    private static void MapPurchaseQCEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IPurchaseQCFeature)}").WithTags("IPurchaseQCFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllPurchaseQCs>()
            .MapEndpoint<ListAllPurchaseQCsWithItem>()
            .MapEndpoint<AddPurchaseQC>()
            .MapEndpoint<GetPurchaseQCWithItemById>()
            .MapEndpoint<GetSampleQuantity>()
            .MapEndpoint<GetPurchaseQCId>()
            .MapEndpoint<UpdatePurchaseQC>()
        ;
    }

    private static void MapItemInspectionCardEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IItemInspectionCardFeature)}").WithTags("IItemInspectionCardFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllCardsWithBothCharacteristics>()
            .MapEndpoint<ListCardByCodeWithBothCharacteristics>()
            .MapEndpoint<GetCardByIdWithBothCharacteristics>()
            .MapEndpoint<GetCardByCodeWithBothCharacteristics>()
            .MapEndpoint<AddItemInspectionCardWithBothInspections>()
            .MapEndpoint<UpdateItemInspectionCard>()
        ;
    }

    private static void MapItemSampleEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IItemSampleFeature)}").WithTags("IItemSampleFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<AddSampleWithRanges>()
            .MapEndpoint<ListAllItemSamplesWithItems>()
            .MapEndpoint<ListRangesBySampleId>()
            .MapEndpoint<GetItemSampleById>()
            .MapEndpoint<GetItemSampleByItemId>()
            .MapEndpoint<UpdateSampleWithRanges>()
            ;
    }

    private static void MapInspectionCardEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IInspectionCardFeature)}").WithTags("IInspectionCardFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<AddInspectionCardWithCharacteristics>()
            .MapEndpoint<ListCharacteristicsByInspectionCardId>()
            .MapEndpoint<ListAllInspectionCards>()
            .MapEndpoint<GetInspectionCardById>()
            .MapEndpoint<UpdateInspectionCard>()
            .MapEndpoint<GetCardByIdWithCharacteristicsWithCriteria>()
            ;
    }

    private static void MapItemEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IItemCardFeature)}").WithTags("IItemCardFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllItems>()
            .MapEndpoint<GetItemById>()
            .MapEndpoint<GetItemByCode>()
            .MapEndpoint<AddItemCard>()
            .MapEndpoint<GetItemTypesById>()
        ;
    }

    private static void MapInspectionCharacteristicsEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IInspectionCharacteristicFeature)}").WithTags("IInspectionCharacteristicFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllInspectionCharacteristics>()
            .MapEndpoint<AddCharacteristicWithCriteria>()
            .MapEndpoint<ListCharacteristicsWithCriteria>()
            .MapEndpoint<UpdateCharacteristic>()
            .MapEndpoint<GetInspectionCharacteristicById>()
            ;
    }

    private static void MapInspectionAttributeEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IInspectionAttributeFeature)}").WithTags("IInspectionAttributeFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllInspectionAttributes>()
            .MapEndpoint<AddInspectionAttribute>()
            .MapEndpoint<UpdateInspectionAttribute>()
            .MapEndpoint<GetInspectionAttributeById>()
        ;
    }

    private static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IUserFeature)}").WithTags("IUserFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<SignupUser>()
            .MapEndpoint<LoginUser>()
            .MapEndpoint<GetUserById>()
            .MapEndpoint<ListAllUsers>()
            .MapEndpoint<AddUserRaw>()      // Admin Add
            .MapEndpoint<DeleteUser>();     // Delete
    }




    private static void MapUnitOfMeasureEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IUnitOfMeasureFeature)}").WithTags("IUnitOfMeasureFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllUnitOfMeasures>()
            .MapEndpoint<AddUnitOfMeasure>()
            .MapEndpoint<GetUnitOfMeasureById>()
            .MapEndpoint<UpdateUnitOfMeasure>()
        ;
    }

    private static void MapQualitativeResultEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IQualitativeResultFeature)}").WithTags("IQualitativeResultFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllQualitativeResults>()
            .MapEndpoint<AddQualitativeResult>()
            .MapEndpoint<UpdateQualitativeResult>()
            .MapEndpoint<GetQualitativeResultById>()
        ;
    }

    private static void MapNextCountEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(INextIntCodeFeature)}").WithTags("INextIntCodeFeature");

        endpoints.MapPublicGroup()
             .MapEndpoint<AddLogPostSap>()
             .MapEndpoint<GetNextIntCount>()
             .MapEndpoint<ListAllEntityNames>()
             .MapEndpoint<GetTotalRecordsCount>()
             .MapEndpoint<AddTestRecords>()
             .MapEndpoint<GetDeploymentInfo>()
             .MapEndpoint<GetQualityStatus>() 
        ;
    }

    private static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IAuthFeature)}").WithTags("IAuthFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<SignUp>()
            .MapEndpoint<Login>()
            .MapEndpoint<RefreshToken>()
        ;
    }

    #region Endpoint Configurations
    private static readonly OpenApiSecurityScheme securityScheme = new()
    {
        Type = SecuritySchemeType.Http,
        Name = JwtBearerDefaults.AuthenticationScheme,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Reference = new()
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    private static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
                  .AllowAnonymous();
    }

    private static RouteGroupBuilder MapAuthorizedGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
            .RequireAuthorization(KPolicyDescriptor.CustomPolicy)
            .WithOpenApi(x => new(x)
            {
                Security = [new() { [securityScheme] = [] }],
            });
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IFeature
    {
        TEndpoint.Map(app);
        return app;
    }
    #endregion Endpoint Configurations
}