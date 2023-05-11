using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler.Context;

/// <summary>
/// The context associated with authentication success
/// </summary>
public class AuthenticationSuccessContext : ResultContext<ApiKeyAuthenticationOptions>
{
    /// <summary>
    /// The claims principle associated with the authentication success
    /// </summary>
    public ClaimsPrincipal ClaimsPrincipal { get; }

    /// <inheritdoc/>
    public AuthenticationSuccessContext(HttpContext context, AuthenticationScheme scheme, ApiKeyAuthenticationOptions options, ClaimsPrincipal claimsPrincipal)
        : base(context, scheme, options)
    {
        ClaimsPrincipal = claimsPrincipal;
    }
}
