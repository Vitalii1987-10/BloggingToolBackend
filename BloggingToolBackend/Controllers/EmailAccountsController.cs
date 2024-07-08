using Microsoft.AspNetCore.Mvc;
using BloggingTool.Models;
using BloggingTool.Context;
using BloggingTool.DTOs; 

[ApiController]
[Route("[controller]")]
public class EmailAccountsController : ControllerBase {
  private readonly AppDbContext _context;

  public EmailAccountsController(AppDbContext context) {
    _context = context;
  }

  //Add emailAccount
  [HttpPost("add-emailAccount")]
  public async Task<IActionResult> AddEmailAccount(ExistingUserEmailAccountDto emailAccountDto) {

    if(!ModelState.IsValid) {
      return BadRequest(ModelState);
    }

    var emailAccount = new EmailAccount
    {
      EmailAddress = emailAccountDto.EmailAddress,
      UserId = emailAccountDto.UserId
    };

    var response = new EmailAccountResponse
    {
      EmailAddress = emailAccount.EmailAddress
    };

    _context.EmailAccounts.Add(emailAccount);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetEmailAccountById), new { id = emailAccount.EmailAccountId}, response);
  }

  [HttpGet("get-emailAccount/{id}")]
  public async Task<ActionResult<EmailAccount>> GetEmailAccountById(int id) {
    var emailAccount = await _context.EmailAccounts.FindAsync(id);

    if(emailAccount == null) {
      return NotFound();
    }

    return emailAccount;
  }
}