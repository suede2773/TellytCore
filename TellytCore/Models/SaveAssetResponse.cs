using System.Text.Json.Serialization;

namespace TellytCore.Models
{
  public class SaveAssetResponse
  {
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }

  }
}
