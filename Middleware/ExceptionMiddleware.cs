using System.Net;
using System.Text.Json;

namespace ReservationApi.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // In real apps, log with ILogger/Serilog and hide internal details.
            Console.WriteLine(ex);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var payload = new { message = "Unexpected server error." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
