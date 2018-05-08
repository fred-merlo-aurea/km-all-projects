using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using Core_AMS.Utilities.Fakes;
using DQM.Modules;
using FrameworkUAS.Object;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;
using UAS.UnitTests.DQM.Helpers.Validation.Common;
using ClientFtp = FrameworkUAS.Entity.ClientFTP;
using EntityClient = KMPlatform.Entity.Client;

namespace UAS.UnitTests.DQM.Modules
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class UAD_FileValidatorTest : Fakes
    {
        private const string UnicodeHeaderName = "Unicode";

        [SetUp]
        public void Setup()
        {
            InitializeFakes();
        }

        [Test]
        public void WorkerDoWork_HasUnicode_ReturnTrue()
        {
            // Arrange
            ShimFileWorker.AllInstances.CheckHeadersForUnicodeCharsFileInfoFileConfigurationBoolean =
                (worker, info, _, __) =>
                {
                    var headers = new Dictionary<string, bool> { { UnicodeHeaderName, true } };
                    return headers;
                };
            ShimWPF.MessageErrorString = _ => { };

            // Act
            var result = UAD_FileValidator.CheckUnicode(null, null);

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void WorkerDoWork_HasNoUnicode_ReturnFalse()
        {
            // Arrange
            ShimFileWorker.AllInstances.CheckHeadersForUnicodeCharsFileInfoFileConfigurationBoolean =
                (worker, info, _, __) =>
                {
                    var headers = new Dictionary<string, bool> { { UnicodeHeaderName, false } };
                    return headers;
                };
            ShimWPF.MessageErrorString = _ => { };

            // Act
            var result = UAD_FileValidator.CheckUnicode(null, null);

            // Assert
            result.ShouldBeFalse();
        }

        [Test]
        public void WorkerDoWork_HasHeaders_ReturnTrue()
        {
            // Arrange
            ShimFileWorker.AllInstances.CheckHeadersForUnicodeCharsFileInfoFileConfigurationBoolean =
                (worker, info, _, __) =>
                {
                    var headers = new Dictionary<string, bool>();
                    return headers;
                };
            ShimWPF.MessageErrorString = _ => { };

            // Act
            var result = UAD_FileValidator.CheckUnicode(null, null);

            // Assert
            result.ShouldBeTrue();
        }

        [TestCase(1UL, false, true, false)]
        [TestCase(3UL, true, false, true)]
        [TestCase(5UL, true, false, true)]
        [TestCase(9UL, true, false, true)]
        public void CheckMemory_MemoryPresent_ControlsSet(
            ulong gBytes, 
            bool rbLocalChecked, 
            bool rbOfflineChecked, 
            bool spProcessEnabled)
        {
            // Arrange
            var rbLocal = new RadioButton();
            var rbOffline = new RadioButton();
            var spProcess = new StackPanel();

            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageString = (_, __, ___, ____) => {};

            // Act
            UAD_FileValidator.CheckMemory(
                gBytes * UAD_FileValidator.BytesInKb * UAD_FileValidator.BytesInKb, 
                rbLocal,
                rbOffline,
                spProcess);

            // Assert
            rbLocal.ShouldSatisfyAllConditions(
                () => rbLocal.IsChecked.ShouldBe(rbLocalChecked),
                () => rbOffline.IsChecked.ShouldBe(rbOfflineChecked),
                () => spProcess.IsEnabled.ShouldBe(spProcessEnabled));
        }

        [Test]
        public void ValidateOffline_FileUploaded_IsBusyUnset()
        {
            // Arrange
            var busyIndicator = new RadBusyIndicator();

            var client = new EntityClient
            {
                ClientID = 1
            };

            AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[1] = new ClientAdditionalProperties
            {
                ClientFtpDirectoriesList = new List<ClientFtp>()
            };

            var messageErrorCalled = false;
            ShimWPF.MessageErrorString = _ => { messageErrorCalled = true; };

            var fileInfo = new FileInfo("c:\\1.txt");

            // Act
            UAD_FileValidator.ValidateOffline(fileInfo, client, busyIndicator);

            // Assert
            messageErrorCalled.ShouldBeTrue();
        }
    }
}
