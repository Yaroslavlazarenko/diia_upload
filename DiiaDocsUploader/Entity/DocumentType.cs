namespace DiiaDocsUploader.Entity;

public class DocumentType
{
    public int Id { get; init; }
    public string NameDiia { get; init; } = null!;
    public string NameUa { get; init; } = null!;
    
    public ICollection<BranchDocumentType> BranchDocumentTypes { get; init;} = new HashSet<BranchDocumentType>();
    public ICollection<OfferDocumentType> OfferDocumentTypes { get; init; } = new HashSet<OfferDocumentType>();
}