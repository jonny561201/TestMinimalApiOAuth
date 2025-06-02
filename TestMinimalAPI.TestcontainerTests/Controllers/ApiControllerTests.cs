using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestMinimalAPI.Data.Config;
using TestMinimalAPI.Data.Models;
using TestMinimalAPI.TestcontainerTests.Config;
using Xunit;

namespace TestMinimalAPI.TestcontainerTests.Controllers;

[Collection(DatabaseFixture.CollectionName)]
public class ApiControllerTests : IClassFixture<AuthWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly PersonDbContext _dbContext;

    public ApiControllerTests(AuthWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _dbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<PersonDbContext>();
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