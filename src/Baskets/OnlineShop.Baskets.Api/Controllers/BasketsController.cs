using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Baskets.Api.Entities;

namespace OnlineShop.Baskets.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketsController : ControllerBase
{
	private readonly BasketsDbContext _context;

	public BasketsController(BasketsDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Basket>>> GetBaskets()
	{
		var Baskets = await _context.Baskets.ToListAsync();

		return Ok(Baskets);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<Basket>> GetBasketById(int id)
	{
		var Basket = await _context.Baskets.FirstOrDefaultAsync(u => u.Id == id);

		if (Basket is null)
			return NotFound();

		return Ok(Basket);
	}

	[HttpPost]
	public async Task<ActionResult<Basket>> AddBasket([FromBody] Basket basket)
	{
		await _context.Baskets.AddAsync(basket);
		await _context.SaveChangesAsync();

		return Ok(basket);
	}

	[HttpPut("{id:int}")]
	public async Task<ActionResult<Basket>> UpdateBasket(int id, [FromBody] Basket basket)
	{
		if (id != basket.Id)
			return BadRequest();

		_context.Entry(basket).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!BasketExists(id))
				return NotFound();
			else
				throw;
		}

		return Ok();
	}

	[HttpDelete("{id:int}")]
	public async Task<ActionResult> DeleteBasket(int id)
	{
		var Basket = await _context.Baskets.FirstOrDefaultAsync(u => u.Id == id);

		if (Basket is null)
			return NotFound();

		_context.Baskets.Remove(Basket);
		await _context.SaveChangesAsync();

		return Ok();
	}

	private bool BasketExists(int id)
	{
		return (_context.Baskets?.Any(e => e.Id == id)).GetValueOrDefault();
	}
}
