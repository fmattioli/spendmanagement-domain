namespace Crosscutting.Models
{
    public interface ISettings
    {
        public TracingSettings? TracingSettings { get; }
        public KafkaSettings KafkaSettings { get; }
        public SqlSettings SqlSettings { get; }
    }

    public record Settings : ISettings
    {
        public TracingSettings? TracingSettings { get; set; }
        public KafkaSettings KafkaSettings { get; set; } = null!;
        public SqlSettings SqlSettings { get; set; } = null!;
    }
}
