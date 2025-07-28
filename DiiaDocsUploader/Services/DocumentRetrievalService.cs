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

    public async Task<List<EncryptedDocumentsResponse>> GetDocumentsAsync(EncryptedDocumentRequest request, CancellationToken cancellationToken)
    {
        if (request.PageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(request.PageNumber), "Номер страницы не может быть меньше 1.");
        }

        if (request.PageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(request.PageSize), "Размер страницы не может быть меньше 1.");
        }
        
        _logger.LogInformation("Запит на отримання документів. Сторінка: {PageNumber}, Розмір сторінки: {PageSize}", request.PageNumber, request.PageSize);

        var metadataRecords = await _context.DocumentMetadatas
            .Where(m => !m.IsDeleted)
            .Include(m => m.DocumentFiles) 
            .OrderByDescending(m => m.DeepLinkId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
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

                var documentContentTasks = metadata.DocumentFiles
                    .Select(docFile => _storageService.ReadAsBase64Async(docFile.DocumentFilePath, cancellationToken))
                    .ToList();

                var documentContents = await Task.WhenAll(documentContentTasks);
                
                response.DocumentBase64.AddRange(documentContents);
                
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