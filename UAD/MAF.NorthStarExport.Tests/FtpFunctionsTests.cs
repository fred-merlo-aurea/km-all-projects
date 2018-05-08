using MAF.NorthStarExport;
using NUnit.Framework;
using Shouldly;

namespace MAF.NorthStarExport.Tests
{
    [TestFixture]
    public class FtpFunctionsTests
    {
        [Test]
        public void FtpFunctions_Constructor()
        {
            // Arrange
            var hostIP = "TestHostIpSample";
            var userName = "TestUserNameSample";
            var password = "TestPasswordSample";

            // Act
            var ftp = new FtpFunctions(hostIP, userName, password);

            // Assert
            ftp.ShouldNotBeNull();
            ftp.ShouldSatisfyAllConditions(
                () => ftp.Host.ShouldBe(hostIP),
                () => ftp.User.ShouldBe(userName),
                () => ftp.Pass.ShouldBe(password));
        }
    }
}

