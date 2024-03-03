using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net;
using Microsoft.Extensions.Primitives;
using static System.Net.Mime.MediaTypeNames;

namespace EasyCache.Attributes;

public class ETagFilterAttribute : Attribute, IActionFilter
{
    private readonly int[] _statusCodes;

    public ETagFilterAttribute(params HttpStatusCode[] statusCodes)
    {
        _statusCodes = statusCodes.Cast<int>().ToArray();
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (_statusCodes.Contains(context.HttpContext.Response.StatusCode))
        {
            var etag = string.Empty; //TODO: add etag generator

            if (context.HttpContext.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out StringValues ifNoneMatch)
                && ifNoneMatch.Contains(etag))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.NotModified);
            }

            context.HttpContext.Response.Headers[HeaderNames.ETag] = new[] { etag };
        }
    }
}