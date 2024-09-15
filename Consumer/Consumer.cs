/*
 * Here we create several consumers. And you can see that messages are going to consumers in a round-robbin manner.
 * Namely, every consumer gets message one after another.
 * Consumer 1 Consumer 2
 * 
 * message 1  Message 2
 * message 3  Message 4
 *
 * etc...
 *
 * You can provide waiting function to consumers and see that each one will get next message from the queue
 */
using System.Text;
using QueueSettings;
using QueueSettings.Exchanges;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace Consumer;

public class Consumer
{
    public static async Task Run(int number, List<string> greetingTypes, TaskCompletionSource taskCompletionSource)
    {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            
            var indent = new string(' ', number * Constants.IndentLength);

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    Console.WriteLine(indent + $"Started Consumer for types: {string.Join(", ", greetingTypes)}");
                    
                    var queue = channel.QueueDeclare();
                    var fanOutExchangeConfig = new DirectExchangeConfig();

                    // binds queue to exchange to listen for a messages with specific Greeting Types
                    foreach (var greetingType in greetingTypes)
                    {
                        channel.QueueBind(
                            queue: queue.QueueName,
                            exchange: fanOutExchangeConfig.Name, 
                            routingKey: greetingType);   
                    }
                    
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (sender, eventArgs) =>
                    {
                        var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                        Console.WriteLine(indent + $"Recieved: \"{message}\"");
                    };

                    channel.BasicConsume(queue.QueueName, true, consumer);
                    
                    await taskCompletionSource.Task;
                }
            }
    }
}