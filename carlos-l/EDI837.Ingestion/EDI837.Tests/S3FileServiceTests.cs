using EDI837.src.HelperObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI837.Tests
{
    [TestFixture]
    public class S3FileServiceTests : BaseTests 
    {
        [Test]
        public async Task BucketExists_ShouldReturnTrue()
        {
            //Arrange
            var bucketName = "edi-bucket";
            //Act
           var result = await this._s3FileService.BucketExistsAsync(bucketName);
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetClaimStreamByFileNameAsync_ShouldReturnAStream()
        {
            //Arrange
            var bucketName = "edi-bucket";
            var fileName = "837File.edi";
            //Act
            var result = await this._s3FileService.GetClaimStreamByFileNameAsync(bucketName,fileName);
            //Assert
            Assert.IsTrue(result is Stream);
        }

        [Test]
        public async Task GetClaimStreamsByBucketNameAndPrefixAsync_ShouldReturnAListofStreamsAndFileNames()
        {
            //Arrange
            var bucketName = "edi-bucket";
            var prefix = "837";
            //Act
            var result = await this._s3FileService.GetClaimStreamsByBucketNameAndPrefixAsync(bucketName, prefix);
            //Assert
            Assert.IsTrue(result is List<StreamResult>);
            Assert.IsTrue(result.Any());
        }
    }
}
