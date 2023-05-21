using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Baskets.Api.Entities;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var options = new DbContextOptionsBuilder<BasketsDbContext>()
    .UseNpgsql(config.GetConnectionString("PostgresConnection"))
    .Options;

var dbContext = new BasketsDbContext(options);
await dbContext.Database.MigrateAsync();
