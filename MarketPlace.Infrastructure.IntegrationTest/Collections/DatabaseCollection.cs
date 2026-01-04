using MarketPlace.Infrastructure.IntegrationTest.Fixtures;

namespace MarketPlace.Infrastructure.IntegrationTest.Collections
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}
