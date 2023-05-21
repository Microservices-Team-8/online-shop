using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.EntityHistory.Console;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var options = new DbContextOptionsBuilder<EntityHistoryDbContext>()
    .UseNpgsql(config.GetConnectionString("PostgresConnection"))
    .Options;

var dbContext = new EntityHistoryDbContext(options);
var rabbitMqOptions = config.GetValue<RabbitMQOptions>(RabbitMQOptions.SectionName);

var factory = new ConnectionFactory
{
    HostName = rabbitMqOptions.Host
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();
	
channel.ExchangeDeclare(rabbitMqOptions.EntityExchange, "direct" , false, false, null);
		
channel.QueueDeclare(rabbitMqOptions.EntityCreateQueue, false, false, false, null);
channel.QueueBind(rabbitMqOptions.EntityExchange, rabbitMqOptions.EntityCreateQueue, "create");

channel.QueueDeclare(rabbitMqOptions.EntityUpdateQueue, false, false, false, null);
channel.QueueBind(rabbitMqOptions.EntityExchange, rabbitMqOptions.EntityUpdateQueue, "update");
		
channel.QueueDeclare(rabbitMqOptions.EntityDeleteQueue, false, false, false, null);
channel.QueueBind(rabbitMqOptions.EntityExchange, rabbitMqOptions.EntityDeleteQueue, "delete");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (model, eventArgs) =>
{
    var message = JsonSerializer.Deserialize<EntityChangedMessage>(eventArgs.Body.Span);
    await dbContext.EntityChanges.AddAsync(message);
    await dbContext.SaveChangesAsync();
};
    
channel.BasicConsume(
    queue: rabbitMqOptions.EntityCreateQueue,
    autoAck: true,
    consumer: consumer);
        
channel.BasicConsume(
    queue: rabbitMqOptions.EntityUpdateQueue,
    autoAck: true,
    consumer: consumer);
        
channel.BasicConsume(
    queue: rabbitMqOptions.EntityDeleteQueue,
    autoAck: true,
    consumer: consumer);
    