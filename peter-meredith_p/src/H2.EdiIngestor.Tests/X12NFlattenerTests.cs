using FluentAssertions;
using H2.EdiIngestor.Data;

namespace H2.EdiIngestor.Tests;

/// <summary>
/// Unit tests for <see cref="X12NFlattener"/>.
/// Tests parameter validation, return types, and async enumerable behavior.
/// </summary>
[TestClass]
public class X12NFlattenerTests
{
    private X12NFlattener _flattener = null!;

    [TestInitialize]
    public void Setup()
    {
        _flattener = new X12NFlattener();
    }

    #region Parameter Validation Tests

    [TestMethod]
    public async Task GetFlattenedClaimFiles_WithNullFileName_ThrowsArgumentException()
    {
        // Act
        var act = async () =>
        {
            await foreach (
                var _ in _flattener.GetFlattenedClaimFiles(null!, CancellationToken.None)
            ) { }
        };

        // Assert
        var exception = await act.Should().ThrowAsync<ArgumentException>();
        exception.And.ParamName.Should().Be("fileName");
    }

    [TestMethod]
    public async Task GetFlattenedClaimFiles_WithEmptyFileName_ThrowsArgumentException()
    {
        // Act
        var act = async () =>
        {
            await foreach (
                var _ in _flattener.GetFlattenedClaimFiles(string.Empty, CancellationToken.None)
            ) { }
        };

        // Assert
        var exception = await act.Should().ThrowAsync<ArgumentException>();
        exception.And.ParamName.Should().Be("fileName");
    }

    [TestMethod]
    public async Task GetFlattenedClaimFiles_WithWhitespaceFileName_ThrowsArgumentException()
    {
        // Act
        var act = async () =>
        {
            await foreach (
                var _ in _flattener.GetFlattenedClaimFiles("   ", CancellationToken.None)
            ) { }
        };

        // Assert
        var exception = await act.Should().ThrowAsync<ArgumentException>();
        exception.And.ParamName.Should().Be("fileName");
    }

    #endregion

    #region File Operation Tests

    [TestMethod]
    public async Task GetFlattenedClaimFiles_WithNonexistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var nonexistentFile = Path.Combine(
            Path.GetTempPath(),
            "nonexistent_" + Guid.NewGuid() + ".edi"
        );

        // Act
        var act = async () =>
        {
            await foreach (
                var _ in _flattener.GetFlattenedClaimFiles(nonexistentFile, CancellationToken.None)
            ) { }
        };

        // Assert
        await act.Should().ThrowAsync<FileNotFoundException>();
    }

    #endregion

    #region Return Type Tests

    [TestMethod]
    public void GetFlattenedClaimFiles_ReturnsIAsyncEnumerable()
    {
        // Arrange
        var fileName = "test.edi";

        // Act
        var result = _flattener.GetFlattenedClaimFiles(fileName, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IAsyncEnumerable<FlattenedClaimFile>>();
    }

    [TestMethod]
    public void GetFlattenedClaimFiles_ReturnsDeferredExecution()
    {
        // Arrange
        var nonexistentFile = Path.Combine(Path.GetTempPath(), "fake_" + Guid.NewGuid() + ".edi");

        // Act - Creating the enumerable itself should not throw
        var result = _flattener.GetFlattenedClaimFiles(nonexistentFile, CancellationToken.None);

        // Assert - The enumerable is created without error (deferred execution)
        result.Should().NotBeNull();
    }

    #endregion
}
