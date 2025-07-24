using DiiaDocsUploader.Contexts;
using DiiaDocsUploader.Models.Document;
using DiiaDocsUploader.Storage;
using Microsoft.EntityFrameworkCore;

namespace DiiaDocsUploader.Services;

public class DocumentRetrievalService
{
    private readonly DiiaDbContext _context;
    private readonly IStorageService _storageService;
    private readonly ILogger<DocumentRetrievalService> _logger;

    public DocumentRetrievalService(DiiaDbContext context, IStorageService storageService, ILogger<DocumentRetrievalService> logger)
    {
        _context = context;
        _storageService = storageService;
        _logger = logger;
    }

    public async Task<List<EncryptedDocumentsResponse>> GetDocumentsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы не может быть меньше 1.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы не может быть меньше 1.");
        }
        
        _logger.LogInformation("Запит на отримання документів. Сторінка: {PageNumber}, Розмір сторінки: {PageSize}", pageNumber, pageSize);

        var metadataRecords = await _context.DocumentMetadatas
            .Include(m => m.DocumentFiles) 
            .OrderByDescending(m => m.DeepLinkId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var results = new List<EncryptedDocumentsResponse>();

        foreach (var metadata in metadataRecords)
        {
            try
            {
                var response = new EncryptedDocumentsResponse
                {
                    RequestId = metadata.DeepLinkId,
                    MetadataJsonBase64 = await _storageService.ReadAsBase64Async(metadata.MetadataFilePath, cancellationToken)
                };

                foreach (var docFile in metadata.DocumentFiles)
                {
                    var documentContentTask = _storageService.ReadAsBase64Async(docFile.DocumentFilePath, cancellationToken);
                    var signatureContentTask = _storageService.ReadAsBase64Async(docFile.DigitalSignaturePath, cancellationToken);

                    await Task.WhenAll(documentContentTask, signatureContentTask);

                    response.DocumentsFilesBase64.Add(new DocumentsFilesBase64
                    {
                        DocumentBase64 = await documentContentTask,
                        SignatureBase64 = await signatureContentTask
                    });
                }
                
                results.Add(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при обробці та завантаженні даних для DeepLinkId: {DeepLinkId}", metadata.DeepLinkId);
            }
        }

        _logger.LogInformation("Успішно сформовано {Count} записів.", results.Count);
        return results;
    }
}