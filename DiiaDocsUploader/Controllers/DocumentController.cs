using DiiaDocsUploader.Services;
using DiiaDocsUploader.Storage;
using Microsoft.AspNetCore.Mvc;

namespace DiiaDocsUploader.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DocumentController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(IStorageService storageService, ILogger<DocumentController> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<IActionResult> OnlineUpload([FromForm] IFormCollection collection, CancellationToken cancellationToken)
    {
        string? requestId = GetRequestIdFromHeader(Request.Headers);
        if (string.IsNullOrEmpty(requestId))
        {
            _logger.LogWarning("Отримано запит без заголовка X-Document-Request-Trace-Id. Генерується новий GUID.");
            requestId = Guid.NewGuid().ToString();
        }
        
        _logger.LogInformation("Обробка запиту від Дії. Ім'я теки: {RequestId}", requestId);

        try
        {
            var uploadTasks = new List<Task>();

            string? encodeDataContent = collection["encodeData"].FirstOrDefault();
            if (!string.IsNullOrEmpty(encodeDataContent))
            {
                _logger.LogInformation("Отримано метадані (encodeData). Додавання до черги на завантаження.");
                uploadTasks.Add(SaveBase64ContentAsync("metadata.json.p7s.p7e", encodeDataContent, requestId, cancellationToken));
            }
            else
            {
                _logger.LogWarning("Метадані (encodeData) не знайдено для RequestId: {RequestId}", requestId);
            }

            var files = collection.Files;
            _logger.LogInformation("Отримано {FileCount} файлів для RequestId: {RequestId}", files.Count, requestId);
            
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                     _logger.LogInformation("Додавання файлу документа {FileName} до черги на обробку.", file.FileName);
                     uploadTasks.Add(SaveBase64FormFileAsync(file, requestId, cancellationToken));
                }
            }

            if (!uploadTasks.Any())
            {
                 _logger.LogWarning("Отримано порожній запит (без метаданих та файлів) для RequestId: {RequestId}", requestId);
                 return BadRequest(new { success = false, error = "Request is empty." });
            }
            
            await Task.WhenAll(uploadTasks);
            
            _logger.LogInformation("Успішно оброблено та завантажено всі файли до теки: {RequestId}", requestId);
            return Ok(new { success = true });
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, "Помилка формату Base64 для RequestId: {RequestId}. Перевірте вміст файлів.", requestId);
            return BadRequest(new { success = false, error = "Invalid Base64 format." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Критична помилка під час обробки запиту від Дії з RequestId: {RequestId}", requestId);
            return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, error = "An internal error occurred." });
        }
    }
    
    private async Task SaveBase64FormFileAsync(IFormFile formFile, string requestId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Читання потоку файлу {FileName} для декодування з Base64.", formFile.FileName);
        
        using var reader = new StreamReader(formFile.OpenReadStream());
        var base64Content = await reader.ReadToEndAsync(cancellationToken);
        
        await SaveBase64ContentAsync(formFile.FileName, base64Content, requestId, cancellationToken);
    }
    
    private async Task SaveBase64ContentAsync(string fileName, string base64Content, string requestId, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(base64Content))
        {
            _logger.LogWarning("Вміст для файлу {FileName} є порожнім. Пропускається. RequestId: {RequestId}", fileName, requestId);
            return;
        }

        var fileBytes = Convert.FromBase64String(base64Content);
        await using var finalStream = new MemoryStream(fileBytes);
        
        _logger.LogInformation("Завантаження декодованого файлу {FileName} (Розмір: {Size} байт) до теки {RequestId}", fileName, finalStream.Length, requestId);
        await _storageService.UploadAsync(fileName, finalStream, requestId, ct);
    }
    
    private string? GetRequestIdFromHeader(IHeaderDictionary headers)
    {
        const string headerName = "X-Document-Request-Trace-Id";
        var header = headers.Keys.FirstOrDefault(k => k.Equals(headerName, StringComparison.OrdinalIgnoreCase));
        return header != null ? headers[header].FirstOrDefault() : null;
    }
}