using Application.Kafka.Commands.Handlers;
using Application.Kafka.Events.Interfaces;
using AutoFixture;
using Domain.Entities;
using Domain.Interfaces;
using KafkaFlow;
using Moq;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;

namespace SpendManagement.Domain.Unit.Tests.Handlers.Receipt
{
    public class CreateReceiptHandlerTests
    {
        private readonly ReceiptCommandHandler _receiptHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<ISpendManagementCommandRepository> _commandRepository = new();
        private readonly Mock<ISpendManagementEventRepository> _eventRepository = new();
        private readonly Mock<IMessageContext> _messageContext = new();

        public CreateReceiptHandlerTests()
        {
            _receiptHandler = new(_commandRepository.Object, _eventRepository.Object, _eventProducer.Object);
        }

        [Fact(DisplayName = "On Given a CreateReceiptCommand, a command should inserted on DB and an CreateReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidCreateReceiptCommand_CommandShouldBeInserted_And_ShouldBeProducedCreateReceiptEvent()
        {
            //Arrange
            var createReceiptCommand = _fixture.Create<CreateReceiptCommand>();

            _commandRepository
                .Setup(x => x.Add(It.IsAny<SpendManagementCommand>()))
                .ReturnsAsync(_fixture.Create<int>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            _eventRepository
                .Setup(x => x.Add(It.IsAny<SpendManagementEvent>()))
                .ReturnsAsync(_fixture.Create<int>());

            //Act
            await _receiptHandler.Handle(_messageContext.Object, createReceiptCommand);

            //Assert
            _commandRepository
               .Verify(
                  x => x.Add(It.IsAny<SpendManagementCommand>()),
                   Times.Once);

            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<SpendManagement.Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _eventRepository
                .Verify(
                    x => x.Add(It.IsAny<SpendManagementEvent>()),
                    Times.Once);

            _commandRepository.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
            _eventRepository.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "On Given a CreateReceiptCommand, an event should inserted on DB and an CreateReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidCreateReceiptCommand_EventShouldBeInserted_And_ShouldBeProducedCreateReceiptEvent()
        {
            //Arrange
            var createReceiptCommand = _fixture.Create<CreateReceiptCommand>();

            _eventRepository
                .Setup(x => x.Add(It.IsAny<SpendManagementEvent>()))
                .ReturnsAsync(_fixture.Create<int>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _receiptHandler.Handle(_messageContext.Object, createReceiptCommand);

            //Assert
            _eventRepository
               .Verify(
                  x => x.Add(It.IsAny<SpendManagementEvent>()),
                   Times.Once);

            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<SpendManagement.Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _eventRepository.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
        }
    }
}
