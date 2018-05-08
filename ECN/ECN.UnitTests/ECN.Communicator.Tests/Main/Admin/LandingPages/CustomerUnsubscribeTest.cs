using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using AjaxControlToolkit;
using AjaxControlToolkit.Fakes;
using Ecn.Communicator.Main.Admin.Interfaces;
using ecn.communicator.main.admin.landingpages;
using ecn.communicator.main.admin.landingpages.Fakes;
using Ecn.Communicator.Main.Interfaces;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN.Tests.Helpers;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using CommunicatorMasterFakes = ecn.communicator.MasterPages.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Accounts.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Admin.LandingPages
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CustomerUnsubscribeTest
    {
        private const int DummyUserID = -1;
        private const int DummyCustomerID = -2;
        private const string Yes = "Yes";
        private const string NonEmptyText = "Non Empty Text";
        private const string ECNButtonMedium = "ECN-Button-Medium";
        private const string InvalidCodeSnippet = "%%";
        private const string EmptyCodeSnippet = "%% %%";
        private const string DummyCodeSnippet = "%%Dummy%%";
        private const string CustomerNameCodeSnippet = "%%customername%%";
        private const string GroupNameCodeSnippet = "%%groupname%%";
        private const string GroupDescriptionCodeSnippet = "%%groupdescription%%";
        private const string ErrorBadFormattedCodeSnippet = "There is a badly formed codesnippet";
        private const string ErrorInvalidCodeSnippet = "Invalid codesnippet, only %%customername%%, %%groupname%% and %%groupdescription%% are allowed";
        private const string ErrorBadFormattedCodeSnippetInThankYou = "There is a badly formed codesnippet in Thank You Message";
        private const string ErrorInvalidUrlForRedirect = "Invalid URL for Redirect";
        private const string MethodBtnSaveClick = "btnSave_Click";
        private const string LPAFieldName = "LPA";
        private const string RblVisibilityPageLabel = "rblVisibilityPageLabel";
        private const string RblVisibilityMainLabel = "rblVisibilityMainLabel";
        private const string RblVisibilityMasterSuppression = "rblVisibilityMasterSuppression";
        private const string RblRedirectThankYou = "rblRedirectThankYou";
        private const string ThankYou = "thankyou";
        private const string RblVisibilityReason = "rblVisibilityReason";
        private const string Redirect = "redirect";
        private const string Both = "both";
        private const string RblReasonControlType = "rblReasonControlType";
        private const string TextBoxLabel = "Text Box";
        private const string CustomerMainAspx = "CustomerMain.aspx";
        private const string TxtPageLabel = "txtPageLabel";
        private const string TxtMainLabel = "txtMainLabel";
        private const string TxtMasterSuppressionLabel = "txtMasterSuppressionLabel";
        private const string TxtThankYouMessage = "txtThankYouMessage";
        private const string TxtRedirectUrl = "txtRedirectURL";
        private const string TxtReasonLabel = "txtReasonLabel";
        private const string TxtUnsubscribeText = "txtUnsubscribeText";
        private const string CurrentUser = "CurrentUser";
        private const string CurrentCustomer = "CurrentCustomer";
        private const string TempURL = "http://www.tempuri.org";
        private const string DdlRedirectDelay = "ddlRedirectDelay";
        private const string ReasonDataTableName = "dtReason";
        private const string FieldReasonName = "Reason";
        private const string FieldReasonID = "ID";
        private const string FieldReasonIsDeleted = "IsDeleted";
        private const string FieldReasonSortOrder = "SortOrder";
        private const string EditReasonCommandName = "EditReason";
        private const string DeleteReasonCommandName = "DeleteReason";
        private const string ReasonCommandArgument = "ReasonID";
        private const string FieldPnlReasonDropDown = "pnlReasonDropDown";
        private const string FieldRlReasonDropDown = "rlReasonDropDown";
        private const string DropDownText = "Drop Down";
        private const int ReasonSortOrder = 4;
        private const string LblErrorMessage = "lblErrorMessage";
        private const string TextBoxValue = "Text Box";
        private const string MethodReasonControlType = "rblReasonControlType_SelectedIndexChanged";
        private const string Reason = "Reason";
        private const string Id = "ID";
        private const string SortOrder = "SortOrder";
        private const string IsDeleted = "IsDeleted";
        private const string DropDownReason = "dtReason";
        private const string TextNewReason = "txtNewReason";
        private const string PanelDropDown = "pnlReasonDropDown";
        private const string RlDropDown = "rlReasonDropDown";
        private const string LblCustomerOverride = "lblCustomerOverrideWarning";
        private const string Test = "test";
        private const string TestNo = "no";
        private const string TestDropDown = "drop down";
        private const string TestYes = "yes";
        private const string TestDummy = "";
        private const string MethodAddNewReason = "btnAddNewReason_Click";
        private const string Argument = "args";
        private const string MethodVisibilityReason = "rblVisibilityReason_SelectedIndexChanged";
        private const string MethodLoadPreview = "loadPreview";
        private const string Lpa = "LPA";
        private const string LblSentBlast = "lblSentBlastsWarning"; 
        private const string LblReasonLabel = "lblReasonLabel"; 
        private const string MethodPageLoad = "Page_Load";
        private const string MethodLoadData = "LoadData";
        private const string PnlNoAccess = "pnlNoAccess";
        private const string PnlSettings = "pnlSettings";
        private const string CurrentBaseChannel = "CurrentBaseChannel";
        private const string PnlReasonDropDown = "pnlReasonDropDown";
        private const string ThankMessage = "thank";
        private const string ReasonControl = "reason";
        private const string BtnPreview = "btnPreview";
        private const string RowNumber = "RowNumber";
        private const string Column1 = "Column1";
        private const string Column2 = "Column2";
        private const string Column3 = "Column3";
        private const string Column4 = "Column4";
        private const string Column5 = "Column5";
        private const string TblReasonResponseType = "tblReasonResponseType";
        private const string LblSentBlastMessage = "Preview functionality will not be available until after you have sent at least one blast from this customer account.";
        private const string LblCustomerOverrideMessage = "Note: The above settings will not be visible to customers until you override the Basechannel settings.";
        private const string StringOne = "1";
        private const int Zero = 0;
        private const int One = 1;
        private const int Two = 2;
        private const int Three = 3;
        private const int Four = 4;
        private const int Five = 5;
        private const int Six = 6;
        private const int Seven = 7;
        private const int Ten = 10;
        private const int Twelve = 12;
        private const int Thirteen = 13;
        private bool _lpaSaved = false;
        private IDisposable _shimObject;

        private CustomerUnsubscribe _customer;
        private Mock<HttpResponseBase> _response;
        private DataTable _dtReason;
        private LandingPageAssign LPA;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            _customer = CreateCustomerUnsubscribe();
            ShimLandingPageAssignContent.DeleteInt32User = (_, __) => { };
            ShimLandingPageAssignContent.SaveLandingPageAssignContentUser = (_, __) => { };
            var _shimMaster = new CommunicatorMasterFakes::ShimCommunicator();
            ShimBaseChannelUnsubscribe.AllInstances.MasterGet = (obj) => _shimMaster.Instance;
            ShimPage.AllInstances.MasterGet = (obj) => _shimMaster.Instance;
            var shimECNSession = new ShimECNSession();
            var fieldCurrentCustomer = typeof(ECNSession).GetField("CurrentCustomer");
            fieldCurrentCustomer.SetValue(
                shimECNSession.Instance,
                new Customer()
                {
                    CustomerID = 1
                });
            var fieldCurrentUser = typeof(ECNSession).GetField("CurrentUser");
            fieldCurrentUser.SetValue(
                shimECNSession.Instance,
                new User()
                {
                    UserID = 1
                });
            CommunicatorMasterFakes::ShimCommunicator.AllInstances.UserSessionGet = (obj) => shimECNSession.Instance;
            ShimHttpContext.CurrentGet = () =>
            {
                var context = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));
                return context;
            };
            _dtReason = new DataTable();
            ReflectionHelper.SetValue(_customer, LPAFieldName, null);
            LPA = new LandingPageAssign();
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
            _dtReason.Clear();
        }

        [Test]
        public void BtnSaveClick_NullLandingPageAssign_UpdateControlsAndRedirectToCustomerMainPage()
        {
            // Arrange
            InitializeControls(_customer, NonEmptyText, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName).GetValue(_customer) as LandingPageAssign;
            Assert.That(landingPageAssign, Is.Not.Null);
            Assert.That(landingPageAssign.LPID, Is.EqualTo(1));
            Assert.That(landingPageAssign.CustomerDoesOverride, Is.False);
            Assert.That(landingPageAssign.Header, Is.EqualTo(NonEmptyText));
            Assert.That(landingPageAssign.Footer, Is.EqualTo(NonEmptyText));
            Assert.That(landingPageAssign.CreatedUserID, Is.EqualTo(DummyUserID));
            Assert.That(landingPageAssign.CustomerID, Is.EqualTo(DummyCustomerID));

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LblErrorMessage).GetValue(_customer) as Label;
            Assert.That(errorMessageLabel, Is.Not.Null);
            Assert.That(errorMessageLabel.Text, Is.EqualTo(NonEmptyText));

            _response.Verify(x => x.Redirect(CustomerMainAspx), Times.Once());
        }

        [Test]
        public void BtnSaveClick_InvalidCodeSnippetsForHeaderText_UpdateControlsAndRedirectToCustomerMainPage()
        {
            // Arrange
            InitializeControls(_customer, InvalidCodeSnippet, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName).GetValue(_customer) as LandingPageAssign;
            Assert.That(landingPageAssign, Is.Not.Null);
            Assert.That(landingPageAssign.LPID, Is.EqualTo(1));
            Assert.That(landingPageAssign.CustomerDoesOverride, Is.False);
            Assert.That(landingPageAssign.Header, Is.EqualTo(string.Empty));
            Assert.That(landingPageAssign.Footer, Is.EqualTo(string.Empty));
            Assert.That(landingPageAssign.CreatedUserID, Is.EqualTo(DummyUserID));
            Assert.That(landingPageAssign.CustomerID, Is.EqualTo(DummyCustomerID));

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LblErrorMessage).GetValue(_customer) as Label;
            Assert.That(errorMessageLabel, Is.Not.Null);
            Assert.That(errorMessageLabel.Text, Is.EqualTo("<br/>LandingPage: There is a badly formed codesnippet in Header"));

            _response.Verify(x => x.Redirect(CustomerMainAspx), Times.Never());
        }

        [Test]
        public void BtnSaveClick_InvalidCodeSnippetsForFooterText_UpdateControlsAndRedirectToCustomerMainPage()
        {
            // Arrange
            InitializeControls(_customer, NonEmptyText, InvalidCodeSnippet);

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName).GetValue(_customer) as LandingPageAssign;
            Assert.That(landingPageAssign, Is.Not.Null);
            Assert.That(landingPageAssign.LPID, Is.EqualTo(1));
            Assert.That(landingPageAssign.CustomerDoesOverride, Is.False);
            Assert.That(landingPageAssign.Header, Is.EqualTo(NonEmptyText));
            Assert.That(landingPageAssign.Footer, Is.EqualTo(InvalidCodeSnippet));
            Assert.That(landingPageAssign.CreatedUserID, Is.EqualTo(DummyUserID));
            Assert.That(landingPageAssign.CustomerID, Is.EqualTo(DummyCustomerID));

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LblErrorMessage).GetValue(_customer) as Label;
            Assert.That(errorMessageLabel, Is.Not.Null);
            Assert.That(errorMessageLabel.Text, Is.EqualTo("<br/>LandingPage: There is a badly formed codesnippet in Footer"));

            _response.Verify(x => x.Redirect(CustomerMainAspx), Times.Once());
        }

        [Test]
        public void ValidCodeSnippets_EmptyString_ReturnsTrueCodeSnippetWithNullMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _customer,
                "validCodeSnippets",
                new object[]
                {
                    string.Empty
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.True);
            Assert.That(codeSnippetError.message, Is.Null);
        }

        [Test]
        public void ValidCodeSnippets_SingleMatch_ReturnsFalseCodeSnippetWithErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _customer,
                "validCodeSnippets",
                new object[]
                {
                    InvalidCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.False);
            Assert.That(codeSnippetError.message, Is.EqualTo(ErrorBadFormattedCodeSnippet));
        }

        [Test]
        public void ValidCodeSnippets_TwoMatches_ReturnsTrueCodeSnippetWithNullErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _customer,
                "validCodeSnippets",
                new object[]
                {
                    EmptyCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.True);
            Assert.That(codeSnippetError.message, Is.Null);
        }

        [Test]
        public void ValidCodeSnippets_TwoMatchesButNotcustomernameORgroupnameORgroupdescription_ReturnsFalseCodeSnippetWithErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _customer,
                "validCodeSnippets",
                new object[]
                {
                    DummyCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.False);
            Assert.That(codeSnippetError.message, Is.EqualTo(ErrorInvalidCodeSnippet));
        }

        [Test]
        public void ValidCodeSnippets_TwoMatchesWithcustomername_ReturnsTrueCodeSnippetWithNullErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _customer,
                "validCodeSnippets",
                new object[]
                {
                    CustomerNameCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.True);
            Assert.That(codeSnippetError.message, Is.Null);
        }

        [Test]
        public void ValidCodeSnippets_TwoMatchesWithgroupname_ReturnsTrueCodeSnippetWithNullErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _customer,
                "validCodeSnippets",
                new object[]
                {
                    GroupNameCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.True);
            Assert.That(codeSnippetError.message, Is.Null);
        }

        [Test]
        public void ValidCodeSnippets_TwoMatchesWithgroupdescription_ReturnsTrueCodeSnippetWithNullErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _customer,
                "validCodeSnippets",
                new object[]
                {
                    GroupDescriptionCodeSnippet
                }) as CodeSnippetError;

            // Assert
            Assert.That(codeSnippetError, Is.Not.Null);
            Assert.That(codeSnippetError.valid, Is.True);
            Assert.That(codeSnippetError.message, Is.Null);
        }

        [Test]
        [TestCase(ThankYou, TextBoxLabel)]
        [TestCase(Redirect, NonEmptyText)]
        [TestCase(Both, NonEmptyText)]
        public void BtnSaveClick_OnValidCodeSnippets_SaveLandingPageAssignContentAndRedirectToCustomerMainPage(
            string redirectThankYou,
            string reasonControlType)
        {
            // Arrange
            InitializeControls(_customer, NonEmptyText, InvalidCodeSnippet);
            SetupForBtnSaveClick(redirectThankYou, reasonControlType);
            ReflectionHelper.SetValue(_customer, TxtRedirectUrl, new TextBox {Text = TempURL});

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName)
                .GetValue(_customer) as LandingPageAssign;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.CustomerDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(InvalidCodeSnippet),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.CustomerID.ShouldBe(DummyCustomerID),
                () => _lpaSaved.ShouldBeTrue(),
                () => _response.Verify(x => x.Redirect(CustomerMainAspx), Times.Once())
            );
        }

        [Test]
        public void BtnSaveClick_OnInValidCodeSnippets_ThrowECNException()
        {
            // Arrange
            InitializeControls(
                _customer,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                InvalidCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);
            ReflectionHelper.SetValue(_customer, ReasonDataTableName, new DataTable());

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName)
                .GetValue(_customer) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LblErrorMessage)
                .GetValue(_customer) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain($"{ErrorBadFormattedCodeSnippet} in Reason label"),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.CustomerDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(string.Empty),
                () => landingPageAssign.Footer.ShouldBe(string.Empty),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.CustomerID.ShouldBe(DummyCustomerID)
            );
        }

        [Test]
        public void BtnSaveClick_VisibilityPageLabel_ThrowECNException()
        {
            // Arrange
            InitializeControls(_customer, NonEmptyText, string.Empty, InvalidCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName)
                .GetValue(_customer) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LblErrorMessage)
                .GetValue(_customer) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain($"{ErrorBadFormattedCodeSnippet} in Page Label"),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.CustomerDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(string.Empty),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.CustomerID.ShouldBe(DummyCustomerID),
                () => _lpaSaved.ShouldBeFalse()
            );
        }

        [Test]
        public void BtnSaveClick_VisibilityMainLabel_ThrowECNException()
        {
            // Arrange
            InitializeControls(_customer, NonEmptyText, string.Empty, string.Empty, string.Empty, InvalidCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);
            ReflectionHelper.SetValue(_customer, RblVisibilityPageLabel, new RadioButtonList());

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName)
                .GetValue(_customer) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LblErrorMessage)
                .GetValue(_customer) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain($"{ErrorBadFormattedCodeSnippet} in Main Label"),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.CustomerDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(string.Empty),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.CustomerID.ShouldBe(DummyCustomerID),
                () => _lpaSaved.ShouldBeFalse()
            );
        }

        [Test]
        public void BtnSaveClick_VisibilityMasterSuppression_ThrowECNException()
        {
            // Arrange
            InitializeControls(_customer, NonEmptyText, NonEmptyText, string.Empty, string.Empty, string.Empty, InvalidCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);
            ReflectionHelper.SetValue(_customer, RblVisibilityPageLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_customer, RblVisibilityMainLabel, new RadioButtonList());

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName)
                .GetValue(_customer) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LblErrorMessage)
                .GetValue(_customer) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain($"{ErrorBadFormattedCodeSnippet} in Master Suppression"),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.CustomerDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(NonEmptyText),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.CustomerID.ShouldBe(DummyCustomerID),
                () => _lpaSaved.ShouldBeFalse()
            );
        }

        [Test]
        public void BtnSaveClick_UnsubscribeText_ThrowECNException()
        {
            // Arrange
            InitializeControls(_customer, NonEmptyText, NonEmptyText, string.Empty, InvalidCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);
            ReflectionHelper.SetValue(_customer, RblVisibilityPageLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_customer, RblVisibilityMainLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_customer, RblVisibilityMasterSuppression, new RadioButtonList());

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName)
                .GetValue(_customer) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LblErrorMessage)
                .GetValue(_customer) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain($"{ErrorBadFormattedCodeSnippet} in Unsubscribe Text"),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.CustomerDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(NonEmptyText),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.CustomerID.ShouldBe(DummyCustomerID),
                () => _lpaSaved.ShouldBeFalse()
            );
        }

        //[Test]
        [TestCase(ThankYou, InvalidCodeSnippet, ErrorBadFormattedCodeSnippetInThankYou)]
        [TestCase(Redirect, NonEmptyText, ErrorInvalidUrlForRedirect)]
        [TestCase(Both, InvalidCodeSnippet, ErrorBadFormattedCodeSnippetInThankYou)]
        [TestCase(Both, TxtThankYouMessage, ErrorInvalidUrlForRedirect)]
        public void BtnSaveClick_RedirectThankYou_ThrowECNException(string redirectThankYou, string thankYouMessage, string expectedErrorMessage)
        {
            // Arrange
            InitializeControls(_customer, NonEmptyText);
            SetupForBtnSaveClick(redirectThankYou, TextBoxLabel);
            ReflectionHelper.SetValue(_customer, TxtUnsubscribeText, new TextBox {Text = TxtUnsubscribeText});
            ReflectionHelper.SetValue(_customer, RblVisibilityPageLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_customer, RblVisibilityMainLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_customer, RblVisibilityMasterSuppression, new RadioButtonList());
            ReflectionHelper.SetValue(_customer, TxtRedirectUrl, new TextBox());
            ReflectionHelper.SetValue(_customer, TxtThankYouMessage, new TextBox { Text = thankYouMessage });

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName)
                .GetValue(_customer) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LblErrorMessage)
                .GetValue(_customer) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain(expectedErrorMessage),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.CustomerDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(string.Empty),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.CustomerID.ShouldBe(DummyCustomerID)
            );
        }

        [Test]
        public void BtnSaveClick_VisibilityReason_ThrowECNException()
        {
            // Arrange
            InitializeControls(_customer, NonEmptyText);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);
            var dtReason = new DataTable();
            dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            var row = dtReason.NewRow();
            row[0] = bool.TrueString.ToLower();
            dtReason.Rows.Add(row);
            ReflectionHelper.SetValue(_customer, ReasonDataTableName, dtReason);
            const string ExpectedErrorMessage = "Please enter at least one Reason";
            ReflectionHelper.SetValue(_customer, RblVisibilityPageLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_customer, RblVisibilityMainLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_customer, RblVisibilityMasterSuppression, new RadioButtonList());
            ReflectionHelper.SetValue(_customer, RblRedirectThankYou, new RadioButtonList());
            ReflectionHelper.SetValue(_customer, TxtUnsubscribeText, new TextBox { Text = TxtUnsubscribeText });
            ReflectionHelper.SetValue(_customer, TxtReasonLabel, new TextBox { Text = TxtReasonLabel });

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LPAFieldName)
                .GetValue(_customer) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, LblErrorMessage)
                .GetValue(_customer) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain(ExpectedErrorMessage),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.CustomerDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(string.Empty),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.CustomerID.ShouldBe(DummyCustomerID)
            );
        }

        [Test]
        [TestCase("thankyou", new bool[] { false, false, true })]
        [TestCase("redirect", new bool[] { false, true, false })]
        [TestCase("both", new bool[] { true, true, true })]
        [TestCase("neither", new bool[] { false, false, false })]
        public void RblRedirectThankYouSelectedIndexChanged_SelectedValue_UpdateControlVisibility(string selectedValue, bool[] expectedControlsVisibility)
        {
            // Arrange
            InitializeControls(_customer);
            InitializeRedirectRadioAndDropDownList(selectedValue);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_customer, "rblRedirectThankYou_SelectedIndexChanged", parameters);

            // Assert
            AssertTableVisibility("tblDelay", expectedControlsVisibility[0]);
            AssertTableVisibility("tblRedirect", expectedControlsVisibility[1]);
            AssertTableVisibility("tblThankYou", expectedControlsVisibility[2]);

            AssertTextBox("txtRedirectURL", string.Empty);
            AssertTextBox("txtThankYouMessage", string.Empty);
            AssertDropDownListSelectedIndex("ddlRedirectDelay", 0);
        }

        [Test]
        [TestCase("no", false)]
        [TestCase("not no", true)]
        public void RblVisibilityMainLabelSelectedIndexChanged_rblVisibilityMainLabelSelectedIndexChanged_UpdateControls(
            string radioButtonSelectedValue,
            bool expectedTextBoxVisibility)
        {
            // Arrange
            InitializeControls(_customer);
            InitializeRadioButton("rblVisibilityMainLabel", radioButtonSelectedValue);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_customer, "rblVisibilityMainLabel_SelectedIndexChanged", parameters);

            // Assert
            AssertTextBoxInitialized("txtMainLabel", expectedTextBoxVisibility);
        }

        [Test]
        [TestCase("no", false)]
        [TestCase("not no", true)]
        public void RblVisibilityMasterSuppressionSelectedIndexChanged_rblVisibilityMainLabelSelectedIndexChanged_UpdateControls(
            string radioButtonSelectedValue,
            bool expectedTextBoxVisibility)
        {
            // Arrange
            InitializeControls(_customer);
            InitializeRadioButton("rblVisibilityMasterSuppression", radioButtonSelectedValue);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_customer, "rblVisibilityMasterSuppression_SelectedIndexChanged", parameters);

            // Assert
            AssertTextBoxInitialized("txtMasterSuppressionLabel", expectedTextBoxVisibility);
        }

        [Test]
        [TestCase("no", false)]
        [TestCase("not no", true)]
        public void RblVisibilityPageLabelSelectedIndexChanged_rblVisibilityMainLabelSelectedIndexChanged_UpdateControls(
            string radioButtonSelectedValue,
            bool expectedTextBoxVisibility)
        {
            // Arrange
            InitializeControls(_customer);
            InitializeRadioButton("rblVisibilityPageLabel", radioButtonSelectedValue);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_customer, "rblVisibilityPageLabel_SelectedIndexChanged", parameters);

            // Assert
            AssertTextBoxInitialized("txtPageLabel", expectedTextBoxVisibility);
        }

        [Test]
        [TestCase("no", false)]
        [TestCase("drop down", true)]
        public void RblVisibilityReasonSelectedIndexChanged_rblVisibilityMainLabelSelectedIndexChanged_UpdateControls(
            string radioButtonSelectedValue,
            bool expectedTextBoxVisibility)
        {
            // Arrange
            InitializeControls(_customer);
            InitializeRadioButton("rblVisibilityReason", radioButtonSelectedValue);
            InitializeRadioButton("rblReasonControlType", radioButtonSelectedValue);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_customer, "rblVisibilityReason_SelectedIndexChanged", parameters);

            // Assert
            AssertTextBoxInitialized("txtNewReason", true);
            AssertTextBoxInitialized("txtReasonLabel", expectedTextBoxVisibility);
            AssertTableVisibility("tblReasonResponseType", expectedTextBoxVisibility);
            AssertPanelVisibility("pnlReasonDropDown", expectedTextBoxVisibility);
            AssertRadioButtonVisibility("rblReasonControlType", expectedTextBoxVisibility);
        }

        [Test]
        public void RlReasonDropDownItemCommand_EditReasonCommand_UpdateControls()
        {
            // Arrange
            InitializeReasonControls();
            InitializeReasonTable(ReasonCommandArgument);

            // Act
            var reorderListCommandEventArgs = ReflectionHelper.CreateInstance<ReorderListCommandEventArgs>(
                new object[] { new CommandEventArgs(EditReasonCommandName, ReasonCommandArgument), this, null });
            var parameters = new object[] { null, reorderListCommandEventArgs };
            ReflectionHelper.ExecuteMethod(_customer, "rlReasonDropDown_ItemCommand", parameters);

            // Assert
            AssertTextBox("txtReasonLabelEdit", FieldReasonName);
            AssertButtonCommand("btnSaveReason", ReasonCommandArgument);
        }

        [Test]
        public void RlReasonDropDownItemCommand_DeleteReasonCommand_SetsIsDeletedAndDecrementSortOrderAndCallsLoadReasonData()
        {
            // Arrange
            InitializeControls(_customer);
            InitializeReasonTable(ReasonCommandArgument);

            // Act
            var reorderListCommandEventArgs = ReflectionHelper.CreateInstance<ReorderListCommandEventArgs>(
                new object[] { new CommandEventArgs(DeleteReasonCommandName, ReasonCommandArgument), this, null });
            var parameters = new object[] { null, reorderListCommandEventArgs };
            ReflectionHelper.ExecuteMethod(_customer, "rlReasonDropDown_ItemCommand", parameters);

            // Assert
            AssertDataTableDataLength($"{FieldReasonIsDeleted} = false", 1);
            AssertDataTableDataLength($"{FieldReasonIsDeleted} = true", 1);
            AssertDataTableDataLength($"{FieldReasonSortOrder} = {ReasonSortOrder}", 2);
        }

        [Test]
        [TestCase(ReasonSortOrder, ReasonSortOrder + 1, ReasonSortOrder + 2)]
        [TestCase(ReasonSortOrder, ReasonSortOrder - 1, ReasonSortOrder)]
        public void RlReasonDropDownItemReorder_OldIndex_NewIndex(int oldIndex, int newIndex, int expectedResult)
        {
            // Arrange
            InitializeControls(_customer);
            InitializeReasonTable(ReasonCommandArgument);
            ShimReorderList.AllInstances.PerformDataBindingIEnumerable = (_, __) => { };

            // Act
            var reorderListItemReorderEventArgs = ReflectionHelper.CreateInstance<ReorderListItemReorderEventArgs>(
                new object[] { null, oldIndex, newIndex });
            var parameters = new object[] { null, reorderListItemReorderEventArgs };
            ReflectionHelper.ExecuteMethod(_customer, "rlReasonDropDown_ItemReorder", parameters);

            // Assert
            AssertDataTableDataLength($"{FieldReasonSortOrder} = {expectedResult}", 2);
        }

        [Test]
        public void RblReasonControlTypeSelectedIndexChanged_ReasonControlTypeNotTextBox_FillDataTableWithDefaultReasons()
        {
            // Arrange
            InitializeControls(_customer);
            InitializeReasonColumns();
            ReflectionHelper.SetValue(_customer, ReasonDataTableName, _dtReason);
            ReflectionHelper.SetValue(_customer, RblReasonControlType, GetRadioButtonList(string.Empty));
            ShimReorderList.AllInstances.PerformDataBindingIEnumerable = (_, __) => { };

            // Act
            var parameters = new object[] { null, EventArgs.Empty};
            ReflectionHelper.ExecuteMethod(_customer, "rblReasonControlType_SelectedIndexChanged", parameters);

            // Assert
            AssertDefaultReasons();
        }

        [Test]
        public void LoadData_LandingPageAssignIsNull_FillDataTable()
        {
            // Arrange
            ShimLandingPageAssign.GetByCustomerIDInt32Int32 = (_, __) => null;
            InitializeReasonColumns();
            ReflectionHelper.SetValue(_customer, ReasonDataTableName, _dtReason);

            // Act	
            ReflectionHelper.ExecuteMethod(_customer, "LoadData", new object[] { });

            // Assert
            AssertDefaultReasons();
        }

        [Test]
        public void LoadData_GetByLPOIDReturnEmptyList_FillDataTable()
        {
            // Arrange
            InitializeControls(_customer);
            ReflectionHelper.SetValue(_customer, RblVisibilityReason, GetRadioButtonList(Yes));
            ReflectionHelper.SetValue(_customer, RblReasonControlType, GetRadioButtonList(DropDownText));

            ShimLandingPageAssign.GetByCustomerIDInt32Int32 = (_, __) => GetLandingPageAssign(1);
            InitializeReasonColumns();
            ReflectionHelper.SetValue(_customer, ReasonDataTableName, _dtReason);
            const int LPOID = 3;
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (id) =>
            {
                return new List<LandingPageAssignContent>
                {
                    new LandingPageAssignContent
                    {
                        LPOID = LPOID,
                        Display = NonEmptyText
                    }
                };
            };
            ShimLandingPageAssignContent.GetByLPOIDInt32Int32 = (_, __) =>
            {
                return new List<LandingPageAssignContent>();
            };

            // Act	
            ReflectionHelper.ExecuteMethod(_customer, "LoadData", new object[] { });

            // Assert
            AssertDefaultReasons();

            var reasonDropDown = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, FieldRlReasonDropDown)
                .GetValue(_customer) as ReorderList;
            reasonDropDown.ShouldSatisfyAllConditions(
                () => reasonDropDown.ShouldNotBeNull(),
                () => reasonDropDown.DataSource.ShouldNotBeNull(),
                () => (reasonDropDown.DataSource as IList).ShouldNotBeNull(),
                () => (reasonDropDown.DataSource as IList).Count.ShouldBe(6));

            var reasonPanel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, FieldPnlReasonDropDown)
                .GetValue(_customer) as Panel;
            reasonPanel.ShouldSatisfyAllConditions(
                () => reasonPanel.ShouldNotBeNull(),
                () => reasonPanel.Visible.ShouldBeTrue());
        }

        [Test]
        public void LoadData_LandingPageAssignIsNotNull_DataTableIsEmpty()
        {
            // Arrange
            InitializeControls(_customer);
            ShimLandingPageAssign.GetByCustomerIDInt32Int32 = (_, __) => GetLandingPageAssign(1);
            ShimLandingPageAssignContent.GetByLPOIDInt32Int32 = (_, __) => GetLandingPagesAssignContentList(1);
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_customer, ReasonDataTableName, _dtReason);
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (id) => GetLandingPagesAssignContentList(id);

            // Act	
            ReflectionHelper.ExecuteMethod(_customer, "LoadData", new object[] { });

            // Assert
            AssertTextBoxInitialized("txtMasterSuppressionLabel", true);
        }

        [Test]
        [TestCase(2, "txtReasonLabel")]
        [TestCase(3, "txtReasonLabel")]
        [TestCase(4, "txtUnsubscribeText")]
        [TestCase(6, "txtPageLabel")]
        [TestCase(7, "txtMainLabel")]
        public void LoadData_LandingPageAssignIsNotNull_DataTableIsEmpty(int lpId, string textBoxName)
        {
            // Arrange
            InitializeControls(_customer);
            ShimLandingPageAssign.GetByCustomerIDInt32Int32 = (_, __) => GetLandingPageAssign(lpId);
            ShimLandingPageAssignContent.GetByLPOIDInt32Int32 = (_, __) => GetLandingPagesAssignContentList(lpId);
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_customer, ReasonDataTableName, _dtReason);
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (id) => GetLandingPagesAssignContentList(id);

            // Act	
            ReflectionHelper.ExecuteMethod(_customer, "LoadData", new object[] { });

            // Assert
            AssertTextBoxInitialized(textBoxName, true, NonEmptyText);
        }

        [TestCase(5, 5, 5, true, false, false)]
        [TestCase(12, 12, 12, false, true, false)]
        [TestCase(5, 12, 13, true, true, true)]
        [TestCase(0, 0, 0, false, false, false)]
        public void LoadData_HasThankYouRedirectDelayRedirectURL_DataTableIsEmpty(
            int thankYouId,
            int redirectURLId,
            int redirectDelayId,
            bool isThankYouTableVisible,
            bool isRedirectTableVisible,
            bool isDelayTableVisible)
        {
            // Arrange
            InitializeControls(_customer);
            ShimLandingPageAssign.GetByCustomerIDInt32Int32 = (id, lpid) => GetLandingPageAssign(1);
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_customer, ReasonDataTableName, _dtReason);
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (id) =>
            {
                return new List<LandingPageAssignContent>
                {
                    new LandingPageAssignContent
                    {
                        LPOID = thankYouId,
                        Display = "TestString"
                    },
                    new LandingPageAssignContent
                    {
                        LPOID = redirectURLId,
                        Display = "TestString"
                    },
                    new LandingPageAssignContent
                    {
                        LPOID = redirectDelayId,
                        Display = "TestString"
                    }
                };
            };

            // Act	
            ReflectionHelper.ExecuteMethod(_customer, "LoadData", new object[] { });

            // Assert
            AssertTableVisibility("tblDelay", isDelayTableVisible);
            AssertTableVisibility("tblRedirect", isRedirectTableVisible);
            AssertTableVisibility("tblThankYou", isThankYouTableVisible);
        }

        private static List<LandingPageAssignContent> GetLandingPagesAssignContentList(int lpId)
        {
            return new List<LandingPageAssignContent>
                {
                    new LandingPageAssignContent
                    {
                        LPOID = lpId,
                        Display = NonEmptyText,
                        SortOrder = 0,
                        IsDeleted = false
                    }
                };
        }

        private LandingPageAssign GetLandingPageAssign(int landingPageAssignId)
        {
            return new LandingPageAssign
            {
                CustomerDoesOverride = true,
                BaseChannelDoesOverride = true,
                CustomerCanOverride = true,
                LPAID = landingPageAssignId
            };
        }

        private void AssertDefaultReasons()
        {
            var actualResult = ReflectionHelper
                .GetFieldInfoFromInstanceTypeByName(typeof(CustomerUnsubscribe), ReasonDataTableName)
                .GetValue(null) as DataTable;
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows
                    .Cast<DataRow>()
                    .ShouldNotBeEmpty(),
                () => actualResult.Rows.Count.ShouldBeGreaterThanOrEqualTo(6));

            var reasonNames = new string[]
            {
                "Email Frequency",
                "Email Volume",
                "Content not relevant",
                "Signed up for one-time email",
                "Circumstances changed(moved, married, changed jobs, etc.)",
                "Prefer to get information another way"
            };
            for (var i = 0; i < reasonNames.Length; i++)
            {
                var dataRow = actualResult.Select($"{FieldReasonSortOrder} = {i + 1}");
                dataRow.ShouldSatisfyAllConditions(
                    () => dataRow.ShouldNotBeNull(),
                    () => dataRow.Length.ShouldBeGreaterThanOrEqualTo(1),
                    () => dataRow[0][FieldReasonName].ShouldBe(reasonNames[i]));
            }
        }

        private void AssertDataTableDataLength(string columnfilter, int dataCount)
        {
            var dtReason = ReflectionHelper.GetFieldInfoFromInstanceTypeByName(typeof(CustomerUnsubscribe), ReasonDataTableName)
                .GetValue(null) as DataTable;
            dtReason.ShouldNotBeNull();

            var columnData = dtReason.Select(columnfilter);
            columnData.ShouldSatisfyAllConditions(
                () => columnData.ShouldNotBeNull(),
                () => columnData.Length.ShouldBe(dataCount));
        }

        private void AssertButtonCommand(string buttonName, string commandArgument)
        {
            var button = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, buttonName)
                       .GetValue(_customer) as Button;
            button.ShouldSatisfyAllConditions(
                () => button.ShouldNotBeNull(),
                () => button.CommandArgument.ShouldBe(commandArgument));
        }

        private void InitializeReasonTable(string reasonID)
        {
            InitializeReasonColumns();
            AddNewRow(ReasonSortOrder);
            AddNewRow(ReasonSortOrder + 1);
            ReflectionHelper.SetValue(_customer, ReasonDataTableName, _dtReason);
        }

        private void InitializeReasonColumns()
        {
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
        }

        private void AddNewRow(int sortOrder)
        {
            var row = _dtReason.NewRow();
            row[FieldReasonID] = ReasonCommandArgument;
            row[FieldReasonName] = FieldReasonName;
            row[FieldReasonIsDeleted] = false;
            row[FieldReasonSortOrder] = sortOrder;
            _dtReason.Rows.Add(row);
        }

        private void InitializeReasonControls(string reasonText = "", string commandArgument = "")
        {
            ReflectionHelper.SetValue(_customer, "txtReasonLabelEdit", new TextBox() { Text = reasonText });
            ReflectionHelper.SetValue(_customer, "btnSaveReason", new Button() { CommandArgument = commandArgument });
            ReflectionHelper.SetValue(_customer, "mpeEditReason", new ModalPopupExtender());
        }

        private void AssertRadioButtonVisibility(string radioButtonName, bool expectedTextBoxVisibility)
        {
            var radioButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, radioButtonName)
                       .GetValue(_customer) as RadioButtonList;
            radioButton.ShouldSatisfyAllConditions(
                () => radioButton.ShouldNotBeNull(),
                () => radioButton.Visible.ShouldBe(expectedTextBoxVisibility));
        }

        private void AssertPanelVisibility(string panelName, bool expectedTextBoxVisibility)
        {
            var panel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, panelName)
                       .GetValue(_customer) as Panel;
            panel.ShouldSatisfyAllConditions(
                () => panel.ShouldNotBeNull(),
                () => panel.Visible.ShouldBe(expectedTextBoxVisibility));
        }

        private void AssertTextBoxInitialized(string textboxName, bool expectedTextBoxVisibility, string initialText = "")
        {
            var textBox = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, textboxName)
                       .GetValue(_customer) as TextBox;
            textBox.ShouldSatisfyAllConditions(
                () => textBox.ShouldNotBeNull(),
                () => textBox.Visible.ShouldBe(expectedTextBoxVisibility),
                () => textBox.Text.ShouldBe(initialText));
        }

        private void InitializeRadioButton(string radioButtonName, string selectedValue)
        {
            var radioButtonList = new RadioButtonList();
            radioButtonList.Items.Add(selectedValue);
            radioButtonList.SelectedValue = selectedValue;
            ReflectionHelper.SetValue(_customer, radioButtonName, radioButtonList);
        }

        private void AssertDropDownListSelectedIndex(string dropDownListName, int selectedIndex)
        {
            var dropDownList = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, dropDownListName)
                       .GetValue(_customer) as DropDownList;
            dropDownList.ShouldSatisfyAllConditions(
                () => dropDownList.ShouldNotBeNull(),
                () => dropDownList.SelectedIndex.ShouldBe(selectedIndex));
        }

        private void AssertTextBox(string textBoxName, string text)
        {
            var textBox = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, textBoxName)
                       .GetValue(_customer) as TextBox;
            textBox.ShouldSatisfyAllConditions(
                () => textBox.ShouldNotBeNull(),
                () => textBox.Text.ShouldBe(text));
        }

        private void InitializeRedirectRadioAndDropDownList(string selectedValue)
        {
            var radioButtonList = new RadioButtonList();
            radioButtonList.Items.Add(selectedValue);
            radioButtonList.SelectedValue = selectedValue;
            ReflectionHelper.SetValue(_customer, "rblRedirectThankYou", radioButtonList);

            var dropDownList = new DropDownList();
            dropDownList.Items.Add(selectedValue);
            ReflectionHelper.SetValue(_customer, "ddlRedirectDelay", dropDownList);
        }

        private void SetRadioButtonListSelectedValue(string radioButtonListName, string selectedValue)
        {
            var radioButtonList = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, radioButtonListName)
                .GetValue(_customer) as RadioButtonList;

            radioButtonList.SelectedValue = selectedValue;
        }

        private void AssertTableVisibility(string tableName, bool isVisible)
        {
            var htmlTable = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, tableName)
                .GetValue(_customer) as HtmlTable;
            htmlTable.ShouldSatisfyAllConditions(
                () => htmlTable.ShouldNotBeNull(),
                () => htmlTable.Visible.ShouldBe(isVisible));
        }

        private void InitializeControls(
            CustomerUnsubscribe customerUnsubscribe, 
            string headerText = "", 
            string footerText = "", 
            string pageLabel = "", 
            string unsubscribeText = "",
            string mainLabel = "",
            string masterSuppression = "",
            string reasonLabel = "")
        {
            ReflectionHelper.SetValue(customerUnsubscribe, "phError", new PlaceHolder() { Visible = true });
            ReflectionHelper.SetValue(customerUnsubscribe, "rblBasechannelOverride", new RadioButtonList() { SelectedValue = string.Empty });
            ReflectionHelper.SetValue(customerUnsubscribe, "txtHeader", new TextBox() { Text = headerText });
            ReflectionHelper.SetValue(customerUnsubscribe, "txtFooter", new TextBox() { Text = footerText });
            ReflectionHelper.SetValue(customerUnsubscribe, LblErrorMessage, new Label() { Text = NonEmptyText });
            ReflectionHelper.SetValue(customerUnsubscribe, LblCustomerOverride, new Label() { Text = NonEmptyText });
            ReflectionHelper.SetValue(customerUnsubscribe, LblSentBlast, new Label() { Text = NonEmptyText });
            ReflectionHelper.SetValue(customerUnsubscribe, LblReasonLabel, new Label() { Text = NonEmptyText });

            ReflectionHelper.SetValue(customerUnsubscribe, PanelDropDown, new Panel());
            ReflectionHelper.SetValue(customerUnsubscribe, PnlReasonDropDown, new Panel());
            ReflectionHelper.SetValue(customerUnsubscribe, PnlNoAccess, new Panel());
            ReflectionHelper.SetValue(customerUnsubscribe, PnlSettings, new Panel());

            ReflectionHelper.SetValue(customerUnsubscribe, RblVisibilityPageLabel, new RadioButtonList());
            ReflectionHelper.SetValue(customerUnsubscribe, RblVisibilityMainLabel, new RadioButtonList());
            ReflectionHelper.SetValue(customerUnsubscribe, RblVisibilityMasterSuppression, new RadioButtonList());
            ReflectionHelper.SetValue(customerUnsubscribe, RblRedirectThankYou, new RadioButtonList());
            ReflectionHelper.SetValue(customerUnsubscribe, RblVisibilityReason, new RadioButtonList());
            ReflectionHelper.SetValue(customerUnsubscribe, RblReasonControlType, new RadioButtonList());
            ReflectionHelper.SetValue(customerUnsubscribe, BtnPreview, new Button());
            ReflectionHelper.SetValue(customerUnsubscribe, RlDropDown, new ReorderList());
            ReflectionHelper.SetValue(customerUnsubscribe, TxtUnsubscribeText, new TextBox() { Text = unsubscribeText });
            ReflectionHelper.SetValue(customerUnsubscribe, TxtPageLabel, new TextBox() { Text = pageLabel });
            ReflectionHelper.SetValue(customerUnsubscribe, TxtMainLabel, new TextBox() { Text = mainLabel });
            ReflectionHelper.SetValue(customerUnsubscribe, TxtMasterSuppressionLabel, new TextBox() { Text = masterSuppression});
            ReflectionHelper.SetValue(customerUnsubscribe, TxtThankYouMessage, new TextBox());
            ReflectionHelper.SetValue(customerUnsubscribe, TxtRedirectUrl, new TextBox());
            ReflectionHelper.SetValue(customerUnsubscribe, TxtReasonLabel, new TextBox() { Text = reasonLabel });
            ReflectionHelper.SetValue(customerUnsubscribe, TextNewReason, new TextBox());

            ReflectionHelper.SetValue(customerUnsubscribe, DdlRedirectDelay, new DropDownList());
            ReflectionHelper.SetValue(customerUnsubscribe, "tblThankYou", new HtmlTable());
            ReflectionHelper.SetValue(customerUnsubscribe, "tblRedirect", new HtmlTable());
            ReflectionHelper.SetValue(customerUnsubscribe, "tblDelay", new HtmlTable());
            ReflectionHelper.SetValue(customerUnsubscribe, TblReasonResponseType, new HtmlTable());
            ReflectionHelper.SetValue(customerUnsubscribe, "pnlReasonDropDown", new Panel());

            ReflectionHelper.SetValue(customerUnsubscribe, "btnPreview", new Button());
            ReflectionHelper.SetValue(customerUnsubscribe, "rlReasonDropDown", new ReorderList());

            ReflectionHelper.SetValue(customerUnsubscribe, "rblVisibilityReason", new RadioButtonList());
            ReflectionHelper.SetValue(customerUnsubscribe, "rblReasonControlType", new RadioButtonList());
            ReflectionHelper.SetValue(customerUnsubscribe, "lblReasonLabel", new Label());            
        }

        private CustomerUnsubscribe CreateCustomerUnsubscribe()
        {
            var master = new Mock<IMasterCommunicator>();
            master.Setup(x => x.GetUserID()).Returns(DummyUserID);
            master.Setup(x => x.GetCustomerID()).Returns(DummyCustomerID);
            master.Setup(x => x.GetCurrentUser()).Returns(new User());

            var landingPageAssign = new Mock<ILandingPageAssign>();
            landingPageAssign.Setup(x => x.Save(It.IsAny<LandingPageAssign>(), It.IsAny<User>()));
            var dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn());
            dataTable.Rows.Add(dataTable.NewRow());
            landingPageAssign.Setup(x => x.GetPreviewParameters(It.IsAny<int>(), It.IsAny<int>())).Returns(dataTable);

            var landingPageAssignContent = new Mock<ILandingPageAssignContent>();
            landingPageAssignContent.Setup(x => x.Save(It.IsAny<LandingPageAssignContent>(), It.IsAny<User>()));
            landingPageAssignContent.Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<User>()));

            _response = new Mock<HttpResponseBase>();
            _response.Setup(x => x.Redirect(CustomerMainAspx));

            var customerAbuse = new CustomerUnsubscribe(master.Object, landingPageAssign.Object, landingPageAssignContent.Object, _response.Object);
            return customerAbuse;
        }

        private void SetupForBtnSaveClick(string redirectThankYou, string reasonControlType)
        {
            _lpaSaved = false;
            ReflectionHelper.SetValue(_customer, LPAFieldName, new LandingPageAssign());
            ReflectionHelper.SetValue(_customer, RblVisibilityPageLabel, GetRadioButtonList(Yes));
            ReflectionHelper.SetValue(_customer, RblVisibilityMainLabel, GetRadioButtonList(Yes));
            ReflectionHelper.SetValue(_customer, RblVisibilityMasterSuppression, GetRadioButtonList(Yes));
            ReflectionHelper.SetValue(_customer, RblRedirectThankYou, GetRadioButtonList(redirectThankYou));
            ReflectionHelper.SetValue(_customer, RblVisibilityReason, GetRadioButtonList(Yes));
            ReflectionHelper.SetValue(_customer, RblReasonControlType, GetRadioButtonList(reasonControlType));
            ReflectionHelper.SetValue(_customer, ReasonDataTableName, GetDtReason());

            var shimMaster = new CommunicatorMasterFakes::ShimCommunicator();
            ShimCustomerUnsubscribe.AllInstances.MasterGet = (obj) => shimMaster.Instance;
            ShimPage.AllInstances.MasterGet = (obj) => shimMaster.Instance;
            var shimECNSession = new ShimECNSession();
            var fieldCurrentUser = typeof(ECNSession).GetField(CurrentUser);
            fieldCurrentUser.SetValue(shimECNSession.Instance, new User
            {
                UserID = 1
            });
            CommunicatorMasterFakes::ShimCommunicator.AllInstances.UserSessionGet = (obj) => shimECNSession.Instance;
            ShimHttpContext.CurrentGet = () =>
            {
                var context = new HttpContext(new HttpRequest(null, TempURL, null), new HttpResponse(null));
                return context;
            };
            ShimLandingPageAssignContent.SaveLandingPageAssignContentUser = (_, __) => { _lpaSaved = true; };
        }

        private DataTable GetDtReason()
        {
            var dtReason = new DataTable();
            dtReason.Columns.Add(FieldReasonName, typeof(string));
            dtReason.Columns.Add(FieldReasonID, typeof(string));
            dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            var row1 = dtReason.NewRow();
            row1[0] = NonEmptyText;
            row1[1] = $"{NonEmptyText}-{NonEmptyText}";
            row1[2] = 1;
            row1[3] = bool.FalseString.ToLower();
            dtReason.Rows.Add(row1);
            var row2 = dtReason.NewRow();
            row2[0] = NonEmptyText;
            row2[1] = NonEmptyText;
            row2[2] = 1;
            row2[3] = bool.FalseString.ToLower();
            dtReason.Rows.Add(row2);
            return dtReason;
        }

        private RadioButtonList GetRadioButtonList(string itemValue)
        {
            var rbList = new RadioButtonList();
            rbList.Items.Add(new ListItem(NonEmptyText, itemValue));
            rbList.SelectedIndex = 0;
            return rbList;
        }

        [Test]
        public void RblReasonControlType_SelectedIndexChanged_ForTextBox_SetVisibleFalse()
        {
            // Arrange
            CreateCustomerUnsubscribe();
            InitializeControls(_customer);
            ShimListControl.AllInstances.SelectedValueGet = (x) => TextBoxValue;

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodReasonControlType, new object[] { this, new EventArgs() });

            // Assert
            AssertPanel(PanelDropDown, false);
        }

        [Test]
        public void RblReasonControlType_SelectedIndexChanged_ForSelectedValue_SetVisibleFalse()
        {
            // Arrange
            CreateCustomerUnsubscribe();
            InitializeControls(_customer);
            ShimListControl.AllInstances.SelectedValueGet = (x) => string.Empty;
            _dtReason.Columns.Add(Reason, typeof(string));
            _dtReason.Columns.Add(Id, typeof(string));
            _dtReason.Columns.Add(SortOrder, typeof(int));
            _dtReason.Columns.Add(IsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_customer, DropDownReason, _dtReason);
            ShimBaseDataBoundControl.AllInstances.DataBind = _ => { };

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodReasonControlType, new object[] { this, new EventArgs() });

            // Assert
            AssertTextBox(TextNewReason, string.Empty);
        }

        [Test]
        public void LoadPreview_ForCountGreaterThanZero_LoadPreview()
        {
            // Arrange
            CreateCustomerUnsubscribe();
            InitializeControls(_customer);
            ShimLandingPageAssign.GetPreviewParametersInt32Int32 = (x, y) => CreateDataTable();
            LPA.LPAID = Ten;
            LPA.CustomerDoesOverride = false;
            ReflectionHelper.SetValue(_customer, Lpa, LPA);
            CreateMasterCustomerId();

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodLoadPreview);

            // Assert
            AssertLabel(LblCustomerOverride, LblCustomerOverrideMessage);
        }

        [Test]
        public void LoadPreview_ForCountLessThanZero_LoadPreview()
        {
            // Arrange
            CreateCustomerUnsubscribe();
            InitializeControls(_customer);
            ShimLandingPageAssign.GetPreviewParametersInt32Int32 = (x, y) => new DataTable();
            LPA.LPAID = Ten;
            LPA.CustomerDoesOverride = true;
            ReflectionHelper.SetValue(_customer, Lpa, LPA);
            CreateMasterCustomerId();

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodLoadPreview);

            // Assert
            AssertLabel(LblSentBlast, LblSentBlastMessage);
        }

        [Test]
        [TestCase(Test)]
        [TestCase(TestDummy)]
        public void BtnAddNewReason_Click_ForNewReason_LoadGrid(string param)
        {
            // Arrange
            CreateCustomerUnsubscribe();
            InitializeControls(_customer);
            _dtReason.Columns.Add(Reason, typeof(string));
            _dtReason.Columns.Add(Id, typeof(string));
            _dtReason.Columns.Add(SortOrder, typeof(int));
            _dtReason.Columns.Add(IsDeleted, typeof(bool));
            var dataRow = _dtReason.NewRow();
            dataRow[Reason] = string.Empty;
            dataRow[Id] = Argument;
            dataRow[SortOrder] = One;
            dataRow[IsDeleted] = true;
            _dtReason.Rows.Add(dataRow);
            ReflectionHelper.SetValue(_customer, DropDownReason, _dtReason);
            ShimBaseDataBoundControl.AllInstances.DataBind = _ => { };
            ShimTextBox.AllInstances.TextGet = (x) => param;

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodAddNewReason, new object[] { this, new EventArgs() });

            // Assert
            AssertTextBox(TextNewReason, param);
        }

        [Test]
        [TestCase(TestNo)]
        [TestCase(TestDropDown)]
        [TestCase(TestYes)]
        public void RblVisibilityReason_SelectedIndexChanged_ForSelectedValueNo_SetVisibilityFalse(string param)
        {
            // Arrange
            CreateCustomerUnsubscribe();
            InitializeControls(_customer);
            ReflectionHelper.SetValue(_customer, DropDownReason, _dtReason);
            ShimListControl.AllInstances.SelectedValueGet = (x) => param;

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodVisibilityReason, new object[] { this, new EventArgs() });

            // Assert
            AssertTextBox(TextNewReason, string.Empty);
            AssertTextBox(TxtReasonLabel, string.Empty);
        }

       [Test]
        public void PageLoad_ForCurrentUser_LoadData()
        {
            // Arrange
            ShimPageLoad();

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodPageLoad, new object[] { this, new EventArgs() });

            // Assert
            AssertPanel(PnlNoAccess, false);
            AssertPanel(PnlSettings, true);
        }

        [Test]
        [TestCase(One)]
        [TestCase(Zero)]
        public void LoadData_ForLPOID_LoadDataRow(int param)
        {
            // Arrange
            CreateCustomerUnsubscribe();
            InitializeControls(_customer);
            SetupForBtnSaveClick(ThankMessage, ReasonControl);
            CreateMasterCustomerId();
            var landingPageAssignContent = new LandingPageAssignContent()
            {
                LPOID = Two
            };
            var landingPageAssignContentList = new List<LandingPageAssignContent>
            {
                landingPageAssignContent
            };
            DataLayerFakes.ShimLandingPageAssignContent.GetListSqlCommand = (x) => landingPageAssignContentList;
            var landingPageAssign = new LandingPageAssign()
            {
                LPID = One,
                CustomerCanOverride = true,
                LPAID = param,
                CustomerDoesOverride = true
            };
            DataLayerFakes.ShimLandingPageAssign.GetSqlCommand = (x) => landingPageAssign;

            //Act
            ReflectionHelper.ExecuteMethod(_customer, MethodLoadData);

            // Assert
            if(param == One)
            {
                AssertPanel(PnlReasonDropDown, false);
            }
            else
            {
                AssertPanel(PnlReasonDropDown, true);
            }
            AssertTextBox(TxtMainLabel, string.Empty);
        }

        private void ShimPageLoad()
        {
            CreateCustomerUnsubscribe();
            InitializeControls(_customer);
            SetupForBtnSaveClick(ThankMessage, ReasonControl);
            CreateMasterCustomerId();
            KM.Platform.Fakes.ShimUser.IsAdministratorUser = _ => true;
            var landingPageAssign = new LandingPageAssign()
            {
                LPID = One,
                CustomerCanOverride = true,
                LPAID = One,
                CustomerDoesOverride = true
            };
            var landingPageList = new List<LandingPageAssign> { landingPageAssign };
            DataLayerFakes.ShimLandingPageAssign.GetListSqlCommand = (x) => landingPageList;
            DataLayerFakes.ShimLandingPageAssign.GetSqlCommand = (x) => landingPageAssign;
            var landingPageAssignContent = new LandingPageAssignContent()
            {
                LPOID = Six
            };
            var landingPageAssignContent1 = new LandingPageAssignContent()
            {
                LPOID = Seven
            };
            var landingPageAssignContent2 = new LandingPageAssignContent()
            {
                LPOID = One
            };
            var landingPageAssignContent3 = new LandingPageAssignContent()
            {
                LPOID = Four
            };
            var landingPageAssignContent4 = new LandingPageAssignContent()
            {
                LPOID = Five
            };
            var landingPageAssignContent5 = new LandingPageAssignContent()
            {
                LPOID = Twelve
            };
            var landingPageAssignContent6 = new LandingPageAssignContent()
            {
                LPOID = Thirteen
            };
            var landingPageAssignContent7 = new LandingPageAssignContent()
            {
                LPOID = Three
            };
            var landingPageAssignContentList = new List<LandingPageAssignContent>
            {
                landingPageAssignContent,
                landingPageAssignContent1,
                landingPageAssignContent2,
                landingPageAssignContent3,
                landingPageAssignContent4,
                landingPageAssignContent5,
                landingPageAssignContent6,
                landingPageAssignContent7
            };
            DataLayerFakes.ShimLandingPageAssignContent.GetListSqlCommand = (x) => landingPageAssignContentList;
            var emptyList = new List<LandingPageAssignContent>();
            DataLayerFakes.ShimLandingPageAssignContent.GetByLPOIDInt32Int32 = (x, y) => emptyList;
            ShimBaseDataBoundControl.AllInstances.DataBind = _ => { };
            ShimLandingPageAssign.GetPreviewParametersInt32Int32 = (x, y) => new DataTable();
        }

        private void CreateMasterCustomerId()
        {
            var shimMaster = new CommunicatorMasterFakes::ShimCommunicator();
            ShimCustomerUnsubscribe.AllInstances.MasterGet = (obj) => shimMaster.Instance;
            ShimPage.AllInstances.MasterGet = (obj) => shimMaster.Instance;
            var shimECNSession = new ShimECNSession();
            var fieldCurrentCustomer = typeof(ECNSession).GetField(CurrentCustomer);
            if (fieldCurrentCustomer == null)
            {
                throw new InvalidOperationException($"Field fieldCurrentCustomer should not be null");
            }
            fieldCurrentCustomer.SetValue(shimECNSession.Instance, new Customer
            {
                CustomerID = One
            });
            var fieldCurrentBaseChannel = typeof(ECNSession).GetField(CurrentBaseChannel);
            if (fieldCurrentBaseChannel == null)
            {
                throw new InvalidOperationException($"Field fieldCurrentBaseChannel should not be null");
            }
            fieldCurrentBaseChannel.SetValue(shimECNSession.Instance, new BaseChannel
            {
                BaseChannelID = One
            });
            CommunicatorMasterFakes::ShimCommunicator.AllInstances.UserSessionGet = (obj) => shimECNSession.Instance;
            ShimHttpContext.CurrentGet = () =>
            {
                var context = new HttpContext(new HttpRequest(null, TempURL, null), new HttpResponse(null));
                return context;
            };
        }

        private void AssertLabel(string labelName, string text)
        {
            var label = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, labelName)
                       .GetValue(_customer) as Label;
            label.ShouldSatisfyAllConditions(
                () => label.ShouldNotBeNull(),
                () => label.Text.ShouldBe(text));
        }

        private void AssertPanel(string panelName, bool visible)
        {
            var panel = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, panelName)
                .GetValue(_customer) as Panel;
            panel.ShouldSatisfyAllConditions(
                () => panel.ShouldNotBeNull(),
                () => panel.Visible.ShouldBe(visible));
        }

        private static DataTable CreateDataTable()
        {
            var datatable = new DataTable();
            datatable.Columns.Add(new DataColumn(RowNumber, typeof(string)));
            datatable.Columns.Add(new DataColumn(Column1, typeof(string)));
            datatable.Columns.Add(new DataColumn(Column2, typeof(string)));
            datatable.Columns.Add(new DataColumn(Column3, typeof(string)));
            datatable.Columns.Add(new DataColumn(Column4, typeof(string)));
            datatable.Columns.Add(new DataColumn(Column5, typeof(string)));
            var dataRow = datatable.NewRow();
            dataRow[RowNumber] = One;
            dataRow[Column1] = StringOne;
            dataRow[Column2] = StringOne;
            dataRow[Column3] = StringOne;
            dataRow[Column4] = string.Empty;
            dataRow[Column5] = string.Empty;
            datatable.Rows.Add(dataRow);
            return datatable;
        }
    }
}
