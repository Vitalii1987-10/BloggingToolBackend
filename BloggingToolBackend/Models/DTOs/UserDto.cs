using System.Text.Json.Serialization;

namespace BloggingTool.DTOs
{
  public class UserDto 
  {
    [JsonIgnore]
    public int UserId {get; set; }

    public required string UserName { get; set; }

    public required ICollection<NewUserEmailAccountDto> EmailAccounts { get; set; }
  }

  public class UserResponseDto 
  {
    public required string UserName { get; set; }

    public required string EmailAccount { get; set; }
  }
}