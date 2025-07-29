using Microsoft.AspNetCore.Mvc;

namespace DiiaDocsUploader.Filters.Attributes;

public class DecrypterKeyAuthorizeAttribute : TypeFilterAttribute
{
    public DecrypterKeyAuthorizeAttribute() : base(typeof(DecrypterKeyAuthFilter))
    {
        
    }
}