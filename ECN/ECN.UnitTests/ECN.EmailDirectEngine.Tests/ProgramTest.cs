using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECN.EmailDirectEngine.Tests
{
    [TestClass]
    public partial class ProgramTest
    {
        private IDisposable _shimObject;
        private readonly NameValueCollection appSettings = new NameValueCollection();
        private const string smtpServerPort = "25";
        private const string smtpServer = "10.10.208.101";
        private const string ecnEngineAccessKey = "651A1297-59D1-4857-93CB-0B049635762E";
        private const string AppSettingsKeyActivityDomainPath = "Activity_DomainPath";

        [TestInitialize]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            appSettings.Add("SmtpServerPort", smtpServerPort);
            appSettings.Add("SmtpServer", smtpServer);
            appSettings.Add("ECNEngineAccessKey", ecnEngineAccessKey);
            appSettings.Add("KMCommon_Application", "1");
            ShimConfigurationManager.AppSettingsGet = () => appSettings;
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}
