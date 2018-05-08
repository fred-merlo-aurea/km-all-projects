using System;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.lists;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.DomainTracker.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.DomainTracker;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    /// UT for <see cref="DomainTrackerEdit"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DomainTrackerEditTest : BaseListsTest<DomainTrackerEdit>
    {
        [Test]
        public void Page_Load_Success()
        {
            // Arrange
            InitilizeTestObjects();
            KM.Platform.Fakes.ShimUser.IsSystemAdministratorUser = (p) => false;
            ShimClientGroup.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var txtDomainName = privateObject.GetFieldOrProperty("txtDomainName") as TextBox;
            var lblDomainName = privateObject.GetFieldOrProperty("lblDomainName") as Label;
            ShimDomainTracker.GetByDomainTrackerIDInt32User = (p1, p2) => new DomainTracker { Domain = "TestDomain" };
            ShimDomainTrackerFields.GetByDomainTrackerID_DTInt32User = (p1, p2) => new DataTable { };
            QueryString.Add("domainTrackerID", "1");

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBe("domainTrackerList.aspx"),
                () => phError.Visible.ShouldBeFalse(),
                () => txtDomainName.Visible.ShouldBeFalse(),
                () => lblDomainName.Visible.ShouldBeTrue(),
                () => lblDomainName.Text.ShouldBe("TestDomain"));
        }

        [Test]
        public void Page_Load_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            KM.Platform.Fakes.ShimUser.IsSystemAdministratorUser = (p) => true;
            ShimClientGroup.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => false;

            // Act, Assert
            var result = Should.Throw<Exception>(() => privateObject.Invoke("Page_Load", new object[] { null, null }));
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.InnerException.ShouldBeOfType(typeof(SecurityException)));
        }

        [Test]
        public void BtnSave_Click_Save_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var trackerSaved = false;
            var trackerFieldsSaved = false;
            ShimDomainTracker.SaveDomainTrackerUser = (p1, p2) => { trackerSaved = true; return 0; };
            ShimDomainTrackerFields.SaveDomainTrackerFieldsUser = (p1, p2) => { trackerFieldsSaved = true; return 0; };
            privateObject.SetFieldOrProperty("DomainTrackerFields_DT", 
                new DataTable
                {
                    Columns = { "IsDeleted", "DomainTrackerFieldsID", "Source", "SourceID", "FieldName" },
                    Rows = { { false, "1-2", "1", "1", "1" }}
                });

            // Act
            privateObject.Invoke("btnSave_Click", new object[] { null, null });

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBe("domainTrackerList.aspx"),
                () => trackerSaved.ShouldBeTrue(),
                () => trackerFieldsSaved.ShouldBeTrue());
        }

        [Test]
        public void BtnSave_Click_Delete_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var trackerFieldsDeleted = false;
            ShimDomainTrackerFields.DeleteInt32Int32User = (p1, p2, p3) => trackerFieldsDeleted = true;
            privateObject.SetFieldOrProperty("DomainTrackerFields_DT",
                new DataTable
                {
                    Columns = { "IsDeleted", "DomainTrackerFieldsID", "Source", "SourceID", "FieldName" },
                    Rows = { { true, "1", "1", "1", "1" } }
                });
            QueryString.Add("domainTrackerID", "1");

            // Act
            privateObject.Invoke("btnSave_Click", new object[] { null, null });

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBe("domainTrackerList.aspx"),
                () => trackerFieldsDeleted.ShouldBeTrue());
        }

        [Test]
        public void BtnSave_Click_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            var txtDomainName = privateObject.GetFieldOrProperty("txtDomainName") as TextBox;
            txtDomainName.Text = "http://km.com";
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("btnSave_Click", new object[] { null, null });

            // Assert
            lblErrorMessage.Text.ShouldBe("<br/>Blast: Please check the domain name entered. Do NOT include the protocol (http:// or https://) in the domain name.");
        }

        [Test]
        public void BtnAddDomainTrackerFields_Click()
        {
            // Arrange
            InitilizeTestObjects();
            var drpSource = privateObject.GetFieldOrProperty("drpSource") as DropDownList;
            drpSource.Items.Add("-Select-");
            drpSource.Items.Add("1");
            drpSource.SelectedValue = "1";
            privateObject.SetFieldOrProperty("DomainTrackerFields_DT", null);

            // Act
            privateObject.Invoke("btnAddDomainTrackerFields_Click", new object[] { null, null });

            // Assert
            var domainTrackerFields_DT = privateObject.GetFieldOrProperty("DomainTrackerFields_DT") as DataTable;
            testObject.ShouldSatisfyAllConditions(
                () => drpSource.SelectedValue.ShouldBe("-Select-"),
                () => domainTrackerFields_DT.ShouldNotBeNull(),
                () => domainTrackerFields_DT.Rows.Count.ShouldBe(1),
                () => domainTrackerFields_DT.Rows[0]["Source"].ShouldBe("1"));
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
