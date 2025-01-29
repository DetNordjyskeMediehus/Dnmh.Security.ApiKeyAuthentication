
/* Unmerged change from project 'ApiKeyAuthentication (net9.0)'
Before:
using Dnmh.Security.ApiKeyAuthentication.Internal;
After:
using Dnmh;
using Dnmh.Security;
using Dnmh.Security.ApiKeyAuthentication;
using Dnmh.Security.ApiKeyAuthentication;
using Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;
using Dnmh.Security.ApiKeyAuthentication.Internal;
*/
using Dnmh.Security.ApiKeyAuthentication.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dnmh.Security.ApiKeyAuthentication;

/// <summary>
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

    /// <summary>
    /// Adds swagger authorization to the swagger generator.
    /// </summary>
    /// <param name="description">The description for this authentication scheme</param>
    public ApiKeyAuthenticationBuilder AddSwaggerAuthorization(string description = "Please add a valid api key")
    {
        Services.Configure<ApiKeySchemeSwaggerOptions>(_authenticationScheme, options =>
        {
            options.AuthenticationScheme = _authenticationScheme;
            options.SwaggerDescription = description;
        });
        Services.AddSingleton<IPostConfigureOptions<SwaggerGenOptions>, PostConfigureSwaggerAuthorization>(sp =>
            new PostConfigureSwaggerAuthorization(_authenticationScheme,
                sp.GetRequiredService<IOptionsMonitor<ApiKeySchemeSwaggerOptions>>(),
                sp.GetRequiredService<IOptionsMonitor<ApiKeyAuthenticationOptions>>()));

        return this;
    }
}
