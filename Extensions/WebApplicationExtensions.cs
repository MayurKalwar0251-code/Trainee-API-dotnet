using Microsoft.EntityFrameworkCore;
using TrainineeAPI.Models;

namespace TraineeApi.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<TraineeContext>();

        context.Database.Migrate();

        if( !context.Users.Any())
        {
            context.Users.Add( new User
            {
                Username = "admin",
                Email = "admin@trainee.com",
                PasswordHash = PasswordUtility.HashUserPassword("Admin@123456"),
                Role = "Admin"
            });

            await context.SaveChangesAsync();
        }
    }
}