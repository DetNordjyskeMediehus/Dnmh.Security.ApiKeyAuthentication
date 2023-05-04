using System.Security.Claims;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

/// <summary>
/// A simple abstract implementation of the <see cref="IApiKeyAuthenticationService"/>, 
/// that creates a minimal <see cref="ClaimsPrincipal"/>, if a given api key is valid.
/// </summary>
public abstract class SimpleApiKeyAuthenticationServiceBase : IApiKeyAuthenticationService
{
    /// <inheritdoc/>
    public Task<ClaimsPrincipal?> ValidateAsync(string apiKey)
    {
        ClaimsPrincipal? claimsPrinciple = null;

        if (ValidateKey(apiKey))
        {
            claimsPrinciple = new ClaimsPrincipal(new ClaimsIdentity("ApiKey"));
        }

        return Task.FromResult(claimsPrinciple);
    }

    /// <summary>
    /// Validates the given <paramref name="apiKey"/>.
    /// </summary>
    /// <param name="apiKey">The api key to validate</param>
    /// <returns>True if the given api key is valid. False if not.</returns>
    protected abstract bool ValidateKey(string apiKey);
}

/// <summary>
/// An implementation of the <see cref="SimpleApiKeyAuthenticationService"/>, 
/// which validates api keys against the valid api key given in the constructor (using case-sensitive equality).
/// </summary>
public class SimpleApiKeyAuthenticationService : SimpleApiKeyAuthenticationServiceBase
{
    private readonly string _validApiKey;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="validApiKey">The valid api key other api keys are validated against (using case-sensitive equality).</param>
    public SimpleApiKeyAuthenticationService(string validApiKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(validApiKey, nameof(validApiKey));

        _validApiKey = validApiKey;
    }

    /// <inheritdoc/>
    protected override bool ValidateKey(string apiKey) => _validApiKey == apiKey;
}
