using System.Text;
using System.Text.Json;
using MassTransit.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMQPublisher(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task PublishMessageAsync<T>(T message, string queueName)
    {
        try
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var messageJson = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageJson);
            var properties = new BasicProperties()
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body, basicProperties: properties, mandatory: false);
        }
        catch (System.Exception)
        {
            Console.WriteLine("Some error happened in background while connecting with rabbitmq");   
        }
    }
}
