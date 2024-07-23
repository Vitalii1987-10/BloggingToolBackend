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

    public ICollection<Blog>? Blogs { get; set; }

    public ICollection<ArticleLike>? ArticleLikes { get; set; } = new List<ArticleLike>();
  }
}