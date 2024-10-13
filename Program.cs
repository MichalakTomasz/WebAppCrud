using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataAccess.EfCore;
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
using WebAppCrud.GraphQl.Exceptions;
using WebAppCrud.GraphQl.Mutations;
using WebAppCrud.GraphQl.Queries;
using WebAppCrud.Validators;
using DataAccess.Sqlite;
using Microsoft.Extensions.Logging.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
	.ConfigureContainer<ContainerBuilder>(c =>
	{
		c.RegisterType<PersonValidator>().As<IValidator<Person>>();
		c.RegisterType<AddresValidator>().As<IValidator<Address>>();
		c.RegisterType<ProductDtoValidator>().As<IValidator<InputProduct>>();
		c.RegisterGeneric(typeof(GetGenericRequestHandlerAsync<>)).As(typeof(IRequestHandler<,>));
		c.RegisterGeneric(typeof(GetGenericByIdAsyncQueryHandler<>)).As(typeof(IRequestHandler<,>));
		c.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
		
		if (configuration[CommonConsts.CurrentDb] == CommonConsts.SqlServerDb)
		{
            c.RegisterGeneric(typeof(DataAccess.EfCore.GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            c.RegisterType<DataAccess.EfCore.PersonRepository>().As<IPersonRepository>();
            c.RegisterType<DataAccess.EfCore.ProductRepository>().As<IProductRepository>();
            c.RegisterType<DataAccess.EfCore.AddressRepositiory>().As<IAddressRepository>();
            c.RegisterType<DataAccess.EfCore.UnitOfWork>().As<IUnitOfWork>();
        }
		if (configuration.GetSection(CommonConsts.CurrentDb).Get<string>() == CommonConsts.SqliteDb)
		{
            c.RegisterGeneric(typeof(DataAccess.Sqlite.GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            c.RegisterType<DataAccess.Sqlite.PersonRepository>().As<IPersonRepository>();
            c.RegisterType<DataAccess.Sqlite.ProductRepository>().As<IProductRepository>();
            c.RegisterType<DataAccess.Sqlite.AddressRepositiory>().As<IAddressRepository>();
            c.RegisterType<DataAccess.Sqlite.UnitOfWork>().As<IUnitOfWork>();
        }
	});
if (configuration[CommonConsts.CurrentDb] == CommonConsts.SqlServerDb)
{
    builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString(CommonConsts.SqlServerConnectionString)));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
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
		//p.RequireAssertion(_ => false);
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
	.AddAuthorization();
	//.UseField<DomainExceptionMiddleware>();


builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Logging.AddDebug();
builder.Logging.AddConsole();
//var connectionString = GetConnectionstring();
//builder.Logging.AddApplicationInsights(configureTelemetryConfiguration: c =>
//c.ConnectionString = configuration.GetConnectionString(connectionString), configureApplicationInsightsLoggerOptions: o => { });
//builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("Mine-cathegory", LogLevel.Trace);

string GetConnectionstring()
	=> configuration[CommonConsts.CurrentDb] == CommonConsts.SqlServerDb ?
	configuration.GetConnectionString(CommonConsts.SqlServerConnectionString) :
	configuration.GetConnectionString(CommonConsts.SqliteConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
//app.MapIdentityApi<IdentityUser>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	List<string> notExistRoles = new();
	foreach (var role in Roles.List)
	{
		if (!await roleManager.RoleExistsAsync(role))
			notExistRoles.Add(role);
	}

	foreach (var role in notExistRoles)
	{
		await roleManager.CreateAsync(new IdentityRole(role));
	}
}

//app.Use(async (context, next) =>
//{
//	if (!context.User.Identity.IsAuthenticated)
//	{
//		var claims = new List<Claim>
//			{
//				new Claim(ClaimTypes.Role, "Guest")
//			};
//		var identity = new ClaimsIdentity(claims, "Guest");
//		context.User.AddIdentity(identity);
//	}
//	await next.Invoke();
//});

app.Run();
