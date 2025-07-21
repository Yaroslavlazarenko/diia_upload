using DiiaDocsUploader.Credentials;
using DiiaDocsUploader.Services.Auth;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Services;

public abstract class DiiaServiceBase
{
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly ISessionTokenService _sessionTokenService;
    protected readonly DiiaCredentials _diiaCredentials;

    protected DiiaServiceBase(IHttpClientFactory httpClientFactory, ISessionTokenService sessionTokenService, IOptions<DiiaCredentials> options)
    {
        _httpClientFactory = httpClientFactory;
        _sessionTokenService = sessionTokenService;
        _diiaCredentials = options.Value;
    }
}