using Microsoft.AspNetCore.Mvc;

namespace DiiaDocsUploader.Filters.Attributes;

public class AdminKeyAuthorizeAttribute : TypeFilterAttribute
{
    public AdminKeyAuthorizeAttribute() : base(typeof(AdminKeyAuthFilter))
    {
        
    }
}