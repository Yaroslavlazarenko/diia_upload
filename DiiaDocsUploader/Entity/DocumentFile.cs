namespace DiiaDocsUploader.Entity;

public class DocumentFile
{
    public int Id { get; init; }
    public Guid DeepLinkId { get; init; }
    public string DocumentFilePath { get; init; } = null!;
    public string DigitalSignaturePath { get; init; } = null!;
    
    public DocumentMetadata DocumentMetadata { get; init; } = null!;
}