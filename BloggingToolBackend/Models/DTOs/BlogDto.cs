using System.Text.Json.Serialization;

namespace BloggingTool.DTOs{
  public class BlogDto
  {
    [JsonIgnore]
    public int BlogId { get; set; }
    public required string BlogTitle { get; set; }
    public required string BlogAuthor { get; set; }
    public required string BlogCategory { get; set; }
    public required int EmailAccountid { get; set; } 
  }

  public class BlogResponseDto 
  {
    [JsonIgnore]
    public int BlogId { get; set; }
    public required string BlogTitle { get; set; }
    public required string BlogAuthor { get; set; }
    public required string BlogCategory { get; set; }
    [JsonIgnore]
    public int EmailAccountId { get; set; }
  }

  public class BlogUpdateDto
  {
    public required string BlogTitle { get; set; }
    public required string BlogAuthor { get; set; }
    public required string BlogCategory { get; set; }
  }
}