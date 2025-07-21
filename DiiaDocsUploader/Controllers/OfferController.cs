using DiiaDocsUploader.Models.Offer;
using DiiaDocsUploader.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiiaDocsUploader.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OfferController : ControllerBase
{
    private readonly OfferService _offerService;

    public OfferController(OfferService offerService)
    {
        _offerService = offerService;
    }

    [HttpPost("{branchId}")]
    public async Task<IActionResult> Create(string branchId, OfferCreateRequest request, CancellationToken ct)
    {
        var id = await _offerService.CreateAsync(branchId, request, ct);
        
        return Ok(id);
    }

    [HttpGet("{branchId}")]
    public async Task<IActionResult> List(string branchId, CancellationToken ct)
    {
        return Ok(await _offerService.ListAsync(branchId, ct: ct));
    }

    [HttpDelete("{branchId}/{offerId}")]
    public async Task<IActionResult> Delete(string branchId, string offerId, CancellationToken ct)
    {
        await _offerService.DeleteAsync(branchId, offerId, ct);

        return NoContent();
    }
}