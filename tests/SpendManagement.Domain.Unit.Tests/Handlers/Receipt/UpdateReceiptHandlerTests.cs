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
    public class UpdateReceiptHandlerTests
    {
        private readonly ReceiptCommandHandler _receiptHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<ISpendManagementCommandRepository> _commandRepository = new();
        private readonly Mock<ISpendManagementEventRepository> _eventRepository = new();
        private readonly Mock<IMessageContext> _messageContext = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        public UpdateReceiptHandlerTests()
        {
            _receiptHandler = new(_eventProducer.Object, _unitOfWork.Object);
        }

        [Fact(DisplayName = "On Given a UpdateReceiptCommand, a command should inserted on DB and an UpdateReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidUpdateReceiptCommand_CommandShouldBeInserted_And_UpdateReceiptEventShouldBeProduced()
        {
            //Arrange
            var updateReceiptCommand = _fixture.Create<UpdateReceiptCommand>();

            _commandRepository
                .Setup(x => x.Add(It.IsAny<SpendManagementCommand>()))
                .ReturnsAsync(_fixture.Create<int>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _receiptHandler.Handle(_messageContext.Object, updateReceiptCommand);

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

        [Fact(DisplayName = "On Given a UpdateReceiptCommand, an Event should inserted on DB and an UpdateReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidUpdateReceiptCommand_EventShouldBeInserted_And_UpdateReceiptEventShouldBeProduced()
        {
            //Arrange
            var updateReceiptCommand = _fixture.Create<UpdateReceiptCommand>();

            _eventRepository
                .Setup(x => x.Add(It.IsAny<SpendManagementEvent>()))
                .ReturnsAsync(_fixture.Create<int>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _receiptHandler.Handle(_messageContext.Object, updateReceiptCommand);

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
