using Microsoft.AspNetCore.Authentication;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

public static partial class ApiKeyAuthenticationExtensions
{
    /// <summary>
    /// Adds a given authentication scheme name and registers the <see cref="ApiKeyAuthenticationHandler"/> as the authentication handler 
    /// with a given valid api key.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/></param>
    /// <param name="authenticationScheme">The authentication scheme name</param>
    /// <param name="validApiKey">The valid api key that the api keys provided in the requests are validated against.</param>
    /// <param name="configureOptions">The action used to configure options</param>
    public static ApiKeyAuthenticationBuilder AddApiKeyAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string validApiKey, Action<ApiKeyAuthenticationOptions> configureOptions) =>
        builder.AddApiKeyAuthentication(authenticationScheme, _ => validApiKey, configureOptions);

    /// <summary>
    /// Adds a given authentication scheme name and registers the <see cref="ApiKeyAuthenticationHandler"/> as the authentication handler 
    /// with a function to provide a valid api key.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/></param>
    /// <param name="authenticationScheme">The authentication scheme name</param>
    /// <param name="validApiKeyFunction">A function that returns a valid api key. The valid api key is validated against the api keys provided in the requests.</param>
    /// <param name="configureOptions">The action used to configure options</param>
    public static ApiKeyAuthenticationBuilder AddApiKeyAuthentication(this AuthenticationBuilder builder, string authenticationScheme, Func<IServiceProvider, string> validApiKeyFunction, Action<ApiKeyAuthenticationOptions> configureOptions) =>
        builder.AddApiKeyAuthentication(authenticationScheme, configureOptions, sp => new SimpleApiKeyAuthenticationService(validApiKeyFunction?.Invoke(sp) ?? throw new ArgumentNullException(nameof(validApiKeyFunction))));
}
