using Microsoft.AspNetCore.Diagnostics;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine("We are here");
        httpContext.Response.StatusCode = 500;
        await httpContext.Response.WriteAsJsonAsync(new
        {
            message = ErrorConstants.UnexpectedServerError,
            status = 500
        });

        return true;
    }
}