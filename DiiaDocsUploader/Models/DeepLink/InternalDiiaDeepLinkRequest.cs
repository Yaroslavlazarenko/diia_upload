using System.Text.Json.Serialization;

namespace DiiaDocsUploader.Models.DeepLink;

public class InternalDiiaDeepLinkRequest
{
    [JsonPropertyName("offerId")]
    public string OfferId { get; set; } = null!;

    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = null!;

    [JsonPropertyName("useDiiaId")]
    public bool UseDiiaId { get; set; } = true;
    
    public InternalDiiaDeepLinkRequest(DeepLinkCreateRequest externalRequest)
    {
        OfferId = externalRequest.OfferId;
        UseDiiaId = externalRequest.UseDiiaId;
        RequestId = Guid.NewGuid().ToString();
    }
}