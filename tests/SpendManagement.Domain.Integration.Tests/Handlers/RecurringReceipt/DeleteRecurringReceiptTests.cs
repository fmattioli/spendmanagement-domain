﻿using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using Polly;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;
using SpendManagement.Contracts.V1.Events.RecurringReceiptEvents;
using SpendManagement.Domain.Integration.Tests.Configuration;
using SpendManagement.Domain.Integration.Tests.Fixtures;

namespace SpendManagement.Domain.Integration.Tests.Handlers.RecurringReceipt
{
    [Collection(nameof(SharedFixtureCollection))]
    public class DeleteReceiptTests(KafkaFixture kafkaFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture _kafkaFixture = kafkaFixture;

        [Fact]
        private async Task OnGivenAValidReceipt_ShouldBeCreateACommandAndEventOnDb_And_ShouldBeProduce_DeleteRecurringReceiptEvent()
        {
            //Arrange
            var receiptId = fixture.Create<Guid>();

            var deleteReceiptCommand = fixture
                .Build<DeleteRecurringReceiptCommand>()
                .With(x => x.RoutingKey, receiptId.ToString())
                .With(x => x.Id, receiptId)
                .Create();

            //Act
            await _kafkaFixture.ProduceCommandAsync(deleteReceiptCommand);

            // Assert
            var command = await Policy
                .HandleResult<SpendManagementCommand>(
                    p => p?.RoutingKey == null)
                .WaitAndRetryAsync(
                    TestSettings.Polling!.RetryCount,
                    _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
                .ExecuteAsync(() => SqlFixture.GetCommandAsync(receiptId.ToString()));

            command.Should().NotBeNull();
            command.NameCommand.Should().Be(nameof(DeleteRecurringReceiptCommand));
            command.RoutingKey.Should().Be(receiptId.ToString());
            command.CommandBody.Should().NotBeNull();

            var deleteReceiptEvent = _kafkaFixture.Consume<DeleteRecurringReceiptEvent>(
                (deleteReceiptEvent, _) =>
                deleteReceiptEvent.Id == receiptId &&
                deleteReceiptEvent.RoutingKey == receiptId.ToString());

            deleteReceiptEvent
                .Should()
                .NotBeNull();
        }
    }
}
