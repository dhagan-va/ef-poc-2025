using EdiFabric.Templates.X12004010;
using PayerEDI.Data.Helpers;

namespace PayerEDI.Data.Models.Attachments.Factory;

/// <summary>Maps EdiFabric 275 structures to attachment domain records.</summary>
public static class AttachmentFactory
{
    public static AttachmentMapping New(DateTime transactionDateTime, TS275 transaction)
    {
        var subjects = transaction
            .NM1Loop.Select(MapSubject)
            .Where(subject => subject is not null)
            .Cast<AttachmentSubject>()
            .ToList();

        var references = transaction
            .NM1Loop.SelectMany(loop => loop.REF)
            .Where(reference =>
                !string.IsNullOrWhiteSpace(reference.ReferenceIdentification_02.EdiValue())
            )
            .Select(reference => new AttachmentReference(
                reference.ReferenceIdentificationQualifier_01.EdiValue() ?? string.Empty,
                reference.ReferenceIdentification_02.EdiValue()!
            ))
            .ToList();

        var errors = new List<AttachmentMappingError>();
        var subject = subjects.Count == 1 ? subjects[0] : null;

        if (subjects.Count > 1)
        {
            errors.Add(
                new AttachmentMappingError(
                    "NM1",
                    null,
                    "Each 275 attachment must resolve to one patient/member subject.",
                    "NM109",
                    "MULTIPLE_SUBJECTS"
                )
            );
        }

        var attachments = transaction
            .LXLoop.SelectMany(lx => MapAttachments(lx, subject, references, errors))
            .ToList();

        if (attachments.Count == 0)
        {
            errors.Add(
                new AttachmentMappingError(
                    "BIN",
                    null,
                    "At least one attachment is required.",
                    "BIN02",
                    "ATTACHMENT_REQUIRED"
                )
            );
        }

        if (subjects.Count == 0)
        {
            errors.Add(
                new AttachmentMappingError(
                    "NM1",
                    null,
                    "A patient/member subject is required.",
                    "NM109",
                    "SUBJECT_REQUIRED"
                )
            );
        }

        return new AttachmentMapping(
            new AttachmentTransaction(
                transactionDateTime,
                transaction.ST.TransactionSetControlNumber_02.EdiValue(),
                subjects,
                attachments
            ),
            errors
        );
    }

    private static AttachmentSubject? MapSubject(Loop_NM1_275 loop)
    {
        var nm1 = loop.NM1;
        var id = nm1.IdentificationCode_09.EdiValue();
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return new AttachmentSubject(
            nm1.EntityIdentifierCode_01.EdiValue() ?? string.Empty,
            nm1.IdentificationCodeQualifier_08.EdiValue(),
            id,
            nm1.NameLastorOrganizationName_03.EdiValue(),
            nm1.NameFirst_04.EdiValue()
        );
    }

    private static IEnumerable<Attachment> MapAttachments(
        Loop_LX_275 loop,
        AttachmentSubject? subject,
        IList<AttachmentReference> references,
        ICollection<AttachmentMappingError> errors
    )
    {
        foreach (var dateLoop in loop.DTPLoop)
        {
            var efiLoop = dateLoop.EFILoop;
            var bin = efiLoop?.BIN;
            var status = AttachmentStatus.Extracted;

            if (bin is null || string.IsNullOrWhiteSpace(bin.BinaryData_02))
            {
                status = AttachmentStatus.Failed;
                errors.Add(
                    new AttachmentMappingError(
                        "BIN",
                        "EFI",
                        "Attachment binary data is required.",
                        "BIN02",
                        "ATTACHMENT_CONTENT_REQUIRED"
                    )
                );
            }
            else
            {
                try
                {
                    var content = Convert.FromBase64String(bin.BinaryData_02);
                    var declaredLength = bin.LengthofBinaryData_01.EdiValue();
                    if (
                        !string.IsNullOrWhiteSpace(declaredLength)
                        && (
                            !int.TryParse(declaredLength, out var length)
                            || length != content.Length
                        )
                    )
                    {
                        throw new InvalidDataException(
                            "Declared binary length does not match content."
                        );
                    }
                }
                catch (FormatException)
                {
                    status = AttachmentStatus.Failed;
                    errors.Add(
                        new AttachmentMappingError(
                            "BIN",
                            "EFI",
                            "Attachment binary data is not valid Base64.",
                            "BIN02",
                            "ATTACHMENT_CONTENT_INVALID"
                        )
                    );
                }
                catch (InvalidDataException exception)
                {
                    status = AttachmentStatus.Failed;
                    errors.Add(
                        new AttachmentMappingError(
                            "BIN",
                            "EFI",
                            exception.Message,
                            "BIN01",
                            "ATTACHMENT_LENGTH_INVALID"
                        )
                    );
                }
            }

            yield return new Attachment(
                loop.LX.AssignedNumber_01.EdiValue(),
                subject?.PatientMemberId,
                subject?.PatientMemberIdQualifier,
                references,
                efiLoop?.EFI.FileName_11.EdiValue(),
                efiLoop?.EFI.InterchangeFormat_07.EdiValue(),
                bin?.LengthofBinaryData_01.EdiValue(),
                status == AttachmentStatus.Extracted ? "s3://pending/275/{attachment-id}" : null,
                status
            );
        }
    }
}
