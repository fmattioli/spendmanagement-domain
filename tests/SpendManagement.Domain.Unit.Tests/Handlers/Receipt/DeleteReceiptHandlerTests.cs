using Application.Kafka.Commands.Handlers;
using Application.Kafka.Events.Interfaces;
using AutoFixture;
using Data.Persistence.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using KafkaFlow;
using Moq;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;

namespace SpendManagement.Domain.Unit.Tests.Handlers.Receipt
{
    public class DeleteReceiptHandlerTests
    {
        private readonly ReceiptCommandHandler _receiptHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<ISpendManagementCommandRepository> _commandRepository = new();
        private readonly Mock<ISpendManagementEventRepository> _eventRepository = new();
        private readonly Mock<IMessageContext> _messageContext = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        public DeleteReceiptHandlerTests()
        {
            _receiptHandler = new(_eventProducer.Object, _unitOfWork.Object);
        }

        [Fact(DisplayName = "On Given a DeleteReceiptCommand, a command should inserted on DB and an DeleteReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidDeleteReceiptCommand_CommandShouldBeInserted_ShouldBeProduced_An_DeleteReceiptEvent()
        {
            //Arrange
            var deleteReceiptCommand = _fixture.Create<DeleteReceiptCommand>();

            _commandRepository
                .Setup(x => x.Add(It.IsAny<SpendManagementCommand>()))
                .ReturnsAsync(_fixture.Create<int>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _receiptHandler.Handle(_messageContext.Object, deleteReceiptCommand);

            //Assert
            _commandRepository
               .Verify(
                  x => x.Add(It.IsAny<SpendManagementCommand>()),
                   Times.Once);

            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _commandRepository.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "On Given a DeleteReceiptCommand, an event should inserted on DB and an DeleteReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidDeleteReceiptEvent_CommandShouldBeInserted_ShouldBeProduced_An_DeleteReceiptEvent()
        {
            //Arrange
            var deleteReceiptCommand = _fixture.Create<DeleteReceiptCommand>();

            _eventRepository
                .Setup(x => x.Add(It.IsAny<SpendManagementEvent>()))
                .ReturnsAsync(_fixture.Create<int>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _receiptHandler.Handle(_messageContext.Object, deleteReceiptCommand);

            //Assert
            _eventRepository
               .Verify(
                  x => x.Add(It.IsAny<SpendManagementEvent>()),
                   Times.Once);

            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _eventRepository.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
        }
    }
}
