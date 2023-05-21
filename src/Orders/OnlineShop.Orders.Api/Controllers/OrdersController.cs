using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineShop.Orders.Api.EF;
using OnlineShop.Orders.Api.Models;
using OnlineShop.Orders.Api.Options;
using RabbitMQ.Client;

namespace OnlineShop.Orders.Api.Controllers
{
	[Route("api/orders")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
        private readonly OrdersDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ServiceUrls _serviceUrls; 
        private readonly RabbitMQOptions _rabbitMqOptions;
        private readonly IModel _channel;

        public OrdersController(OrdersDbContext context, HttpClient httpClient,
            IOptions<ServiceUrls> serviceUrls, IOptions<RabbitMQOptions> rabbitMqOptions)
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
            _httpClient = httpClient;
            _serviceUrls = serviceUrls.Value; 
            _rabbitMqOptions = rabbitMqOptions.Value;


            var connectionFactory = new ConnectionFactory { HostName = _rabbitMqOptions.Host };
            using var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(_rabbitMqOptions.EmailExchange, "fanout", false, false, null);

            channel.QueueDeclare(_rabbitMqOptions.EmailSendQueue, false, false, false, null);
            channel.QueueBind(_rabbitMqOptions.EmailExchange, _rabbitMqOptions.EmailSendQueue, "send");

            channel.ExchangeDeclare(_rabbitMqOptions.EntityExchange, "direct", false, false, null);

            channel.QueueDeclare(_rabbitMqOptions.EntityCreateQueue, false, false, false, null);
            channel.QueueBind(_rabbitMqOptions.EntityExchange, _rabbitMqOptions.EntityCreateQueue, "create");

            channel.QueueDeclare(_rabbitMqOptions.EntityUpdateQueue, false, false, false, null);
            channel.QueueBind(_rabbitMqOptions.EntityExchange, _rabbitMqOptions.EntityUpdateQueue, "update");

            channel.QueueDeclare(_rabbitMqOptions.EntityDeleteQueue, false, false, false, null);
            channel.QueueBind(_rabbitMqOptions.EntityExchange, _rabbitMqOptions.EntityDeleteQueue, "delete");

            _channel = channel;
        }

        [HttpGet]
		public async Task<IActionResult> GetOrders()
		{
			return Ok(await _context.Orders
                .Include(o => o.Items).ToListAsync());
		}

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            return Ok(await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id) ?? throw new ArgumentException());
        }

        [HttpPost]
		public async Task<IActionResult> CreateOrder([FromBody] Order order)
		{
            var response = await _httpClient.GetAsync(_serviceUrls.UsersService + $"/{order.UserId}");

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            var res = await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();

            return Ok(res.Entity);
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
