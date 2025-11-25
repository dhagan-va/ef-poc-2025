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
    }
}
