using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OnlineShop.EntityHistory.Console;

public class EntityHistoryContextFactory : IDesignTimeDbContextFactory<EntityHistoryDbContext>
{
    public EntityHistoryDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection")!;
        
        var options = new DbContextOptionsBuilder<EntityHistoryDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new EntityHistoryDbContext(options);
    }
}