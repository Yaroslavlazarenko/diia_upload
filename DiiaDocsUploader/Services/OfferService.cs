using System.Net.Http.Headers;
using DiiaDocsUploader.Credentials;
using DiiaDocsUploader.Exceptions;
using DiiaDocsUploader.Models.Common;
using DiiaDocsUploader.Models.Offer;
using DiiaDocsUploader.Services.Auth;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Services;

public class OfferService : DiiaServiceBase
{
    public OfferService(IHttpClientFactory httpClientFactory,
        ISessionTokenService sessionTokenService,
        IOptions<DiiaCredentials> options) : base(httpClientFactory, sessionTokenService, options)
    {
    }

    public async Task<DiiaIdResponse> CreateAsync(string branchId, OfferCreateRequest request, CancellationToken ct = default)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v1/acquirers/branch/{branchId}/offer";
        
        var token = await _sessionTokenService.GetActiveSessionTokenAsync(ct);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var content = JsonContent.Create(request);

        var response = await client.PostAsync(url, content, ct);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<DiiaIdResponse>(cancellationToken: ct) ?? throw new DiiaApiException();
        }

        throw new DiiaApiException();
    }

    public async Task DeleteAsync(string branchId, string offerId, CancellationToken ct = default)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v1/acquirers/branch/{branchId}/offer/{offerId}";
        
        var token = await _sessionTokenService.GetActiveSessionTokenAsync(ct);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var response = await client.DeleteAsync(url, ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new DiiaApiException();
        }
    }

    public async Task<object> ListAsync(string branchId, int? skip = null, int? limit = null,
        CancellationToken ct = default)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v1/acquirers/branch/{branchId}/offers";
        
        if (skip >= 0 && limit > 0)
        {
            url += $"?skip={skip}&limit={limit}";
        }

        var token = await _sessionTokenService.GetActiveSessionTokenAsync(ct);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        
        var response = await client.GetAsync(url, ct);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<OfferListResponse>(ct) ?? throw new DiiaApiException();
        }

        throw new DiiaApiException();
    }
}