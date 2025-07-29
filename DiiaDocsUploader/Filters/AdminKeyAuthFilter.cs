using DiiaDocsUploader.Credentials;
using Microsoft.Extensions.Options;

namespace DiiaDocsUploader.Filters;

public class AdminKeyAuthFilter : BaseSymmetricKeyAuthFilter
{
    public AdminKeyAuthFilter(IOptions<ApiCredentials> apiCredentialsOptions) : base(apiCredentialsOptions)
    {
        
    }
    protected override string GetSecretKey() => _credentials.AdminKey;
}