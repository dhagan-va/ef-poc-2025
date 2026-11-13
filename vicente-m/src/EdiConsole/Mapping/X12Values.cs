using System.Globalization;

namespace EdiConsole.Mapping;

/// <summary>
/// Converts X12 element values, which EdiFabric always gives as strings, to .NET types the X12 way.
/// </summary>
/// <remarks>
/// <para>
/// X12 writes numbers with "." as the decimal mark and dates as CCYYMMDD, whatever the computer's culture,
/// so every conversion here uses the invariant culture. Keeping these rules in one place means the mapper
/// can't forget them.
/// </para>
/// <para>
/// A missing required value throws <see cref="InvalidDataException"/> with its X12 location
/// (for example "2300 CLM01 (patient control number) is missing."), so a broken file says where it's broken.
/// Optional values that weren't sent stay <see langword="null"/>.
/// </para>
/// </remarks>
internal static class X12Values
{
    private const string DateFormat = "yyyyMMdd";   // X12 D8: CCYYMMDD

    /// <summary>The value itself, or an error naming <paramref name="location"/> when it's missing.</summary>
    public static string Required(string? value, string location)
        => value ?? throw new InvalidDataException($"{location} is missing.");

    /// <summary>A required number (decimal, int, byte...), with "." as the decimal mark.</summary>
    public static T Number<T>(string? value, string location) where T : IParsable<T>
        => T.Parse(Required(value, location), CultureInfo.InvariantCulture);

    /// <summary>An optional number, or <see langword="null"/> when it wasn't sent.</summary>
    public static T? OptionalNumber<T>(string? value) where T : struct, IParsable<T>
        => value is null ? null : T.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>A required CCYYMMDD date.</summary>
    public static DateOnly Date(string? value, string location)
        => DateOnly.ParseExact(Required(value, location), DateFormat, CultureInfo.InvariantCulture);

    /// <summary>An optional CCYYMMDD date, or <see langword="null"/> when it wasn't sent.</summary>
    public static DateOnly? OptionalDate(string? value)
        => value is null ? null : DateOnly.ParseExact(value, DateFormat, CultureInfo.InvariantCulture);

    /// <summary>
    /// A date or a date range, read from a DTP segment's format qualifier (DTP02) and value (DTP03):
    /// <c>D8</c> is one date (<c>To</c> is <see langword="null"/>), <c>RD8</c> is "CCYYMMDD-CCYYMMDD".
    /// </summary>
    public static (DateOnly From, DateOnly? To) Period(string? format, string? value, string location)
    {
        var text = Required(value, location);

        return Required(format, $"{location} format") switch
        {
            "D8" => (Date(text, location), null),
            "RD8" when text.Split('-') is [var from, var to] => (Date(from, location), Date(to, location)),
            var other => throw new InvalidDataException($"{location} has an unexpected format '{other}' or value '{text}'."),
        };
    }
}
