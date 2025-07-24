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
    
    public async Task<string> UploadFromBase64Async(string fileName, string base64Content, string folderName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(base64Content))
        {
            _logger.LogWarning("Base64 content for file {FileName} is empty. Skipping. Folder: {FolderName}", fileName, folderName);
            throw new ArgumentException("Base64 content cannot be empty.", nameof(base64Content));
        }

        _logger.LogInformation("Converting Base64 content to byte stream for file: {FileName}", fileName);
        
        var fileBytes = Convert.FromBase64String(base64Content);
        await using var finalStream = new MemoryStream(fileBytes);
        
        return await UploadAsync(fileName, finalStream, folderName, cancellationToken);
    }
    
    public async Task<string> ReadAsBase64Async(string filePath, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(_rootPath, filePath);
        
        _logger.LogInformation("Attempting to read file for Base64 conversion: {FullPath}", fullPath);

        try
        {
            if (!File.Exists(fullPath))
            {
                _logger.LogWarning("File not found at path: {FullPath}", fullPath);
                throw new FileNotFoundException("The requested file was not found.", fullPath);
            }
            
            var fileBytes = await File.ReadAllBytesAsync(fullPath, cancellationToken);
            
            var base64Content = Convert.ToBase64String(fileBytes);
            
            _logger.LogInformation("Successfully read and converted file to Base64: {FullPath}", fullPath);
            
            return base64Content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read or convert file at path: {FullPath}", fullPath);
            throw;
        }
    }
    
    public Task DeleteAsync(string filePath, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(_rootPath, filePath);
        
        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation("Файл успішно видалено: {FullPath}", fullPath);
            }
            else
            {
                _logger.LogWarning("Спроба видалити файл, який не існує: {FullPath}", fullPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при видаленні файлу: {FullPath}", fullPath);
        }

        return Task.CompletedTask;
    }
}