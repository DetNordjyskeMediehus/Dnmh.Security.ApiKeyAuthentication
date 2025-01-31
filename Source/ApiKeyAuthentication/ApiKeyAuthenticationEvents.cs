using System.Security.Claims;
using DNMH.Security.ApiKeyAuthentication.Context;

namespace DNMH.Security.ApiKeyAuthentication;

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
