using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.Models;
using Scalar.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

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

// call extension ServiceExtension for injection service classes
builder.Services.AddServices();

builder.Services.AddStackExchangeRedisCache( options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "TraineeManagementApi";
}); 

// 1. Retrieve the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Automatically detect or define the MySQL Server version
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<TraineeContext>(opt => opt.UseMySql(connectionString,serverVersion));

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

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
