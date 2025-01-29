using DNMH.Security.ApiKeyAuthentication.Context;
using System.Security.Claims;

namespace DNMH.Security.ApiKeyAuthentication;

/// <summary>
/// A simple abstract implementation of the <see cref="IApiKeyAuthenticationService"/>, 
/// that creates a minimal <see cref="ClaimsPrincipal"/>, if a given api key is valid.
/// </summary>
public abstract class SimpleApiKeyAuthenticationServiceBase : IApiKeyAuthenticationService
{
    /// <inheritdoc/>
    public Task<ClaimsPrincipal?> ValidateAsync(ValidationContext context)
    {
        ClaimsPrincipal? claimsPrinciple = null;

        if (Validate(context))
        {
            claimsPrinciple = new ClaimsPrincipal(new ClaimsIdentity(context.Scheme.Name));
        }

        return Task.FromResult(claimsPrinciple);
    }

    /// <summary>
    /// Validates the given <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The context for the validation, where the api key that needs validation also can be found through <see cref="ValidationContext.ApiKey"/></param>
    /// <returns>True if the given api key is valid. False if not.</returns>
    protected abstract bool Validate(ValidationContext context);
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
    protected override bool Validate(ValidationContext context) => _validApiKey == context.ApiKey;
}
