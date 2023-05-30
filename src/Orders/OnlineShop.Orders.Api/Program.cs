using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Orders.Api.Controllers;
using OnlineShop.Orders.Api.EF;
using OnlineShop.Orders.Api.Options;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"),
        builder => builder.MigrationsAssembly(typeof(OrdersDbContext).Assembly.FullName)));

var sinkOptions = new ElasticsearchSinkOptions(
	new Uri(builder.Configuration.GetConnectionString("ElasticSearchConnection")))
{
	AutoRegisterTemplate = true,
	AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
};

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Debug()
	.WriteTo.Elasticsearch(sinkOptions)
	.CreateLogger();

builder.Services.AddOptions<ServiceUrls>()
    .Bind(builder.Configuration.GetSection(ServiceUrls.SectionName));
builder.Services.AddOptions<RabbitMQOptions>()
    .Bind(builder.Configuration.GetSection(RabbitMQOptions.SectionName));
builder.Services.AddHttpClient<OrdersController>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
