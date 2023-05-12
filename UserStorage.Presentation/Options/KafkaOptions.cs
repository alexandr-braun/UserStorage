namespace UserStorage.Presentation.Options;

public class KafkaOptions
{
    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
    public string Topic { get; set; }
    public int BatchSize { get; set; }
    public int BatchTimeoutInSeconds { get; set; }
}