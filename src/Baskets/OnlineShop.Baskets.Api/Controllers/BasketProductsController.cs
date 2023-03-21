using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Baskets.Api.Entities;

namespace OnlineShop.Baskets.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketProductsController : ControllerBase
{
	private readonly BasketsDbContext _context;

	public BasketProductsController(BasketsDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<BasketProduct>>> GetBasketProducts()
	{
		if (_context.BasketProducts == null)
			return NotFound();

		return await _context.BasketProducts.ToListAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<BasketProduct>> GetBasketProduct(int id)
	{
		if (_context.BasketProducts == null)
			return NotFound();

		var basketProduct = await _context.BasketProducts.FindAsync(id);

		if (basketProduct == null)
			return NotFound();

		return basketProduct;
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateBasketProduct(int id, [FromBody] BasketProduct basketProduct)
	{
		if (id != basketProduct.Id)
			return BadRequest();

		_context.Entry(basketProduct).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!BasketProductExists(id))
				return NotFound();
			else
				throw;
		}

		return Ok();
	}

	[HttpPost]
	public async Task<ActionResult<BasketProduct>> AddBasketProduct([FromBody] BasketProduct basketProduct)
	{
		if (_context.BasketProducts == null)
			return Problem("Entity set 'BasketsDbContext.BasketProducts'  is null.");

		_context.BasketProducts.Add(basketProduct);
		await _context.SaveChangesAsync();

		return Ok(basketProduct);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteBasketProduct(int id)
	{
		if (_context.BasketProducts == null)
			return NotFound();

		var basketProduct = await _context.BasketProducts.FindAsync(id);

		if (basketProduct == null)
			return NotFound();

		_context.BasketProducts.Remove(basketProduct);
		await _context.SaveChangesAsync();

		return Ok();
	}

	private bool BasketProductExists(int id)
	{
		return (_context.BasketProducts?.Any(e => e.Id == id)).GetValueOrDefault();
	}
}
