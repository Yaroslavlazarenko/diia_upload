using DiiaDocsUploader.Credentials;
using DiiaDocsUploader.Infrastructure.ApiAuth;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Services.Auth;

public abstract class SessionTokenServiceBase : ISessionTokenService
{
    protected readonly DiiaCredentials _credentials;

    protected readonly IHttpClientFactory _httpClientFactory;

    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private volatile SessionToken? _sessionToken;

    protected SessionTokenServiceBase(IHttpClientFactory httpClientFactory, IOptions<DiiaCredentials> options)
    {
        _httpClientFactory = httpClientFactory;

        _credentials = options.Value;
    }

    protected abstract Task<SessionToken> GetSessionTokenAsync(CancellationToken ct = default);

    public async ValueTask<SessionToken> GetActiveSessionTokenAsync(CancellationToken ct = default)
    {
        if (_sessionToken is null || _sessionToken.Status != SessionTokenStatus.Active)
        {
            try
            {
                await _semaphore.WaitAsync(ct);
                
                if (_sessionToken is null || _sessionToken.Status != SessionTokenStatus.Active)
                {
                    _sessionToken = await GetSessionTokenAsync(ct);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        return _sessionToken;
    }
}