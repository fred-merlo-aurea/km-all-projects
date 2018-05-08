using aspNetPOP3.Fakes;
using KM.Common;
using KM.Common.Entity.Fakes;
using KM.Common.Fakes;
using ListNanny.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.IO.Fakes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using aspNetPOP3;
using aspNetMime.Fakes;
using aspNetMime;
using ECN.engines.Fakes;
using System.Xml;
using System.Data;
using System.IO;
using System.Collections.Concurrent;
using System.Configuration;
using NUnit.Framework;

namespace BounceEngine.Tests
{
    public partial class MainTests
    {
        private IDisposable _shimsContext;
        private const string True = "True";
        private const string False = "False";
        private const string AnyPath = "Any/Path";
        private const string BounceConfigFileName = "bounce_conf.xml";
        private const string PagerConfigFileName = "pager_conf.xml";
        private const string BounceSignaturesFileName = "bounce_signatures.xml";
        private const string Timeout = "10";
        private const int LogId = 0;
        private TestContext _testContext;

        [SetUp]
        public void Initialize()
        {
            _testContext = new TestContext();
            _shimsContext = ShimsContext.Create();
            SetupShims();
        }

        [TearDown]
        public void Cleanup()
        {
            _shimsContext.Dispose();
        }

        private void SetupShims()
        {
            ShimConfigurationManager.AppSettingsGet = AppSettings;
            ShimNDR.ImportDefinitionFileString = NDRImportDefinitionFile;
            ShimFile.ExistsString = FileExists;
            ShimFile.DeleteString = FileDelete;
            ShimFileFunctions.LogConsoleActivityStringString = LogConsoleActivity;
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 = LogNonCriticalError;
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = LogCriticalError;
            ShimPOP3.ConstructorStringStringString = POP3Constructor;
            ShimPOP3.AllInstances.TimeOutSetInt32 = POP3TimeOutSet;
            ShimPOP3.AllInstances.Connect = POP3Connect;
            ShimPOP3.AllInstances.Disconnect = POP3Disconnect;
            ShimPOP3.AllInstances.PopulateInboxStats = POP3PopulateInboxStats;
            ShimPOP3.AllInstances.InboxMessageCountGet = POP3InboxMessageCountGet;
            ShimPOP3.AllInstances.IsConnectedGet = POP3IsConnectedGet;
            ShimPOP3.AllInstances.Reconnect = POP3Reconnect;
            ShimPOP3.AllInstances.GetMessageAsTextInt32 = POP3GetMessageAsText;
            ShimPOP3.AllInstances.CancelDeletes = POP3CancelDeletes;
            ShimPOP3.AllInstances.SaveToFileInt32String = POP3SaveToFile;
            SetupAddressCollectionShims();
            ShimMimeMessage.ConstructorString = MimeMessageConstructor;
            ShimMimeMessage.ConstructorString = (message, text) => { };
            ShimMimeMessage.AllInstances.ToGet = MimeMessageToAddessCollectionGet;
            ShimMimeMessage.AllInstances.AttachmentsGet = MimeMessageAttachementsGet;
            ShimMimeMessage.AllInstances.FromGet = MimeMessageFromGet;
            ShimMimeMessage.AllInstances.DateGet = MimeMessageDateGet;
            ShimMimeMessage.AllInstances.GetHeaderString = MimeMessageGetHeader;
            ShimMimeMessage.AllInstances.SubjectGet = MimeMessageSubjectGet;
            ShimBounceEngine.DeleteMessageWithCheckPOP3Int32 = BounceEngineDeleteMessageWithCheck;
            ShimBounceEngine.LoadXMLFileString = BouceEngineLoadXMLFile;
        }

        private bool FileExists(string filePath)
        {
            var result = false;
            if (_testContext.FileExistsPathToReturnTrue != null)
            {
                result = filePath.Contains(_testContext.FileExistsPathToReturnTrue);
            }
            if (!result)
            {
                return ShimsContext.ExecuteWithoutShims(() => File.Exists(filePath));
            }
            return result;
        }

        private void FileDelete(string filePath)
        {
            _testContext.FileDeleteCalled = true;
        }

        private void POP3SaveToFile(POP3 pop3, int index, string filePath)
        {
            _testContext.POP3SaveToFileCalled = true;
            var exception = _testContext.POP3SaveToFileExceptionToThrow;
            if (exception != null)
            {
                _testContext.POP3SaveToFileExceptionToThrow = null;
                throw exception;
            }
        }

