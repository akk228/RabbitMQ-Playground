using RabbitMQ.Client;
namespace QueueSettings;
/// <summary>
/// Class that declares settings for a named exclusive queue.
/// </summary>
/// <remarks>
/// I made this queue exclusive, so it will be cleaned up after you close the connection
/// that declared whether it is a publisher or consumer
/// </remarks>
public class BasicQueue
{
    public string Name => "MyFirstQueue";
    public bool IsDurable => false;
    public bool IsExclusive => true;
    public bool AutoDelete => false;
    public IDictionary<string, object> Arguments { get; set; }
}