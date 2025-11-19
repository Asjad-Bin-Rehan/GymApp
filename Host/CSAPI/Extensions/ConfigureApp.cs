using DA.AppDbContexts;
using Helpers.Auth.Middlewares;
using MapConfig;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using System;

namespace CSAPI.Extensions
{
    public static class ConfigureApp
    {
        public static async Task Configure(this WebApplication app)
        {
            //app.UseSerilogRequestLogging();
            app.UseHttpMetrics();
            
            app.UseMetricServer();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            
            // CORS must be before Authentication/Authorization
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              );
            
            // Authentication must come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();
            
            // Custom middleware after authentication
            app.UseMiddleware<UserContextMiddleware>();
            
            app.MapEndpointsExposed();
            app.MapEndpoints();
            
            //await app.EnsureDatabaseCreated();
            //TODO: Add migration await app.EnsureDatabaseCreated();
        }
        public static async Task GlobalExceptionHandler(this WebApplication app)
        {
        }
        private static async Task EnsureDatabaseCreated(this WebApplication app)
        {
            // using var scope = app.Services.CreateScope();
            // var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // await db.Database.MigrateAsync();




            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            

            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                Console.WriteLine("Applying pending migrations...");
                await dbContext.Database.MigrateAsync(); // Apply pending migrations
            }
            else
            {
                Console.WriteLine("No pending migrations found.");
            }
           // dbContext.SeedData();
        }
    }
}