/*
 * This a simple consumer, that listens message from a one named queue
 */
using System.Text;
using QueueSettings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost"
};

using (var connection = connectionFactory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
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
            Console.WriteLine($"Message: {message}, was received");
        };
        
        channel.BasicConsume(queue.Name, true, consumer);
        
        Console.WriteLine("Press [enter] to exit.");
    }
}
