using System.Diagnostics;

namespace ReservationApi.Middleware;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();
            Console.WriteLine($"[{context.Response.StatusCode}] {context.Request.Method} {context.Request.Path} in {sw.ElapsedMilliseconds}ms");
        }
    }
}