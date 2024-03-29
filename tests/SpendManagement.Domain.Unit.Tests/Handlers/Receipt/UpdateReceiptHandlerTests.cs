﻿using Application.Kafka.Commands.Handlers;
using Application.Kafka.Events.Interfaces;
using AutoFixture;
using Data.Persistence.UnitOfWork;
using Domain.Entities;
using Domain.Interfaces;
using KafkaFlow;
using Moq;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using System.Data;

namespace SpendManagement.Domain.Unit.Tests.Handlers.Receipt
{
    public class UpdateReceiptHandlerTests
    {
        private readonly ReceiptCommandHandler _receiptHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<IDbTransaction> _dbTransactionObject = new();
        private readonly Mock<ISpendManagementCommandRepository> _commandRepositoryMock = new();
        private readonly Mock<IMessageContext> _messageContext = new();

        public UpdateReceiptHandlerTests()
        {
            var unitOfWork = new UnitOfWork(_dbTransactionObject.Object, _commandRepositoryMock.Object);
            _receiptHandler = new(_eventProducer.Object, unitOfWork);
        }

        [Fact(DisplayName = "On Given a UpdateReceiptCommand, a command should inserted on DB and an UpdateReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidUpdateReceiptCommand_CommandShouldBeInserted_And_UpdateReceiptEventShouldBeProduced()
        {
            //Arrange
            var updateReceiptCommand = _fixture.Create<UpdateReceiptCommand>();

            _commandRepositoryMock
                .Setup(x => x.Add(It.IsAny<SpendManagementCommand>()))
                .ReturnsAsync(_fixture.Create<Guid>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _receiptHandler.Handle(_messageContext.Object, updateReceiptCommand);

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

        [Fact(DisplayName = "On Given a UpdateReceiptCommand, an Event should inserted on DB and an UpdateReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidUpdateReceiptCommand_EventShouldBeInserted_And_UpdateReceiptEventShouldBeProduced()
        {
            //Arrange
            var updateReceiptCommand = _fixture.Create<UpdateReceiptCommand>();

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            //Act
            await _receiptHandler.Handle(_messageContext.Object, updateReceiptCommand);

            //Assert
            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _eventProducer.VerifyNoOtherCalls();
        }
    }
}
