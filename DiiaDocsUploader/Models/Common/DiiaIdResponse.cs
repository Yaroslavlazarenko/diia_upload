using System.Text.Json.Serialization;

namespace DiiaDocsUploader.Models.Common;

public class DiiaIdResponse
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;
}