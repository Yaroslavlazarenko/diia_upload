namespace DiiaDocsUploader.Models.Document;

public class DeleteDocumentsRequest
{
    public List<string> DeepLinkIds { get; set; } = new List<string>();
}