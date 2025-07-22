namespace DiiaDocsUploader.Entity;

public class Offer
{
    public string Id { get; init; } = null!;
    public string BranchId { get; init; } = null!;
    public string Name { get; init; } = null!;
    
    public Branch Branch { get; init; } = null!;
    public ICollection<OfferDocumentType> OfferDocumentTypes { get; init; } = new HashSet<OfferDocumentType>();
}