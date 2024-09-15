using RabbitMQ.Client;

namespace QueueSettings.Exchanges;

public class DirectExchangeConfig
{
    public string Name => "Greetings-Exchange";
    public string Type => ExchangeType.Direct;
    public bool Durable => false;
    public bool AutoDelete => true;
    public IDictionary<string, object> Arguments { get; set; }
}