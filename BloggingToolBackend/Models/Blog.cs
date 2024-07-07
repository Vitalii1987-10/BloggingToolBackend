using System.ComponentModel.DataAnnotations;

namespace BloggingTool.Models {
  public class Blog {
    [Key] 
    public int BlogId { get; set; }

    [Required]
    [MaxLength(50)]
    public required string BlogTitle { get; set; }

    [Required]
    [MaxLength(50)]
    public required string BlogAuthor { get; set; }

    [Required]
    [MaxLength(50)]
    public required string BlogCategory { get; set; }

    [Required]
    public required int EmailAccountId { get; set; }

    [Required]
    public required EmailAccount EmailAccount { get; set; }
    public ICollection<Article>? Articles { get; set; } = new List<Article>();
  } 
}