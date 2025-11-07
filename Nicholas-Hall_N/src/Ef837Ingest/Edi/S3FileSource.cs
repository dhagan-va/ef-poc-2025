using Amazon.S3;
using Amazon.S3.Model;

namespace Ef837Ingest.Edi
{
    public sealed class FileSource : IFileSource
    {
        private readonly IAmazonS3 _s3;

        public FileSource(IAmazonS3 s3) => _s3 = s3;

        public async Task<Stream> OpenAsync(string uri, CancellationToken ct = default)
        {
            if (uri.StartsWith("s3://", StringComparison.OrdinalIgnoreCase))
            {
                var (bucket, key) = ParseS3(uri);
                var resp = await _s3.GetObjectAsync(bucket, key, ct);
                var ms = new MemoryStream();
                await resp.ResponseStream.CopyToAsync(ms, ct);
                ms.Position = 0;
                return ms;
            }

            if (uri.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
            {
                var path = uri.Substring("file://".Length);
                return File.OpenRead(path);
            }

            // treat as local relative path
            return File.OpenRead(uri);
        }

        private static (string bucket, string key) ParseS3(string s3Uri)
        {
            var rest = s3Uri.Substring("s3://".Length);
            var slash = rest.IndexOf('/');
            if (slash < 0) return (rest, "");
            return (rest[..slash], rest[(slash + 1)..]);
        }
    }
}