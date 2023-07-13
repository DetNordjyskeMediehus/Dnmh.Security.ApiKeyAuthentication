using Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler.Internal;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

/// <summary>
/// Scheme option for api key authentication.
/// </summary>
public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// The default value for <see cref="QueryKeys"/> if none are added.
    /// </summary>
    public static readonly Key DefaultQueryKey = new("apikey", true);

    /// <summary>
    /// The default value for <see cref="HeaderKeys"/> if none are added.
    /// </summary>
    public static readonly Key DefaultHeaderKey = new("X-API-KEY", true);

    /// <inheritdoc/>
    public new ApiKeyAuthenticationEvents? Events
    {
        get => (ApiKeyAuthenticationEvents?)base.Events;
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
    /// Get or set the allowed keys used for the api key, when provided as a query parameter. 
    /// If no keys are added and <see cref="AllowApiKeyInQuery"/> is <c>true</c>, then the value of <see cref="DefaultQueryKey"/> is added automatically to the set.
    /// </summary>
    /// <remarks>
    /// If more than one key in the query matches keys in the allowed set of <see cref="QueryKeys"/>, then all of the matched keys in the query are used for validation. 
    /// </remarks>
    /// <example>
    /// Example of query:
    /// <code>&lt;QueryKey&gt;=abcdef12345</code>
    /// </example>
    public ISet<Key> QueryKeys { get; set; } = new HashSet<Key>();

    /// <summary>
    /// Get or set the allowed keys used for the api key, when provided as a request header parameter. 
    /// If no keys are added and <see cref="AllowApiKeyInRequestHeader"/> is <c>true</c>, then the value of <see cref="DefaultHeaderKey"/> is added automatically to the set.
    /// </summary>
    /// <remarks>
    /// If more than one key in the headers matches keys in the allowed set of <see cref="HeaderKeys"/>, then all of the matched keys in the headers are used for validation. 
    /// </remarks>
    /// <example>
    /// Example of header, if the <see cref="HeaderKeys"/> contains <c>X-API-KEY</c>:
    /// <code>X-API-KEY: abcdef12345</code>
    /// </example>
    public ISet<Key> HeaderKeys { get; set; } = new HashSet<Key>();

    /// <summary>
    /// If <c>true</c>, then the standard <c>Authorization</c> header key is used in combination with <see cref="AuthorizationSchemeInHeader"/>. 
    /// Requires that <see cref="AllowApiKeyInRequestHeader"/> is <c>true</c>.
    /// If this is set to <c>true</c>, then the values in <see cref="HeaderKeys"/> is ignored.
    /// Default is <c>false</c>
    /// </summary>
    /// <example>
    /// Example, if set to <c>true</c> and the <see cref="AuthorizationSchemeInHeader"/> is set to <c>Bearer</c>:
    /// <code>Authorization: Bearer abcdef12345</code>
    /// </example>
    public bool UseAuthorizationHeaderKey { get; set; } = false;

    /// <summary>
    /// If <c>true</c>, the authentication scheme name given in <see cref="ApiKeyAuthenticationExtensions.AddApiKeyAuthentication{TAuthService}(AuthenticationBuilder, string, Action{ApiKeyAuthenticationOptions}, Func{IServiceProvider, TAuthService}?)"/> is used as Authorization header scheme.
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