        private void SetupAddressCollectionShims()
        {
            var addressesStore = new List<Address>();
            var addressStrings = new Dictionary<Address, string>();
            aspNetMime.Fakes.ShimAddressCollection.Constructor = collection => { };
            aspNetMime.Fakes.ShimAddressCollection.AllInstances.AddAddress = (collection, address) =>
            {
                addressesStore.Add(address);
                return addressesStore.Count;
            };
            aspNetMime.Fakes.ShimAddressCollection.AllInstances.ItemGetInt32 = (collection, index) =>
            {
                return addressesStore[index];
            };
            aspNetMime.Fakes.ShimAddress.ConstructorString = (address, rawAddress) =>
            {
                addressStrings[address] = rawAddress;
            };
            aspNetMime.Fakes.ShimAddress.Constructor = (address) => { };
            aspNetMime.Fakes.ShimAddress.AllInstances.EmailAddressGet = address =>
            {
                return addressStrings[address];
            };
            aspNetMime.Fakes.ShimAddress.AllInstances.RawAddressGet = address =>
            {
                return addressStrings[address];
            };
        }

        private Header MimeMessageSubjectGet(MimeMessage message)
        {
            return new Header();
        }

        private Header MimeMessageGetHeader(MimeMessage message, string header)
        {
            return new Header($"{header}:{header}_Value");
        }

        private string MimeMessageDateGet(MimeMessage message)
        {
            return _testContext.MimeMessageDate;
        }

        private Address MimeMessageFromGet(MimeMessage message)
        {
            var exception = _testContext.MimeMessageFromExceptionToThrow;
            if (exception != null)
            {
                _testContext.MimeMessageFromExceptionToThrow = null;
                throw exception;
            }
            var result = new aspNetMime.Fakes.StubAddress
            {
                ToString01 = () => _testContext.MimeMessageFromAddress
            };
            return result;
        }

        private MimePartCollection MimeMessageAttachementsGet(MimeMessage message)
        {
            return new MimePartCollection();
        }

        private AddressCollection MimeMessageToAddessCollectionGet(MimeMessage message)
        {
            var result = new AddressCollection();
            foreach (var rawAddress in _testContext.MimeMessageToAddresses)
            {
                result.Add(new Address(rawAddress));
            }
            return result;
        }

        private void POP3Disconnect(POP3 pop3)
        {
            _testContext.POP3DisconnectCalled = true;
            if (_testContext.POP3DisconnectShouldThrowException)
            {
                _testContext.POP3DisconnectShouldThrowException = false;
                throw new Exception();
            }
            if (_testContext.POP3DisconnectShouldThrowOutOfMemoryException)
            {
                _testContext.POP3DisconnectShouldThrowOutOfMemoryException = false;
                throw new OutOfMemoryException();
            }
            if (_testContext.POP3DisconnectShouldThrowPOP3Exception)
            {
                _testContext.POP3DisconnectShouldThrowPOP3Exception = false;
                throw new POP3Exception();
            }
        }

        private void POP3CancelDeletes(POP3 pop3)
        {
            _testContext.POP3CancelDeletesCalled = true;
        }

        private void POP3Constructor(POP3 pop3, string server, string userName, string password)
        {
        }

        private DataSet BouceEngineLoadXMLFile(string filePath)
        {
            var result = new DataSet();
            ShimsContext.ExecuteWithoutShims(() =>
            {
                var xml = string.Empty;
                if (filePath == BounceConfigFileName)
                {
                    xml = GetBoundConfigXml();
                }
                else if (filePath == PagerConfigFileName)
                {
                    xml = GetPagerConfigXml();
                }
                else if (filePath == BounceSignaturesFileName)
                {
                    xml = File.ReadAllText(BounceSignaturesFileName);
                }
                using (var stringReader = new StringReader(xml))
                {
                    result.ReadXml(stringReader);
                }
            });
            return result;
        }

        private void NDRImportDefinitionFile(string path)
        {
            _testContext.NDRImportDefinitionFileCalled = true;
        }

        private void BounceEngineDeleteMessageWithCheck(POP3 pop3, int messageIndex)
        {
            _testContext.BounceEngineDeleteMessageWithCheckCalled = true;
        }

        private void MimeMessageConstructor(MimeMessage message, string text)
        {
            if (_testContext.MimeMessageConstructorShouldThrowException)
            {
                _testContext.MimeMessageConstructorShouldThrowException = false;
                throw new Exception();
            }
        }

        private string POP3GetMessageAsText(POP3 pop3, int messageIndex)
        {
            _testContext.POP3GetMessageAsTextCalled = true;
            _testContext.POP3InboxMessageCountReturnValue--;
            if (_testContext.POP3GetMessageAsTextShouldThrowException)
            {
                _testContext.POP3GetMessageAsTextShouldThrowException = false;
                throw new Exception();
            }
            if (_testContext.POP3GetMessageAsTextShouldReturnNull)
            {
                return null;
            }
            return _testContext.POP3GetMessageAsTextReturnValue;
        }

        private void POP3Reconnect(POP3 pop3)
        {
            _testContext.POP3ReconnectCalled = true;
        }

