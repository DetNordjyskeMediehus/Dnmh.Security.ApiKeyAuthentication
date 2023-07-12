using Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;
using Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Moq.Protected;
using System.Text.Encodings.Web;

namespace Dnmh.Security.ApiKeyAuthentication.Tests;
internal static class MockHelpers
{
    public static Mock<IOptionsMonitor<T>> CreateMockOptionsMonitor<T>(Action<T>? configureOptions = null)
        where T : new()
    {
        var options = new T();
        configureOptions?.Invoke(options);
        var optionsMock = new Mock<IOptionsMonitor<T>>();
        optionsMock.Setup(x => x.Get(It.IsAny<string>())).Returns(options);
        optionsMock.Setup(x => x.CurrentValue).Returns(options);
        return optionsMock;
    }

    public static Mock<HttpContext> CreateMockHttpContextWithRequestHeaders(Dictionary<string, StringValues> headers) =>
        CreateMockHttpContextWithMockRequest(headers, new Dictionary<string, StringValues>());

    public static Mock<HttpContext> CreateMockHttpContextWithRequestQueryParams(Dictionary<string, StringValues> queryParameters) =>
        CreateMockHttpContextWithMockRequest(new Dictionary<string, StringValues>(), queryParameters);

    public static Mock<HttpContext> CreateMockHttpContextWithMockRequest(Dictionary<string, StringValues> headers, Dictionary<string, StringValues> queryParameters)
    {
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.SetupGet(x => x.Headers).Returns(new HeaderDictionary(headers));
        mockRequest.SetupGet(x => x.Query).Returns(new QueryCollection(queryParameters));
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.SetupGet(x => x.Request).Returns(mockRequest.Object);
        return mockHttpContext;
    }

    public static ApiKeyAuthenticationHandler CreateApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, bool apiKeyValidationResult = true)
    {
        var mockService = new Mock<SimpleApiKeyAuthenticationServiceBase>();
        mockService.Protected().Setup<bool>("Validate", ItExpr.IsAny<ValidationContext>()).Returns(apiKeyValidationResult);

        var mockLogger = new Mock<ILogger>();
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);

        return new ApiKeyAuthenticationHandler(
            options,
            mockLoggerFactory.Object,
            UrlEncoder.Default,
            new Mock<ISystemClock>().Object,
            mockService.Object);
    }
}
