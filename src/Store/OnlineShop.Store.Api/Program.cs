using Microsoft.EntityFrameworkCore;
using OnlineShop.Store.Api.BackgroundServices;
using OnlineShop.Store.Api.Controllers;
using OnlineShop.Store.Api.EF;
using OnlineShop.Store.Api.Options;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;

namespace OnlineShop.Store.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var configuration = builder.Configuration;

			builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreDbContext>(options =>
				options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));

			var sinkOptions = new ElasticsearchSinkOptions(
				new Uri(configuration.GetConnectionString("ElasticSearchConnection")))
			{
				AutoRegisterTemplate = true,
				AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
			};

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Elasticsearch(sinkOptions)
				.CreateLogger();

			builder.Services.AddOptions<ServiceUrls>()
				.Bind(configuration.GetSection(ServiceUrls.SectionName));
			builder.Services.AddOptions<RabbitMQOptions>()
				.Bind(configuration.GetSection(RabbitMQOptions.SectionName));

			builder.Services.AddHttpClient<StoreController>();
			builder.Services.AddSingleton<AvailabilityService>();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}