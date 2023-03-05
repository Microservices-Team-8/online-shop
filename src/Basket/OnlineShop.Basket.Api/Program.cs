using OnlineShop.Domain;

namespace OnlineShop.Basket.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

        app.MapGet("api/basket", (HttpContext httpContext) =>
        {
            var items = new List<BasketItem>
            {
                new BasketItem { Id = 1, Title = "Mivina", ItemCount = 2, Price = 14.05m },
                new BasketItem { Id = 2, Title = "Redbull", ItemCount = 5, Price = 40.99m },
                new BasketItem { Id = 3, Title = "Monster", ItemCount = 100, Price = 50.59m }
            };

            return items;
        });

        app.Run();
    }
}
