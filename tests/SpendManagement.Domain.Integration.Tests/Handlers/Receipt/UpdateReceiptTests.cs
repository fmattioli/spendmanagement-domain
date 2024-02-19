using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using Polly;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Events.ReceiptEvents;
using SpendManagement.Domain.Integration.Tests.Configuration;
using SpendManagement.Domain.Integration.Tests.Fixtures;

namespace SpendManagement.Domain.Integration.Tests.Handlers.Receipt
{
    [Collection(nameof(SharedFixtureCollection))]
    public class UpdateReceiptTests(KafkaFixture kafkaFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture _kafkaFixture = kafkaFixture;

        [Fact(DisplayName = "On updating a valid receipt, a command should be inserted on the database, and a UpdateReceiptEvent should be produced.")]
        private async Task OnGivenAValidReceipt_ShouldBeCreateACommandAndEventOnDb_And_ShouldBeProduce_UpdateReceiptEvent()
        {
            //Arrange
            var receiptId = fixture.Create<Guid>();

            var receipt = fixture
                .Build<Contracts.V1.Entities.Receipt>()
                .With(x => x.Id, receiptId)
                .Create();

            var receiptItems = fixture
                .Build<Contracts.V1.Entities.ReceiptItem>()
                .CreateMany(1);

            var updateReceiptCommand = fixture
                .Build<UpdateReceiptCommand>()
                .With(x => x.RoutingKey, receiptId.ToString())
                .With(x => x.Receipt, receipt)
                .With(x => x.ReceiptItems, receiptItems)
                .Create();

            //Act
            await _kafkaFixture.ProduceCommandAsync(updateReceiptCommand);

            // Assert
            var command = await Policy
                .HandleResult<SpendManagementCommand>(
                    p => p?.RoutingKey == null)
                .WaitAndRetryAsync(
                    TestSettings.Polling!.RetryCount,
                    _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
                .ExecuteAsync(() => SqlFixture.GetCommandAsync(receiptId.ToString()));

            command.Should().NotBeNull();
            command.NameCommand.Should().Be(nameof(UpdateReceiptCommand));
            command.RoutingKey.Should().Be(receiptId.ToString());
            command.CommandBody.Should().NotBeNull();

            var updateReceiptEvent = _kafkaFixture.Consume<UpdateReceiptEvent>(
                (updateReceiptEvent, _) =>
                updateReceiptEvent.Receipt.Id == receiptId &&
                updateReceiptEvent.RoutingKey == receiptId.ToString());

            updateReceiptEvent
                .Should()
                .NotBeNull();
        }
    }
}
