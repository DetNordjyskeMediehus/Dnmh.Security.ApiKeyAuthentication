using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DNMH.Security.ApiKeyAuthentication;

/// <summary>
/// Specialized <see cref="AuthenticationBuilder"/> which enables Swagger authorization to be added.
/// </summary>
/// <remarks>
/// Creates a new instance.
/// </remarks>
/// <param name="authenticationScheme">The authentication scheme name for this builder.</param>
/// <param name="services">The service collection.</param>
public class ApiKeyAuthenticationBuilder(string authenticationScheme, IServiceCollection services) : AuthenticationBuilder(services)
{
    /// <summary>
    /// The authentication scheme name for this builder.
    /// </summary>
    public string AuthenticationScheme { get; } = authenticationScheme;
}
