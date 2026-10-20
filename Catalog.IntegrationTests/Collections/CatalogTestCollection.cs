using Catalog.IntegrationTests.Infrastructure;

namespace Catalog.IntegrationTests.Collections;

[CollectionDefinition("Catalog")]
public class CatalogTestCollection
    : ICollectionFixture<CatalogTestFixture>
{
}