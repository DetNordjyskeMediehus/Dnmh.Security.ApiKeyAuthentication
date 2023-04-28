using FluentValidation;
using Microsoft.AspNetCore.Authentication;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler
{
    /// <summary>
    /// Scheme option for api key authentication.
    /// </summary>
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        /// <inheritdoc/>
        public new ApiKeyAuthenticationEvents Events
        {
            get => (ApiKeyAuthenticationEvents)base.Events;
            set => base.Events = value;
        }

        /// <summary>
        /// Allows the api key to be provided as a query parameter. 
        /// Default is <c>true</c>
        /// </summary>
        public bool AllowApiKeyInQuery { get; set; } = true;

        /// <summary>
        /// Allows the api key to be provided as a request header. 
        /// Default is <c>true</c>
        /// </summary>
        public bool AllowApiKeyInRequestHeader { get; set; } = true;

        /// <summary>
        /// Get or set the key used for the api key, when provided as a query parameter. 
        /// Default is <c>apikey</c>
        /// </summary>
        /// <example>
        /// Example of header:
        /// <code>&lt;HeaderKey&gt;: abcdef12345</code>
        /// </example>
        public string QueryKey { get; set; } = "apikey";

        /// <summary>
        /// Get or set the key used for the api key, when provided as a request header parameter. 
        /// Default is <c>X-API-KEY</c>
        /// </summary>
        /// <example>
        /// Example of header, if the <see cref="HeaderKey"/> is <c>X-API-KEY</c>:
        /// <code>X-API-KEY: abcdef12345</code>
        /// </example>
        public string HeaderKey { get; set; } = "X-API-KEY";

        /// <summary>
        /// If <c>true</c>, then the standard <c>Authorization</c> header key is used in combination with <see cref="AuthorizationSchemeInHeader"/>. 
        /// Requires that <see cref="AllowApiKeyInRequestHeader"/> is <c>true</c>.
        /// If this is set to <c>true</c>, then the value in <see cref="HeaderKey"/> is ignored.
        /// Default is <c>false</c>
        /// </summary>
        /// <example>
        /// Example, if set to <c>true</c> and the <see cref="AuthorizationSchemeInHeader"/> is set to <c>Bearer</c>:
        /// <code>Authorization: Bearer abcdef12345</code>
        /// </example>
        public bool UseAuthorizationHeaderKey { get; set; } = false;

        /// <summary>
        /// If <c>true</c>, the authentication scheme name given in <see cref="ApiKeyAuthenticationExtensions.AddApiKeyAuthentication{TAuthService}(AuthenticationBuilder, string)"/> is used as Authorization header scheme.
        /// Requires that <see cref="AllowApiKeyInRequestHeader"/> and <see cref="UseAuthorizationHeaderKey"/> are both <c>true</c>.
        /// If this is set to <c>true</c>, then the value in <see cref="AuthorizationSchemeInHeader"/> is ignored.
        /// Default is <c>false</c>
        /// </summary>
        /// <example>
        /// Example, if set scheme is given as <c>ApiKey</c>:
        /// <code>Authorization: ApiKey abcdef12345</code>
        /// </example>
        public bool UseSchemeNameInAuthorizationHeader { get; set; } = false;

        /// <summary>
        /// The authorization scheme used when <see cref="UseAuthorizationHeaderKey"/> is set to <c>true</c>.
        /// Default is <c>Bearer</c>.
        /// </summary>
        public string AuthorizationSchemeInHeader { get; set; } = "Bearer";

        /// <inheritdoc/>
        public override void Validate()
        {
            base.Validate();
            var validator = new ApiKeyAuthenticationOptionsValidator();
            validator.ValidateAndThrow(this);
        }
    }
}
