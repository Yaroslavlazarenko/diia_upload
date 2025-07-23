using DiiaDocsUploader.Contexts;
using DiiaDocsUploader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiiaDocsUploader.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DocumentTypeController : ControllerBase
{
    private readonly DiiaDbContext _context;
    
    public DocumentTypeController(DiiaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Selector(CancellationToken cancellationToken)
    {
        var documentTypes = await _context
            .DocumentTypes
            .Select(dt => new Selector
            {
                Id = dt.Id,
                Value = dt.NameUa
            })
            .ToListAsync(cancellationToken);
        
        return Ok(documentTypes);
    }
}