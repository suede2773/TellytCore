using System.Text.Json.Serialization;

namespace TellytCore.Models
{
  public class AssetModel
  {
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("assetstartdate")]
    public DateTime AssetStartDate { get; set; }
    [JsonPropertyName("lastupdated")]
    public DateTime LastUpdated { get; set; }
    [JsonPropertyName("assetcode")]
    public string AssetCode { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("startdate")]
    public string StartDate { get; set; }
    [JsonPropertyName("starttime")]
    public string StartTime { get; set; }
    [JsonPropertyName("enddate")]
    public string EndDate { get; set; }
    [JsonPropertyName("endtime")]
    public string EndTime { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }
    [JsonPropertyName("size")]
    public decimal Size { get; set; }
    [JsonPropertyName("customerid")]
    public int CustomerId { get; set; }
    [JsonPropertyName("filename")]
    public string FileName { get; set; }
    [JsonPropertyName("url")]
    public string URL { get; set; }
    [JsonPropertyName("bucketname")]
    public string BucketName { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }
  }
}
