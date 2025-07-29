using DiiaDocsUploader.Filters.Attributes;
using DiiaDocsUploader.Models.Document;
using DiiaDocsUploader.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiiaDocsUploader.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DocumentController : ControllerBase
{
    private readonly DocumentProcessingService _processingService;
    private readonly DocumentRetrievalService _retrievalService; 
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(
        DocumentProcessingService processingService, 
        DocumentRetrievalService retrievalService, 
        ILogger<DocumentController> logger)
    {
        _processingService = processingService;
        _retrievalService = retrievalService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public async Task<IActionResult> OnlineUpload([FromForm] IFormCollection collection,
        CancellationToken cancellationToken)
    {
        string? requestId = GetRequestIdFromHeader(Request.Headers);
        _logger.LogInformation("Отримано запит на завантаження документів. RequestId з заголовка: {RequestId}",
            requestId ?? "відсутній");

        try
        {
            var result = await _processingService.ProcessUploadAsync(collection, requestId, cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Запит з DeepLinkId {DeepLinkId} успішно оброблено.", result.DeepLinkId);
                return Ok(new { success = true });
            }

            _logger.LogWarning("Не вдалося обробити запит. Причина: {Error}", result.ErrorMessage);
            return BadRequest(new { success = false, error = result.ErrorMessage });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Критична непередбачена помилка під час обробки запиту з RequestId: {RequestId}",
                requestId);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { success = false, error = "An internal error occurred." });
        }
    }

    [HttpPost]
    [DecrypterKeyAuthorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EncryptedDocumentsResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEncryptedDocuments([FromBody]EncryptedDocumentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var documents = await _retrievalService.GetDocumentsAsync(request, cancellationToken);
            return Ok(documents);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogWarning(ex, "Отримано невалідні параметри пагінації.");
            return BadRequest(new { success = false, error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Критична помилка при отриманні списку документів.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, error = "An internal error occurred while retrieving documents." });
        }
    }

    [HttpPost]
    [DecrypterKeyAuthorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDocuments(
        [FromBody] List<Guid> documentsId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Отримано запит на видалення {Count} документів.", documentsId.Count);
        
        try
        {
            await _processingService.DeleteDocumentsAsync(documentsId, cancellationToken);
            
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Некоректний запит на видалення документів.");
            return BadRequest(new { success = false, error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Критична помилка під час видалення документів.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, error = "An internal error occurred while deleting documents." });
        }
    }
    
    private string? GetRequestIdFromHeader(IHeaderDictionary headers)
    {
        const string headerName = "X-Document-Request-Trace-Id";
        var header = headers.Keys.FirstOrDefault(k => k.Equals(headerName, StringComparison.OrdinalIgnoreCase));
        return header != null ? headers[header].FirstOrDefault() : null;
    }
}