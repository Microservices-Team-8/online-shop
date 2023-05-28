using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Orders.Api.EF;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var options = new DbContextOptionsBuilder<OrdersDbContext>()
    .UseNpgsql(config.GetConnectionString("PostgresConnection"))
    .Options;

var dbContext = new OrdersDbContext(options);
await dbContext.Database.MigrateAsync();
