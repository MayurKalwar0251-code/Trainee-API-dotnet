using Microsoft.EntityFrameworkCore;
using TrainineeAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ITraineeService,TraineeService>();

// 1. Retrieve the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Automatically detect or define the MySQL Server version
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<TraineeContext>(opt => opt.UseMySql(connectionString,serverVersion));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
