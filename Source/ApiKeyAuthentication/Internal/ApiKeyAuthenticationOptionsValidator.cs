namespace DNMH.Security.ApiKeyAuthentication.Internal;

/// <summary>
/// Validator for <see cref="ApiKeyAuthenticationOptions"/>
/// </summary>
internal class ApiKeyAuthenticationOptionsValidator
{
    public void ValidateAndThrow(ApiKeyAuthenticationOptions options)
    {
        if (!options.AllowApiKeyInQuery && !options.AllowApiKeyInRequestHeader)
        {
            throw new ArgumentException($"Both {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInQuery)} and {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInRequestHeader)} cannot be false");
        }

        if (options.UseAuthorizationHeaderKey && !options.AllowApiKeyInRequestHeader)
        {
            throw new ArgumentException($"Setting {nameof(ApiKeyAuthenticationOptions.UseAuthorizationHeaderKey)} to true requires {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInRequestHeader)} to also be true");
        }

        if (options.UseSchemeNameInAuthorizationHeader && (!options.AllowApiKeyInRequestHeader || !options.UseAuthorizationHeaderKey))
        {
            throw new ArgumentException($"Setting {nameof(ApiKeyAuthenticationOptions.UseSchemeNameInAuthorizationHeader)} to true requires {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInRequestHeader)} and {nameof(ApiKeyAuthenticationOptions.UseAuthorizationHeaderKey)} to also be true");
        }

        if (options.AllowApiKeyInRequestHeader && options.HeaderKeys == null)
        {
            throw new ArgumentException($"{nameof(ApiKeyAuthenticationOptions.HeaderKeys)} must be non-empty when {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInRequestHeader)} is true");
        }

        if (options.AllowApiKeyInQuery && options.QueryKeys == null)
        {
            throw new ArgumentException($"{nameof(ApiKeyAuthenticationOptions.QueryKeys)} must be non-empty when {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInQuery)} is true");
        }

        if (options.UseAuthorizationHeaderKey && !options.UseSchemeNameInAuthorizationHeader && string.IsNullOrEmpty(options.AuthorizationSchemeInHeader))
        {
            throw new ArgumentException($"{nameof(ApiKeyAuthenticationOptions.AuthorizationSchemeInHeader)} must be non-empty when {nameof(ApiKeyAuthenticationOptions.UseAuthorizationHeaderKey)} is true and {nameof(ApiKeyAuthenticationOptions.UseSchemeNameInAuthorizationHeader)} is false");
        }
    }
}
