using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Baskets.Api.Entities;

namespace OnlineShop.Baskets.Api.Configurations;

public class BasketProductConfiguration : IEntityTypeConfiguration<BasketProduct>
{
	public void Configure(EntityTypeBuilder<BasketProduct> builder)
	{
		builder.HasKey(b => b.Id);
		builder.HasOne(bp => bp.Basket)
			.WithMany(b => b.BasketProducts)
			.HasForeignKey(bp => bp.BasketId);

		var basketProducts = new List<BasketProduct>
		{
			new BasketProduct
			{
				Id = 1,
				ProductId = 1,
				Quantity = 5,
				BasketId = 1,
			},
			new BasketProduct
			{
				Id = 2,
				ProductId = 1,
				Quantity = 10,
				BasketId = 2,
			}
		};

		builder.HasData(basketProducts);
	}
}
