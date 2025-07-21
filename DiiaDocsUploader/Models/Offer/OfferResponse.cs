using System.Text.Json.Serialization;
using DiiaDocsUploader.Models.Common;

namespace DiiaDocsUploader.Models.Offer;

public class OfferResponse
{
    [JsonPropertyName("_id")] 
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")] 
    public string Name { get; set; } = null!;

    [JsonPropertyName("scopes")]
    public Scopes Scopes { get; set; } = null!;

    [JsonPropertyName("returnLink")]
    public string ReturnLink { get; set; } = null!;

    [JsonPropertyName("skipPdf")]
    public bool SkipPdf { get; set; }
}