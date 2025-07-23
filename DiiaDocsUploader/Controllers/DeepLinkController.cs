using DiiaDocsUploader.Infrastructure;
using DiiaDocsUploader.Models.DeepLink;
using DiiaDocsUploader.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiiaDocsUploader.Controllers;

public class LinkResponse
{
    private readonly string _deepLink;

    public LinkResponse(string deepLink)
    {
        _deepLink = deepLink;
    }
    
    public string Link => $"https://diia.page.link?link={_deepLink}&apn=ua.gov.diia.app&isi=1489717872&ibi=ua.gov.diia.app";
    public string Qr => $"data:image/png;base64,{QrCodeService.GenerateQrCode(Link)}";
}

[ApiController]
[Route("api/[controller]/[action]")]
public class DeepLinkController : ControllerBase
{
    private readonly DeepLinkService _deepLinkService;

    public DeepLinkController(DeepLinkService deepLinkService)
    {
        _deepLinkService = deepLinkService;
    }

    [HttpPost("{branchId}")]
    public async Task<IActionResult> Generate(string branchId, DeepLinkCreateRequest request, CancellationToken cancellationToken)
    {
        var deepLink = await _deepLinkService.GenerateAsync(branchId, request, cancellationToken);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }

    [HttpPost("{documentTypeId}")]
    public async Task<IActionResult> GenerateUniversal(int documentTypeId, CancellationToken cancellationToken)
    {
        var deepLink = await _deepLinkService.GenerateByDocumentTypeIdAsync(documentTypeId, cancellationToken);
        
        return Ok(new LinkResponse(deepLink.DeepLink));
    }
}