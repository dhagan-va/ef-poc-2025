
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Azure;
using EDI837IngestionTask.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EDI837IngestionTask.Services
{
    public static class LocalReader
    {

        private static readonly string SamplesDir = EnvSetup.SamplesDir;
        public static List<S3FileInfo> ListFiles()
        {
            var files = Directory.GetFiles(SamplesDir, "*.edi");

            return files.Select(o => new S3FileInfo(o, ComputeETag(File.ReadAllBytes(o)),File.GetLastWriteTimeUtc(o))).ToList();
        }

        private static string ComputeETag(byte[] content)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(content)).Replace("-", "").ToLowerInvariant();
        }
    }

}