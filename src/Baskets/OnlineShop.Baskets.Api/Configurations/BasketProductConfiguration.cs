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
				ProductId = 2,
				Quantity = 50,
				BasketId = 1,
			},
			new BasketProduct
			{
				Id = 3,
				ProductId = 3,
				Quantity = 10,
				BasketId = 1,
			},
			new BasketProduct
			{
				Id = 4,
				ProductId = 4,
				Quantity = 100,
				BasketId = 2,
			},
			new BasketProduct
			{
				Id = 5,
				ProductId = 5,
				Quantity = 20,
				BasketId = 2,
			},
			new BasketProduct
			{
				Id = 6,
				ProductId = 6,
				Quantity = 70,
				BasketId = 2,
			}
		};

		builder.HasData(basketProducts);
	}
}
