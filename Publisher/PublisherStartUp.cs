/*
 * This is the publisher that publishes messages to an exchange
 */
using System.Text;
using RabbitMQ.Client;
using QueueSettings;
using QueueSettings.Exchanges;

var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost"
};

using (var connection = connectionFactory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {

        var fanOutExchangeConfig = new FanOutExchangeConfig();
        
        channel.ExchangeDeclare(
            exchange: fanOutExchangeConfig.Name,
            type: fanOutExchangeConfig.Type,
            durable: fanOutExchangeConfig.Durable,
            autoDelete: fanOutExchangeConfig.AutoDelete,
            arguments: fanOutExchangeConfig.Arguments);
        
        var messageCount = 1;
        
        Console.WriteLine("Enter number of messages : ");
        try
        {
            messageCount = int.Parse(Console.ReadLine() ?? "1");
        }
        catch (Exception e){
        }
        
        
        for (int i = 1; i <= messageCount; i++) {
            var message = "Message " + i;
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(fanOutExchangeConfig.Name, routingKey: "", mandatory: false, body: body);
            Console.WriteLine($"Message: \"{message}\" was Published");
        }
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
