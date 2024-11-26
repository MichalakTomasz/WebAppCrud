using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WebAppCrud.Events;
using WebAppCrud.Notifications;

namespace WebAppCrud.Extensions
{
    public static class AppExtensions
    {
        public static void LogAppStartup(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var mediatior = scope.ServiceProvider.GetRequiredService<IMediator>();

            LoggerNotification notification = new()
            {
                NotificationType = Domain.Models.NotificationType.Information,
                Message = "Application is startingUp"
            };
            mediatior.Publish(notification);
        }

        public static async Task SeedEdentityRolesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in Roles.List)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public static IApplicationBuilder UseServerEvents(this IApplicationBuilder app)
            => app.UseMiddleware<ServerEventsMiddleware>();

        public static void AddApplicationLifetimeEvents(this IServiceCollection services)
            => services.AddSingleton<ApplicationLifetimeEvents>();

        public static void AddLivetimeHostedService(this IServiceCollection services)
            => services.AddHostedService<LifetimeHostedService>();
    }

}
