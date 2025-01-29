/* Unmerged change from project 'ApiKeyAuthentication (net9.0)'
Before:
namespace DNMH.Security.ApiKeyAuthentication.AuthenticationHandler;
After:
using Dnmh;
using DNMH.Security;
using DNMH.Security.ApiKeyAuthentication;
using DNMH.Security.ApiKeyAuthentication;
using DNMH.Security.ApiKeyAuthentication.AuthenticationHandler;

namespace DNMH.Security.ApiKeyAuthentication;
*/
namespace DNMH.Security.ApiKeyAuthentication;

/// <summary>
/// A key with a string value and a case in-/sensitive boolean value
/// </summary>
public class Key : IEquatable<Key>
{
    /// <summary>
    /// The name of the key
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Indicates if the key is case sensitive
    /// </summary>
    public bool IsCaseSensitive { get; }

    /// <summary>
    /// <see cref="System.StringComparer"/> based on <see cref="IsCaseSensitive"/>
    /// </summary>
    public StringComparer StringComparer { get; }

    /// <summary>
    /// Creates a new instance
    /// </summary>
    /// <param name="name">The name of the key</param>
    /// <param name="isCaseSensitive">Indicates wether or not the key is case sensitive or not</param>
    public Key(string name, bool isCaseSensitive)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        Name = name;
        IsCaseSensitive = isCaseSensitive;
        StringComparer = StringComparer.FromComparison(IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Key);

    /// <inheritdoc/>
    public bool Equals(Key? other)
    {
        if (other is null)
        {
            return false;
        }

        // Optimization for a common success case.
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        // If run-time types are not exactly the same, return false.
        if (GetType() != other.GetType())
        {
            return false;
        }

        // Return true if the fields match.
        // If either of the StringComparers return true, then they are equal (one might be case sensitive, and the other not - the non-case sensitive then overrules)
        return StringComparer.Equals(Name, other.Name) || other.StringComparer.Equals(Name, other.Name);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Name); // using case-insensitive HashCode here, in order to trigger an equality test (using .Equals) on hash collisions.

    /// <summary>
    /// Tests equality
    /// </summary>
    public static bool operator ==(Key lhs, Key rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }

            // Only the left side is null.
            return false;
        }
        // Equals handles case of null on right side.
        return lhs.Equals(rhs);
    }

    /// <summary>
    /// Tests inequality
    /// </summary>
    public static bool operator !=(Key lhs, Key rhs) => !(lhs == rhs);
}
