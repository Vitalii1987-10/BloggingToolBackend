using System.Text.Json.Serialization;

namespace BloggingTool.DTOs
{
    public class NewUserEmailAccountDto
    {
        public required string EmailAddress { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }

    public class ExistingUserEmailAccountDto
    {
        public required string EmailAddress { get; set; }
        public int UserId { get; set; }
    }

    public class EmailAccountResponse
    {
        public required string EmailAddress { get; set; }
    }
}
