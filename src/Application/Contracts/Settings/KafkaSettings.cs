namespace Application.Contracts.Settings
{
    public class KafkaSettings : IKafkaSettings
    {
        public string BootstrapServer { get; set; } = string.Empty;
        public string ConsumerGroup { get; set; } = string.Empty;
    }
}