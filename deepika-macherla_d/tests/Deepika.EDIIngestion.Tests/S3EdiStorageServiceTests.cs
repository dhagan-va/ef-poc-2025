using System;
using System.Linq;
using System.Threading.Tasks;
using Deepika.EDIIngestion.Services;
using NUnit.Framework;

namespace Deepika.EDIIngestion.Tests
{
    [TestFixture]
    public class S3EdiStorageServiceTests
    {
        private const string BucketName = "test-edi-bucket";

        [Test]
        public async Task ListObjectsAsync_SkipsWhenNoEndpoint_OtherwiseReturnsCollection()
        {
            var endpoint = Environment.GetEnvironmentVariable("AWS_S3_ENDPOINT");
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                Assert.Ignore("AWS_S3_ENDPOINT not set - skipping integration-style S3 test.");
            }

            var svc = new S3EdiStorageService(endpoint);
            var keys = await svc.ListObjectsAsync(BucketName);
            Assert.IsNotNull(keys, "Expected a collection of keys (may be empty).");
        }

        [Test]
        public async Task ReadObjectAsync_SkipsWhenNoObjects_OtherwiseReturnsBytes()
        {
            var endpoint = Environment.GetEnvironmentVariable("AWS_S3_ENDPOINT");
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                Assert.Ignore("AWS_S3_ENDPOINT not set - skipping integration-style S3 test.");
            }

            var svc = new S3EdiStorageService(endpoint);
            var keys = (await svc.ListObjectsAsync(BucketName)).ToList();
            if (!keys.Any())
            {
                Assert.Ignore($"No objects found in bucket '{BucketName}' - skipping read test.");
            }

            var firstKey = keys.First();
            var data = await svc.ReadObjectAsync(BucketName, firstKey);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length > 0, "Expected non-empty object bytes");
        }
    }
}
