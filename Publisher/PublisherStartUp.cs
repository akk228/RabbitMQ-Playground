/*
 * This is the publisher that publishes messages to an exchange
 */
using System.Text;
using RabbitMQ.Client;
using QueueSettings;
using QueueSettings.Bindings;
using QueueSettings.Exchanges;

var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost"
};

using (var connection = connectionFactory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {

        var directExchangeConfig = new DirectExchangeConfig();
        
        channel.ExchangeDeclare(
            exchange: directExchangeConfig.Name,
            type: directExchangeConfig.Type,
            durable: directExchangeConfig.Durable,
            autoDelete: directExchangeConfig.AutoDelete,
            arguments: directExchangeConfig.Arguments);
        
        var roundsCount = 1;
        
        Console.WriteLine("Enter number of message rounds to send : ");
        try
        {
            roundsCount = int.Parse(Console.ReadLine() ?? "1");
        }
        catch (Exception e){
        }

        string[] greetings = [GreetingType.Greeting, GreetingType.Farewell, GreetingType.InformalFarewell];
        
        for (int i = 1; i <= roundsCount; i++) {
            foreach (var greeting in greetings)
            {
                var message = greeting + i;
                var body = Encoding.UTF8.GetBytes(message);
                
                channel.BasicPublish(
                    directExchangeConfig.Name,
                    routingKey: greeting,
                    mandatory: false,
                    body: body);
                
                Console.WriteLine($"Message: \"{message}\" was Published");   
            }
        }
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
