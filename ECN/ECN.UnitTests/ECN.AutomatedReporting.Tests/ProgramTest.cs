using System;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using ecn.automatedreporting.Reports;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_Entities.Communicator;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.AutomatedReporting.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class ProgramTest
    {
        private const string AssemblyName = "ECN.AutomatedReporting";
        private const string ClassName = "ecn.automatedreporting.Program";
        private const string FieldLogFile = "logFile";
        private const string AppSettingKeyKMCommon_Application = "KMCommon_Application";
        private const string AppSettingKeyReportPath = "ReportPath";
        private const string AppSettingKeyECNEngineAccessKey = "ECNEngineAccessKey";
        private const string XMLKeyReportSchedule = "ReportSchedule"; 
        private const string ExceptionMessageFTP = "FTP Exception";
        private const string XMLKeyGroupIDs = "GroupIDs";
        private const string XMLKeyGroupID = "GroupID";
        private const string XMLKeyFTPURL = "FTPURL";
        private const string XMLKeyCustomerID = "CustomerID";
        private const string XMLKeyFTPUsername = "FTPUsername";
        private const string XMLKeyFTPPassword = "FTPPassword";
        private const string XMLKeyExportSettings = "ExportSettings";
        private const string XMLKeyExportFormat = "ExportFormat";
        private const string XMLKeyExportSubscribeTypeCode = "ExportSubscribeTypeCode";
        private const string XMLKeyFtpUrl = "FtpUrl";
        private const string XMLKeyFtpUsername = "FtpUsername";
        private const string XMLKeyFtpPassword = "FtpPassword";
        private const string FileFormatTxt = ".txt";
        private const string FileFormatCsv = ".csv";
        private const string FileFormatXls = ".xls";
        private const string FileFormatXml = ".xml";
        private const string DummyText = "Dummy";
        private const string ValueText1 = "1";
        private const string ValueText0 = "0";
        private const int ValueInt1 = 1;

        private IDisposable _shimObject;
        private PrivateType _testedClass;
        private string _exceptionMessage = string.Empty;
        private bool _anyException;
        private ReportSchedule _reportSchedule;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _testedClass = new PrivateType(AssemblyName, ClassName);
            ReportsHelper.LogFile = new ShimStreamWriter();

            KM.Common.Entity.Fakes.ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, name, appId, note, gd, ec) =>
                {

                    _exceptionMessage = ex.Message;
                    _anyException = true;
                    return 0;
                };
            _anyException = false ;

            ShimConfigurationManager.AppSettingsGet =
                () => new System.Collections.Specialized.NameValueCollection
                {
                    { AppSettingKeyFilePath, DummyFilePath },
                    { AppSettingKeyKMCommon_Application, ValueText1 },
                    { AppSettingKeyReportPath, DummyFilePath },
                    { AppSettingKeyECNEngineAccessKey, DummyFilePath }                    
                };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}
