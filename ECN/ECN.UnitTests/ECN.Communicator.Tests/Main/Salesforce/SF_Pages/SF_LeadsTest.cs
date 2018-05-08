using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using ecn.communicator.main.Salesforce.Controls;
using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.Entity.Fakes;
using ecn.communicator.main.Salesforce.Extensions;
using ecn.communicator.main.Salesforce.SF_Pages;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN.Communicator.Tests.Main.Salesforce.SF_Pages
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.Salesforce.SF_Pages.SF_Leads"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_LeadsTest : SalesForcePageTestBase
    {
        private const string CommandArgument = "1";
        private const string SfCommandName = "SF";
        private const string EcnCommandName = "ECN";
        private const string ImgbtnCompareProperty = "imgbtnCompare";
        private const string ImgbtnEcntoSfProperty = "imgbtnECNToSF";
        private const string ImgbtnSftoEcnProperty = "imgbtnSFToECN";
        private const string EcnCountPropertyName = "ECN_SelectedCount";
        private const string SfCountPropertyName = "SF_SelectedCount";
        private const string SfCheckboxId = "chkSFSelect";
        private const string EcnCheckboxId = "chkECNSelect";
        private const string EcnHeaderCheckboxId = "chkECNSelectAll";
        private const string SfHeaderCheckboxId = "chkSFSelectAll";
        private const string SfGridId = "dgSFLeads";
        private const string EcnGridId = "dgECNLeads";
        private const string GreyDarkColor = "GreyDark";
        private const string GreyLightColor = "GreyLight";
        private const int Zero = 0;
        private const string SfCheckedMethodName = "chkSFSelectRow_CheckedChanged";
        private const string EcnCheckedMethodName = "chkECNSelectRow_CheckedChanged";
        private const string ECN_LeadsPropertyName = "ECN_Leads";

        private Page _testPage;
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            InitializeFakes();
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
        }

        [Test]
        public void imgbtnSFToECN_Click_Success([Values("new", "existing")]string type)
        {
            // Arrange
            (_testObject.GetField("rblECNGroup") as RadioButtonList).SelectedValue = type;
            (_testObject.GetField("txtNewGroup") as TextBox).Text = "TestGroup";
            var dgSFLeads = _testObject.GetField("dgSFLeads") as GridView;
            dgSFLeads.DataSource = new List<SF_Lead> { new SF_Lead { OwnerId = "1", Email = "dummy" } };
            dgSFLeads.DataBind();
            (dgSFLeads.Rows[0].FindControl("chkSFSelect") as CheckBox).Checked = true;
            (_testObject.GetField("ddlECNGroup") as DropDownList).SelectedValue = "1";
            _testObject.SetFieldOrProperty("SalesForce_Leads", new List<SF_Lead> { new SF_Lead { Email = "dummy", OwnerId = "1" } });

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("imgbtnSFToECN_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldBeNull();
        }

        [Test]
        public void imgbtnSFToECN_Click_NoGroup_Exception()
        {
            // Arrange
            (_testObject.GetField("rblECNGroup") as RadioButtonList).SelectedValue = "new";
            (_testObject.GetField("txtNewGroup") as TextBox).Text = "TestGroup";
            ShimGroup.ExistsInt32StringInt32Int32 = (p1, p2, p3, p4) => true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("imgbtnSFToECN_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Group name entered already exists.  Please enter a new group name.");
        }

        [Test]
        public void imgbtnSFToECN_Click_NoGroupId_Exception()
        {
            // Arrange
            (_testObject.GetField("rblECNGroup") as RadioButtonList).SelectedValue = "new";
            (_testObject.GetField("txtNewGroup") as TextBox).Text = "TestGroup";
            ShimGroup.SaveGroupUser = (p1, p2) => throw new ECNException(new List<ECNError> { });

            // Act
            _testObject.Invoke("imgbtnSFToECN_Click", new object[] { null, null });

            // Assert
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Unable to create new group.");
        }

        [Test]
        public void imgbtnSFToECN_Click_NoGroupName_Exception()
        {
            // Arrange
            (_testObject.GetField("rblECNGroup") as RadioButtonList).SelectedValue = "new";
            ShimGroup.SaveGroupUser = (p1, p2) => throw new Exception("Test Exception");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("imgbtnSFToECN_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Please enter a name for the new group.");
        }

        [Test]
        public void imgbtnSFToECN_Click_NoEcnGroup_Exception()
        {
            // Arrange
            (_testObject.GetField("rblECNGroup") as RadioButtonList).SelectedValue = "existing";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("imgbtnSFToECN_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Please select an ECN group.");
        }

        [Test]
        public void imgbtnSFToECN_Click_Exception()
        {
            // Arrange
            (_testObject.GetField("rblECNGroup") as RadioButtonList).SelectedValue = "new";
            (_testObject.GetField("txtNewGroup") as TextBox).Text = "TestGroup";
            var dgSFLeads = _testObject.GetField("dgSFLeads") as GridView;
            dgSFLeads.DataSource = new List<SF_Lead> { new SF_Lead { OwnerId = "1", Email = "dummy" } };
            dgSFLeads.DataBind();
            (dgSFLeads.Rows[0].FindControl("chkSFSelect") as CheckBox).Checked = true;
            (_testObject.GetField("ddlECNGroup") as DropDownList).SelectedValue = "1";
            _testObject.SetFieldOrProperty("SalesForce_Leads", new List<SF_Lead> { new SF_Lead { Email = "dummy", OwnerId = "1", State = "alabama" } });
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) =>
                throw new Exception("Test Exception");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("imgbtnSFToECN_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("Unable to sync list.");
        }

        [Test]
        [TestCase("GreyLight", "dummy", "0", true)]
        [TestCase("GreyLight", "dummy", "1", true)]
        [TestCase("GreyLight", "", "0", true)]
        [TestCase("GreyLight", "", "1", true)]
        [TestCase("GreyLight", "", "1", false)]
        [TestCase("GreyDark", "dummy", "1", true)]
        [TestCase("GreyDark", "dummy", "1", false)]
        public void imgbtnECNToSF_Click_Success(string color, string email, string ddlValue, bool success)
        {
            // Arrange
            var dgECNLeads = _testObject.GetField("dgECNLeads") as GridView;
            dgECNLeads.DataSource = new List<Email> { new Email { EmailAddress = "dummy" } };
            dgECNLeads.DataBind();
            var chkECNSelect = (dgECNLeads.Rows[0].FindControl("chkECNSelect") as CheckBox);
            chkECNSelect.Checked = true;
            chkECNSelect.Attributes["color"] = color;
            ShimSF_Lead.GetEmailStringString = (p1, p2) => new SF_Lead { Email = email, OwnerId = "1" };
            ShimSF_Lead.InsertStringSF_Lead = (p1, p2) => success;
            ShimSF_Lead.UpdateStringSF_Lead = (p1, p2) => success;
            (_testObject.GetField("ddlSFTags") as DropDownList).SelectedValue = ddlValue;
            ShimSF_TagDefinition.GetAllString = (token) => new List<SF_TagDefinition> { new SF_TagDefinition { Name = "1" } };
            ShimSF_LeadTag.GetByTagNameStringString = (p1, p2) => new List<SF_LeadTag> { new SF_LeadTag { } };
            ShimSF_LeadTag.InsertStringSF_LeadTag = (p1, p2) => true;
            ShimSF_Lead.GetSingleStringString = (p1, p2) => new SF_Lead { OwnerId = "1" };
            _testObject.SetFieldOrProperty("ECN_Leads", new List<Email> { new Email { EmailAddress = "dummy" } });

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("imgbtnECNToSF_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldBeNull();
        }

        [TestCase("ECN")]
        [TestCase("ecn")]
        public void imgbtnCompare_Click_EcnLabelsAreEqualsEmailWithIdFromArgument(string command)
        {
            // Arrange
            const string entityId = "entityId";
            const string email = "email";
            const int id = 1;
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty); ;
            imgbtnCompare.CommandName = command;
            imgbtnCompare.CommandArgument = id.ToString();
            var expectedEmail = CreateEmail(id, email);
            _testObject.SetFieldOrProperty(ECN_LeadsPropertyName, new List<Email> { expectedEmail });
            ShimSF_Lead.GetSingleStringString = (p1, p2) => new SF_Lead { Id = entityId };

            // Act
            _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null });

            // Assert
            _testObject.ShouldSatisfyAllConditions(VerifyEcnLabelsAndHiddenFields(expectedEmail, entityId).ToArray());
        }

        [TestCase("SF")]
        [TestCase("sf")]
        public void imgbtnCompare_Click_EcnLabelsAreEqualsEmailWithEmailFromContact(string command)
        {
            // Arrange
            const string entityId = "entityId";
            const string email = "email";
            const int id = 0;
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            imgbtnCompare.CommandName = command;
            var expectedEmail = CreateEmail(id, email);
            _testObject.SetFieldOrProperty(ECN_LeadsPropertyName, new List<Email> { expectedEmail });
            ShimSF_Lead.GetSingleStringString = (p1, p2) => new SF_Lead { Id = entityId, Email = email };

            // Act
            _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null });

            // Assert
            _testObject.ShouldSatisfyAllConditions(VerifyEcnLabelsAndHiddenFields(expectedEmail, entityId).ToArray());
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void imgbtnCompare_Click_AddreesValid_ControlsHighligthed(bool isEcnValid, bool isSfValid)
        {
            // Arrange
            const int id = 1;
            const string emailAddress = "testEmail";
            var email = new Email { EmailAddress = emailAddress, EmailID = id };
            var lead = new SF_Lead { Email = emailAddress };
            SetupContacts(email, lead);
            SetupAddresses(isEcnValid, isSfValid);
            var ecnExpectedColor = isEcnValid ? KM_Colors.BlueLight : KM_Colors.GreyDark;
            var sfExpectedColor = isSfValid ? KM_Colors.BlueLight : KM_Colors.GreyDark;
            // Act
            _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null });

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => GetLabelColor("lblECNAddress").ShouldBe(ecnExpectedColor),
                () => GetLabelColor("lblSFAddress").ShouldBe(sfExpectedColor),
                () => RadioButtonVisible("rblAddress").ShouldBeTrue(),
                () => RadioButtonVisible("rblCity").ShouldBeTrue(),
                () => RadioButtonVisible("rblState").ShouldBeTrue(),
                () => RadioButtonVisible("rblZip").ShouldBeTrue());
        }

        [Test]
        public void imgbtnCompare_Click_AddressesAreValid_ControlsHided()
        {
            // Arrange
            const int id = 1;
            const string emailAddress = "testEmail";
            var email = new Email { EmailAddress = emailAddress, EmailID = id };
            var lead = new SF_Lead { Email = emailAddress };
            SetupContacts(email, lead);
            SetupAddresses(true, true);

            // Act
            _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null });

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => GetLabelColor("lblECNAddress").ShouldBe(KM_Colors.BlueLight),
                () => GetLabelColor("lblSFAddress").ShouldBe(KM_Colors.BlueLight),
                () => GetLabelColor("lblECNCity").ShouldBe(KM_Colors.BlueLight),
                () => GetLabelColor("lblSFCity").ShouldBe(KM_Colors.BlueLight),
                () => GetLabelColor("lblECNState").ShouldBe(KM_Colors.BlueLight),
                () => GetLabelColor("lblSFState").ShouldBe(KM_Colors.BlueLight),
                () => GetLabelColor("lblECNZip").ShouldBe(KM_Colors.BlueLight),
                () => GetLabelColor("lblSFZip").ShouldBe(KM_Colors.BlueLight),
                () => RadioButtonVisible("rblAddress").ShouldBeFalse(),
                () => RadioButtonVisible("rblCity").ShouldBeFalse(),
                () => RadioButtonVisible("rblState").ShouldBeFalse(),
                () => RadioButtonVisible("rblZip").ShouldBeFalse());
        }

        [TestCase(false)]
        [TestCase(true)]
        public void imgbtnCompare_Click_AddressesAreNotValidAndSfEqualsEcn_ControlsTransparent(bool isEcnEqualsSf)
        {
            // Arrange
            const int id = 1;
            const string emailAddress = "testEmail";
            var email = CreateEmail(id, emailAddress);
            var lead = isEcnEqualsSf ? CreateContact(email) : new SF_Lead { Email = emailAddress };
            SetupContacts(email, lead);
            SetupAddresses(false, false);
            var expectedColor = isEcnEqualsSf ? Color.Transparent : KM_Colors.GreyDark;

            // Act
            _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null });

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => GetLabelColor("lblECNAddress").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFAddress").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblAddress").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNState").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFState").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblState").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNZip").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFZip").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblZip").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNCity").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFCity").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblCity").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNCellPhone").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFCellPhone").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblCellPhone").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNLastName").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFLastName").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblLastName").ShouldBe(!isEcnEqualsSf));
        }

        [Test]
        public void imgbtnCompare_Click_Equal_Success()
        {
            // Arrange
            var imgbtnCompare = _testObject.GetField("imgbtnCompare") as ImageButton;
            imgbtnCompare.CommandName = "SF";
            imgbtnCompare.CommandArgument = "1";
            _testObject.SetFieldOrProperty("ECN_Leads", new List<Email> { new Email { EmailID = 1 } });
            ShimSF_Lead.GetSingleStringString = (p1, p2) => new SF_Lead { };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null }));
        }

        [Test]
        public void imgbtnCompare_Click_NotEqual_Success()
        {
            // Arrange
            var imgbtnCompare = _testObject.GetField("imgbtnCompare") as ImageButton;
            imgbtnCompare.CommandName = "ECN";
            imgbtnCompare.CommandArgument = "1";
            _testObject.SetFieldOrProperty("ECN_Leads", new List<Email> { new Email { EmailID = 1 } });
            ShimSF_Lead.GetSingleStringString = (p1, p2) => new SF_Lead
            {
                Street = "test",
                City = "test",
                State = "test",
                PostalCode = "test",
                Country = "test",
                MobilePhone = "test",
                Phone = "test",
                FirstName = "test",
                LastName = "test",
                Email = "test"
            };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null }));
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void imgbtnCompare_Click_AdressValidated_Success(bool isEcnValid, bool isSfValid)
        {
            // Arrange
            var imgbtnCompare = _testObject.GetField("imgbtnCompare") as ImageButton;
            imgbtnCompare.CommandName = "SF";
            imgbtnCompare.CommandArgument = "1";
            _testObject.SetFieldOrProperty("ECN_Leads", new List<Email> { new Email { EmailID = 1 } });
            ShimSF_Lead.GetSingleStringString = (p1, p2) => new SF_Lead { };
            var ecn = new AddressValidator.AddressLocation { IsValid = isEcnValid };
            var sf = new AddressValidator.AddressLocation { IsValid = isSfValid };
            if (!isSfValid && !isEcnValid)
            {
                ecn.IsValid = true;
                sf.IsValid = true;
                sf.Street = "test";
            }
            _testObject.SetFieldOrProperty("Address_ECN", ecn);
            _testObject.SetFieldOrProperty("Address_SF", sf);

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null }));
        }

        [Test]
        public void btnSyncData_Click_Success([Values(true, false)] bool isECN)
        {
            // Arrange
            (_testObject.GetFieldOrProperty("hfECNCustomerID") as HiddenField).Value = "1";
            (_testObject.GetFieldOrProperty("hfSFAccountID") as HiddenField).Value = "1";
            _testObject.SetFieldOrProperty("ECN_Leads", new List<Email> { new Email { EmailID = 1 } });
            _testObject.SetFieldOrProperty("SalesForce_Leads", new List<SF_Lead> { new SF_Lead { Id = "1", OwnerId = "1" } });
            ShimEmail.SaveEmail = (p) => { };
            ShimSF_Lead.UpdateStringSF_Lead = (p1, p2) => isECN;
            InitializeRBLs();
            SetRBLs(isECN ? "ECN" : "SF");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncData_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("MessageLabel") as Label).Text.ShouldBe(isECN ? "Sync successful" : "Unable to sync to Salesforce");
        }

        [Test]
        public void ddlFilter_SelectedIndexChanged_OnlySF_Success([Values(true, false)]bool isVisible)
        {
            // Arrange
            InitializeDdlFilterTest(isVisible);
            var divECNLeads = _testObject.GetField("divECNLeads") as HtmlGenericControl;
            var lblECNLeads = _testObject.GetField("lblECNLeads") as Label;
            var divSFLeads = _testObject.GetField("divSFLeads") as HtmlGenericControl;
            var lblSFLeads = _testObject.GetField("lblSFLeads") as Label;
            var ddlFilter = _testObject.GetField("ddlFilter") as DropDownList;
            ddlFilter.SelectedValue = "OnlySF";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ddlFilter_SelectedIndexChanged", new object[] { null, null }));
            divECNLeads.Visible.ShouldBeFalse();
            lblECNLeads.Visible.ShouldBeFalse();
            divSFLeads.Visible.ShouldBe(isVisible);
            lblSFLeads.Visible.ShouldBe(isVisible);
        }

        [Test]
        public void ddlFilter_SelectedIndexChanged_OnlyECN_Success([Values(true, false)]bool isVisible)
        {
            // Arrange
            InitializeDdlFilterTest(isVisible);
            var divECNLeads = _testObject.GetField("divECNLeads") as HtmlGenericControl;
            var lblECNLeads = _testObject.GetField("lblECNLeads") as Label;
            var divSFLeads = _testObject.GetField("divSFLeads") as HtmlGenericControl;
            var lblSFLeads = _testObject.GetField("lblSFLeads") as Label;
            var ddlFilter = _testObject.GetField("ddlFilter") as DropDownList;
            ddlFilter.SelectedValue = "OnlyECN";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ddlFilter_SelectedIndexChanged", new object[] { null, null }));
            divECNLeads.Visible.ShouldBe(isVisible);
            lblECNLeads.Visible.ShouldBe(isVisible);
            divSFLeads.Visible.ShouldBeFalse();
            lblSFLeads.Visible.ShouldBeFalse();
        }

        [Test]
        public void ddlFilter_SelectedIndexChanged_DiffData_Success([Values(true, false)]bool isVisible)
        {
            // Arrange
            InitializeDdlFilterTest(isVisible);
            var divECNLeads = _testObject.GetField("divECNLeads") as HtmlGenericControl;
            var lblECNLeads = _testObject.GetField("lblECNLeads") as Label;
            var divSFLeads = _testObject.GetField("divSFLeads") as HtmlGenericControl;
            var lblSFLeads = _testObject.GetField("lblSFLeads") as Label;
            var ddlFilter = _testObject.GetField("ddlFilter") as DropDownList;
            ddlFilter.SelectedValue = "DiffData";
            if (isVisible)
            {
                var dgSFLeads = _testObject.GetField("dgSFLeads") as GridView;
                var dgECNLeads = _testObject.GetField("dgECNLeads") as GridView;
                dgSFLeads.Rows[0].BackColor = KM_Colors.GreyDark;
                dgECNLeads.Rows[0].BackColor = KM_Colors.GreyDark;
            }

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ddlFilter_SelectedIndexChanged", new object[] { null, null }));
            divECNLeads.Visible.ShouldBe(isVisible);
            lblECNLeads.Visible.ShouldBe(isVisible);
            divSFLeads.Visible.ShouldBe(isVisible);
            lblSFLeads.Visible.ShouldBe(isVisible);
        }

        [Test]
        public void ddlFilter_SelectedIndexChanged_All_Success()
        {
            // Arrange
            InitializeDdlFilterTest(true);
            var divECNLeads = _testObject.GetField("divECNLeads") as HtmlGenericControl;
            var lblECNLeads = _testObject.GetField("lblECNLeads") as Label;
            var divSFLeads = _testObject.GetField("divSFLeads") as HtmlGenericControl;
            var lblSFLeads = _testObject.GetField("lblSFLeads") as Label;
            var ddlFilter = _testObject.GetField("ddlFilter") as DropDownList;
            ddlFilter.SelectedValue = "All";
            _testObject.SetFieldOrProperty("ECN_Leads", new List<Email> { new Email { } });

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ddlFilter_SelectedIndexChanged", new object[] { null, null }));
            divECNLeads.Visible.ShouldBeTrue();
            lblECNLeads.Visible.ShouldBeTrue();
            divSFLeads.Visible.ShouldBeTrue();
            lblSFLeads.Visible.ShouldBeTrue();
        }

        [Test]
        [TestCase(0, true, 0)]
        [TestCase(1, false, 0)]
        [TestCase(2, false, 1)]
        public void chkECNSelectRow_CheckedChanged_GreyDark_Success(int rowColor, bool isChecked, int ecnCount)
        {
            // Arrange
            var dgECNLeads = _testObject.GetField("dgECNLeads") as GridView;
            dgECNLeads.DataKeyNames = new string[] { "CustomerID" };
            dgECNLeads.DataSource = new List<Email> { new Email { CustomerID = 1 }, new Email { CustomerID = 2 } };
            dgECNLeads.DataBind();
            dgECNLeads.Rows[0].BackColor = rowColor == 0 ? KM_Colors.GreyDark : rowColor == 1 ? KM_Colors.GreyLight : KM_Colors.BlueDark;
            var checkBox1 = dgECNLeads.Rows[0].FindControl("chkECNSelect") as CheckBox;
            var checkBox2 = dgECNLeads.Rows[1].FindControl("chkECNSelect") as CheckBox;
            checkBox1.Attributes["Color"] = "GreyDark";
            checkBox1.Checked = isChecked;
            _testObject.SetFieldOrProperty("ECN_SelectedCount", ecnCount);
            (_testObject.GetField("rblECNGroup") as RadioButtonList).SelectedValue = "new";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("chkECNSelectRow_CheckedChanged", new object[] { checkBox1, null }));
            checkBox2.Enabled.ShouldBe(!isChecked);
        }

        [Test]
        public void chkECNSelectRow_CheckedChanged_GreyLight_Success()
        {
            // Arrange
            var dgECNLeads = _testObject.GetField("dgECNLeads") as GridView;
            dgECNLeads.DataKeyNames = new string[] { "CustomerID" };
            dgECNLeads.DataSource = new List<Email> { new Email { CustomerID = 1 }, new Email { CustomerID = 2 } };
            dgECNLeads.DataBind();
            dgECNLeads.Rows[0].BackColor = KM_Colors.GreyLight;
            var checkBox1 = dgECNLeads.Rows[0].FindControl("chkECNSelect") as CheckBox;
            var checkBox2 = dgECNLeads.Rows[1].FindControl("chkECNSelect") as CheckBox;
            checkBox1.Attributes["Color"] = "GreyLight";
            checkBox1.Checked = true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("chkECNSelectRow_CheckedChanged", new object[] { checkBox1, null }));
        }

        [Test]
        public void chkECNSelectRow_CheckedChanged_BlueLight_Success()
        {
            // Arrange
            var dgECNLeads = _testObject.GetField("dgECNLeads") as GridView;
            dgECNLeads.DataKeyNames = new string[] { "CustomerID" };
            dgECNLeads.DataSource = new List<Email> { new Email { CustomerID = 1 }, new Email { CustomerID = 2 } };
            dgECNLeads.DataBind();
            dgECNLeads.Rows[0].BackColor = KM_Colors.BlueLight;
            var checkBox1 = dgECNLeads.Rows[0].FindControl("chkECNSelect") as CheckBox;
            var checkBox2 = dgECNLeads.Rows[1].FindControl("chkECNSelect") as CheckBox;
            checkBox1.Attributes["Color"] = "BlueLight";
            checkBox1.Checked = true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("chkECNSelectRow_CheckedChanged", new object[] { checkBox1, null }));
        }

        [Test]
        [TestCase(0, true, 0)]
        [TestCase(1, false, 0)]
        [TestCase(2, false, 1)]
        public void chkSFSelectRow_CheckedChanged_GreyDark_Success(int rowColor, bool isChecked, int ecnCount)
        {
            // Arrange
            var dgSFLeads = _testObject.GetField("dgSFLeads") as GridView;
            dgSFLeads.DataKeyNames = new string[] { "Id" };
            dgSFLeads.DataSource = new List<SF_Lead> { new SF_Lead { OwnerId = "1" }, new SF_Lead { OwnerId = "1" } };
            dgSFLeads.DataBind();
            dgSFLeads.Rows[0].BackColor = rowColor == 0 ? KM_Colors.GreyDark : rowColor == 1 ? KM_Colors.GreyLight : KM_Colors.BlueDark;
            var checkBox1 = dgSFLeads.Rows[0].FindControl("chkSFSelect") as CheckBox;
            var checkBox2 = dgSFLeads.Rows[1].FindControl("chkSFSelect") as CheckBox;
            checkBox1.Attributes["Color"] = "GreyDark";
            checkBox1.Checked = isChecked;
            _testObject.SetFieldOrProperty("SF_SelectedCount", ecnCount);
            (_testObject.GetField("rblECNGroup") as RadioButtonList).SelectedValue = "new";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("chkSFSelectRow_CheckedChanged", new object[] { checkBox1, null }));
            checkBox2.Enabled.ShouldBe(!isChecked);
        }

        [Test]
        public void chkSFSelectRow_CheckedChanged_GreyLight_Success()
        {
            // Arrange
            var dgSFLeads = _testObject.GetField("dgSFLeads") as GridView;
            dgSFLeads.DataKeyNames = new string[] { "Id" };
            dgSFLeads.DataSource = new List<SF_Lead> { new SF_Lead { OwnerId = "1" }, new SF_Lead { OwnerId = "1" } };
            dgSFLeads.DataBind();
            dgSFLeads.Rows[0].BackColor = KM_Colors.GreyLight;
            var checkBox1 = dgSFLeads.Rows[0].FindControl("chkSFSelect") as CheckBox;
            var checkBox2 = dgSFLeads.Rows[1].FindControl("chkSFSelect") as CheckBox;
            checkBox1.Attributes["Color"] = "GreyLight";
            checkBox1.Checked = true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("chkSFSelectRow_CheckedChanged", new object[] { checkBox1, null }));
        }

        [Test]
        public void chkSFSelectRow_CheckedChanged_BlueLight_Success()
        {
            // Arrange
            var dgSFLeads = _testObject.GetField("dgSFLeads") as GridView;
            dgSFLeads.DataKeyNames = new string[] { "Id" };
            dgSFLeads.DataSource = new List<SF_Lead> { new SF_Lead { OwnerId = "1" }, new SF_Lead { OwnerId = "1" } };
            dgSFLeads.DataBind();
            dgSFLeads.Rows[0].BackColor = KM_Colors.BlueLight;
            var checkBox1 = dgSFLeads.Rows[0].FindControl("chkSFSelect") as CheckBox;
            var checkBox2 = dgSFLeads.Rows[1].FindControl("chkSFSelect") as CheckBox;
            checkBox1.Attributes["Color"] = "BlueLight";
            checkBox1.Checked = true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("chkSFSelectRow_CheckedChanged", new object[] { checkBox1, null }));
        }

        [Test]
        public void chkECNSelectALL_CheckedChanged_Success([Values(true, false)]bool isChecked)
        {
            // Arrange
            var dgECNLeads = _testObject.GetField("dgECNLeads") as GridView;
            dgECNLeads.DataKeyNames = new string[] { "CustomerID" };
            dgECNLeads.DataSource = new List<Email> { new Email { CustomerID = 1 }, new Email { CustomerID = 2 } };
            dgECNLeads.DataBind();
            dgECNLeads.Rows[0].BackColor = KM_Colors.GreyLight;
            dgECNLeads.Rows[1].BackColor = KM_Colors.GreyLight;
            var checkBox = dgECNLeads.HeaderRow.FindControl("chkECNSelectALL") as CheckBox;
            var checkBox1 = dgECNLeads.Rows[0].FindControl("chkECNSelect") as CheckBox;
            var checkBox2 = dgECNLeads.Rows[1].FindControl("chkECNSelect") as CheckBox;
            var imgbtnECNToSF = _testObject.GetFieldOrProperty("imgbtnECNToSF") as ImageButton;
            checkBox.Checked = isChecked;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("chkECNSelectAll_CheckedChanged", new object[] { checkBox, null }));
            checkBox1.Checked.ShouldBe(isChecked);
            checkBox2.Checked.ShouldBe(isChecked);
            imgbtnECNToSF.Visible.ShouldBe(isChecked);
        }

        [Test]
        public void chkSFSelectALL_CheckedChanged_Success([Values(true, false)]bool isChecked, [Values(true, false)]bool isNew)
        {
            // Arrange
            var dgSFLeads = _testObject.GetField("dgSFLeads") as GridView;
            dgSFLeads.DataSource = new List<SF_Lead> { new SF_Lead { OwnerId = "1" }, new SF_Lead { OwnerId = "1" } };
            dgSFLeads.DataBind();
            dgSFLeads.Rows[0].BackColor = KM_Colors.GreyLight;
            dgSFLeads.Rows[1].BackColor = KM_Colors.GreyLight;
            var checkBox = dgSFLeads.HeaderRow.FindControl("chkSFSelectALL") as CheckBox;
            var checkBox1 = dgSFLeads.Rows[0].FindControl("chkSFSelect") as CheckBox;
            var checkBox2 = dgSFLeads.Rows[1].FindControl("chkSFSelect") as CheckBox;
            checkBox.Checked = isChecked;
            if (isNew)
                (_testObject.GetField("rblECNGroup") as RadioButtonList).SelectedValue = "new";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("chkSFSelectAll_CheckedChanged", new object[] { checkBox, null }));
            checkBox1.Checked.ShouldBe(isChecked);
            checkBox2.Checked.ShouldBe(isChecked);
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void dgSFLeads_Sorting_Success(bool isAscending, bool isDefault)
        {
            // Arrange
            var dgSFLeads = _testObject.GetField("dgSFLeads") as GridView;
            var contactList = new List<SF_Lead> {
                new SF_Lead { Id = "2", OwnerId = "1", Email = "2" },
                new SF_Lead { Id = "3", OwnerId = "1", Email = "3" },
                new SF_Lead { Id = "1", OwnerId = "1", Email = "1" } };
            dgSFLeads.DataKeyNames = new string[] { "Id" };
            dgSFLeads.DataSource = contactList;
            dgSFLeads.DataBind();
            _testObject.SetFieldOrProperty("SalesForce_Leads", contactList);
            var args = new GridViewSortEventArgs("Email", System.Web.UI.WebControls.SortDirection.Ascending);
            _testObject.SetFieldOrProperty("SFSortExp", !isDefault ? "Email" : string.Empty);
            _testObject.SetFieldOrProperty("SFSortDir", !isAscending ?
                System.Web.UI.WebControls.SortDirection.Ascending : System.Web.UI.WebControls.SortDirection.Descending);

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("dgSFLeads_Sorting", new object[] { dgSFLeads, args }));
            dgSFLeads.ShouldSatisfyAllConditions(
                () => dgSFLeads.ShouldNotBeNull(),
                () => (dgSFLeads.DataKeys[0].Value).ShouldBe((isDefault || isAscending) ? "1" : "3"),
                () => (dgSFLeads.DataKeys[1].Value).ShouldBe("2"),
                () => (dgSFLeads.DataKeys[2].Value).ShouldBe((isDefault || isAscending) ? "3" : "1"));
        }

        [Test]
        [TestCase(true, false, "EmailAddress")]
        [TestCase(false, false, "EmailAddress")]
        [TestCase(true, true, "FirstName")]
        [TestCase(false, false, "FirstName")]
        [TestCase(true, false, "LastName")]
        [TestCase(false, false, "LastName")]
        public void dgECNLeads_Sorting_Success(bool isAscending, bool isDefault, string dataKey)
        {
            // Arrange
            var dgECNLeads = _testObject.GetField("dgECNLeads") as GridView;
            var sortKey = dataKey == "EmailAddress" ? "email" : dataKey.ToLower();
            var ecnLeads = new List<Email> {
                new Email { EmailID = 2, EmailAddress = "2", FirstName = "2", LastName = "2" },
                new Email { EmailID = 3, EmailAddress = "3", FirstName = "3", LastName = "3" },
                new Email { EmailID = 1, EmailAddress = "1", FirstName = "1", LastName = "1" } };
            dgECNLeads.DataKeyNames = new string[] { dataKey };
            dgECNLeads.DataSource = ecnLeads;
            dgECNLeads.DataBind();
            var args = new GridViewSortEventArgs(sortKey, System.Web.UI.WebControls.SortDirection.Ascending);
            _testObject.SetFieldOrProperty("ECN_Leads", ecnLeads);
            _testObject.SetFieldOrProperty("ECNSortExp", !isDefault ? sortKey : string.Empty);
            _testObject.SetFieldOrProperty("ECNSortDir", !isAscending ?
                System.Web.UI.WebControls.SortDirection.Ascending : System.Web.UI.WebControls.SortDirection.Descending);

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("dgECNLeads_Sorting", new object[] { dgECNLeads, args }));
            dgECNLeads.ShouldSatisfyAllConditions(
                () => dgECNLeads.ShouldNotBeNull(),
                () => (dgECNLeads.DataKeys[0].Value).ShouldBe((isDefault || isAscending) ? "1" : "3"),
                () => (dgECNLeads.DataKeys[1].Value).ShouldBe("2"),
                () => (dgECNLeads.DataKeys[2].Value).ShouldBe((isDefault || isAscending) ? "3" : "1"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void chkSFSelectRow_CheckedChanged_OnCheckColorGreyDark_AllSFCheckboxesDisabled(bool isChecked)
        {
            // Arrange 
            var ecnGrid = SetupEcnContactGrid(EcnGridId);
            var allEcnCheckboxes = GetAllCheckBoxes(ecnGrid, EcnCheckboxId, EcnHeaderCheckboxId).ToArray();

            var sfGrid = SetupSFContactGrid(SfGridId);
            var sender = sfGrid.Rows[0].FindControl(SfCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = isChecked;
            var secondCheckbox = sfGrid.Rows[1].FindControl(SfCheckboxId) as CheckBox;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Asset
            sender.ShouldSatisfyAllConditions(
                () => sender.Checked.ShouldBe(isChecked),
                () => sender.Enabled.ShouldBeTrue());

            secondCheckbox.ShouldSatisfyAllConditions(
                () => secondCheckbox.Checked.ShouldBeFalse(),
                () => secondCheckbox.Enabled.ShouldBe(!isChecked),
                () => secondCheckbox.Font.Italic.ShouldBe(isChecked));

            allEcnCheckboxes.ShouldAllBe(x => x.Enabled && !x.Checked);
            var ecnCount = GetProperty<int>(EcnCountPropertyName);
            ecnCount.ShouldBe(Zero);
        }

        [TestCase(true, 0, 1)]
        [TestCase(false, 0, 0)]
        [TestCase(false, 1, 0)]
        public void chkSFSelectRow_CheckedChanged_SFCountShouldBeGreatOrEqualZero(bool isChecked, int initialCount, int expectedCount)
        {
            // Arrange
            var sender = new CheckBox() { Checked = isChecked };
            SetColor(sender, string.Empty);
            SetCount(SfCountPropertyName, initialCount);

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            var sfCount = GetProperty<int>(SfCountPropertyName);
            var ecnCount = GetProperty<int>(EcnCountPropertyName);

            sfCount.ShouldBe(expectedCount);
            ecnCount.ShouldBe(Zero);
        }

        [TestCase(SfCheckedMethodName)]
        [TestCase(EcnCheckedMethodName)]
        public void chkSFSelectRow_CheckedChanged_OnUnCheck_ControlsAreHide(string method)
        {
            // Arrange
            var sender = new CheckBox() { Checked = false };
            SetColor(sender, string.Empty);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            var imgbtnSftoEcn = GetProperty<ImageButton>(ImgbtnSftoEcnProperty);
            imgbtnCompare.Visible = false;
            imgbtnEcntoSf.Visible = false;
            imgbtnSftoEcn.Visible = false;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcntoSf.Visible.ShouldBeFalse();
            imgbtnSftoEcn.Visible.ShouldBeFalse();
        }

        [Test]
        public void chkSFSelectRow_CheckedChanged_OnCheckAndColorGreyLight_ControlsAreHide()
        {
            // Arrange
            var sender = new CheckBox() { Checked = true };
            SetColor(sender, GreyLightColor);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            var imgbtnSftoEcn = GetProperty<ImageButton>(ImgbtnSftoEcnProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcntoSf.Visible = true;
            imgbtnSftoEcn.Visible = false;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcntoSf.Visible.ShouldBeFalse();
            imgbtnSftoEcn.Visible.ShouldBeTrue();
        }

        [Test]
        public void chkSFSelectRow_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorGreyDark_VerifyControls()
        {
            // Arrange
            var sfGrid = SetupSFContactGrid(SfGridId);
            var sender = sfGrid.Rows[0].FindControl(SfCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.GreyDark);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            var imgbtnSftoEcn = GetProperty<ImageButton>(ImgbtnSftoEcnProperty);
            imgbtnCompare.Visible = false;
            imgbtnCompare.CommandArgument = null;
            imgbtnCompare.CommandName = null;
            imgbtnEcntoSf.Visible = true;
            imgbtnSftoEcn.Visible = true;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            imgbtnCompare.ShouldSatisfyAllConditions(
                 () => imgbtnCompare.Visible.ShouldBeTrue(),
                 () => imgbtnCompare.CommandArgument.ShouldBe(CommandArgument),
                 () => imgbtnCompare.CommandName.ShouldBe(SfCommandName));

            imgbtnEcntoSf.Visible.ShouldBeFalse();
            imgbtnSftoEcn.Visible.ShouldBeFalse();
        }

        [Test]
        public void chkSFSelectRow_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorGreyLight_VerifyControls()
        {
            // Arrange
            var sfGrid = SetupSFContactGrid(SfGridId);
            var sender = sfGrid.Rows[0].FindControl(SfCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.GreyLight);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            var imgbtnSftoEcn = GetProperty<ImageButton>(ImgbtnSftoEcnProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcntoSf.Visible = true;
            imgbtnSftoEcn.CommandArgument = null;
            imgbtnSftoEcn.Visible = false;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcntoSf.Visible.ShouldBeFalse();
            imgbtnSftoEcn.ShouldSatisfyAllConditions(
                () => imgbtnSftoEcn.Visible.ShouldBeTrue(),
                () => imgbtnSftoEcn.CommandArgument = CommandArgument);
        }

        [Test]
        public void chkSFSelectRow_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorBlueDark_VerifyControls()
        {
            // Arrange
            var sfGrid = SetupSFContactGrid(SfGridId);
            var sender = sfGrid.Rows[0].FindControl(SfCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.BlueDark);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            var imgbtnSftoEcn = GetProperty<ImageButton>(ImgbtnSftoEcnProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcntoSf.Visible = true;
            imgbtnSftoEcn.Visible = true;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcntoSf.Visible.ShouldBeFalse();
            imgbtnSftoEcn.Visible.ShouldBeFalse();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void chkECNSelectRow_CheckedChanged_OnCheckColorGreyDark_AllEcnCheckboxesDisabled(bool isChecked)
        {
            // Arrange 
            var sfGrid = SetupSFContactGrid(SfGridId);
            var allSfCheckboxes = GetAllCheckBoxes(sfGrid, SfCheckboxId, SfHeaderCheckboxId).ToArray();

            var parentGrid = SetupEcnContactGrid(EcnGridId);
            var sender = parentGrid.Rows[0].FindControl(EcnCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = isChecked;
            var secondCheckbox = parentGrid.Rows[1].FindControl(EcnCheckboxId) as CheckBox;
            var expectedColor = isChecked ? System.Drawing.Color.Gray : System.Drawing.Color.Black;
            // Act
            ExecuteOnCheck(sender, EcnCheckedMethodName);

            // Asset
            sender.ShouldSatisfyAllConditions(
                () => sender.Checked.ShouldBe(isChecked),
                () => sender.Enabled.ShouldBeTrue());

            secondCheckbox.ShouldSatisfyAllConditions(
                () => secondCheckbox.Checked.ShouldBeFalse(),
                () => secondCheckbox.Enabled.ShouldBe(!isChecked),
                () => secondCheckbox.Font.Italic.ShouldBe(isChecked),
                () => secondCheckbox.ForeColor.ShouldBe(expectedColor));

            allSfCheckboxes.ShouldAllBe(x => x.Enabled && !x.Checked);
            var sfCount = GetProperty<int>(SfCountPropertyName);
            sfCount.ShouldBe(Zero);
        }

        [TestCase(true, 0, 1)]
        [TestCase(false, 0, 0)]
        [TestCase(false, 1, 0)]
        public void chkECNSelectRow_CheckedChanged_ECNCountShouldBeGreatOrEqualZero(bool isChecked, int initialCount, int expectedCount)
        {
            // Arrange
            var sender = new CheckBox() { Checked = isChecked };
            SetColor(sender, string.Empty);
            SetCount(EcnCountPropertyName, initialCount);

            // Act
            ExecuteOnCheck(sender, EcnCheckedMethodName);

            // Assert
            var sfCount = GetProperty<int>(SfCountPropertyName);
            var ecnCount = GetProperty<int>(EcnCountPropertyName);

            sfCount.ShouldBe(Zero);
            ecnCount.ShouldBe(expectedCount);
        }

        [Test]
        public void chkECNSelectRow_CheckedChanged_OnCheckAndColorGreyLight_ControlsAreHide()
        {
            // Arrange
            var sender = new CheckBox() { Checked = true };
            SetColor(sender, GreyLightColor);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            var imgbtnSftoEcn = GetProperty<ImageButton>(ImgbtnSftoEcnProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcntoSf.Visible = false;
            imgbtnSftoEcn.Visible = true;

            // Act
            ExecuteOnCheck(sender, EcnCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnSftoEcn.Visible.ShouldBeFalse();
            imgbtnEcntoSf.Visible.ShouldBeTrue();
        }

        [Test]
        public void chkECNSelectRow_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorGreyDark_VerifyControls()
        {
            // Arrange
            var grid = SetupEcnContactGrid(EcnGridId);
            var sender = grid.Rows[0].FindControl(EcnCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.GreyDark);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            var imgbtnSftoEcn = GetProperty<ImageButton>(ImgbtnSftoEcnProperty);
            imgbtnCompare.Visible = false;
            imgbtnCompare.CommandArgument = null;
            imgbtnCompare.CommandName = null;
            imgbtnEcntoSf.Visible = true;
            imgbtnSftoEcn.Visible = true;

            // Act
            ExecuteOnCheck(sender, EcnCheckedMethodName);

            // Assert
            imgbtnCompare.ShouldSatisfyAllConditions(
                 () => imgbtnCompare.Visible.ShouldBeTrue(),
                 () => imgbtnCompare.CommandArgument.ShouldBe(CommandArgument),
                 () => imgbtnCompare.CommandName.ShouldBe(EcnCommandName));

            imgbtnEcntoSf.Visible.ShouldBeFalse();
            imgbtnSftoEcn.Visible.ShouldBeFalse();
        }

        [Test]
        public void chkECNSelectRow_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorGreyLight_VerifyControls()
        {
            // Arrange
            var grid = SetupEcnContactGrid(EcnGridId);
            var sender = grid.Rows[0].FindControl(EcnCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.GreyLight);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            var imgbtnSftoEcn = GetProperty<ImageButton>(ImgbtnSftoEcnProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcntoSf.Visible = true;
            imgbtnSftoEcn.CommandArgument = null;
            imgbtnSftoEcn.Visible = false;

            // Act
            ExecuteOnCheck(sender, EcnCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnSftoEcn.Visible.ShouldBeFalse();
            imgbtnEcntoSf.ShouldSatisfyAllConditions(
                () => imgbtnEcntoSf.Visible.ShouldBeTrue(),
                () => imgbtnEcntoSf.CommandArgument = CommandArgument);
        }

        [Test]
        public void chkECNSelectRow_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorBlueDark_VerifyControls()
        {
            // Arrange
            var grid = SetupEcnContactGrid(EcnGridId);
            var sender = grid.Rows[0].FindControl(EcnCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.BlueDark);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcnToSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            var imgbtnSftoEcn = GetProperty<ImageButton>(ImgbtnSftoEcnProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcnToSf.Visible = true;
            imgbtnSftoEcn.Visible = true;

            // Act
            ExecuteOnCheck(sender, EcnCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcnToSf.Visible.ShouldBeFalse();
            imgbtnSftoEcn.Visible.ShouldBeFalse();
        }

        [TestCase("DiffData", false)]
        [TestCase("All", true)]
        public void dgSFLeads_RowDataBound_SetFilter_ChangeHeaderCheckboxVisibility(string selectedValue, bool expectedVisibility)
        {
            // Arrange
            const string headerCheckboxId = "chkSFSelectAll";
            const string filterPropertyName = "ddlFilter";
            var grid = SetupSFContactGrid(SfGridId);
            var headerRow = grid.HeaderRow;
            var headerCheckbox = headerRow.FindControl(headerCheckboxId) as CheckBox;
            var args = new GridViewRowEventArgs(headerRow);
            var ddlFilter = GetProperty<DropDownList>(filterPropertyName);
            ddlFilter.Items.Add(selectedValue);
            ddlFilter.SelectedIndex = 0;

            // Act
            _testObject.Invoke("dgSFLeads_RowDataBound", new object[] { null, args });

            // Assert
            headerCheckbox.Visible.ShouldBe(expectedVisibility);
        }

        [Test]
        public void CreateUDF_PassNotExistGroupId_ReturnsEmptyHashtable()
        {
            // Arrange
            const int groupId = 1;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (id, user, getChildren) => new List<GroupDataFields>();
            // Act
            var result = _testObject.Invoke("CreateUDF", groupId, new User()) as Hashtable;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Keys.Count.ShouldBe(Zero));
        }

        [Test]
        public void CreateUDF_PassExistGroupId_ReturnsValidHashtable()
        {
            // Arrange
            const int GroupId = 1;
            const int FieldsId = 2;
            const string ShortName = "ShortName";
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (id, user, getChildren) => new[]
            {
                new GroupDataFields()
                {
                    GroupID = id,
                    ShortName = ShortName,
                    GroupDataFieldsID = FieldsId
                }
            }.ToList();

            // Act
            var result = _testObject.Invoke("CreateUDF", GroupId, new User()) as Hashtable;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(1),
                () => result.ContainsValue(FieldsId).ShouldBeTrue(),
                () => result.ContainsKey($"user_{ShortName.ToLower()}").ShouldBeTrue());
        }

        [Test]
        public void CreateUDF_PassGroupId_GroupTryingToSave()
        {
            // Arrange
            const int groupId = 1;
            const string isPublic = "N";
            const string firstLongName = "SalesforceID";
            const string secondLongName = "Salesforce Type";
            const string firstShortName = "SFID";
            const string secondShortName = "SFType";
            GroupDataFields firstExpectedGroup = null;
            GroupDataFields secondExpectedGroup = null;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (id, user, getChildren) => new List<GroupDataFields>();
            ShimGroupDataFields.SaveGroupDataFieldsUser = (gr, user) =>
             {
                 if (firstExpectedGroup == null)
                 {
                     firstExpectedGroup = gr;
                 }
                 else if (secondExpectedGroup == null)
                 {
                     secondExpectedGroup = gr;
                 }
                 return Zero;
             };
            // Act
            var result = _testObject.Invoke("CreateUDF", groupId, new User()) as Hashtable;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Keys.Count.ShouldBe(Zero));

            firstExpectedGroup.ShouldSatisfyAllConditions(
                () => firstExpectedGroup.ShouldNotBeNull(),
                () => firstExpectedGroup.GroupID.ShouldBe(groupId),
                () => firstExpectedGroup.IsPublic.ShouldBe(isPublic),
                () => firstExpectedGroup.ShortName.ShouldBe(firstShortName),
                () => firstExpectedGroup.LongName.ShouldBe(firstLongName));

            secondExpectedGroup.ShouldSatisfyAllConditions(
              () => secondExpectedGroup.ShouldNotBeNull(),
              () => secondExpectedGroup.GroupID.ShouldBe(groupId),
              () => secondExpectedGroup.IsPublic.ShouldBe(isPublic),
              () => secondExpectedGroup.ShortName.ShouldBe(secondShortName),
              () => secondExpectedGroup.LongName.ShouldBe(secondLongName));
        }

[Test]
        public void DisplayResults_PassEmptyTable_DoNotBindAnything()
        {
            // Arrange
            var data = new DataTable();

            // Act
            _testObject.Invoke("DisplayResults", data);

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => GetProperty<Label>("MessageLabel").Text.ShouldBeEmpty(),
                () => GetProperty<DataGrid>("ResultsGrid").DataSource.ShouldBeNull());
        }

        [TestCase("T", "Total Records in the Import", 1)]
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
            const string CountsColumn = "Counts";
            const int FirstRow = 0;
            const int LastRow = 1;
            const int Counts = 1;
            const int expectedTotals = 2;

            var data = new DataTable();
            data.Columns.Add(ActionsColumn);
            data.Columns.Add("Counts");

            var row = data.NewRow();
            row[ActionsColumn] = action;
            row[CountsColumn] = Counts;
            data.Rows.Add(row);

            var duplicateRow = data.NewRow();
            duplicateRow[ActionsColumn] = action;
            duplicateRow[CountsColumn] = Counts;
            data.Rows.Add(duplicateRow);

            // Act
            _testObject.Invoke("DisplayResults", data);

            // Assert
            GetProperty<Label>("MessageLabel").Text.ShouldBe("Import Results");
            var dataSource = GetProperty<DataGrid>("ResultsGrid").DataSource as DataTable;
            dataSource.ShouldNotBeNull();
            dataSource.ShouldSatisfyAllConditions(
                () => dataSource.Rows[FirstRow][ActionsColumn].ShouldBe(expectedActionName),
                () => dataSource.Rows[FirstRow][TotalsColumn].ShouldBe(expectedTotals.ToString()),
                () => dataSource.Rows[FirstRow][SortColumn].ShouldBe(sortOrder.ToString()),
                () => dataSource.Rows[LastRow][ActionsColumn].ShouldBe("&nbsp;"),
                () => dataSource.Rows[LastRow][TotalsColumn].ShouldBe(" "),
                () => dataSource.Rows[LastRow][SortColumn].ShouldBe("8"));
        }
        protected GridView SetupEcnContactGrid(string id)
        {
            var grid = _testObject.GetField(id) as GridView;
            grid.DataKeyNames = new string[] { "CustomerID" };
            grid.DataSource = new List<Email> { new Email { CustomerID = 1 }, new Email { CustomerID = 2 } };
            grid.DataBind();
            return grid;
        }

        protected GridView SetupSFContactGrid(string id)
        {
            var grid = _testObject.GetField(id) as GridView;
            grid.DataKeyNames = new string[] { "OwnerId" };
            grid.DataSource = new List<SF_Lead> { new SF_Lead { OwnerId = "1" }, new SF_Lead { OwnerId = "1" } };
            grid.DataBind();
            return grid;
        }

        private void InitializeDdlFilterTest(bool visible)
        {
            var ddlFilter = _testObject.GetField("ddlFilter") as DropDownList;
            ddlFilter.Items.Add("OnlySF");
            ddlFilter.Items.Add("OnlyECN");
            ddlFilter.Items.Add("DiffData");
            ddlFilter.Items.Add("All");
            var dgSFLeads = _testObject.GetField("dgSFLeads") as GridView;
            dgSFLeads.DataSource = new List<SF_Lead> { new SF_Lead { OwnerId = "1", Email = "dummy" } };
            dgSFLeads.DataBind();
            var dgECNLeads = _testObject.GetField("dgECNLeads") as GridView;
            dgECNLeads.DataSource = new List<Email> { new Email { EmailAddress = "dummy" } };
            dgECNLeads.DataBind();
            if (!visible)
            {
                dgSFLeads.Rows[0].BackColor = KM_Colors.BlueLight;
                dgECNLeads.Rows[0].BackColor = KM_Colors.BlueLight;
            }
            else
            {
                dgSFLeads.Rows[0].BackColor = KM_Colors.GreyLight;
                dgECNLeads.Rows[0].BackColor = KM_Colors.GreyLight;
            }
        }

        private void SetRBLs(string value)
        {
            (_testObject.GetFieldOrProperty("rblFirstName") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblLastName") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblEmail") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblAddress") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblCity") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblState") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblCountry") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblPhone") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblCellPhone") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblZip") as RadioButtonList).SelectedValue = value;
        }

        private void InitializeRBLs()
        {
            FillRBLValues("rblFirstName", new List<string> { "SF", "ECN" });
            FillRBLValues("rblLastName", new List<string> { "SF", "ECN" });
            FillRBLValues("rblEmail", new List<string> { "SF", "ECN" });
            FillRBLValues("rblAddress", new List<string> { "SF", "ECN" });
            FillRBLValues("rblCity", new List<string> { "SF", "ECN" });
            FillRBLValues("rblState", new List<string> { "SF", "ECN" });
            FillRBLValues("rblCountry", new List<string> { "SF", "ECN" });
            FillRBLValues("rblPhone", new List<string> { "SF", "ECN" });
            FillRBLValues("rblCellPhone", new List<string> { "SF", "ECN" });
            FillRBLValues("rblZip", new List<string> { "SF", "ECN" });
        }

        private void FillRBLValues(string filedName, List<string> items)
        {
            var rbl = _testObject.GetFieldOrProperty(filedName) as RadioButtonList;
            foreach (string item in items)
            {
                rbl.Items.Add(item);
            }
        }

        private void CreateTestObjects()
        {
            _testPage = new SF_Leads();
            InitializeAllControls(_testPage);
            _testObject = new PrivateObject(_testPage);
            _testObject.SetFieldOrProperty("SF_UserList", new List<SF_User> { });
            _testObject.SetFieldOrProperty("ECN_Leads", new List<Email> { });
            _testObject.SetFieldOrProperty("SalesForce_Leads", new List<SF_Lead> { new SF_Lead { OwnerId = "1" } });
            var kmSearch = _testObject.GetField("kmSearch");
            InitializeAllControls(kmSearch);
            var rblECNGroup = _testObject.GetField("rblECNGroup") as RadioButtonList;
            rblECNGroup.Items.Add(new ListItem { Value = "new" });
            rblECNGroup.Items.Add(new ListItem { Value = "existing" });
            InitializeGrids();
            var ddlECNGroup = _testObject.GetField("ddlECNGroup") as DropDownList;
            ddlECNGroup.Items.Add(new ListItem { Value = "0" });
            ddlECNGroup.Items.Add(new ListItem { Value = "1" });
            var ddlSFTags = _testObject.GetField("ddlSFTags") as DropDownList;
            ddlSFTags.Items.Add(new ListItem { Value = "0" });
            ddlSFTags.Items.Add(new ListItem { Value = "1" });
        }

        private void InitializeGrids()
        {
            var dgSFLeads = _testObject.GetField("dgSFLeads") as GridView;
            var chkSFSelectAll = new CheckBox { ID = "chkSFSelectAll" };
            var chkSFSelect = new CheckBox { ID = "chkSFSelect" };
            dgSFLeads.Columns.Add(new TemplateField
            {
                HeaderTemplate = new TestTemplateItem { control = chkSFSelectAll },
                ItemTemplate = new TestTemplateItem { control = chkSFSelect }
            });
            dgSFLeads.Columns.Add(new BoundField { DataField = "Email", SortExpression = "Email" });
            dgSFLeads.Columns.Add(new BoundField { DataField = "FirstName", SortExpression = "FirstName" });
            dgSFLeads.Columns.Add(new BoundField { DataField = "LastName", SortExpression = "LastName" });
            dgSFLeads.Columns.Add(new BoundField { DataField = "Company", SortExpression = "Company" });
            dgSFLeads.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new Label { ID = "lblSFLeadOwner" } }
            });
            dgSFLeads.RowDataBound += (s, e) => _testObject.Invoke("dgSFLeads_RowDataBound", new object[] { s, e });
            dgSFLeads.Sorting += (s, e) => _testObject.Invoke("dgSFLeads_Sorting", new object[] { s, e });
            var dgECNLeads = _testObject.GetField("dgECNLeads") as GridView;
            var chkECNSelectAll = new CheckBox { ID = "chkECNSelectAll" };
            var chkECNSelect = new CheckBox { ID = "chkECNSelect" };
            dgECNLeads.Columns.Add(new TemplateField
            {
                HeaderTemplate = new TestTemplateItem { control = chkECNSelectAll },
                ItemTemplate = new TestTemplateItem { control = chkECNSelect }
            });
            dgECNLeads.Columns.Add(new BoundField { DataField = "EmailAddress", SortExpression = "Email" });
            dgECNLeads.Columns.Add(new BoundField { DataField = "FirstName", SortExpression = "FirstName" });
            dgECNLeads.Columns.Add(new BoundField { DataField = "LastName", SortExpression = "LastName" });
            dgECNLeads.RowDataBound += (s, e) => _testObject.Invoke("dgECNLeads_RowDataBound", new object[] { s, e });
            dgECNLeads.Sorting += (s, e) => _testObject.Invoke("dgECNLeads_Sorting", new object[] { s, e });
        }

        private void InitializeFakes()
        {
            ShimFolder.GetByCustomerIDInt32User = (id, user) => new List<Folder> { };
            ShimGroup.GetByCustomerIDInt32UserString = (id, user, p) => new List<Group> { };
            ShimSF_User.GetAllString = (token) => new List<SF_User> { };
            ShimSF_TagDefinition.GetAllString = (token) => new List<SF_TagDefinition> { };
            ShimGroup.ExistsInt32StringInt32Int32 = (p1, p2, p3, p4) => false;
            ShimGroup.SaveGroupUser = (p1, p2) => 1;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { ShortName = "sfid" }, new GroupDataFields { ShortName = "sftype" } };
            ShimEmail.GetByGroupIDInt32User = (p1, p2) => new List<Email> { new Email { } };
            ShimGroup.GetByGroupIDInt32User = (p1, p2) => new Group { };
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) =>
                new DataTable { Columns = { "Action", "Counts" }, Rows = { { "1", "1" } } };
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () =>
            {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
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

        private IEnumerable<System.Action> VerifyEcnLabelsAndHiddenFields(Email expectedEmail, string contactId)
        {
            yield return () => GetProperty<HiddenField>("hfECNCustomerID").Value.ShouldBe(expectedEmail.EmailID.ToString());
            yield return () => GetProperty<HiddenField>("hfSFAccountID").Value.ShouldBe(contactId);
            yield return () => GetLabelText("lblECNAddress").ShouldBe(expectedEmail.Address + " " + expectedEmail.Address2);
            yield return () => GetLabelText("lblECNCellPhone").ShouldBe(expectedEmail.Mobile);
            yield return () => GetLabelText("lblECNCity").ShouldBe(expectedEmail.City);
            yield return () => GetLabelText("lblECNEmail").ShouldBe(expectedEmail.EmailAddress);
            yield return () => GetLabelText("lblECNFirstName").ShouldBe(expectedEmail.FirstName);
            yield return () => GetLabelText("lblECNLastName").ShouldBe(expectedEmail.LastName);
            yield return () => GetLabelText("lblECNPhone").ShouldBe(expectedEmail.Voice);
            yield return () => GetLabelText("lblECNState").ShouldBe(expectedEmail.State);
            yield return () => GetLabelText("lblECNZip").ShouldBe(expectedEmail.Zip);
        }

        private void SetupContacts(Email email, SF_Lead lead)
        {
            _testObject.SetFieldOrProperty("ECN_Leads", new List<Email> { email });
            ShimSF_Lead.GetSingleStringString = (p1, p2) => lead;
        }
        private SF_Lead CreateContact(Email email)
        {
            return new SF_Lead()
            {
                Street = email.Address,
                City = email.City,
                State = email.State,
                PostalCode = email.Zip,
                Country = email.Country,
                Email = email.EmailAddress,
                MailingState = email.State,
                MailingPostalCode = email.Zip,
                MailingStreet = email.FullAddress(),
                MailingCity = email.City,
                LastName = email.LastName,
                FirstName = email.FirstName,
                Phone = email.Voice,
                MobilePhone = email.Mobile
            };
        }
    }
}
