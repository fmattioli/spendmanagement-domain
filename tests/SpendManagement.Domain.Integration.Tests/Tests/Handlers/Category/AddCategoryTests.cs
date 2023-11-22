﻿using AutoFixture;
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
    public class AddCategoryTests
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture _kafkaFixture;
        private readonly SqlFixture _sqlFixture;

        public AddCategoryTests(KafkaFixture kafkaFixture, SqlFixture sqlFixture)
        {
            this._kafkaFixture = kafkaFixture;
            this._sqlFixture = sqlFixture;
        }

        [Fact(DisplayName = "On adding a valid category, a command should be inserted on the database, and a CreateCategoryEvent should be produced.")]
        private async Task OnGivenAValidCategory_ShouldBeProducedACreateCategoryCommand()
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
                .HandleResult<Command>(
                    p => p?.RoutingKey == null)
                .WaitAndRetryAsync(
                    TestSettings.Polling!.RetryCount,
                    _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
                .ExecuteAsync(() => _sqlFixture.GetCommandAsync(categoryCommand.RoutingKey));

            command.Should().NotBeNull();
            command.NameCommand.Should().Be(nameof(CreateCategoryCommand));
            command.RoutingKey.Should().Be(categoryCommand.RoutingKey);
            command.CommandBody.Should().NotBeNull();

            var createCategoryEvent = _kafkaFixture.Consume<CreateCategoryEvent>(
                (@event, _) =>
                @event.Category.Id == categoryId &&
                @event.RoutingKey == categoryId.ToString());

            createCategoryEvent
                .Should()
                .NotBeNull();
        }

        [Fact(DisplayName = "On adding a valid category, an event should be inserted on the database, and a CreateCategoryEvent should be produced.")]
        private async Task OnGivenAValidCategory_EventsShouldHaveCorrectName()
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
            var @event = await Policy
                .HandleResult<Event>(
                    p => p?.RoutingKey == null)
                .WaitAndRetryAsync(
                    TestSettings.Polling!.RetryCount,
                    _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
                .ExecuteAsync(() => _sqlFixture.GetEventAsync(categoryCommand.RoutingKey));

            @event.Should().NotBeNull();
            @event.NameEvent.Should().Be(nameof(CreateCategoryEvent));
            @event.RoutingKey.Should().Be(categoryCommand.RoutingKey);
            @event.EventBody.Should().NotBeNull();

            var createCategoryEvent = _kafkaFixture.Consume<CreateCategoryEvent>(
                (@event, _) =>
                @event.Category.Id == categoryId &&
                @event.RoutingKey == categoryId.ToString());

            createCategoryEvent
                .Should()
                .NotBeNull();
        }
    }
}