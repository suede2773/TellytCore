using System.Text.Json.Serialization;

namespace TellytCore.Models
{
  public class SaveAssetModel
  {
    public SaveAssetModel()
    {
      Id = 0;
      OriginalFileName = string.Empty;
      Size = 0;
    }
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("customerid")]
    public int CustomerId { get; set; }
    [JsonPropertyName("filename")]
    public string FileName { get; set; }
    [JsonPropertyName("originalfilename")]
    public string OriginalFileName { get; set; }
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("bucketname")]
    public string BucketName { get; set; }
    [JsonPropertyName("size")]
    public decimal Size { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }
    [JsonPropertyName("jobtype")]
    public string JobType { get; set; }
    [JsonPropertyName("videoguid")]
    public string VideoGuid { get; set; }
    [JsonPropertyName("assetcode")]
    public string AssetCode { get; set; }
    [JsonPropertyName("enddatetimestring")]
    public string EndDateTimeString { get; set; }
    [JsonPropertyName("duration")]
    public decimal Duration { get; set; }
  }
}
