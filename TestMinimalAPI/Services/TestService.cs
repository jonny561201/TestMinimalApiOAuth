using System.Net;
using TestMinimalAPI.Data;
using TestMinimalAPI.Data.Config;
using TestMinimalAPI.Data.Models;

namespace TestMinimalAPI.Services;

interface ITestService
{
    string GetResponseText();
    Task<Person> GetPerson(Guid id);
}

public class TestService : ITestService
{
    private readonly PersonDbContext _context;

    public TestService(PersonDbContext context)
    {
        _context = context;
    }
    
    public string GetResponseText()
    {
        return "Success Test";
    }

    public async Task<Person> GetPerson(Guid id)
    {
        var person = await _context.People.FindAsync(id);

        return person ?? throw new HttpRequestException("Record Not Found", null, HttpStatusCode.NotFound);
    }
}