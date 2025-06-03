using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TestMinimalAPI.Data.Config;
using Xunit;

namespace TestMinimalAPI.TestcontainerTests.Config;

[Collection(DatabaseFixture.CollectionName)]
public class AuthWebApplicationFactory(DatabaseFixture databaseFixture) : WebApplicationFactory<Program>, IClassFixture<DatabaseFixture>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        databaseFixture.ResetDatabase().Wait();
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<PersonDbContext>()
                .AddPersonDbContext(databaseFixture.ConnectionString);
            
            services
                .AddAuthentication(FakeAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>(FakeAuthHandler.SchemeName, opt => { });
        
            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = FakeAuthHandler.SchemeName;
                options.DefaultChallengeScheme = FakeAuthHandler.SchemeName;
            });
        });
    }
}