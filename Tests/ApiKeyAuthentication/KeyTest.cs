using Shouldly;

namespace DNMH.Security.ApiKeyAuthentication.Tests;

public class KeyTest
{
    [Fact]
    public void KeyEqualityTest()
    {
        // Arrange
        var key1 = new Key("key", true);
        var key2 = new Key("key", true);

        // Act & Assert
        key1.ShouldBe(key2);
    }

    [Fact]
    public void KeyInequalityTest()
    {
        // Arrange
        var key1 = new Key("key", true);
        var key2 = new Key("KEY", true);

        // Act & Assert
        key1.ShouldNotBe(key2);
    }

    [Fact]
    public void KeyCaseInsensitiveEqualityTest()
    {
        // Arrange
        var key1 = new Key("key", true);
        var key2 = new Key("KEY", false);

        // Act & Assert
        key1.ShouldBe(key2);
    }

    [Fact]
    public void KeyCaseInsensitiveEqualityReverseTest()
    {
        // Arrange
        var key1 = new Key("key", false);
        var key2 = new Key("KEY", true);

        // Act & Assert
        key1.ShouldBe(key2);
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
        result.ShouldBeFalse();
        set.ShouldHaveSingleItem();
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
        result.ShouldBeTrue();
        set.ShouldContain(key1);
        set.ShouldContain(key2);
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
        result.ShouldBeFalse();
        set.ShouldHaveSingleItem();
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
        result.ShouldBeFalse();
        set.ShouldHaveSingleItem();
    }
}
