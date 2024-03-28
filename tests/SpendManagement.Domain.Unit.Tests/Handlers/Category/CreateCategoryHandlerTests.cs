using Application.Services.Commands.Handlers;
using Application.Services.Events.Interfaces;
using AutoFixture;
using Data.Persistence.Interfaces;
using Data.Persistence.UnitOfWork;
using Domain.Entities;
using Domain.Interfaces;
using KafkaFlow;
using Moq;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;

using System.Data;

namespace SpendManagement.Domain.Unit.Tests.Handlers.Category
{
    public class CreateCategoryHandlerTests
    {
        private readonly CategoryCommandHandler _categoryHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<IDbTransaction> _dbTransactionObject = new();
        private readonly Mock<ISpendManagementCommandRepository> _commandRepositoryMock = new();
        private readonly Mock<IMessageContext> _messageContext = new();

        public CreateCategoryHandlerTests()
        {
            var unitOfWork = new UnitOfWork(_dbTransactionObject.Object, _commandRepositoryMock.Object);
            _categoryHandler = new(_eventProducer.Object, unitOfWork);
        }

        [Fact(DisplayName = "On Given a CreateCategoryCommand, a command should inserted on DB and a CreateCategoryEvent should be produced")]
        public async Task Handle_OnGivenAValidCreateCategoryCommand_SpendManagementCommandShouldBeInserted()
        {
            //Arrange
            var createCategoryCommand = _fixture.Create<CreateCategoryCommand>();

            _commandRepositoryMock
                .Setup(x => x.Add(It.IsAny<SpendManagementCommand>()))
                .ReturnsAsync(_fixture.Create<Guid>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _categoryHandler.Handle(_messageContext.Object, createCategoryCommand);

            //Assert
            _commandRepositoryMock
               .Verify(
                  x => x.Add(It.IsAny<SpendManagementCommand>()),
                   Times.Once);

            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<SpendManagement.Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _commandRepositoryMock.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "On Given a CreateCategoryCommand, an event should inserted on DB and a CreateCategoryEvent should be produced")]
        public async Task Handle_OnGivenAValidCreateCategoryCommand_AnCreateCategoryEvent_ShouldBeProduced()
        {
            //Arrange
            var createCategoryCommand = _fixture.Create<CreateCategoryCommand>();

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);


            //Act
            await _categoryHandler.Handle(_messageContext.Object, createCategoryCommand);

            //Assert
            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _eventProducer.VerifyNoOtherCalls();
        }
    }
}
