namespace PropagationMw;

public class SafeHeaderPropagationHandler(
    IHttpContextAccessor httpContextAccessor,
    IEnumerable<string> headerNames)
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            return base.SendAsync(request, cancellationToken);
        }

        foreach (var headerName in headerNames)
        {
            if (!httpContext.Request.Headers.TryGetValue(headerName, out var headerValue))
            {
                continue;
            }

            if (!request.Headers.Contains(headerName))
            {
                request.Headers.TryAddWithoutValidation(headerName, (IEnumerable<string>)headerValue);
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}