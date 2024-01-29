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
    public class DeleteCategoryHandlerTests
    {
        private readonly CategoryCommandHandler _categoryHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<IMessageContext> _messageContext = new();
        private readonly Mock<IDbTransaction> _dbTransactionObject = new();
        private readonly Mock<ISpendManagementCommandRepository> _commandRepositoryMock = new();

        public DeleteCategoryHandlerTests()
        {
            var unitOfWork = new UnitOfWork(_dbTransactionObject.Object, _commandRepositoryMock.Object);
            _categoryHandler = new(_eventProducer.Object, unitOfWork);
        }

        [Fact(DisplayName = "On Given a DeleteCategoryCommand, an event and command should inserted on DB and an Event should be produced")]
        public async Task Handle_OnGivenAValidDeleteCategoryCommand_ShouldBeProduced_DeleteCategoryEvent()
        {
            //Arrange
            var deleteCategoryCommand = _fixture.Create<DeleteCategoryCommand>();

            _commandRepositoryMock
                .Setup(x => x.Add(It.IsAny<SpendManagementCommand>()))
                .ReturnsAsync(_fixture.Create<Guid>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _categoryHandler.Handle(_messageContext.Object, deleteCategoryCommand);

            //Assert
            _commandRepositoryMock
               .Verify(
                  x => x.Add(It.IsAny<SpendManagementCommand>()),
                   Times.Once);

            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _commandRepositoryMock.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "On Given a DeleteCategoryCommand, a command should inserted on DB and a DeleteCategoryEvent should be produced")]
        public async Task Handle_OnGivenAValidDeleteCategoryCommand_CommandShouldBeInserted_ShouldBeProduced_DeleteCategoryEvent()
        {
            //Arrange
            var deleteCategoryCommand = _fixture.Create<DeleteCategoryCommand>();

            _commandRepositoryMock
                .Setup(x => x.Add(It.IsAny<SpendManagementCommand>()))
                .ReturnsAsync(_fixture.Create<Guid>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);
            //Act
            await _categoryHandler.Handle(_messageContext.Object, deleteCategoryCommand);

            //Assert
            _commandRepositoryMock
               .Verify(
                  x => x.Add(It.IsAny<SpendManagementCommand>()),
                   Times.Once);

            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _commandRepositoryMock.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
        }
    }
}
