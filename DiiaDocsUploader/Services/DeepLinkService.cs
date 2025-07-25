using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using DiiaDocsUploader.Contexts;
using DiiaDocsUploader.Credentials;
using DiiaDocsUploader.Exceptions;
using DiiaDocsUploader.Models.DeepLink;
using DiiaDocsUploader.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Services;

public class DeepLinkService : DiiaServiceBase
{
    private readonly DiiaDbContext _context;
    public DeepLinkService(DiiaDbContext context, IHttpClientFactory httpClientFactory,
        ISessionTokenService sessionTokenService,
        IOptions<DiiaCredentials> options) : base(httpClientFactory, sessionTokenService, options)
    {
        _context = context;
    }

    public async Task<DeepLinkResponse> GenerateAsync(string branchId, DeepLinkCreateRequest request, CancellationToken cancellationToken)
    {
        var url = $"https://{_diiaCredentials.Host}/api/v2/acquirers/branch/{branchId}/offer-request/dynamic";
        
        var token = await _sessionTokenService.GetActiveSessionTokenAsync(cancellationToken);

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        var internalRequest = new InternalDiiaDeepLinkRequest(request);
        
        var content = JsonContent.Create(internalRequest, options: new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        var response = await client.PostAsync(url, content, cancellationToken);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<DeepLinkResponse>(cancellationToken: cancellationToken) 
                   ?? throw new DiiaApiException("Failed to deserialize Diia response.");
        }

        var error = await response.Content.ReadAsStringAsync(cancellationToken);
    
        throw new DiiaApiException(error); 
    }

    public async Task<DeepLinkResponse> GenerateByDocumentTypeIdAsync(int documentTypeId, CancellationToken cancellationToken)
    {
        var offerDocumentType = await _context.OfferDocumentTypes
            .Include(oft => oft.Offer)
            .FirstOrDefaultAsync(oft => oft.DocumentTypeId == documentTypeId, cancellationToken);

        if (offerDocumentType is null)
        {
            throw new DiiaApiException("Offer document type not found");
        }
        
        var offer = offerDocumentType.Offer;

        if (offer is null)
        {
            throw new DiiaApiException("Offer not found");
        }
        
        var branchId = offerDocumentType.Offer.BranchId;

        var newRequest = new DeepLinkCreateRequest
        {
            OfferId = offerDocumentType.OfferId,
            UseDiiaId = true,
            ReturnLink = $"{_diiaCredentials.DefaultReturnLink}/confirmed/{documentTypeId}" 
        };
        
        return await GenerateAsync(branchId, newRequest, cancellationToken);
    }
}