using BloggingTool.Models; // Add this using directive

namespace BloggingTool.DTOs 
{
  public class ArticleDto 
  {
    public required string ArticleTitle { get; set; }
    public required string ArticleAuthor { get; set; }
    public required string ArticleStatus { get; set; }
    public required DateTime CreatedTimestamp { get; set; } 
    public DateOnly? UpdatedTimestamp { get; set; }
    public DateOnly? PublishedTimestamp { get; set; }
    public int? ArticleViewsCount { get; set; } = 0;
    public required string Content { get; set; }
    public required int BlogId { get; set; }
  }

  public class ArticleUpdateDto
  {
    public required string ArticleTitle { get; set; }
    public required string ArticleAuthor { get; set; }
  }

    public class ArticleOnCreateResponseDto
  {
    public required string ArticleTitle { get; set; }

    public required string ArticleAuthor { get; set; }
    public required string ArticleStatus { get; set; }
    public required DateTime CreatedTimestamp { get; set; } 
    public DateOnly? UpdatedTimestamp { get; set; }
    public DateOnly? PublishedTimestamp { get; set; }
    public int? ArticleViewsCount { get; set; }

    public required string Content { get; set; }

    public required int BlogId { get; set; }
  }

  public class GetAllArticlesResponseDto 
  {
      public int ArticleId { get; set; }
      public required string ArticleTitle { get; set; }
      public required string ArticleAuthor { get; set; }
      public DateTime CreatedTimestamp { get; set; }
  }

  public class GetArticleByIdResponseDto
  {
    public required string ArticleTitle { get; set; }
    public required string ArticleAuthor { get ; set; }
    public required string Content { get; set; }
  }
}