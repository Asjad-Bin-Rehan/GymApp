using BS.Services.AccessLogService;
using BS.Services.AdminService;
using BS.Services.LocationService;
using BS.Services.MembershipPlanService;
using BS.Services.PartnerGymService;
using BS.Services.PointsHistoryService;
using BS.Services.RedemptionService;
using BS.Services.RewardCatalogService;
using BS.Services.SubscriptionService;
using BS.Services.UserService;
using BS.Services.GymImagesService;

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
        services.TryAddScoped<IUserService, UserService>();
        services.AddScoped<IPartnerGymService, PartnerGymService>();
        services.TryAddScoped<ILocationService, LocationService>();
        services.TryAddScoped<IAccessLogService, AccessLogService>();
        services.TryAddScoped<IMembershipPlanService, MembershipPlanService>();
        services.TryAddScoped<IAdminService, AdminService>();
        services.TryAddScoped<IRewardCatalogService, RewardCatalogService>();
        services.TryAddScoped<IRedemptionService, RedemptionService>();
        services.TryAddScoped<ISubscriptionService, SubscriptionService>();
        services.TryAddScoped<IPointsHistoryService, PointsHistoryService>();
        services.TryAddScoped<IGymImagesService, GymImagesService>();

        Console.WriteLine($"[Info]----->{nameof(AddServices)} service added");
        return services;
    }
}

