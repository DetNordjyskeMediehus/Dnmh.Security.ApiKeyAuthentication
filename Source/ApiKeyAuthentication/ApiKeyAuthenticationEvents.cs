using System.Security.Claims;
using Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler.Context;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

/// <summary>
/// Api Key authentication events, used by <see cref="ApiKeyAuthenticationHandler"/>.
/// </summary>
public class ApiKeyAuthenticationEvents
{
    /// <summary>
    /// Event that is raised when authentication fails, with the exception as parameter.
    /// </summary>
    public Func<AuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } = _ => Task.CompletedTask;

    /// <summary>
    /// Event that is raised when authentication succeeds, with a <see cref="ClaimsPrincipal"/> as parameter.
    /// </summary>
    public Func<AuthenticationSuccessContext, Task> OnAuthenticationSuccess { get; set; } = _ => Task.CompletedTask;

}
