using System.Text.Json.Serialization; // Glöm inte denna using!
using BookService.Domain.Entities;

namespace BookService.Api.DTOs
{
    public class LoginRequestDTO
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}