using Dnmh.Security.ApiKeyAuthentication.ApiKeyAuthenticationHandler;
using System.Security.Claims;

namespace Dnmh.Security.ApiKeyAuthentication.Sample.Services;

/// <summary>
/// Simple implementation of the <see cref="IApiKeyAuthenticationService"/> that validates the api key in appsettings.json with the supplied key.
/// </summary>
public class ApiKeyAuthenticationService : IApiKeyAuthenticationService
{
    private readonly string _validApiKey;

    public ApiKeyAuthenticationService(IConfiguration configuration)
    {
        var apiKey = configuration.GetRequiredSection("MyApiKey").Value;
        ArgumentException.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
        _validApiKey = apiKey;
    }

    public Task<ClaimsPrincipal?> ValidateAsync(string apiKey) 
    {
        ClaimsPrincipal? claimsPrinciple = null;

        if (_validApiKey == apiKey)
        {
            claimsPrinciple = new ClaimsPrincipal(new ClaimsIdentity("ApiKey"));
        }

        return Task.FromResult(claimsPrinciple);
    }
}
