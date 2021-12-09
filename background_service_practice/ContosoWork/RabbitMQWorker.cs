using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ContosoWork
{
    public class RabbitMQWorker : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private string _cosumerTag;
        private readonly ILogger<RabbitMQWorker> _loggerRabbitMQ;

        public RabbitMQWorker(ILogger<RabbitMQWorker> logger)
        {
            _loggerRabbitMQ = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                UserName = "root",
                Password = "admin1234",
                VirtualHost = "/",
                HostName = "localhost"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "task_queue",
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false
            );
            _loggerRabbitMQ.LogInformation(" [*] Waiting for messages.");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Person ms = JsonSerializer.Deserialize<Person>(message);
                _loggerRabbitMQ.LogInformation($"Name: {ms.name}");
                _loggerRabbitMQ.LogInformation($"Age: {ms.age}");
                _loggerRabbitMQ.LogInformation(" [x] Done");
                _channel.BasicAck(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false
                );
                Thread.Sleep(500);
            };
            _cosumerTag = _channel.BasicConsume(
                queue: "task_queue",
                autoAck: false,
                consumer: consumer
            );
            _loggerRabbitMQ.LogInformation(" Press [enter] to exit");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.BasicCancel(_cosumerTag);
            _channel.Close(); // first
            _connection.Close(); // second after closing channel
            return base.StopAsync(cancellationToken);
        }
    }

    class Person
    {
        public string name { get; set; }
        public int age { get; set; }
    }

}