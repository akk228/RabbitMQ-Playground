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
        
        var message = "Wazzap!";
        var body = Encoding.UTF8.GetBytes(message);
        
        channel.BasicPublish("", routingKey: queue.Name, mandatory: false, body: body);
        Console.WriteLine($"Message: {message} was Published");
    }
}
Console.ReadKey();