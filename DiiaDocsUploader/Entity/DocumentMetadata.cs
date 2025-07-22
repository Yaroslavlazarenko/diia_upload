namespace DiiaDocsUploader.Entity;

public class DocumentMetadata
{
    public Guid DeepLinkId { get; set; }
    public string MetadataFilePath { get; set; } = null!;
}