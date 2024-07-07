using System.ComponentModel.DataAnnotations;

namespace BloggingTool.Models {
  public class Article {
    [Key]
    public int ArticleId { get; set; }

    [Required]
    [MaxLength(50)]
    public required string ArticleTitle { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public required string ArticleStatus { get; set; }

    [Required]
    public required DateTime CreatedTimestamp { get; set; } = DateTime.Now;
    public DateTime? UpdatedTimestamp { get; set; } = DateTime.Now;
    public DateTime? PublishedTimestamp { get; set; } = DateTime.Now;
    public int? ArticleViewsCount { get; set; } = 0;

    [Required]
    public required int BlogId { get; set; }

    [Required]
    public required Blog Blog { get; set; }
    
    [Required]
    public required ArticleContent ArticleContent { get; set; }

    public ICollection<ArticleComment>? ArticleComments { get; set; } = new List<ArticleComment>();

    public ICollection<ArticleLike>? ArticleLikes { get; set; } = new List<ArticleLike>();

  }
}