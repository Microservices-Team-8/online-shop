using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Baskets.Api.Entities;

namespace OnlineShop.Baskets.Api.Controllers;

[Route("api/baskets/{basketId:int}/products")]
[ApiController]
public class BasketProductsController : ControllerBase
{
	private readonly BasketsDbContext _context;

	public BasketProductsController(BasketsDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<ActionResult<List<BasketProduct>>> GetBasketProducts(int basketId)
	{
		var basket = await _context.Baskets.Include(b => b.BasketProducts)
			.FirstOrDefaultAsync(b => b.Id == basketId);

		if (basket is null)
		{
			return NotFound("Basket not found");
		}

		var basketProducts = basket.BasketProducts;
		
		return Ok(basketProducts);
	}
	
	[HttpGet("{productId:int}")]
	public async Task<ActionResult<BasketProduct>> GetBasketProduct(int basketId, int productId)
	{
		var basket = await _context.Baskets.Include(b => b.BasketProducts)
			.FirstOrDefaultAsync(b => b.Id == basketId);

		if (basket is null)
		{
			return NotFound("Basket not found");
		}

		var product = basket.BasketProducts.FirstOrDefault(p => p.ProductId == productId);

		if (product is null)
		{
			return NotFound("Product not found");
		}

		return Ok(product);
	}

	[HttpPut("{productId:int}")]
	public async Task<IActionResult> UpdateBasketProduct(int basketId, int productId, [FromBody] BasketProduct productToUpdate)
	{
		if (productId != productToUpdate.Id)
			return BadRequest();

		var basket = await _context.Baskets.Include(b => b.BasketProducts)
			.FirstOrDefaultAsync(b => b.Id == basketId);

		if (basket is null)
		{
			return NotFound("Basket not found");
		}

		var product = basket.BasketProducts.FirstOrDefault(p => p.ProductId == productId);

		if (product is null)
		{
			return NotFound("Product not found");
		}

		product.ProductId = productToUpdate.ProductId;
		product.Quantity = productToUpdate.Quantity;
		product.BasketId = productToUpdate.BasketId;
		
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!BasketProductExists(productId))
				return NotFound();
			else
				throw;
		}

		return Ok();
	}

	[HttpPost]
	public async Task<ActionResult<BasketProduct>> AddBasketProduct(int basketId, [FromBody] BasketProduct basketProduct)
	{
		var basket = await _context.Baskets.Include(b => b.BasketProducts)
			.FirstOrDefaultAsync(b => b.Id == basketId);

		if (basket is null)
		{
			return NotFound("Basket not found");
		}
		
		basket.BasketProducts.Add(basketProduct);
		await _context.SaveChangesAsync();

		return Created(nameof(GetBasketProduct), new { basketId = basket.Id, productId = basketProduct.Id });
	}
	
	[HttpPost]
	public async Task<ActionResult<BasketProduct>> AddBasketProducts(int basketId, [FromBody] ICollection<BasketProduct> basketProducts)
	{
		var basket = await _context.Baskets.Include(b => b.BasketProducts)
			.FirstOrDefaultAsync(b => b.Id == basketId);

		if (basket is null)
		{
			return NotFound("Basket not found");
		}
		
		basket.BasketProducts.AddRange(basketProducts);
		await _context.SaveChangesAsync();

		return Created(nameof(GetBasketProducts), new { basketId = basket.Id });
	}

	[HttpDelete("{productId:int}")]
	public async Task<ActionResult> DeleteBasketProduct(int basketId, int productId)
	{
		var basket = await _context.Baskets.Include(b => b.BasketProducts)
			.FirstOrDefaultAsync(b => b.Id == basketId);

		if (basket is null)
		{
			return NotFound("Basket not found");
		}

		var product = basket.BasketProducts.FirstOrDefault(p => p.ProductId == productId);

		if (product is null)
		{
			return NotFound("Product not found");
		}

		basket.BasketProducts.Remove(product);
		await _context.SaveChangesAsync();

		return Ok();
	}

	private bool BasketProductExists(int id)
	{
		return (_context.BasketProducts?.Any(e => e.Id == id)).GetValueOrDefault();
	}
}
