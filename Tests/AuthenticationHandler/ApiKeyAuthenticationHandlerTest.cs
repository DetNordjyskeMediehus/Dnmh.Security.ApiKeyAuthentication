using Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Dnmh.Security.ApiKeyAuthentication.Tests.AuthenticationHandler;
public class ApiKeyAuthenticationHandlerTest
{
    [Fact]
    public async Task TestDefaultOptionsHeaderKeySuccessfully()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>();
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestHeaders(new Dictionary<string, StringValues> { { "X-API-KEY", "key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task TestOptionsWithCustomHeaderKeySuccessfully()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>(options =>
        {
            options.AllowApiKeyInQuery = false;
            options.AllowApiKeyInRequestHeader = true;
            options.HeaderKeys.Add("ApiKey");
        });
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestHeaders(new Dictionary<string, StringValues> { { "ApiKey", "key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task TestOptionsWithMultipleCustomHeaderKeysSuccessfully()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>(options =>
        {
            options.AllowApiKeyInQuery = false;
            options.AllowApiKeyInRequestHeader = true;
            options.HeaderKeys.Add("ApiKey");
            options.HeaderKeys.Add("XApiKey");
        });
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestHeaders(new Dictionary<string, StringValues> { { "XApiKey", "key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task TestOptionsWithMultipleCustomHeaderKeysButNoneMatchShouldFail()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>(options =>
        {
            options.AllowApiKeyInQuery = false;
            options.AllowApiKeyInRequestHeader = true;
            options.HeaderKeys.Add("ApiKey");
            options.HeaderKeys.Add("XApiKey");
        });
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestHeaders(new Dictionary<string, StringValues> { { "mykey", "key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task TestOptionsWithAuthorizationHeaderKeySuccessfully()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>(options =>
        {
            options.AllowApiKeyInQuery = false;
            options.AllowApiKeyInRequestHeader = true;
            options.UseAuthorizationHeaderKey = true;
        });
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestHeaders(new Dictionary<string, StringValues> { { HeaderNames.Authorization, "Bearer key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task TestOptionsWithAuthorizationHeaderAndCustomSchemeKeySuccessfully()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>(options =>
        {
            options.AllowApiKeyInQuery = false;
            options.AllowApiKeyInRequestHeader = true;
            options.UseAuthorizationHeaderKey = true;
            options.AuthorizationSchemeInHeader = "ApiKey";
        });
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestHeaders(new Dictionary<string, StringValues> { { HeaderNames.Authorization, "ApiKey key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task TestOptionsWithAuthorizationHeaderAndCustomSchemeKeyButWrongSchemeShouldFail()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>(options =>
        {
            options.AllowApiKeyInQuery = false;
            options.AllowApiKeyInRequestHeader = true;
            options.UseAuthorizationHeaderKey = true;
            options.AuthorizationSchemeInHeader = "ApiKey";
        });
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestHeaders(new Dictionary<string, StringValues> { { HeaderNames.Authorization, "Bearer key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task TestOptionsWithAuthorizationHeaderKeyNoBearerShouldFail()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>(options =>
        {
            options.AllowApiKeyInQuery = false;
            options.AllowApiKeyInRequestHeader = true;
            options.UseAuthorizationHeaderKey = true;
        });
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestHeaders(new Dictionary<string, StringValues> { { HeaderNames.Authorization, "key" } }); // "Bearer" is missing
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task TestDefaultOptionsQueryKeySuccessfully()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>();
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestQueryParams(new Dictionary<string, StringValues> { { "apikey", "key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task TestDefaultOptionsWrongQueryKeyShouldFail()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>();
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestQueryParams(new Dictionary<string, StringValues> { { "jwt", "key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task TestOptionsWithCustomQueryKeySuccessfully()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>(options =>
        {
            options.QueryKeys.Add("mykey");
        });
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestQueryParams(new Dictionary<string, StringValues> { { "mykey", "key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task TestOptionsWithMultipleCustomQueryKeysSuccessfully()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>(options =>
        {
            options.QueryKeys.Add("mykey");
            options.QueryKeys.Add("mykey2");
        });
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestQueryParams(new Dictionary<string, StringValues> { { "mykey", "key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task TestOptionsWithMultipleCustomQueryKeysButNoneMatchShouldFail()
    {
        // Arrange
        var optionsMock = MockHelpers.CreateMockOptionsMonitor<ApiKeyAuthenticationOptions>(options =>
        {
            options.QueryKeys.Add("mykey");
            options.QueryKeys.Add("mykey2");
        });
        var handler = MockHelpers.CreateApiKeyAuthenticationHandler(optionsMock.Object);
        var mockHttpContext = MockHelpers.CreateMockHttpContextWithRequestQueryParams(new Dictionary<string, StringValues> { { "someother", "key" } });
        await handler.InitializeWithSchemeNameAsync(mockHttpContext.Object);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
    }
}
