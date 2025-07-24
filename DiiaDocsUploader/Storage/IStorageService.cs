namespace DiiaDocsUploader.Storage;

public interface IStorageService
{
    Task<string> UploadAsync(string fileName, Stream contentStream, string folderName, CancellationToken cancellationToken);
    
    Task<string> UploadFromBase64Async(string fileName, string base64Content, string folderName, CancellationToken cancellationToken);
    Task<string> ReadAsBase64Async(string filePath, CancellationToken cancellationToken);
    Task DeleteAsync(string filePath, CancellationToken cancellationToken);
}