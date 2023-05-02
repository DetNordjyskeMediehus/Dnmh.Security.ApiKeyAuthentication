using Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

namespace Dnmh.Security.ApiKeyAuthentication.Sample.Services;

/// <summary>
/// Simple implementation of the <see cref="IApiKeyAuthenticationService"/> that validates the api key in appsettings.json with the supplied key.
/// </summary>
public class ApiKeyAuthenticationService : SimpleApiKeyAuthenticationService
{
    private readonly string _validApiKey;

    public ApiKeyAuthenticationService(IConfiguration configuration)
    {
        var apiKey = configuration.GetRequiredSection("MyApiKey").Value;
        ArgumentException.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
        _validApiKey = apiKey;
    }

    protected override bool ValidateKey(string apiKey) => _validApiKey == apiKey;
}
