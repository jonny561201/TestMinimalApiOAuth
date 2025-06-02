using System.Data.Common;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using Xunit;

namespace TestMinimalAPI.TestcontainerTests.Config;

public class DatabaseFixture : IAsyncLifetime
{
    public const string CollectionName = "Database";

    private PostgreSqlContainer _dbContainer = null!;

    private IContainer _flywayContainer = null!;
    
    private Respawner _respawner = null!;
    private const string UsernameAndPassword = "postgres";
    private const string DatabaseName = "test-minimal-api";
    private const int DatabasePort = 5432;

    public string ConnectionString => _dbContainer.GetConnectionString();

    private DbConnection GetOpenDbConnection()
    {
        var connection = new NpgsqlConnection(ConnectionString);
        connection.Open();
        return connection;
    }
    
    public async Task ResetDatabase() => await _respawner.ResetAsync(GetOpenDbConnection());

    
    private string GetRootDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var rootDirectory = currentDirectory[..currentDirectory.IndexOf("/TestMinimalAPI.TestcontainerTests", StringComparison.Ordinal)];
        return rootDirectory;
    }

    private string GetFlywayDirectory()
    {
        var rootDirectory = GetRootDirectory();
        var hostFlywayDirectory = Path.Combine(rootDirectory, "infra/migrations/");
        return hostFlywayDirectory;
    }

    public async Task InitializeAsync()
    {
        var network = new NetworkBuilder()
            .Build();
        
        _dbContainer = GetPostgresContainer(network);
        _flywayContainer = GetFlywayContainer(network);
        
        await _dbContainer.StartAsync();
        await _flywayContainer.StartAsync();
        await _flywayContainer.GetExitCodeAsync();
        
        _respawner = await Respawner.CreateAsync(GetOpenDbConnection(), new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }

    private static PostgreSqlContainer GetPostgresContainer(INetwork network)
    {
        return new PostgreSqlBuilder()
            .WithName("postgres-db")
            .WithUsername(UsernameAndPassword)
            .WithPassword(UsernameAndPassword)
            .WithDatabase(DatabaseName)
            .WithPortBinding(DatabasePort)
            .WithNetwork(network)
            .WithAutoRemove(true)
            .Build();
    }

    private IContainer GetFlywayContainer(INetwork network)
    {
        return new ContainerBuilder()
            .WithImage("flyway/flyway:10-alpine")
            .WithName("flyway-migrations")
            .WithResourceMapping(GetFlywayDirectory(), "/flyway/sql")
            .WithEntrypoint("flyway")
            .WithCommand($"-url=jdbc:postgresql://postgres-db:{DatabasePort}/{DatabaseName}",
                $"-user={UsernameAndPassword}",
                $"-password={UsernameAndPassword}",
                "-connectRetries=20",
                "-placeholders.env=local",
                "migrate")
            .WithNetwork(network)
            .WithAutoRemove(true)
            .Build();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _flywayContainer.StopAsync();
        
        await _dbContainer.DisposeAsync();
        await _flywayContainer.DisposeAsync();
    }
}

[CollectionDefinition(DatabaseFixture.CollectionName)]
public class DatabaseFixtureCollection : ICollectionFixture<DatabaseFixture>;
