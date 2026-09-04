---
name: map-837-to-domain
description: Map EdiFabric X12 healthcare transactions to the existing PayerEDI.Data domain models and factories. Use when adding, reviewing, or explaining mappings for 837P, 837D, or another supported transaction where loop and segment variants must be identified from context rather than assumed.
---

# Map EDI X12 to domain models

Map parsed EdiFabric transactions into the domain records under `src/PayerEDI.Data/Models` without silently losing data.

## Scope

The current domain model represents:

- Shared claim metadata through `HealthCareClaim`.
- Professional claims through `ProfessionalCareClaim`.
- Dental claims through `DentalCareClaim`.
- Submitters through `ClaimSubmitter`.
- Subscribers and dependents through `Subscriber`.
- People and organizations through `IndividualOrOrganization`.
- Providers through the polymorphic `HealthcareProvider` hierarchy.

`Procedure` represents mapped service-line data and is attached to claim records through the claim factories. Do not invent additional procedure or service-line fields; report source values as unmapped unless the user explicitly requests a domain-model expansion.

## Workflow

1. Inspect the parsed EdiFabric type before inspecting segment names. Start with `TS837P` or `TS837D`; do not assume a service-line segment from the claim family alone.
2. Identify the loop and entity context for each value. A segment's meaning depends on its loop, qualifier, and surrounding structure.
3. Before adding or changing C# code, inspect `global.json`, the target framework, and project-level language settings. Use syntax supported by the selected SDK and language version. This repository selects .NET 10/C# 14; use modern features such as positional records, primary constructors, collection expressions, and extension blocks when they improve clarity and match surrounding code. Do not introduce preview syntax from a newer language version.
4. Search the entire `src/PayerEDI.Data/Models` tree—including all files under `Factory` and `Claims/Factory`—for an existing target model, factory method, extension method, overload, or related mapping pattern before writing code. The currently known claim, submitter, subscriber, provider, and identity factories are useful starting points, not an exhaustive list.
5. Reuse the closest existing factory method or mapping pattern when one exists, regardless of which factory file contains it. Add a narrowly scoped overload or helper only when the search shows that no suitable mapping exists or when the EdiFabric template exposes a genuinely distinct loop type.
6. Preserve transaction-specific behavior. Keep `TS837P` and `TS837D` mappings separate where their generated template types or loop structures differ.
7. Use the existing `RequireNm1`-style validation conventions for required identity elements. Keep optional values nullable and trim values consistently with neighboring factories.
8. Preserve repeated loops as collections. Do not use `First()` or overwrite earlier values when the EDI permits multiple subscribers, dependents, providers, claims, or loop occurrences.
9. Produce a mapping report with:
   - source transaction type;
   - source loop/segment/element for every mapped field;
   - target domain property and factory used;
   - unsupported or unmapped structures;
   - validation failures and assumptions.
10. Add or update focused xUnit tests beside the relevant feature. Test successful mapping, missing required values, optional values, repeated loops, and both supported claim families when applicable.

## Domain record and documentation conventions

When creating or expanding a domain record:

- Prefer a positional `record` with constructor parameters in the domain's stable property order. Use nullable parameters for optional EDI elements and default them to `null` when a transaction-specific mapping legitimately omits them.
- Update every factory call to use named constructor arguments when the record has several fields. This prevents a positional call from silently assigning an EDI value to the wrong property and makes the mapping auditable.
- Add XML documentation to the record and every primary-constructor parameter. Each parameter's documentation must name its source transaction, loop, segment, element, and composite component where applicable, such as `837P Loop 2400 SV1-01 component 2`.
- Document differences between 837P and 837D explicitly. If a property is only available in one transaction type, say so and state that it remains null for the other type.
- Document null, validation, and normalization behavior that is implemented by the factory, including wrapper-to-text conversion such as `EdiValue()`.
- Use role-specific names and loop context. Do not infer a provider role solely from a convenient class name or create `RenderingProvider`/`SupervisingProvider` instances unless the factory actually traverses the corresponding EDI loop.
- Keep EDI traversal in factories. A record should describe the mapped data; it should not contain raw EdiFabric traversal or segment-specific parsing logic.

## Segment and service-line guidance

Do not treat `SV1` and `SV3` as universal rules. Determine the service-line representation from the parsed transaction type, implementation-guide/template loop, and actual EdiFabric object available in the input. Other claim families or variants may use different service-line structures or may not expose one in the same location.

For this iteration, service-line data is an explicit audit item: identify it, state why it is or is not represented by the current domain model, and report it as unmapped when no target property exists. Never silently drop a non-empty loop or segment.

## Expected implementation shape

Top-level mapping should continue to look like:

```csharp
ProfessionalCareClaim.New(groupDate, groupTime, ts837P)
DentalCareClaim.New(groupDate, groupTime, ts837D)
```

Nested mapping should remain composable through the existing factories, for example:

```csharp
var submitter = ClaimSubmitter.New(claim);
var subscribers = Subscriber.New(claim);
var providers = HealthcareProvider.New(claim);
```

Avoid putting raw EDI traversal throughout the processor or persistence service. Keep EDI-to-domain translation in `Models/Factory` and `Models/Claims/Factory`.

When mapping repeated EDI loops into records, separate collection traversal from record construction. The array/collection pipeline may use `SelectMany`, `Where`, and a short `Select`, but must delegate each source item to a dedicated static `New` mapping method. Do not put a large object initializer or constructor call that instantiates the target record inline inside the traversal `Select`; keep transaction-specific overloads such as the 837P and 837D procedure mappings in their own `New` methods. `src/PayerEDI.Data/Models/Factory/Procedure.Factory.cs` is the reference pattern to follow.

## Completion checklist

Before declaring a mapping complete, verify:

- The transaction type and loop context are documented.
- `global.json`, target framework, and language version were checked before C# changes.
- The entire models/factories area was searched, existing factories were reused where appropriate, or the reason for a new factory is recorded.
- 837P and 837D differences are handled explicitly.
- Positional record parameters and factory named arguments preserve the source-to-target mapping.
- Every mapped record parameter documents its EDI source, including composite component numbers.
- Required and optional element behavior is defined.
- Repeated loops are preserved.
- Unmapped segments and unsupported variants are reported.
- Domain-model gaps and any unmapped procedure/service-line fields are called out.
- Focused tests pass, followed by the project test command when the change affects shared mapping behavior.
