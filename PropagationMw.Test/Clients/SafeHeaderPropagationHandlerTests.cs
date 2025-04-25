using Microsoft.AspNetCore.Http;

namespace PropagationMw.Test.Clients;

public class SafeHeaderPropagationHandlerTests
{
    [Fact]
    public async Task Adds_Headers_When_HttpContext_Is_Available()
    {
        // Arrange
        const string headerName = "X-Test-Header";
        const string headerValue = "TestValue";

        var context = new DefaultHttpContext();
        context.Request.Headers[headerName] = headerValue;

        var httpContextAccessor = new TestHttpContextAccessor(context);

        var handler = new SafeHeaderPropagationHandler(httpContextAccessor, [headerName])
        {
            InnerHandler = new TestHandler()
        };

        var client = new HttpClient(handler);

        // Act
        var response = await client.GetAsync("http://localhost");

        // Assert
        Assert.True(TestHandler.CapturedRequest.Headers.Contains(headerName));
        Assert.Equal(headerValue, TestHandler.CapturedRequest.Headers.GetValues(headerName).Single());
    }

    [Fact]
    public async Task Skips_Headers_When_HttpContext_Is_Null()
    {
        // Arrange
        var httpContextAccessor = new TestHttpContextAccessor(null);

        var handler = new SafeHeaderPropagationHandler(httpContextAccessor, ["X-Whatever"])
        {
            InnerHandler = new TestHandler()
        };

        var client = new HttpClient(handler);

        // Act
        var response = await client.GetAsync("http://localhost");

        // Assert
        Assert.Empty(TestHandler.CapturedRequest.Headers);
    }

    private class TestHttpContextAccessor(HttpContext? context) : IHttpContextAccessor
    {
        public HttpContext? HttpContext { get; set; } = context;
    }

    private class TestHandler : DelegatingHandler
    {
        public static HttpRequestMessage CapturedRequest { get; private set; } = new HttpRequestMessage();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CapturedRequest = request;
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            return Task.FromResult(response);
        }
    }
}