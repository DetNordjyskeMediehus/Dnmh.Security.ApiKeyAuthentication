namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

/// <summary>
/// Extension methods for <see cref="ISet{Task}"/> with type <see cref="Key"/>
/// </summary>
public static class KeyHashSetExtensions
{
    /// <summary>
    /// Adds a new <see cref="Key"/> to the set
    /// </summary>
    public static bool Add(this ISet<Key> set, string value, bool isCaseSensitive = true) =>
        set.Add(new Key(value, isCaseSensitive));
}
