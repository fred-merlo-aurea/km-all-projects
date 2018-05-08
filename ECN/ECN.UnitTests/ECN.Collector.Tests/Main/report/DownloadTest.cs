using System.Diagnostics.CodeAnalysis;
using System.IO;
using ecn.collector.main.report;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Collector.Tests.main.report
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public partial class DownloadTest : PageHelper
    {
        private const string PageLoadMethodName = "Page_Load";
        private const string FilterID = "FilterID";
        private const string ChIDKey = "chID";
        private const string CustIDKey = "custID";
        private const string GroupIDKey = "grpID";
        private const string SearchEmailKey = "srchEm";
        private const string FileTypeKey = "fileType";
        private const string SearchTypeKey = "srchType";
        private const string SubTypeKey = "subType";
        private const string OneString = "1";
        private const string TestEmailAddress = "test@test.com";
        private const string TestUser = "TestUser";
        private static readonly string TestBasePath = NUnit.Framework.TestContext.CurrentContext.TestDirectory;
        private static readonly string[] FileTypes = new[] { ".xls", ".xml" };

        private download _testEntity;
        private PrivateObject _privateObject;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _testEntity = new download();
            _privateObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
            QueryString.Clear();
            QueryString.Add(FilterID, OneString);
            QueryString.Add(ChIDKey, OneString);
            QueryString.Add(CustIDKey, OneString);
            QueryString.Add(GroupIDKey, OneString);
            QueryString.Add(SearchEmailKey, TestEmailAddress);
            QueryString.Add(FileTypeKey, FileTypes[0]);
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = TestUser, IsActive = true };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        [TearDown]
        public new void CleanUp()
        {
            base.CleanUp();
            var directoryPath = $"{TestBasePath}\\customers\\";
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
    }
}
