namespace OnlineShop.Orders.Api
{
	public class Order
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public DateTime OrderDate { get; set; }
		public IEnumerable<int> BasketItems { get; set; }
		public decimal TotalPrice { get; set; }
	}
}
