using System;
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
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using ecn.communicator.main.Salesforce.Controls;
using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.Entity.Fakes;
using ecn.communicator.main.Salesforce.SF_Pages;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;

namespace ECN.Communicator.Tests.Main.Salesforce.SF_Pages
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.Salesforce.SF_Pages.SF_Accounts"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_AccountsTest : SalesForcePageTestBase
    {
        private const string CommandArgument = "1";
        private const string SfCommandName = "SF";
        private const string EcnCommandName = "ECN";
        private const string ImgbtnCompareProperty = "imgbtnCompare";
        private const string ImgbtnEcntoSfProperty = "imgbtnECNToSF";
        private const string EcnCountPropertyName = "ECN_SelectedCount";
        private const string SfCountPropertyName = "SF_SelectedCount";
        private const string SfCheckboxId = "cbSFselect";
        private const string EcnCheckboxId = "cbECNselect";
        private const string EcnHeaderCheckboxId = "cbECNselectALL";
        private const string SfHeaderCheckboxId = "cbSFselectALL";
        private const string SfGridId = "gvSFAccounts";
        private const string EcnGridId = "gvECNAccounts";
        private const string GreyDarkColor = "GreyDark";
        private const string GreyLightColor = "GreyLight";
        private const int Zero = 0;
        private const string SfCheckedMethodName = "cbSFselect_CheckedChanged";
        private const string EcnCheckedMethodName = "cbECNselect_CheckedChanged";
        private const string ECN_Customer_ListPropertyName = "ECN_Customer_List";
        private const string SF_Account_ListPropertyName = "SF_Account_List";

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
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldContain("You must first log into Salesforce to use this page");
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void imgbtnECNToSF_Click_Success(bool isLightColor, bool success)
        {
            // Arrange
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            gvECNAccounts.DataSource = new List<Customer> { new Customer { CustomerID = 1 } };
            gvECNAccounts.DataBind();
            var cbECNselect = (gvECNAccounts.Rows[0].FindControl("cbECNselect") as CheckBox);
            cbECNselect.Checked = true;
            cbECNselect.Attributes["CustomerID"] = "1";
            gvECNAccounts.Rows[0].BackColor = isLightColor ? KM_Colors.GreyLight : KM_Colors.GreyDark;
            ShimSF_Account.InsertStringSF_Account = (p1, p2) => success;
            ShimSF_Account.UpdateStringSF_Account = (p1, p2) => success;
            _testObject.SetFieldOrProperty("ECN_Customer_List", new List<Customer> { new Customer { CustomerID = 1 } });
            _testObject.SetFieldOrProperty("SF_Account_List", new List<SF_Account> { new SF_Account { Id = "1" } });

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("imgbtnECNToSF_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("kmMsg") as Message).MessageText.ShouldBeNull();
        }

        [TestCase("ECN")]
        [TestCase("ecn")]
        public void imgbtnCompare_Click_EcnLabelsAreEqualsCustomerWithIdFromArgument(string command)
        {
            // Arrange
            const string entityId = "entityId";
            const string email = "email";
            const int id = 1;
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty); ;
            imgbtnCompare.CommandName = command;
            imgbtnCompare.CommandArgument = id.ToString();
            var expectedCustomer = CreateCustomer(id, email);
            var account = new SF_Account() { Id = entityId, Name = expectedCustomer.CustomerName };
            SetupListValues(expectedCustomer, account);

            // Act
            _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null });

            // Assert
            _testObject.ShouldSatisfyAllConditions(VerifyEcnLabelsAndHiddenFields(expectedCustomer, entityId).ToArray());
        }

        [TestCase("SF")]
        [TestCase("sf")]
        public void imgbtnCompare_Click_EcnLabelsAreEqualsCustomerWithNameFromAccount(string command)
        {
            // Arrange
            const string email = "email";
            const int id = 1;
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            imgbtnCompare.CommandName = command;
            imgbtnCompare.CommandArgument = id.ToString();
            var expectedCustomer = CreateCustomer(id, email);
            var account = new SF_Account() { Id = id.ToString(), Name = expectedCustomer.CustomerName };
            SetupListValues(expectedCustomer, account);

            // Act
            _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null });

            // Assert
            _testObject.ShouldSatisfyAllConditions(VerifyEcnLabelsAndHiddenFields(expectedCustomer, id.ToString()).ToArray());
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void imgbtnCompare_Click_AddreesValid_ControlsHighligthed(bool isEcnValid, bool isSfValid)
        {
            // Arrange
            const int id = 1;
            const string emailAddress = "testEmail";
            var customer = new Customer { Email = emailAddress, CustomerID = id };
            var account = new SF_Account { Email = emailAddress };
            SetupListValues(customer, account);
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
            var customer = new Customer { Email = emailAddress, CustomerID = id };
            var account = new SF_Account { Email = emailAddress };
            SetupListValues(customer, account);
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
                () => GetLabelColor("lblECNCountry").ShouldBe(KM_Colors.BlueLight),
                () => GetLabelColor("lblSFCountry").ShouldBe(KM_Colors.BlueLight),
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
            var customer = CreateCustomer(id, emailAddress);
            var account = isEcnEqualsSf
                ? CreateAccount(customer)
                : new SF_Account { Name = customer.CustomerName, Email = emailAddress };
            SetupListValues(customer, account);
            SetupAddresses(false, false);
            var expectedColor = isEcnEqualsSf ? Color.Transparent : KM_Colors.GreyDark;

            // Act
            _testObject.Invoke("imgbtnCompare_Click", new object[] { null, null });

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => GetLabelColor("lblECNAddress").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFAddress").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblAddress").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNCity").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFCity").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblCity").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNCountry").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFCountry").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblCountry").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNState").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFState").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblState").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNZip").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFZip").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblZip").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNFax").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFFax").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblFax").ShouldBe(!isEcnEqualsSf),
                () => GetLabelColor("lblECNPhone").ShouldBe(expectedColor),
                () => GetLabelColor("lblSFPhone").ShouldBe(expectedColor),
                () => RadioButtonVisible("rblPhone").ShouldBe(!isEcnEqualsSf));
        }

        [Test]
        public void imgbtnCompare_Click_Equal_Success()
        {
            // Arrange
            var imgbtnCompare = _testObject.GetField("imgbtnCompare") as ImageButton;
            imgbtnCompare.CommandName = "SF";
            imgbtnCompare.CommandArgument = "1";
            _testObject.SetFieldOrProperty("ECN_Customer_List", new List<Customer> { new Customer { CustomerName = "test", CustomerID = 1 } });
            _testObject.SetFieldOrProperty("SF_Account_List", new List<SF_Account> { new SF_Account { Id = "1", Name = "test" } });

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
            _testObject.SetFieldOrProperty("ECN_Customer_List", new List<Customer> { new Customer { CustomerName = "test", CustomerID = 1, Address = "address" } });
            _testObject.SetFieldOrProperty("SF_Account_List", new List<SF_Account>
            {
                new SF_Account {
                    Id = "1",
                    Name = "test",
                    BillingCity = "test",
                    BillingStreet = "test",
                    BillingCountry = "test",
                    BillingState = "test",
                    BillingPostalCode = "test",
                    Fax = "test",
                    MailingStreet = "test",
                    MailingCity = "test",
                    MailingState = "test",
                    MailingPostalCode = "test",
                    MailingCountry = "test",
                    MobilePhone = "test",
                    Phone = "test",
                    FirstName = "test",
                    LastName = "test",
                    Email = "test"
                } });

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
            _testObject.SetFieldOrProperty("ECN_Customer_List", new List<Customer> { new Customer { CustomerName = "test", CustomerID = 1 } });
            _testObject.SetFieldOrProperty("SF_Account_List", new List<SF_Account> { new SF_Account { Id = "1", Name = "test" } });
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
            _testObject.SetFieldOrProperty("ECN_Customer_List", new List<Customer> { new Customer { CustomerID = 1 } });
            _testObject.SetFieldOrProperty("SF_Account_List", new List<SF_Account> { new SF_Account { Id = "1", OwnerId = "1" } });
            ShimCustomer.SaveCustomerUser = (p1, p2) => 1;
            ShimSF_Account.UpdateStringSF_Account = (p1, p2) => isECN;
            InitializeRBLs();
            SetRBLs(isECN ? "ECN" : "SF");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnSyncData_Click", new object[] { null, null }));
            (_testObject.GetFieldOrProperty("lblMessage") as Label).Text.ShouldBe(isECN ? "Update Successful" : "Update Failed");
        }

        [Test]
        public void ddlFilter_SelectedIndexChanged_OnlySF_Success([Values(true, false)]bool isVisible)
        {
            // Arrange
            InitializeDdlFilterTest(isVisible);
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            var lblECNAccounts = _testObject.GetField("lblECNAccounts") as Label;
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            var lblSFAccounts = _testObject.GetField("lblSFAccounts") as Label;
            var ddlFilter = _testObject.GetField("ddlFilter") as DropDownList;
            ddlFilter.SelectedValue = "OnlySF";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ddlFilter_SelectedIndexChanged", new object[] { null, null }));
            gvECNAccounts.Visible.ShouldBeFalse();
            lblECNAccounts.Visible.ShouldBeFalse();
            gvSFAccounts.Visible.ShouldBe(isVisible);
            lblSFAccounts.Visible.ShouldBe(isVisible);
        }

        [Test]
        public void ddlFilter_SelectedIndexChanged_OnlyECN_Success([Values(true, false)]bool isVisible)
        {
            // Arrange
            InitializeDdlFilterTest(isVisible);
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            var lblECNAccounts = _testObject.GetField("lblECNAccounts") as Label;
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            var lblSFAccounts = _testObject.GetField("lblSFAccounts") as Label;
            var ddlFilter = _testObject.GetField("ddlFilter") as DropDownList;
            ddlFilter.SelectedValue = "OnlyECN";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ddlFilter_SelectedIndexChanged", new object[] { null, null }));
            gvECNAccounts.Visible.ShouldBe(isVisible);
            lblECNAccounts.Visible.ShouldBe(isVisible);
            gvSFAccounts.Visible.ShouldBeFalse();
            lblSFAccounts.Visible.ShouldBeFalse();
        }

        [Test]
        public void ddlFilter_SelectedIndexChanged_DiffData_Success([Values(true, false)]bool isVisible)
        {
            // Arrange
            InitializeDdlFilterTest(isVisible);
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            var lblECNAccounts = _testObject.GetField("lblECNAccounts") as Label;
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            var lblSFAccounts = _testObject.GetField("lblSFAccounts") as Label;
            var ddlFilter = _testObject.GetField("ddlFilter") as DropDownList;
            ddlFilter.SelectedValue = "DiffData";
            if (isVisible)
            {
                gvSFAccounts.Rows[0].BackColor = KM_Colors.GreyDark;
                gvECNAccounts.Rows[0].BackColor = KM_Colors.GreyDark;
            }

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ddlFilter_SelectedIndexChanged", new object[] { null, null }));
            gvECNAccounts.Visible.ShouldBe(isVisible);
            lblECNAccounts.Visible.ShouldBe(isVisible);
            gvSFAccounts.Visible.ShouldBe(isVisible);
            lblSFAccounts.Visible.ShouldBe(isVisible);
        }

        [Test]
        public void ddlFilter_SelectedIndexChanged_All_Success()
        {
            // Arrange
            InitializeDdlFilterTest(true);
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            var lblECNAccounts = _testObject.GetField("lblECNAccounts") as Label;
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            var lblSFAccounts = _testObject.GetField("lblSFAccounts") as Label;
            var ddlFilter = _testObject.GetField("ddlFilter") as DropDownList;
            ddlFilter.SelectedValue = "All";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ddlFilter_SelectedIndexChanged", new object[] { null, null }));
            gvECNAccounts.Visible.ShouldBeTrue();
            lblECNAccounts.Visible.ShouldBeTrue();
            gvSFAccounts.Visible.ShouldBeTrue();
            lblSFAccounts.Visible.ShouldBeTrue();
        }

        [Test]
        public void LoadECN_Customers_Success()
        {
            // Arrange
            _testPage.Session["LoggedIn"] = true;
            ShimCustomer.GetByBaseChannelIDInt32 = (id) => new List<Customer> {
                new Customer { CustomerID = 1, CharityChannelID = 1 },
                new Customer { CustomerID = 2}
            };
            ShimUser.GetUsersByChannelIDInt32 = (id) => new List<User> {
                new User { IsActive = true, CustomerID = 1 },
                new User { CustomerID = 3}
            };
            ShimChannel.GetByBaseChannelIDInt32 = (id) => new List<Channel> { new Channel { ChannelID = 1 } };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("LoadECN_Customers", new object[] { }));
            var ECN_Customer_List = _testObject.GetFieldOrProperty("ECN_Customer_List") as List<Customer>;
            ECN_Customer_List.ShouldSatisfyAllConditions(
                () => ECN_Customer_List.ShouldNotBeNull(),
                () => ECN_Customer_List.Count.ShouldBe(1),
                () => ECN_Customer_List[0].CustomerID.ShouldBe(1));
        }

        [Test]
        public void LoadCustomers_Success()
        {
            // Arrange
            _testPage.Session["LoggedIn"] = true;
            ShimCustomer.GetByBaseChannelIDInt32 = (id) => new List<Customer> {
                new Customer { CustomerID = 1, CharityChannelID = 1 },
                new Customer { CustomerID = 2}
            };
            ShimUser.GetUsersByChannelIDInt32 = (id) => new List<User> {
                new User { IsActive = true, CustomerID = 1 },
                new User { CustomerID = 3}
            };
            ShimChannel.GetByBaseChannelIDInt32 = (id) => new List<Channel> { new Channel { ChannelID = 1 } };
            var ddlBaseChannel = _testObject.GetFieldOrProperty("ddlBaseChannel") as DropDownList;
            ddlBaseChannel.Items.Add("1");
            ddlBaseChannel.SelectedValue = "1";
            var ddlProductLine = _testObject.GetFieldOrProperty("ddlProductLine") as DropDownList;
            ddlProductLine.Items.Add("1");
            ddlProductLine.SelectedValue = "1";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("LoadCustomers", new object[] { }));
            var ECN_Customer_List = _testObject.GetFieldOrProperty("ECN_Customer_List") as List<Customer>;
            ECN_Customer_List.ShouldSatisfyAllConditions(
                () => ECN_Customer_List.ShouldNotBeNull(),
                () => ECN_Customer_List.Count.ShouldBe(1),
                () => ECN_Customer_List[0].CustomerID.ShouldBe(1));
        }

        [Test]
        [TestCase(0, true, 0)]
        [TestCase(1, false, 0)]
        [TestCase(2, false, 1)]
        public void cbECNselect_CheckedChanged_GreyDark_Success(int rowColor, bool isChecked, int ecnCount)
        {
            //Arrange
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            gvECNAccounts.DataKeyNames = new string[] { "CustomerID" };
            gvECNAccounts.DataSource = new List<Customer> { new Customer { CustomerID = 1 }, new Customer { CustomerID = 2 } };
            gvECNAccounts.DataBind();
            gvECNAccounts.Rows[0].BackColor = rowColor == 0 ? KM_Colors.GreyDark : rowColor == 1 ? KM_Colors.GreyLight : KM_Colors.BlueLight;
            var checkBox1 = gvECNAccounts.Rows[0].FindControl("cbECNselect") as CheckBox;
            var checkBox2 = gvECNAccounts.Rows[1].FindControl("cbECNselect") as CheckBox;
            checkBox1.Attributes["Color"] = "GreyDark";
            checkBox1.Checked = isChecked;
            _testObject.SetFieldOrProperty("ECN_SelectedCount", ecnCount);

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("cbECNselect_CheckedChanged", new object[] { checkBox1, null }));
            checkBox2.Enabled.ShouldBe(!isChecked);
        }

        [Test]
        public void cbECNselect_CheckedChanged_GreyLight_Success()
        {
            //Arrange
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            gvECNAccounts.DataKeyNames = new string[] { "CustomerID" };
            gvECNAccounts.DataSource = new List<Customer> { new Customer { CustomerID = 1 }, new Customer { CustomerID = 2 } };
            gvECNAccounts.DataBind();
            gvECNAccounts.Rows[0].BackColor = KM_Colors.GreyLight;
            var checkBox1 = gvECNAccounts.Rows[0].FindControl("cbECNselect") as CheckBox;
            var checkBox2 = gvECNAccounts.Rows[1].FindControl("cbECNselect") as CheckBox;
            checkBox1.Attributes["Color"] = "GreyLight";
            checkBox1.Checked = true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("cbECNselect_CheckedChanged", new object[] { checkBox1, null }));
        }

        [Test]
        public void cbECNselect_CheckedChanged_BlueLight_Success()
        {
            //Arrange
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            gvECNAccounts.DataKeyNames = new string[] { "CustomerID" };
            gvECNAccounts.DataSource = new List<Customer> { new Customer { CustomerID = 1 }, new Customer { CustomerID = 2 } };
            gvECNAccounts.DataBind();
            gvECNAccounts.Rows[0].BackColor = KM_Colors.BlueLight;
            var checkBox1 = gvECNAccounts.Rows[0].FindControl("cbECNselect") as CheckBox;
            var checkBox2 = gvECNAccounts.Rows[1].FindControl("cbECNselect") as CheckBox;
            checkBox1.Attributes["Color"] = "BlueLight";
            checkBox1.Checked = true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("cbECNselect_CheckedChanged", new object[] { checkBox1, null }));
        }

        [Test]
        [TestCase(0, true, 0)]
        [TestCase(1, false, 0)]
        [TestCase(2, false, 1)]
        public void cbSFselect_CheckedChanged_GreyDark_Success(int rowColor, bool isChecked, int ecnCount)
        {
            //Arrange
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            gvSFAccounts.DataKeyNames = new string[] { "Id" };
            gvSFAccounts.DataSource = new List<SF_Account> { new SF_Account { Id = "1" }, new SF_Account { Id = "2" } };
            gvSFAccounts.DataBind();
            gvSFAccounts.Rows[0].BackColor = rowColor == 0 ? KM_Colors.GreyDark : rowColor == 1 ? KM_Colors.GreyLight : KM_Colors.BlueLight;
            var checkBox1 = gvSFAccounts.Rows[0].FindControl("cbSFselect") as CheckBox;
            var checkBox2 = gvSFAccounts.Rows[1].FindControl("cbSFselect") as CheckBox;
            checkBox1.Attributes["Color"] = "GreyDark";
            checkBox1.Checked = isChecked;
            _testObject.SetFieldOrProperty("SF_SelectedCount", ecnCount);

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("cbSFselect_CheckedChanged", new object[] { checkBox1, null }));
            checkBox2.Enabled.ShouldBe(!isChecked);
        }

        [Test]
        public void cbSFselect_CheckedChanged_GreyLight_Success()
        {
            //Arrange
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            gvSFAccounts.DataKeyNames = new string[] { "Id" };
            gvSFAccounts.DataSource = new List<SF_Account> { new SF_Account { Id = "1" }, new SF_Account { Id = "2" } };
            gvSFAccounts.DataBind();
            gvSFAccounts.Rows[0].BackColor = KM_Colors.GreyLight;
            var checkBox1 = gvSFAccounts.Rows[0].FindControl("cbSFselect") as CheckBox;
            var checkBox2 = gvSFAccounts.Rows[1].FindControl("cbSFselect") as CheckBox;
            checkBox1.Attributes["Color"] = "GreyLight";
            checkBox1.Checked = true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("cbSFselect_CheckedChanged", new object[] { checkBox1, null }));
        }

        [Test]
        public void cbSFselect_CheckedChanged_BlueLight_Success()
        {
            //Arrange
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            gvSFAccounts.DataKeyNames = new string[] { "Id" };
            gvSFAccounts.DataSource = new List<SF_Account> { new SF_Account { Id = "1" }, new SF_Account { Id = "2" } };
            gvSFAccounts.DataBind();
            gvSFAccounts.Rows[0].BackColor = KM_Colors.BlueLight;
            var checkBox1 = gvSFAccounts.Rows[0].FindControl("cbSFselect") as CheckBox;
            var checkBox2 = gvSFAccounts.Rows[1].FindControl("cbSFselect") as CheckBox;
            checkBox1.Attributes["Color"] = "BlueLight";
            checkBox1.Checked = true;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("cbSFselect_CheckedChanged", new object[] { checkBox1, null }));
        }

        [Test]
        public void cbECNselectALL_CheckedChanged_Success([Values(true, false)]bool isChecked)
        {
            //Arrange
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            gvECNAccounts.DataKeyNames = new string[] { "CustomerID" };
            gvECNAccounts.DataSource = new List<Customer> { new Customer { CustomerID = 1 }, new Customer { CustomerID = 2 } };
            gvECNAccounts.DataBind();
            var checkBox = gvECNAccounts.HeaderRow.FindControl("cbECNselectALL") as CheckBox;
            var checkBox1 = gvECNAccounts.Rows[0].FindControl("cbECNselect") as CheckBox;
            var checkBox2 = gvECNAccounts.Rows[1].FindControl("cbECNselect") as CheckBox;
            var imgbtnECNToSF = _testObject.GetFieldOrProperty("imgbtnECNToSF") as ImageButton;
            checkBox.Checked = isChecked;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("cbECNselectALL_CheckedChanged", new object[] { checkBox, null }));
            checkBox1.Checked.ShouldBe(isChecked);
            checkBox2.Checked.ShouldBe(isChecked);
            imgbtnECNToSF.Visible.ShouldBe(isChecked);
        }

        [Test]
        public void cbSFselectALL_CheckedChanged_Success([Values(true, false)]bool isChecked)
        {
            //Arrange
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            gvSFAccounts.DataKeyNames = new string[] { "Id" };
            gvSFAccounts.DataSource = new List<SF_Account> { new SF_Account { Id = "1" }, new SF_Account { Id = "2" } };
            gvSFAccounts.DataBind();
            var checkBox = gvSFAccounts.HeaderRow.FindControl("cbSFselectALL") as CheckBox;
            var checkBox1 = gvSFAccounts.Rows[0].FindControl("cbSFselect") as CheckBox;
            var checkBox2 = gvSFAccounts.Rows[1].FindControl("cbSFselect") as CheckBox;
            checkBox.Checked = isChecked;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("cbSFselectALL_CheckedChanged", new object[] { checkBox, null }));
            checkBox1.Checked.ShouldBe(isChecked);
            checkBox2.Checked.ShouldBe(isChecked);
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void gvSFAccounts_Sorting_Success(bool isAscending, bool isDefault)
        {
            //Arrange
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            gvSFAccounts.DataKeyNames = new string[] { "Name" };
            _testObject.SetFieldOrProperty("SF_Account_List",
                new List<SF_Account> { new SF_Account { Name = "2" }, new SF_Account { Name = "3" }, new SF_Account { Name = "1" } });
            var args = new GridViewSortEventArgs("Name", System.Web.UI.WebControls.SortDirection.Ascending);
            _testObject.SetFieldOrProperty("SFSortExp", !isDefault ? "Name" : string.Empty);
            _testObject.SetFieldOrProperty("SFSortDir", !isAscending ?
                System.Web.UI.WebControls.SortDirection.Ascending : System.Web.UI.WebControls.SortDirection.Descending);

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("gvSFAccounts_Sorting", new object[] { gvSFAccounts, args }));
            gvSFAccounts.ShouldSatisfyAllConditions(
                () => gvSFAccounts.ShouldNotBeNull(),
                () => (gvSFAccounts.DataKeys[0].Value).ShouldBe((isDefault || isAscending) ? "1" : "3"),
                () => (gvSFAccounts.DataKeys[1].Value).ShouldBe("2"),
                () => (gvSFAccounts.DataKeys[2].Value).ShouldBe((isDefault || isAscending) ? "3" : "1"));
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void gvECNAccounts_Sorting_Success(bool isAscending, bool isDefault)
        {
            //Arrange
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            gvECNAccounts.DataKeyNames = new string[] { "CustomerName" };
            _testObject.SetFieldOrProperty("ECN_Customer_List",
                new List<Customer> { new Customer { CustomerName = "2" }, new Customer { CustomerName = "3" }, new Customer { CustomerName = "1" } });
            var args = new GridViewSortEventArgs("CustomerName", System.Web.UI.WebControls.SortDirection.Ascending);
            _testObject.SetFieldOrProperty("ECNSortExp", !isDefault ? "CustomerName" : string.Empty);
            _testObject.SetFieldOrProperty("ECNSortDir", !isAscending ?
                System.Web.UI.WebControls.SortDirection.Ascending : System.Web.UI.WebControls.SortDirection.Descending);

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("gvECNAccounts_Sorting", new object[] { gvECNAccounts, args }));
            gvECNAccounts.ShouldSatisfyAllConditions(
                () => gvECNAccounts.ShouldNotBeNull(),
                () => (gvECNAccounts.DataKeys[0].Value).ShouldBe((isDefault || isAscending) ? "1" : "3"),
                () => (gvECNAccounts.DataKeys[1].Value).ShouldBe("2"),
                () => (gvECNAccounts.DataKeys[2].Value).ShouldBe((isDefault || isAscending) ? "3" : "1"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void cbSFselect_CheckedChanged_OnCheckColorGreyDark_AllSFCheckboxesDisabled(bool isChecked)
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
        public void cbSFselect_CheckedChanged_SFCountShouldBeGreatOrEqualZero(bool isChecked, int initialCount, int expectedCount)
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
        public void cbSFselect_CheckedChanged_OnUnCheck_ControlsAreHide(string method)
        {
            // Arrange
            var sender = new CheckBox() { Checked = false };
            SetColor(sender, string.Empty);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            imgbtnCompare.Visible = false;
            imgbtnEcntoSf.Visible = false;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcntoSf.Visible.ShouldBeFalse();
        }

        [Test]
        public void cbSFselect_CheckedChanged_OnCheckAndColorGreyLight_ControlsAreHide()
        {
            // Arrange
            var sender = new CheckBox() { Checked = true };
            SetColor(sender, GreyLightColor);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            imgbtnCompare.Visible = true;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
        }

        [Test]
        public void cbSFselect_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorGreyDark_VerifyControls()
        {
            // Arrange
            var sfGrid = SetupSFContactGrid(SfGridId);
            var sender = sfGrid.Rows[0].FindControl(SfCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.GreyDark);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            imgbtnCompare.Visible = false;
            imgbtnCompare.CommandArgument = null;
            imgbtnCompare.CommandName = null;
            imgbtnEcntoSf.Visible = true;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            imgbtnCompare.ShouldSatisfyAllConditions(
                 () => imgbtnCompare.Visible.ShouldBeTrue(),
                 () => imgbtnCompare.CommandArgument.ShouldBe(CommandArgument),
                 () => imgbtnCompare.CommandName.ShouldBe(SfCommandName));

            imgbtnEcntoSf.Visible.ShouldBeFalse();
        }

        [Test]
        public void cbSFselect_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorGreyLight_VerifyControls()
        {
            // Arrange
            var sfGrid = SetupSFContactGrid(SfGridId);
            var sender = sfGrid.Rows[0].FindControl(SfCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.GreyLight);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcntoSf.Visible = true;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcntoSf.Visible.ShouldBeFalse();
        }

        [Test]
        public void cbSFselect_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorBlueLight_VerifyControls()
        {
            // Arrange
            var sfGrid = SetupSFContactGrid(SfGridId);
            var sender = sfGrid.Rows[0].FindControl(SfCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.BlueLight);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcntoSf.Visible = true;

            // Act
            ExecuteOnCheck(sender, SfCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcntoSf.Visible.ShouldBeFalse();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void cbECNselect_CheckedChanged_OnCheckColorGreyDark_AllEcnCheckboxesDisabled(bool isChecked)
        {
            // Arrange 
            var sfGrid = SetupSFContactGrid(SfGridId);
            var allSfCheckboxes = GetAllCheckBoxes(sfGrid, SfCheckboxId, SfHeaderCheckboxId).ToArray();
            var parentGrid = SetupEcnContactGrid(EcnGridId);
            var sender = parentGrid.Rows[0].FindControl(EcnCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = isChecked;
            var secondCheckbox = parentGrid.Rows[1].FindControl(EcnCheckboxId) as CheckBox;
            var expectedColor = isChecked ? Color.Gray : Color.Black;

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
        public void cbECNselect_CheckedChanged_ECNCountShouldBeGreatOrEqualZero(bool isChecked, int initialCount, int expectedCount)
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
        public void cbECNselect_CheckedChanged_OnCheckAndColorGreyLight_ControlsAreHide()
        {
            // Arrange
            var sender = new CheckBox() { Checked = true };
            SetColor(sender, GreyLightColor);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcntoSf.Visible = false;

            // Act
            ExecuteOnCheck(sender, EcnCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcntoSf.Visible.ShouldBeTrue();
        }

        [Test]
        public void cbECNselect_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorGreyDark_VerifyControls()
        {
            // Arrange
            var grid = SetupEcnContactGrid(EcnGridId);
            var sender = grid.Rows[0].FindControl(EcnCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.GreyDark);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            imgbtnCompare.Visible = false;
            imgbtnCompare.CommandArgument = null;
            imgbtnCompare.CommandName = null;
            imgbtnEcntoSf.Visible = true;

            // Act
            ExecuteOnCheck(sender, EcnCheckedMethodName);

            // Assert
            imgbtnCompare.ShouldSatisfyAllConditions(
                 () => imgbtnCompare.Visible.ShouldBeTrue(),
                 () => imgbtnCompare.CommandArgument.ShouldBe(CommandArgument),
                 () => imgbtnCompare.CommandName.ShouldBe(EcnCommandName));

            imgbtnEcntoSf.Visible.ShouldBeFalse();
        }

        [Test]
        public void cbECNselect_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorGreyLight_VerifyControls()
        {
            // Arrange
            var grid = SetupEcnContactGrid(EcnGridId);
            var sender = grid.Rows[0].FindControl(EcnCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.GreyLight);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcntoSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcntoSf.Visible = true;

            // Act
            ExecuteOnCheck(sender, EcnCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcntoSf.ShouldSatisfyAllConditions(
                () => imgbtnEcntoSf.Visible.ShouldBeTrue(),
                () => imgbtnEcntoSf.CommandArgument = CommandArgument);
        }

        [Test]
        public void cbECNselect_CheckedChanged_OnCheckAndColoGreyDarkAndRowColorBlueLight_VerifyControls()
        {
            // Arrange
            var grid = SetupEcnContactGrid(EcnGridId);
            var sender = grid.Rows[0].FindControl(EcnCheckboxId) as CheckBox;
            SetColor(sender, GreyDarkColor);
            sender.Checked = true;
            SetParentRowColor(sender, KM_Colors.BlueLight);
            var imgbtnCompare = GetProperty<ImageButton>(ImgbtnCompareProperty);
            var imgbtnEcnToSf = GetProperty<ImageButton>(ImgbtnEcntoSfProperty);
            imgbtnCompare.Visible = true;
            imgbtnEcnToSf.Visible = true;

            // Act
            ExecuteOnCheck(sender, EcnCheckedMethodName);

            // Assert
            imgbtnCompare.Visible.ShouldBeFalse();
            imgbtnEcnToSf.Visible.ShouldBeFalse();
        }

        protected GridView SetupEcnContactGrid(string id)
        {
            var grid = _testObject.GetField(id) as GridView;
            grid.DataKeyNames = new string[] { "CustomerID" };
            grid.DataSource = new List<Customer> { new Customer { CustomerID = 1 }, new Customer { CustomerID = 2 } };
            grid.DataBind();
            return grid;
        }

        protected GridView SetupSFContactGrid(string id)
        {
            var grid = _testObject.GetField(id) as GridView;
            grid.DataKeyNames = new string[] { "Id" };
            grid.DataSource = new List<SF_Account> { new SF_Account { Id = "1" }, new SF_Account { Id = "2" } };
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
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            gvSFAccounts.DataSource = new List<SF_Account> { new SF_Account { OwnerId = "1", Name = "dummy" } };
            gvSFAccounts.DataBind();
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            gvECNAccounts.DataSource = new List<Customer> { new Customer { CustomerName = "dummy" } };
            gvECNAccounts.DataBind();
            if (!visible)
            {
                gvSFAccounts.Rows[0].BackColor = KM_Colors.BlueLight;
                gvECNAccounts.Rows[0].BackColor = KM_Colors.BlueLight;
            }
            else
            {
                gvSFAccounts.Rows[0].BackColor = KM_Colors.GreyLight;
                gvECNAccounts.Rows[0].BackColor = KM_Colors.GreyLight;
            }
        }

        private void SetRBLs(string value)
        {
            (_testObject.GetFieldOrProperty("rblName") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblAddress") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblCity") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblState") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblZip") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblCountry") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblPhone") as RadioButtonList).SelectedValue = value;
            (_testObject.GetFieldOrProperty("rblFax") as RadioButtonList).SelectedValue = value;
        }

        private void InitializeRBLs()
        {
            FillRBLValues("rblName", new List<string> { "SF", "ECN" });
            FillRBLValues("rblAddress", new List<string> { "SF", "ECN" });
            FillRBLValues("rblCity", new List<string> { "SF", "ECN" });
            FillRBLValues("rblState", new List<string> { "SF", "ECN" });
            FillRBLValues("rblZip", new List<string> { "SF", "ECN" });
            FillRBLValues("rblCountry", new List<string> { "SF", "ECN" });
            FillRBLValues("rblPhone", new List<string> { "SF", "ECN" });
            FillRBLValues("rblFax", new List<string> { "SF", "ECN" });
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
            _testPage = new SF_Accounts();
            InitializeAllControls(_testPage);
            _testObject = new PrivateObject(_testPage);
            _testObject.SetFieldOrProperty("ECN_Customer_List", new List<Customer> { });
            _testObject.SetFieldOrProperty("SF_Account_List", new List<SF_Account> { });
            var kmSearch = _testObject.GetField("kmSearch");
            InitializeAllControls(kmSearch);
            InitializeGrids();
        }

        private void InitializeGrids()
        {
            var gvSFAccounts = _testObject.GetField("gvSFAccounts") as GridView;
            var cbSFselectALL = new CheckBox { ID = "cbSFselectALL" };
            var cbSFselect = new CheckBox { ID = "cbSFselect" };
            gvSFAccounts.Columns.Add(new TemplateField
            {
                HeaderTemplate = new TestTemplateItem { control = cbSFselectALL },
                ItemTemplate = new TestTemplateItem { control = cbSFselect }
            });
            gvSFAccounts.Columns.Add(new BoundField { DataField = "Name", SortExpression = "Name" });
            gvSFAccounts.Columns.Add(new BoundField { DataField = "BillingCity", SortExpression = "BillingCity" });
            gvSFAccounts.Columns.Add(new BoundField { DataField = "BillingState", SortExpression = "BillingState" });
            gvSFAccounts.Columns.Add(new BoundField { DataField = "BillingPostalCode", SortExpression = "BillingPostalCode" });
            gvSFAccounts.Columns.Add(new BoundField { DataField = "Industry", SortExpression = "Industry" });
            gvSFAccounts.Columns.Add(new BoundField { DataField = "AnnualRevenue", SortExpression = "AnnualRevenue" });
            gvSFAccounts.RowDataBound += (s, e) => _testObject.Invoke("gvSFAccounts_RowDataBound", new object[] { s, e });
            gvSFAccounts.Sorting += (s, e) => _testObject.Invoke("gvSFAccounts_Sorting", new object[] { s, e });
            var gvECNAccounts = _testObject.GetField("gvECNAccounts") as GridView;
            var cbECNselectALL = new CheckBox { ID = "cbECNselectALL" };
            var cbECNselect = new CheckBox { ID = "cbECNselect" };
            gvECNAccounts.Columns.Add(new TemplateField
            {
                HeaderTemplate = new TestTemplateItem { control = cbECNselectALL },
                ItemTemplate = new TestTemplateItem { control = cbECNselect }
            });
            gvECNAccounts.Columns.Add(new BoundField { DataField = "CustomerName", SortExpression = "CustomerName" });
            gvECNAccounts.Columns.Add(new BoundField { DataField = "City", SortExpression = "City" });
            gvECNAccounts.Columns.Add(new BoundField { DataField = "State", SortExpression = "State" });
            gvECNAccounts.Columns.Add(new BoundField { DataField = "Zip", SortExpression = "Zip" });
            gvECNAccounts.RowDataBound += (s, e) => _testObject.Invoke("gvECNAccounts_RowDataBound", new object[] { s, e });
            gvECNAccounts.Sorting += (s, e) => _testObject.Invoke("gvECNAccounts_Sorting", new object[] { s, e });
        }

        private void InitializeFakes()
        {
            ShimFolder.GetByCustomerIDInt32User = (id, user) => new List<Folder> { };
            ShimGroup.GetByCustomerIDInt32UserString = (id, user, p) => new List<Group> { };
            ShimSF_User.GetAllString = (token) => new List<SF_User> { };
            ShimSF_Account.GetAllString = (token) => new List<SF_Account> { };
            ShimBaseChannel.GetAll = () => new List<BaseChannel> { new BaseChannel { BaseChannelID = 1 } };
            ShimChannel.GetByBaseChannelIDInt32 = (id) => new List<Channel> { };
            ShimCustomer.GetByBaseChannelIDInt32 = (id) => new List<Customer> { };
            ShimUser.GetUsersByChannelIDInt32 = (id) => new List<User> { };
            ShimSF_TagDefinition.GetAllString = (token) => new List<SF_TagDefinition> { };
            ShimGroup.ExistsInt32StringInt32Int32 = (p1, p2, p3, p4) => false;
            ShimGroup.SaveGroupUser = (p1, p2) => 1;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { ShortName = "sfid" }, new GroupDataFields { ShortName = "sftype" } };
            ShimEmail.GetByGroupIDInt32User = (p1, p2) => new List<Email> { new Email { } };
            ShimGroup.GetByGroupIDInt32User = (p1, p2) => new Group { };
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) =>
                new DataTable { Columns = { "Action", "Counts" }, Rows = { { "1", "1" } } };
            ShimContact.GetByCustomerIDInt32User = (p1, p2) => new Contact { };
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
                session.CurrentCustomer = new Customer();
                session.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
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

        private Customer CreateCustomer(int id, string email)
        {
            return new Customer()
            {
                CustomerID = id,
                Address = "Address",
                Country = "Country",
                Phone = "Phone",
                City = "City",
                Email = email,
                CustomerName = "CustomerName",
                State = "State",
                Zip = "Zip",
                Fax = "Fax",
            };
        }

        private SF_Account CreateAccount(Customer customer)
        {
            return new SF_Account
            {
                BillingStreet = customer.Address,
                BillingCity = customer.City,
                BillingCountry = customer.Country,
                BillingState = customer.State,
                BillingPostalCode = customer.Zip,
                Fax = customer.Fax,
                Phone = customer.Phone,
                Name = customer.CustomerName
            };
        }

        private IEnumerable<System.Action> VerifyEcnLabelsAndHiddenFields(Customer expectedEmail, string accountId)
        {
            yield return () => GetProperty<HiddenField>("hfECNCustomerID").Value.ShouldBe(expectedEmail.CustomerID.ToString());
            yield return () => GetProperty<HiddenField>("hfSFAccountID").Value.ShouldBe(accountId);
            yield return () => GetLabelText("lblECNAddress").ShouldBe(expectedEmail.Address);
            yield return () => GetLabelText("lblECNCity").ShouldBe(expectedEmail.City);
            yield return () => GetLabelText("lblECNCountry").ShouldBe(expectedEmail.Country);
            yield return () => GetLabelText("lblECNFax").ShouldBe(expectedEmail.Fax);
            yield return () => GetLabelText("lblECNName").ShouldBe(expectedEmail.CustomerName);
            yield return () => GetLabelText("lblECNPhone").ShouldBe(expectedEmail.Phone);
            yield return () => GetLabelText("lblECNState").ShouldBe(expectedEmail.State);
            yield return () => GetLabelText("lblECNZip").ShouldBe(expectedEmail.Zip);
        }

        private void SetupListValues(Customer customer, SF_Account account)
        {
            _testObject.SetFieldOrProperty(ECN_Customer_ListPropertyName, new List<Customer> { customer });
            _testObject.SetFieldOrProperty(SF_Account_ListPropertyName, new List<SF_Account> { account });
        }
    }
}
