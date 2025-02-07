using RabbitMQ.Client;
using System.Text;

namespace Tech.Challenge.Persistence.Functional.Tests.Shared.RabbitMq;

public class Publisher
{
    private string _queueName = "FunctionalTestQueue";
    private readonly string _hostName = "localhost";
    private IModel _channel;

    public void ConfigureQueue(string queueName)
    {
        _queueName = queueName;

        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.QueueDeclare(
            queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: true,
            arguments: null
        );

        Console.WriteLine($"Fila {_queueName} configurada no Publisher.");
    }

    public void PublishMessage(string message)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: _queueName,
                             basicProperties: null,
                             body: body);

        Console.WriteLine($"[x] Sent {message}");
    }
}
