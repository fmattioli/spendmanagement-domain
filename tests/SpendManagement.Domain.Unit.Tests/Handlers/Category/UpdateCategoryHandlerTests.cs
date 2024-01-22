using Application.Kafka.Commands.Handlers;
using Application.Kafka.Events.Interfaces;
using AutoFixture;
using Data.Persistence.UnitOfWork;
using Domain.Entities;
using Domain.Interfaces;
using KafkaFlow;
using Moq;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using System.Data;

namespace SpendManagement.Domain.Unit.Tests.Handlers.Category
{
    public class UpdateCategoryHandlerTests
    {
        private readonly CategoryCommandHandler _categoryHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IMessageContext> _messageContext = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<IDbTransaction> _dbTransactionObject = new();
        private readonly Mock<ISpendManagementCommandRepository> _commandRepositoryMock = new();
        private readonly Mock<ISpendManagementEventRepository> _eventRepositoryMock = new();

        public UpdateCategoryHandlerTests()
        {
            var unitOfWork = new UnitOfWork(_dbTransactionObject.Object, _commandRepositoryMock.Object, _eventRepositoryMock.Object);
            _categoryHandler = new(_eventProducer.Object, unitOfWork);
        }

        [Fact(DisplayName = "On Given a UpdateCategoryCommand, a command should inserted on DB and an UpdateCategoryEvent should be produced")]
        public async Task Handle_OnGivenAValidUpdateCategoryCommand_CommandShouldBeInserted_And_ShouldBeProducedUpdateCategoryEvent()
        {
            //Arrange
            var updateCategoryCommand = _fixture.Create<UpdateCategoryCommand>();

            _commandRepositoryMock
                .Setup(x => x.Add(It.IsAny<SpendManagementCommand>()))
                .ReturnsAsync(_fixture.Create<int>());

            _eventRepositoryMock
               .Setup(x => x.Add(It.IsAny<SpendManagementEvent>()))
               .ReturnsAsync(_fixture.Create<int>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _categoryHandler.Handle(_messageContext.Object, updateCategoryCommand);

            //Assert
            _commandRepositoryMock
               .Verify(
                  x => x.Add(It.IsAny<SpendManagementCommand>()),
                   Times.Once);

            _eventRepositoryMock
               .Verify(
                  x => x.Add(It.IsAny<SpendManagementEvent>()),
                   Times.Once);

            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<SpendManagement.Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _eventRepositoryMock.VerifyNoOtherCalls();
            _commandRepositoryMock.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
        }
    }
}
