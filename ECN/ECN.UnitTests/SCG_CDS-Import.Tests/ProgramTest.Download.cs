using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Fakes;
using System.IO.Fakes;
using System.Net.Fakes;
using ECN.TestHelpers;
using NUnit.Framework;
using Shouldly;

namespace SCG_CDS_Import.Tests
{
    public partial class ProgramTest
    {
        private const int MaxCacheSize = 2097152;
        private const int MaxBytesSize = 2048;
        private const int ZeroBytesSize = 0;
        private const string DummyUrl = "http://www.google.com/";
        private const string FilePath = "filePath";
        private const string MethodName = "DownLoad";

        [Test]
        public void Download_NonEmptyFilePathAndNonZeroBytesSize_WritesBytesToMemoryStream()
        {
            // Arrange
            InitializeAppSettings();
            var ftpWebRequest = new ShimFtpWebRequest();
            ShimWebRequest.CreateString = (_) => ftpWebRequest;
            var ftpWebResponse = new ShimFtpWebResponse();
            ftpWebRequest.GetResponse = () => ftpWebResponse;
            var stream = new ShimFileStream();
            ftpWebResponse.GetResponseStream = () => stream;
            var bytesSize = ZeroBytesSize;
            var firstCall = true;
            stream.ReadByteArrayInt32Int32 = (_, __, ___) =>
            {
                if (firstCall)
                {
                    firstCall = false;
                    return MaxBytesSize;
                }

                return ZeroBytesSize;
            };

            ShimFileStream.AllInstances.WriteByteArrayInt32Int32 = (_, __, ___, bytesSizeParameter) =>
            {
                bytesSize = bytesSizeParameter;
            };

            var program = new Program();

            // Act
            var parameters = new object[] { DummyUrl, FilePath, string.Empty };
            ReflectionHelper.CallMethod(typeof(Program), MethodName, parameters, program);

            // Assert
            bytesSize.ShouldBe(MaxBytesSize);
        }

        [Test]
        public void Download_NonEmptyFilePathAndZeroBytesSize_DoesNotWriteBytesToMemoryStream()
        {
            // Arrange
            InitializeAppSettings();
            var ftpWebRequest = new ShimFtpWebRequest();
            ShimWebRequest.CreateString = (_) => ftpWebRequest;
            var ftpWebResponse = new ShimFtpWebResponse();
            ftpWebRequest.GetResponse = () => ftpWebResponse;
            var stream = new ShimFileStream();
            ftpWebResponse.GetResponseStream = () => stream;
            var bytesSize = ZeroBytesSize;
            stream.ReadByteArrayInt32Int32 = (_, __, ___) => ZeroBytesSize;

            ShimFileStream.AllInstances.WriteByteArrayInt32Int32 =
                (_, __, ___, bytesSizeParameter) => { bytesSize = bytesSizeParameter; };

            var program = new Program();

            // Act
            var parameters = new object[] { DummyUrl, FilePath, string.Empty };
            ReflectionHelper.CallMethod(typeof(Program), MethodName, parameters, program);

            // Assert
            bytesSize.ShouldBe(ZeroBytesSize);
        }

        [Test]
        public void Download_EmptyFilePath_ThrowsException()
        {
            // Arrange
            InitializeAppSettings();
            var ftpWebRequest = new ShimFtpWebRequest();
            ShimWebRequest.CreateString = (_) => ftpWebRequest;
            var ftpWebResponse = new ShimFtpWebResponse();
            ftpWebRequest.GetResponse = () => ftpWebResponse;
            var stream = new ShimFileStream();
            ftpWebResponse.GetResponseStream = () => stream;
            stream.ReadByteArrayInt32Int32 = (_, __, ___) => MaxBytesSize;

            var errorMessage = string.Empty;
            ShimApplicationException.ConstructorStringException = (_, messageParameter, ___) =>
            {
                errorMessage = messageParameter;
            };

            var program = new Program();

            // Act
            var parameters = new object[] { DummyUrl, string.Empty, string.Empty };
            ReflectionHelper.CallMethod(typeof(Program), MethodName, parameters, program);

            // Assert
            errorMessage.ShouldContain(string.Format(
                "There is an error while downloading {0}{1}.  See InnerException for detailed error. ",
                DummyUrl,
                string.Empty));
        }

        [Test]
        public void Download_NonEmptyFilePathAndOverCacheBytesSize_WritesBytesToMemoryStreamThenResetCacheSize()
        {
            // Arrange
            InitializeAppSettings();
            var ftpWebRequest = new ShimFtpWebRequest();
            ShimWebRequest.CreateString = (_) => ftpWebRequest;
            var ftpWebResponse = new ShimFtpWebResponse();
            ftpWebRequest.GetResponse = () => ftpWebResponse;
            var stream = new ShimFileStream();
            ftpWebResponse.GetResponseStream = () => stream;
            var bytesSize = ZeroBytesSize;
            var callsCount = MaxCacheSize / MaxBytesSize + 1;
            stream.ReadByteArrayInt32Int32 = (_, __, ___) =>
            {
                if (callsCount == 0)
                {
                    return ZeroBytesSize;
                }

                callsCount--;
                return MaxBytesSize;
            };

            ShimFileStream.AllInstances.WriteByteArrayInt32Int32 = (_, __, ___, bytesSizeParameter) =>
            {
                bytesSize = MaxBytesSize;
            };

            var program = new Program();

            // Act
            var parameters = new object[] { DummyUrl, FilePath, string.Empty };
            ReflectionHelper.CallMethod(typeof(Program), "DownLoad", parameters, program);

            // Assert
            bytesSize.ShouldBe(MaxBytesSize);
        }

        private void InitializeAppSettings()
        {
            _fakedAppSettings.Add("FTPUser", string.Empty);
            _fakedAppSettings.Add("FTPPassword", string.Empty);
            ShimConfigurationManager.AppSettingsGet = () => _fakedAppSettings;
        }
    }
}
