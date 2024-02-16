using AutoFixture;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using Application.Kafka.Mappers.Category;
using FluentAssertions;

namespace SpendManagement.Domain.Unit.Tests.Mappers
{
    public class CategoryMapperTests
    {
        private readonly Fixture fixture;

        public CategoryMapperTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void CreateCategoryCommand_ToDomain_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var createCategoryCommand = fixture.Create<CreateCategoryCommand>();

            // Act
            var result = createCategoryCommand.ToDomain();

            // Assert
            result.RoutingKey
                .Should()
                .Be(createCategoryCommand.RoutingKey);

            result.DataCommand
                .Should()
                .Be(createCategoryCommand.CommandCreatedDate);

            result.NameCommand
                .Should()
                .Be(nameof(CreateCategoryCommand));

            result.CommandBody
                .Should()
                .BeEquivalentTo(JsonConvert.SerializeObject(createCategoryCommand));
        }

        [Fact]
        public void UpdateCategoryCommand_ToDomain_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var createCategoryCommand = fixture.Create<UpdateCategoryCommand>();

            // Act
            var result = createCategoryCommand.ToDomain();

            // Assert
            result.RoutingKey
                .Should()
                .Be(createCategoryCommand.RoutingKey);

            result.DataCommand
                .Should()
                .Be(createCategoryCommand.CommandCreatedDate);

            result.NameCommand
                .Should()
                .Be(nameof(UpdateCategoryCommand));

            result.CommandBody
                .Should()
                .BeEquivalentTo(JsonConvert.SerializeObject(createCategoryCommand));
        }

        [Fact]
        public void DeleteCategoryCommand_ToDomain_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var deleteCategoryCommand = fixture.Create<DeleteCategoryCommand>();

            // Act
            var result = deleteCategoryCommand.ToDomain();

            // Assert
            result.RoutingKey
                .Should()
                .Be(deleteCategoryCommand.RoutingKey);

            result.DataCommand
                .Should()
                .Be(deleteCategoryCommand.CommandCreatedDate);

            result.NameCommand
                .Should()
                .Be(nameof(DeleteCategoryCommand));

            result.CommandBody
                .Should()
                .BeEquivalentTo(JsonConvert.SerializeObject(deleteCategoryCommand));
        }

        [Fact]
        public void ToCreateCategoryEvent_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var categoryId = fixture.Create<Guid>();

            var category = fixture
                .Build<Contracts.V1.Entities.Category>()
                .With(x => x.Id, categoryId)
                .Create();

            var createCategoryCommand = fixture
                .Build<CreateCategoryCommand>()
                .With(x => x.RoutingKey, categoryId.ToString())
                .With(x => x.Category, category)
                .Create();

            // Act
            var result = createCategoryCommand.ToCreateCategoryEvent();

            // Assert
            result.Category.Id
                .Should()
                .Be(createCategoryCommand.Category.Id);

            result.Category.Name
                .Should()
                .Be(createCategoryCommand.Category.Name);

            result.RoutingKey
                .Should()
                .Be(createCategoryCommand.RoutingKey);
        }

        [Fact]
        public void ToUpdateCategoryEvent_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var categoryId = fixture.Create<Guid>();

            var category = fixture
                .Build<Contracts.V1.Entities.Category>()
                .With(x => x.Id, categoryId)
                .Create();

            var updateCategoryCommand = fixture
                .Build<UpdateCategoryCommand>()
                .With(x => x.RoutingKey, categoryId.ToString())
                .With(x => x.Category, category)
                .Create();

            // Act
            var result = updateCategoryCommand.ToUpdateCategoryEvent();

            // Assert
            result.Category.Id
                .Should()
                .Be(updateCategoryCommand.Category.Id);

            result.Category.Name
                .Should()
                .Be(updateCategoryCommand.Category.Name);

            result.RoutingKey
                .Should()
                .Be(updateCategoryCommand.RoutingKey);
        }

        [Fact]
        public void ToDeleteCategoryEvent_ShouldMapPropertiesCorrectly()
        {
            // Arrange
            var categoryId = fixture.Create<Guid>();

            var deleteCategoryCommand = fixture
                .Build<DeleteCategoryCommand>()
                .With(x => x.RoutingKey, categoryId.ToString())
                .With(x => x.Id, categoryId)
                .Create();

            // Act
            var result = deleteCategoryCommand.ToDeleteCategoryEvent();

            // Assert
            result.Id
                .Should()
                .Be(deleteCategoryCommand.Id);

            result.RoutingKey
                .Should()
                .Be(deleteCategoryCommand.RoutingKey);
        }
    }
}
