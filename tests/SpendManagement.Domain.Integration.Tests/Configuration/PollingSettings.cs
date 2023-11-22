namespace SpendManagement.Domain.Integration.Tests.Configuration
{
    public sealed class PollingSettings
    {
        public int RetryCount { get; set; }

        public int Delay { get; set; }
    }
}
