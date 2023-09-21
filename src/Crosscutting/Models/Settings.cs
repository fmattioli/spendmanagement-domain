namespace Crosscutting.Models
{
    public interface ISettings
    {
        public KafkaSettings KafkaSettings { get; }
        public SqlSettings SqlSettings { get; }
    }

    public record Settings : ISettings
    {
        public KafkaSettings KafkaSettings { get; set; } = null!;
        public SqlSettings SqlSettings { get; set; } = null!;
    }
}
