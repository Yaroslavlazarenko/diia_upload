using System.Text.Json.Serialization;

namespace DiiaDocsUploader.Models.Common;

public class Scopes
{
    [JsonPropertyName("sharing")]
    public List<string> Sharing { get; set; } = null!;
}