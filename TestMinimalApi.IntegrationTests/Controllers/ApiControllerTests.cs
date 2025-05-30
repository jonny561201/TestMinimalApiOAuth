using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestMinimalAPI.Data.Config;
using TestMinimalAPI.Data.Models;
using TestMinimalApi.IntegrationTests.Config;

namespace TestMinimalApi.IntegrationTests.Controllers;

public class ApiControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly PersonDbContext _dbContext;

    public ApiControllerTests()
    {
        _factory = new AuthWebApplicationFactory();
        _client = _factory.CreateClient();
        _dbContext = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<PersonDbContext>();
        _dbContext.People.ExecuteDelete();
        _dbContext.SaveChanges();
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

    [Fact]
    public async Task GetDbRecord_ShouldReturnRecord()
    {
        var id = Guid.NewGuid();
        var person = new Person
        {
            Id = id,
            FirstName = "Jon",
            LastName = "Tester",
            Email = "JonTester123@gmail.com"
        };
        await _dbContext.People.AddAsync(person);
        await _dbContext.SaveChangesAsync();
        var result = await _client.GetFromJsonAsync<Person>($"/test/person/{id}");
        
        Assert.Equal(person, result);
    }
}