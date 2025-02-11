﻿using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Sqlite
{
    public class SqliteDbContext : IdentityDbContext<IdentityUser>
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
    }
}
