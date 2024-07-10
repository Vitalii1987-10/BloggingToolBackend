using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;
using BloggingTool.Context; 
using BloggingTool.DTOs;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase {
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context) {
        _context = context;
    }

    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="userDto">The DTO containing user details.</param>
    /// <returns>A newly created user with associated email accounts.</returns>
    [HttpPost]
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

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user with the specified ID, including associated email accounts.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id) {
        var user = await _context.Users
          .Include(emailAccount => emailAccount.EmailAccounts) 
          .FirstOrDefaultAsync(emailAccount => emailAccount.UserId == id);

        if (user == null) {
            return NotFound();
        }

        var response = new UserDetailDto 
        {
            UserName = user.UserName,
            EmailAccounts = user.EmailAccounts.Select(emailAccount => new EmailAccountDto
            {
                EmailAccountId = emailAccount.EmailAccountId,
                EmailAddress = emailAccount.EmailAddress
            }).ToList()
        };

        return Ok(response);
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A list of all users with their associated email accounts.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _context.Users
          .Include(user => user.EmailAccounts)
          .ToListAsync();

        var response = users.Select(user => new UserDetailDto
        {
            UserName = user.UserName,
            EmailAccounts = user.EmailAccounts.Select(emailAccount => new EmailAccountDto
            {
                EmailAccountId = emailAccount.EmailAccountId,
                EmailAddress = emailAccount.EmailAddress
            }).ToList()
        }).ToList();

        return Ok(response);
    }
}