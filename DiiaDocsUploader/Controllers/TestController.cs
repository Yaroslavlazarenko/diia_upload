using DiiaDocsUploader.Models.Branch;
using DiiaDocsUploader.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiiaDocsUploader.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController : ControllerBase
{
    private readonly BranchService _service;

    public TestController(BranchService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBranch(CancellationToken ct)
    {
        var branch = new BranchCreateRequest()
        {
            Name = "Test branch",
            Email = "test@op.edu.ua",
            District = "Odesa",
            Region = "Odesa region",
            Location = "Odesa",
            Street = "Shevchenko str.",
            House = "1"
        };

        var response = await _service.CreateAsync(branch, ct);

        return Ok(response);
    }
}