using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestMinimalAPI.Data.Config;

public static class PersonDbConfig
{
    public static IServiceCollection AddPersonDbContext(this IServiceCollection service)
    {
        service.AddDbContext<PersonDbContext>(opt => 
            opt.UseNpgsql("Server=localhost:5432;Database=test-minimal-api;User Id=postgres;Password=postgres"));

        return service;
    }
}