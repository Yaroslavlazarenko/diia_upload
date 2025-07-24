using DiiaDocsUploader.Contexts;
using DiiaDocsUploader.Models;
using Microsoft.EntityFrameworkCore;

namespace DiiaDocsUploader.Services;

public class DocumentTypeService
{
    private readonly DiiaDbContext _context;
    private readonly ILogger<DocumentTypeService> _logger;

    public DocumentTypeService(DiiaDbContext context, ILogger<DocumentTypeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Selector>> GetDocumentTypesAsSelectorAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Запит на отримання списку типів документів для селектора.");
        
        var documentTypes = await _context.DocumentTypes
            .AsNoTracking()
            .OrderBy(dt => dt.NameUa)
            .Select(dt => new Selector
            {
                Id = dt.Id,
                Value = dt.NameUa
            })
            .OrderBy(dt => dt.Value)
            .ToListAsync(cancellationToken);
            
        _logger.LogInformation("Успішно отримано {Count} типів документів.", documentTypes.Count);
        return documentTypes;
    }
}