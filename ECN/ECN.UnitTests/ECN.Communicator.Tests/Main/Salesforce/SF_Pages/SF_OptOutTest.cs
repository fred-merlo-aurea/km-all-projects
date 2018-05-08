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
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;


namespace ECN.Communicator.Tests.Main.Salesforce.SF_Pages
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.Salesforce.SF_Pages.SF_OptOut"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_OptOutTest : PageHelper
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
        public void Page_Load_Success()
        {
            // Arrange
            _testPage.Session["LoggedIn"] = true;
            ShimFolder.GetByCustomerIDInt32User = (p1, p2) => new List<Folder> { new Folder { } };
            ShimGroup.GetByCustomerIDInt32UserString = (p1, p2, p3) => new List<Group> { new Group { FolderID = 0 } };

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
        public void btnSyncOptOut_Click_Contacts_CannotCreate_Error()
        {
            // Arrange 
            InitilizeSyncFakes();
            var ddlECNGroups = _testObject.GetFieldOrProperty("ddlECNGroups") as DropDownList;
            ddlECNGroups.Items.Add("0");
            ddlECNGroups.Items.Add("1");
            ddlECNGroups.SelectedValue = "1";
            ShimSF_Contact.GetListStringString = (p1, p2) => new List<SF_Contact> {
                new SF_Contact { Id = "1", Email = "email" }, new SF_Contact { Id = "1", Email = "email" } };
            ShimSF_Job.CreateStringStringSF_UtilitiesSFObject = (p1, p2, p3) => string.Empty;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncOptOut_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMessage") as Message).MessageText.ShouldContain(
                "Unable to create new job, please ensure that the Salesforce Bulk API is enabled for your organization");
        }

        [Test]
        public void btnSyncOptOut_Click_Contacts_CannotProcess_Error()
        {
            // Arrange 
            InitilizeSyncFakes();
            var ddlECNGroups = _testObject.GetFieldOrProperty("ddlECNGroups") as DropDownList;
            ddlECNGroups.Items.Add("0");
            ddlECNGroups.Items.Add("1");
            ddlECNGroups.SelectedValue = "1";
            ShimSF_Contact.GetListStringString = (p1, p2) => new List<SF_Contact> {
                new SF_Contact { Id = "1", Email = "email" }, new SF_Contact { Id = "1", Email = "email" } };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncOptOut_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMessage") as Message).MessageText.ShouldContain(
                "Unable to process job, please ensure that the Salesforce Bulk API is enabled for your organization");
        }

        [Test]
        public void btnSyncOptOut_Click_Leads_CannotCreate_Error()
        {
            // Arrange 
            InitilizeSyncFakes();
            var ddlECNGroups = _testObject.GetFieldOrProperty("ddlECNGroups") as DropDownList;
            ddlECNGroups.Items.Add("0");
            ddlECNGroups.Items.Add("1");
            ddlECNGroups.SelectedValue = "1";
            ShimSF_Lead.GetListStringString = (p1, p2) => new List<SF_Lead> {
                new SF_Lead { Id = "1", Email = "email" }, new SF_Lead { Id = "1", Email = "email" } };
            ShimSF_Job.CreateStringStringSF_UtilitiesSFObject = (p1, p2, p3) => string.Empty;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncOptOut_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMessage") as Message).MessageText.ShouldContain(
                "Unable to create new job, please ensure that the Salesforce Bulk API is enabled for your organization");
        }

        [Test]
        public void btnSyncOptOut_Click_Leads_CannotProcess_Error()
        {
            // Arrange 
            InitilizeSyncFakes();
            var ddlECNGroups = _testObject.GetFieldOrProperty("ddlECNGroups") as DropDownList;
            ddlECNGroups.Items.Add("0");
            ddlECNGroups.Items.Add("1");
            ddlECNGroups.SelectedValue = "1";
            ShimSF_Lead.GetListStringString = (p1, p2) => new List<SF_Lead> {
                new SF_Lead { Id = "1", Email = "email" }, new SF_Lead { Id = "1", Email = "email" } };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncOptOut_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMessage") as Message).MessageText.ShouldContain(
                "Unable to create new job, please ensure that the Salesforce Bulk API is enabled for your organization");
        }

        [Test]
        public void btnSyncOptOut_Click_NoGroup_Error()
        {
            // Arrange 
            var ddlECNGroups = _testObject.GetFieldOrProperty("ddlECNGroups") as DropDownList;
            ddlECNGroups.Items.Add("0");
            ddlECNGroups.Items.Add("1");
            ddlECNGroups.SelectedValue = "0";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncOptOut_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMessage") as Message).MessageText.ShouldContain(
                "Please select a group to sync");
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
        [TestCase("all", true, true, true)]
        public void ddlFilter_SelectedIndexChanged_Success(string value, bool chkSF, bool chkECN, bool result)
        {
            // Arrange 
            InitializeGrid();
            var gvOptOut = _testObject.GetField("gvOptOut") as GridView;
            gvOptOut.DataSource = new List<OptOutComp> {
                new OptOutComp { SF_Type = SF_Utilities.SFObject.Lead , SFOptOut = true, ECNOptOut = true, Email = "email" },
                new OptOutComp { SF_Type = SF_Utilities.SFObject.Lead , SFOptOut = true, ECNOptOut = false, Email = "email" },
                new OptOutComp { SF_Type = SF_Utilities.SFObject.Contact , SFOptOut = false, ECNOptOut = true, Email = "email" },
                new OptOutComp { SF_Type = SF_Utilities.SFObject.Contact , SFOptOut = false, ECNOptOut = false, Email = "email" }};
            gvOptOut.DataBind();
            var ddlFilter = _testObject.GetFieldOrProperty("ddlFilter") as DropDownList;
            ddlFilter.Items.Add(value);
            ddlFilter.SelectedValue = value;
            foreach (GridViewRow gvr in gvOptOut.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    (gvr.FindControl("chkSFOptOut") as CheckBox).Checked = chkSF;
                    (gvr.FindControl("chkECNOptOut") as CheckBox).Checked = chkECN;
                }
            }

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ddlFilter_SelectedIndexChanged", new object[] { null, null }));
            gvOptOut.ShouldSatisfyAllConditions(
                () => gvOptOut.ShouldNotBeNull(),
                () => gvOptOut.Visible.ShouldBe(result),
                () => gvOptOut.Rows.Count.ShouldBe(4),
                () => gvOptOut.Rows[0].Visible.ShouldBe(result));
        }

        private void InitializeGrid()
        {
            var gvOptOut = _testObject.GetField("gvOptOut") as GridView;
            gvOptOut.Columns.Add(new TemplateField
            {
                HeaderTemplate = new TestTemplateItem { control = new CheckBox { ID = "chkSFOptOutAll" } },
                ItemTemplate = new TestTemplateItem { control = new CheckBox { ID = "chkSFOptOut" } }
            });
            gvOptOut.Columns.Add(new BoundField { DataField = "SF_Type", SortExpression = "type" });
            gvOptOut.Columns.Add(new BoundField { DataField = "Email", SortExpression = "email" });
            gvOptOut.Columns.Add(new TemplateField
            {
                HeaderTemplate = new TestTemplateItem { control = new CheckBox { ID = "chkECNOptOutAll" } },
                ItemTemplate = new TestTemplateItem { control = new CheckBox { ID = "chkECNOptOut" } }
            });
            gvOptOut.RowDataBound += (s, e) => _testObject.Invoke("gvOptOut_RowDataBound", new object[] { s, e });
            gvOptOut.Sorting += (s, e) => _testObject.Invoke("gvOptOut_Sorting", new object[] { s, e });            
        }

        private void InitilizeSyncFakes()
        {
            _testObject.SetFieldOrProperty("listComp", new List<OptOutComp> { new OptOutComp { SFId = "1" } });
            ShimEmailGroup.GetByGroupIDInt32User = (p1, p2) => new List<EmailGroup> { new EmailGroup { SubscribeTypeCode = "U" } };
            ShimEmail.GetByEmailIDGroupIDInt32Int32User = (p1,p2,p3) => new Email();
            ShimSF_Contact.GetListStringString = (p1, p2) => new List<SF_Contact> { };
            ShimSF_Lead.GetListStringString = (p1, p2) => new List<SF_Lead> { };
            ShimGroup.GetMasterSuppressionGroupInt32User = (p1, p2) => new Group { };
            ShimEmailGroup.GetSubscriberCountInt32Int32User = (p1, p2, p3) => 1;
            var ds = new DataSet();
            ds.Tables.Add(new DataTable());
            ds.Tables.Add(new DataTable { Columns = { "EmailID", "EmailAddress" }, Rows = { { "1", "email" } } });
            ShimEmailGroup.GetBySearchStringPagingInt32Int32Int32Int32String = (p1, p2, p3, p4, p5) => ds;
            ShimSF_Job.CreateStringStringSF_UtilitiesSFObject = (p1, p2, p3) => "1";
            ShimSF_Contact.GetXMLForOptOutJobDictionaryOfStringString = (p) => string.Empty;
            ShimSF_Lead.GetXMLForOptOutJobDictionaryOfStringString = (p) => string.Empty;
            ShimSF_Job.AddBatchStringStringString = (p1, p2, p3) => string.Empty;
            ShimSF_Job.CloseStringString = (p1, p2) => true;
        }

        private void CreateTestObjects()
        {
            _testPage = new SF_OptOut();
            InitializeAllControls(_testPage);
            _testObject = new PrivateObject(_testPage);
            _testObject.SetFieldOrProperty("dtECN", new DataTable { Columns = { "EmailID", "EmailAddress" }, Rows = { { "1", "email" } } });
            _testObject.SetFieldOrProperty("listSFLead", new List<SF_Lead> { new SF_Lead { Email = "email"} });
            _testObject.SetFieldOrProperty("listSF", new List<SF_Contact> { new SF_Contact { Email = "email" } });
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
