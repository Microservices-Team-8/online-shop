using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.ContentModel;
using OnlineShop.Users.Api.EF;
using OnlineShop.Users.Api.Models;
using OnlineShop.Users.Api.Options;

namespace OnlineShop.Users.Api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly UsersDbContext _context;
	private readonly HttpClient _httpClient;
	private readonly ServiceUrls _serviceUrls;

	public UsersController(UsersDbContext context, HttpClient httpClient, IOptions<ServiceUrls> serviceUrls)
	{
		_context = context;
		_httpClient = httpClient;
		_serviceUrls = serviceUrls.Value;
	}

	[HttpGet]
	public async Task<IActionResult> GetUsers()
	{
		var users = await _context.Users.ToListAsync();

		return Ok(users);
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetUserById(int id)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

		if (user is null)
			return NotFound();

		return Ok(user);
	}

	[HttpPost]
	public async Task<IActionResult> AddUser([FromBody] User user)
	{
		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();

		var response = await _httpClient.PostAsync(_serviceUrls.BasketsServiceUrl,
			JsonContent.Create($"\"userId\": {user.Id}"));

		if (!response.IsSuccessStatusCode)
		{
			return BadRequest("Can't create basket");
		}

		return Ok(user);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> UpdateUser(int id, [FromBody] User userToUpdate)
	{
		if (id != userToUpdate.Id)
			return BadRequest();

		_context.Entry(userToUpdate).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!UserExists(id))
				return NotFound();
			else
				throw;
		}

		return Ok(userToUpdate);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteUser(int id)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

		if (user is null)
			return NotFound();

		_context.Users.Remove(user);
		await _context.SaveChangesAsync();

		return Ok();
	}

	private bool UserExists(int id)
	{
		return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
	}
}