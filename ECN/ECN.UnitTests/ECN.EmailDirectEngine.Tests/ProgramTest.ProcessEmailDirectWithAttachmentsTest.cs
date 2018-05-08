using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mail.Fakes;
using ecn.EmailDirectEngine;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN.EmailDirectEngine.Tests
{
    public partial class ProgramTest
    {
        [TestMethod]
        public void ProcessEmailDirectWithAttachments_OnException_LogAndReturnNegativeNumber()
        {
            // Arrange
            var emailDirect = new EmailDirect();
            var exceptionLogged = false;
            var expectedResult = -1;
            var statusUpdated = false;
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimEmailDirect.UpdateStatusInt32EnumsStatus = (id, stst) =>
              {
                  statusUpdated = true;
              };

            // Act	
            var actualResult = Program.ProcessEmailDirectWithAttachments(emailDirect);

            // Assert
            Assert.IsTrue(exceptionLogged);
            Assert.IsTrue(statusUpdated);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void ProcessEmailDirectWithAttachments_OnECNException_LogAndReturnNegativeNumber()
        {
            // Arrange
            var emailDirect = new EmailDirect()
            {
                EmailDirectID = -1
            };
            var exceptionLogged = false;
            var expectedResult = -1;
            var statusUpdated = false;
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimEmailDirect.UpdateStatusInt32EnumsStatus = (id, stst) =>
            {
                statusUpdated = true;
            };
            ShimEmailDirect.SaveEmailDirect = (ed) =>
            {
                throw new ECNException(new List<ECNError>()
                {
                    new ECNError
                    {
                        ErrorMessage = nameof(ECNException),
                        Entity = Entity.EmailDirect
                    }
                });
            };

            // Act	
            var actualResult = Program.ProcessEmailDirectWithAttachments(emailDirect);

            // Assert
            Assert.IsTrue(exceptionLogged);
            Assert.IsTrue(statusUpdated);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void ProcessEmailDirectWithAttachments_OnValidCall_ReturnPositiveNumber()
        {
            // Arrange
            var emailDirect = new EmailDirect()
            {
                EmailDirectID = 1,
                EmailAddress = "email@test.com",
                FromName = "email@test.com",
                EmailSubject = "EmailSubject",
                Content = "<body>Content</body>",
                ReplyEmailAddress = "email@test.com",
                CCAddresses = new List<string>
                {
                    "email@test.com"
                },
                Attachments = new List<Attachment>
                {
                    new Attachment(new MemoryStream(),"fileName")
                }
            };
            appSettings.Add(AppSettingsKeyActivityDomainPath, "");
            var exceptionLogged = false;
            var statusUpdated = false;
            var messageSent = false;
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimEmailDirect.UpdateStatusInt32EnumsStatus = (id, stst) =>
            {
                statusUpdated = true;
            };
            ShimEmailDirect.SaveEmailDirect = (ed) =>
            {
                throw new ECNException(new List<ECNError>()
                {
                    new ECNError
                    {
                        ErrorMessage = "ECNException",
                        Entity = Entity.EmailDirect
                    }
                });
            };
            ShimSmtpClient.AllInstances.SendMailMessage = (obj, msg) => 
            {
                messageSent = true;
            };

            // Act	
            var actualResult = Program.ProcessEmailDirectWithAttachments(emailDirect);

            // Assert
            Assert.IsFalse(exceptionLogged);
            Assert.IsTrue(statusUpdated);
            Assert.AreEqual(emailDirect.EmailDirectID, actualResult);
            Assert.IsTrue(messageSent);
        }
    }
}
