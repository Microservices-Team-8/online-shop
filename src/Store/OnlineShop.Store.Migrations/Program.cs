using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Store.Api.EF;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var options = new DbContextOptionsBuilder<StoreDbContext>()
    .UseNpgsql(config.GetConnectionString("PostgresConnection"))
    .Options;

var dbContext = new StoreDbContext(options);
await dbContext.Database.MigrateAsync();
