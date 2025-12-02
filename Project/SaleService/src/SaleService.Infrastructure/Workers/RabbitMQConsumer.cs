using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace SaleService.Infrastructure.Workers
{
    public class RabbitMQConsumer
    {
        private readonly IConnection connection;
        private readonly Dictionary<string, Func<string, Task>> handlers = new();
        public RabbitMQConsumer(IConnection _connection)
        {
            this.connection = _connection;
        }
        public void RegisterHandler(string eventName, Func<string, Task> _handler)
        {
            this.handlers[eventName] = _handler;
        }
        public async Task Start()
        {
            try
            {
                foreach (var eventName in this.handlers.Keys)
                {
                    var channel = await this.connection.CreateChannelAsync();
                    await channel.ExchangeDeclareAsync(exchange: eventName, type: ExchangeType.Fanout, durable: true);
                    var queueName = $"{eventName}-queue";
                    var queue = await channel.QueueDeclareAsync(
                        queue: queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false
                        );
                    await channel.QueueBindAsync(queue: queueName, exchange: eventName, routingKey: "");
                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += async (_, ea) =>
                    {
                        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                        await this.handlers[eventName](body);
                    };
                    await channel.BasicConsumeAsync(queue: queue, autoAck: true, consumer: consumer);
                    Console.WriteLine($"✅ Consumer ativo para o evento '{eventName}' usando fila '{queueName}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}