using System;
using System.Data;
using System.IO;
using System.IO.Fakes;
using System.Net;
using System.Net.Fakes;
using System.Reflection;
using System.Text;
using KM.Common.Functions;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    public class FtpFunctionsTests
    {
        private const string HostIP = "ftp://sampleftpserver";
        private const string HostIPInvalid = "InvalidHostName";
        private const string UserName = "UserNameSample";
        private const string Password = "PasswordSample";
        private const string RemoteFileName = "RemoteFileNameSample";
        private const string LocalFileName = "LocalFileNameSample";
        private const string remoteFileContent = "FileContentSample";
        private const string IOExceptionMessage = "Unknown Write Error";

        private IDisposable _shims;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void Teardown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void FtpFunctions_Constructor()
        {
            // Arrange, Act
            var ftp = new FtpFunctions(HostIP, UserName, Password);

            // Assert
            ftp.ShouldNotBeNull();
            ftp.ShouldSatisfyAllConditions(
                () => ftp.Host.ShouldBe(HostIP),
                () => ftp.User.ShouldBe(UserName),
                () => ftp.Pass.ShouldBe(Password));
        }

        [Test]
        public void Download_Succeeds()
        {
            using (var streamRemote = new MemoryStream(Encoding.UTF8.GetBytes(remoteFileContent)))
            using (var streamLocal = new MemoryStream())
            {
                // Arrange
                var ftp = new FtpFunctions(HostIP, UserName, Password);
                ShimFtpWebRequest.AllInstances.GetResponse = (_) => new ShimFtpWebResponse();
                ShimFtpWebResponse.AllInstances.GetResponseStream = (_) => streamRemote;
                ShimFileStream.ConstructorStringFileMode = ShimFileStream.ConstructorStringFileMode = (obj, path, mode) => { };
                ShimFileStream.AllInstances.ReadByteArrayInt32Int32 = (_, byteBuffer, index, bufferSize) => 0;
                ShimFileStream.AllInstances.WriteByteArrayInt32Int32 = (_, byteBuffer, index, bufferSize) => streamLocal.Write(byteBuffer, index, bufferSize);

                // Act
                ftp.Download(RemoteFileName, LocalFileName);

                // Assert
                var localFileContent = Encoding.UTF8.GetString(streamLocal.ToArray());
                localFileContent.ShouldBe(remoteFileContent);
            }
        }

        [Test]
        public void Download_BadUri_ThrowsWebException()
        {
            // Arrange
            var ftp = new FtpFunctions(HostIPInvalid, UserName, Password);

            // Act, Assert
            Shouldly.Should.Throw<UriFormatException>(() => ftp.Download(RemoteFileName, LocalFileName));
        }
      
        [Test]
        public void Download_WriterError_EndWithoutDownload()
        {
            using (var streamRemote = new MemoryStream(Encoding.UTF8.GetBytes(remoteFileContent)))
            using (var streamLocal = new MemoryStream())
            {
                // Arrange
                var ftp = new FtpFunctions(HostIP, UserName, Password);
                ShimFtpWebRequest.AllInstances.GetResponse = (_) => new ShimFtpWebResponse();
                ShimFtpWebResponse.AllInstances.GetResponseStream = (_) => streamRemote;
                ShimFileStream.ConstructorStringFileMode = (obj, path, mode) => { };
                ShimFileStream.AllInstances.ReadByteArrayInt32Int32 = (_, byteBuffer, index, bufferSize) => 0;
                ShimFileStream.AllInstances.WriteByteArrayInt32Int32 = (_, byteBuffer, index, bufferSize) => { throw new IOException(IOExceptionMessage); };

                // Act
                ftp.Download(RemoteFileName, LocalFileName);

                // Assert
                var localFileContent = Encoding.UTF8.GetString(streamLocal.ToArray());
                localFileContent.ShouldNotBe(remoteFileContent);
            }
        }
    }
}