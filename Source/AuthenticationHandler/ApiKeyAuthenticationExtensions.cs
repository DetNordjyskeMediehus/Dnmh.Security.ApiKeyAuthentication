using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

/// <summary>
/// Extension methods for the <see cref="AuthenticationBuilder"/>
/// </summary>
public static partial class ApiKeyAuthenticationExtensions
{
    /// <summary>
    /// Adds a given authentication scheme name and registers the <see cref="ApiKeyAuthenticationHandler"/> as the authentication handler.
    /// </summary>
    /// <typeparam name="TAuthService">The implementing class of interface <see cref="IApiKeyAuthenticationService"/></typeparam>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/></param>
    /// <param name="authenticationScheme">The authentication scheme name</param>
    /// <param name="configureOptions">The action used to configure options</param>
    /// <param name="serviceImplementationFactory">Optional implementation factory for the registration of <typeparamref name="TAuthService"/></param>
    public static ApiKeyAuthenticationBuilder AddApiKeyAuthentication<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyAuthenticationOptions> configureOptions, Func<IServiceProvider, TAuthService>? serviceImplementationFactory = null)
        where TAuthService : class, IApiKeyAuthenticationService
    {
        if (serviceImplementationFactory == null)
        {
            builder.Services.AddTransient<IApiKeyAuthenticationService, TAuthService>();
        }
        else
        {
            builder.Services.AddTransient<IApiKeyAuthenticationService, TAuthService>(serviceImplementationFactory);
        }

        builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
            authenticationScheme, configureOptions);

        return new ApiKeyAuthenticationBuilder(authenticationScheme, builder.Services);
    }
}
