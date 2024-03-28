using Application.Services.Commands.Handlers;
using Application.Services.Events.Interfaces;
using AutoFixture;
using Data.Persistence.Interfaces;
using Data.Persistence.UnitOfWork;

using Domain.Entities;
using Domain.Interfaces;
using KafkaFlow;
using Moq;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;

using System.Data;

namespace SpendManagement.Domain.Unit.Tests.Handlers.Receipt
{
    public class DeleteReceiptHandlerTests
    {
        private readonly VariableReceiptCommandHandler _receiptHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<IDbTransaction> _dbTransactionObject = new();
        private readonly Mock<ISpendManagementCommandRepository> _commandRepositoryMock = new();
        private readonly Mock<IMessageContext> _messageContext = new();

        public DeleteReceiptHandlerTests()
        {
            var unitOfWork = new UnitOfWork(_dbTransactionObject.Object, _commandRepositoryMock.Object);
            _receiptHandler = new(_eventProducer.Object, unitOfWork);
        }

        [Fact(DisplayName = "On Given a DeleteReceiptCommand, a command should inserted on DB and an DeleteReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidDeleteReceiptCommand_CommandShouldBeInserted_ShouldBeProduced_An_DeleteReceiptEvent()
        {
            //Arrange
            var deleteReceiptCommand = _fixture.Create<DeleteReceiptCommand>();

            _commandRepositoryMock
                .Setup(x => x.Add(It.IsAny<SpendManagementCommand>()))
                .ReturnsAsync(_fixture.Create<Guid>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _receiptHandler.Handle(_messageContext.Object, deleteReceiptCommand);

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

        [Fact(DisplayName = "On Given a DeleteReceiptCommand, an event should inserted on DB and an DeleteReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidDeleteReceiptEvent_CommandShouldBeInserted_ShouldBeProduced_An_DeleteReceiptEvent()
        {
            //Arrange
            var deleteReceiptCommand = _fixture.Create<DeleteReceiptCommand>();

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _receiptHandler.Handle(_messageContext.Object, deleteReceiptCommand);

            //Assert
            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _eventProducer.VerifyNoOtherCalls();
        }
    }
}
