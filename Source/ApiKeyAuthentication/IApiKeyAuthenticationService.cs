using System.Security.Claims;
using Dnmh.Security.ApiKeyAuthentication.Context;

namespace Dnmh.Security.ApiKeyAuthentication;

/// <summary>
/// Interface for the api key authentication service
/// </summary>
public interface IApiKeyAuthenticationService
{
    /// <summary>
    /// Validates a given api key.
    /// </summary>
    /// <param name="context">The context for the validation, where the api key that needs validation also can be found through <see cref="ValidationContext.ApiKey"/></param>
    /// <returns>A non-null <see cref="ClaimsPrincipal"/> indicating success or null indicating failure</returns>
    Task<ClaimsPrincipal?> ValidateAsync(ValidationContext context);
}
