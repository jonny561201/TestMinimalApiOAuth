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
            string message;
            
            switch (error)
            {
                case BadHttpRequestException e:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = error.Message;
                    break;
                case HttpRequestException e:
                    context.Response.StatusCode = (int)(e.StatusCode ?? HttpStatusCode.BadRequest);
                    message = e.Message;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = $"Internal Error: {error.Message}";
                    break;
            }

            await context.Response.WriteAsync(message);
        }
    }
}