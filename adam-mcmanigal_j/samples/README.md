# Sample EDI Files

This folder contains small, fake EDI files used for local parser experiments and tests.

## Original sample

- `837-sample-file.edi` was added with this project as the initial 837P parser fixture. This is the example file on their documentation page <https://support.edifabric.com/hc/en-us/articles/360000369472-HIPAA-5010-837P-Professional-Claim>.

## EdiFabric examples

The `.txt` files in this folder were copied from EdiFabric's public X12 examples repository:

- Repository: <https://github.com/EdiFabric/X12.NET>
- Source folder: `Files/HIPAA`
- Retrieved for local development while exploring envelope parsing and sample transaction coverage.

The most relevant files for the current 837P parser work are:

- `ClaimPayment.txt` - professional claim, `ST*837...005010X222A1`
- `ClaimPaymentEVV.txt` - professional claim, `ST*837...005010X222A1`

Other included files cover related HIPAA transaction types, such as 834, 835, 837D, 837I, 270, 271, 276, 277, and 278.

These examples are public demo data and should not be treated as realistic production claim volume, partner-specific structure, or PHI.
