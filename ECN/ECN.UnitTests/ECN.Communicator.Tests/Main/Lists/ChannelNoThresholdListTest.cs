using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ActiveUp.WebControls;
using ecn.communicator.main.lists;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    /// UT for <see cref="channelNoThresholdList"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ChannelNoThresholdListTest : BaseListsTest<channelNoThresholdList>
    {
        [Test]
        public void Page_Load_Admin_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimUser.IsSystemAdministratorUser = (p) => true;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBeEmpty();
        }

        [Test]
        public void Page_Load_Default_Success()
        {
            // Arrange
            InitilizeTestObjects();

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("../default.aspx");
        }

        [Test]
        public void AddEmailBTN_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var emailAddresses = privateObject.GetFieldOrProperty("emailAddresses") as TextBox;
            emailAddresses.Text = "test1@km.com,test2@km.com";
            ShimEmailGroup.ImportEmailsToNoThresholdUserInt32String = (p1, p2, p3) => new DataTable
            {
                Columns = { "Action", "Counts" },
                Rows = { { "T", "1" }, { "T", "1" }, { "I", "1" }, { "U", "1" }, { "D", "1" }, { "S", "1" } }
            };
            var resultsGrid = privateObject.GetFieldOrProperty("ResultsGrid") as DataGrid;

            // Act
            privateObject.Invoke("addEmailBTN_Click", new object[] { null, null });

            // Assert
            resultsGrid.ShouldSatisfyAllConditions(
                () => resultsGrid.Visible.ShouldBeTrue(),
                () => resultsGrid.Items.Count.ShouldBe(7));
        }

        [Test]
        public void AddEmailBTN_Click_NoEmail_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var emailAddresses = privateObject.GetFieldOrProperty("emailAddresses") as TextBox;
            emailAddresses.Text = string.Empty;
            var resultsGrid = privateObject.GetFieldOrProperty("ResultsGrid") as DataGrid;

            // Act
            privateObject.Invoke("addEmailBTN_Click", new object[] { null, null });

            // Assert
            resultsGrid.Visible.ShouldBeFalse();
        }

        [Test]
        public void ExportEmailsBTN_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimHttpServerUtility.AllInstances.MapPathString = (p1, p2) => string.Empty;
            ShimFile.AppendTextString = (p) => new ShimStreamWriter();
            var fileText = string.Empty;
            ShimTextWriter.AllInstances.WriteLineString = (p1, p2) => fileText = p2;
            var fileName = string.Empty;
            ShimHttpResponse.AllInstances.WriteFileString = (p1, p2) => fileName = p2;
            ShimHttpResponse.AllInstances.End = (p) => { };
            ShimChannelNoThresholdList.GetByBaseChannelIDInt32User = (p1, p2) =>
                new List<ChannelNoThresholdList> { new ChannelNoThresholdList { EmailAddress = "test@km.com" } };

            // Act
            privateObject.Invoke("exportEmailsBTN_Click", new object[] { null, null });

            // Assert
            fileText.ShouldBe("test@km.com, ");
            fileName.ShouldBe("-1_NoThreshold_Emails.CSV");
        }

        [Test]
        public void LoadEmailsGrid_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimChannelNoThresholdList.GetByBaseChannelIDInt32User = (p1, p2) => new List<ChannelNoThresholdList> { new ChannelNoThresholdList { } };
            var exportEmailsBTN = privateObject.GetFieldOrProperty("exportEmailsBTN") as Button;
            var emailsPager = privateObject.GetFieldOrProperty("emailsPager") as PagerBuilder;

            // Act
            privateObject.Invoke("loadEmailsGrid", new object[] { });

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => exportEmailsBTN.Visible.ShouldBeTrue(),
                () => emailsPager.PageCount.ShouldBe(1));
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