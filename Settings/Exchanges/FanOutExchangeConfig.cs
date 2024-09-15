using RabbitMQ.Client;

namespace QueueSettings.Exchanges;

public class FanOutExchangeConfig
{
    public string Name => "simple-fan-out";
    public string Type => ExchangeType.Fanout;
    public bool Durable => false;
    public bool AutoDelete => true;
    public IDictionary<string, object> Arguments { get; set; }
}