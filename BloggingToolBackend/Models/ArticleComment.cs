using System.ComponentModel.DataAnnotations;

namespace BloggingTool.Models {
  public class ArticleComment {
    [Key]
    public int CommentId { get; set; }

    [Required]
    public required string CommentatorName { get; set; }

    [Required]
    public required string Comment { get; set; } = string.Empty;

    [Required]
    public required DateTime CreatedTimestamp { get; set; } = DateTime.Now;

    [Required]
    public required int ArticleId { get; set; }

    [Required]
    public required Article Article { get; set; }
  }
}