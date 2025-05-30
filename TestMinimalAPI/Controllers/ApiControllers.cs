using System.Net;
using TestMinimalAPI.Config;
using TestMinimalAPI.Services;

namespace TestMinimalAPI.Controllers;

public static class WebApplicationExtensions
{
    public static WebApplication RegisterEndpoints(this WebApplication application)
    {
        application.MapGet("/test", () => Results.Created())
            .WithName("Get Test")
            .WithDescription("This is kinda silly")
            .WithOpenApi()
            .Produces((int)HttpStatusCode.Created);

        application.MapGet("/test/{id:int}", (int id) => Results.Ok(id))
            .WithName("Get Test by Id")
            .WithDescription("Getting a test by the test id")
            .WithOpenApi()
            .Produces((int)HttpStatusCode.OK);

        application.MapGet("/test/auth", () => Results.Ok())
            .WithName("Test OAuth")
            .WithDescription("Test that the OAuth integration works with Auth0")
            .WithOpenApi()
            .RequireAuthorization(AuthPolicies.TestUser)
            .Produces((int)HttpStatusCode.OK);

        application.MapGet("/test/di", (ITestService service) =>
            {
                var message = service.GetResponseText();
                return Results.Ok(message);
            })
            .WithName("Get with DI")
            .WithDescription("Get But with Dependency Injection")
            .WithOpenApi()
            .Produces((int)HttpStatusCode.OK);

        application.MapGet("/test/exception", new Func<object>(() => throw new BadHttpRequestException("duh")))
            .WithName("Get Global Exception")
            .WithDescription("Get Method to test if the global error handler is working as expected")
            .WithOpenApi()
            .Produces((int)HttpStatusCode.OK);

        application.MapGet("/test/person/{id:guid}", (ITestService service, Guid id) => service.GetPerson(id))
            .WithName("Get Db Record")
            .WithDescription("Get record from Db for integration Testing")
            .WithOpenApi()
            .Produces((int)HttpStatusCode.OK);
        
        return application;
    }
}