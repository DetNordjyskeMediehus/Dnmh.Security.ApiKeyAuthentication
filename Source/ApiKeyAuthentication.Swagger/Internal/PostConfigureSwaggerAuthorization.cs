using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DNMH.Security.ApiKeyAuthentication.Swagger.Internal
{
    /// <summary>
    /// An implementation of <see cref="IPostConfigureOptions{TOptions}"/> used to setup security definition and requirement 
    /// for Swagger based on <see cref="ApiKeySchemeSwaggerOptions"/> and <see cref="ApiKeyAuthenticationOptions"/>.
    /// </summary>
    internal class PostConfigureSwaggerAuthorization : IPostConfigureOptions<SwaggerGenOptions>
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
            var keyName = parameterLocation switch
            {
                ParameterLocation.Query => _authenticationOptions.QueryKeys.FirstOrDefault()?.Name ?? ApiKeyAuthenticationOptions.DefaultQueryKey.Name,
                ParameterLocation.Header => _authenticationOptions.HeaderKeys.FirstOrDefault()?.Name ?? ApiKeyAuthenticationOptions.DefaultHeaderKey.Name,
                _ => null
            };
            if (string.IsNullOrEmpty(keyName))
            {
                return;
            }
            var scheme = _authenticationOptions.UseSchemeNameInAuthorizationHeader ? _swaggerSchemeOptions.AuthenticationScheme : _authenticationOptions.AuthorizationSchemeInHeader;
            // Setup the security definition
            options.AddSecurityDefinition(keyName, new OpenApiSecurityScheme
            {
                In = parameterLocation,
                Description = _swaggerSchemeOptions.SwaggerDescription,
                Name = keyName,
                Type = SecuritySchemeType.ApiKey,
                Scheme = scheme
            });
            // Setup the security requirement
            options.AddSecurityRequirement(document =>
            {
                // Define the security scheme reference
                var securitySchemeReference = new OpenApiSecuritySchemeReference(keyName, document);

                // Define the security requirement
                return new OpenApiSecurityRequirement
                {
                    { securitySchemeReference, [] }
                };
            });
        }
    }
}
