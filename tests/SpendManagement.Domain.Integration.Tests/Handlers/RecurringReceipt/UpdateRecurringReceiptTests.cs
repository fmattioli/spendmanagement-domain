using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using Polly;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;
using SpendManagement.Domain.Integration.Tests.Configuration;
using SpendManagement.Domain.Integration.Tests.Fixtures;

namespace SpendManagement.Domain.Integration.Tests.Handlers.RecurringReceipt
{
    [Collection(nameof(SharedFixtureCollection))]
    public class UpdateRecurringReceiptTests(KafkaFixture kafkaFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture _kafkaFixture = kafkaFixture;

        [Fact]
        private async Task OnGivenAValidReceipt_ShouldBeCreateACommandAndEventOnDb_And_ShouldBeProduce_UpdateRecurringReceiptEvent()
        {
            //Arrange
            var receiptId = fixture.Create<Guid>();

            var receipt = fixture
                .Build<Contracts.Contracts.V1.Entities.RecurringReceipt>()
                .With(x => x.Id, receiptId)
                .Create();

            var updateReceiptCommand = fixture
                .Build<UpdateRecurringReceiptCommand>()
                .With(x => x.RoutingKey, receiptId.ToString())
                .With(x => x.RecurringReceipt, receipt)
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

            command
                .Should()
                .NotBeNull();

            command.NameCommand
                .Should()
                .Be(nameof(UpdateRecurringReceiptCommand));

            command.RoutingKey
                .Should()
                .Be(receiptId.ToString());

            command.CommandBody
                .Should()
                .NotBeNull();

            var updateReceiptEvent = _kafkaFixture.Consume<UpdateRecurringReceiptCommand>(
                (updateReceiptEvent, _) =>
                updateReceiptEvent.RecurringReceipt.Id == receiptId &&
                updateReceiptEvent.RoutingKey == receiptId.ToString());

            updateReceiptEvent
                .Should()
                .NotBeNull();
        }
    }
}
