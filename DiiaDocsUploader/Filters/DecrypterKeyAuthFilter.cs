using DiiaDocsUploader.Credentials;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Filters;

public class DecrypterKeyAuthFilter : BaseSymmetricKeyAuthFilter
{
    public DecrypterKeyAuthFilter(IOptions<ApiCredentials> apiCredentialsOptions) : base(apiCredentialsOptions)
    {
        
    }
    protected override string GetSecretKey() => _credentials.DecrypterKey;
}