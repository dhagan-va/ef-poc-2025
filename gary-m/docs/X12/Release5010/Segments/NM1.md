# NM1

## Mapping Reference

`NM1` identifies an individual or organization in a transaction. The meaning of the segment depends on the loop and the
`NM101` entity identifier code.

### Name and Identity Fields

| Element | Meaning                                                                                                        | Max size | Current model mapping                                               | My Notes                |
|---------|----------------------------------------------------------------------------------------------------------------|---------:|---------------------------------------------------------------------|-------------------------|
| `NM101` | Entity Identifier Code; identifies the entity's role, such as submitter, receiver, patient, payer, or provider |        3 | `EntityIdentifierCode`                                              |                         |
| `NM102` | Entity Type Qualifier (There are 16 possible values)                                                           |        1 | `1` creates `Person`; `2` creates `NonPerson`                       |                         |
| `NM103` | Primary last name or organization name                                                                         |       60 | `Person.LastName` or `NonPerson.OrganizationName`                   |                         |
| `NM104` | First name                                                                                                     |       35 | `Person.FirstName`                                                  |                         |
| `NM105` | Middle name                                                                                                    |       25 | `Person.MiddleName`                                                 |                         |
| `NM106` | Name prefix                                                                                                    |       10 | `Person.Prefix`                                                     |                         |
| `NM107` | Name suffix                                                                                                    |       10 | `Person.Suffix`                                                     |                         |
| `NM108` | Identification Code Qualifier                                                                                  |        2 | `IdentificationCodeQualifier`                                       |                         |
| `NM109` | Identification code/value                                                                                      |       80 | `ResponseContactIdentifier`                                         |                         |
| `NM110` | Entity Relationship Code (required if NM111 is present)                                                        |        2 | `EntityRelationshipCode?` through `EntityRelationshipCode.FromCode` |                         |
| `NM111` | Entity Identifier Code (there are 1500 possible values)                                                        |        3 | `EntityRelationshipCode?` through `EntityRelationshipCode.FromCode` |                         |
| `NM112` | Additional last name or organization name; may represent a second surname NM103 is required if this is present |       60 | `SecondLastName`                                                    | Must act as an overflow |

`NM108` and `NM109` must be interpreted together. `NM108` identifies what kind of value is contained in `NM109`, such as
an NPI, member identifier, tax identifier, or submitter identifier. Storing only `NM109` would lose important meaning.

### Person and Organization Rules

- `NM102 = 1` maps to `Person`.
- `NM102 = 2` maps to `NonPerson`.
- `NM103` is required by the current domain model for both variants: it becomes the person's last name or the
  organization's name.
- `NM112` is optional. When present, it supplements `NM103`; it does not replace the primary name. For a person, it can
  represent a second surname.
- `NM104` through `NM107` are person-specific optional name fields.
- `NM110` is optional in the model and becomes `null` when it is absent or its code is not recognized by the local
  relationship-code enumeration.

### Validation Behavior

The current mapper requires `NM101`, `NM102`, `NM103`, `NM108`, and `NM109` to be present and non-blank. Accepted string
values are trimmed. Missing required values and unsupported `NM102` values raise `InvalidNm1Exception`.

The EdiFabric property names used by the current mapping include
`ResponseContactLastorOrganizationName_03` for `NM103`,
`ResponseContactIdentifier_09` for `NM109`, and
`NameLastorOrganizationName_12` for `NM112`.
