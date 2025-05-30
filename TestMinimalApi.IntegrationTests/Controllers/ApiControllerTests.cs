using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using TestMinimalApi.IntegrationTests.Config;

namespace TestMinimalApi.IntegrationTests.Controllers;

public class ApiControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ApiControllerTests()
    {
        _factory = new AuthWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetTest_ShouldReturnCreatedStatus()
    {
        var result = await _client.GetAsync("/test");
        
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
    }

    [Fact]
    public async Task GetTestAuth_ShouldReturnSuccessStatus()
    {
        var result = await _client.GetAsync("/test/auth");
        
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}