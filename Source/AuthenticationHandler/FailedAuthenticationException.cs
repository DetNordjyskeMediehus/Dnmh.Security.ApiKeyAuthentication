namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

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
