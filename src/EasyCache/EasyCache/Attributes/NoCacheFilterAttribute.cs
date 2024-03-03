using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace EasyCache.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class NoCacheFilterAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next.Invoke();

        var responseHeaders = context.HttpContext.Response.Headers;
        responseHeaders[HeaderNames.CacheControl] = "max-age=0,no-cache,no-store,must-revalidate";
        responseHeaders[HeaderNames.Pragma] = "no-cache";
        responseHeaders[HeaderNames.Expires] = DateTime.UtcNow.AddSeconds(-1).ToString("R");
    }
}