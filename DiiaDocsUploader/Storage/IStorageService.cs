namespace DiiaDocsUploader.Storage;

public interface IStorageService
{
    Task<string> UploadAsync(string fileName, Stream contentStream, string folderName, CancellationToken cancellationToken);
}