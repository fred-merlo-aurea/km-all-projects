using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.UI.WebControls;
using ecn.communicator.main.admin.landingpages;
using Ecn.Communicator.Main.Admin.Interfaces;
using Ecn.Communicator.Main.Interfaces;
using ECN.Tests.Helpers;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Admin.LandingPages
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CustomerForwardToFriendTest
    {
        private const string InvalidCodeSnippet = "%%";
        private const int DummyUserID = -1;
        private const int DummyCustomerID = -2;
        private const string Yes = "Yes";
        private const string NonEmptyText = "Non Empty Text";
        private const string BlastId = "1";
        private const string EmailId = "1";
        private const int LandingPageID = 3;
        
        private Mock<HttpResponseBase> _response;
        private CustomerForwardToFriend _customer;

        [SetUp]
        public void SetUp()
        {
            _customer = CreateCustomer();
        }

        [Test]
        public void BtnSaveClick_NullLandingPageAssign_UpdateControlsAndRedirectToCustomerMainPage()
        {
            // Arrange
            InitializeControls(NonEmptyText, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_customer, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "LPA").GetValue(_customer) as LandingPageAssign;
            landingPageAssign.ShouldNotBeNull();
            landingPageAssign.LPID.ShouldBe(LandingPageID);
            landingPageAssign.CreatedUserID.ShouldBe(DummyUserID);
            landingPageAssign.CustomerID.ShouldBe(DummyCustomerID);
            landingPageAssign.CustomerDoesOverride.Value.ShouldNotBeNull();
            landingPageAssign.CustomerDoesOverride.Value.ShouldBeFalse();
            landingPageAssign.Header.ShouldBe(NonEmptyText);
            landingPageAssign.Footer.ShouldBe(NonEmptyText);
            landingPageAssign.UpdatedUserID.ShouldBe(DummyUserID);
            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "btnPreview").GetValue(_customer) as Button;
            previewButton.ShouldNotBeNull();
            previewButton.Enabled.ShouldBeTrue();
            previewButton.Visible.ShouldBeTrue();
            previewButton.Attributes.ShouldNotBeNull();
            previewButton.Attributes.Count.ShouldBe(1);
            previewButton.Attributes["onclick"].ShouldBe(
                string.Format(
                    "window.open('{0}/engines/emailtofriend.aspx?e=1&b=1&preview=-1', 'popup_window', 'width=1000,height=750,resizable=yes');",
                    ConfigurationManager.AppSettings["Activity_DomainPath"]));

            var sentBlastsLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "lblSentBlastsWarning").GetValue(_customer) as Label;
            sentBlastsLabel.ShouldNotBeNull();
            sentBlastsLabel.Text.ShouldBe(NonEmptyText);

            var customOverrideWarning = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "lblCustomerOverrideWarning").GetValue(_customer) as Label;
            customOverrideWarning.ShouldNotBeNull();
            customOverrideWarning.Text.ShouldBe("Note: The above settings will not be visible to customers until you override the Basechannel settings.");

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "lblErrorMessage").GetValue(_customer) as Label;
            errorMessageLabel.ShouldNotBeNull();
            errorMessageLabel.Text.ShouldBe(NonEmptyText);

            _response.Verify(x => x.Redirect("CustomerMain.aspx"), Times.Once());
        }

        [Test]
        public void BtnSaveClick_InvalidCodeSnippetsForHeaderText_UpdateControlsAndRedirectToCustomerMainPage()
        {
            // Arrange
            InitializeControls(InvalidCodeSnippet, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_customer, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "LPA").GetValue(_customer) as LandingPageAssign;
            landingPageAssign.ShouldNotBeNull();
            landingPageAssign.LPID.ShouldBe(LandingPageID);
            landingPageAssign.CustomerDoesOverride.HasValue.ShouldBeTrue();
            landingPageAssign.CustomerDoesOverride.Value.ShouldBeFalse();
            landingPageAssign.Header.ShouldBeEmpty();
            landingPageAssign.Footer.ShouldBeEmpty();
            landingPageAssign.UpdatedUserID.ShouldBeNull();

            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "btnPreview").GetValue(_customer) as Button;
            previewButton.ShouldNotBeNull();
            previewButton.Enabled.ShouldBeFalse();
            previewButton.Visible.ShouldBeFalse();
            previewButton.CssClass.ShouldBeEmpty();
            previewButton.Attributes.ShouldNotBeNull();
            previewButton.Attributes.Count.ShouldBe(0);

            var sentBlastsLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "lblSentBlastsWarning").GetValue(_customer) as Label;
            sentBlastsLabel.ShouldNotBeNull();
            sentBlastsLabel.Text.ShouldBe(NonEmptyText);

            var customOverrideWarning = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "lblCustomerOverrideWarning").GetValue(_customer) as Label;
            customOverrideWarning.ShouldNotBeNull();
            customOverrideWarning.Text.ShouldBe(NonEmptyText);

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "lblErrorMessage").GetValue(_customer) as Label;
            errorMessageLabel.ShouldNotBeNull();
            errorMessageLabel.Text.ShouldBe("<br/>LandingPage: There is a badly formed codesnippet in Header");

            _response.Verify(x => x.Redirect("CustomerMain.aspx"), Times.Never());
        }

        [Test]
        public void BtnSaveClick_InvalidCodeSnippetsForFooterText_UpdateControlsAndRedirectToCustomerMainPage()
        {
            // Arrange
            InitializeControls(NonEmptyText, InvalidCodeSnippet);

            // Act
            ReflectionHelper.ExecuteMethod(_customer, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "LPA").GetValue(_customer) as LandingPageAssign;
            landingPageAssign.ShouldNotBeNull();
            landingPageAssign.LPID.ShouldBe(LandingPageID);
            landingPageAssign.CustomerDoesOverride.HasValue.ShouldBeTrue();
            landingPageAssign.CustomerDoesOverride.Value.ShouldBeFalse();
            landingPageAssign.Header.ShouldBeEmpty();
            landingPageAssign.Footer.ShouldBeEmpty();
            landingPageAssign.UpdatedUserID.ShouldBeNull();

            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "btnPreview").GetValue(_customer) as Button;
            previewButton.ShouldNotBeNull();
            previewButton.Enabled.ShouldBeFalse();
            previewButton.Visible.ShouldBeFalse();
            previewButton.CssClass.ShouldBeEmpty();
            previewButton.Attributes.ShouldNotBeNull();
            previewButton.Attributes.Count.ShouldBe(0);

            var sentBlastsLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "lblSentBlastsWarning").GetValue(_customer) as Label;
            sentBlastsLabel.ShouldNotBeNull();
            sentBlastsLabel.Text.ShouldBe(NonEmptyText);

            var customOverrideWarning = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "lblCustomerOverrideWarning").GetValue(_customer) as Label;
            customOverrideWarning.ShouldNotBeNull();
            customOverrideWarning.Text.ShouldBe(NonEmptyText);

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, "lblErrorMessage").GetValue(_customer) as Label;
            errorMessageLabel.ShouldNotBeNull();
            errorMessageLabel.Text.ShouldBe("<br/>LandingPage: There is a badly formed codesnippet in Footer");

            _response.Verify(x => x.Redirect("CustomerMain.aspx"), Times.Never());
        }

        private void InitializeControls(string headerText, string footerText)
        {
            ReflectionHelper.SetValue(_customer, "phError", new PlaceHolder() { Visible = true });
            ReflectionHelper.SetValue(_customer, "rblBasechannelOverride", new RadioButtonList() { SelectedValue = string.Empty });
            ReflectionHelper.SetValue(_customer, "txtHeader", new TextBox() { Text = headerText });
            ReflectionHelper.SetValue(_customer, "txtFooter", new TextBox() { Text = footerText });
            ReflectionHelper.SetValue(_customer, "lblCustomerOverrideWarning", new Label() { Text = NonEmptyText });
            ReflectionHelper.SetValue(_customer, "btnPreview", new Button() { Enabled = false, Visible = false, CssClass = string.Empty });
            ReflectionHelper.SetValue(_customer, "lblSentBlastsWarning", new Label() { Text = NonEmptyText });
            ReflectionHelper.SetValue(_customer, "lblErrorMessage", new Label() { Text = NonEmptyText });
        }

        private CustomerForwardToFriend CreateCustomer()
        {
            var master = new Mock<IMasterCommunicator>();
            master.Setup(x => x.GetUserID()).Returns(DummyUserID);
            master.Setup(x => x.GetCustomerID()).Returns(DummyCustomerID);
            master.Setup(x => x.GetCurrentUser()).Returns(new User());

            var landingPageAssign = new Mock<ILandingPageAssign>();
            landingPageAssign.Setup(x => x.Save(It.IsAny<LandingPageAssign>(), It.IsAny<User>()));
            var dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn());
            dataTable.Columns.Add(new DataColumn());
            var dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
            {
                BlastId,
                EmailId
            };
            dataTable.Rows.Add(dataRow);
            landingPageAssign.Setup(x => x.GetPreviewParameters(It.IsAny<int>(), It.IsAny<int>())).Returns(dataTable);

            var landingPageAssignContent = new Mock<ILandingPageAssignContent>();
            landingPageAssignContent.Setup(x => x.Save(It.IsAny<LandingPageAssignContent>(), It.IsAny<User>()));
            landingPageAssignContent.Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<User>()));

            _response = new Mock<HttpResponseBase>();
            _response.Setup(x => x.Redirect("CustomerMain.aspx"));

            return new CustomerForwardToFriend(master.Object, landingPageAssign.Object, landingPageAssignContent.Object, _response.Object);
        }
    }
}
