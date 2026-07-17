using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.Models;
using Scalar.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TraineeApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "http://localhost:5173")
                .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddAutoMapper(cfg => 
{
    // Scans the assembly containing "Program" for Profile classes
    cfg.AddMaps(typeof(Program)); 
});

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

// 2. Register your custom exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// 3. Register the standard Problem Details service
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var rabbitMQSection = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddSingleton(sp => new ConnectionFactory()
{
    HostName = rabbitMQSection["HostName"] ?? "localhost",
    UserName = rabbitMQSection["UserName"] ?? "guest",
    Password = rabbitMQSection["Password"] ?? "guest",
    VirtualHost = rabbitMQSection["VirtualHost"] ?? "/",
});

// call extension ServiceExtension for injection service classes
builder.Services.AddServices();

builder.Services.AddRedisService(builder.Configuration); 

// 1. Retrieve the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Automatically detect or define the MySQL Server version
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<TraineeContext>(opt => opt.UseMySql(connectionString,serverVersion));

builder.Services.AddHealthChecks().AddMySql(
    builder.Configuration.GetConnectionString("DefaultConnection")!,
    name: "MySQL",
    tags: new[] {"ready"}
).AddRedis(
    builder.Configuration.GetConnectionString("RedisConnection")!,
    name: "Redis",
    tags: new[] {"ready"}
).AddRabbitMQ(
    async sp => await sp.GetRequiredService<ConnectionFactory>().CreateConnectionAsync(),
    name: "RabbitMQ",
    tags: new[] {"ready"}
);

// call extension JWTExtension for injection JWT configuration
builder.Services.AddJwtService(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Accessible at /scalar/v1
    app.MapScalarApiReference(options =>
    {
        // Explicitly force the theme to stay in Dark Mode
        options.ForceDarkMode(); 
        
        // Optional: Pick a specific dark aesthetic theme flavor 
        // options.WithTheme(ScalarTheme.Moon); 
    });
}

app.MapHealthChecks("/health/live",new HealthCheckOptions { Predicate = _ => false,ResponseWriter = HealthCheckReportExtension.WriteHealthCheckResponse});

app.MapHealthChecks("/health/ready",new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready"),ResponseWriter = HealthCheckReportExtension.WriteHealthCheckResponse });

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.SeedDatabaseAsync();

app.Run();
