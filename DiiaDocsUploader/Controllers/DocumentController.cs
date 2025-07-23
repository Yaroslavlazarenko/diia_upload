using DiiaDocsUploader.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiiaDocsUploader.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DocumentController : ControllerBase
{
    private readonly DocumentProcessingService _processingService;
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(DocumentProcessingService processingService, ILogger<DocumentController> logger)
    {
        _processingService = processingService;
        _logger = logger;
    }

    [HttpPost]
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

    private string? GetRequestIdFromHeader(IHeaderDictionary headers)
    {
        const string headerName = "X-Document-Request-Trace-Id";
        var header = headers.Keys.FirstOrDefault(k => k.Equals(headerName, StringComparison.OrdinalIgnoreCase));
        return header != null ? headers[header].FirstOrDefault() : null;
    }
}