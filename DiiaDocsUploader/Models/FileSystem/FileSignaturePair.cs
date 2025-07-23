namespace DiiaDocsUploader.Models.FileSystem;

public class FileSignaturePair
{
    public IFormFile? Document { get; set; }
    public IFormFile? Signature { get; set; }
}