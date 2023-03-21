using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using OnlineShop.Users.Api.EF;
using OnlineShop.Users.Api.Models;

namespace OnlineShop.Users.Api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly UsersDbContext _context;

	public UsersController(UsersDbContext context)
	{
		_context = context;
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