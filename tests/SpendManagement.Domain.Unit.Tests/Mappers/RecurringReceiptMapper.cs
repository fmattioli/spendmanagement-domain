using AutoFixture;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;
using Application.Services.Mappers.RecurringReceipt;
using FluentAssertions;
namespace SpendManagement.Domain.Unit.Tests.Mappers
{
    public class RecurringRecurringReceiptMapper
    {
        private readonly Fixture fixture;

        public RecurringRecurringReceiptMapper()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void CreateRecurringReceiptCommand_ToDomain_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var createRecurringReceiptCommand = fixture.Create<CreateRecurringReceiptCommand>();

            // Act
            var result = createRecurringReceiptCommand.ToCommandDomain();

            // Assert
            result.RoutingKey
                .Should()
                .Be(createRecurringReceiptCommand.RoutingKey);

            result.DataCommand
                .Should()
                .Be(createRecurringReceiptCommand.CommandCreatedDate);

            result.NameCommand
                .Should()
                .Be(nameof(CreateRecurringReceiptCommand));

            result.CommandBody
                .Should()
                .BeEquivalentTo(JsonConvert.SerializeObject(createRecurringReceiptCommand));
        }

        [Fact]
        public void UpdateRecurringReceiptCommand_ToDomain_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var updateRecurringReceiptCommand = fixture.Create<UpdateRecurringReceiptCommand>();

            // Act
            var result = updateRecurringReceiptCommand.ToCommandDomain();

            // Assert
            result.RoutingKey
                .Should()
                .Be(updateRecurringReceiptCommand.RoutingKey);

            result.DataCommand
                .Should()
                .Be(updateRecurringReceiptCommand.CommandCreatedDate);

            result.NameCommand
                .Should()
                .Be(nameof(UpdateRecurringReceiptCommand));

            result.CommandBody
                .Should()
                .BeEquivalentTo(JsonConvert.SerializeObject(updateRecurringReceiptCommand));
        }

        [Fact]
        public void DeleteRecurringReceiptCommand_ToDomain_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var deleteRecurringReceiptCommand = fixture.Create<DeleteRecurringReceiptCommand>();

            // Act
            var result = deleteRecurringReceiptCommand.ToCommandDomain();

            // Assert
            result.RoutingKey
                .Should()
                .Be(deleteRecurringReceiptCommand.RoutingKey);

            result.DataCommand
                .Should()
                .Be(deleteRecurringReceiptCommand.CommandCreatedDate);

            result.NameCommand
                .Should()
                .Be(nameof(DeleteRecurringReceiptCommand));

            result.CommandBody
                .Should()
                .BeEquivalentTo(JsonConvert.SerializeObject(deleteRecurringReceiptCommand));
        }

        [Fact]
        public void ToRecurringReceiptCreatedEvent_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var createRecurringReceiptCommand = fixture.Create<CreateRecurringReceiptCommand>();

            // Act
            var result = createRecurringReceiptCommand.ToRecurringReceiptCreatedEvent();

            // Assert
            result.RecurringReceipt.Id
                .Should()
                .Be(createRecurringReceiptCommand.RecurringReceipt.Id);

            result.RecurringReceipt.CategoryId
                .Should()
                .Be(createRecurringReceiptCommand.RecurringReceipt.CategoryId);

            result.RecurringReceipt.EstablishmentName
                .Should()
                .Be(createRecurringReceiptCommand.RecurringReceipt.EstablishmentName);

            result.RecurringReceipt.DateInitialRecurrence
                .Should()
                .Be(createRecurringReceiptCommand.RecurringReceipt.DateInitialRecurrence);

            result.RecurringReceipt.DateEndRecurrence
                .Should()
                .Be(createRecurringReceiptCommand.RecurringReceipt.DateEndRecurrence);

            result.RecurringReceipt.RecurrenceTotalPrice
                .Should()
                .Be(createRecurringReceiptCommand.RecurringReceipt.RecurrenceTotalPrice);

            result.RecurringReceipt.Observation
                .Should()
                .Be(createRecurringReceiptCommand.RecurringReceipt.Observation);
        }

        [Fact]
        public void ToRecurringReceiptUpdatedEvent_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var updateRecurringReceiptCommand = fixture.Create<UpdateRecurringReceiptCommand>();

            // Act
            var result = updateRecurringReceiptCommand.ToUpdateRecurringReceiptEvent();

            // Assert
            result.RecurringReceipt.CategoryId
                .Should()
                .Be(updateRecurringReceiptCommand.RecurringReceipt.CategoryId);

            result.RecurringReceipt.EstablishmentName
                .Should()
                .Be(updateRecurringReceiptCommand.RecurringReceipt.EstablishmentName);

            result.RecurringReceipt.DateInitialRecurrence
                .Should()
                .Be(updateRecurringReceiptCommand.RecurringReceipt.DateInitialRecurrence);

            result.RecurringReceipt.DateEndRecurrence
                .Should()
                .Be(updateRecurringReceiptCommand.RecurringReceipt.DateEndRecurrence);

            result.RecurringReceipt.RecurrenceTotalPrice
                .Should()
                .Be(updateRecurringReceiptCommand.RecurringReceipt.RecurrenceTotalPrice);

            result.RecurringReceipt.Observation
                .Should()
                .Be(updateRecurringReceiptCommand.RecurringReceipt.Observation);
        }

        [Fact]
        public void ToDeleteRecurringReceiptEvent_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var deleteRecurringReceiptCommand = fixture.Create<DeleteRecurringReceiptCommand>();

            // Act
            var result = deleteRecurringReceiptCommand.ToDeleteRecurringReceiptEvent();

            // Assert
            result.Id
                .Should()
                .Be(deleteRecurringReceiptCommand.Id);
        }
    }
}
