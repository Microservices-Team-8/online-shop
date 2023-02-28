using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Users.Api.Models;

namespace OnlineShop.Users.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ICollection<User> _users;

    public UsersController()
    {
        _users = new List<User>()
        {
            new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                PhoneNumber = "555-555-5555",
                OrderIds = new List<int> { 1, 2 }
            },
            new User
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "janesmith@example.com",
                PhoneNumber = "555-555-5555",
                OrderIds = new List<int> { 3 }
            }
        };
    }

    [HttpGet]
    public Task<IActionResult> GetUsers()
    {
       return Task.FromResult<IActionResult>(Ok(_users));
    }
    
    [HttpGet("{id}")]
    public Task<IActionResult> GetUserById(int? id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return Task.FromResult<IActionResult>(NotFound());
        }
        
        return Task.FromResult<IActionResult>(Ok(user));
    }

    [HttpPost]
    public Task<IActionResult> AddUser([FromBody] User user)
    {
        _users.Add(user);
        return Task.FromResult<IActionResult>(Ok(user));
    }
    
    [HttpPut("{id}")]
    public Task<IActionResult> UpdateUser(int? id, [FromBody] User userToUpdate)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return Task.FromResult<IActionResult>(NotFound());
        }

        user.FirstName = userToUpdate.FirstName;
        user.LastName = userToUpdate.LastName;
        user.Email = userToUpdate.Email;
        user.PhoneNumber = userToUpdate.PhoneNumber;
        user.OrderIds = userToUpdate.OrderIds;

        return Task.FromResult<IActionResult>(Ok(user));
    }

    [HttpDelete("{id}")]
    public Task<IActionResult> DeleteUser(int? id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return Task.FromResult<IActionResult>(NotFound());
        }
        
        return Task.FromResult<IActionResult>(Ok());
    }
}