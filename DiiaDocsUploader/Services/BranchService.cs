using System.Net.Http.Headers;
using DiiaDocsUploader.Credentials;
using DiiaDocsUploader.Exceptions;
using DiiaDocsUploader.Models.Branch;
using DiiaDocsUploader.Models.Common;
using DiiaDocsUploader.Services.Auth;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Services;

public class BranchService : DiiaServiceBase
{
    public BranchService(IHttpClientFactory httpClientFactory,
        ISessionTokenService sessionTokenService,
        IOptions<DiiaCredentials> options) : base(httpClientFactory, sessionTokenService, options)
    {
    }

    public async Task<BranchListResponse> ListAsync(int? skip = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v2/acquirers/branches";

        if (skip >= 0 && limit > 0)
        {
            url += $"?skip={skip}&limit={limit}";
        }

        var token = await _sessionTokenService.GetActiveSessionTokenAsync(cancellationToken);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var response = await client.GetAsync(url, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<BranchListResponse>(cancellationToken) ?? throw new DiiaApiException();
        }

        throw new DiiaApiException();
    }

    public async Task<DiiaIdResponse> CreateAsync(BranchCreateRequest request, CancellationToken cancellationToken = default)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v2/acquirers/branch";

        var token = await _sessionTokenService.GetActiveSessionTokenAsync(cancellationToken);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var content = JsonContent.Create(request);

        var response = await client.PostAsync(url, content, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            
            return await response.Content.ReadFromJsonAsync<DiiaIdResponse>(cancellationToken: cancellationToken) ?? throw new DiiaApiException();
        }
        Console.WriteLine(response.Content.ReadAsStringAsync(cancellationToken).Result);

        throw new DiiaApiException();
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v2/acquirers/branch/{id}";

        var token = await _sessionTokenService.GetActiveSessionTokenAsync(ct);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var response = await client.DeleteAsync(url, ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new DiiaApiException();
        }
    }

    public async Task<BranchResponse> GetById(string id, CancellationToken ct = default)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v2/acquirers/branch/{id}";

        var token = await _sessionTokenService.GetActiveSessionTokenAsync(ct);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var response = await client.GetAsync(url, ct);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<BranchResponse>(cancellationToken: ct) ?? throw new DiiaApiException();
        }
        
        throw new DiiaApiException();
    }
}