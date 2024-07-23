using System.ComponentModel.DataAnnotations;

namespace BloggingTool.Models {
  public class User {

    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public required string UserName { get; set; }
    
    [Required]
    public required ICollection<EmailAccount> EmailAccounts { get; set; }
  }
}