


using Helpers;

using Microsoft.OpenApi.Models;
using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using CSAPI.Common.Constant;
using CSAPI.Common.Auth;
using CSAPI.Common;
using CSAPI.Extensions.Validators;
using CSAPI.Common.Auth.Requirements;

using Helpers.ServiceCollectionExtensions;
using AuthProvider;

using Helpers.Auth.Models;
using Helpers.Auth.JWT;
using FluentValidation;
using Logger;
using Helpers.Auth.Middlewares;

namespace CSAPI.Extensions
{
    public static class ConfigDI
    {
        public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration)
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var featureType = typeof(IFeature);
            string validatorName = "RequestValidator";

            services
                  .AddAuthProvider(configuration)
                    .AddCustomLogger(configuration)
                    .AddSwagger(KConstant.ApiName)
                    //TODO: AddServicesLayers
                    //TODO: AddFluentValidation.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly)
                    .AddMiddlewares()
                    .AddAuthDI(configuration)

                    .AddBusinessLayer(configuration)
                    .AddHelpers(configuration)
                    .AddEndpointGRPC(configuration, KConstant.ApiName, Assembly.GetExecutingAssembly(), typeof(IFeature))
                    .AddCors(options =>
                    {
                        options.AddDefaultPolicy(policy =>
                        {
                            policy.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  ;
                        });
                    })
                    .AddValidatorUsingAssemblies(assemblies, featureType, validatorName, typeof(IValidator<>))
                    .AddMetrics();
                   

            


            return services;
        }

        private static IServiceCollection AddMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();

            return services;
        }

        public static IServiceCollection AddAuthDI(this IServiceCollection services, IConfiguration configuration)
        {
            // Load JWT settings from configuration with environment variable fallback
            var Key = Environment.GetEnvironmentVariable("JWT_KEY") ?? configuration["Jwt:Secret"] ?? configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured");
            var Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured");
            var Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured");
            var AccessTokenExpirationInMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_ACCESS_TOKEN_EXPIRATION_IN_MINUTES") ?? configuration["Jwt:AccessTokenExpirationInMinutes"] ?? "60");
            var RefreshTokenExpirationInDays = int.Parse(Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN_EXPIRATION_IN_DAYS") ?? configuration["Jwt:RefreshTokenExpirationInDays"] ?? "7");

            // Configure JWT options
            services.Configure<JwtOptions>(options =>
            {
                options.Key = Key;
                options.Issuer = Issuer;
                options.Audience = Audience;
                options.AccessTokenExpirationInMinutes = AccessTokenExpirationInMinutes;
                options.RefreshTokenExpirationInDays = RefreshTokenExpirationInDays;
            });

            // Add JWT authentication and authorization
            services
                .AddJwtValidator(configuration, Key, Issuer, Audience)
                .AddCustomAuthorization();

            services.AddTransient<Jwt>();

            return services;
        }

        private static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {

            services.AddHttpContextAccessor();

            services.AddAuthorization(options =>
            {
                // options.AddPolicy(KPolicyDescriptor.SuperAdminPolicy, policy=>policy.RequireAuthenticatedUser());
                options.AddPolicy(KPolicyDescriptor.CustomPolicy, policy => policy.RequireAuthenticatedUser()
                .AddRequirements(new CustomAuthorizationRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
            services.AddSingleton<Func<UserPayload, AccessAndRefreshTokens>>(sp =>
            {
                var jwt = sp.GetRequiredService<Jwt>();
                return (user) => jwt.GenerateToken(user);
            });
            /* services.AddAuthorization(options =>
             {
                 options.AddPolicy(KPolicyDescriptor.SuperAdminPolicy, policy =>
                 {
                     policy.RequireAssertion(context =>
                     {
                         // Ensure the Resource is an HttpContext
                         if (context.Resource is HttpContext httpContext)
                         {
                             var roleManager = httpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
                             var userRoles = context.User.FindAll(ClaimTypes.Role).Select(r => r.Value);

                             foreach (var role in userRoles)
                             {
                                 var roleEntity = roleManager.FindByNameAsync(role).Result;
                                 if (roleEntity != null)
                                 {
                                     *//*var rolePolicies = roleEntity.Policies; // Assuming roleEntity has a Policies property

                                     if (rolePolicies.Any(p => p.Name == "RequiredPolicy"))
                                     {
                                         return true;
                                     }*//*
                                     return true;
                                 }
                             }
                         }

                         return false;
                     });
                 });
             });*/
            return services;
        }

        private static IServiceCollection AddSwagger(this IServiceCollection services, string pTitle)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = pTitle,
                    Version = "v1",
                });
                options.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));
                options.InferSecuritySchemes();
            });
            return services;

        }
  
    }
}