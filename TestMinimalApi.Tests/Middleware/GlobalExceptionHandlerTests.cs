using System.Net;
using Microsoft.AspNetCore.Http;
using TestMinimalAPI.Middleware;

namespace TestMinimalApi.Tests.Middleware;

public class GlobalExceptionHandlerTests
{
    private DefaultHttpContext _context;

    public GlobalExceptionHandlerTests()
    {
        _context = new DefaultHttpContext();
        _context.Response.Body = new MemoryStream();
    }

    [Fact]
    public async Task Invoke_ShouldNotThrowWhenSuccessfulResponse()
    {
        RequestDelegate request = (HttpContext _) => Task.FromResult<object>(null);
        var handler = new GlobalExceptionHandler(request);

        await handler.Invoke(_context);
        
        Assert.Equal((int)HttpStatusCode.OK, _context.Response.StatusCode);
    }

    [Fact]
    public async Task Invoke_ShouldReturn400WhenBadRequestRaised()
    {
        var message = "Fail Message";
        RequestDelegate request = (HttpContext _) => throw new BadHttpRequestException(message);
        var handler = new GlobalExceptionHandler(request);

        await handler.Invoke(_context);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await new StreamReader(_context.Response.Body).ReadToEndAsync();
        Assert.Equal((int)HttpStatusCode.BadRequest, _context.Response.StatusCode);
        Assert.Equal(message, response);
    }

    [Fact]
    public async Task Invoke_ShouldReturnStatusCodeFromHttpRequestException()
    {
        var message = "Exception Message";
        RequestDelegate request = (HttpContext _) => throw new HttpRequestException(message, null, HttpStatusCode.Conflict);
        var handler = new GlobalExceptionHandler(request);

        await handler.Invoke(_context);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await new StreamReader(_context.Response.Body).ReadToEndAsync();
        Assert.Equal((int)HttpStatusCode.Conflict, _context.Response.StatusCode);
        Assert.Equal(message, response);
    }
}