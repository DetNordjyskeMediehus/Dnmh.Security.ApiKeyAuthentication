using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

/// <summary>
/// Extension methods for the <see cref="AuthenticationBuilder"/>
/// </summary>
public static class ApiKeyAuthenticationExtensions
{
    /// <summary>
    /// Adds a default authentication scheme named <c>"ApiKey"</c> and registers the <see cref="ApiKeyAuthenticationHandler"/> as the authentication handler.
    /// </summary>
    /// <typeparam name="TAuthService">The implementing class of interface <see cref="IApiKeyAuthenticationService"/></typeparam>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/></param>
    public static ApiKeyAuthenticationBuilder AddApiKeyAuthentication<TAuthService>(this AuthenticationBuilder builder)
        where TAuthService : class, IApiKeyAuthenticationService =>
        builder.AddApiKeyAuthentication<TAuthService>("ApiKey", _ => { });

    /// <summary>
    /// Adds a given authentication scheme name and registers the <see cref="ApiKeyAuthenticationHandler"/> as the authentication handler.
    /// </summary>
    /// <typeparam name="TAuthService">The implementing class of interface <see cref="IApiKeyAuthenticationService"/></typeparam>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/></param>
    /// <param name="authenticationScheme">The authentication scheme name</param>
    public static ApiKeyAuthenticationBuilder AddApiKeyAuthentication<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme)
        where TAuthService : class, IApiKeyAuthenticationService =>
        builder.AddApiKeyAuthentication<TAuthService>(authenticationScheme, _ => { });

    /// <summary>
    /// Adds a default authentication scheme named <c>"ApiKey"</c> and registers the <see cref="ApiKeyAuthenticationHandler"/> as the authentication handler.
    /// </summary>
    /// <typeparam name="TAuthService">The implementing class of interface <see cref="IApiKeyAuthenticationService"/></typeparam>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/></param>
    /// <param name="configureOptions">The action used to configure options</param>
    public static ApiKeyAuthenticationBuilder AddApiKeyAuthentication<TAuthService>(this AuthenticationBuilder builder, Action<ApiKeyAuthenticationOptions> configureOptions)
        where TAuthService : class, IApiKeyAuthenticationService =>
        builder.AddApiKeyAuthentication<TAuthService>("ApiKey", configureOptions);

    /// <summary>
    /// Adds a given authentication scheme name and registers the <see cref="ApiKeyAuthenticationHandler"/> as the authentication handler.
    /// </summary>
    /// <typeparam name="TAuthService">The implementing class of interface <see cref="IApiKeyAuthenticationService"/></typeparam>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/></param>
    /// <param name="authenticationScheme">The authentication scheme name</param>
    /// <param name="configureOptions">The action used to configure options</param>
    public static ApiKeyAuthenticationBuilder AddApiKeyAuthentication<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyAuthenticationOptions> configureOptions)
        where TAuthService : class, IApiKeyAuthenticationService
    {
        ArgumentNullException.ThrowIfNull(nameof(builder));
        ArgumentNullException.ThrowIfNull(nameof(authenticationScheme));
        ArgumentNullException.ThrowIfNull(nameof(configureOptions));

        builder.Services.AddTransient<IApiKeyAuthenticationService, TAuthService>();

        builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
            authenticationScheme, configureOptions);

        return new ApiKeyAuthenticationBuilder(authenticationScheme, builder.Services);
    }
}
