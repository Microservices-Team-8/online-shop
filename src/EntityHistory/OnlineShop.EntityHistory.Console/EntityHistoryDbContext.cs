using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OnlineShop.EntityHistory.Console;

public class EntityHistoryDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public DbSet<EntityChangedMessage> EntityChanges { get; set; }

    public EntityHistoryDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgresConnection"));
    }
}