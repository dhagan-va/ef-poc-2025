using H2.EdiIngestor.Data;

public interface IX12N837Parser
{
    IAsyncEnumerable<Claim> ReadFile(string fileName);
}
