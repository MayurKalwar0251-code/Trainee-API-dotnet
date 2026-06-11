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
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ITraineeService,TraineeService>();
builder.Services.AddScoped<IJWTService,JWTService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IMentorService,MentorService>();
builder.Services.AddScoped<ILearningTaskService,LearningTaskService>();

// 1. Retrieve the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Automatically detect or define the MySQL Server version
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<TraineeContext>(opt => opt.UseMySql(connectionString,serverVersion));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

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
