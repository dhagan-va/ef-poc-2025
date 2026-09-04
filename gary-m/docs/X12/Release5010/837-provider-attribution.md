# 837 Provider Attribution

## Answer

Yes. An 837 can identify the provider who performed a service, but the provider must be interpreted in its loop context. The most specific source is the provider in the service-line `2420` loop. A claim-level provider does not necessarily identify who performed every service line on the claim.

## Service-line sources

| Claim | Service line | Provider context | Typical provider segment | Attribution |
|---|---|---|---|---|
| 837P | `Loop2400/SV1` | `Loop2420A` | `NM1*82` | Rendering provider for that service line |
| 837D | `Loop2400/SV3` | `Loop2420A` | `NM1*82` | Rendering provider for that dental service line |

The `NM1` qualifier and the loop are both required to interpret the identity. The provider's name and identifier are carried by `NM103`-`NM109`; the identifier qualifier in `NM108` is required to know whether the identifier is, for example, an NPI (`XX`) or another identifier type.

When a populated service-line `2420A` rendering-provider loop is present, it is the best answer to “which provider did this procedure?” The association is made by the surrounding `Loop2400`: the provider applies to that service line, not automatically to all services in the claim.

## Claim-level provider sources

837P and 837D also contain provider loops around the claim, including billing and rendering/referring provider contexts in the `2310` area. These identify providers participating in the claim, but they are broader context. They should not be treated as proof that one provider performed every `SV1` or `SV3` line when line-level `2420A` data is available or when multiple providers are present.

## Current application mapping

The current application maps:

- 837P procedures from `Loop2000A → Loop2000B → Loop2300 → Loop2400/SV1` (and dependent claim loops).
- 837D procedures from the equivalent path ending in `Loop2400/SV3`.
- Some claim-level provider identity data into `HealthcareProvider` records.

`Procedure` currently has no provider property, and the procedure factory does not traverse or retain `Loop2400.AllNM1.Loop2420A`. Therefore, the current `Procedure` collection cannot answer which provider performed each individual line. The existing `HealthcareProviders` collection must not be interpreted as line-level attribution.

## Attribution limitations

- A transaction may omit `2420A`; in that case, the EDI does not provide a line-specific rendering provider at that location.
- A line may contain a provider identity that differs from the billing provider.
- Additional provider roles and companion loops may be present; their role must be read from the loop and `NM101` value.
- Provider identity is not the same as proof that the service was clinically performed. The 837 is a submitted claim, and payer adjudication or other records may be needed for that determination.

A future domain change would need to associate a `Procedure` with a provider identity (or a provider reference) while preserving the claim-loop and service-line context. That is outside this documentation-only change.
