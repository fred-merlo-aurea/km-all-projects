using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.UI.WebControls;
using Ecn.Communicator.Main.Admin.Interfaces;
using ecn.communicator.main.admin.landingpages;
using Ecn.Communicator.Main.Interfaces;
using ECN.Tests.Helpers;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Admin.LandingPages
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CustomerAbuseTest
    {
        private const int DummyUserID = -1;
        private const int DummyCustomerID = -2;
        private const string Yes = "Yes";
        private const string NonEmptyText = "Non Empty Text";
        private const string ECNButtonMedium = "ECN-Button-Medium";
        private const string InvalidCodeSnippet = "%%";
        private const string TextContainsThankYou = "Text Contains Thank You";
        private const string FieldLandingPageAssign = "LPA";
        private const string PreviewButtonName = "btnPreview";
        private const string ActivityDomainPathKey = "Activity_DomainPath";
        private const int LandingPageID = 4;

        private Mock<HttpResponseBase> _response;
        
        [Test]
        public void BtnSaveClick_NullLandingPageAssign_UpdateControlsAndRedirectToCustomerMainPage()
        {
            // Arrange
            var customerAbuse = CreateCustomerAbuse();
            InitializeControls(customerAbuse, NonEmptyText, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(customerAbuse, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "LPA").GetValue(customerAbuse) as LandingPageAssign;
            Assert.That(landingPageAssign, Is.Not.Null);
            Assert.That(landingPageAssign.LPID, Is.EqualTo(4));
            Assert.That(landingPageAssign.CustomerDoesOverride, Is.False);
            Assert.That(landingPageAssign.Header, Is.EqualTo(NonEmptyText));
            Assert.That(landingPageAssign.Footer, Is.EqualTo(NonEmptyText));
            Assert.That(landingPageAssign.UpdatedUserID, Is.EqualTo(DummyUserID));

            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "btnPreview").GetValue(customerAbuse) as Button;
            Assert.That(previewButton, Is.Not.Null);
            Assert.That(previewButton.Enabled, Is.True);
            Assert.That(previewButton.Visible, Is.True);
            Assert.That(previewButton.CssClass, Is.EqualTo(ECNButtonMedium));
            Assert.That(previewButton.Attributes, Is.Not.Null);
            Assert.That(previewButton.Attributes.Count, Is.EqualTo(1));
            Assert.That(previewButton.Attributes["onclick"]
                , Is.EqualTo(string.Format(
                    "window.open('{0}/engines/reportSpam.aspx?p=&preview=-1', 'popup_window', 'width=1000,height=750,resizable=yes');",
                    ConfigurationManager.AppSettings["Activity_DomainPath"])));

            var sentBlastsLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "lblSentBlastsWarning").GetValue(customerAbuse) as Label;
            Assert.That(sentBlastsLabel, Is.Not.Null);
            Assert.That(sentBlastsLabel.Text, Is.EqualTo(NonEmptyText));

            var customOverrideWarning = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "lblCustomerOverrideWarning").GetValue(customerAbuse) as Label;
            Assert.That(customOverrideWarning, Is.Not.Null);
            Assert.That(customOverrideWarning.Text, Is.EqualTo("Note: The above settings will not be visible to customers until you override the Basechannel settings."));

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "lblErrorMessage").GetValue(customerAbuse) as Label;
            Assert.That(customOverrideWarning, Is.Not.Null);
            Assert.That(errorMessageLabel.Text, Is.EqualTo(NonEmptyText));

            _response.Verify(x => x.Redirect("CustomerMain.aspx"), Times.Once());
        }

        [Test]
        public void BtnSaveClick_InvalidCodeSnippetsForHeaderText_UpdateControlsAndRedirectToCustomerMainPage()
        {
            // Arrange
            var customerAbuse = CreateCustomerAbuse();
            InitializeControls(customerAbuse, InvalidCodeSnippet, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(customerAbuse, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "LPA").GetValue(customerAbuse) as LandingPageAssign;
            Assert.That(landingPageAssign, Is.Not.Null);
            Assert.That(landingPageAssign.LPID, Is.EqualTo(4));
            Assert.That(landingPageAssign.CustomerDoesOverride, Is.False);
            Assert.That(landingPageAssign.Header, Is.EqualTo(string.Empty));
            Assert.That(landingPageAssign.Footer, Is.EqualTo(string.Empty));
            Assert.That(landingPageAssign.UpdatedUserID, Is.Null);

            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "btnPreview").GetValue(customerAbuse) as Button;
            Assert.That(previewButton, Is.Not.Null);
            Assert.That(previewButton.Enabled, Is.False);
            Assert.That(previewButton.Visible, Is.False);
            Assert.That(previewButton.CssClass, Is.EqualTo(string.Empty));
            Assert.That(previewButton.Attributes.Count, Is.Zero);

            var sentBlastsLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "lblSentBlastsWarning").GetValue(customerAbuse) as Label;
            Assert.That(sentBlastsLabel, Is.Not.Null);
            Assert.That(sentBlastsLabel.Text, Is.EqualTo(NonEmptyText));

            var customOverrideWarning = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "lblCustomerOverrideWarning").GetValue(customerAbuse) as Label;
            Assert.That(customOverrideWarning, Is.Not.Null);
            Assert.That(customOverrideWarning.Text, Is.EqualTo(NonEmptyText));

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "lblErrorMessage").GetValue(customerAbuse) as Label;
            Assert.That(errorMessageLabel, Is.Not.Null);
            Assert.That(errorMessageLabel.Text, Is.EqualTo("<br/>LandingPage: There is a badly formed codesnippet in Header"));

            _response.Verify(x => x.Redirect("CustomerMain.aspx"), Times.Never());
        }

        [Test]
        public void BtnSaveClick_InvalidCodeSnippetsForFooterText_UpdateControlsAndRedirectToCustomerMainPage()
        {
            // Arrange
            var customerAbuse = CreateCustomerAbuse();
            InitializeControls(customerAbuse, NonEmptyText, InvalidCodeSnippet);

            // Act
            ReflectionHelper.ExecuteMethod(customerAbuse, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "LPA").GetValue(customerAbuse) as LandingPageAssign;
            Assert.That(landingPageAssign, Is.Not.Null);
            Assert.That(landingPageAssign.LPID, Is.EqualTo(4));
            Assert.That(landingPageAssign.CustomerDoesOverride, Is.False);
            Assert.That(landingPageAssign.Header, Is.EqualTo(string.Empty));
            Assert.That(landingPageAssign.Footer, Is.EqualTo(string.Empty));
            Assert.That(landingPageAssign.UpdatedUserID, Is.Null);

            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "btnPreview").GetValue(customerAbuse) as Button;
            Assert.That(previewButton, Is.Not.Null);
            Assert.That(previewButton.Enabled, Is.False);
            Assert.That(previewButton.Visible, Is.False);
            Assert.That(previewButton.CssClass, Is.EqualTo(string.Empty));
            Assert.That(previewButton.Attributes.Count, Is.Zero);

            var sentBlastsLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "lblSentBlastsWarning").GetValue(customerAbuse) as Label;
            Assert.That(sentBlastsLabel, Is.Not.Null);
            Assert.That(sentBlastsLabel.Text, Is.EqualTo(NonEmptyText));

            var customOverrideWarning = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "lblCustomerOverrideWarning").GetValue(customerAbuse) as Label;
            Assert.That(customOverrideWarning, Is.Not.Null);
            Assert.That(customOverrideWarning.Text, Is.EqualTo(NonEmptyText));

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, "lblErrorMessage").GetValue(customerAbuse) as Label;
            Assert.That(errorMessageLabel, Is.Not.Null);
            Assert.That(errorMessageLabel.Text, Is.EqualTo("<br/>LandingPage: There is a badly formed codesnippet in Footer"));

            _response.Verify(x => x.Redirect("CustomerMain.aspx"), Times.Never());
        }

        [Test]
        public void BtnSaveClick_NullLandingPageAssignAndNonEmptyThankYouText_UpdateControlsAndRedirectToBaseChannelMainPage()
        {
            // Arrange
            var listOptions = ReflectionHelper.GetFieldInfoFromInstanceTypeByName(typeof(CustomerAbuse), "_listOptions");
            listOptions.SetValue(
                null,
                new List<LandingPageOption>()
                {
                    new LandingPageOption()
                    {
                        LPOID = LandingPageID,
                        Name = TextContainsThankYou
                    }
                });

            LandingPageAssignContent landingPageAssignContentParameterValue = null;
            var landingPageAssignContent = new Mock<ILandingPageAssignContent>();
            landingPageAssignContent
                .Setup(x => x.Save(It.IsAny<LandingPageAssignContent>(), It.IsAny<User>()))
                .Callback<LandingPageAssignContent, User>((landingPageAssignContentValue, userValue) =>
                {
                    landingPageAssignContentParameterValue = landingPageAssignContentValue;
                });

            var customerAbuse = CreateCustomerAbuse(landingPageAssignContent);
            InitializeControls(customerAbuse, NonEmptyText, NonEmptyText, NonEmptyText);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(customerAbuse, "btnSave_Click", parameters);

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, FieldLandingPageAssign).GetValue(customerAbuse) as LandingPageAssign;
            landingPageAssign.ShouldSatisfyAllConditions(
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(LandingPageID),
                () => landingPageAssign.CustomerDoesOverride.HasValue.ShouldBeTrue(),
                () => landingPageAssign.CustomerDoesOverride.Value.ShouldBeFalse(),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(NonEmptyText),
                () => landingPageAssign.UpdatedUserID.ShouldBe(DummyUserID));

            landingPageAssignContentParameterValue.ShouldSatisfyAllConditions(
                () => landingPageAssignContentParameterValue.ShouldNotBeNull(),
                () => landingPageAssignContentParameterValue.Display.ShouldBe(NonEmptyText),
                () => landingPageAssignContentParameterValue.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssignContentParameterValue.LPAID = landingPageAssign.LPAID,
                () => landingPageAssignContentParameterValue.LPOID = LandingPageID);

            var previewButton = ReflectionHelper.GetFieldInfoFromInstanceByName(customerAbuse, PreviewButtonName).GetValue(customerAbuse) as Button;
            previewButton.ShouldSatisfyAllConditions(
                () => previewButton.ShouldNotBeNull(),
                () => previewButton.Enabled.ShouldBeTrue(),
                () => previewButton.Visible.ShouldBeTrue(),
                () => previewButton.CssClass.ShouldBe(ECNButtonMedium),
                () => previewButton.Attributes.ShouldNotBeNull(),
                () => previewButton.Attributes.Count.ShouldBe(1),
                () => previewButton.Attributes["onclick"].ShouldNotBeNull(),
                () => previewButton.Attributes["onclick"].ShouldBe(
                    string.Format(
                        "window.open('{0}/engines/reportSpam.aspx?p=&preview=-1', 'popup_window', 'width=1000,height=750,resizable=yes');",
                        ConfigurationManager.AppSettings[ActivityDomainPathKey])));

            var sentBlastsLabel = ReflectionHelper
                .GetFieldInfoFromInstanceByName(customerAbuse, "lblSentBlastsWarning")
                .GetValue(customerAbuse)
                as Label;
            sentBlastsLabel.ShouldSatisfyAllConditions(
                () => sentBlastsLabel.ShouldNotBeNull(),
                () => sentBlastsLabel.Text.ShouldBe(NonEmptyText));

            var customOverrideWarning = ReflectionHelper
                .GetFieldInfoFromInstanceByName(customerAbuse, "lblCustomerOverrideWarning")
                .GetValue(customerAbuse) 
                as Label;
            customOverrideWarning.ShouldSatisfyAllConditions(
                () => customOverrideWarning.ShouldNotBeNull(),
                () => customOverrideWarning.Text.ShouldBe(
                    "Note: The above settings will not be visible to customers until you override the Basechannel settings."));

            var errorMessageLabel = ReflectionHelper
                .GetFieldInfoFromInstanceByName(customerAbuse, "lblErrorMessage")
                .GetValue(customerAbuse)
                as Label;
            errorMessageLabel.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldBe(NonEmptyText));

            _response.Verify(x => x.Redirect("CustomerMain.aspx"), Times.Once());
        }

        [Test]
        public void BtnSaveClick_ExceptionOnSave_UpdateControlsAndRedirectToBaseChannelMainPage()
        {
            // Arrange
            var applicationLog = new Mock<IApplicationLog>();
            applicationLog.Setup(x => x.LogCriticalError(
                It.IsAny<ECNException>(),
                "BaseChannelAbuse.btnSave_Click",
                DummyUserID,
                string.Empty,
                DummyUserID,
                DummyUserID));

            var landingPageAssignContent = new Mock<ILandingPageAssignContent>();
            landingPageAssignContent
                .Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<User>()))
                .Throws(new ECNException(null));

            var master = new Mock<IMasterCommunicator>();
            master.Setup(x => x.GetUserID()).Throws(new ECNException(null));

            var customerAbuse = CreateCustomerAbuse(landingPageAssignContent, applicationLog.Object, master);
            InitializeBtnSaveException(customerAbuse);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(customerAbuse, "btnSave_Click", parameters);

            // Assert
            applicationLog.VerifyAll();
        }

        private void InitializeBtnSaveException(CustomerAbuse customerAbuse)
        {
            ReflectionHelper.SetValue(customerAbuse, "phError", new PlaceHolder() { Visible = true });
            ReflectionHelper.SetValue(customerAbuse, "lblErrorMessage", new Label() { Text = NonEmptyText });
        }

        private void InitializeControls(CustomerAbuse customerAbuse, string headerText, string footerText, string thankYouText = "")
        {
            ReflectionHelper.SetValue(customerAbuse, "rblBasechannelOverride", new RadioButtonList() { SelectedValue = string.Empty });
            ReflectionHelper.SetValue(customerAbuse, "txtHeader", new TextBox() { Text = headerText });
            ReflectionHelper.SetValue(customerAbuse, "txtFooter", new TextBox() { Text = footerText });
            ReflectionHelper.SetValue(customerAbuse, "txtThankYou", new TextBox() { Text = thankYouText });
            ReflectionHelper.SetValue(customerAbuse, "lblCustomerOverrideWarning", new Label() { Text = NonEmptyText });
            ReflectionHelper.SetValue(customerAbuse, "btnPreview", new Button() { Enabled = false, Visible = false, CssClass = string.Empty });
            ReflectionHelper.SetValue(customerAbuse, "lblSentBlastsWarning", new Label() { Text = NonEmptyText });
            InitializeBtnSaveException(customerAbuse);
        }

        private CustomerAbuse CreateCustomerAbuse(
            Mock<ILandingPageAssignContent> landingPageAssignContent = null,
            IApplicationLog applicationLog = null,
            Mock<IMasterCommunicator> master = null)
        {
            if (master == null)
            {
                master = new Mock<IMasterCommunicator>();
                master.Setup(x => x.GetUserID()).Returns(DummyUserID);
                master.Setup(x => x.GetCustomerID()).Returns(DummyCustomerID);
                master.Setup(x => x.GetCurrentUser()).Returns(new User());
            }

            var landingPageAssign = new Mock<ILandingPageAssign>();
            landingPageAssign.Setup(x => x.Save(It.IsAny<LandingPageAssign>(), It.IsAny<User>()));
            var dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn());
            dataTable.Rows.Add(dataTable.NewRow());
            landingPageAssign.Setup(x => x.GetPreviewParameters(It.IsAny<int>(), It.IsAny<int>())).Returns(dataTable);

            if (landingPageAssignContent == null)
            {
                landingPageAssignContent = new Mock<ILandingPageAssignContent>();
                landingPageAssignContent.Setup(x => x.Save(It.IsAny<LandingPageAssignContent>(), It.IsAny<User>()));
                landingPageAssignContent.Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<User>()));
            }

            _response = new Mock<HttpResponseBase>();
            _response.Setup(x => x.Redirect("CustomerMain.aspx"));

            var customerAbuse = new CustomerAbuse(
                master.Object, 
                landingPageAssign.Object, 
                landingPageAssignContent.Object, 
                _response.Object,
                applicationLog);
            return customerAbuse;
        }
    }
}
