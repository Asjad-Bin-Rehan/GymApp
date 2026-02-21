using BS.Services.AuthService;
using BS.Services.InspectionAttributeService;
using BS.Services.InspectionCardService;
using BS.Services.InspectionCharactersticService;
using BS.Services.ItemCardService;
using BS.Services.ItemInspectionCardService;
using BS.Services.ItemSampleService;
using BS.Services.LocationService;
using BS.Services.NextIntCodeService;
using BS.Services.ProductionQACavitySampleService;
using BS.Services.ProductionQACavityService;
using BS.Services.ProductionQAService;
using BS.Services.ProductionQCSampleService;
using BS.Services.ProductionQCService;
using BS.Services.PurchaseQCSampleService;
using BS.Services.PurchaseQCService;
using BS.Services.QualitativeResultService;
using BS.Services.UnitOfMeasure;
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
        services.TryAddScoped<ILocationService, LocationService>();

        services.TryAddScoped<IAuthService, AuthService>();
        services.TryAddScoped<IProductionQACavitySampleService, ProductionQACavitySampleService>();
        services.TryAddScoped<IProductionQACavityService, ProductionQACavityService>();
        services.TryAddScoped<IProductionQCSampleService, ProductionQCSampleService>();
        services.TryAddScoped<IPurchaseQCSampleService, PurchaseQCSampleService>();
        services.TryAddScoped<IProductionQAService, ProductionQAService>();
        services.TryAddScoped<IProductionQCService, ProductionQCService>();
        services.TryAddScoped<IPurchaseQCService, PurchaseQCService>();
        services.TryAddScoped<IItemInspectionCardService, ItemInspectionCardService>();
        services.TryAddScoped<IItemSampleService,  ItemSampleService>();
        services.TryAddScoped<IInspectionCardService, InspectionCardService>();
        services.TryAddScoped<IItemCardService, ItemCardService>();
        services.TryAddScoped<IInspectionCharacteristicService, InspectionCharacteristicService>();
        services.TryAddScoped<IUnitOfMeasureService, UnitOfMeasureService>();
        services.TryAddScoped<IQualitativeResultService, QualitativeResultService>();
        services.TryAddScoped<INextIntCodeService, NextIntCodeService>();
        services.TryAddScoped<IInspectionAttributeService, InspectionAttributeService>();

        Console.WriteLine($"[Info]----->{nameof(AddServices)} service added");
        return services;
    }
}

