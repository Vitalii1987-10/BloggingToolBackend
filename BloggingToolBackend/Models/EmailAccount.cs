using System.ComponentModel.DataAnnotations;

namespace BloggingTool.Models {
  public class EmailAccount {
    [Key]
    public int EmailAccountId { get; set; }

    [Required]
    [MaxLength(100)]
    public required string EmailAddress { get; set; }

    [Required]
    public required int UserId { get; set; }

    [Required]
    public required User User { get; set; }

    public required ICollection<Blog> Blogs { get; set; }

    public required ICollection<ArticleLike> ArticleLikes { get; set; }
  }
}