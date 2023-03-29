using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Store.Api.EF;
using OnlineShop.Store.Api.Models;

namespace OnlineShop.Store.Api.Controllers
{
	[Route("api/store")]
	[ApiController]
	public class StoreController : ControllerBase
	{
		private readonly StoreDbContext _context;

		public StoreController(StoreDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			var products = _context.Products.ToListAsync();
			return Ok(products);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetProductById(int id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

			if (product == null)
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

		[HttpPut("id:int")]
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
			var product = await _context.Products.FirstOrDefaultAsync(u => u.Id == id);

			if (product == null)
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