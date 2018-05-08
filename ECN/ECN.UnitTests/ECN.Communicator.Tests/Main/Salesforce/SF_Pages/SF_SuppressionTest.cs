using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using ecn.communicator.main.Salesforce.Controls;
using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.Entity.Fakes;
using ecn.communicator.main.Salesforce.SF_Pages;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Sorting = System.Web.UI.WebControls.SortDirection;

namespace ECN.Communicator.Tests.Main.Salesforce.SF_Pages
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.Salesforce.SF_Pages.SF_Suppression1"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_SuppressionTest : PageHelper
    {
        private PrivateObject _testObject;
        private Page _testPage;
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            CreateMasterPage();
            CreateTestObjects();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void SortExp_GetAndSet_Success()
        {
            // Arrange
            const string propertyName = "sortExp";
            const string valueToSet = "valueToSet";
            
            // Act
            var defaultValue =_testObject.GetFieldOrProperty(propertyName);
            _testObject.SetFieldOrProperty(propertyName, valueToSet);
            var result = _testObject.GetFieldOrProperty(propertyName);

            // Assert
            defaultValue.ShouldBeNull();
            result.ShouldBe(valueToSet);
        }

        [Test]
        public void SortDir_GetAndSet_Success()
        {
            // Arrange
            const string propertyName = "sortDir";
            var valueToSet = Sorting.Descending;

            // Act
            var defaultValue = _testObject.GetFieldOrProperty(propertyName);
            _testObject.SetFieldOrProperty(propertyName, valueToSet);
            var result = _testObject.GetFieldOrProperty(propertyName);

            // Assert
            defaultValue.ShouldBe(Sorting.Ascending);
            result.ShouldBe(valueToSet);
        }

        [Test]
        public void Page_Load_Success()
        {
            // Arrange
            _testPage.Session["LoggedIn"] = true;
            ShimCampaign.GetByCustomerIDInt32UserBoolean = (p1,p2,p3) => new List<Campaign> { new Campaign { } };
            KMPlatform.BusinessLogic.Fakes.ShimUser.GetByCustomerIDInt32 = (p) => new List<User> { new User { } };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null }));
        }

        [Test]
        public void Page_Load_WithoutLogin_Success()
        {
            // Arrange
            _testPage.Session["LoggedIn"] = false;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMessage") as Message).MessageText.ShouldContain("You must first log into Salesforce to use this page");
        }

        [Test]
        public void btnSyncSuppression_Click_NoJob_Error()
        {
            // Arrange 
            InitilizeSyncFakes();

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncSuppression_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMessage") as Message).MessageText.ShouldContain(
                "Unable to create job, please ensure Salesforce Bulk API is enabled for your organization");
        }

        [Test]
        public void btnSyncSuppression_Click_NoData_Error()
        {
            // Arrange 
            InitilizeSyncFakes();

            ShimSF_Contact.GetListForMSStringString = (p1, p2) => new List<SF_Contact> { new SF_Contact { } };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncSuppression_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMessage") as Message).MessageText.ShouldContain(
                "No Contacts to Master Suppress");
        }

        [Test]
        [TestCase("both", true, true, true)]
        [TestCase("both", true, false, false)]
        [TestCase("sf", true, false, true)]
        [TestCase("sf", true, true, false)]
        [TestCase("ecn", false, true, true)]
        [TestCase("ecn", true, true, false)]
        [TestCase("none", false, false, true)]
        [TestCase("none", true, true, false)]
        [TestCase("0", false, false, true)]
        [TestCase("0", true, true, false)]
        public void ddlFilter_SelectedIndexChanged_Success(string value, bool chkSF, bool chkECN, bool result)
        {
            // Arrange 
            InitializeGrid();
            var ddlFilter = _testObject.GetFieldOrProperty("ddlFilter") as DropDownList;
            ddlFilter.Items.Add("0");
            ddlFilter.Items.Add(value);
            ddlFilter.SelectedValue = value;
            _testObject.SetFieldOrProperty("listSupp", (value != "0" || result) ? 
                new List<SuppList> { new SuppList { SFSupp = chkSF, ECNSupp = chkECN} } : new List<SuppList> { } );
            var btnSyncSuppression = _testObject.GetFieldOrProperty("btnSyncSuppression") as Button;
            var btnSyncSelected = _testObject.GetFieldOrProperty("btnSyncSelected") as Button;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ddlFilter_SelectedIndexChanged", new object[] { null, null }));
            btnSyncSuppression.ShouldSatisfyAllConditions(
                () => btnSyncSuppression.ShouldNotBeNull(),
                () => btnSyncSuppression.Enabled.ShouldBe(result));
            btnSyncSelected.ShouldSatisfyAllConditions(
                () => btnSyncSelected.ShouldNotBeNull(),
                () => btnSyncSelected.Enabled.ShouldBe(result));
        }        

        private void InitializeGrid()
        {
            var gvSuppression = _testObject.GetField("gvSuppression") as GridView;
            gvSuppression.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new CheckBox { ID = "chkSFSupp" } }
            });
            gvSuppression.Columns.Add(new BoundField { DataField = "Email", SortExpression = "Email" });
            gvSuppression.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new CheckBox { ID = "chkECNSupp" } }
            });
            gvSuppression.RowDataBound += (s, e) => _testObject.Invoke("gvSuppression_RowDataBound", new object[] { s, e });
            gvSuppression.Sorting += (s, e) => _testObject.Invoke("gvSuppression_Sorting", new object[] { s, e });
        }

        private void InitilizeSyncFakes()
        {
            ShimSF_Contact.GetListForMSStringString = (p1, p2) => new List<SF_Contact> { new SF_Contact { Email = "email" } };
            ShimGroup.GetMasterSuppressionGroupInt32User = (p1, p2) => new Group { };
            ShimEmailGroup.GetSubscriberCountInt32Int32User = (p1, p2, p3) => 1;
            var ds = new DataSet();
            ds.Tables.Add(new DataTable());
            ds.Tables.Add(new DataTable { Columns = { "EmailID", "EmailAddress" }, Rows = { { "1", "email" } } });
            ShimEmailGroup.GetBySearchStringPagingInt32Int32Int32Int32String = (p1, p2, p3, p4, p5) => ds;
            ShimSF_Job.CreateStringStringSF_UtilitiesSFObject = (p1, p2, p3) => string.Empty;
        }

        private void CreateTestObjects()
        {
            _testPage = new SF_Suppression1();
            InitializeAllControls(_testPage);
            _testObject = new PrivateObject(_testPage);
            var kmSearch = _testObject.GetField("kmSearch");
            InitializeAllControls(kmSearch);
            _testObject.SetFieldOrProperty("ECN_SuppList", new List<Email> { });
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
