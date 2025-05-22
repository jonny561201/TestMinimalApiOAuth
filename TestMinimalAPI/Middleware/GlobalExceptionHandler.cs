using System.Net;

namespace TestMinimalAPI.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var message = "";
            
            switch (error)
            {
                case BadHttpRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = error.Message;
                    break;
            }

            await context.Response.WriteAsync(message);
        }
    }
}