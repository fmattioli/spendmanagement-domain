using Application.Kafka.Converters;
using Application.Kafka.Events.Interfaces;
using Data.Statements;
using Domain.Interfaces;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;

using SpendManagement.Contracts.V1.Commands.ReceiptCommands;

namespace Application.Kafka.Commands.Handlers
{
    public class ReceiptCommandHandler : IMessageHandler<CreateReceiptCommand>
    {
        private readonly ILogger log;
        private readonly IReceiptRepository receiptRepository;
        private readonly IReceiptProducer receiptProducer;

        public ReceiptCommandHandler(ILogger log, IReceiptRepository receiptRepository, IReceiptProducer receiptProducer)
        {
            this.log = log;
            this.receiptRepository = receiptRepository;
            this.receiptProducer = receiptProducer;
        }

        public async Task Handle(IMessageContext context, CreateReceiptCommand message)
        {
            var receiptDomain = message.ToDomain();
            var receiptId = await receiptRepository.Add(receiptDomain, SqlCommands.InsertReceipt());
            await receiptRepository.AddReceiptItem(receiptId, receiptDomain.ReceiptItems);

            var receiptCreatedEvent = receiptDomain.ToReceiptCreatedEvent();
            await receiptProducer.SendEventAsync(receiptCreatedEvent);

            log.Information(
                $"Spent saved with successfully on database.",
                () => new
                {
                    receiptDomain
                });
        }
    }
}
