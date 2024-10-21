using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var projectPath = @"D:\\Repos\\WebAppCrud";
                        
            builder.UseSolutionRelativeContentRoot(projectPath);
            builder.ConfigureServices(async services =>
            {
                var serviceProvider = services.BuildServiceProvider();
                
                using var scope = serviceProvider.CreateScope();

                try
                {
                    await DatabaseSeeder.SeedAsync(scope.ServiceProvider, projectPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
    }
}