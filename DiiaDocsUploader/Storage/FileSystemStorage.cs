using DiiaDocsUploader.Models.FileSystem;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Storage;

public class FileSystemStorage : IStorageService
{
    private readonly string _rootPath;
    private readonly ILogger<FileSystemStorage> _logger;

    public FileSystemStorage(IOptions<FileSystemStorageOptions> options, ILogger<FileSystemStorage> logger)
    {
        _logger = logger;
        
        if (string.IsNullOrWhiteSpace(options.Value.RootPath))
        {
            throw new ArgumentNullException(nameof(options.Value.RootPath), "Root path for file storage is not configured.");
        }
        
        _rootPath = options.Value.RootPath;
        _logger.LogInformation("File system storage initialized. Root path: {RootPath}", _rootPath);
    }

    public async Task<string> UploadAsync(string fileName, Stream contentStream, string folderName, CancellationToken cancellationToken)
    {
        var folderPath = Path.Combine(_rootPath, folderName);

        if (!Directory.Exists(folderPath))
        {
            _logger.LogInformation("Creating directory: {FolderPath}", folderPath);
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, fileName);
        _logger.LogInformation("Preparing to save file to: {FilePath}", filePath);

        contentStream.Seek(0, SeekOrigin.Begin);

        try
        {
            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await contentStream.CopyToAsync(fileStream, cancellationToken);
            _logger.LogInformation("Successfully saved file: {FilePath}", filePath);
            return filePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save file to {FilePath}", filePath);
            throw;
        }
    }
}