using RabbitMQ.Client;
namespace QueueSettings;

public class BasicQueue
{
    public string Name => "MyFirstQueue";
    public bool IsDurable => false;
    public bool IsExclusive => false;
    public bool AutoDelete => false;
    public IDictionary<string, object> Arguments { get; set; }
}