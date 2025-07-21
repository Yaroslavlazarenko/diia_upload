using System.Net.Http.Headers;
using DiiaDocsUploader.Credentials;
using DiiaDocsUploader.Exceptions;
using DiiaDocsUploader.Infrastructure.ApiAuth;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Services.Auth;

public class SandboxSessionTokenService : SessionTokenServiceBase
{
    public SandboxSessionTokenService(IHttpClientFactory httpClientFactory, IOptions<DiiaCredentials> options) : base(httpClientFactory, options)
    {
        
    }

    protected override async Task<SessionToken> GetSessionTokenAsync(CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient();

        var url = $"https://{_credentials.Host}/api/v1/auth/acquirer/{_credentials.AcquirerToken}";

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials.AuthAcquirerToken);
        
        var response = await client.GetAsync(url, ct);

        if (response.IsSuccessStatusCode)
        {
            var sessionToken = await response.Content.ReadFromJsonAsync<SessionToken>(cancellationToken: ct);

            return sessionToken ?? throw new DiiaApiException();
        }

        throw new DiiaApiException();
    }
}