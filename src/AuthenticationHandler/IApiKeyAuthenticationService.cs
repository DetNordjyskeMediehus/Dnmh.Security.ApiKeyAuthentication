using System.Security.Claims;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler
{
    /// <summary>
    /// Interface for the api key authentication service
    /// </summary>
    public interface IApiKeyAuthenticationService
    {
        /// <summary>
        /// Validates a given api key.
        /// </summary>
        /// <param name="apiKey">The api key to validate</param>
        /// <returns>A non-null <see cref="ClaimsPrincipal"/> indicating success or null indicating failure</returns>
        Task<ClaimsPrincipal?> ValidateAsync(string apiKey);
    }
}
