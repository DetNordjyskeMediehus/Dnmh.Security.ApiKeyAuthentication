using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Dnmh.Security.ApiKeyAuthentication.Context;

/// <summary>
/// The context associated with validiting the api key.
/// </summary>
public class ValidationContext : BaseContext<ApiKeyAuthenticationOptions>
{
    /// <summary>
    /// The api key that needs validation
    /// </summary>
    public string ApiKey { get; }

    /// <inheritdoc/>
    public ValidationContext(HttpContext context, AuthenticationScheme scheme, ApiKeyAuthenticationOptions options, string apiKey)
        : base(context, scheme, options)
    {
        ApiKey = apiKey;
    }
}
