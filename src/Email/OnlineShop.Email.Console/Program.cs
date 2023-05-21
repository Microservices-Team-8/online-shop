using System.Text.Json;
using Microsoft.Extensions.Configuration;
using OnlineShop.Email.Console;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var rabbitMqOptions = config.GetValue<RabbitMQOptions>(RabbitMQOptions.SectionName);

var factory = new ConnectionFactory
{
    HostName = config.GetConnectionString("RabbitMQ")
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.ExchangeDeclare(rabbitMqOptions.EmailExchange, "fanout" , false, false, null);
		
channel.QueueDeclare(rabbitMqOptions.EmailSendQueue, false, false, false, null);
channel.QueueBind(rabbitMqOptions.EmailExchange, rabbitMqOptions.EmailSendQueue, "send");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var email = JsonSerializer.Deserialize<Email>(eventArgs.Body.Span);
    SendEmail(email);
};
    
channel.BasicConsume(
    queue: rabbitMqOptions.EmailSendQueue,
    autoAck: true,
    consumer: consumer);

void SendEmail(Email email)
{
    Console.WriteLine($"Email to {email.To} with subject {email.Subject} and body {email.Body} has been sent.");
}