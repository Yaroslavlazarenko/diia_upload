namespace DiiaDocsUploader.Storage;

public interface IStorageService
{
    Task UploadAsync(string fileName, Stream contentStream, string folderName, CancellationToken cancellationToken = default);
}