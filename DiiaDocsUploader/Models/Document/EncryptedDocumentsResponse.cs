namespace DiiaDocsUploader.Models.Document;

public class EncryptedDocumentsResponse
{
    public Guid RequestId { get; set; }
    public string MetadataJsonBase64 { get; set; } =  null!;
    public List<DocumentsFilesBase64> DocumentsFilesBase64 { get; set; } = new List<DocumentsFilesBase64>();
}

public class DocumentsFilesBase64
{
    public string DocumentBase64{ get; set; } = null!;
    public string SignatureBase64 { get; set; } = null!;
}

