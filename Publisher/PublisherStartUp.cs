/*
 * This is the simplest possible publisher, that publishes messages to a single named queue.
 */
using System.Text;
using RabbitMQ.Client;
using QueueSettings;

var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost"
};

using (var connection = connectionFactory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        // use just Named queue
        var queue = new BasicQueue();
        
        channel.QueueDeclare(
            queue.Name,
            queue.IsDurable,
            queue.IsExclusive,
            queue.AutoDelete,
            queue.Arguments
        );

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
            channel.BasicPublish("", routingKey: queue.Name, mandatory: false, body: body);
            Console.WriteLine($"Message: \"{message}\" was Published");
        }
    }
}
