using DiiaDocsUploader.Models.Branch;
using DiiaDocsUploader.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiiaDocsUploader.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BranchController : ControllerBase
{
    private readonly BranchService _branchService;

    public BranchController(BranchService branchService)
    {
        _branchService = branchService;
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        return Ok(await _branchService.ListAsync(cancellationToken: cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var branch = await _branchService.GetById(id, cancellationToken);

        return Ok(branch);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BranchCreateRequest request, CancellationToken cancellationToken)
    {
        var id = await _branchService.CreateAsync(request, cancellationToken);

        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await _branchService.DeleteAsync(id, cancellationToken);
        
        return NoContent();
    }
}