﻿using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using Polly;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Events.CategoryEvents;
using SpendManagement.Domain.Integration.Tests.Configuration;
using SpendManagement.Domain.Integration.Tests.Fixtures;

namespace SpendManagement.Domain.Integration.Tests.Handlers.Category
{
    [Collection(nameof(SharedFixtureCollection))]
    public class UpdateCategoryTests(KafkaFixture kafkaFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture _kafkaFixture = kafkaFixture;

        [Fact(DisplayName = "On updating a valid category, a command should be inserted on the database, and a UpdateCategoryEvent should be produced.")]
        private async Task OnGivenAValidCategory_ShouldBeCreateACommandAndEventOnDb_And_ShouldBeProduce_UpdateCategoryEvent()
        {
            //Arrange
            var categoryId = fixture.Create<Guid>();

            var category = fixture
                .Build<Contracts.V1.Entities.Category>()
                .With(x => x.Id, categoryId)
                .Create();

            var categoryUpdateCommand = fixture
                .Build<UpdateCategoryCommand>()
                .With(x => x.RoutingKey, categoryId.ToString())
                .With(x => x.Category, category)
                .Create();

            // Act
            await _kafkaFixture.ProduceCommandAsync(categoryUpdateCommand);

            // Assert
            var command = await Policy
                .HandleResult<SpendManagementCommand>(
                    p => p?.RoutingKey == null)
                .WaitAndRetryAsync(
                    TestSettings.Polling!.RetryCount,
                    _ => TimeSpan.FromMilliseconds(TestSettings.Polling.Delay))
                .ExecuteAsync(() => SqlFixture.GetCommandAsync(categoryId.ToString()));

            command.Should().NotBeNull();
            command.NameCommand.Should().Be(nameof(UpdateCategoryCommand));
            command.RoutingKey.Should().Be(categoryId.ToString());
            command.CommandBody.Should().NotBeNull();

            var updateCategoryEvent = _kafkaFixture.Consume<UpdateCategoryEvent>(
                (createCategoryEvent, _) =>
                createCategoryEvent.Category.Id == categoryId &&
                createCategoryEvent.RoutingKey == categoryId.ToString());

            updateCategoryEvent
                .Should()
                .NotBeNull();
        }
    }
}
