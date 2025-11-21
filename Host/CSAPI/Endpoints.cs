using CSAPI.Common;
using CSAPI.Common.Auth;
using CSAPI.Common.Constant;
using CSAPI.Common.Filters;
using CSAPI.Feature.AccessLogFeature;
using CSAPI.Feature.AdminFeature;
using CSAPI.Feature.LocationFeature;
using CSAPI.Feature.MemberShipPlanFeature;
using CSAPI.Feature.PartnerGym;
using CSAPI.Feature.PointsHistoryFeature;
using CSAPI.Feature.RedemptionFeature;
using CSAPI.Feature.RewardCatalogFeature;
using CSAPI.Feature.SubscriptionFeature;
using CSAPI.Feature.UserFeature;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace CSAPI;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup(KConstant.ApiName).AddEndpointFilter<RequestLoggingFilter>().WithOpenApi();


        endpoints.MapUserEndpoints();
        endpoints.MapPartnerGymEndpoints();
        endpoints.MapLocationEndpoints();
        endpoints.MapAccessLogEndpoints();
        endpoints.MapAdminFeatureEndpoints();
        endpoints.MapMembershipPlanEndpoints();
        endpoints.MapRewardCatalogEndpoints();
        endpoints.MapRedemptionEndpoints();
        endpoints.MapSubscriptionEndpoints();
        endpoints.MapPointsHistoryEndpoints();


        //// Auto-Increment Code & Paginate
        //endpoints.MapAuthEndpoints();
        //endpoints.MapNextCountEndpoints();

        //// Master (Data) Stepper
        //endpoints.MapUnitOfMeasureEndpoints();
        //endpoints.MapQualitativeResultEndpoints();
        //endpoints.MapInspectionAttributeEndpoints();
        //endpoints.MapInspectionCharacteristicsEndpoints();
        //endpoints.MapItemEndpoints();
        //endpoints.MapInspectionCardEndpoints();
        //endpoints.MapItemSampleEndpoints();
        //endpoints.MapItemInspectionCardEndpoints();

        //// Evaluation (Plan) Stepper
        //endpoints.MapPurchaseQCEndpoints();
        //endpoints.MapProductionQCEndpoints();
        //endpoints.MapProductionQAEndpoints();
        //endpoints.MapPurchaseQCSampleEndpoints();
        //endpoints.MapProductionQCSampleEndpoints();
        //endpoints.MapProductionQACavityEndpoints();
        //endpoints.MapProductionQACavitySampleEndpoints();
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

    private static void MapPartnerGymEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IPartnerGymFeature)}").WithTags("IPartnerGymFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<AddPartnerGym>()
            .MapEndpoint<AddPartnerGymManual>()
            .MapEndpoint<GetPartnerGymById>()
            .MapEndpoint<GetPartnerGymsByAdminId>()
            .MapEndpoint<ListPartnerGyms>()
            .MapEndpoint<ListActivePartnerGyms>()
            .MapEndpoint<UpdatePartnerGym>()
            .MapEndpoint<DeletePartnerGym>();
    }

    private static void MapLocationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ILocationFeature)}").WithTags("ILocationFeature");

        endpoints.MapEndpoint<AddLocation>();
        endpoints.MapEndpoint<GetLocationById>();
        endpoints.MapEndpoint<ListAllLocations>();
        endpoints.MapEndpoint<UpdateLocation>();
        endpoints.MapEndpoint<DeleteLocation>();
    }

    private static void MapAccessLogEndpoints(this IEndpointRouteBuilder app)
{
    var endpoints = app.MapGroup($"/{nameof(IAccessLogFeature)}").WithTags("IAccessLogFeature");

    endpoints.MapPublicGroup()
        .MapEndpoint<ListAllAccessLogs>()
        .MapEndpoint<AddAccessLog>()
        .MapEndpoint<GetAccessLogsByUserId>()
        .MapEndpoint<GetAccessLogById>()
        .MapEndpoint<DeleteAccessLog>()
        .MapEndpoint<GetAccessLogsByGymId>();
    }

    private static void MapMembershipPlanEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IMembershipPlanFeature)}").WithTags("IMembershipPlanFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllMembershipPlans>()
            .MapEndpoint<GetMembershipPlanById>()
            .MapEndpoint<AddMembershipPlan>()
            .MapEndpoint<UpdateMembershipPlan>()
            .MapEndpoint<DeleteMembershipPlan>()
            ;
    }



    public static void MapAdminFeatureEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IAdminFeature)}").WithTags("IAdminFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<SignUpAdmin>()               // POST / Sign up new admin
            .MapEndpoint<GetAdminById>()              // GET / Get admin by ID
            .MapEndpoint<ListAllAdmins>()             // GET / List all admins





        // .MapEndpoint<LoginAdminWithoutJWT>();    // POST / Login admin without JWT
        ;
    LoginAdminWithoutJWT.Map(endpoints);            // Fix: Call static Map directly since LoginAdminWithoutJWT does not implement IFeature
}


    private static void MapRewardCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IRewardCatalogFeature)}").WithTags("IRewardCatalogFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<AddRewardFeature>()           // POST / Add new reward
            .MapEndpoint<ListAllRewardsFeature>()      // GET / List all rewards
            .MapEndpoint<GetRewardByIdFeature>()       // GET / Get reward by ID
            .MapEndpoint<UpdateRewardFeature>()        // PUT / Update reward
            .MapEndpoint<DeleteRewardFeature>();       // DELETE / Delete reward
    }



    private static void MapRedemptionEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IRedemptionFeature)}").WithTags("IRedemptionFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<AddRedemption>()
            .MapEndpoint<ListAllRedemptions>()
            .MapEndpoint<GetRedemptionById>()
            .MapEndpoint<GetRedemptionsByUserId>()
            .MapEndpoint<UpdateRedemption>()
            .MapEndpoint<DeleteRedemption>();
    }

    private static void MapSubscriptionEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ISubscriptionFeature)}")
                           .WithTags("ISubscriptionFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<AddSubscription>()
            .MapEndpoint<ListAllSubscriptions>()
            .MapEndpoint<GetSubscriptionById>()
            .MapEndpoint<UpdateSubscription>()
            .MapEndpoint<DeleteSubscription>();
    }

    private static void MapPointsHistoryEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IPointsHistoryFeature)}")
                           .WithTags("IPointsHistoryFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<GetPointsHistoryByUserId>()
            .MapEndpoint<ListAllPointsHistory>(); // optional, for admin dashboards
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