namespace DNMH.Security.ApiKeyAuthentication;

/// <summary>
/// Exception for when authentication fails.
/// </summary>
public class FailedAuthenticationException : Exception
{
    /// <inheritdoc/>
    public FailedAuthenticationException(string message) : base(message)
    {
    }
}
