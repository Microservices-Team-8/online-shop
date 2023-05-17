using Microsoft.EntityFrameworkCore;
using OnlineShop.Store.Api.Controllers;
using OnlineShop.Store.Api.EF;
using OnlineShop.Store.Api.Options;

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

            builder.Services.AddOptions<ServiceUrls>()
					.Bind(builder.Configuration.GetSection(ServiceUrls.SectionName));
            builder.Services.AddHttpClient<StoreController>();


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