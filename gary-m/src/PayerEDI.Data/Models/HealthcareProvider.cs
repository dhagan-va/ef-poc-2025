using System.Text.Json.Serialization;

namespace PayerEDI.Data.Models;

/// <summary>
/// A health-care provider identified in an X12 837P or 837D claim.
/// </summary>
/// <remarks>
/// The provider identity is represented by the nested <see cref="Person"/>
/// record. Its values come from the provider's NM1 Name segment: NM101 is the
/// entity identifier code, NM103 through NM107 contain the person's name
/// components, NM108 is the identification-code qualifier, and NM109 is the
/// provider identifier. NM110, when present and recognized, supplies the
/// relationship code. The provider role is represented by the concrete record
/// type because provider NM1 segments occur in role-specific claim loops.
///
/// <para>
/// In the current 837P and 837D factories, providers are read from claim Loop
/// 2310A and created as <see cref="ReferringProvider"/> instances. Rendering
/// and supervising provider types are available for role-specific mappings but
/// are not currently created by those factories.
/// </para>
/// </remarks>
/// <param name="Provider">The provider's person identity mapped from the role-specific NM1 segment.</param>
[JsonPolymorphic]
[JsonDerivedType(typeof(ReferringProvider), "referringProvider")]
[JsonDerivedType(typeof(RenderingProvider), "renderingProvider")]
[JsonDerivedType(typeof(SupervisingProvider), "supervisingProvider")]
public abstract record HealthcareProvider(Person Provider);

/// <summary>
/// A provider who referred the patient or claim to another provider for care.
/// </summary>
/// <remarks>
/// In the 837P and 837D implementation guides, this role is represented by the
/// claim-level Loop 2310A Referring Provider NM1 segment, normally with NM101
/// equal to <c>DN</c>. The nested <see cref="HealthcareProvider.Provider"/>
/// contains the identity and identifier elements from that NM1 segment.
/// The current factories map each supported Loop 2310A provider to this type.
/// </remarks>
/// <param name="Provider">The referring provider identity from Loop 2310A's NM1 segment.</param>
public record ReferringProvider(Person Provider) : HealthcareProvider(Provider);

/// <summary>
/// A provider who rendered the billed health-care service.
/// </summary>
/// <remarks>
/// In an 837P or 837D payload, this role is normally represented by the
/// claim-level Loop 2310B Rendering Provider NM1 segment, normally with NM101
/// equal to <c>82</c>. The nested <see cref="HealthcareProvider.Provider"/>
/// contains the identity and identifier elements from that NM1 segment. The
/// current provider factories do not yet map Loop 2310B into this type.
/// </remarks>
/// <param name="Provider">The rendering provider identity from Loop 2310B's NM1 segment.</param>
public record RenderingProvider(Person Provider) : HealthcareProvider(Provider);

/// <summary>
/// A provider who supervised the care reported on the claim.
/// </summary>
/// <remarks>
/// In an 837P payload, this role is normally represented by the claim-level
/// Loop 2310E Supervising Provider NM1 segment, normally with NM101 equal to
/// <c>DQ</c>. Loop 2310E is not part of the usual 837D provider-loop structure.
/// The nested
/// <see cref="HealthcareProvider.Provider"/> contains the identity and
/// identifier elements from that NM1 segment. The current provider factories
/// do not yet map Loop 2310E into this type.
/// </remarks>
/// <param name="Provider">The supervising provider identity from Loop 2310E's NM1 segment.</param>
public record SupervisingProvider(Person Provider) : HealthcareProvider(Provider);
