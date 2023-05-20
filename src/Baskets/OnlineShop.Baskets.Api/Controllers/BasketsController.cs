using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Baskets.Api.Entities;

namespace OnlineShop.Baskets.Api.Controllers;

[Route("api/baskets")]
[ApiController]
public class BasketsController : ControllerBase
{
	private readonly BasketsDbContext _context;
	private readonly HttpClient _httpClient;
	private readonly IConfiguration _configuration;

	public BasketsController(BasketsDbContext context, HttpClient httpClient, IConfiguration configuration)
	{
		_context = context;
		_httpClient = httpClient;
		_configuration = configuration;
	}

	[HttpGet]
	public async Task<ActionResult<Basket>> GetBasketByUserId([FromQuery] int userId)
	{
		if (!await EnsureUserExists(userId))
			return BadRequest();

		var basket = await _context.Baskets
			.FirstOrDefaultAsync(u => u.UserId == userId);

		if (basket is null)
			return NotFound();

		return Ok(basket);
	}

	[HttpPost]
	public async Task<ActionResult<Basket>> CreateBasket([FromBody] Basket basket)
	{
		Console.WriteLine("Test");
		if (Random.Shared.NextDouble() < 0.4)
		{
			Console.WriteLine("Waiting");
			await Task.Delay(TimeSpan.FromSeconds(12));
			return StatusCode(500);
		}

		await _context.Baskets.AddAsync(basket);
		await _context.SaveChangesAsync();

		return Ok(basket);
	}

	[HttpPost("{id:int}/addProduct")]
	public async Task<ActionResult<Basket>> AddProductsToBasket(int id, [FromBody] int[] productIds)
	{
		var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.Id == id);

		if (basket is null ||
			!await EnsureProductIdsExists(productIds))
			return BadRequest();

		basket.BasketProductIds?.AddRange(productIds);

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
	public async Task<IActionResult> DeleteBasket(int id)
	{
		var basket = await _context.Baskets.FirstOrDefaultAsync(u => u.Id == id);

		if (basket is null)
			return NotFound();

		_context.Baskets.Remove(basket);
		await _context.SaveChangesAsync();

		return Ok();
	}

	private async Task<bool> EnsureUserExists(int userId)
	{
		string uri = _configuration.GetValue<string>("ServiceUrls:UsersService");

		var response = await _httpClient.GetAsync(uri + $"/{userId}");

		if (!response.IsSuccessStatusCode)
			return false;

		return true;
	}

	private async Task<bool> EnsureProductIdsExists(int[] productIds)
	{
		string uri = _configuration.GetValue<string>("ServiceUrls:StoreService");

		foreach (int id in productIds)
		{
			var response = await _httpClient.GetAsync(uri + $"/{id}");

			if (!response.IsSuccessStatusCode)
				return false;
		}

		return true;
	}

	private bool BasketExists(int id)
	{
		return (_context.Baskets?.Any(e => e.Id == id))
			.GetValueOrDefault();
	}
}
