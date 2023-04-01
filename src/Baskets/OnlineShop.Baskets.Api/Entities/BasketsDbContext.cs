using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Baskets.Api.Entities;

public class BasketsDbContext : DbContext
{
	public BasketsDbContext(DbContextOptions<BasketsDbContext> opts)
		: base(opts)
	{
		Database.EnsureCreated();
	}

	public DbSet<Basket> Baskets { get; set; }
	public DbSet<BasketProduct> BasketProducts { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}
