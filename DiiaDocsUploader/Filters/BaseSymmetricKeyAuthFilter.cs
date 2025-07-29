using DiiaDocsUploader.Credentials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace DiiaDocsUploader.Filters;

public abstract class BaseSymmetricKeyAuthFilter : IAsyncActionFilter
{
    protected readonly ApiCredentials _credentials;

    protected BaseSymmetricKeyAuthFilter(IOptions<ApiCredentials> apiCredentialsOptions)
    {
        _credentials = apiCredentialsOptions.Value;
    }

    protected abstract string GetSecretKey();

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var secretApiKey = GetSecretKey();

        if (string.IsNullOrEmpty(secretApiKey))
        {
            context.Result = new ObjectResult("API-ключ не налаштований на сервері.") { StatusCode = 500 };
            return;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizationHeader))
        {
            context.Result = new UnauthorizedObjectResult($"Заголовок {HeaderNames.Authorization} відсутній.");
            return;
        }

        var headerValue = authorizationHeader.ToString();
        
        if (!secretApiKey.Equals(headerValue))
        {
            context.Result = new UnauthorizedObjectResult("Невірний API ключ.");
            return;
        }

        await next();
    }
}