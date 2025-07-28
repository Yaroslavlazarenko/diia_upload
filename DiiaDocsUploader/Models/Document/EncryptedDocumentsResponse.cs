using DiiaDocsUploader.Entity;

namespace DiiaDocsUploader.Models.Document;

public class EncryptedDocumentsResponse
{
    public Guid RequestId { get; set; }
    public string MetadataJsonBase64 { get; set; } =  null!;
    public List<string> DocumentFilesBase64 { get; set; } = [];
}
