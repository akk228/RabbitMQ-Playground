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
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace Reciever;

public class Consumer
{
    public static async Task Run(int number, int delay, TaskCompletionSource taskCompletionSource)
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
                    Console.WriteLine(indent + $"Started Consumer {number + 1}");
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
                        Thread.Sleep(delay);
                        var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                        Console.WriteLine(indent + $"Recieved: \"{message}\" by consumer {number + 1}");
                    };

                    channel.BasicConsume(queue.Name, true, consumer);
                    
                    await taskCompletionSource.Task;
                }
            }
    }
}