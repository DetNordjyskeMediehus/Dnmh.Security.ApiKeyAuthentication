using Dnmh.Security.ApiKeyAuthentication.Internal;

namespace Dnmh.Security.ApiKeyAuthentication.Internal;

/// <summary>
/// Extension methods for <see cref="Key"/>
/// </summary>
internal static class KeyExtensions
{
    /// <summary>
    /// Intersects an <see cref="IEnumerable{T}"/> with type <see cref="Key"/> and an <see cref="IEnumerable{T}"/> with type <see cref="string"/> using each key's <see cref="Key.StringComparer"/>
    /// </summary>
    public static IEnumerable<Key> Intersect(this IEnumerable<Key> first, IEnumerable<string> second)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));

        return first.Where(x => second.Contains(x.Name, x.StringComparer));
    }

    /// <summary>
    /// Determines if an <see cref="IEnumerable{T}"/> with type <see cref="Key"/> contains the given <paramref name="value"/> using each key's <see cref="Key.StringComparer"/>
    /// </summary>
    public static bool Contains(this IEnumerable<Key> keys, string value)
    {
        ArgumentNullException.ThrowIfNull(keys, nameof(keys));

        foreach (var key in keys)
        {
            if (key.StringComparer.Equals(key.Name, value))
            {
                return true;
            }
        }
        return false;
    }
}
