using Microsoft.EntityFrameworkCore;

namespace OnlineShop.EntityHistory.Console;

public class EntityHistoryDbContext : DbContext
{
    
    public DbSet<EntityChange> EntityChanges { get; set; }
}