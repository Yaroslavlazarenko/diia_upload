using DiiaDocsUploader.Credentials;

namespace DiiaDocsUploader.DIExtensions;

public static class ConfigurationExtensions
{
    public static WebApplicationBuilder AddDiiaCredentials(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("diia_credentials.json");

        builder.Services.Configure<DiiaCredentials>(builder.Configuration.GetSection("DiiaCredentials"));

        return builder;
    }
}