using System.Text.Json.Serialization;

namespace TellytCore.Models
{
  public class UserRequest
  {
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
  }
}
