using Application.Kafka.Mappers.Receipt;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;

namespace SpendManagement.Domain.Unit.Tests.Mappers
{
    public class ReceiptMapperTests
    {
        private readonly Fixture fixture;

        public ReceiptMapperTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void CreateReceiptCommand_ToDomain_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var createReceiptCommand = fixture.Create<CreateReceiptCommand>();

            // Act
            var result = createReceiptCommand.ToDomain();

            // Assert
            result.RoutingKey
                .Should()
                .Be(createReceiptCommand.RoutingKey);

            result.DataCommand
                .Should()
                .Be(createReceiptCommand.CommandCreatedDate);

            result.NameCommand
                .Should()
                .Be(nameof(CreateReceiptCommand));

            result.CommandBody
                .Should()
                .BeEquivalentTo(JsonConvert.SerializeObject(createReceiptCommand));
        }

        [Fact]
        public void UpdateReceiptCommand_ToDomain_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var updateReceiptCommand = fixture.Create<UpdateReceiptCommand>();

            // Act
            var result = updateReceiptCommand.ToDomain();

            // Assert
            result.RoutingKey
                .Should()
                .Be(updateReceiptCommand.RoutingKey);

            result.DataCommand
                .Should()
                .Be(updateReceiptCommand.CommandCreatedDate);

            result.NameCommand
                .Should()
                .Be(nameof(UpdateReceiptCommand));

            result.CommandBody
                .Should()
                .BeEquivalentTo(JsonConvert.SerializeObject(updateReceiptCommand));
        }

        [Fact]
        public void DeleteReceiptCommand_ToDomain_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var deleteReceiptCommand = fixture.Create<DeleteReceiptCommand>();

            // Act
            var result = deleteReceiptCommand.ToDomain();

            // Assert
            result.RoutingKey
                .Should()
                .Be(deleteReceiptCommand.RoutingKey);

            result.DataCommand
                .Should()
                .Be(deleteReceiptCommand.CommandCreatedDate);

            result.NameCommand
                .Should()
                .Be(nameof(DeleteReceiptCommand));

            result.CommandBody
                .Should()
                .BeEquivalentTo(JsonConvert.SerializeObject(deleteReceiptCommand));
        }

        [Fact]
        public void ToReceiptCreatedEvent_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var createReceiptCommand = fixture.Create<CreateReceiptCommand>();

            // Act
            var result = createReceiptCommand.ToReceiptCreatedEvent();

            // Assert
            result.Receipt.Id
                .Should()
                .Be(createReceiptCommand.Receipt.Id);

            result.Receipt.CategoryId
                .Should()
                .Be(createReceiptCommand.Receipt.CategoryId);

            result.Receipt.EstablishmentName
                .Should()
                .Be(createReceiptCommand.Receipt.EstablishmentName);

            result.Receipt.ReceiptDate
                .Should()
                .Be(createReceiptCommand.Receipt.ReceiptDate);

            result.Receipt.Discount
                .Should()
                .Be(createReceiptCommand.Receipt.Discount);

            result.Receipt.Total
                .Should()
                .Be(createReceiptCommand.Receipt.Total);

            result.ReceiptItem
                .Should()
                .HaveSameCount(createReceiptCommand.ReceiptItems);

            result.ReceiptItem
                .Should()
                .BeEquivalentTo(createReceiptCommand.ReceiptItems);
        }

        [Fact]
        public void ToReceiptUpdatedEvent_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var updateReceiptCommand = fixture.Create<UpdateReceiptCommand>();

            // Act
            var result = updateReceiptCommand.ToUpdateReceiptEvent();

            // Assert
            result.Receipt.Id
                .Should()
                .Be(updateReceiptCommand.Receipt.Id);

            result.Receipt.CategoryId
                .Should()
                .Be(updateReceiptCommand.Receipt.CategoryId);

            result.Receipt.EstablishmentName
                .Should()
                .Be(updateReceiptCommand.Receipt.EstablishmentName);

            result.Receipt.ReceiptDate
                .Should()
                .Be(updateReceiptCommand.Receipt.ReceiptDate);

            result.Receipt.Discount
                .Should()
                .Be(updateReceiptCommand.Receipt.Discount);

            result.Receipt.Total
                .Should()
                .Be(updateReceiptCommand.Receipt.Total);

            result.ReceiptItems
                .Should()
                .HaveSameCount(updateReceiptCommand.ReceiptItems);

            result.ReceiptItems
                .Should()
                .BeEquivalentTo(updateReceiptCommand.ReceiptItems);
        }

        [Fact]
        public void ToDeleteReceiptEvent_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var deleteReceiptCommand = fixture.Create<DeleteReceiptCommand>();

            // Act
            var result = deleteReceiptCommand.ToDeleteReceiptEvent();

            // Assert
            result.Id
                .Should()
                .Be(deleteReceiptCommand.Id);
        }
    }
}
