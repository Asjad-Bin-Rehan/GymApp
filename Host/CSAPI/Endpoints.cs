using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CSAPI.Common.Constant;
using CSAPI.Common.Filters;
using CSAPI.Common.Auth;
using CSAPI.Common;
using CSAPI.Feature.AuthMonolithicFeature;
using CSAPI.Feature.BookingFeature;
using CSAPI.Feature.LocationFeature;
using CSAPI.Feature.LocationFromLocationFeature;
using CSAPI.Feature.CourtFeature;
using CSAPI.Feature.CustomerProfileFeature;
using CSAPI.Feature.OrganizationFeature;
using CSAPI.Feature.PricingFeature;
using CSAPI.Feature.SlotFeature;
using CSAPI.Feature.TenantFeature;
using CSAPI.Feature.UserFeature;
using CSAPI.Feature.VenueFeature;

namespace CSAPI;

public static class Endpoints
{

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

    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup(KConstant.ApiName).AddEndpointFilter<RequestLoggingFilter>().WithOpenApi();

        endpoints.MapAuthEndpoints();
        endpoints.MapLocationEndpoints();
        endpoints.MapLocationFromLocationEndpoints();
        endpoints.MapBookingEndpoints();
        endpoints.MapCourtEndpoints();
        endpoints.MapCustomerProfileEndpoints();
        endpoints.MapOrganizationEndpoints();
        endpoints.MapPricingEndpoints();
        endpoints.MapSlotEndpoints();
        endpoints.MapTenantEndpoints();
        endpoints.MapUserEndpoints();
        endpoints.MapVenueEndpoints();
    }

    static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IAuthFeature)}").WithTags("IAuthFeature");

        endpoints.MapPublicGroup()
            .MapEndpoint<SignUp>()
            .MapEndpoint<Login>()
            .MapEndpoint<RefreshToken>()
        ;
    }

    static void MapLocationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ILocationFeature)}").WithTags(nameof(ILocationFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertLocation>()
            .MapEndpoint<GetLocation>()
        ;
    }

    static void MapLocationFromLocationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ILocationFromLocationFeature)}").WithTags(nameof(ILocationFromLocationFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertLocationFromLocation>()
            .MapEndpoint<GetLocationFromLocation>()
        ;
    }

    static void MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IBookingFeature)}").WithTags(nameof(IBookingFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertBooking>()
            .MapEndpoint<GetBooking>()
        ;
    }

    static void MapCourtEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ICourtFeature)}").WithTags(nameof(ICourtFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertCourt>()
            .MapEndpoint<GetCourt>()
        ;
    }

    static void MapCustomerProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ICustomerProfileFeature)}").WithTags(nameof(ICustomerProfileFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertCustomerProfile>()
            .MapEndpoint<GetCustomerProfile>()
        ;
    }

    static void MapOrganizationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IOrganizationFeature)}").WithTags(nameof(IOrganizationFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertOrganization>()
            .MapEndpoint<GetOrganization>()
        ;
    }

    static void MapPricingEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IPricingFeature)}").WithTags(nameof(IPricingFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertPricing>()
            .MapEndpoint<GetPricing>()
        ;
    }

    static void MapSlotEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ISlotFeature)}").WithTags(nameof(ISlotFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertSlot>()
            .MapEndpoint<GetSlot>()
        ;
    }

    static void MapTenantEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ITenantFeature)}").WithTags(nameof(ITenantFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertTenant>()
            .MapEndpoint<GetTenant>()
        ;
    }

    static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IUserFeature)}").WithTags(nameof(IUserFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertUser>()
            .MapEndpoint<GetUser>()
        ;
    }

    static void MapVenueEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IVenueFeature)}").WithTags(nameof(IVenueFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertVenue>()
            .MapEndpoint<GetVenue>()
        ;
    }
}