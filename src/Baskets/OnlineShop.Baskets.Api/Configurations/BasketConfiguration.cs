using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Baskets.Api.Entities;

namespace OnlineShop.Baskets.Api.Configurations;

public class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
	public void Configure(EntityTypeBuilder<Basket> builder)
	{
		builder.HasKey(b => b.Id);
		builder.HasMany(b => b.BasketProducts)
			.WithOne(bp => bp.Basket)
			.HasForeignKey(bp => bp.BasketId);

		var basketProducts = new List<Basket>
		{
			new Basket
			{
				Id = 1,
				UserId = 1,
			},
			new Basket
			{
				Id = 2,
				UserId = 2,
			}
		};

		builder.HasData(basketProducts);
	}
}
