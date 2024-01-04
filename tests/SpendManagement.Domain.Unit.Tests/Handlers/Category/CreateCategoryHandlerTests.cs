using Application.Kafka.Commands.Handlers;
using Application.Kafka.Events.Interfaces;
using AutoFixture;
using Data.Persistence.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using KafkaFlow;
using Moq;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;

namespace SpendManagement.Domain.Unit.Tests.Handlers.Category
{
    public class CreateCategoryHandlerTests
    {
        private readonly CategoryCommandHandler _categoryHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<ISpendManagementCommandRepository> _commandRepository = new();
        private readonly Mock<ISpendManagementEventRepository> _eventRepository = new();
        private readonly Mock<IMessageContext> _messageContext = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        public CreateCategoryHandlerTests()
        {
            _categoryHandler = new(_commandRepository.Object, _eventRepository.Object, _eventProducer.Object, _unitOfWork.Object);
        }

        [Fact(DisplayName = "On Given a CreateCategoryCommand, a command should inserted on DB and a CreateCategoryEvent should be produced")]
        public async Task Handle_OnGivenAValidCreateCategoryCommand_SpendManagementCommandShouldBeInserted()
        {
            //Arrange
            var createCategoryCommand = _fixture.Create<CreateCategoryCommand>();

            _commandRepository
                .Setup(x => x.Add(It.IsAny<SpendManagementCommand>()))
                .ReturnsAsync(_fixture.Create<int>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _categoryHandler.Handle(_messageContext.Object, createCategoryCommand);

            //Assert
            _commandRepository
               .Verify(
                  x => x.Add(It.IsAny<SpendManagementCommand>()),
                   Times.Once);

            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<SpendManagement.Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _commandRepository.VerifyNoOtherCalls();
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

            _eventRepository
                .Setup(x => x.Add(It.IsAny<SpendManagementEvent>()))
                .ReturnsAsync(_fixture.Create<int>());

            //Act
            await _categoryHandler.Handle(_messageContext.Object, createCategoryCommand);

            //Assert
            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<SpendManagement.Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _eventRepository
                .Verify(
                    x => x.Add(It.IsAny<SpendManagementEvent>()),
                    Times.Once);

            _eventRepository.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
        }
    }
}
