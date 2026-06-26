using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class HealthCheckReportExtension
{
    public static async Task WriteHealthCheckResponse(HttpContext context,HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(entry => new
            {
                dependency = entry.Key,
                status = entry.Value.Status.ToString(),
                duration = entry.Value.Duration.TotalMilliseconds
            })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}