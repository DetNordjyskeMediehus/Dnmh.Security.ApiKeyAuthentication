using DNMH.Security.ApiKeyAuthentication.Swagger.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DNMH.Security.ApiKeyAuthentication.Swagger;

/// <summary>
/// Extension methods for <see cref="ApiKeyAuthenticationBuilder"/>.
/// </summary>
public static class ApiKeyAuthenticationBuilderExtensions
{
    /// <summary>
    /// Adds swagger authorization to the swagger UI generator.
    /// </summary>
    /// <param name="builder">The <see cref="ApiKeyAuthenticationBuilder"/> that this method extends.</param>
    /// <param name="description">The description for this authentication scheme.</param>
    public static ApiKeyAuthenticationBuilder AddSwaggerAuthorization(this ApiKeyAuthenticationBuilder builder, string description = "Please add a valid api key")
    {
        builder.Services.Configure<ApiKeySchemeSwaggerOptions>(builder.AuthenticationScheme, options =>
        {
            options.AuthenticationScheme = builder.AuthenticationScheme;
            options.SwaggerDescription = description;
        });
        builder.Services.AddSingleton<IPostConfigureOptions<SwaggerGenOptions>, PostConfigureSwaggerAuthorization>(sp =>
            new PostConfigureSwaggerAuthorization(builder.AuthenticationScheme,
                sp.GetRequiredService<IOptionsMonitor<ApiKeySchemeSwaggerOptions>>(),
                sp.GetRequiredService<IOptionsMonitor<ApiKeyAuthenticationOptions>>()));

        return builder;
    }
}
