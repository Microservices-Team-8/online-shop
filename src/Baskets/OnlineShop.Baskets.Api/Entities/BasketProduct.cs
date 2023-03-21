using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Baskets.Api.Entities;

public class BasketProduct
{
	public int Id { get; set; }

	public int ProductId { get; set; }

	public int Quantity { get; set; }

	public int BasketId { get; set; }

	[ForeignKey("BasketId")]
	public Basket Basket { get; set; }
}
