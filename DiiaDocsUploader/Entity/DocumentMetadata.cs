namespace DiiaDocsUploader.Entity;

public class DocumentMetadata
{
    public Guid DeepLinkId { get; set; }
    public string MetadataFilePath { get; set; } = null!;
    
    public ICollection<DocumentFile> DocumentFiles { get; set; } = new HashSet<DocumentFile>();
}