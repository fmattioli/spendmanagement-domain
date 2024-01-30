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
    public class AddReceiptTests(KafkaFixture kafkaFixture, SqlFixture sqlFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture _kafkaFixture = kafkaFixture;
        private readonly SqlFixture _sqlFixture = sqlFixture;

        [Fact(DisplayName = "On adding a valid receipt, a command should be inserted on the database, and a CreateReceiptEvent should be produced.")]
        private async Task OnGivenAValidReceipt_ShouldBeCreateACommandAndEventOnDb_And_ShouldBeProduce_CreateReceiptEvent()
        {
            var receiptId = fixture.Create<Guid>();

            var receipt = fixture
                .Build<Contracts.V1.Entities.Receipt>()
                .With(x => x.Id, receiptId)
                .Create();

            var receiptItems = fixture
                .Build<Contracts.V1.Entities.ReceiptItem>()
                .CreateMany(1);

            var createReceiptCommand = fixture
                .Build<CreateReceiptCommand>()
                .With(x => x.RoutingKey, receiptId.ToString())
                .With(x => x.Receipt, receipt)
                .With(x => x.ReceiptItems, receiptItems)
                .Create();

            await this._kafkaFixture.ProduceCommandAsync(createReceiptCommand);

            // Assert
            var command = await Policy
                .HandleResult<SpendManagementCommand>(
                    p => p?.RoutingKey == null)
                .WaitAndRetryAsync(
                    TestSettings.Polling!.RetryCount,
                    _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
                .ExecuteAsync(() => _sqlFixture.GetCommandAsync(receiptId.ToString()));

            command.Should().NotBeNull();
            command.NameCommand.Should().Be(nameof(CreateReceiptCommand));
            command.RoutingKey.Should().Be(receiptId.ToString());
            command.CommandBody.Should().NotBeNull();

            var createReceiptEvent = _kafkaFixture.Consume<CreatedReceiptEvent>(
                (createReceiptEvent, _) =>
                createReceiptEvent.Receipt.Id == receiptId &&
                createReceiptEvent.RoutingKey == receiptId.ToString());

            createReceiptEvent
                .Should()
                .NotBeNull();
        }
    }
}
