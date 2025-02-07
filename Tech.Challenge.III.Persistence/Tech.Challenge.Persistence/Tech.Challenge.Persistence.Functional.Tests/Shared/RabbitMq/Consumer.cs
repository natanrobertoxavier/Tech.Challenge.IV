using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Tech.Challenge.Persistence.Functional.Tests.Shared.RabbitMq;

public class Consumer
{
    private readonly string _hostName = "localhost";
    public string ProcessedMessage { get; private set; }

    public event Action<string> OnMessageReceived;
    public void StartConsuming(string queueName)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[x] Received {message}");

            ProcessedMessage = message;
            OnMessageReceived?.Invoke(message);
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }
}
