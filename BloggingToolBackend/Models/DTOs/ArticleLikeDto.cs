using System.Text.Json.Serialization;

namespace BloggingTool.DTOs
{
  public class ArticleLikeDto 
  {
    public bool IsLiked { get; set; }
  }

  public class ArticleLikeGetLikeByIdResponse
  {
    public bool IsLiked { get; set; } = true;
  }
}