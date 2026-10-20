using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.IntegrationTests.Infrastructure;

public sealed class DatabaseResetter
{
    private readonly CatalogTestFixture _fixture;

    public DatabaseResetter(
        CatalogTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task ResetProductsAsync()
    {
        using var scope =
            _fixture.Factory.Services.CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<CatalogDbContext>();

        await dbContext.Products.ExecuteDeleteAsync();
    }
}