using QRCoder;
using SixLabors.ImageSharp;

namespace DiiaDocsUploader.Infrastructure;

public static class QrCodeService
{
    public static string GenerateQrCode(string deepLink)
    {
        using var qrGenerator = new QRCodeGenerator();

        var qrData = qrGenerator.CreateQrCode(deepLink, QRCodeGenerator.ECCLevel.Q);

        var imageType = Base64QRCode.ImageType.Png;

        var qrCode = new Base64QRCode(qrData);

        var logo = Image.Load(Path.Combine("Logo", "logo.png"));

        if (logo is not null)
        {
            return qrCode.GetGraphic(20, Color.Black, Color.White, logo);
        }

        return qrCode.GetGraphic(20, SixLabors.ImageSharp.Color.Black,
            SixLabors.ImageSharp.Color.White);
    }
}