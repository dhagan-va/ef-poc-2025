using EdiConsole.Domain.Claims;
using EdiFabric.Templates.Hipaa5010;

namespace EdiConsole.Mapping;

/// <summary>
/// Turns a validated 837P transaction into domain claims. A pure transformation: no I/O.
/// </summary>
public interface IClaimMapper
{
    /// <summary>
    /// Returns one claim per 2300 loop, whether it sits under the subscriber (2000B)
    /// or under a dependent patient (2000C).
    /// </summary>
    IReadOnlyList<Claim> Map(TS837P transaction);
}
