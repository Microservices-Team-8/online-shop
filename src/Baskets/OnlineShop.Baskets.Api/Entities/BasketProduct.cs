using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineShop.Baskets.Api.Entities;

public class BasketProduct
{
	public int Id { get; set; }

	public int ProductId { get; set; }

	public int Quantity { get; set; }

	public int BasketId { get; set; }

	[JsonIgnore]
	[ForeignKey("BasketId")]
	public Basket Basket { get; set; }
}
