namespace DiiaDocsUploader.Models.DeepLink;

public class DeepLinkCreateRequest
{
    public string OfferId { get; set; } = null!;
    public bool UseDiiaId { get; set; } = true;
}