using H2.EdiIngestor;

public interface IX12NFlattener
{
    IAsyncEnumerable<FlattenedClaimFile> GetFlattenedClaimFiles(
        string fileName,
        CancellationToken cancellationToken
    );
}
