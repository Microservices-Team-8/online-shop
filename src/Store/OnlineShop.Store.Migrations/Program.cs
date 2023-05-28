using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Store.Api.EF;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var connectionString = config.GetConnectionString("PostgresConnection");
Console.WriteLine(connectionString);

var options = new DbContextOptionsBuilder<StoreDbContext>()
    .UseNpgsql(connectionString)
    .Options;

var dbContext = new StoreDbContext(options);
await dbContext.Database.MigrateAsync();
