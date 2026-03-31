using EdiFabric.Templates.Hipaa5010;
using System.Reflection;
using X12EDI837Ingestion.Consumer.Application.Models;

namespace X12EDI837Ingestion.Consumer.Application.Validator;

public sealed class Snip2Validator : ISnipLevelValidator
{
    public int SnipLevel => 2;
    const int MaxServiceLinesPerClaim = 50;

    public Task<IReadOnlyList<SnipValidationError>> ValidateAsync(
        ParsedEdiDocument document,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);

        var errors = new List<SnipValidationError>();

        cancellationToken.ThrowIfCancellationRequested();

        ValidateMissingClaimIdentifier(document, errors);
        ValidateMissingServiceLine(document, errors);
        ValidateMaximumServiceLinesPerClaim(document, errors);
        ValidateMissingDependentDmg(document, errors);
        ValidateMissingSeTrailer(document, errors);
        ValidateMatchingTransactionControlNumber(document, errors);
        ValidateSubscriberHlChildRecord(document, errors);

        return Task.FromResult<IReadOnlyList<SnipValidationError>>(errors);
    }

    /// <summary>
    /// Missing Claim Identifier (CLM01)
    /// </summary>
    /// <param name="document"></param>
    /// <param name="errors"></param>
    private void ValidateMissingClaimIdentifier(
     ParsedEdiDocument document,
     List<SnipValidationError> errors)
    {
        if (document?.TransactionSets == null)
            return;

        foreach (var ts in document.TransactionSets)
        {
            if (ts.Loop2000A == null)
                continue;

            foreach (var loop2000A in ts.Loop2000A)
            {
                if (loop2000A.Loop2000B == null)
                    continue;

                foreach (var loop2000B in loop2000A.Loop2000B)
                {
                    // CASE 1: Claims directly under Subscriber
                    if (loop2000B.Loop2300 != null)
                    {
                        ValidateClaims(loop2000B.Loop2300, errors);
                    }

                    // CASE 2: Claims under Patient (Dependent)
                    if (loop2000B.Loop2000C != null)
                    {
                        foreach (var loop2000C in loop2000B.Loop2000C)
                        {
                            if (loop2000C.Loop2300 != null)
                            {
                                ValidateClaims(loop2000C.Loop2300, errors);
                            }
                        }
                    }
                }
            }
        }
    }

    //Missing claims Identifier in CLM01
    private void ValidateClaims(
    List<Loop_2300_837P> claims,
    List<SnipValidationError> errors)
    {
        foreach (var claimLoop in claims)
        {
            var clm = claimLoop.CLM_ClaimInformation;

            if (clm == null || string.IsNullOrWhiteSpace(clm.PatientControlNumber_01))
            {
                errors.Add(new SnipValidationError
                {
                    SnipLevel = 2,
                    SegmentId = "CLM",
                    Message = "Missing claims Identifier in CLM01"
                });
            }
        }
    }

    /// <summary>
    /// Missing claims Identifier in CLM01
    /// </summary>
    /// <param name="document"></param>
    /// <param name="errors"></param>
    private void ValidateMissingServiceLine(
    ParsedEdiDocument document,
    List<SnipValidationError> errors)
    {
        if (document?.TransactionSets == null)
            return;

        foreach (var ts in document.TransactionSets)
        {
            if (ts.Loop2000A == null)
                continue;

            foreach (var loop2000A in ts.Loop2000A)
            {
                if (loop2000A.Loop2000B == null)
                    continue;

                foreach (var loop2000B in loop2000A.Loop2000B)
                {
                    // Claims under Subscriber
                    if (loop2000B.Loop2300 != null)
                    {
                        ValidateServiceLines(loop2000B.Loop2300, errors);
                    }

                    // Claims under Patient (Dependent)
                    if (loop2000B.Loop2000C != null)
                    {
                        foreach (var loop2000C in loop2000B.Loop2000C)
                        {
                            if (loop2000C.Loop2300 != null)
                            {
                                ValidateServiceLines(loop2000C.Loop2300, errors);
                            }
                        }
                    }
                }
            }
        }
    }

   
    private void ValidateServiceLines(
    List<Loop_2300_837P> claims,
    List<SnipValidationError> errors)
    {
        foreach (var claimLoop in claims)
        {
            if (claimLoop.Loop2400 == null || !claimLoop.Loop2400.Any())
            {
                errors.Add(new SnipValidationError
                {
                    SnipLevel = 2,
                    SegmentId = "SV1",
                    Message = "Claim number is missing from a service line"
                });
            }
        }
    }

    /// <summary>
    /// Exceed number of maximum service lines per claim
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="errors"></param>
    private void ValidateMaximumServiceLinesPerClaim(
    ParsedEdiDocument document,
    List<SnipValidationError> errors)
    {
        if (document?.TransactionSets == null)
            return;

        foreach (var ts in document.TransactionSets)
        {
            if (ts.Loop2000A == null)
                continue;

            foreach (var loop2000A in ts.Loop2000A)
            {
                if (loop2000A.Loop2000B == null)
                    continue;

                foreach (var loop2000B in loop2000A.Loop2000B)
                {
                    // Claims under Subscriber
                    if (loop2000B.Loop2300 != null)
                    {
                        ValidateMaximumServiceLines(loop2000B.Loop2300, errors);
                    }

                    // Claims under Patient / Dependent
                    if (loop2000B.Loop2000C != null)
                    {
                        foreach (var loop2000C in loop2000B.Loop2000C)
                        {
                            if (loop2000C.Loop2300 != null)
                            {
                                ValidateMaximumServiceLines(loop2000C.Loop2300, errors);
                            }
                        }
                    }
                }
            }
        }
    }

    private void ValidateMaximumServiceLines(
    List<Loop_2300_837P> claims,
    List<SnipValidationError> errors)
    {
        foreach (var claimLoop in claims)
        {
            int counter = 0;

            if (claimLoop.Loop2400 != null)
            {
                foreach (var svc in claimLoop.Loop2400)
                {
                    counter++;
                }
            }

            if (counter > MaxServiceLinesPerClaim)
            {
                errors.Add(new SnipValidationError
                {
                    SnipLevel = 2,
                    SegmentId = "SV1",
                    Message = $"Exceeded maximum number of service lines per claim. Maximum allowed is {MaxServiceLinesPerClaim}."
                });
            }
        }
    }

    /// <summary>
    /// Missing DMG Segment for dependent
    /// </summary>
    /// <param name="document"></param>
    /// <param name="errors"></param>
    private void ValidateMissingDependentDmg(
    ParsedEdiDocument document,
    List<SnipValidationError> errors)
    {
        if (document?.TransactionSets == null)
            return;

        foreach (var ts in document.TransactionSets)
        {
            if (ts.Loop2000A == null)
                continue;

            foreach (var loop2000A in ts.Loop2000A)
            {
                if (loop2000A.Loop2000B == null)
                    continue;

                foreach (var loop2000B in loop2000A.Loop2000B)
                {
                    if (loop2000B.Loop2000C == null)
                        continue;

                    foreach (var loop2000C in loop2000B.Loop2000C)
                    {
                        ValidateDependentDmg(loop2000C, errors);
                    }
                }
            }
        }
    }

    private void ValidateDependentDmg(
    Loop_2000C_837P loop2000C,
    List<SnipValidationError> errors)
    {
        if (loop2000C == null)
            return;

        var dependentNameLoop = loop2000C.Loop2010CA;

        if (dependentNameLoop == null || dependentNameLoop.DMG_PatientDemographicInformation == null)
        {
            errors.Add(new SnipValidationError
            {
                SnipLevel = 2,
                SegmentId = "DMG",
                Message = "Missing DMG Segment for dependent"
            });
        }
    }

    /// <summary>
    /// Missing trailer segment SE
    /// </summary>
    /// <param name="document"></param>
    /// <param name="errors"></param>
    private void ValidateMissingSeTrailer(
    ParsedEdiDocument document,
    List<SnipValidationError> errors)
    {
        if (document?.TransactionSets == null)
            return;

        foreach (var transactionSet in document.TransactionSets)
        {
            if (transactionSet.SE == null)
            {
                errors.Add(new SnipValidationError
                {
                    SnipLevel = 2,
                    SegmentId = "SE",
                    Message = "Missing trailer segment SE"
                });
            }
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    /// <param name="errors"></param>
    private void ValidateMatchingTransactionControlNumber(
    ParsedEdiDocument document,
    List<SnipValidationError> errors)
    {
        if (document?.TransactionSets == null)
            return;

        foreach (var transactionSet in document.TransactionSets)
        {
            var st = transactionSet.ST;
            var se = transactionSet.SE;

            if (st == null || se == null)
                continue;

            if (!string.Equals(
                    st.TransactionSetControlNumber_02,
                    se.TransactionSetControlNumber_02,
                    StringComparison.Ordinal))
            {
                errors.Add(new SnipValidationError
                {
                    SnipLevel = 2,
                    SegmentId = "ST/SE",
                    Message = "Not matching transaction control number"
                });
            }
        }
    }

    private void ValidateSubscriberHlChildRecord(
    ParsedEdiDocument document,
    List<SnipValidationError> errors)
    {
        if (document?.TransactionSets == null)
            return;

        foreach (var transactionSet in document.TransactionSets)
        {
            if (transactionSet.Loop2000A == null)
                continue;

            foreach (var loop2000A in transactionSet.Loop2000A)
            {
                if (loop2000A.Loop2000B == null)
                    continue;

                foreach (var loop2000B in loop2000A.Loop2000B)
                {
                    var subscriberHl = loop2000B.HL_SubscriberHierarchicalLevel;

                    if (subscriberHl == null)
                        continue;

                    var hl04 = subscriberHl.HierarchicalChildCode_04;
                    var hasPatientChild = loop2000B.Loop2000C != null && loop2000B.Loop2000C.Any();

                    if (hasPatientChild && hl04 != "1")
                    {
                        errors.Add(new SnipValidationError
                        {
                            SnipLevel = 2,
                            SegmentId = "HL",
                            Message = "Subscriber HL level element 04 not correct for child record"
                        });
                    }

                    if (!hasPatientChild && hl04 != "0")
                    {
                        errors.Add(new SnipValidationError
                        {
                            SnipLevel = 2,
                            SegmentId = "HL",
                            Message = "Subscriber HL level element 04 not correct for child record"
                        });
                    }
                }
            }
        }
    }
}



