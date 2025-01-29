using FluentAssertions;

namespace Dnmh.Security.ApiKeyAuthentication.Tests;

public class KeyTest
{
    [Fact]
    public void KeyEqualityTest()
    {
        // Arrange
        var key1 = new Key("key", true);
        var key2 = new Key("key", true);

        // Act & Assert
        key1.Should().Be(key2);
    }

    [Fact]
    public void KeyInequalityTest()
    {
        // Arrange
        var key1 = new Key("key", true);
        var key2 = new Key("KEY", true);

        // Act & Assert
        key1.Should().NotBe(key2);
    }

    [Fact]
    public void KeyCaseInsensitiveEqualityTest()
    {
        // Arrange
        var key1 = new Key("key", true);
        var key2 = new Key("KEY", false);

        // Act & Assert
        key1.Should().Be(key2);
    }

    [Fact]
    public void KeyCaseInsensitiveEqualityReverseTest()
    {
        // Arrange
        var key1 = new Key("key", false);
        var key2 = new Key("KEY", true);

        // Act & Assert
        key1.Should().Be(key2);
    }

    [Fact]
    public void KeyHashSetAddSameKeyTest()
    {
        // Arrange
        var key1 = new Key("key", true);
        var key2 = new Key("key", true);

        // Act
        var set = new HashSet<Key>() { key1 };
        var result = set.Add(key2);

        // Assert
        result.Should().BeFalse();
        set.Should().ContainSingle();
    }

    [Fact]
    public void KeyHashSetAddDifferentKeyTest()
    {
        // Arrange
        var key1 = new Key("key", true);
        var key2 = new Key("KEY", true);

        // Act
        var set = new HashSet<Key>() { key1 };
        var result = set.Add(key2);

        // Assert
        result.Should().BeTrue();
        set.Should().HaveCount(2);
    }

    [Fact]
    public void KeyHashSetAddSameCaseInsensitiveKeyTest()
    {
        // Arrange
        var key1 = new Key("key", true);
        var key2 = new Key("KEY", false);

        // Act
        var set = new HashSet<Key>() { key1 };
        var result = set.Add(key2);

        // Assert
        result.Should().BeFalse();
        set.Should().ContainSingle();
    }

    [Fact]
    public void KeyHashSetAddSameCaseInsensitiveKeyReverseTest()
    {
        // Arrange
        var key1 = new Key("key", false);
        var key2 = new Key("KEY", true);

        // Act
        var set = new HashSet<Key>() { key1 };
        var result = set.Add(key2);

        // Assert
        result.Should().BeFalse();
        set.Should().ContainSingle();
    }
}
