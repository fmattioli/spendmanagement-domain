namespace SpendManagement.Domain.Integration.Tests.Fixtures
{
    [CollectionDefinition(nameof(SharedFixtureCollection))]
    public class SharedFixtureCollection :
         ICollectionFixture<SharedFixture>,
         ICollectionFixture<KafkaFixture>,
         ICollectionFixture<SqlFixture>
    {
    }
}
