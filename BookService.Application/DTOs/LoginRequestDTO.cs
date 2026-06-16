using System.Text.Json.Serialization;

namespace BookService.Application.DTOs
{
    public class LoginRequestDTO
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}