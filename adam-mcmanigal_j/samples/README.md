# Sample EDI Files

This folder contains small, fake EDI files used for local parser experiments and tests.
Every file here is an 837 (health care claim) transaction — the only type this project
parses. The set covers all three 837 variants: professional (837P), institutional (837I),
and dental (837D).

## Original sample

- `837-sample-file.edi` was added with this project as the initial 837P parser fixture. This is the example file on their documentation page <https://support.edifabric.com/hc/en-us/articles/360000369472-HIPAA-5010-837P-Professional-Claim>.

## EdiFabric examples

The `.txt` files in this folder were copied from EdiFabric's public X12 examples repository:

- Repository: <https://github.com/EdiFabric/X12.NET>
- Source folder: `Files/HIPAA`
- Retrieved for local development while exploring envelope parsing and 837 transaction coverage.

The 837 files are:

- `837-sample-file.edi` — 837**P** professional claim, `ST*837...005010X222A1`
- `ClaimPayment.txt` — 837**P** professional claim, `ST*837...005010X222A1`
- `ClaimPaymentEVV.txt` — 837**P** professional claim, `ST*837...005010X222A1`
- `InstitutionalClaim.txt` — 837**I** institutional claim, `ST*837...005010X223A2`
- `DentalClaim.txt` — 837**D** dental claim, `ST*837...005010X224A2`

These examples are public demo data and should not be treated as realistic production claim volume, partner-specific structure, or PHI.
