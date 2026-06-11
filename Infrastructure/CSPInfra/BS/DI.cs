using BS.Services.AuthService;
using BS.Services.BookingService;
using BS.Services.CourtService;
using BS.Services.CustomerProfileService;
using BS.Services.LocationService;
using BS.Services.LocationHasLocationService;
using BS.Services.OrganizationService;
using BS.Services.PricingService;
using BS.Services.SlotService;
using BS.Services.TenantService;
using BS.Services.UserService;
using BS.Services.VenueService;
using DA;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddDALayer(configuration)
        .AddServices();

        Console.WriteLine($"[Info]----->{nameof(AddBusinessLayer)} service added");
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.TryAddScoped<IAuthService, AuthService>();
        services.TryAddScoped<ILocationService, LocationService>();
        services.TryAddScoped<ILocationFromLocationService, LocationFromLocationService>();
        services.TryAddScoped<IBookingService, BookingService>();
        services.TryAddScoped<ICourtService, CourtService>();
        services.TryAddScoped<ICustomerProfileService, CustomerProfileService>();
        services.TryAddScoped<IOrganizationService, OrganizationService>();
        services.TryAddScoped<IPricingService, PricingService>();
        services.TryAddScoped<ISlotService, SlotService>();
        services.TryAddScoped<ITenantService, TenantService>();
        services.TryAddScoped<IUserService, UserService>();
        services.TryAddScoped<IVenueService, VenueService>();

        Console.WriteLine($"[Info]----->{nameof(AddServices)} service added");
        return services;
    }
}