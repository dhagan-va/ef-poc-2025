using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using System.Net;

namespace PayerEDI.Processor.Console.Services;

public class S3FileLoader(ILogger<S3FileLoader> logger, IAmazonS3 s3Client) : IEdiFileLoader
{
    public async Task<Stream> OpenStreamAsync(string ediLocation)
    {
        logger.LogDebug("Loading File from {location}", ediLocation);

        var s3Uri = new Uri(ediLocation, UriKind.Absolute);
        var path = s3Uri.AbsolutePath.TrimStart('/');
        var bucket = s3Uri.Host;
        var key = path;

        if (string.IsNullOrEmpty(bucket))
        {
            var separator = path.IndexOf('/');

            if (separator > 0)
            {
                bucket = path[..separator];
                key = path[(separator + 1)..];
            }
        }

        if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException(
                $"The S3 location must contain a bucket and object key: {ediLocation}",
                nameof(ediLocation)
            );
        }

        try
        {
            await s3Client.GetObjectMetadataAsync(
                new GetObjectMetadataRequest
                {
                    BucketName = bucket,
                    Key = key,
                }
            );
        }
        catch (AmazonS3Exception exception) when (
            exception.StatusCode == HttpStatusCode.NotFound
            || string.Equals(exception.ErrorCode, "NoSuchKey", StringComparison.OrdinalIgnoreCase)
            || string.Equals(exception.ErrorCode, "NotFound", StringComparison.OrdinalIgnoreCase)
        )
        {
            throw new FileNotFoundException($"S3 file {ediLocation} not found", exception);
        }

        var response = await s3Client.GetObjectAsync(
                new GetObjectRequest
                {
                    BucketName = bucket,
                    Key = key,
                }
            );

        return response.ResponseStream;
    }
}
