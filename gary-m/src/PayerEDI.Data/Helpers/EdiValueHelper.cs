using System.Globalization;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Templates.Hipaa5010;
using PayerEDI.Data.Exceptions;

namespace PayerEDI.Data.Helpers;

public static class EdiValueHelper
{
    public static string? EdiValue(this string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public static string RequireNm1(this NM1 nm1, Func<NM1, string?> valueSelector, string element)
    {
        var value = valueSelector(nm1);

        return string.IsNullOrWhiteSpace(value)
            ? throw new InvalidNm1Exception($"{element} is required for an NM1 identity.")
            : value.Trim();
    }

    public static DateTime GroupDateTime(this GS group) =>
        DateTime.ParseExact(
            $"{group.Date_4}{group.Time_5}",
            ["yyyyMMddHHmm", "yyMMddHHmm", "yyyyMMddHHmmss", "yyMMddHHmmss"],
            CultureInfo.InvariantCulture,
            DateTimeStyles.None
        );
}
