using System.Text.Json;
using Microsoft.Extensions.Configuration;
using OnlineShop.Email.Console;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var configBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");

var config = configBuilder.Build();

var factory = new ConnectionFactory
{
    HostName = config.GetConnectionString("RabbitMQ")
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "email",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);
    
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var email = JsonSerializer.Deserialize<Email>(eventArgs.Body.Span);
    SendEmail(email!);
};
    
channel.BasicConsume(
    queue: "email",
    autoAck: true,
    consumer: consumer);

void SendEmail(Email email)
{
    Console.WriteLine($"Email to {email.To} with subject {email.Subject} and body {email.Body} has been sent.");
}