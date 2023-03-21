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
	public async Task<ActionResult<Basket>> AddBasket([FromBody] Basket Basket)
	{
		await _context.Baskets.AddAsync(Basket);
		await _context.SaveChangesAsync();

		return Ok(Basket);
	}

	[HttpPut("{id:int}")]
	public async Task<ActionResult<Basket>> UpdateBasket(int id, [FromBody] Basket BasketToUpdate)
	{
		var Basket = await _context.Baskets.FirstOrDefaultAsync(u => u.Id == id);

		if (Basket is null)
			return NotFound();

		_context.Baskets.Update(Basket);
		await _context.SaveChangesAsync();

		return Ok(Basket);
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
}
