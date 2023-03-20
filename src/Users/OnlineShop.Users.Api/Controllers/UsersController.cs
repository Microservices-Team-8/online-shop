using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return NotFound();

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok(user);
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
}