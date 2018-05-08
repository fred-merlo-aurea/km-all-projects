using System;
using System.Data;
using System.Reflection;
using Core_AMS.Utilities;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core_AMS.Utilities
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
