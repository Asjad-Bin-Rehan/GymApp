using DA.AppDbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public static class DI
    {
        public static IServiceCollection AddDALayer(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext(configuration)
                .AddUOW();

            Console.WriteLine($"[Info]----->{nameof(AddDALayer)} service added");
            return services;
        }

        #region DbContext
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetSection("ConnectionStrings:db").Value, x => x.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
            return services;
        }
        #endregion DbContext

        #region UoW
        public static IServiceCollection AddUOW(this IServiceCollection services)
        {
            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        #endregion UoW

    }
}
