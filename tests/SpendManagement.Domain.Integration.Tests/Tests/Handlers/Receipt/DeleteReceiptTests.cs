using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using Polly;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Events.ReceiptEvents;
using SpendManagement.Domain.Integration.Tests.Configuration;
using SpendManagement.Domain.Integration.Tests.Fixtures;

namespace SpendManagement.Domain.Integration.Tests.Tests.Handlers.Receipt
{
    [Collection(nameof(SharedFixtureCollection))]
    public class DeleteReceiptTests(KafkaFixture kafkaFixture, SqlFixture sqlFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture _kafkaFixture = kafkaFixture;
        private readonly SqlFixture _sqlFixture = sqlFixture;

        [Fact(DisplayName = "On deleting a valid receipt, a command should be inserted on the database, and a DeleteReceiptEvent should be produced.")]
        private async Task OnGivenAValidReceipt_ShouldBeCreateACommandAndEventOnDb_And_ShouldBeProduce_DeleteReceiptEvent()
        {
            var receiptId = fixture.Create<Guid>();

            var deleteReceiptCommand = fixture
                .Build<DeleteReceiptCommand>()
                .With(x => x.RoutingKey, receiptId.ToString())
                .With(x => x.Id, receiptId)
                .Create();

            await this._kafkaFixture.ProduceCommandAsync(deleteReceiptCommand);

            // Assert
            var command = await Policy
                .HandleResult<SpendManagementCommand>(
                    p => p?.RoutingKey == null)
                .WaitAndRetryAsync(
                    TestSettings.Polling!.RetryCount,
                    _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
                .ExecuteAsync(() => _sqlFixture.GetCommandAsync(receiptId.ToString()));

            command.Should().NotBeNull();
            command.NameCommand.Should().Be(nameof(DeleteReceiptCommand));
            command.RoutingKey.Should().Be(receiptId.ToString());
            command.CommandBody.Should().NotBeNull();

            var deleteReceiptEvent = _kafkaFixture.Consume<DeleteReceiptEvent>(
                (deleteReceiptEvent, _) =>
                deleteReceiptEvent.Id == receiptId &&
                deleteReceiptEvent.RoutingKey == receiptId.ToString());

            deleteReceiptEvent
                .Should()
                .NotBeNull();
        }
    }
}
