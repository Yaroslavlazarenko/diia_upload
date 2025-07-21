using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiiaDocsUploader.Models.Common;

public class DiiaMetadata
{
    [JsonPropertyName("requestId")]
    public string? RequestId { get; set; }

    [JsonPropertyName("barcode")]
    public string? Barcode { get; set; }

    [JsonPropertyName("documentTypes")]
    public List<string>? DocumentTypes { get; set; }
    
    [JsonPropertyName("data")]
    public JsonElement Data { get; set; }
}