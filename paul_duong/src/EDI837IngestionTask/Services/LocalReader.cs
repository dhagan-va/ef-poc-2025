using EDI837IngestionTask.Models;

namespace EDI837IngestionTask.Services
{
    public static class LocalReader
    {

        private static readonly string _samplesDir = EnvSetup.SamplesDir;

        /// <summary>
        /// check all end with .edi files in Samples dir and return a list of the file
        /// </summary>
        /// <returns>a list of the file</returns>
        public static List<S3FileInfo> ListFiles()
        {
            var files = Directory.GetFiles(_samplesDir, "*.edi");

            return files.Select(o => new S3FileInfo(o, ComputeETag(File.ReadAllBytes(o)), File.GetLastWriteTimeUtc(o))).ToList();
        }

        /// <summary>
        /// based on the file content to calculate ETag value
        /// </summary>
        /// <returns>ETag value</returns>
        private static string ComputeETag(byte[] content)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(content)).Replace("-", "").ToLowerInvariant();
        }
    }

}