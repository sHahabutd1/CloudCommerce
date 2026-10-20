using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Catalog.IntegrationTests.Infrastructure;

public sealed class CatalogWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public CatalogWebApplicationFactory(
        string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override IHost CreateHost(
        IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(
            config =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:CatalogDb"] =
                            _connectionString
                    });
            });

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
}