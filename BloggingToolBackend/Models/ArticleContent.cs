using System.ComponentModel.DataAnnotations;

namespace BloggingTool.Models {
  public class ArticleContent {
    [Key]
    public int ContentId { get; set; }

    [Required]
    public required string Content { get; set; } = string.Empty;

    [Required]
    public required int ArticleId { get; set; }

    [Required]
    public required Article Article { get; set; }
  }
}