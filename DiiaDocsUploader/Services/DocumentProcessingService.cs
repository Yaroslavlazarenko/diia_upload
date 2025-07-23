using DiiaDocsUploader.Contexts;
using DiiaDocsUploader.Entity;
using DiiaDocsUploader.Models.FileSystem;
using DiiaDocsUploader.Storage;

namespace DiiaDocsUploader.Services;

public class DocumentProcessingService
{
    private readonly IStorageService _storageService;
    private readonly DiiaDbContext _context;
    private readonly ILogger<DocumentProcessingService> _logger;
    
    public DocumentProcessingService(IStorageService storageService, DiiaDbContext context, ILogger<DocumentProcessingService> logger)
    {
        _storageService = storageService;
        _context = context;
        _logger = logger;
    }
    
    public async Task<ProcessingResult> ProcessUploadAsync(IFormCollection collection, string? requestIdHeader, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(requestIdHeader, out var deepLinkId))
        {
            _logger.LogWarning("Отримано запит без валідного заголовка X-Document-Request-Trace-Id. Генерується новий GUID.");
            deepLinkId = Guid.NewGuid();
        }
        
        var requestId = deepLinkId.ToString();
        _logger.LogInformation("Обробка запиту від Дії. DeepLinkId (RequestId): {RequestId}", requestId);
        
        string? encodeDataContent = collection["encodeData"].FirstOrDefault();
        if (string.IsNullOrEmpty(encodeDataContent))
        {
            _logger.LogWarning("Метадані (encodeData) не знайдено для RequestId: {RequestId}", requestId);
            return ProcessingResult.Failure("encodeData is missing.");
        }
        
        try
        {
            var metadataPath = await SaveBase64ContentAsync("metadata.json.p7s.p7e", encodeDataContent, requestId, cancellationToken);
            var metadataRecord = new DocumentMetadata
            {
                DeepLinkId = deepLinkId,
                MetadataFilePath = metadataPath
            };
            _context.DocumentMetadatas.Add(metadataRecord);
            _logger.LogInformation("Підготовлено запис для DocumentMetadatas. DeepLinkId: {DeepLinkId}", deepLinkId);
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, "Помилка формату Base64 для метаданих. RequestId: {RequestId}", requestId);
            return ProcessingResult.Failure("Invalid Base64 format for metadata.");
        }
        
        var files = collection.Files;
        var documentSignaturePairs = GroupFilesAndSignatures(files);

        foreach (var pair in documentSignaturePairs)
        {
             if (pair.Value.Document is null || pair.Value.Signature is null)
             {
                _logger.LogWarning("Для документа {BaseFileName} відсутній файл або підпис. Пропускається.", pair.Key);
                continue;
             }
             
             try
             {
                var documentPath = await SaveBase64FormFileAsync(pair.Value.Document, requestId, cancellationToken);
                var signaturePath = await SaveBase64FormFileAsync(pair.Value.Signature, requestId, cancellationToken);
                
                var documentFileRecord = new DocumentFile
                {
                    DeepLinkId = deepLinkId,
                    DocumentFilePath = documentPath,
                    DigitalSignaturePath = signaturePath
                };
                _context.DocumentFiles.Add(documentFileRecord);
                _logger.LogInformation("Підготовлено запис для DocumentFiles. DeepLinkId: {DeepLinkId}", deepLinkId);
             }
             catch(FormatException ex)
             {
                _logger.LogError(ex, "Помилка формату Base64 для файлу {FileName}. RequestId: {RequestId}", pair.Key, requestId);
                return ProcessingResult.Failure($"Invalid Base64 format for file '{pair.Key}'.");
             }
        }
        
        if (!_context.ChangeTracker.HasChanges())
        {
            _logger.LogWarning("Не знайдено коректних даних для збереження. RequestId: {RequestId}", requestId);
            return ProcessingResult.Failure("No valid data to save.");
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Успішно оброблено та збережено в БД. DeepLinkId: {RequestId}", requestId);

        return ProcessingResult.Success(deepLinkId);
    }
    
    private async Task<string> SaveBase64FormFileAsync(IFormFile formFile, string requestId, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(formFile.OpenReadStream());
        var base64Content = await reader.ReadToEndAsync(cancellationToken);
        return await SaveBase64ContentAsync(formFile.FileName, base64Content, requestId, cancellationToken);
    }

    private async Task<string> SaveBase64ContentAsync(string fileName, string base64Content, string requestId, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(base64Content))
        {
            _logger.LogWarning("Вміст для файлу {FileName} є порожнім. Пропускається. RequestId: {RequestId}", fileName, requestId);
            throw new ArgumentException("Base64 content is empty.");
        }

        var fileBytes = Convert.FromBase64String(base64Content);
        await using var finalStream = new MemoryStream(fileBytes);
        return await _storageService.UploadAsync(fileName, finalStream, requestId, ct);
    }
    
    private Dictionary<string, FileSignaturePair> GroupFilesAndSignatures(IFormFileCollection files)
    {
        var groupedFiles = new Dictionary<string, FileSignaturePair>();
        const string signatureExtension = ".p7s";

        foreach (var file in files)
        {
            string baseName;
            bool isSignature = file.FileName.EndsWith(signatureExtension, StringComparison.OrdinalIgnoreCase);

            if (isSignature)
            {
                baseName = file.FileName.Substring(0, file.FileName.Length - signatureExtension.Length);
            }
            else
            {
                baseName = file.FileName;
            }

            if (!groupedFiles.ContainsKey(baseName))
            {
                groupedFiles[baseName] = new FileSignaturePair();
            }

            if (isSignature)
            {
                groupedFiles[baseName].Signature = file;
            }
            else
            {
                groupedFiles[baseName].Document = file;
            }
        }

        return groupedFiles;
    }
}