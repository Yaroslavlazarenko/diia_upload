using System.Net.Http.Headers;
using DiiaDocsUploader.Contexts;
using DiiaDocsUploader.Credentials;
using DiiaDocsUploader.Entity;
using DiiaDocsUploader.Exceptions;
using DiiaDocsUploader.Models.Common;
using DiiaDocsUploader.Models.Offer;
using DiiaDocsUploader.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Services;

public class OfferService : DiiaServiceBase
{
    private readonly DiiaDbContext _context;
    
    public OfferService(DiiaDbContext context, IHttpClientFactory httpClientFactory,
        ISessionTokenService sessionTokenService,
        IOptions<DiiaCredentials> options) : base(httpClientFactory, sessionTokenService, options)
    {
        _context = context;
    }

    public async Task<DiiaIdResponse> CreateAsync(string branchId, OfferCreateRequest request, CancellationToken cancellationToken)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v1/acquirers/branch/{branchId}/offer";
        
        var token = await _sessionTokenService.GetActiveSessionTokenAsync(cancellationToken);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var content = JsonContent.Create(request);

        var response = await client.PostAsync(url, content, cancellationToken);
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<DiiaIdResponse>(cancellationToken: cancellationToken);
            var offerId = result?.Id;

            if (offerId == null)
            {
                throw new DiiaApiException("Branch ID was null.");
            }

            var documentTypes = await _context.DocumentTypes
                .Where(dt => request.Scopes.Sharing.Contains(dt.NameDiia))
                .ToListAsync(cancellationToken);
            
            var offer = new Offer
            {
                Id = offerId,
                BranchId = branchId,
                Name = request.Name,
                OfferDocumentTypes = documentTypes.Select(dt => new OfferDocumentType
                {
                    OfferId = offerId,
                    DocumentTypeId = dt.Id
                }).ToList()
            };

            await _context.Offers.AddAsync(offer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return result!;
        }

        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        Console.WriteLine(errorContent);
        throw new DiiaApiException($"Diia API request failed with status {response.StatusCode}. Content: {errorContent}");
    }

    public async Task DeleteAsync(string branchId, string offerId, CancellationToken cancellationToken)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v1/acquirers/branch/{branchId}/offer/{offerId}";
        
        var token = await _sessionTokenService.GetActiveSessionTokenAsync(cancellationToken);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var response = await client.DeleteAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new DiiaApiException($"Failed to delete offer: {response.StatusCode}. Content: {errorContent}");
        }

        var offer = await _context.Offers
            .FirstOrDefaultAsync(o => o.Id == offerId, cancellationToken);

        if (offer is not null)
        {
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<object> ListAsync(string branchId, OfferListRequest request, 
        CancellationToken cancellationToken)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v1/acquirers/branch/{branchId}/offers";
        
        if (request.Skip >= 0 && request.Limit > 0)
        {
            url += $"?skip={request.Skip}&limit={request.Limit}";
        }

        var token = await _sessionTokenService.GetActiveSessionTokenAsync(cancellationToken);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        
        var response = await client.GetAsync(url, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<OfferListResponse>(cancellationToken) ?? throw new DiiaApiException();
        }

        throw new DiiaApiException();
    }
}