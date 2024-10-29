using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
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
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;

namespace TestProject.IngegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private string currentDb;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var projectPath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName, "WebAppCrud");

            builder.UseContentRoot(projectPath);
            builder.ConfigureServices(async services =>
            {
                var projectDir = Directory.GetCurrentDirectory();
                var configPath = Path.Combine(projectDir, projectPath, "appsettings.json");
                var configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

                currentDb = configuration[CommonConsts.CurrentDb];

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
                    db.Database.EnsureCreated();
                }
                else
                {
                    var db = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
                    db.Database.EnsureCreated();
                }

                try
                {
                    await DatabaseSeeder.SeedAsync(scope.ServiceProvider);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        protected override void ConfigureClient(HttpClient client)
        {
            using var scope = Services.CreateScope();

            IdentityDbContext<IdentityUser> db;
            if (currentDb == CommonConsts.SqlServerDb)
            {
                db = scope.ServiceProvider.GetRequiredService<SqlServerDbContext>();
            }
            else
            {
                db = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
            }

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            DatabaseSeeder.SeedAsync(Services);
        }
    }

}