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
    public async Task<IActionResult> Generate(string branchId, DeepLinkCreateRequest request, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync(branchId, request, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }

    [HttpPost]
    public async Task<IActionResult> GenerateInternalPassport(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "18891377c16fde4d27069b827fa4101439b07b4bc3dff8aab41f9907d38182c2a4aef49c7e6db87a0042f9537c8dd59c5e6b5fd26a26be541fd55845703f3260",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateForeignPassport(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "925d87f891d1b47238dd1a5ef10ee42fafe4b4770861c469ae6f318cc8e0ea023e56a0863ca7a4000cbec39aaa85e0de2b798932752f96f0006f0605e82e113d",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateTaxpayerCard(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "9c82ac8bc45f165c5b12836189650025da485d4b70d08d442befce9547f006ded6146debfd0c7fe2344e132022af123c9ee04958becf8a415939fcc46414a19a",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateDriverLicense(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "50869d5bd7c14be9cf15c5e00ed4e94a12abdf2e6e5473e072c131c9ebea566353187562da904af71ac1b82c880ed4756d159575808c37a6837e9ddadef54bdc",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateVehicleLicense(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "54796a5befffb8619ff8b449cf3ea610b487f5e2645bec895e0aa6872fd4c8bd70b106243038b0a5c0c3dc3af66a4fbeac3ef34df307a64c0585c8b41e77c757",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateUserBirthRecord(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "46290ce35517ca5908ae4584b22a58cc4d05eda85f1c093adcfd3e1503a9b4aa04f877d96d76ca6c2754064d4efab570009c37b9c5decbe902485bcf10a967b0",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateBirthCertificate(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "a85a376c03c66d299111db6f0e278757e005f9c46513ed2fe5e8645a8ce617bdc11d246f53f7870276cad9a85b8ddfb8becc38ffb16b60433fa738e67863799f",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateReferenceInternallyDisplacedPerson(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "feccb41161bee17dae2596f64c10dd7702c77a155abbcbc13f1392034b55fa4d816a5b88fcc294671a7930d8b2b8b27a17a76cdfaa92a5eebbcff3d0ca0e388b",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateStudentIdCard(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "a6a196164cb1e230ce19bb31964ff50103151c01ec3f57f94a1f0bc421475cfad27da8107fd5802f91671aca50cf78c6560a20e415fee5dff6c9d203e9a7496b",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GeneratePensionCard(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "b94e5006d8c3e84935467918220ee84eaa589751f6fc9718690c90f8ce58ff9b4bdeb4292e5cef1df6a6c0e04fd154fd302b04c027c99bfee7fbf8df38c6e78c",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateNameChangeActRecord(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "3aedf5b80e7f95fe3e31fd90c5b82a458979c98055822169581155cd863a249d820803f496fdf572b75cf5257c61195c60e766e455be7221b06b70f322e96ce0",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateMarriageActRecord(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "8aa4ff5f9fc5623306a49b7f167d03f72f602506c6c4fb360f439723a983ac9d6b74adbb80f7e062df151c0421271d02267b8a5ffe5e8a427b0be1f8a11f6b6b",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateDivorceActRecord(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "8aa4ff5f9fc5623306a49b7f167d03f72f602506c6c4fb360f439723a983ac9d6b74adbb80f7e062df151c0421271d02267b8a5ffe5e8a427b0be1f8a11f6b6b",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateVeteranCertificate(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "bad2f753f7b50fa863267a0ec339014040adacdfc8363018e0da11746c18b72dae3530d61455ff16c981b839378eb7efd64c84b7d39974cc33b0dd6ba7d93e07",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateEducationDocument(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "d6965412b9c8a6a1805ae6b7ac76baee923589715483fb5bc0cb6506ca727e2e77f531d174b722c419c838f6fc3bf0326aa1de5f721348ade38bc4f1c6230742",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateResidencePermitPermanent(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "e5e0a64879950dce22fd8e8ca82db259674bb4a94872840a6021bf4e9c2af4c45352930e221c5a70b92b6650a722e19130f5e1b4fc628bc5a59aa19f1834b0be",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateResidencePermitTemporary(bool useDiiaId, CancellationToken ct)
    {
        var deepLink = await _deepLinkService.GenerateAsync("10c29a5a51d966154d6ffb723b61c0c163c29baf2956cd2e38fbb51d583489329c3201334c3896d303dc71d945da99ef1a8870ae807c1cb9d8b0f7d2ab36754a", new DeepLinkCreateRequest
        {
            OfferId = "36d57e4cca4ed2a4785eef9020456d03e8d36d71e4fc56bf015e60712682771e38accabd95181d0c4c092e3c2674f41a4a799f989f0bb4b5b62638b17ec16391",
            UseDiiaId = useDiiaId
        }, ct);

        return Ok(new LinkResponse(deepLink.DeepLink));
    }
}