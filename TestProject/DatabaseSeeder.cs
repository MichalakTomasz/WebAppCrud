using DataAccess.Sqlite;
using DataAccess.SqlServer;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TestProject
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, string projectPath)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = System.IO.Path.Combine(projectDir, projectPath, "appsettings.json");
            var configuration = new ConfigurationBuilder()
            .AddJsonFile(configPath)
            .Build();

            IdentityDbContext<IdentityUser> db = configuration[CommonConsts.CurrentDb] == CommonConsts.SqlServerDb ?
                    serviceProvider.GetRequiredService<SqlServerDbContext>() :
                    serviceProvider.GetRequiredService<SqliteDbContext>();

            await db.Database.EnsureDeletedAsync();
            await db.Database.MigrateAsync();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            List<string> roles = new() { CommonConsts.Admin, CommonConsts.Guest };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
