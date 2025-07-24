using System.Text;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NotificationService.Config;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService.Services
{
    public class MessageConsumer : BackgroundService
    {
        private readonly ILogger<MessageConsumer> _logger;
        private readonly RabbitMQSettings _rabbitSettings;
        public MessageConsumer(ILogger<MessageConsumer> logger, IOptions<RabbitMQSettings> rabbitOptions)
        {
            _logger = logger;
            _rabbitSettings = rabbitOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitSettings.Uri)
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            _logger.LogInformation("Connected to RabbitMQ");

            await channel.QueueDeclareAsync(
                queue: _rabbitSettings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _logger.LogInformation($"Listening to queue: {_rabbitSettings.QueueName}");
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation($"[NotificationService] Received message: {message}");

                    await Task.Delay(500, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing message.");
                }
            };

            await channel.BasicConsumeAsync(
                queue: _rabbitSettings.QueueName,
                autoAck: true,
                consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);
            }

        }
    }
}
