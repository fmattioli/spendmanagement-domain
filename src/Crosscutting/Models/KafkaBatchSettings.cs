namespace Crosscutting.Models
{
    public class KafkaBatchSettings
    {
        public int WorkerCount { get; set; }
        public int BufferSize { get; set; }
        public int MessageTimeoutSec { get; set; }
    }
}
