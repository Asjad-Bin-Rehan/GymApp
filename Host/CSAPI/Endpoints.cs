using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CSAPI.Common.Constant;
using CSAPI.Common.Filters;
using CSAPI.Common.Auth;
using CSAPI.Common;
using CSAPI.Feature.AuthMonolithicFeature;
using CSAPI.Feature.BookingFeature;

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
        endpoints.MapBookingEndpoints();        
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

    static void MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IBookingFeature)}").WithTags(nameof(IBookingFeature));

        endpoints.MapPublicGroup()
            .MapEndpoint<UpsertBooking>()
            .MapEndpoint<GetBooking>()
        ;
    }
}