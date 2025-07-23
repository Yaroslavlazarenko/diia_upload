using System.Net.Http.Headers;
using DiiaDocsUploader.Contexts;
using DiiaDocsUploader.Credentials;
using DiiaDocsUploader.Entity;
using DiiaDocsUploader.Exceptions;
using DiiaDocsUploader.Models.Branch;
using DiiaDocsUploader.Models.Common;
using DiiaDocsUploader.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Services;

public class BranchService : DiiaServiceBase
{
    private readonly DiiaDbContext _context;
    public BranchService(DiiaDbContext context, IHttpClientFactory httpClientFactory,
        ISessionTokenService sessionTokenService,
        IOptions<DiiaCredentials> options) : base(httpClientFactory, sessionTokenService, options)
    {
        _context = context;
    }

    public async Task<BranchListResponse> ListAsync(BranchListRequest request, CancellationToken cancellationToken)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v2/acquirers/branches";
        
        var queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
        if (request.Skip >= 0)
        {
            queryParams["skip"] = request.Skip.ToString();
        }
        if (request.Limit > 0)
        {
            queryParams["limit"] = request.Limit.ToString();
        }

        var queryString = queryParams.ToString();
        if (!string.IsNullOrEmpty(queryString))
        {
            url += "?" + queryString;
        }

        var token = await _sessionTokenService.GetActiveSessionTokenAsync(cancellationToken);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var response = await client.GetAsync(url, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var branchListResponse = await response.Content.ReadFromJsonAsync<BranchListResponse>(cancellationToken);
            
            if (branchListResponse is null)
            {
                throw new DiiaApiException("Failed to deserialize the successful response from Diia API.");
            }
        
            return branchListResponse;
        }
        
        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new DiiaApiException($"Failed to retrieve branches from Diia API. Status code: {response.StatusCode}. Response: {errorContent}");
    }

    public async Task<DiiaIdResponse> CreateAsync(BranchCreateRequest request, CancellationToken cancellationToken)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v2/acquirers/branch";

        var token = await _sessionTokenService.GetActiveSessionTokenAsync(cancellationToken);
        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var content = JsonContent.Create(request);
        var response = await client.PostAsync(url, content, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<DiiaIdResponse>(cancellationToken: cancellationToken);
            var branchId = result?.Id;

            if (branchId == null)
            {
                throw new DiiaApiException("Branch ID was null.");
            }

            var documentTypes = await _context.DocumentTypes
                .Where(dt => request.Scopes.Sharing.Contains(dt.NameDiia))
                .ToListAsync(cancellationToken);
            
            var branch = new Branch
            {
                Id = branchId,
                CustomFullAddress = request.CustomFullAddress,
                CustomFullName = request.CustomFullName,
                Name = request.Name,
                Email = request.Email,
                Region = request.Region,
                District = request.District,
                Location = request.Location,
                Street = request.Street,
                House = request.House,
                BranchDocumentTypes = documentTypes.Select(dt => new BranchDocumentType
                {
                    BranchId = branchId,
                    DocumentTypeId = dt.Id
                }).ToList()
            };

            await _context.Branches.AddAsync(branch, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return result!;
        }

        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        Console.WriteLine(errorContent);
        throw new DiiaApiException($"Diia API request failed with status {response.StatusCode}. Content: {errorContent}");
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v2/acquirers/branch/{id}";

        var token = await _sessionTokenService.GetActiveSessionTokenAsync(cancellationToken);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var response = await client.DeleteAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new DiiaApiException($"Failed to delete branch: {response.StatusCode}. Content: {errorContent}");
        }

        var branch = await _context.Branches
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (branch is not null)
        {
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<BranchResponse> GetById(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Branch ID cannot be null or empty.");
        }
    
        var url = $"https://{_diiaCredentials.Host}/api/v2/acquirers/branch/{id}";

        var token = await _sessionTokenService.GetActiveSessionTokenAsync(cancellationToken);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var response = await client.GetAsync(url, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var branchResponse = await response.Content.ReadFromJsonAsync<BranchResponse>(cancellationToken: cancellationToken);
            
            if (branchResponse is null)
            {
                throw new DiiaApiException($"Failed to deserialize the successful response from Diia API for branch '{id}'.");
            }
        
            return branchResponse;
        }
        
        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new DiiaApiException($"Failed to retrieve branch '{id}' from Diia API. Status code: {response.StatusCode}. Response: {errorContent}");
    }
}