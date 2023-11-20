﻿using Application.Kafka.Commands.Handlers;
using Application.Kafka.Events.Interfaces;
using AutoFixture;
using Domain.Entities;
using Domain.Interfaces;
using KafkaFlow;
using Moq;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;

namespace SpendManagement.Domain.Unit.Tests.Handlers.Category
{
    public class DeleteCategoryHandlerTests
    {
        private readonly CategoryCommandHandler _categoryHandler;
        private readonly Fixture _fixture = new();
        private readonly Mock<IEventProducer> _eventProducer = new();
        private readonly Mock<ICommandRepository> _commandRepository = new();
        private readonly Mock<IEventRepository> _eventRepository = new();
        private readonly Mock<IMessageContext> _messageContext = new();

        public DeleteCategoryHandlerTests()
        {
            _categoryHandler = new(_commandRepository.Object, _eventRepository.Object, _eventProducer.Object);
        }

        [Fact(DisplayName = "On Given a DeleteCategoryCommand, an event and command should inserted on DB and an Event should be produced")]
        public async Task Handle_OnGivenAValidDeleteCategoryCommand_ShouldBeProduced_DeleteCategoryEvent()
        {
            //Arrange
            var deleteCategoryCommand = _fixture.Create<DeleteCategoryCommand>();

            _commandRepository
                .Setup(x => x.Add(It.IsAny<Command>()))
                .ReturnsAsync(_fixture.Create<int>());

            _eventProducer
                .Setup(x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()))
                .Returns(Task.CompletedTask);

            _eventRepository
                .Setup(x => x.Add(It.IsAny<Event>()))
                .ReturnsAsync(_fixture.Create<int>());

            //Act
            await _categoryHandler.Handle(_messageContext.Object, deleteCategoryCommand);

            //Assert
            _commandRepository
               .Verify(
                  x => x.Add(It.IsAny<Command>()),
                   Times.Once);

            _eventProducer
                .Verify(
                    x => x.SendEventAsync(It.IsAny<Contracts.V1.Interfaces.IEvent>()),
                    Times.Once);

            _eventRepository
                .Verify(
                    x => x.Add(It.IsAny<Event>()),
                    Times.Once);

            _commandRepository.VerifyNoOtherCalls();
            _eventProducer.VerifyNoOtherCalls();
            _eventRepository.VerifyNoOtherCalls();
        }
    }
}
