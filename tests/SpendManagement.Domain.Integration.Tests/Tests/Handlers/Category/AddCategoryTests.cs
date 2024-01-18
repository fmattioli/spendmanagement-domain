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
    public class AddCategoryTests(KafkaFixture kafkaFixture, SqlFixture sqlFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture _kafkaFixture = kafkaFixture;
        private readonly SqlFixture _sqlFixture = sqlFixture;

        [Fact(DisplayName = "On adding a valid category, a command should be inserted on the database, and a CreateCategoryEvent should be produced.")]
        private async Task OnGivenAValidCategory_ShouldBeCreateACommandAndEventOnDb_And_ShouldBeProduce_CreateCategoryEvent()
        {
            // Arrange
            var categoryId = fixture.Create<Guid>();

            var category = fixture
                .Build<Contracts.V1.Entities.Category>()
                .With(x => x.Id, categoryId)
                .Create();

            var categoryCommand = fixture
                .Build<CreateCategoryCommand>()
                .With(x => x.RoutingKey, categoryId.ToString())
                .With(x => x.Category, category)
                .Create();

            // Act
            await this._kafkaFixture.ProduceCommandAsync(categoryCommand);

            // Assert
            var command = await Policy
                .HandleResult<SpendManagementCommand>(
                    p => p?.RoutingKey == null)
                .WaitAndRetryAsync(
                    TestSettings.Polling!.RetryCount,
                    _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
                .ExecuteAsync(() => _sqlFixture.GetCommandAsync(categoryId.ToString()));

            command.Should().NotBeNull();
            command.NameCommand.Should().Be(nameof(CreateCategoryCommand));
            command.RoutingKey.Should().Be(categoryId.ToString());
            command.CommandBody.Should().NotBeNull();

            var spendManagementEvent = await Policy
                .HandleResult<SpendManagementEvent>(
                    p => p?.RoutingKey == null)
                .WaitAndRetryAsync(
                    TestSettings.Polling!.RetryCount,
                    _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
                .ExecuteAsync(() => _sqlFixture.GetEventAsync(categoryCommand.RoutingKey));

            spendManagementEvent.Should().NotBeNull();
            spendManagementEvent.NameEvent.Should().Be(nameof(CreateCategoryEvent));
            spendManagementEvent.RoutingKey.Should().Be(categoryCommand.RoutingKey);
            spendManagementEvent.EventBody.Should().NotBeNull();

            var createCategoryEvent = _kafkaFixture.Consume<CreateCategoryEvent>(
                (createCategoryEvent, _) =>
                createCategoryEvent.Category.Id == categoryId &&
                createCategoryEvent.RoutingKey == categoryId.ToString());

            createCategoryEvent
                .Should()
                .NotBeNull();
        }
    }
}
