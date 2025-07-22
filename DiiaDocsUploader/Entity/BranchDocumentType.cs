namespace DiiaDocsUploader.Entity;

public class BranchDocumentType
{
    public string BranchId { get; init; } = null!;
    public int DocumentTypeId { get; init; }

    public Branch Branch { get; init; } = null!;
    public DocumentType DocumentType { get; init; } = null!;
}