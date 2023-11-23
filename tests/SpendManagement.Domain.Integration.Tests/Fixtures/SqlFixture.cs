using Dapper;
using Domain.Entities;
using SpendManagement.Domain.Integration.Tests.Configuration;
using System.Data.SqlClient;

namespace SpendManagement.Domain.Integration.Tests.Fixtures
{
    public class SqlFixture : IAsyncLifetime
    {
        private readonly List<string> routingKeys = new();

        public async Task<SpendManagementCommand> GetCommandAsync(string commandId)
        {
            using var connection = new SqlConnection(TestSettings.SqlSettings?.ConnectionString);
            var command = await connection.QueryFirstOrDefaultAsync<SpendManagementCommand>("SELECT * FROM SpendManagementCommands WHERE RoutingKey = @id", new { id = commandId });
            routingKeys.Add(commandId);
            return command!;
        }

        public async Task<SpendManagementEvent> GetEventAsync(string eventId)
        {
            using var connection = new SqlConnection(TestSettings.SqlSettings?.ConnectionString);
            {
                var @event = await connection.QueryFirstOrDefaultAsync<SpendManagementEvent>("SELECT * FROM SpendManagementEvents WHERE RoutingKey = @id", new { id = eventId });
                routingKeys.Add(eventId);
                return @event!;
            }
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            if (routingKeys.Any())
            {
                using var connection = new SqlConnection(TestSettings.SqlSettings?.ConnectionString);
                await connection.ExecuteAsync("DELETE FROM SpendManagementEvents WHERE RoutingKey in @ids", new { ids = routingKeys.Select(x => x) });
                await connection.ExecuteAsync("DELETE FROM SpendManagementCommands WHERE RoutingKey in @ids", new { ids = routingKeys.Select(x => x) });
            }
        }
    }
}
