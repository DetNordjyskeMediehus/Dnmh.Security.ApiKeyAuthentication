namespace DNMH.Security.ApiKeyAuthentication.Internal;

/// <summary>
/// Extensions methods for <see cref="ApiKeyAuthenticationOptions"/>
/// </summary>
internal static class ApiKeyAuthenticationOptionsExtensions
{
    /// <summary>
    /// Initializes default values for <see cref="ApiKeyAuthenticationOptions.HeaderKeys"/> and <see cref="ApiKeyAuthenticationOptions.QueryKeys"/>.
    /// </summary>
    public static void InitializeDefaultValues(this ApiKeyAuthenticationOptions options)
    {
        if (options.AllowApiKeyInRequestHeader && options.HeaderKeys != null && !options.HeaderKeys.Any())
        {
            options.HeaderKeys.Add(ApiKeyAuthenticationOptions.DefaultHeaderKey);
        }
        if (options.AllowApiKeyInQuery && options.QueryKeys != null && !options.QueryKeys.Any())
        {
            options.QueryKeys.Add(ApiKeyAuthenticationOptions.DefaultQueryKey);
        }
    }
}
