using DiiaDocsUploader.Infrastructure.ApiAuth;

namespace DiiaDocsUploader.Services.Auth;

public interface ISessionTokenService
{
    ValueTask<SessionToken> GetActiveSessionTokenAsync(CancellationToken ct = default);
}