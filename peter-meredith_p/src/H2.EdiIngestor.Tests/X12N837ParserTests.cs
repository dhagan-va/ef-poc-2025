using EdiFabric.Templates.Hipaa5010;
using FluentAssertions;
using H2.EdiIngestor.Data;
using Microsoft.Extensions.Logging;
using Moq;

namespace H2.EdiIngestor.Tests;

/// <summary>
/// Unit tests for X12N837Parser class.
/// Focuses on constructor validation, parameter validation, and static utility methods.
/// Complex EdiFabric object graph construction tests are omitted due to library coupling.
/// </summary>
[TestClass]
public class X12N837ParserTests
{
    private Mock<ILogger<X12N837Parser>> _mockLogger = null!;
    private Mock<IX12NFlattener> _mockFlattener = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockLogger = new Mock<ILogger<X12N837Parser>>();
        _mockFlattener = new Mock<IX12NFlattener>();
    }

    #region Constructor Tests

    /// <summary>
    /// Verifies that the constructor throws ArgumentNullException when logger is null.
    /// </summary>
    [TestMethod]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act
        Action act = () => new X12N837Parser(null!, _mockFlattener.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("logger");
    }

    /// <summary>
    /// Verifies that the constructor accepts null for x12NFlattener parameter.
    /// </summary>
    [TestMethod]
    public void Constructor_WithNullFlattener_DoesNotThrow()
    {
        // Arrange & Act
        Action act = () => new X12N837Parser(_mockLogger.Object, null!);

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Verifies that the constructor succeeds with valid parameters.
    /// </summary>
    [TestMethod]
    public void Constructor_WithValidParameters_Succeeds()
    {
        // Arrange & Act
        Action act = () => new X12N837Parser(_mockLogger.Object, _mockFlattener.Object);

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #region ReadFile Parameter Validation Tests

    /// <summary>
    /// Verifies that ReadFile throws ArgumentException when fileName is null.
    /// </summary>
    [TestMethod]
    public async Task ReadFile_WithNullFileName_ThrowsArgumentException()
    {
        // Arrange
        var parser = new X12N837Parser(_mockLogger.Object, _mockFlattener.Object);

        // Act
        Func<Task> act = async () =>
        {
            await foreach (var claim in parser.ReadFile(null!, CancellationToken.None))
            {
                // Should not reach here
            }
        };

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithParameterName("fileName");
    }

    /// <summary>
    /// Verifies that ReadFile throws ArgumentException when fileName is empty.
    /// </summary>
    [TestMethod]
    public async Task ReadFile_WithEmptyFileName_ThrowsArgumentException()
    {
        // Arrange
        var parser = new X12N837Parser(_mockLogger.Object, _mockFlattener.Object);

        // Act
        Func<Task> act = async () =>
        {
            await foreach (var claim in parser.ReadFile(string.Empty, CancellationToken.None))
            {
                // Should not reach here
            }
        };

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithParameterName("fileName");
    }

    /// <summary>
    /// Verifies that ReadFile throws ArgumentException when fileName is whitespace.
    /// </summary>
    [TestMethod]
    public async Task ReadFile_WithWhitespaceFileName_ThrowsArgumentException()
    {
        // Arrange
        var parser = new X12N837Parser(_mockLogger.Object, _mockFlattener.Object);

        // Act
        Func<Task> act = async () =>
        {
            await foreach (var claim in parser.ReadFile("   ", CancellationToken.None))
            {
                // Should not reach here
            }
        };

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithParameterName("fileName");
    }

    #endregion

    #region GetName Tests

    /// <summary>
    /// Verifies that GetName returns empty string when NM1 segment is null.
    /// </summary>
    [TestMethod]
    public void GetName_WithNullSegment_ReturnsEmptyString()
    {
        // Act
        var result = X12N837Parser.GetName(null);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that GetName returns organization name for entity type qualifier "2".
    /// </summary>
    [TestMethod]
    public void GetName_WithOrganizationEntityType_ReturnsOrganizationName()
    {
        // Arrange
        var nm1Segment = new NM1
        {
            EntityTypeQualifier_02 = "2",
            ResponseContactLastorOrganizationName_03 = "Acme Healthcare"
        };

        // Act
        var result = X12N837Parser.GetName(nm1Segment);

        // Assert
        result.Should().Be("Acme Healthcare");
    }

    /// <summary>
    /// Verifies that GetName returns formatted full name for person entity type.
    /// </summary>
    [TestMethod]
    public void GetName_WithPersonEntityType_ReturnsFormattedFullName()
    {
        // Arrange
        var nm1Segment = new NM1
        {
            EntityTypeQualifier_02 = "1",
            ResponseContactFirstName_04 = "John",
            ResponseContactLastorOrganizationName_03 = "Doe"
        };

        // Act
        var result = X12N837Parser.GetName(nm1Segment);

        // Assert
        result.Should().Be("John Doe");
    }

    /// <summary>
    /// Verifies that GetName handles null first name gracefully.
    /// </summary>
    [TestMethod]
    public void GetName_WithNullFirstName_ReturnsFormattedName()
    {
        // Arrange
        var nm1Segment = new NM1
        {
            EntityTypeQualifier_02 = "1",
            ResponseContactFirstName_04 = null,
            ResponseContactLastorOrganizationName_03 = "Doe"
        };

        // Act
        var result = X12N837Parser.GetName(nm1Segment);

        // Assert
        result.Should().Contain("Doe");
    }

    #endregion

    #region GetDate Tests

    /// <summary>
    /// Verifies that GetDate parses date correctly from DTP segment.
    /// </summary>
    [TestMethod]
    public void GetDate_WithValidDTPSegment_ReturnsCorrectDate()
    {
        // Arrange
        var dtpSegment = new DTP { DateTimePeriod_03 = "20231115000000" };

        // Act
        var result = X12N837Parser.GetDate(dtpSegment);

        // Assert
        result.Should().Be(new DateTime(2023, 11, 15));
    }

    /// <summary>
    /// Verifies that GetDate handles date at beginning of year.
    /// </summary>
    [TestMethod]
    public void GetDate_WithJanuaryFirst_ReturnsCorrectDate()
    {
        // Arrange
        var dtpSegment = new DTP { DateTimePeriod_03 = "20240101" };

        // Act
        var result = X12N837Parser.GetDate(dtpSegment);

        // Assert
        result.Should().Be(new DateTime(2024, 1, 1));
    }

    /// <summary>
    /// Verifies that GetDate handles leap year date.
    /// </summary>
    [TestMethod]
    public void GetDate_WithLeapYearDate_ReturnsCorrectDate()
    {
        // Arrange
        var dtpSegment = new DTP { DateTimePeriod_03 = "20240229" };

        // Act
        var result = X12N837Parser.GetDate(dtpSegment);

        // Assert
        result.Should().Be(new DateTime(2024, 2, 29));
    }

    /// <summary>
    /// Verifies that GetDate handles end of year date.
    /// </summary>
    [TestMethod]
    public void GetDate_WithDecember31_ReturnsCorrectDate()
    {
        // Arrange
        var dtpSegment = new DTP { DateTimePeriod_03 = "20231231235959" };

        // Act
        var result = X12N837Parser.GetDate(dtpSegment);

        // Assert
        result.Should().Be(new DateTime(2023, 12, 31));
    }

    #endregion

    #region GetDiag Tests - String Pointer

    /// <summary>
    /// Verifies that GetDiag returns null when diagnosis pointer is null.
    /// </summary>
    [TestMethod]
    public void GetDiag_WithNullPointer_ReturnsNull()
    {
        // Arrange
        var hiSegment = new HI_DependentHealthCareDiagnosisCode_2();

        // Act
        var result = X12N837Parser.GetDiag((string)null!, hiSegment);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Verifies that GetDiag returns null when diagnosis pointer is empty string.
    /// </summary>
    [TestMethod]
    public void GetDiag_WithEmptyPointer_ReturnsNull()
    {
        // Arrange
        var hiSegment = new HI_DependentHealthCareDiagnosisCode_2();

        // Act
        var result = X12N837Parser.GetDiag(string.Empty, hiSegment);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Verifies that GetDiag throws ArgumentOutOfRangeException for invalid pointer "13".
    /// </summary>
    [TestMethod]
    public void GetDiag_WithInvalidPointer13_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var hiSegment = new HI_DependentHealthCareDiagnosisCode_2();

        // Act
        Action act = () => X12N837Parser.GetDiag("13", hiSegment);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("diagnosisPointer");
    }

    /// <summary>
    /// Verifies that GetDiag throws ArgumentOutOfRangeException for invalid pointer "0".
    /// </summary>
    [TestMethod]
    public void GetDiag_WithInvalidPointer0_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var hiSegment = new HI_DependentHealthCareDiagnosisCode_2();

        // Act
        Action act = () => X12N837Parser.GetDiag("0", hiSegment);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("diagnosisPointer");
    }

    /// <summary>
    /// Verifies that GetDiag throws ArgumentOutOfRangeException for invalid pointer "abc".
    /// </summary>
    [TestMethod]
    public void GetDiag_WithInvalidPointerAbc_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var hiSegment = new HI_DependentHealthCareDiagnosisCode_2();

        // Act
        Action act = () => X12N837Parser.GetDiag("abc", hiSegment);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("diagnosisPointer");
    }

    #endregion

    #region GetDiag Tests - Integer Pointer

    /// <summary>
    /// Verifies that GetDiag throws ArgumentOutOfRangeException for pointer beyond 4.
    /// </summary>
    [TestMethod]
    public void GetDiag_WithIntegerPointer5_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var hiSegment = new HI_DependentHealthCareDiagnosisCode_2();
        var pointers = new C004_CompositeDiagnosisCodePointer();

        // Act
        Action act = () => X12N837Parser.GetDiag(5, hiSegment, pointers);

        // Assert
        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Only four diagnosis codes are supported*");
    }

    /// <summary>
    /// Verifies that GetDiag throws ArgumentOutOfRangeException for pointer 0.
    /// </summary>
    [TestMethod]
    public void GetDiag_WithIntegerPointer0_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var hiSegment = new HI_DependentHealthCareDiagnosisCode_2();
        var pointers = new C004_CompositeDiagnosisCodePointer();

        // Act
        Action act = () => X12N837Parser.GetDiag(0, hiSegment, pointers);

        // Assert
        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Only four diagnosis codes are supported*");
    }

    /// <summary>
    /// Verifies that GetDiag throws ArgumentOutOfRangeException for negative pointer.
    /// </summary>
    [TestMethod]
    public void GetDiag_WithNegativeIntegerPointer_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var hiSegment = new HI_DependentHealthCareDiagnosisCode_2();
        var pointers = new C004_CompositeDiagnosisCodePointer();

        // Act
        Action act = () => X12N837Parser.GetDiag(-1, hiSegment, pointers);

        // Assert
        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Only four diagnosis codes are supported*");
    }

    #endregion
}
