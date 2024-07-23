using System.Text.Json.Serialization;

namespace BloggingTool.DTOs
{
  public class ArticleCommentDto 
  {
    public required string CommentatorName { get; set; }
    public required string Comment { get; set; }
    public required DateTime CreatedTimestamp { get; set; }
  }

    public class ArticleCommentResponseOnCreateDto 
  {
    public required string CommentatorName { get; set; }
    public required string Comment { get; set; } = string.Empty;
    public required DateTime CreatedTimestamp { get; set; } = DateTime.Now;
  }
}