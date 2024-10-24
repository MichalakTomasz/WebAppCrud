using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using DataAccess.SqlServer;
using DataAccess.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TestProject
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var projectPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.FullName; //@"D:\\Repos\\WebAppCrud";
                        
            builder.UseSolutionRelativeContentRoot(projectPath);
            builder.ConfigureServices(async services =>
            {
                var projectDir = Directory.GetCurrentDirectory();
                var configPath = System.IO.Path.Combine(projectDir, projectPath, "appsettings.json");
                var configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

                if (configuration[CommonConsts.CurrentDb] == CommonConsts.SqlServerDb)
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SqlServerDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }
                    services.AddDbContext<SqlServerDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                }
                else
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SqlServerDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }
                    services.AddDbContext<SqliteDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                }
               
                var serviceProvider = services.BuildServiceProvider();
                
                using var scope = serviceProvider.CreateScope();

                if (configuration[CommonConsts.CurrentDb] == CommonConsts.SqlServerDb)
                {
                    var db = scope.ServiceProvider.GetRequiredService<SqlServerDbContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
                else
                {
                    var db = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
                    
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