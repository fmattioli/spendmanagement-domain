using Microsoft.Extensions.Configuration;

namespace SpendManagement.Domain.Integration.Tests.Configuration
{
    public static class TestSettings
    {
        static TestSettings()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("testsettings.json", false, true)
               .Build();

            Kafka = config.GetSection("Kafka").Get<KafkaSettings>();
            SqlSettings = config.GetSection("SqlSettings").Get<SqlTestSettings>();
            Polling = config.GetSection("PollingSettings").Get<PollingSettings>();
        }

        public static KafkaSettings? Kafka { get; }
        public static SqlTestSettings? SqlSettings { get; }
        public static PollingSettings? Polling { get; }
    }
}
