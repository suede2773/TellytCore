using System.Text.Json.Serialization;

namespace TellytCore.Models
{
  public class SaveAssetLogModel
  {
    [JsonPropertyName("assetid")]
    public int AssetId { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
  }
}
