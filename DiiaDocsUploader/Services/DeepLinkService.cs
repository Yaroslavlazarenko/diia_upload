using System.Net.Http.Headers;
using DiiaDocsUploader.Credentials;
using DiiaDocsUploader.Exceptions;
using DiiaDocsUploader.Models.DeepLink;
using DiiaDocsUploader.Services.Auth;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Services;

public class DeepLinkService : DiiaServiceBase
{
    public DeepLinkService(IHttpClientFactory httpClientFactory,
        ISessionTokenService sessionTokenService,
        IOptions<DiiaCredentials> options) : base(httpClientFactory, sessionTokenService, options)
    {
    }

    public async Task<DeepLinkResponse> GenerateAsync(string branchId, DeepLinkCreateRequest request, CancellationToken ct = default)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v2/acquirers/branch/{branchId}/offer-request/dynamic";
        
        var token = await _sessionTokenService.GetActiveSessionTokenAsync(ct);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var internalRequest = new InternalDiiaDeepLinkRequest(request);
        
        var content = JsonContent.Create(internalRequest);

        var response = await client.PostAsync(url, content, ct);
        
        if (response.IsSuccessStatusCode)
        {
            var contentStr = await response.Content.ReadAsStringAsync(ct);
            
            return await response.Content.ReadFromJsonAsync<DeepLinkResponse>(cancellationToken: ct) ?? throw new DiiaApiException();
        }

        var error = await response.Content.ReadAsStringAsync(ct);
        
        throw new DiiaApiException();
    }
}