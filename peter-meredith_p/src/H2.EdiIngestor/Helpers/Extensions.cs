namespace H2.EdiIngestor;

public static class IEnumerableExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> values)
    {
        return values is null || !values.Any();
    }
}
