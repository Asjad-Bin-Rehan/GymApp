using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NATS.Client.Core;
using NATS.Client.JetStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NATSNotificationSystem
{
    public static class DI
    {
        public static IServiceCollection AddNatsService(this IServiceCollection services, IConfiguration configuration)
        { return services; }


        //public static IServiceCollection AddNatsService(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddSingleton<NatsConnectionManager>();

        //    services.AddSingleton<INatsService,NatsService>(provider =>
        //    {
        //        var connectionManager = provider.GetRequiredService<NatsConnectionManager>();
        //        return new NatsService(connectionManager);
        //    });

        //    //services.TryAddSingleton<INatsService, NatsService>();

        //    return services;
        //}
    }
}
