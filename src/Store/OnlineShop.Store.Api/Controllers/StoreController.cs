using Microsoft.AspNetCore.Mvc;
using OnlineShop.Store.Api.Models;

namespace OnlineShop.Store.Api.Controllers
{
	[Route("api/store")]
	[ApiController]
	public class StoreController : ControllerBase
	{
		private readonly ICollection<Product> _products;

		public StoreController()
		{
			_products = new List<Product>()
			{
				new Product
				{
					Id = 1,
					Code = "j34k9",
					Name = "Samsung S32A600N",
					Description = "PC Monitor",
					Price = 12999.99,
				},
				new Product
				{
					Id = 2,
					Code = "83kl1",
					Name = "Akko 5075b Plus",
					Description = "Keyboard",
					Price = 4700
				},
				new Product
				{
					Id= 3,
					Code = "pw301",
					Name = "Apple Watch Ultra GPS",
					Description = "Hand watch",
					Price = 39999.99
				}
			};
		}

		[HttpGet]
		public Task<IActionResult> GetAllProducts()
		{
			return Task.FromResult<IActionResult>(Ok(_products));
		}

		[HttpGet("{id:int}")]
		public Task<IActionResult> GetProductById(int id)
		{
			var product = _products.FirstOrDefault(x => x.Id == id);

			if (product == null)
			{
				return Task.FromResult<IActionResult>(NotFound());
			}
			return Task.FromResult<IActionResult>(Ok(product));
		}

		[HttpPost]
		public Task<IActionResult> AddUser([FromBody] Product product)
		{
			_products.Add(product);
			return Task.FromResult<IActionResult>(Ok(product));
		}

		[HttpDelete("{id:int}")]
		public Task<IActionResult> DeleteProduct(int id)
		{
			var product = _products.FirstOrDefault(u => u.Id == id);

			if (product == null)
			{
				return Task.FromResult<IActionResult>(NotFound());
			}

			return Task.FromResult<IActionResult>(Ok());
		}
	}
}