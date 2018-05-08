using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BounceEngine.Tests
{
    [ExcludeFromCodeCoverage]
    class TestContext
    {
        public TestContext()
        {
            MimeMessageToAddresses = new List<string>();
        }

        public bool NDRLoadLicenseKeyCalled { get; set; }
        public bool POP3TimeoutSet { get; set; }
        public bool POP3ConnectCalled { get; set; }
        public bool POP3PopulateInboxStatsCalled { get; set; }
        public int POP3InboxMessageCountReturnValue { get; set; }//>0 and ==0
        public bool POP3DisconnectCalled { get; set; }
        public bool POP3CancelDeletesCalled { get; set; }
        public bool POP3ReconnectCalled { get; set; }
        public bool POP3IsConnectedReturnValue { get; set; }
        public bool POP3GetMessageAsTextCalled { get; set; }
        public bool POP3GetMessageAsTextShouldThrowException { get; set; }
        public bool POP3GetMessageAsTextShouldReturnNull { get; set; }
        public string POP3GetMessageAsTextReturnValue { get; set; }
        public bool POP3SaveToFileCalled { get; set; }
        public Exception POP3SaveToFileExceptionToThrow { get; set; }
        public bool POP3DisconnectShouldThrowPOP3Exception { get; set; }
        public bool POP3DisconnectShouldThrowOutOfMemoryException { get; set; }
        public bool POP3DisconnectShouldThrowException { get; set; }
        public Exception POP3IsConnectedExceptionToThrow { get; set; }
        public bool LogCriticalErrorCalled { get; set; }
        public bool LogNonCriticalErrorCalled { get; set; }
        public bool LogConsoleActivityCalled { get; set; }
        public bool MimeMessageConstructorShouldThrowException { get; set; }
        public IList<string> MimeMessageToAddresses { get; private set; }
        public string MimeMessageFromAddress { get; set; }
        public Exception MimeMessageFromExceptionToThrow { get; set; }
        public string MimeMessageDate { get; set; }
        public int AppSettingsMessagesToProcess { get; set; }
        public bool AppSettingsWriteMessageToFileIfWeCannotParseTheToAddress { get; set; }
        public bool AppSettingsParallelThreadsValid { get; set; }
        public bool DeleteNonBouncesReturnValue { get; set; }
        public bool BounceEngineDeleteMessageWithCheckCalled { get; set; }
        public bool NDRImportDefinitionFileCalled { get; set; }
        public string FileExistsPathToReturnTrue { get; set; }
        public bool FileDeleteCalled { get; set; }
    }
}