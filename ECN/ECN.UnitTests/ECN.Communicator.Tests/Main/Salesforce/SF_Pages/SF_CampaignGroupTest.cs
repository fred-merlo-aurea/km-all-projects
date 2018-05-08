using System;
using System.Collections;
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
using ecn.communicator.main.Salesforce.SF_Pages.Fakes;
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
    ///     Unit tests for <see cref="ecn.communicator.main.Salesforce.SF_Pages.SF_CampaignGroup"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_CampaignGroupTest : SalesForcePageTestBase
    {
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
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("You must first log into Salesforce to use this page");
        }

        [Test]
        public void btnSyncGroup_Click_New_Success()
        {
            // Arrange 
            InitilizeSyncFakes();
            var rblECNGroup = _testObject.GetFieldOrProperty("rblECNGroup") as RadioButtonList;
            rblECNGroup.SelectedValue = "new";
            (_testObject.GetFieldOrProperty("txtNewGroup") as TextBox).Text = "1";


            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncGroup_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("ResultsGrid") as DataGrid).Items.Count.ShouldBe(7);
        }

        [Test]
        public void btnSyncGroup_Click_New_ContactDb_Error()
        {
            // Arrange 
            InitilizeSyncFakes();
            var rblECNGroup = _testObject.GetFieldOrProperty("rblECNGroup") as RadioButtonList;
            rblECNGroup.SelectedValue = "new";
            (_testObject.GetFieldOrProperty("txtNewGroup") as TextBox).Text = "1";
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields {}};
            ShimSF_Lead.GetCampaignMembersStringString = (p1, p2) => new List<SF_Lead> { };
            ShimSF_CampaignGroup.AllInstances.UpdateToDBInt32StringString = (p1, p2, p3, p4) => throw new Exception();

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncGroup_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Import Unsuccessful");
        }

        [Test]
        public void btnSyncGroup_Click_New_LeadDb_Error()
        {
            // Arrange 
            InitilizeSyncFakes();
            var rblECNGroup = _testObject.GetFieldOrProperty("rblECNGroup") as RadioButtonList;
            rblECNGroup.SelectedValue = "new";
            (_testObject.GetFieldOrProperty("txtNewGroup") as TextBox).Text = "1";
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            ShimSF_Contact.GetCampaignMembersStringString = (p1, p2) => new List<SF_Contact> { };
            ShimSF_CampaignGroup.AllInstances.UpdateToDBInt32StringString = (p1, p2, p3, p4) => throw new Exception();

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncGroup_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Import Unsuccessful");
        }

        [Test]
        public void btnSyncGroup_Click_New_CreateNewGroupError()
        {
            // Arrange 
            var rblECNGroup = _testObject.GetFieldOrProperty("rblECNGroup") as RadioButtonList;
            rblECNGroup.SelectedValue = "new";
            (_testObject.GetFieldOrProperty("txtNewGroup") as TextBox).Text = "1";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncGroup_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Error creating new group");
        }

        [Test]
        public void btnSyncGroup_Click_New_NoGroup()
        {
            // Arrange 
            var rblECNGroup = _testObject.GetFieldOrProperty("rblECNGroup") as RadioButtonList;
            rblECNGroup.SelectedValue = "new";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncGroup_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Please enter a name for the new group");
        }

        [Test]
        public void btnSyncGroup_Click_Existing_Success()
        {
            // Arrange 
            InitilizeSyncFakes();
            var rblECNGroup = _testObject.GetFieldOrProperty("rblECNGroup") as RadioButtonList;
            rblECNGroup.SelectedValue = "existing";
            (_testObject.GetFieldOrProperty("txtNewGroup") as TextBox).Text = "1";


            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncGroup_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("ResultsGrid") as DataGrid).Items.Count.ShouldBe(7);
        }

        [Test]
        public void btnSyncGroup_Click_Existing_ContactDb_Error()
        {
            // Arrange 
            InitilizeSyncFakes();
            var rblECNGroup = _testObject.GetFieldOrProperty("rblECNGroup") as RadioButtonList;
            rblECNGroup.SelectedValue = "existing";
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            ShimSF_Lead.GetCampaignMembersStringString = (p1, p2) => new List<SF_Lead> { };
            ShimSF_CampaignGroup.AllInstances.UpdateToDBInt32StringString = (p1, p2, p3, p4) => throw new Exception();

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncGroup_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Import Unsuccessful");
        }

        [Test]
        public void btnSyncGroup_Click_Existing_LeadDb_Error()
        {
            // Arrange 
            InitilizeSyncFakes();
            var rblECNGroup = _testObject.GetFieldOrProperty("rblECNGroup") as RadioButtonList;
            rblECNGroup.SelectedValue = "existing";
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            ShimSF_Contact.GetCampaignMembersStringString = (p1, p2) => new List<SF_Contact> { };
            ShimSF_CampaignGroup.AllInstances.UpdateToDBInt32StringString = (p1, p2, p3, p4) => throw new Exception();

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncGroup_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Import Unsuccessful");
        }

        [Test]
        public void btnSyncGroup_Click_Existing_NoGroup()
        {
            // Arrange 
            var rblECNGroup = _testObject.GetFieldOrProperty("rblECNGroup") as RadioButtonList;
            rblECNGroup.SelectedValue = "existing";
            (_testObject.GetFieldOrProperty("ddlExistingGroup") as DropDownList).SelectedValue = "0";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncGroup_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Please select an existing group to insert into");
        }

        [Test]
        public void DisplayResults_PassEmptyTable_DoNotBindAnything()
        {
            // Arrange
            var data = new DataTable();

            // Act
            _testObject.Invoke("DisplayResults");

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => GetProperty<Label>("MessageLabel").Text.ShouldBeEmpty(),
                () => GetProperty<DataGrid>("ResultsGrid").DataSource.ShouldBeNull());
        }

        [TestCase("T", "Total Records in the File", 1)]
        [TestCase("I", "New", 2)]
        [TestCase("U", "Changed", 3)]
        [TestCase("D", "Duplicate(s)", 4)]
        [TestCase("S", "Skipped", 5)]
        [TestCase("M", "Skipped (Emails in Master Suppression)", 6)]
        public void DisplayResults_PassTableWithActions_BindToResultsGrid(string action, string expectedActionName, int sortOrder)
        {
            // Arrange
            const string ActionsColumn = "Action";
            const string TotalsColumn = "Totals";
            const string SortColumn = "sortOrder";
            const int FirstRow = 0;
            const int LastRow = 1;
            const int Counts = 1;

            var hashTable = new Hashtable();
            hashTable.Add(action, Counts);
            _testObject.SetFieldOrProperty("hUpdatedRecords", hashTable);
            // Act
            _testObject.Invoke("DisplayResults");

            // Assert
            GetProperty<Label>("MessageLabel").Text.ShouldBe("Import Results");
            var dataSource = GetProperty<DataGrid>("ResultsGrid").DataSource as DataTable;
            dataSource.ShouldNotBeNull();
            dataSource.ShouldSatisfyAllConditions(
                () => dataSource.Rows[FirstRow][ActionsColumn].ShouldBe(expectedActionName),
                () => dataSource.Rows[FirstRow][TotalsColumn].ShouldBe(Counts.ToString()),
                () => dataSource.Rows[FirstRow][SortColumn].ShouldBe(sortOrder.ToString()),
                () => dataSource.Rows[LastRow][ActionsColumn].ShouldBe("&nbsp;"),
                () => dataSource.Rows[LastRow][TotalsColumn].ShouldBe(" "),
                () => dataSource.Rows[LastRow][SortColumn].ShouldBe("8"));
        }

        private void InitilizeSyncFakes()
        {
            ShimGroup.SaveGroupUser = (p1, p2) => 1;
            ShimGroup.GetByGroupIDInt32User = (p1, p2) => new Group { GroupID = 1 };
            ShimGroup.ExistsInt32StringInt32Int32 = (p1, p2, p3, p4) => false;
            ShimGroupDataFields.SaveGroupDataFieldsUser = (p1, p2) => 0;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> {
                new GroupDataFields { ShortName = "sfid", GroupDataFieldsID = 1 },
                new GroupDataFields { ShortName = "sftype", GroupDataFieldsID = 1 } };
            ShimSF_Contact.GetCampaignMembersStringString = (p1, p2) => new List<SF_Contact> { new SF_Contact { } };
            ShimSF_Lead.GetCampaignMembersStringString = (p1, p2) => new List<SF_Lead> { new SF_Lead { } };
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) =>
                new DataTable { Columns = { "Action", "Counts" }, Rows = { { "T", "1" }, { "T", "1" }, { "I", "1" }, { "U", "1" }, { "D", "1" }, { "S", "1" }, { "M", "1" } } };
        }

        private void CreateTestObjects()
        {
            _testPage = new SF_CampaignGroup();
            InitializeAllControls(_testPage);
            _testObject = new PrivateObject(_testPage); 
             var ddlSFCampaign = _testObject.GetFieldOrProperty("ddlSFCampaign") as DropDownList;
            ddlSFCampaign.Items.Add("0");
            ddlSFCampaign.Items.Add("1");
            ddlSFCampaign.SelectedValue = "1";
            var ddlExistingGroup = _testObject.GetFieldOrProperty("ddlExistingGroup") as DropDownList;
            ddlExistingGroup.Items.Add("0");
            ddlExistingGroup.Items.Add("1");
            ddlExistingGroup.SelectedValue = "1";
            var rblECNGroup = _testObject.GetFieldOrProperty("rblECNGroup") as RadioButtonList;
            rblECNGroup.Items.Add("new");
            rblECNGroup.Items.Add("existing");
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
