using System.Text.Json.Serialization;

namespace DiiaDocsUploader.Models.DeepLink;

public class InternalDiiaDeepLinkRequest
{
    [JsonPropertyName("offerId")]
    public string OfferId { get; }

    [JsonPropertyName("requestId")]
    public string RequestId { get; }
    
    [JsonPropertyName("returnLink")]
    public string? ReturnLink { get; }

    [JsonPropertyName("useDiiaId")]
    public bool UseDiiaId { get; }
    
    public InternalDiiaDeepLinkRequest(DeepLinkCreateRequest request)
    {
        OfferId = request.OfferId;
        UseDiiaId = request.UseDiiaId;
        ReturnLink = request.ReturnLink;
        RequestId = Guid.NewGuid().ToString();
    }
}