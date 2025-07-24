using DiiaDocsUploader.Models;
using DiiaDocsUploader.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiiaDocsUploader.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DocumentTypeController : ControllerBase
{
    private readonly DocumentTypeService _documentTypeService;
    private readonly ILogger<DocumentTypeController> _logger; 
    
    public DocumentTypeController(DocumentTypeService documentTypeService, ILogger<DocumentTypeController> logger)
    {
        _documentTypeService = documentTypeService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Selector>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Selector(CancellationToken cancellationToken)
    {
        try
        {
            var documentTypes = await _documentTypeService.GetDocumentTypesAsSelectorAsync(cancellationToken);
            return Ok(documentTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при отриманні списку типів документів.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }
}