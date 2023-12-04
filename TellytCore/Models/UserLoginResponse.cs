using System.Text.Json.Serialization;

namespace TellytCore.Models
{
  public class UserLoginResponse
  {
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("displayname")]
    public string DisplayName { get; set; }
  }
}
