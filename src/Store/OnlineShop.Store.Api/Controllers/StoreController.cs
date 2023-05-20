using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Store.Api.EF;
using OnlineShop.Store.Api.Models;
using Microsoft.Extensions.Options;
using OnlineShop.Store.Api.Options;
using Newtonsoft.Json;
using OnlineShop.Store.Api.BackgroundServices;

namespace OnlineShop.Store.Api.Controllers
{
	[Route("api/store")]
	[ApiController]
	public class StoreController : ControllerBase
	{
		private readonly StoreDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ServiceUrls _serviceUrls;
        private readonly AvailabilityService _availabilityService;
        
        public StoreController(StoreDbContext context, HttpClient httpClient, IOptions<ServiceUrls> serviceUrls,
	        AvailabilityService availabilityService)
		{
			_context = context;
            _httpClient = httpClient;
            _availabilityService = availabilityService;
            _serviceUrls = serviceUrls.Value;
        }

		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			if (_availabilityService.IsBroken())
			{
				return StatusCode(503);
			}

			var products = await _context.Products.ToListAsync();
			
			return Ok(products);
		}
		
		[HttpGet("break")]
		public async Task<IActionResult> Break()
		{
			_availabilityService.Break();

			return Ok();
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetProductById(int id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

			if (product is null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		[HttpPost]
		public async Task<IActionResult> AddProduct([FromBody] Product product)
		{
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();

            return Ok(product);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
		{
			if (id != product.Id)
			{
				return BadRequest();
			}

			_context.Entry(product).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
			return Ok(product);
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
            var response = await _httpClient.GetAsync(_serviceUrls.BasketsService + $"/{id}");

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            // Parse the response content to check if the product exists in any basket
            var content = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(content);
            bool existsInBasket = responseObject.existsInBasket;

            if (existsInBasket)
            {
                // If the product exists in any basket, return a BadRequest
                return BadRequest("Product exists in a basket and cannot be deleted.");
            }     

            var product = await _context.Products.FirstOrDefaultAsync(u => u.Id == id);

			if (product is null)
			{
				return NotFound();
			}
			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			return Ok();
		}

		private bool ProductExists(int id)
		{
			return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}