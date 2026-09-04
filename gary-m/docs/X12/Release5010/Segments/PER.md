# PER

## Mapping Reference

`PER` identifies a person or office to whom administrative communications should be directed. The meaning of the segment
depends on the loop and the `PER01` contact function code.

### Contact and Communication Fields

| Element | Meaning                                                      | Max size | Current model mapping       | My Notes |
|---------|--------------------------------------------------------------|---------:|-----------------------------|----------|
| `PER01` | Contact Function Code; identifies the purpose of the contact |        2 | `ContactFunctionCode`       |          |
| `PER02` | Contact name                                                 |       60 | `Name`                      |          |
| `PER03` | Communication Number Qualifier for `PER04`                   |        2 | `PrimaryNumber.Qualifier`   |          |
| `PER04` | Communication number                                         |       80 | `PrimaryNumber.Number`      |          |
| `PER05` | Communication Number Qualifier for `PER06`                   |        2 | `SecondaryNumber.Qualifier` |          |
| `PER06` | Communication number                                         |       80 | `SecondaryNumber.Number`    |          |
| `PER07` | Communication Number Qualifier for `PER08`                   |        2 | `TertiaryNumber.Qualifier`  |          |
| `PER08` | Communication number                                         |       80 | `TertiaryNumber.Number`     |          |
| `PER09` | Contact Inquiry Reference                                    |       20 | Not currently mapped        |          |

Each communication number must be interpreted together with its qualifier. The qualifier identifies what kind of value
is contained in the number, such as a telephone number, facsimile number, or email address. Storing only the number
would lose important meaning.

### Communication Number Rules

- `PER03` and `PER04` map to `PrimaryNumber` when both values are present and the qualifier is recognized.
- `PER05` and `PER06` map to `SecondaryNumber` when both values are present and the qualifier is recognized.
- `PER07` and `PER08` map to `TertiaryNumber` when both values are present and the qualifier is recognized.
- A communication number is `null` when either its qualifier or number is absent, blank, or has an unsupported
  qualifier.
- `PER02` is optional and maps directly to `Name`.

### Validation Behavior

The current mapper requires `PER01` to be present and non-blank. The value is retained as `ContactFunctionCode`; it is
not converted to a local enumeration. Missing `PER01` values raise an `ArgumentException`.

Communication number values and qualifiers are trimmed by the EDI parser before mapping. A number requires both a
non-blank value and a recognized communication number qualifier; invalid or incomplete pairs are ignored and become
`null`. The current model does not map `PER09`.

The EdiFabric property names used by the current mapping include
`ContactFunctionCode_01` for `PER01`,
`ResponseContactName_02` for `PER02`,
`CommunicationNumberQualifier_03` and `ResponseContactCommunicationNumber_04` for the first communication pair,
`CommunicationNumberQualifier_05` and `ResponseContactCommunicationNumber_06` for the second pair, and
`CommunicationNumberQualifier_07` and `ResponseContactCommunicationNumber_08` for the third pair.
