using System.Security.Claims;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

/// <summary>
/// A simple abstract implementation of the <see cref="IApiKeyAuthenticationService"/>, 
/// that creates a minimal <see cref="ClaimsPrincipal"/>, if a given api key is valid.
/// </summary>
public abstract class SimpleApiKeyAuthenticationService : IApiKeyAuthenticationService
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
