using System.IO;
using System.Linq;
using NUnit.Framework;
using Moq;
using Deepika.EDIIngestion.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Deepika.EDIIngestion.Tests
{
    [TestFixture]
    public class EDIParserTests
    {
        // Reach up from the test output folder to the solution root, then into the samples folder
        private string SamplesFolder => Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", "..", "samples");

        [Test]
        public void CountClaims_InSample1_IsFive()
        {
            var path = Path.GetFullPath(Path.Combine(SamplesFolder, "sample_837_1.edi"));
            Assert.That(File.Exists(path), Is.True, "Sample file must exist for the test to run.");

            // Arrange: use the real parser directly (no validator required for this simple check)
            var parser = new EdiFabricParser();

            // Act: use parser to get interchanges and count CLM segments across all transactions
            var interchanges = parser.ParseFile(path);
            var clmCount = interchanges?.Sum(i => i.Claims?.Count ?? 0) ?? 0;

            // Assert
            Assert.AreEqual(5, clmCount, "Expected 5 CLM segments in sample_837_1.edi");
        }

        [Test]
        public void CountClaims_InSample3_IsFive()
        {
            var path = Path.GetFullPath(Path.Combine(SamplesFolder, "sample_837_3.edi"));
            Assert.That(File.Exists(path), Is.True, "Sample file must exist for the test to run.");

            var text = File.ReadAllText(path);
            var clmCount = text.Split('~').Count(s => s.TrimStart().StartsWith("CLM*"));
            Assert.AreEqual(5, clmCount, "Expected 5 CLM segments in sample_837_3.edi");
        }

        [Test]
        public void MalformedSample_ReturnsZeroOrFailsGracefully()
        {
            var path = Path.GetFullPath(Path.Combine(SamplesFolder, "sample_837_2..edi"));
            Assert.That(File.Exists(path), Is.True, "Malformed sample must exist for the test to run.");

            var text = File.ReadAllText(path);
            // The malformed file is not expected to contain valid CLM segments; assert none found
            var clmCount = text.Split('~').Count(s => s.TrimStart().StartsWith("CLM*"));
            Assert.That(clmCount, Is.EqualTo(0).Or.LessThan(1));
        }
    }
}
