using Catalog.Infrastructure.Persistence;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.IntegrationTests.Infrastructure;

public sealed class CatalogTestFixture : IAsyncLifetime
{
    private readonly IContainer _postgresContainer;

    public CatalogWebApplicationFactory Factory { get; private set; } = default!;

    public HttpClient Client { get; private set; } = default!;

    public CatalogTestFixture()
    {
        _postgresContainer =
    new ContainerBuilder("postgres:18")
        .WithEnvironment(
            "POSTGRES_USER",
            "postgres")
        .WithEnvironment(
            "POSTGRES_PASSWORD",
            "postgres")
        .WithEnvironment(
            "POSTGRES_DB",
            "CatalogDb")
        .WithPortBinding(5432, true)
        .WithWaitStrategy(
            Wait.ForUnixContainer()
                .UntilMessageIsLogged(
                    "database system is ready to accept connections"))
        .Build();
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        var connectionString =
            $"Host={_postgresContainer.Hostname};" +
            $"Port={_postgresContainer.GetMappedPublicPort(5432)};" +
            "Database=CatalogDb;" +
            "Username=postgres;" +
            "Password=postgres";

        Factory =
            new CatalogWebApplicationFactory(
                connectionString);

        Client = Factory.CreateClient();

        using var scope =
            Factory.Services.CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<CatalogDbContext>();

        await dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        Client?.Dispose();

        Factory?.Dispose();

        await _postgresContainer.DisposeAsync();
    }
}