using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mail.Fakes;
using AMS_Operations;
using Core_AMS.Utilities.Fakes;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.AMS_Operations
{
    /// <summary>
    /// Unit Test for <see cref="Operations.EmailErrorLog"/>
    /// </summary>
    public partial class OperationTest
    {
        private const string FileImportDirKey = "SubGenSubscriberFileImportFolder";
        private const string FileImportDirValue = "c:\\";
        private const string ErrorNotificationKey = "ErrorNotification";
        private const string ErrorNotificationValue = "test@test.com";
        private const string ErrorMethod = "method";
        private const string ErrorMessage = "test_error_message";
        private const string ErrorDataKey = "test_error_key";
        private const string ErrorDataValue = "test_error_value";

        private MailMessage _sentMessage;

        [Test]
        public void EmailErrorLog_WithFilledArguments_ShouldSendEmail()
        {
            // arrange
            EmailLogSetupShims();

            var args = new object[]
            {
                ErrorMethod,
                ErrorMessage,
                new StringDictionary
                {
                    { ErrorDataKey, ErrorDataValue }
                }, 
                null,
                null,
                null
            };

            // act
            _privateOperationsObj.Invoke(LogImportErrorMethodName, args);
            _privateOperationsObj.Invoke(EmailErrorLogMethodName);

            // assert
            using (var reader = new StreamReader(_sentMessage.Attachments.First().ContentStream))
            {
                var attachmentContent = reader.ReadToEnd();

                _sentMessage.ShouldSatisfyAllConditions(
                    () => _sentMessage.Body.ShouldContain(ErrorMessage),
                    () => _sentMessage.Attachments.ShouldNotBeNull(),
                    () => _sentMessage.Attachments.ShouldNotBeEmpty(),
                    () => attachmentContent.ShouldContain(ErrorDataKey),
                    () => attachmentContent.ShouldContain(ErrorDataValue));
            }
        }

        private void EmailLogSetupShims()
        {
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                {ErrorNotificationKey, ErrorNotificationValue},
                {FileImportDirKey, FileImportDirValue}
            };

            System.IO.Fakes.ShimDirectory.CreateDirectoryString = dirName => null;

            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (fileFunction, dataTable, fileName, deleteExisting) => { };

            ShimFileFunctions.AllInstances.CreateFileStringString =
                (fileFunction, fileName, body) => { };

            ShimSmtpClient.AllInstances.SendMailMessage = (smtpClient, message) => { _sentMessage = message; };
        }
    }
}