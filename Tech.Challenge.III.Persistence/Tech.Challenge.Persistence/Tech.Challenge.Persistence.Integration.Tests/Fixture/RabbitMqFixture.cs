using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Tech.Challenge.Persistence.Api.Models;
using Tech.Challenge.Persistence.Infrasctructure.Queue;

namespace Tech.Challenge.Persistence.Integration.Tests.Fixture;
public class RabbitMqFixture : IDisposable
{
    public IConnectionFactory ConnectionFactory { get; private set; }
    private IConnection _connection;
    private IModel _channel;

    public RabbitMqFixture()
    {
        ConnectionFactory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest", Port = 5672 };
        _connection = ConnectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: RabbitMqConstants.RegisterRegionQueueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public Task SendMessageAsync(RegisterRegionDDDModel message)
    {
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        _channel.BasicPublish(exchange: "",
                             routingKey: RabbitMqConstants.RegisterRegionQueueName,
                             basicProperties: null,
                             body: body);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
