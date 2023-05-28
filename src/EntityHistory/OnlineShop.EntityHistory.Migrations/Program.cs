using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.EntityHistory.Console;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var options = new DbContextOptionsBuilder<EntityHistoryDbContext>()
    .UseNpgsql(config.GetConnectionString("PostgresConnection"))
    .Options;
    
var dbContext = new EntityHistoryDbContext(options);
await dbContext.Database.MigrateAsync();