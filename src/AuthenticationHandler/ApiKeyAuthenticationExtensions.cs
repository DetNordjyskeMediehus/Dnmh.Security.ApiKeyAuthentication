using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler
{
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

    public class ApiKeySchemeSwaggerOptions
    {
        public string AuthenticationScheme { get; set; }
        public string SwaggerDescription { get; set; }
    }

    public class ApiKeyAuthenticationBuilder : AuthenticationBuilder
    {
        private readonly string _authenticationScheme;

        public ApiKeyAuthenticationBuilder(string authenticationScheme, IServiceCollection services) : base(services)
        {
            _authenticationScheme = authenticationScheme;
        }
        public ApiKeyAuthenticationBuilder AddSwaggerAuthorization(string description)
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

    class PostConfigureSwaggerAuthorization : IPostConfigureOptions<SwaggerGenOptions>
    {
        private readonly ApiKeySchemeSwaggerOptions _swaggerSchemeOptions;
        private readonly ApiKeyAuthenticationOptions _authenticationOptions;

        public PostConfigureSwaggerAuthorization(string authenticationScheme, IOptionsMonitor<ApiKeySchemeSwaggerOptions> schemeOptions, IOptionsMonitor<ApiKeyAuthenticationOptions> options)
        {
            _swaggerSchemeOptions = schemeOptions.Get(authenticationScheme);
            _authenticationOptions = options.Get(authenticationScheme);
        }

        public void PostConfigure(string? name, SwaggerGenOptions options)
        {
            var parameterLocation = _authenticationOptions.AllowApiKeyInQuery ? ParameterLocation.Query : ParameterLocation.Header;
            var keyName = parameterLocation == ParameterLocation.Query ? _authenticationOptions.QueryKey : _authenticationOptions.HeaderKey;
            var scheme = _authenticationOptions.UseSchemeNameInAuthorizationHeader ? _swaggerSchemeOptions.AuthenticationScheme : _authenticationOptions.AuthorizationSchemeInHeader;
            // Setup the "Authorize" button in the Swagger UI
            options.AddSecurityDefinition(keyName, new OpenApiSecurityScheme
            {
                In = parameterLocation,
                Description = _swaggerSchemeOptions.SwaggerDescription,
                Name = keyName,
                Type = SecuritySchemeType.ApiKey,
                Scheme = scheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = keyName
                        },
                    },
                    Array.Empty<string>()
                }
            });
        }
    }
}
