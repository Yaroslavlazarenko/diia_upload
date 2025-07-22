namespace DiiaDocsUploader.Entity;

public class OfferDocumentType
{
    public string OfferId { get; init; } = null!;
    public int DocumentTypeId { get; init; }

    public Offer Offer { get; init; } = null!;
    public DocumentType DocumentType { get; init; } = null!;
}