using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataAccess.SqlServer;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAppCrud.Behaviors;
using WebAppCrud.Mediator;
using WebAppCrud.GraphQl.Mutations;
using WebAppCrud.GraphQl.Queries;
using WebAppCrud.Validators;
using DataAccess.Sqlite;
using Microsoft.AspNetCore.HttpOverrides;
using WebAppCrud.Extensions;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using FluentAssertions.Common;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationLifetimeEvents();
builder.Services.AddLivetimeHostedService();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
	.ConfigureContainer<ContainerBuilder>(c =>
	{
		c.RegisterType<PersonValidator>().As<IValidator<Person>>();
		c.RegisterType<AddresValidator>().As<IValidator<Address>>();
		c.RegisterType<InputProductValidator>().As<IValidator<InputProduct>>();
        c.RegisterGeneric(typeof(GetGenericRequestHandlerAsync<>)).As(typeof(IRequestHandler<,>));
        c.RegisterGeneric(typeof(GetGenericQueryableRequestHandlerAsync<>)).As(typeof(IRequestHandler<,>));
		c.RegisterGeneric(typeof(GetGenericByIdAsyncQueryHandler<>)).As(typeof(IRequestHandler<,>));
		c.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
		
		if (configuration[CommonConsts.CurrentDb] == CommonConsts.SqlServerDb)
		{
            c.RegisterGeneric(typeof(DataAccess.SqlServer.Repositories.GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            c.RegisterType<DataAccess.SqlServer.Repositories.PersonRepository>().As<IPersonRepository>();
            c.RegisterType<DataAccess.SqlServer.Repositories.ProductRepository>().As<IProductRepository>();
            c.RegisterType<DataAccess.SqlServer.Repositories.AddressRepositiory>().As<IAddressRepository>();
            c.RegisterType<DataAccess.SqlServer.Repositories.UnitOfWork>().As<IUnitOfWork>();
        }
		if (configuration.GetSection(CommonConsts.CurrentDb).Get<string>() == CommonConsts.SqliteDb)
		{
            c.RegisterGeneric(typeof(DataAccess.Sqlite.Repositories.GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            c.RegisterType<DataAccess.Sqlite.Repositories.PersonRepository>().As<IPersonRepository>();
            c.RegisterType<DataAccess.Sqlite.Repositories.ProductRepository>().As<IProductRepository>();
            c.RegisterType<DataAccess.Sqlite.Repositories.AddressRepositiory>().As<IAddressRepository>();
            c.RegisterType<DataAccess.Sqlite.Repositories.UnitOfWork>().As<IUnitOfWork>();
        }
	});
if (configuration[CommonConsts.CurrentDb] == CommonConsts.SqlServerDb)
{
    builder.Services.AddDbContext<SqlServerDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString(CommonConsts.SqlServerConnectionString)));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<SqlServerDbContext>();
}
if (configuration[CommonConsts.CurrentDb] == CommonConsts.SqliteDb)
{
    builder.Services.AddDbContext<SqliteDbContext>(o => 
	o.UseSqlite(builder.Configuration.GetConnectionString(CommonConsts.SqliteConnectionString)));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<SqliteDbContext>();
}

builder.Services.AddAuthentication(o =>
{
	o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
	o.SaveToken = true;
	o.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = configuration[CommonConsts.Issuer],
		ValidAudience = configuration[CommonConsts.Audience],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection(CommonConsts.JwtKey).Get<string>()))
	};
});
	
builder.Services.AddAuthorization(o => 
{
	o.AddPolicy(CommonConsts.GuestPolicy, p =>
	{
		p.RequireRole(CommonConsts.Guest);
		p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
	});
	o.AddPolicy(CommonConsts.AdminPolicy, p =>
	{
		p.RequireRole(CommonConsts.Admin);
		p.RequireAuthenticatedUser();
		p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
	});
});


builder.Services.AddGraphQLServer()
	.AddQueryType<Query>()
	.AddMutationType<Mutation>()
	.AddAuthorization()
	.AddFiltering()
	.AddSorting()
	.AddQueryableCursorPagingProvider();

builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Logging.AddDebug();
builder.Logging.AddConsole();
//builder.Logging.AddApplicationInsights(configureTelemetryConfiguration: c =>
//c.ConnectionString = configuration.GetConnectionString(connectionStringKey), configureApplicationInsightsLoggerOptions: o => { });
//builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("Mine-cathegory", LogLevel.Trace);

builder.Services.Configure<ForwardedHeadersOptions>(o => o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

builder.Services.AddCors(o =>
o.AddPolicy(CommonConsts.CorsPolicy, builder => builder
.WithOrigins("http://localhost:4200", "http://localhost:3000")
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

app.UseCors(CommonConsts.CorsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.MapControllers();
await app.SeedEdentityRolesAsync();

app.UseForwardedHeaders();
app.LogAppStartup();
app.UseServerEvents();

//app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "dist")), RequestPath = "" });
//app.UseSpa(spa =>
//{
//	spa.Options.SourcePath = "ClientApp";

//	if (app.Environment.IsDevelopment())
//	{
//		spa.Options.DevServerPort = 4200;
//		spa.UseAngularCliServer(npmScript: "start");
//	}
//});

app.Run();

public partial class Program { }