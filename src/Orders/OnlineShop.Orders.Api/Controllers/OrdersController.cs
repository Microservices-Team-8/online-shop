using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Orders.Api.EF;
using OnlineShop.Orders.Api.Models;

namespace OnlineShop.Orders.Api.Controllers
{
	[Route("api/orders")]
	[ApiController]
	public class OrdersController : ControllerBase
	{

        private readonly OrdersDbContext _context;

		public OrdersController(OrdersDbContext context)
		{
			_context = context;
			var items = new List<Order>
			{
				new Order {
					UserId = 1,
					OrderDate = DateTime.UtcNow,
					Items = new List<OrderItem> {
						new OrderItem {
							Price = 12999.99m,
							ProductId = 1,
							Amount = 1} 
						},
					TotalPrice = 12999.99m
				},
				new Order {
					UserId = 1,
					OrderDate = DateTime.UtcNow,
					Items = new List<OrderItem> {
						new OrderItem
						{
							Price = 4700m,
							ProductId = 2,
							Amount = 2
						}
					},
					TotalPrice = 9400m },
				new Order {
					UserId = 2,
					OrderDate = DateTime.UtcNow,
					Items = new List<OrderItem> {
						new OrderItem
						{
							Price = 39999.99m,
							ProductId = 13,
							Amount = 1
						}
					},
					TotalPrice = 39999.99m },
			};
			_context.AddRange(items);
			_context.SaveChanges();
        }

        [HttpGet]
		public async Task<IEnumerable<Order>> GetOrders()
		{
			return await _context.Orders
                .Include(o => o.Items).ToListAsync();
		}

        [HttpGet("{id:int}")]
        public async Task<Order> GetOrder(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
				.FirstOrDefaultAsync(o => o.Id == id) ?? throw new ArgumentException();
        }

        [HttpPost]
		public async Task<Order> CreateOrder([FromBody] Order order)
		{
            var res = await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();

			return res.Entity;
		}

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _context.Orders
				.Include(o => o.Items)
				.FirstOrDefaultAsync(o => o.Id == orderId) ?? throw new ArgumentException();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
