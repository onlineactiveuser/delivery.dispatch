namespace Application.Contracts.Settings
{
    public interface IKafkaSettings
    {
        string BootstrapServer { get; set; }
        string ConsumerGroup { get; set; }
    }
}