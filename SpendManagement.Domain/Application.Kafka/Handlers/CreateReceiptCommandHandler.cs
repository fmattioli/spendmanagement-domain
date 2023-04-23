using Application.Kafka.Converters;
using Data.Statements;
using Domain.Interfaces;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using SpendManagement.Contracts.V1.Commands;


namespace Application.Kafka.Handlers
{
    public class CreateReceiptCommandHandler : IMessageHandler<CreateReceiptCommand>
    {
        private readonly ILogger log;
        private readonly IReceiptRepository receiptRepository;
        public CreateReceiptCommandHandler(ILogger log, IReceiptRepository receiptRepository)
        {
            this.log = log;
            this.receiptRepository = receiptRepository;
        }

        public async Task Handle(IMessageContext context, CreateReceiptCommand message)
        {
            var receiptDomain = message.ToDomain();
            var receiptId = await receiptRepository.Add(receiptDomain, SqlCommands.InsertReceipt());
            await receiptRepository.AddReceiptItem(receiptId, receiptDomain.ReceiptItems);

            this.log.Information(
                $"Spent saved with successfully on database.",
                () => new
                {
                    receiptDomain
                });
        }
    }
}
