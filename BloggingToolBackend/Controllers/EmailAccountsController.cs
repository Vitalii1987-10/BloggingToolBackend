using Microsoft.AspNetCore.Mvc;
using BloggingTool.Models;
using BloggingTool.Context;
using BloggingTool.DTOs; 

[ApiController]
// [Route("[controller]")]
[Route("user/{emailAccountId}")]
public class EmailAccountsController : ControllerBase {
  private readonly AppDbContext _context;

  public EmailAccountsController(AppDbContext context) {
    _context = context;
  }

  /// <summary>
  /// Adds a new email account for a specific user.
  /// </summary>
  /// <param name="emailAccountId">The ID of the user to add the email account to.</param>
  /// <param name="emailAccountDto">The email account details.</param>
  /// <returns>The created email account.</returns>
  [HttpPost]
  public async Task<IActionResult> AddEmailAccount(int emailAccountId, ExistingUserEmailAccountDto emailAccountDto) {

    if(!ModelState.IsValid) {
      return BadRequest(ModelState);
    }

    var emailAccount = new EmailAccount
    {
      EmailAddress = emailAccountDto.EmailAddress,
      UserId = emailAccountId
    };

    var response = new EmailAccountResponseDto
    {
      EmailAddress = emailAccount.EmailAddress
    };

    _context.EmailAccounts.Add(emailAccount);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetEmailAccountById), new { id = emailAccount.EmailAccountId}, response);
  }

  /// <summary>
  /// Retrieves an email account by its ID.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account to retrieve.</param>
  /// <returns>The email account details.</returns>
  [HttpGet]
  public async Task<ActionResult<EmailAccount>> GetEmailAccountById(int emailAccountId) {
    var emailAccount = await _context.EmailAccounts.FindAsync(emailAccountId);

    if(emailAccount == null) {
      return NotFound();
    }

    var response = new EmailAccountResponseDto
    {
      EmailAddress = emailAccount.EmailAddress,
    };

    return Ok(response);
  }
}