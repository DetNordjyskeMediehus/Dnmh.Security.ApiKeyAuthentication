using Shouldly;

namespace DNMH.Security.ApiKeyAuthentication.Tests;

public class ApiKeyAuthenticationOptionsTest
{
    [Fact]
    public void TestValidateDefaultOptions()
    {
        // Arrange
        var options = new ApiKeyAuthenticationOptions();

        // Act
        options.Validate();
    }

    [Fact]
    public void TestValidateTrueAllowApiKeyInQuery()
    {
        // Arrange
        var options = new ApiKeyAuthenticationOptions
        {
            AllowApiKeyInQuery = true,
            AllowApiKeyInRequestHeader = false,
        };

        // Act
        options.Validate();
    }

    [Fact]
    public void TestValidateTrueAllowApiKeyInRequestHeader()
    {
        // Arrange
        var options = new ApiKeyAuthenticationOptions
        {
            AllowApiKeyInQuery = false,
            AllowApiKeyInRequestHeader = true,
        };

        // Act
        options.Validate();
    }

    [Fact]
    public void TestValidateTrueUseAuthorizationHeaderKey()
    {
        // Arrange
        var options = new ApiKeyAuthenticationOptions
        {
            AllowApiKeyInRequestHeader = true,
            UseAuthorizationHeaderKey = true,
        };

        // Act
        options.Validate();
    }

    [Fact]
    public void TestValidateTrueUseSchemeNameInAuthorizationHeader()
    {
        // Arrange
        var options = new ApiKeyAuthenticationOptions
        {
            AllowApiKeyInRequestHeader = true,
            UseAuthorizationHeaderKey = true,
            UseSchemeNameInAuthorizationHeader = true,
        };

        // Act
        options.Validate();
    }

    [Fact]
    public void TestFailValidationOnAllowOptions()
    {
        // Arrange
        var options = new ApiKeyAuthenticationOptions
        {
            AllowApiKeyInQuery = false,
            AllowApiKeyInRequestHeader = false
        };

        // Act
        var act = () => options.Validate();

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void TestFailValidationOnUseAuthorizationHeaderKey()
    {
        // Arrange
        var options = new ApiKeyAuthenticationOptions
        {
            UseAuthorizationHeaderKey = true,
            AllowApiKeyInRequestHeader = false
        };

        // Act
        var act = () => options.Validate();

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void TestFailValidationOnUseSchemeNameInAuthorizationHeader()
    {
        // Arrange
        var options1 = new ApiKeyAuthenticationOptions
        {
            UseSchemeNameInAuthorizationHeader = true,
            UseAuthorizationHeaderKey = false,
            AllowApiKeyInRequestHeader = false
        };
        var options2 = new ApiKeyAuthenticationOptions
        {
            UseSchemeNameInAuthorizationHeader = true,
            UseAuthorizationHeaderKey = true,
            AllowApiKeyInRequestHeader = false
        };
        var options3 = new ApiKeyAuthenticationOptions
        {
            UseSchemeNameInAuthorizationHeader = true,
            UseAuthorizationHeaderKey = false,
            AllowApiKeyInRequestHeader = true
        };

        // Act
        var act1 = () => options1.Validate();
        var act2 = () => options2.Validate();
        var act3 = () => options3.Validate();

        // Assert
        act1.ShouldThrow<ArgumentException>();
        act2.ShouldThrow<ArgumentException>();
        act3.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void TestFailValidationOnEmptyAuthorizationSchemeInHeader()
    {
        // Arrange
        var options = new ApiKeyAuthenticationOptions
        {
            AuthorizationSchemeInHeader = string.Empty,
            UseAuthorizationHeaderKey = true,
            UseSchemeNameInAuthorizationHeader = false
        };

        // Act
        var act = () => options.Validate();

        // Assert
        act.ShouldThrow<ArgumentException>();
    }
}