        private bool POP3IsConnectedGet(POP3 pop3)
        {
            if (_testContext.POP3IsConnectedExceptionToThrow != null)
            {
                var exception = _testContext.POP3IsConnectedExceptionToThrow;
                _testContext.POP3IsConnectedExceptionToThrow = null;
                throw exception;
            }
            return _testContext.POP3IsConnectedReturnValue;
        }

        private int POP3InboxMessageCountGet(POP3 pop3)
        {
            return _testContext.POP3InboxMessageCountReturnValue;
        }

        private void POP3PopulateInboxStats(POP3 pop3)
        {
            _testContext.POP3PopulateInboxStatsCalled = true;
        }

        private void POP3Connect(POP3 pop3)
        {
            _testContext.POP3ConnectCalled = true;
        }

        private void POP3TimeOutSet(POP3 pop3, int timeout)
        {
            _testContext.POP3TimeoutSet = true;
        }

        private int LogCriticalError(Exception ex, string sourceMethod, int applicationID, string note,
                                     int gdCharityID, int ecnCustomerID)
        {
            _testContext.LogCriticalErrorCalled = true;
            return LogId;
        }

        private int LogNonCriticalError(string error, string sourceMethod, int applicationID, string note,
                                        int gdCharityID, int ecnCustomerID)
        {
            _testContext.LogNonCriticalErrorCalled = true;
            return LogId;
        }

        private void LogConsoleActivity(string activity, string logSuffix)
        {
            _testContext.LogConsoleActivityCalled = true;
        }

        private NameValueCollection AppSettings()
        {
            var parallelThreadsCount = _testContext.AppSettingsParallelThreadsValid ? "2" : null;
            return new NameValueCollection
            {
                {"LastestNDR_DefPath", AnyPath},
                { "msgsToProcess", _testContext.AppSettingsMessagesToProcess.ToString()},
                { "TestMode", False },
                { "DeleteCompleted", True},
                { "MessageFiles.ToProcessFolderPath", AnyPath },
                { "MessageFiles.ProcessingFailedFolderPath", AnyPath },
                { "MessageFiles.ProcessingCompletedFolderPath", AnyPath },
                {
                    "WriteMessageToFileIfWeCannotParseTheToAddress",
                    _testContext.AppSettingsWriteMessageToFileIfWeCannotParseTheToAddress.ToString()
                },
                { "NotificationValues", string.Empty },
                { "OutLog", "OutLog" },
                { "ParallelThreads", parallelThreadsCount },
                { "NDR.Timeout", Timeout},
                {
                    "NDR.LicenseKey",
                    "QUQ58-CTV97-G9598-RPPV6-5D4E1-AQYD8-RE9V2-RBQ1A-RCRU7-XDXK4-JY6JT-5HCUQ-3LFVY-DKT3L-SX"
                },
                {
                    "aspNetPOP3.LicenseKey",
                    "DGFNQ-QZFQY-SLA9Q-HKC3Q-5PA11-AU639-KTEH9-5JA1C-CJWRC-RU7XS-XK6UY-6JTSW-6VX2L-SFY88-Q"
                }
            };
        }

        private void CalllBounceEngineMainMethod()
        {
            typeof(ECN.engines.Utility.BlastInfo)
                .Assembly
                .GetType("ECN.engines.BounceEngine")
                .GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic)
                .Invoke(null, new[] { new string[0] });
        }

        private string GetBoundConfigXml()
        {
            var deleteNonBounces = _testContext.DeleteNonBouncesReturnValue
                ? "Y"
                : "N";
            var BounceConfigXml = $@"
			<configtable>
				<mailsettings>
					<mailServer>216.17.41.221</mailServer>
					<mailUser>km\catchall</mailUser>
					<mailPass>Computer1</mailPass>
					<popLog>aspNetPOP3.log</popLog>
					<outLog>bounceout.log</outLog>
					<deleteNonBounces>{deleteNonBounces}</deleteNonBounces>
					<bounceDomains>kmpsgroupbounce.com</bounceDomains>     
					<masterSuppressBounceDomains>Y</masterSuppressBounceDomains>   
				<masterSuppressBounceDomainsList></masterSuppressBounceDomainsList>	
				<toEmail>dev@knowledgemarketing.com</toEmail>
				<fromEmail>info@knowledgemarketing.com</fromEmail>
				<subject>KMPS bounce engine</subject>
				<notificationToEmail>dev@knowledgemarketing.com</notificationToEmail>
				</mailsettings>
			</configtable>";
            return BounceConfigXml;
        }

        private string GetPagerConfigXml()
        {
            const string PagerConfigXml = @"
			<configtable>
			  <mailsettings>
				<mailServer>216.17.41.200</mailServer>
				<fromEmail>bounce@kmpsgroupbounce.com</fromEmail>
				<toEmail>dev@knowledgemarketing.com</toEmail>
				<notificationToEmail>BLMonitor@TeamKM.com</notificationToEmail>
				<subject>Error Notification Pager</subject>
			  </mailsettings>
			</configtable>";
            return PagerConfigXml;
        }
    }
}
