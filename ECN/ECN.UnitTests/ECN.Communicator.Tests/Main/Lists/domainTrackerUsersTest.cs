using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using ecn.communicator.main.lists;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.DomainTracker.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.DomainTracker;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    /// UT for <see cref="domainTrackerUsers"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DomainTrackerUsersTest : BaseListsTest<domainTrackerUsers>
    {
        [Test]
        public void Page_Load_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblDomainName = privateObject.GetFieldOrProperty("lblDomainName") as Label;
            ShimDomainTracker.GetByDomainTrackerIDInt32User = (p1, p2) => new DomainTracker { Domain = "TestDomain" };
            ShimDomainTrackerUserProfile.GetByDomainTrackerIDInt32NullableOfInt32NullableOfInt32UserStringStringStringStringStringStringString =
                (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => new List<DomainTrackerUserProfile> { new DomainTrackerUserProfile { } };

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => phError.Visible.ShouldBeFalse(),
                () => lblDomainName.Visible.ShouldBeTrue(),
                () => lblDomainName.Text.ShouldBe("User Profiles from TestDomain"));
        }

        [Test]
        public void LoadProfileDetails_Success([Values("-Details", "+Details")]string type)
        {
            // Arrange
            InitilizeTestObjects();
            var gvDomainTrackingUsers = privateObject.GetFieldOrProperty("gvDomainTrackingUsers") as GridView;
            gvDomainTrackingUsers.DataSource = new List<DomainTrackerUserProfile> { new DomainTrackerUserProfile { } };
            gvDomainTrackingUsers.DataBind();
            gvDomainTrackingUsers.Rows[0].Cells.Add(new TableCell());
            var pnlProfileReport = new Panel { ID = "pnlProfileReport" };
            gvDomainTrackingUsers.Rows[0].Cells[0].Controls.Add(pnlProfileReport);
            var tabContainer = new TabContainer { ID = "TabContainer1" };
            var tabStandard = new TabPanel { ID = "TabStandard" };
            tabStandard.Controls.Add(new GridView { ID = "gvStandardDataPoints" });
            var tabAdditional = new TabPanel { ID = "TabAdditional" };
            tabAdditional.Controls.Add(new GridView { ID = "gvAdditionalDataPoints" });
            tabContainer.Controls.Add(tabStandard);
            tabContainer.Controls.Add(tabAdditional);
            gvDomainTrackingUsers.Rows[0].Cells[0].Controls.Add(tabContainer);
            gvDomainTrackingUsers.Rows[0].Cells[0].Controls.Add(new Label { ID= "lblProfileID", Text = "1"});
            gvDomainTrackingUsers.Rows[0].Cells[0].Controls.Add(new LinkButton { ID = "lnkbtnProfile", Text = type });
            ShimDomainTrackerActivity.GetByProfileIDInt32Int32UserStringStringString = (p1, p2, p3, p4, p5, p6) => new List<DomainTrackerActivity> { new DomainTrackerActivity { } };
            ShimDomainTrackerValue.GetByProfileIDInt32Int32User = (p1, p2, p3) => new DataTable { };

            // Act
            privateObject.Invoke("loadProfileDetails", new object[] { 0 });

            // Assert
            pnlProfileReport.Visible.ShouldBe(type == "+Details");
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
            QueryString = new NameValueCollection();
            RedirectUrl = string.Empty;
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer();
                session.CurrentBaseChannel = new BaseChannel();
                return session;
            };
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            var sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;
            ShimUserControl.AllInstances.SessionGet = (p) => sessionState;
        }
	}
}