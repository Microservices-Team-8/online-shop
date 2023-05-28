using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Users.Api.EF;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var options = new DbContextOptionsBuilder<UsersDbContext>()
    .UseNpgsql(config.GetConnectionString("PostgresConnection"))
    .Options;
    
var dbContext = new UsersDbContext(options);
await dbContext.Database.MigrateAsync();