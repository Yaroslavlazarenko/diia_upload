namespace DiiaDocsUploader.Infrastructure.ApiAuth;

public enum SessionTokenStatus
{
    Active,
    Deprecated,
    Invalid
}

public record SessionToken(string Token)
{
    public DateTimeOffset ExpiresAtUtc { get; } = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(2));
    public SessionTokenStatus Status => TokenStatus();

    private SessionTokenStatus TokenStatus()
    {
        if (DateTimeOffset.UtcNow <= ExpiresAtUtc.Add(TimeSpan.FromMinutes(-30)))
        {
            return SessionTokenStatus.Active;
        }

        if (DateTimeOffset.UtcNow < ExpiresAtUtc)
        {
            return SessionTokenStatus.Deprecated;
        }

        return SessionTokenStatus.Invalid;
    }
};