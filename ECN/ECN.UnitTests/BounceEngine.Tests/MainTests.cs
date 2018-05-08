using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Configuration.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using System.Collections.Specialized;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Data.SqlClient;
using BounceEngine.Tests.Helpers;
using aspNetMime;
using aspNetPOP3;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BounceEngine.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class MainTests
    {
        const int InboxMessageCount = 2;
        const string MessageToEmailAddress = "to.address@domain";
        const string MessageToValidEmailAddressBounceDomain = "bounce_123-456@kmpsgroupbounce.com";
        const string MessageToInvalidEmailAddressBounceDomain = "bounce_qwe-asd@kmpsgroupbounce.com";
        const string MessageFromEmailAddress = "from.address@domain";
        const string MessageDate = "2/14/2018 10:42:18 AM";
        const string TextMessageOurEmail = "something@enterprisecommunicationnetwork.com";
        const string TextMessageOtherEmail = "something@othermail.com";

        [Test]
        public void Main_IfPOP3Exception_CallCancelDeletes()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = 0;
            _testContext.POP3DisconnectShouldThrowPOP3Exception = true;

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.POP3CancelDeletesCalled);
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
            Assert.IsTrue(_testContext.LogNonCriticalErrorCalled);
        }

        [Test]
        public void Main_IfOutOfMemoryExceptionThrown_CallCancelDeletes()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = 0;
            _testContext.POP3DisconnectShouldThrowOutOfMemoryException = true;

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.POP3CancelDeletesCalled);
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
            Assert.IsTrue(_testContext.LogCriticalErrorCalled);
        }

        [Test]
        public void Main_IfExceptionThrown_CallCancelDeletes()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = 0;
            _testContext.POP3DisconnectShouldThrowException = true;

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.POP3CancelDeletesCalled);
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
            Assert.IsTrue(_testContext.LogCriticalErrorCalled);
        }

        [TestMethod]
        public void Main_IfNotIsConnected_CallReconnect()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.POP3IsConnectedReturnValue = false;
            _testContext.AppSettingsParallelThreadsValid = true;
            //Act
            CalllBounceEngineMainMethod();
            //Assert
            Assert.IsTrue(_testContext.POP3ReconnectCalled);
        }

        [Test]
        public void Main_IfPOP3GetMessageAsTextThrowException_CallDeleteMessageWithCheck()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.POP3GetMessageAsTextShouldThrowException = true;

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfMimeMessageEmailAddressesAndDateStringAreNull_ShouldSwallowException()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(null);
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            //Act, Assert
            CalllBounceEngineMainMethod();
        }

        [Test]
        public void Main_IfDeleteNoneBounceIsTrueAndToEmailNotContainedInBounceDomains_ShouldCallDeleteMessageWithCheck()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.DeleteNonBouncesReturnValue = true;

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfSqlExceptionTthrown_ShouldLogConsoleActivity()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.POP3IsConnectedExceptionToThrow = ReflectionHelper.NewSqlException();

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
        }

        [Test]
        public void Main_IfArgumentOutOfRangeExceptionAndDeleteNoneBounceIsTrue_LogConsoleActivityAndDeleteMessageWithCheck()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.POP3IsConnectedExceptionToThrow = new ArgumentOutOfRangeException();
            _testContext.DeleteNonBouncesReturnValue = true;

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfArgumentOutOfRangeException_LogConsoleActivity()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.POP3IsConnectedExceptionToThrow = new ArgumentOutOfRangeException();

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
        }

        [Test]
        public void Main_IfMimeExceptionThronwAndDeleteNonBounceIsTrue_LogConsoleActivityAndDeleteMessageWithCheck()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.POP3IsConnectedExceptionToThrow = new MimeException();
            _testContext.DeleteNonBouncesReturnValue = true;

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfMimeExceptionThronw_LogConsoleActivity()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.POP3IsConnectedExceptionToThrow = new MimeException();

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
        }

        [Test]
        public void Main_IfArgumentExceptionThrownAndAndDeleteNonBounceIsTrue_LogConsoleActivityAndDeleteMessageWithCheck()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.POP3IsConnectedExceptionToThrow = new ArgumentException();
            _testContext.DeleteNonBouncesReturnValue = true;

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfArgumentExceptionThronw_LogConsoleActivity()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.POP3IsConnectedExceptionToThrow = new ArgumentException();

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
        }

        [Test]
        public void Main_IfOutOfMemoryExceptionThrow_LogConsoleActivityAndLogCriticalError()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.POP3IsConnectedExceptionToThrow = new OutOfMemoryException();

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
            Assert.IsTrue(_testContext.LogCriticalErrorCalled);
        }

        [Test]
        public void Main_IfPOP3ProtocolExceptionThrown_ShouldLogNonCriticalErrorAndCallDeleteMessageWithCheck()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.POP3IsConnectedExceptionToThrow = new POP3ProtocolException();

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
            Assert.IsTrue(_testContext.LogNonCriticalErrorCalled);
            Assert.IsTrue(_testContext.POP3DisconnectCalled);
            Assert.IsTrue(_testContext.POP3ConnectCalled);
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfExceptionThrown_ShouldLogCriticalError()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToEmailAddress);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = string.Empty;
            _testContext.POP3IsConnectedExceptionToThrow = new Exception();

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
            Assert.IsTrue(_testContext.LogCriticalErrorCalled);
        }

        [Test]
        public void Main_IfToEmailValidAndHasValidBounceDomainAndFileExists_CallFileDelete()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToValidEmailAddressBounceDomain);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = "SomeMail@enterprisecommunicationnetwork.com";
            _testContext.FileExistsPathToReturnTrue = "BOUNCE_EMAIL_";

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.FileDeleteCalled);
            Assert.IsTrue(_testContext.POP3SaveToFileCalled);
        }

        [Test]
        public void Main_IfToEmailValidAndHasValidBounceDomainAndDateTimeIsInvalid_CallPOP3SavtToFile()
        {
            //Arrange
            var invalidDate = DateTime.MinValue.ToString();
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToValidEmailAddressBounceDomain);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = invalidDate;
            _testContext.POP3GetMessageAsTextReturnValue = "SomeMail@enterprisecommunicationnetwork.com";

            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.POP3SaveToFileCalled);
        }

        [Test]
        public void Main_IfToEmailValidAndHasValidBounceDomainAndFileExists_CallPOP3SaveToFile()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToValidEmailAddressBounceDomain);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = "SomeMail@enterprisecommunicationnetwork.com";
            //Act
            CalllBounceEngineMainMethod();
            //Assert
            Assert.IsTrue(_testContext.POP3SaveToFileCalled);
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfToEmailContainsDoubleQuotes_CallPOP3SaveToFile()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add("\"" + MessageToValidEmailAddressBounceDomain);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = "SomeMail@enterprisecommunicationnetwork.com";
            //Act
            CalllBounceEngineMainMethod();
            //Assert
            Assert.IsTrue(_testContext.POP3SaveToFileCalled);
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfPOP3SaveToFileThrowsException_CallLogConsoleActivity()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            ECN.engines.BounceEngine.WriteMessageToFileIfWeCannotParseTheToAddress = true;
            _testContext.MimeMessageToAddresses.Add(MessageToInvalidEmailAddressBounceDomain);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = "SomeMail@enterprisecommunicationnetwork.com";
            _testContext.POP3SaveToFileExceptionToThrow = new Exception();
            //Act
            CalllBounceEngineMainMethod();
            //Assert
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfFromEmailThrowsNullReferenceException_CallPOP3SaveToFile()
        {
            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToValidEmailAddressBounceDomain);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = "SomeMail@enterprisecommunicationnetwork.com";
            _testContext.MimeMessageFromExceptionToThrow = new NullReferenceException();
            //Act
            CalllBounceEngineMainMethod();
            //Assert
            Assert.IsTrue(_testContext.POP3SaveToFileCalled);
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfToEmailValidButMessageHasNoValidBounceDomain_ShouldCallDeleteMessageWithCheck()
        {

            //Arrange
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToValidEmailAddressBounceDomain);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = "SomeMail@_enterprisecommunicationnetwork.com";
            //Act
            CalllBounceEngineMainMethod();
            //Assert
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
            Assert.IsTrue(_testContext.LogConsoleActivityCalled);
        }

        [Test]
        public void Main_IfToEmailInvalidAndHasBounceDomainAndWriteCannotPareToAddressTrue_CallPOP3SaveToFile()
        {
            //Arrange
            ECN.engines.BounceEngine.WriteMessageToFileIfWeCannotParseTheToAddress = true;
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToInvalidEmailAddressBounceDomain);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = TextMessageOurEmail;
            //Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsTrue(_testContext.POP3SaveToFileCalled);
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_IfToEmailInvalidAndHasBounceDomainAndWriteCannotPareToAddressFalse_POP3SaveToFileNotCalled()
        {
            //Arrange
            ECN.engines.BounceEngine.WriteMessageToFileIfWeCannotParseTheToAddress = false;
            _testContext.POP3InboxMessageCountReturnValue = InboxMessageCount;
            ECN.engines.BounceEngine.msgsToProcess = InboxMessageCount + 1;
            _testContext.MimeMessageToAddresses.Add(MessageToInvalidEmailAddressBounceDomain);
            _testContext.MimeMessageFromAddress = MessageFromEmailAddress;
            _testContext.MimeMessageDate = MessageDate;
            _testContext.POP3GetMessageAsTextReturnValue = TextMessageOurEmail;
            //Act
            CalllBounceEngineMainMethod();
            //Assert
            Assert.IsFalse(_testContext.POP3SaveToFileCalled);
            Assert.IsTrue(_testContext.BounceEngineDeleteMessageWithCheckCalled);
        }

        [Test]
        public void Main_NoExceptionThrown_CancelDeletesNotCalled()
        {
            //Arrange, Act
            CalllBounceEngineMainMethod();

            //Assert
            Assert.IsFalse(_testContext.POP3CancelDeletesCalled);
        }
    }
}