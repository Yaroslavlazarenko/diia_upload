using DiiaDocsUploader.Contexts;
using DiiaDocsUploader.Entity;
using DiiaDocsUploader.Models.FileSystem;
using DiiaDocsUploader.Storage;
using Microsoft.EntityFrameworkCore;

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
        
        var files = collection.Files;
        var documentSignaturePairs = GroupFilesAndSignatures(files);
        var documentFileRecords = new List<DocumentFile>();

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
                
                documentFileRecords.Add(new DocumentFile
                {
                    DocumentFilePath = documentPath,
                    DigitalSignaturePath = signaturePath
                });
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Помилка формату Base64 для файлу {FileName}. RequestId: {RequestId}", pair.Key, requestId);
                return ProcessingResult.Failure($"Invalid Base64 format for file '{pair.Key}'.");
            }
        }
        
        if (!documentFileRecords.Any())
        {
            _logger.LogWarning("Не знайдено коректних пар 'документ-підпис' для збереження. RequestId: {RequestId}", requestId);
            return ProcessingResult.Failure("No valid document-signature pairs to save.");
        }
        
        try
        {
            var metadataPath = await _storageService.UploadFromBase64Async("metadata.json.p7s.p7e", encodeDataContent, requestId, cancellationToken);
            
            var metadataRecord = new DocumentMetadata
            {
                DeepLinkId = deepLinkId,
                MetadataFilePath = metadataPath,
                IsDeleted = false
            };
            
            foreach (var fileRecord in documentFileRecords)
            {
                metadataRecord.DocumentFiles.Add(fileRecord);
            }
            
            _context.DocumentMetadatas.Add(metadataRecord); 
            
            await _context.SaveChangesAsync(cancellationToken); 
            
            _logger.LogInformation("Успішно створено та збережено в БД запис для DeepLinkId: {RequestId}", requestId);
            return ProcessingResult.Success(deepLinkId);
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, "Помилка формату Base64 для метаданих. RequestId: {RequestId}", requestId);
            return ProcessingResult.Failure("Invalid Base64 format for metadata.");
        }
    }
    
    private async Task<string> SaveBase64FormFileAsync(IFormFile formFile, string requestId, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(formFile.OpenReadStream());
        var base64Content = await reader.ReadToEndAsync(cancellationToken);
        
        return await _storageService.UploadFromBase64Async(formFile.FileName, base64Content, requestId, cancellationToken);
    }
    
    private Dictionary<string, FileSignaturePair> GroupFilesAndSignatures(IFormFileCollection files)
    {
        var groupedFiles = new Dictionary<string, FileSignaturePair>();
        
        const string signatureExtension = ".p7s";
        const string encryptedExtension = ".p7e";

        foreach (var file in files)
        {
            string baseName;
            bool isEncryptedDocument = file.FileName.EndsWith(encryptedExtension, StringComparison.OrdinalIgnoreCase);
            bool isSignedDocument = !isEncryptedDocument && file.FileName.EndsWith(signatureExtension, StringComparison.OrdinalIgnoreCase);

            if (isEncryptedDocument)
            {
                baseName = file.FileName.Substring(0, file.FileName.Length - encryptedExtension.Length);
            }
            else if (isSignedDocument)
            {
                baseName = file.FileName;
            }
            else
            {
                _logger.LogWarning("Пропущено невідомий тип файлу: {FileName}", file.FileName);
                continue;
            }

            if (!groupedFiles.ContainsKey(baseName))
            {
                groupedFiles[baseName] = new FileSignaturePair();
            }

            if (isEncryptedDocument)
            {
                groupedFiles[baseName].Document = file;
            }
            else if (isSignedDocument)
            {
                groupedFiles[baseName].Signature = file;
            }
        }
        
        return groupedFiles;
    }
    
    public async Task DeleteDocumentsAsync(IEnumerable<Guid> deepLinkIds, CancellationToken cancellationToken)
    {
        if (deepLinkIds == null)
        {
            throw new ArgumentNullException(nameof(deepLinkIds), "Вхідний параметр не може бути null.");
        }
        
        var guidsToDelete = deepLinkIds.ToList();
        
        if (!guidsToDelete.Any())
        {
            throw new ArgumentException("Список ID для видалення не може бути порожнім.", nameof(deepLinkIds));
        }

        if (!guidsToDelete.Any())
        {
            _logger.LogWarning("Після валідації не залишилось жодного коректного ID для видалення.");
            return;
        }
        
        var recordsToMarkAsDeleted = await _context.DocumentMetadatas
            .Where(m => guidsToDelete.Contains(m.DeepLinkId))
            .ToListAsync(cancellationToken);

        if (!recordsToMarkAsDeleted.Any())
        {
            _logger.LogWarning("Не знайдено документів для видалення за наданими ID.");
            return; 
        }
        
        foreach (var record in recordsToMarkAsDeleted)
        {
            record.IsDeleted = true;
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Успішно позначено як видалені {Count} записів про документи.", recordsToMarkAsDeleted.Count);
    }
}