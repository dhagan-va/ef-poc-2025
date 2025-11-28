using System.Reflection;
using System.Runtime.CompilerServices;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using H2.EdiIngestor;

public class X12NFlattener : IX12NFlattener
{
    public async IAsyncEnumerable<FlattenedClaimFile> GetFlattenedClaimFiles(string fileName, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("fileName must be provided", nameof(fileName));

        await using Stream edi = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 8192, useAsync: true);
        using var ediReader = new X12Reader(edi, TypeFactory);

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var read = await ediReader.ReadAsync();
            if (!read)
                break;

            var readerErrorContext = ediReader.Item as ReaderErrorContext;
            if (readerErrorContext != null)
                throw new InvalidX12NFileException(
                    fileName,
                    readerErrorContext.MessageErrorContext.Flatten()
                );

            var claimFile = ediReader.Item as TS837P;
            if (claimFile == null)
                continue;

            MessageErrorContext messageErrorContext;
            if (!claimFile.IsValid(out messageErrorContext))
                throw new InvalidX12NFileException(fileName, messageErrorContext.Flatten());

            var claimTemplate = new FlattenedClaimFile();
            claimTemplate.Loop1000A = claimFile.AllNM1.Loop1000A;
            claimTemplate.Loop1000B = claimFile.AllNM1.Loop1000B;
            foreach (var loop2000A in claimFile.Loop2000A)
            {
                cancellationToken.ThrowIfCancellationRequested();
                claimTemplate.Loop2000A = loop2000A;
                foreach (var loop2000B in loop2000A.Loop2000B)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    claimTemplate.Loop2000B = loop2000B;

                    if (loop2000B.Loop2000C.IsEmpty())
                    {
                        foreach (var result in Process23000Loop(loop2000B.Loop2300))
                            yield return result;
                    }
                    else
                    {
                        foreach (var loop2000C in loop2000B.Loop2000C)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            claimTemplate.Loop2000C = loop2000C;
                            foreach (var result in Process23000Loop(loop2000C.Loop2300))
                                yield return result;
                        }
                    }

                    IEnumerable<FlattenedClaimFile> Process23000Loop(IEnumerable<Loop_2300_837P> loop2300s)
                    {
                        foreach (var loop2300 in loop2300s)
                        {
                            claimTemplate.Loop2300 = loop2300;
                            claimTemplate.Loop2400 = [.. loop2300.Loop2400];
                            yield return claimTemplate.ShallowCopy();
                        }
                    }
                }
            }
        }
    }

    public static TypeInfo TypeFactory(ISA isa, GS gs, ST st)
    {
        if (st.TransactionSetIdentifierCode_01 == "837" && st.ImplementationConventionPreference_03 == "005010X222A1")
            return typeof(TS837P).GetTypeInfo();

        throw new Exception("Unsupported transaction.");
    }
}
