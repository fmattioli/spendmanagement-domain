﻿using Application.Kafka.Commands.Handlers;
using Application.Kafka.Events.Interfaces;
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
    public class CreateReceiptHandlerTests
    {
        private readonly ReceiptCommandHandler _receiptHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<IMessageContext> _messageContext = new();
        private readonly Mock<IDbTransaction> _dbTransactionObject = new();
        private readonly Mock<ISpendManagementCommandRepository> _commandRepositoryMock = new();
        private readonly Mock<ISpendManagementEventRepository> _eventRepositoryMock = new();

        public CreateReceiptHandlerTests()
        {
            var unitOfWork = new UnitOfWork(_dbTransactionObject.Object, _commandRepositoryMock.Object, _eventRepositoryMock.Object);
            _receiptHandler = new(_eventProducer.Object, unitOfWork);
        }

        [Fact(DisplayName = "On Given a CreateReceiptCommand, a command should inserted on DB and an CreateReceiptEvent should be produced")]
        public async Task Handle_OnGivenAValidCreateReceiptCommand_CommandShouldBeInserted_And_ShouldBeProducedCreateReceiptEvent()
        {
            //Arrange
            var createReceiptCommand = _fixture.Create<CreateReceiptCommand>();

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
            await _receiptHandler.Handle(_messageContext.Object, createReceiptCommand);

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

            _commandRepositoryMock.VerifyNoOtherCalls();
            _eventRepositoryMock.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
        }
    }
}
