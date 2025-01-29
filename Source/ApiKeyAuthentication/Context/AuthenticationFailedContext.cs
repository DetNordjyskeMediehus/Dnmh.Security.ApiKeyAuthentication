using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler.Context;

/// <summary>
/// The context associated with failed authentication
/// </summary>
public class AuthenticationFailedContext : ResultContext<ApiKeyAuthenticationOptions>
{
    /// <summary>
    /// The optional exception that occured when authentication failed.
    /// </summary>
    public Exception? Exception { get; }

    /// <inheritdoc/>
    public AuthenticationFailedContext(HttpContext context, AuthenticationScheme scheme, ApiKeyAuthenticationOptions options, Exception? exception)
        : base(context, scheme, options)
    {
        Exception = exception;
    }
}
