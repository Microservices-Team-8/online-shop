using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Orders.Api.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Order> GetOrders(int userId)
        {
            var items = new List<Order>
            {
                new Order { Id = 1, UserId = 1, OrderDate = new DateTime(), BasketItems = new List<int> { 1, 1, 2 }, TotalPrice = 69.09m },
                new Order { Id = 2, UserId = 1, OrderDate = new DateTime(), BasketItems = new List<int> { 2, 3 }, TotalPrice = 91.58m },
                new Order { Id = 3, UserId = 2, OrderDate = new DateTime(), BasketItems = new List<int> { 1, 2 }, TotalPrice = 55 }
            };

            var orders = items.Where(o => o.UserId == userId);

            return orders;
        }

        [HttpPost]
        public Order GetOrders([FromQuery] int userId, [FromBody] List<int> basketItems)
        {
            var order = new Order
            {
                Id = 4,
                UserId = userId,
                OrderDate = new DateTime(),
                BasketItems = basketItems,
                TotalPrice = 34
            };

            return order;
        }
    }
}
