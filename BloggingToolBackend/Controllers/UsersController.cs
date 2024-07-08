using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;
using BloggingTool.Context; 
using BloggingTool.DTOs;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase {
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context) {
        _context = context;
    }

    // Adds a new user.
    [HttpPost("add-user")]
    public async Task<IActionResult> AddUser(UserDto userDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        string lastCreatedEmailAddress = "";

        var user = new User
        {
            UserName = userDto.UserName,
            EmailAccounts = userDto.EmailAccounts.Select(emailAccount => 
            {
                var newEmailAccount = new EmailAccount
                {
                    EmailAddress = emailAccount.EmailAddress,
                    UserId = userDto.UserId 
                };
                lastCreatedEmailAddress = newEmailAccount.EmailAddress;
                return newEmailAccount;
            }).ToList()
            
        };

        var response = new UserResponseDto
        {
            UserName = user.UserName,
            EmailAccount = lastCreatedEmailAddress
        };

        if (user.EmailAccounts == null || !user.EmailAccounts.Any())
        {
            ModelState.AddModelError("EmailAccounts", "At least one EmailAccount is required.");
            return BadRequest(ModelState);
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, response);
    }

    // Gets a user by ID.
    [HttpGet("get-user/{id}")]
    public async Task<ActionResult<User>> GetUserById(int id) {
        var user = await _context.Users
          .Include(emailAccount => emailAccount.EmailAccounts) 
          .FirstOrDefaultAsync(emailAccount => emailAccount.UserId == id);

        if (user == null) {
            return NotFound();
        }

        return user;
    }
}