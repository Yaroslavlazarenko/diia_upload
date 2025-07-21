using DiiaDocsUploader.Models.Common;

namespace DiiaDocsUploader.Models.Offer;

public class OfferCreateRequest
{
    public string Name { get; set; } = null!;
    public Scopes Scopes { get; set; } = null!;
}