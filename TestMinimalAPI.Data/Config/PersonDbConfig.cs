using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestMinimalAPI.Data.Config;

public static class PersonDbConfig
{
    private const string ConnectionString = "Server=localhost:5432;Database=test-minimal-api;User Id=postgres;Password=postgres";
    public static IServiceCollection AddPersonDbContext(this IServiceCollection service, string connectionString = ConnectionString)
    {
        service.AddDbContext<PersonDbContext>(opt => 
            opt.UseNpgsql(connectionString));

        return service;
    }
}