using System.Diagnostics.CodeAnalysis;
using ecn.communicator.main.Reports;
using ecn.communicator.main.Reports.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Reports
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ScheduledReportSetupTest : PageHelper
    {
        private const string ReportScheduleIdKey = "ReportSchedule";

        private scheduledReportSetup _testEntity;
        private PrivateObject _privateTestObject;
        private ReportSchedule _savedReportSchedule;
        private bool _isSavedReportSchedule;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _savedReportSchedule = null;
            _isSavedReportSchedule = false;
            _testEntity = new scheduledReportSetup();
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        private void InitializeSessionFakes()
        {
            QueryString.Clear();
            QueryString.Add(ReportScheduleIdKey, "1");
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User()
            {
                UserID = 1,
                UserName = "TestUser",
                IsActive = true,
                CustomerID = 1
            };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimscheduledReportSetup.AllInstances.MasterGet = (s) => new ecn.communicator.MasterPages.Communicator { };
            ecn.communicator.MasterPages.Fakes.ShimCommunicator.AllInstances.UserSessionGet = (m) => shimSession.Instance;
        }
    }
}
