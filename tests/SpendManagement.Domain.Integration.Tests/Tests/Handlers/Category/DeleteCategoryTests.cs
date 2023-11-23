using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using Polly;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Events.CategoryEvents;
using SpendManagement.Domain.Integration.Tests.Configuration;
using SpendManagement.Domain.Integration.Tests.Fixtures;

namespace SpendManagement.Domain.Integration.Tests.Tests.Handlers.Category
{
    [Collection(nameof(SharedFixtureCollection))]
    public class DeleteCategoryTests
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture _kafkaFixture;
        private readonly SqlFixture _sqlFixture;

        public DeleteCategoryTests(KafkaFixture kafkaFixture, SqlFixture sqlFixture)
        {
            this._kafkaFixture = kafkaFixture;
            this._sqlFixture = sqlFixture;
        }

        [Fact(DisplayName = "On deleting a valid category, a command should be inserted on the database, and a DeleteCategoryEvent should be produced.")]
        private async Task OnGivenAValidCategory_ShouldBeInsertedACommand_Should_Produced_DeleteCategoryCommand()
        {
            //Arrange
            var categoryId = fixture.Create<Guid>();

            var categoryDeleteComand = fixture
                .Build<DeleteCategoryCommand>()
                .With(x => x.RoutingKey, categoryId.ToString())
                .With(x => x.Id, categoryId)
                .Create();

            // Act
            await this._kafkaFixture.ProduceCommandAsync(categoryDeleteComand);

            // Assert
            var command = await Policy
                .HandleResult<SpendManagementCommand>(
                    p => p?.RoutingKey == null)
                .WaitAndRetryAsync(
                    TestSettings.Polling!.RetryCount,
                    _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
                .ExecuteAsync(() => _sqlFixture.GetCommandAsync(categoryDeleteComand.RoutingKey));

            command.Should().NotBeNull();
            command.NameCommand.Should().Be(nameof(DeleteCategoryCommand));
            command.RoutingKey.Should().Be(categoryDeleteComand.RoutingKey);
            command.CommandBody.Should().NotBeNull();

            var spendManagementEvent = await Policy
               .HandleResult<SpendManagementEvent>(
                   p => p?.RoutingKey == null)
               .WaitAndRetryAsync(
                   TestSettings.Polling!.RetryCount,
                   _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
               .ExecuteAsync(() => _sqlFixture.GetEventAsync(categoryDeleteComand.RoutingKey));

            spendManagementEvent.Should().NotBeNull();
            spendManagementEvent.NameEvent.Should().Be(nameof(DeleteCategoryEvent));
            spendManagementEvent.RoutingKey.Should().Be(categoryDeleteComand.RoutingKey);
            spendManagementEvent.EventBody.Should().NotBeNull();

            var deleteCategoryEvent = _kafkaFixture.Consume<DeleteCategoryEvent>(
                (deleteCategoryEvent, _) =>
                deleteCategoryEvent.Id == categoryId &&
                deleteCategoryEvent.RoutingKey == categoryId.ToString());

            deleteCategoryEvent
                .Should()
                .NotBeNull();
        }
    }
}
