using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN.Tests.Helpers;
using ECN_Framework_Common.Functions;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Common.Tests.Functions
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
            var sshHostKey = "TestSSHSample";

            // Act
            var ftp = new FtpFunctions(hostIP, userName, password, sshHostKey);

            // Assert
            ftp.ShouldNotBeNull();
            ftp.ShouldSatisfyAllConditions(
                () => ftp.Host.ShouldBe(hostIP),
                () => ftp.User.ShouldBe(userName),
                () => ftp.Pass.ShouldBe(password),
                () => ftp.HostKey.ShouldBe(sshHostKey));
        }
    }
}
