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
    public async Task<IActionResult> List(CancellationToken ct)
    {
        return Ok(await _branchService.ListAsync(ct: ct));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var branch = await _branchService.GetById(id, ct);

        return Ok(branch);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BranchCreateRequest request, CancellationToken ct)
    {
        var id = await _branchService.CreateAsync(request, ct);

        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _branchService.DeleteAsync(id, ct);
        
        return NoContent();
    }
}