using System.Text;
using QueueSettings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace Reciever;

public class Consumer
{
    public static async Task Run(object number, TaskCompletionSource taskCompletionSource)
    {
        if (number is int numberInt)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            
            var indent = new string(' ', numberInt * Constants.IndentLength);

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    Console.WriteLine(indent + $"Started Consumer {numberInt + 1}");
                    var queue = new BasicQueue();

                    channel.QueueDeclare(
                        queue.Name,
                        queue.IsDurable,
                        queue.IsExclusive,
                        queue.AutoDelete,
                        queue.Arguments
                    );

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (sender, eventArgs) =>
                    {
                        var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                        Console.WriteLine(indent + $"Recieved: \"{message}\" by consumer {numberInt + 1}");
                    };

                    channel.BasicConsume(queue.Name, true, consumer);
                    
                    await taskCompletionSource.Task;
                }
            }
        }
    }
}