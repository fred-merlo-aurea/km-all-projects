using System;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.lists;
using ecn.communicator.main.lists.Fakes;
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
    /// UT for <see cref="DomainTrackerReport"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DomainTrackerReportTest : BaseListsTest<DomainTrackerReport>
    {
        [Test]
        public void Page_Load_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimDomainTrackerReport.AllInstances.CreateChart = (p) => { };
            ShimDomainTrackerReport.AllInstances.GetURLStats = (p) => { };
            ShimDomainTracker.GetByDomainTrackerIDInt32User = (p1, p2) => new DomainTracker { Domain = "TestDomain" };
            var sampleTable = new DataTable { Columns = { "Views" }, Rows = { { "0" } } };
            ShimDomainTrackerActivity.GetOSStatsInt32 = (p) => sampleTable;
            ShimDomainTrackerActivity.GetBrowserStatsInt32 = (p) => sampleTable;
            ShimDomainTrackerActivity.GetTotalViewsInt32 = (p) => sampleTable;
            var lblDomainTracker = privateObject.GetFieldOrProperty("lblDomainTracker") as Label;
            var lblTotalViews = privateObject.GetFieldOrProperty("lblTotalViews") as Label;
            var lblPlatformStats = privateObject.GetFieldOrProperty("lblPlatformStats") as Label;
            var lblMostVisitedPages = privateObject.GetFieldOrProperty("lblMostVisitedPages") as Label;
            var lblBrowserStats = privateObject.GetFieldOrProperty("lblBrowserStats") as Label;
            var gvBrowserStats = privateObject.GetFieldOrProperty("gvBrowserStats") as GridView;
            var gvPlatformStats = privateObject.GetFieldOrProperty("gvPlatformStats") as GridView;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => lblDomainTracker.Text.ShouldBe("Domain Tracking Report (TestDomain)"),
                () => lblTotalViews.Text.ShouldBe("Total Page Views: 0"),
                () => lblPlatformStats.Visible.ShouldBeFalse(),
                () => lblMostVisitedPages.Visible.ShouldBeFalse(),
                () => lblBrowserStats.Visible.ShouldBeFalse(),
                () => gvBrowserStats.Rows.Count.ShouldBePositive(),
                () => gvPlatformStats.Rows.Count.ShouldBePositive());
        }

        [Test]
        public void GetURLStats_Success([Values(-1,7, 30, 60, 90, 365)]int period)
        {
            // Arrange
            InitilizeTestObjects();
            var ddlMostVisitedPagesRange = privateObject.GetFieldOrProperty("ddlMostVisitedPagesRange") as DropDownList;
            ddlMostVisitedPagesRange.Items.Add(period.ToString());
            ddlMostVisitedPagesRange.SelectedValue = period.ToString();
            var startDate = new DateTime();
            var endDate = new DateTime();
            ShimDomainTrackerActivity.GetURLStatsInt32StringStringStringInt32String = (p1, p2, p3, p4, p5, p6) =>
            {
                startDate = DateTime.Parse(p2);
                endDate = DateTime.Parse(p3);
                return new DataTable { Columns = { "Views" }, Rows = { { "0" } } };
            };
            var lblMostVisitedPagesRange = privateObject.GetFieldOrProperty("lblMostVisitedPagesRange") as Label;

            // Act
            privateObject.Invoke("GetURLStats", new object[] { });

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => startDate.ShouldBe(period == -1 ? DateTime.MinValue : endDate.AddDays(-1*period)),
                () => ddlMostVisitedPagesRange.Visible.ShouldBeTrue(),
                () => lblMostVisitedPagesRange.Visible.ShouldBeTrue());
        }

        [Test]
        public void CreateChart_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimDomainTrackerActivity.GetPageViewsPerDayInt32 = (p) => new DataTable();
            var chtPageViews = privateObject.GetFieldOrProperty("chtPageViews") as Chart;
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            phError.Visible = false;

            // Act
            privateObject.Invoke("CreateChart", new object[] { });

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => chtPageViews.ShouldNotBeNull(),
                () => chtPageViews.Series["PageViews"].ShouldNotBeNull(),
                () => phError.Visible.ShouldBeFalse());
        }

        [Test]
        public void GvPlatformStats_RowDataBound_Success([Values("linux", "windows", "android", "iphone", "default")]string platform)
        {
            // Arrange
            InitilizeTestObjects();
            var imageUrl = "~/images/img" + (platform == "linux" ? "Linux" : platform == "windows" ? "Windows" : platform == "android" ? "Android" : platform == "iphone" ? "Apple" : "Other") + ".png";
            var row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            row.Cells.Add(new TableCell());
            row.Cells[0].Controls.Add(new Label { ID = "lblOS", Text = platform });
            var imgPlatform = new System.Web.UI.WebControls.Image { ID = "imgPlatform" };
            row.Cells[0].Controls.Add(imgPlatform);
            var args = new GridViewRowEventArgs(row);

            // Act
            privateObject.Invoke("gvPlatformStats_RowDataBound", new object[] { null, args });

            // Assert
            imgPlatform.ImageUrl.ShouldBe(imageUrl);
        }

        [Test]
        public void GvBrowserStats_RowDataBound_Success([Values("chrome", "ie", "firefox", "safari", "opera", "default")]string browser)
        {
            // Arrange
            InitilizeTestObjects();
            var imageUrl = "~/images/img" + (browser == "chrome" ? "Chrome" : browser == "ie" ? "IE" : browser == "firefox" ? "Firefox" : browser == "safari" ? "Safari" : browser == "opera" ? "Opera" : "Other") + ".png";
            var row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            row.Cells.Add(new TableCell());
            row.Cells[0].Controls.Add(new Label { ID = "lblBrowser", Text = browser });
            var imgBrowser = new System.Web.UI.WebControls.Image { ID = "imgBrowser" };
            row.Cells[0].Controls.Add(imgBrowser);
            var args = new GridViewRowEventArgs(row);

            // Act
            privateObject.Invoke("gvBrowserStats_RowDataBound", new object[] { null, args });

            // Assert
            imgBrowser.ImageUrl.ShouldBe(imageUrl);
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