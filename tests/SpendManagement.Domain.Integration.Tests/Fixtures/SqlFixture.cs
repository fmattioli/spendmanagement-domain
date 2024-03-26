using Dapper;
using Domain.Entities;
using Npgsql;
using SpendManagement.Domain.Integration.Tests.Configuration;

namespace SpendManagement.Domain.Integration.Tests.Fixtures
{
    public class SqlFixture : IAsyncLifetime
    {
        public static async Task<SpendManagementCommand> GetCommandAsync(string commandId)
        {
            using var connection = new NpgsqlConnection(TestSettings.SqlSettings?.ConnectionString);
            var command = await connection.QueryFirstOrDefaultAsync<SpendManagementCommand>("SELECT * FROM SpendManagementCommands WHERE RoutingKey = @id", new { id = commandId });
            return command!;
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            using var connection = new NpgsqlConnection(TestSettings.SqlSettings?.ConnectionString);
            await connection.ExecuteAsync("DELETE FROM SpendManagementCommands");
        }
    }

    public class SpendManagementEvent(string routingKey, DateTime dataEvent, string nameEvent, string eventBody)
    {
        public string RoutingKey { get; set; } = routingKey;
        public DateTime DataEvent { get; set; } = dataEvent;
        public string NameEvent { get; set; } = nameEvent;
        public string EventBody { get; set; } = eventBody;
    }
}
