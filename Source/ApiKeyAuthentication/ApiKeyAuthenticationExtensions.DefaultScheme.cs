using Microsoft.AspNetCore.Authentication;

namespace DNMH.Security.ApiKeyAuthentication;

public static partial class ApiKeyAuthenticationExtensions
{
    /// <summary>
    /// Adds a default authentication scheme named <c>"ApiKey"</c> and registers the <see cref="ApiKeyAuthenticationHandler"/> as the authentication handler 
    /// with a given valid api key.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/></param>
    /// <param name="validApiKey">The valid api key that the api keys provided in the requests are validated against.</param>
    public static ApiKeyAuthenticationBuilder AddApiKeyAuthentication(this AuthenticationBuilder builder, string validApiKey) =>
        builder.AddApiKeyAuthentication(_ => validApiKey);

    /// <summary>
    /// Adds a default authentication scheme named <c>"ApiKey"</c> and registers the <see cref="ApiKeyAuthenticationHandler"/> as the authentication handler 
    /// with a function to provide a valid api key.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/></param>
    /// <param name="validApiKeyFunction">A function that returns a valid api key. The valid api key is validated against the api keys provided in the requests.</param>
    public static ApiKeyAuthenticationBuilder AddApiKeyAuthentication(this AuthenticationBuilder builder, Func<IServiceProvider, string> validApiKeyFunction) =>
        builder.AddApiKeyAuthentication(sp => new SimpleApiKeyAuthenticationService(validApiKeyFunction?.Invoke(sp) ?? throw new ArgumentNullException(nameof(validApiKeyFunction))));

    /// <summary>
    /// Adds a default authentication scheme named <c>"ApiKey"</c> and registers the <see cref="ApiKeyAuthenticationHandler"/> as the authentication handler.
    /// </summary>
    /// <typeparam name="TAuthService">The implementing class of interface <see cref="IApiKeyAuthenticationService"/></typeparam>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/></param>
    /// <param name="serviceImplementationFactory">Optional implementation factory for the registration of <typeparamref name="TAuthService"/></param>
    public static ApiKeyAuthenticationBuilder AddApiKeyAuthentication<TAuthService>(this AuthenticationBuilder builder, Func<IServiceProvider, TAuthService>? serviceImplementationFactory = null)
        where TAuthService : class, IApiKeyAuthenticationService =>
        builder.AddApiKeyAuthentication("ApiKey", _ => { }, serviceImplementationFactory);
}
