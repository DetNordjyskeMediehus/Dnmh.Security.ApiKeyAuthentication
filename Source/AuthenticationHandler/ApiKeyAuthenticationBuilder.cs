using Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

/// <summary>
/// TODO: fix summary
/// Specialized <see cref="AuthenticationBuilder"/> which enables Swagger authorization to be added.
/// </summary>
public class ApiKeyAuthenticationBuilder : AuthenticationBuilder
{
    private readonly string _authenticationScheme;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="authenticationScheme">The authentication scheme name for this builder</param>
    /// <param name="services">The services</param>
    public ApiKeyAuthenticationBuilder(string authenticationScheme, IServiceCollection services) : base(services)
    {
        _authenticationScheme = authenticationScheme;
    }
}
