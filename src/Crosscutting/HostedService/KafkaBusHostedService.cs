using KafkaFlow;
using Microsoft.Extensions.Hosting;

namespace Crosscutting.HostedService
{
    internal class KafkaBusHostedService : IHostedService
    {
        private readonly IKafkaBus kafkaBus;

        public KafkaBusHostedService(IServiceProvider serviceProvider)
        {
            kafkaBus = serviceProvider.CreateKafkaBus();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Kafka started");
            await kafkaBus.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Kafka stopped");
            await kafkaBus.StopAsync();
        }
    }
}
