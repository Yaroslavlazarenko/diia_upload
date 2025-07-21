using System.Text.Json.Serialization;

namespace DiiaDocsUploader.Models.Offer;

public class OfferListResponse
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("offers")] public List<OfferResponse> Offers { get; set; } = null!;
}