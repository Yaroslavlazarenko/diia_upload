using System.Text.Json.Serialization;

namespace DiiaDocsUploader.Models.DeepLink;

public class DeepLinkResponse
{
    [JsonPropertyName("deeplink")]
    public string DeepLink { get; set; } = null!;
}