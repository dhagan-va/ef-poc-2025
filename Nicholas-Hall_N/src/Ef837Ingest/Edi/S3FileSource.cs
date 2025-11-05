using Amazon.S3;
using Amazon.S3.Model;

namespace Ef837Ingest.Edi
{

public class S3FileSource : IFileSource
{
    private readonly IAmazonS3 _s3;
    public S3FileSource(IAmazonS3 s3) => _s3 = s3;

    public async Task<Stream> OpenAsync(string locator, CancellationToken ct = default)
    {
        var uri = new Uri(locator);
        var bucket = uri.Host;
        var key = uri.AbsolutePath.TrimStart('/');

        var resp = await _s3.GetObjectAsync(new GetObjectRequest
        {
            BucketName = bucket,
            Key = key
        }, ct);

        var ms = new MemoryStream();
        await resp.ResponseStream.CopyToAsync(ms, ct);
        ms.Position = 0;
        return ms;
    }
}

}
