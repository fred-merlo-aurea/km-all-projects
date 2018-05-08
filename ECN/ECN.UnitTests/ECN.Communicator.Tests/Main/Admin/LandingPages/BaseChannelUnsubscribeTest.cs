using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
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
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;
using CommunicatorMasterFakes = ecn.communicator.MasterPages.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Accounts.Fakes;
using EntityAccountFakes = ECN_Framework_Entities.Accounts.Fakes;

namespace ECN.Communicator.Tests.Main.Admin.LandingPages
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BaseChannelUnsubscribeTest
    {
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
        private const int DummyUserID = -1;
        private const int DummyBaseChannelID = -2;
        private const string Yes = "Yes";
        private const string NonEmptyText = "Non Empty Text";
        private const string ECNButtonMedium = "ECN-Button-Medium";
        private const string BlastId = "1";
        private const string EmailId = "1";
        private const string ReasonDataTableName = "dtReason";
        private const string FieldReasonName = "Reason";
        private const string FieldReasonID = "ID";
        private const string FieldReasonIsDeleted = "IsDeleted";
        private const string FieldReasonSortOrder = "SortOrder";
        private const string FieldRblReasonControlType = "rblReasonControlType";
        private const string EditReasonCommandName = "EditReason";
        private const string DeleteReasonCommandName = "DeleteReason";
        private const string ReasonCommandArgument = "ReasonID";
        private const int ReasonSortOrder = 4;
        private const int LandingPageID = 1;
        private const string LabelUrlWarning = "Something is wrong with the  unsubscribe page. \n Please ensure that the customer has sent blasts to at least one group.";
        private const string TextBoxValue = "Text Box";
        private const string Error = "error";
        private const string Key = "key";
        private const string MethodPageLoad = "Page_Load";
        private const string PanelAccess = "pnlNoAccess";
        private const string PanelSettings = "pnlSettings";
        private const string MethodButtonPreviewClick = "btnPreview_Click";
        private const string LabelChannel = "lblBaseChannelOverride";
        private const string LabelCustomer = "lblCustomerOverride";
        private const string LabelChannelValue = "Note: You must override the default landing page settings for your saved changes to take effect.";
        private const string LabelCustomerValue = "Note: If any Customer overrides the Basechannel settings they may see different results.";
        private const string StringTen = "10";
        private const string MethodCustomerSelectedIndex = "ddlCustomer_SelectedIndexChanged";
        private const string LabelUrlWarningText = "lblUrlWarning";
        private const string MethodException = "throwECNException";
        private const string LabelErrorMessage = "lblErrorMessage";
        private const string LabelErrorMessageValue = "<br/>LandingPage: error";
        private const string MethodReasonControlType = "rblReasonControlType_SelectedIndexChanged";
        private const string PanelDropDown = "pnlReasonDropDown";
        private const string Reason = "Reason";
        private const string Id = "ID";
        private const string SortOrder = "SortOrder";
        private const string IsDeleted = "IsDeleted";
        private const string TextNewReason = "txtNewReason";
        private const string EditReason = "EditReason";
        private const string DeleteReason = "DeleteReason";
        private const string Argument = "args";
        private const string MethodReasonDropDown = "gvReasonDropDown_RowCommand";
        private const string TextLabelEdit = "txtReasonLabelEdit";
        private const string Test = "test";
        private const string TestDummy = "";
        private const string MethodAddNewReason = "btnAddNewReason_Click";
        private const string TestNo = "no";
        private const string TestYes = "yes";
        private const string TestDropDown = "drop down";
        private const string MethodVisibilityReason = "rblVisibilityReason_SelectedIndexChanged";
        private const string TextLabel = "txtReasonLabel";
        private const string MethodHtmlPreview = "btnHtmlPreview_Hide";
        private const string MethodReasonRowData = "gvReasonDropDown_RowDataBound";
        private const string DropDownReason = "dtReason";
        private const string RowNumber = "RowNumber";
        private const string Column1 = "Column1";
        private const string Column2 = "Column2";
        private const string Column3 = "Column3";
        private const string Column4 = "Column4";
        private const string Column5 = "Column5";
        private const string DdlCustomer = "ddlCustomer";
        private const string ModalPopupHtmlPreview = "modalPopupHtmlPreview";
        private const string MpeEditReason = "mpeEditReason";
        private const string Label = "Label1";
        private const string BtnHtmlPreviewShow = "btnHtmlPreviewShow";
        private const string BtnSaveReason = "btnSaveReason";
        private const string StringOne = "1";
        private const int Zero = 0;
        private const int One = 1;
        private const int Ten = 10;
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
        private const string BaseChannelMainAspx = "BaseChannelMain.aspx";
        private const string TxtPageLabel = "txtPageLabel";
        private const string TxtMainLabel = "txtMainLabel";
        private const string TxtMasterSuppressionLabel = "txtMasterSuppressionLabel";
        private const string TxtThankYouMessage = "txtThankYouMessage";
        private const string TxtRedirectUrl = "txtRedirectURL";
        private const string TxtReasonLabel = "txtReasonLabel";
        private const string TxtUnsubscribeText = "txtUnsubscribeText";
        private const string CurrentUser = "CurrentUser";
        private const string TempURL = "http://www.tempuri.org";
        private const string DdlRedirectDelay = "ddlRedirectDelay";
        private const string dtReasonName = "dtReason";
        private const string LblErrorMessage = "lblErrorMessage";
        private bool _lpaSaved = false;
        private IDisposable _shimObject;
        private BaseChannelUnsubscribe _channel;
        private CommunicatorMasterFakes::ShimCommunicator _shimMaster;
        private DataTable _dtReason;
        private Mock<HttpResponseBase> _response;

        private void InitializePageAndControls()
        {
            ReflectionHelper.SetValue(_channel, "rblOverrideDefaultSettings", new RadioButtonList());
            ReflectionHelper.SetValue(_channel, "rblAllowCustomerOverrideSettings", new RadioButtonList());
            ReflectionHelper.SetValue(_channel, "rblVisibilityPageLabel", new RadioButtonList());
            ReflectionHelper.SetValue(_channel, "rblVisibilityMainLabel", new RadioButtonList());
            ReflectionHelper.SetValue(_channel, "rblVisibilityMasterSuppression", new RadioButtonList());
            ReflectionHelper.SetValue(_channel, "rblRedirectThankYou", new RadioButtonList());
            ReflectionHelper.SetValue(_channel, "rblVisibilityReason", new RadioButtonList());
            ReflectionHelper.SetValue(_channel, "rblReasonControlType", new RadioButtonList());

            ReflectionHelper.SetValue(_channel, "txtHeader", new TextBox());
            ReflectionHelper.SetValue(_channel, "txtFooter", new TextBox());
            ReflectionHelper.SetValue(_channel, "txtPageLabel", new TextBox());
            ReflectionHelper.SetValue(_channel, "txtMainLabel", new TextBox());
            ReflectionHelper.SetValue(_channel, "txtMasterSuppressionLabel", new TextBox());
            ReflectionHelper.SetValue(_channel, "txtUnsubscribeText", new TextBox());
            ReflectionHelper.SetValue(_channel, "txtThankYouMessage", new TextBox());
            ReflectionHelper.SetValue(_channel, "txtRedirectURL", new TextBox());
            ReflectionHelper.SetValue(_channel, "txtReasonLabel", new TextBox());
            ReflectionHelper.SetValue(_channel, TextNewReason, new TextBox());
            ReflectionHelper.SetValue(_channel, TextLabelEdit, new TextBox());

            ReflectionHelper.SetValue(_channel, "tblThankYou", new HtmlTable());
            ReflectionHelper.SetValue(_channel, "tblRedirect", new HtmlTable());
            ReflectionHelper.SetValue(_channel, "tblDelay", new HtmlTable());
            ReflectionHelper.SetValue(_channel, "tblReasonResponseType", new HtmlTable());
            ReflectionHelper.SetValue(_channel, "ddlRedirectDelay", new DropDownList());
            ReflectionHelper.SetValue(_channel, DdlCustomer, new DropDownList() { SelectedIndex = 1, SelectedValue = "1" });
            ReflectionHelper.SetValue(_channel, ModalPopupHtmlPreview, new ModalPopupExtender());
            ReflectionHelper.SetValue(_channel, MpeEditReason, new ModalPopupExtender());

            ReflectionHelper.SetValue(_channel, "pnlReasonDropDown", new Panel());
            ReflectionHelper.SetValue(_channel, "lblReasonLabel", new Label());
          
            ReflectionHelper.SetValue(_channel, Label, new Label());
            ReflectionHelper.SetValue(_channel, LabelChannel, new Label());
            ReflectionHelper.SetValue(_channel, LabelCustomer, new Label());
            ReflectionHelper.SetValue(_channel, LabelUrlWarningText, new Label());

            ReflectionHelper.SetValue(_channel, "btnPreview", new Button());
            ReflectionHelper.SetValue(_channel, BtnHtmlPreviewShow, new Button());
            ReflectionHelper.SetValue(_channel, BtnSaveReason, new Button());

            ReflectionHelper.SetValue(_channel, "rlReasonDropDown", new ReorderList());
            ReflectionHelper.SetValue(_channel, "pnlReasonDropDown", new Panel());
            ReflectionHelper.SetValue(_channel, PanelAccess, new Panel());
            ReflectionHelper.SetValue(_channel, PanelSettings, new Panel());
        }

        private BaseChannelUnsubscribe CreateChannel()
        {
            var master = new Mock<IMasterCommunicator>();
            master.Setup(x => x.GetUserID()).Returns(DummyUserID);
            master.Setup(x => x.GetBaseChannelID()).Returns(DummyBaseChannelID);
            master.Setup(x => x.GetCurrentUser()).Returns(new User());

            var landingPageAssign = new Mock<ILandingPageAssign>();
            landingPageAssign.Setup(x => x.Save(It.IsAny<LandingPageAssign>(), It.IsAny<User>()));
            LandingPageAssign lpa = null;
            landingPageAssign.Setup(x => x.GetByBaseChannelID(It.IsAny<int>(), It.IsAny<int>())).Returns(lpa);
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
            _response.Setup(x => x.Redirect("BaseChannelMain.aspx"));

            return new BaseChannelUnsubscribe(master.Object, landingPageAssign.Object, landingPageAssignContent.Object, _response.Object);
        }

        private void InitializeControls(
            string headerText = "",
            string footerText = "",
            string pageLabel = "",
            string unsubscribeText = "",
            string mainLabel = "",
            string masterSuppression = "",
            string reasonLabel = "")
        {
            ReflectionHelper.SetValue(_channel, "phError", new PlaceHolder() { Visible = true });
            ReflectionHelper.SetValue(_channel, "rblOverrideDefaultSettings", new RadioButtonList() { SelectedValue = string.Empty });
            ReflectionHelper.SetValue(_channel, "rblAllowCustomerOverrideSettings", new RadioButtonList() { SelectedValue = string.Empty });
            ReflectionHelper.SetValue(_channel, "txtHeader", new TextBox() { Text = headerText });
            ReflectionHelper.SetValue(_channel, "txtFooter", new TextBox() { Text = footerText });
            ReflectionHelper.SetValue(_channel, LblErrorMessage, new Label() { Text = NonEmptyText });

            ReflectionHelper.SetValue(_channel, RblVisibilityPageLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblVisibilityMainLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblVisibilityMasterSuppression, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblRedirectThankYou, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblVisibilityReason, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, TxtUnsubscribeText, new TextBox() { Text = unsubscribeText });
            ReflectionHelper.SetValue(_channel, TxtPageLabel, new TextBox() { Text = pageLabel });
            ReflectionHelper.SetValue(_channel, TxtMainLabel, new TextBox() { Text = mainLabel });
            ReflectionHelper.SetValue(_channel, TxtMasterSuppressionLabel, new TextBox() { Text = masterSuppression });
            ReflectionHelper.SetValue(_channel, TxtThankYouMessage, new TextBox());
            ReflectionHelper.SetValue(_channel, TxtRedirectUrl, new TextBox());
            ReflectionHelper.SetValue(_channel, TxtReasonLabel, new TextBox() { Text = reasonLabel });

            ReflectionHelper.SetValue(_channel, DdlRedirectDelay, new DropDownList());
        }

        private LandingPageAssign GetLandingPageAssign(int lpId)
        {
            return new LandingPageAssign
            {
                BaseChannelDoesOverride = true,
                CustomerCanOverride = true,
                LPAID = lpId
            };
        }

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            _shimMaster = new CommunicatorMasterFakes::ShimCommunicator();
            ShimLandingPageAssignContent.DeleteInt32User = (_, __) => { };
            ShimLandingPageAssignContent.SaveLandingPageAssignContentUser = (_, __) => { };
            ShimBaseChannelUnsubscribe.AllInstances.MasterGet = (obj) => _shimMaster.Instance;
            ShimPage.AllInstances.MasterGet = (obj) => _shimMaster.Instance;
            var shimECNSession = new ShimECNSession();
            var fieldCurrentCustomer = typeof(ECNSession).GetField("CurrentCustomer");
            var currCustomer = new Customer
            {
                BaseChannelID = 1
            };
            fieldCurrentCustomer.SetValue(shimECNSession.Instance, currCustomer);
            CommunicatorMasterFakes::ShimCommunicator.AllInstances.UserSessionGet = (obj) => shimECNSession.Instance;
            ShimHttpContext.CurrentGet = () =>
            {
                var context = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));
                return context;
            };
            _dtReason = new DataTable();
            _channel = CreateChannel();
        }

        [TearDown]
        public void TearDown()
        {
            _dtReason.Clear();
            _shimObject.Dispose();
        }

        [Test]
        public void ValidCodeSnippets_EmptyString_ReturnsTrueCodeSnippetWithNullMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _channel,
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
                _channel,
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
                _channel,
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
        public void ValidCodeSnippets_TwoMatchesButNotbasechannelnameORpagenameORgroupdescription_ReturnsFalseCodeSnippetWithErrorMessage()
        {
            // Arrange, Assert
            var codeSnippetError = ReflectionHelper.ExecuteMethod(
                _channel,
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
                _channel,
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
                _channel,
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
                _channel,
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
        public void LoadData_LandingPageAssignIsNull_FillDataTable()
        {
            // Arrange
            ShimLandingPageAssign.GetByBaseChannelIDInt32Int32 = (id, lpid) => null;
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_channel, ReasonDataTableName, _dtReason);

            // Act	
            ReflectionHelper.ExecuteMethod(_channel, "LoadData", new object[] { });

            // Assert
            AssertDefaultReasons();
        }

        [Test]
        public void LoadData_LandingPageAssignIsNullAndThrowsException_DataTableIsEmpty()
        {
            // Arrange
            ShimLandingPageAssign.GetByBaseChannelIDInt32Int32 = (id, lpid) => null;
            ReflectionHelper.SetValue(_channel, ReasonDataTableName, _dtReason);

            // Act	
            ReflectionHelper.ExecuteMethod(_channel, "LoadData", new object[] { });

            // Assert
            var actualResult = ReflectionHelper
                .GetFieldInfoFromInstanceTypeByName(typeof(BaseChannelUnsubscribe), ReasonDataTableName)
                .GetValue(null) as DataTable;
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows
                        .Cast<DataRow>()
                        .ShouldBeEmpty(),
                () => actualResult.Rows.Count.ShouldBe(0));
        }

        [Test]
        public void LoadData_LandingPageAssignIsNotNull_DataTableIsEmpty()
        {
            // Arrange
            InitializePageAndControls();
            ShimLandingPageAssign.GetByBaseChannelIDInt32Int32 = (id, lpid) => GetLandingPageAssign(1);
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_channel, ReasonDataTableName, _dtReason);
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (id) =>
            {
                return new List<LandingPageAssignContent>
                {
                    new LandingPageAssignContent
                    {
                        LPOID = 1,
                        Display = NonEmptyText
                    }
                };
            };

            // Act	
            ReflectionHelper.ExecuteMethod(_channel, "LoadData", new object[] { });

            // Assert
            var actualResult = ReflectionHelper
                .GetFieldInfoFromInstanceTypeByName(typeof(BaseChannelUnsubscribe), ReasonDataTableName)
                .GetValue(null) as DataTable;
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows
                        .Cast<DataRow>()
                        .ShouldBeEmpty(),
                () => actualResult.Rows.Count.ShouldBe(0));

            AssertTextBoxInitialized("txtMasterSuppressionLabel", true);
            AssertButtonVisibility("btnPreview");
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
            InitializePageAndControls();
            ShimLandingPageAssign.GetByBaseChannelIDInt32Int32 = (id, lpid) => GetLandingPageAssign(lpId);
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_channel, ReasonDataTableName, _dtReason);
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (id) =>
            {
                return new List<LandingPageAssignContent>
                {
                    new LandingPageAssignContent
                    {
                        LPOID = lpId,
                        Display = NonEmptyText
                    }
                };
            };

            // Act	
            ReflectionHelper.ExecuteMethod(_channel, "LoadData", new object[] { });

            // Assert
            var actualResult = ReflectionHelper
                .GetFieldInfoFromInstanceTypeByName(typeof(BaseChannelUnsubscribe), ReasonDataTableName)
                .GetValue(null) as DataTable;
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows
                        .Cast<DataRow>()
                        .ShouldBeEmpty(),
                () => actualResult.Rows.Count.ShouldBe(0));

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
            InitializePageAndControls();
            ShimLandingPageAssign.GetByBaseChannelIDInt32Int32 = (id, lpid) => GetLandingPageAssign(1);
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_channel, ReasonDataTableName, _dtReason);
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
            ReflectionHelper.ExecuteMethod(_channel, "LoadData", new object[] { });

            // Assert
            var actualResult = ReflectionHelper
                .GetFieldInfoFromInstanceTypeByName(typeof(BaseChannelUnsubscribe), ReasonDataTableName)
                .GetValue(null) as DataTable;
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows
                        .Cast<DataRow>()
                        .ShouldBeEmpty(),
                () => actualResult.Rows.Count.ShouldBe(0));

            AssertTableVisibility("tblDelay", isDelayTableVisible);
            AssertTableVisibility("tblRedirect", isRedirectTableVisible);
            AssertTableVisibility("tblThankYou", isThankYouTableVisible);
        }

        [Test]
        public void LoadData_GetByLPOIDThrowsException_DataTableIsEmpty()
        {
            // Arrange
            InitializePageAndControls();
            ShimLandingPageAssign.GetByBaseChannelIDInt32Int32 = (id, lpid) => GetLandingPageAssign(1);
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_channel, ReasonDataTableName, _dtReason);
            const int LPOID = 2;
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (id) =>
            {
                return new List<LandingPageAssignContent>
                {
                    new LandingPageAssignContent
                    {
                        LPOID = LPOID,
                        Display = "TestString"
                    }
                };
            };
            ShimLandingPageAssignContent.GetByLPOIDInt32Int32 = (lpid, lpoid) => throw new Exception();

            // Act	
            ReflectionHelper.ExecuteMethod(_channel, "LoadData", new object[] { });

            // Assert
            var actualResult = ReflectionHelper
                .GetFieldInfoFromInstanceTypeByName(typeof(BaseChannelUnsubscribe), ReasonDataTableName)
                .GetValue(null) as DataTable;
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows
                        .Cast<DataRow>()
                        .ShouldBeEmpty(),
                () => actualResult.Rows.Count.ShouldBe(0));
        }

        [Test]
        public void LoadData_GetByLPOIDReturnEmptyList_FillDataTable()
        {
            // Arrange
            InitializePageAndControls();
            ShimLandingPageAssign.GetByBaseChannelIDInt32Int32 = (id, lpid) => GetLandingPageAssign(1);
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_channel, ReasonDataTableName, _dtReason);
            const int LPOID = 3;
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (id) =>
            {
                return new List<LandingPageAssignContent>
                {
                    new LandingPageAssignContent
                    {
                        LPOID = LPOID,
                        Display = "TestString"
                    }
                };
            };
            ShimLandingPageAssignContent.GetByLPOIDInt32Int32 = (lpid, lpoid) =>
            {
                return new List<LandingPageAssignContent>();
            };

            // Act	
            ReflectionHelper.ExecuteMethod(_channel, "LoadData", new object[] { });

            // Assert
            AssertDefaultReasons();

            var reasonDropDown = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "rlReasonDropDown")
                .GetValue(_channel) as ReorderList;
            reasonDropDown.ShouldSatisfyAllConditions(
                () => reasonDropDown.ShouldNotBeNull(),
                () => reasonDropDown.DataSource.ShouldNotBeNull(),
                () => (reasonDropDown.DataSource as IList).ShouldNotBeNull(),
                () => (reasonDropDown.DataSource as IList).Count.ShouldBe(6));

            var reasonPanel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "pnlReasonDropDown")
                .GetValue(_channel) as Panel;
            reasonPanel.ShouldSatisfyAllConditions(
                () => reasonPanel.ShouldNotBeNull(),
                () => reasonPanel.Visible.ShouldBeTrue());
        }

        [Test]
        public void LoadData_GetByLPOIDReturnyListWithData_FillDataTable()
        {
            // Arrange
            InitializePageAndControls();
            ShimLandingPageAssign.GetByBaseChannelIDInt32Int32 = (id, lpid) => GetLandingPageAssign(1);
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_channel, ReasonDataTableName, _dtReason);
            const int LPOID = 3;
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (id) =>
            {
                return new List<LandingPageAssignContent>
                {
                    new LandingPageAssignContent
                    {
                        LPOID = LPOID,
                        Display = "TestString"
                    }
                };
            };
            ShimLandingPageAssignContent.GetByLPOIDInt32Int32 = (lpid, lpoid) =>
            {
                return new List<LandingPageAssignContent>
                {
                    new LandingPageAssignContent
                    {
                        Display = "TestDisplay",
                        LPACID = 1,
                        SortOrder = 1,
                        IsDeleted = false
                    }
                };
            };

            // Act	
            ReflectionHelper.ExecuteMethod(_channel, "LoadData", new object[] { });

            // Assert
            var actualResult = ReflectionHelper
                .GetFieldInfoFromInstanceTypeByName(typeof(BaseChannelUnsubscribe), ReasonDataTableName)
                .GetValue(null) as DataTable;
            actualResult.ShouldNotBeNull();
            actualResult.Rows.ShouldNotBeNull();
            actualResult.Rows.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void BtnSaveClick_NullLandingPageAssign_UpdateControlsAndRedirectToBaseChannelMainPage()
        {
            // Arrange
            InitializeControls(NonEmptyText, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_channel, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "LPA").GetValue(_channel) as LandingPageAssign;
            landingPageAssign.ShouldNotBeNull();
            landingPageAssign.CreatedUserID.ShouldBe(DummyUserID);
            landingPageAssign.BaseChannelID.HasValue.ShouldBeTrue();
            landingPageAssign.BaseChannelID.Value.ShouldBe(DummyBaseChannelID);
            landingPageAssign.LPID.ShouldBe(LandingPageID);
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldNotBeNull();
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldBeFalse();
            landingPageAssign.CustomerCanOverride.Value.ShouldNotBeNull();
            landingPageAssign.CustomerCanOverride.Value.ShouldBeFalse();
            landingPageAssign.Header.ShouldBe(NonEmptyText);
            landingPageAssign.Footer.ShouldBe(NonEmptyText);
            landingPageAssign.UpdatedUserID.ShouldBe(DummyUserID);

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "lblErrorMessage").GetValue(_channel) as Label;
            errorMessageLabel.ShouldNotBeNull();
            errorMessageLabel.Text.ShouldBe(NonEmptyText);

            _response.Verify(x => x.Redirect("BaseChannelMain.aspx"), Times.Once());
        }

        [Test]
        public void BtnSaveClick_InvalidCodeSnippetsForHeaderText_UpdateControlsAndRedirectToBaseChannelMainPage()
        {
            // Arrange
            InitializeControls(InvalidCodeSnippet, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_channel, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "LPA").GetValue(_channel) as LandingPageAssign;
            landingPageAssign.ShouldNotBeNull();
            landingPageAssign.LPID.ShouldBe(LandingPageID);
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldNotBeNull();
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldBeFalse();
            landingPageAssign.CustomerCanOverride.Value.ShouldNotBeNull();
            landingPageAssign.CustomerCanOverride.Value.ShouldBeFalse();
            landingPageAssign.Header.ShouldBeEmpty();
            landingPageAssign.Footer.ShouldBeEmpty();
            landingPageAssign.UpdatedUserID.ShouldBeNull();

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "lblErrorMessage").GetValue(_channel) as Label;
            errorMessageLabel.ShouldNotBeNull();
            errorMessageLabel.Text.ShouldBe("<br/>LandingPage: There is a badly formed codesnippet in Header");

            _response.Verify(x => x.Redirect("BaseChannelMain.aspx"), Times.Never());
        }

        [Test]
        public void BtnSaveClick_InvalidCodeSnippetsForFooterText_UpdateControlsAndRedirectToBaseChannelMainPage()
        {
            // Arrange
            InitializeControls(NonEmptyText, InvalidCodeSnippet);

            // Act
            ReflectionHelper.ExecuteMethod(_channel, "btnSave_Click", new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "LPA").GetValue(_channel) as LandingPageAssign;
            landingPageAssign.ShouldNotBeNull();
            landingPageAssign.LPID.ShouldBe(LandingPageID);
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldNotBeNull();
            landingPageAssign.BaseChannelDoesOverride.Value.ShouldBeFalse();
            landingPageAssign.CustomerCanOverride.Value.ShouldNotBeNull();
            landingPageAssign.CustomerCanOverride.Value.ShouldBeFalse();
            landingPageAssign.Header.ShouldBeEmpty();
            landingPageAssign.Footer.ShouldBeEmpty();
            landingPageAssign.UpdatedUserID.ShouldBeNull();

            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, "lblErrorMessage").GetValue(_channel) as Label;
            errorMessageLabel.ShouldNotBeNull();
            errorMessageLabel.Text.ShouldBe("<br/>LandingPage: There is a badly formed codesnippet in Footer");

            _response.Verify(x => x.Redirect("BaseChannelMain.aspx"), Times.Never());
        }

        [Test]
        [TestCase(ThankYou, TextBoxLabel)]
        [TestCase(Redirect, NonEmptyText)]
        [TestCase(Both, NonEmptyText)]
        public void BtnSaveClick_OnValidCodeSnippets_SaveLandingPageAssignContentAndRedirectToBaseChannelMainPage(
            string redirectThankYou,
            string reasonControlType)
        {
            // Arrange
            InitializeControls(NonEmptyText, EmptyCodeSnippet);
            SetupForBtnSaveClick(redirectThankYou, reasonControlType);
            ReflectionHelper.SetValue(_channel, TxtRedirectUrl, new TextBox { Text = TempURL });

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LPAFieldName)
                .GetValue(_channel) as LandingPageAssign;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.BaseChannelDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(EmptyCodeSnippet),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.BaseChannelID.ShouldBe(DummyBaseChannelID),
                () => _lpaSaved.ShouldBeTrue(),
                () => _response.Verify(x => x.Redirect(BaseChannelMainAspx), Times.Once()));
        }

        [Test]
        public void BtnSaveClick_OnInValidCodeSnippets_ThrowECNException()
        {
            // Arrange
            InitializeControls(
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                InvalidCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);
            ReflectionHelper.SetValue(_channel, dtReasonName, new DataTable());

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LPAFieldName)
                .GetValue(_channel) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LblErrorMessage)
                .GetValue(_channel) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain($"{ErrorBadFormattedCodeSnippet} in Reason label"),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.BaseChannelDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(string.Empty),
                () => landingPageAssign.Footer.ShouldBe(string.Empty),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.BaseChannelID.ShouldBe(DummyBaseChannelID));
        }

        [Test]
        public void BtnSaveClick_VisibilityPageLabel_ThrowECNException()
        {
            // Arrange
            InitializeControls(NonEmptyText, string.Empty, InvalidCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LPAFieldName)
                .GetValue(_channel) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LblErrorMessage)
                .GetValue(_channel) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain($"{ErrorBadFormattedCodeSnippet} in Page Label"),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.BaseChannelDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(string.Empty),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.BaseChannelID.ShouldBe(DummyBaseChannelID),
                () => _lpaSaved.ShouldBeFalse()
            );
        }

        [Test]
        public void BtnSaveClick_VisibilityMainLabel_ThrowECNException()
        {
            // Arrange
            InitializeControls(NonEmptyText, string.Empty, string.Empty, string.Empty, InvalidCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);
            ReflectionHelper.SetValue(_channel, RblVisibilityPageLabel, new RadioButtonList());

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LPAFieldName)
                .GetValue(_channel) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LblErrorMessage)
                .GetValue(_channel) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain($"{ErrorBadFormattedCodeSnippet} in Main Label"),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.BaseChannelDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(string.Empty),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.BaseChannelID.ShouldBe(DummyBaseChannelID),
                () => _lpaSaved.ShouldBeFalse());
        }

        [Test]
        public void BtnSaveClick_VisibilityMasterSuppression_ThrowECNException()
        {
            // Arrange
            InitializeControls(NonEmptyText, NonEmptyText, string.Empty, string.Empty, string.Empty, InvalidCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);
            ReflectionHelper.SetValue(_channel, RblVisibilityPageLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblVisibilityMainLabel, new RadioButtonList());

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LPAFieldName)
                .GetValue(_channel) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LblErrorMessage)
                .GetValue(_channel) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain($"{ErrorBadFormattedCodeSnippet} in Master Suppression"),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.BaseChannelDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(NonEmptyText),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.BaseChannelID.ShouldBe(DummyBaseChannelID),
                () => _lpaSaved.ShouldBeFalse());
        }

        [Test]
        public void BtnSaveClick_UnsubscribeText_ThrowECNException()
        {
            // Arrange
            InitializeControls(NonEmptyText, EmptyCodeSnippet, string.Empty, InvalidCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);
            ReflectionHelper.SetValue(_channel, RblVisibilityPageLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblVisibilityMainLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblVisibilityMasterSuppression, new RadioButtonList());

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LPAFieldName)
                .GetValue(_channel) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LblErrorMessage)
                .GetValue(_channel) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain($"{ErrorBadFormattedCodeSnippet} in Unsubscribe Text"),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.BaseChannelDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(EmptyCodeSnippet),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.BaseChannelID.ShouldBe(DummyBaseChannelID),
                () => _lpaSaved.ShouldBeFalse());
        }

        [Test]
        [TestCase(ThankYou, InvalidCodeSnippet, ErrorBadFormattedCodeSnippetInThankYou)]
        [TestCase(Redirect, NonEmptyText, ErrorInvalidUrlForRedirect)]
        [TestCase(Both, InvalidCodeSnippet, ErrorBadFormattedCodeSnippetInThankYou)]
        [TestCase(Both, TxtThankYouMessage, ErrorInvalidUrlForRedirect)]
        public void BtnSaveClick_RedirectThankYou_ThrowECNException(string redirectThankYou, string thankYouMessage, string expectedErrorMessage)
        {
            // Arrange
            InitializeControls(NonEmptyText);
            SetupForBtnSaveClick(redirectThankYou, TextBoxLabel);
            ReflectionHelper.SetValue(_channel, TxtUnsubscribeText, new TextBox { Text = TxtUnsubscribeText });
            ReflectionHelper.SetValue(_channel, RblVisibilityPageLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblVisibilityMainLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblVisibilityMasterSuppression, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, TxtRedirectUrl, new TextBox());
            ReflectionHelper.SetValue(_channel, TxtThankYouMessage, new TextBox { Text = thankYouMessage });

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LPAFieldName)
                .GetValue(_channel) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LblErrorMessage)
                .GetValue(_channel) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain(expectedErrorMessage),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.BaseChannelDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(string.Empty),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.BaseChannelID.ShouldBe(DummyBaseChannelID));
        }

        [Test]
        public void BtnSaveClick_VisibilityReason_ThrowECNException()
        {
            // Arrange
            InitializeControls(NonEmptyText, EmptyCodeSnippet);
            SetupForBtnSaveClick(ThankYou, NonEmptyText);
            var dtReason = new DataTable();
            dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            var row = dtReason.NewRow();
            row[0] = bool.TrueString.ToLower();
            dtReason.Rows.Add(row);
            ReflectionHelper.SetValue(_channel, dtReasonName, dtReason);
            const string ExpectedErrorMessage = "Please enter at least one Reason";
            ReflectionHelper.SetValue(_channel, RblVisibilityPageLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblVisibilityMainLabel, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblVisibilityMasterSuppression, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, RblRedirectThankYou, new RadioButtonList());
            ReflectionHelper.SetValue(_channel, TxtUnsubscribeText, new TextBox { Text = TxtUnsubscribeText });
            ReflectionHelper.SetValue(_channel, TxtReasonLabel, new TextBox { Text = TxtReasonLabel });

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodBtnSaveClick, new object[] { null, null });

            // Assert
            var landingPageAssign = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LPAFieldName)
                .GetValue(_channel) as LandingPageAssign;
            var errorMessageLabel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, LblErrorMessage)
                .GetValue(_channel) as Label;

            landingPageAssign.ShouldSatisfyAllConditions(
                () => errorMessageLabel.ShouldNotBeNull(),
                () => errorMessageLabel.Text.ShouldContain(ExpectedErrorMessage),
                () => landingPageAssign.ShouldNotBeNull(),
                () => landingPageAssign.LPID.ShouldBe(1),
                () => landingPageAssign.BaseChannelDoesOverride.ShouldBe(false),
                () => landingPageAssign.Header.ShouldBe(NonEmptyText),
                () => landingPageAssign.Footer.ShouldBe(EmptyCodeSnippet),
                () => landingPageAssign.CreatedUserID.ShouldBe(DummyUserID),
                () => landingPageAssign.BaseChannelID.ShouldBe(DummyBaseChannelID));
        }

        private void SetupForBtnSaveClick(string redirectThankYou, string reasonControlType)
        {
            _lpaSaved = false;
            ReflectionHelper.SetValue(_channel, LPAFieldName, new LandingPageAssign());
            ReflectionHelper.SetValue(_channel, RblVisibilityPageLabel, GetRadioButtonList(Yes));
            ReflectionHelper.SetValue(_channel, RblVisibilityMainLabel, GetRadioButtonList(Yes));
            ReflectionHelper.SetValue(_channel, RblVisibilityMasterSuppression, GetRadioButtonList(Yes));
            ReflectionHelper.SetValue(_channel, RblRedirectThankYou, GetRadioButtonList(redirectThankYou));
            ReflectionHelper.SetValue(_channel, RblVisibilityReason, GetRadioButtonList(Yes));
            ReflectionHelper.SetValue(_channel, RblReasonControlType, GetRadioButtonList(reasonControlType));
            ReflectionHelper.SetValue(_channel, dtReasonName, GetDtReason());

            var shimMaster = new CommunicatorMasterFakes::ShimCommunicator();
            ShimBaseChannelUnsubscribe.AllInstances.MasterGet = (obj) => shimMaster.Instance;
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

        private static DataTable GetDtReason()
        {
            var dtReason = new DataTable();
            dtReason.Columns.Add("Reason", typeof(string));
            dtReason.Columns.Add("ID", typeof(string));
            dtReason.Columns.Add("SortOrder", typeof(int));
            dtReason.Columns.Add("IsDeleted", typeof(bool));
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

        [TestCase("thankyou", false, false, true)]
        [TestCase("redirect", false, true, false)]
        [TestCase("both", true, true, true)]
        [TestCase("neither", false, false, false)]
        public void RblRedirectThankYouSelectedIndexChanged_SelectedValue_UpdateControlVisibility(
            string selectedValue,
            bool isDelayTableVisible,
            bool isRedirectTableVisible,
            bool isThankYouTableVisible)
        {
            // Arrange
            InitializePageAndControls();
            InitializeRedirectRadioAndDropDownList(selectedValue);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_channel, "rblRedirectThankYou_SelectedIndexChanged", parameters);

            // Assert
            AssertTableVisibility("tblDelay", isDelayTableVisible);
            AssertTableVisibility("tblRedirect", isRedirectTableVisible);
            AssertTableVisibility("tblThankYou", isThankYouTableVisible);

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
            InitializePageAndControls();
            InitializeRadioButton("rblVisibilityMainLabel", radioButtonSelectedValue);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_channel, "rblVisibilityMainLabel_SelectedIndexChanged", parameters);

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
            InitializePageAndControls();
            InitializeRadioButton("rblVisibilityMasterSuppression", radioButtonSelectedValue);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_channel, "rblVisibilityMasterSuppression_SelectedIndexChanged", parameters);

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
            InitializePageAndControls();
            InitializeRadioButton("rblVisibilityPageLabel", radioButtonSelectedValue);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_channel, "rblVisibilityPageLabel_SelectedIndexChanged", parameters);

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
            InitializePageAndControls();
            InitializeRadioButton("rblVisibilityReason", radioButtonSelectedValue);
            InitializeRadioButton("rblReasonControlType", radioButtonSelectedValue);
            
            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_channel, "rblVisibilityReason_SelectedIndexChanged", parameters);

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
            ReflectionHelper.ExecuteMethod(_channel, "rlReasonDropDown_ItemCommand", parameters);

            // Assert
            AssertTextBox("txtReasonLabelEdit", FieldReasonName);
            AssertButtonCommand("btnSaveReason", ReasonCommandArgument);
        }

        [Test]
        public void RlReasonDropDownItemCommand_DeleteReasonCommand_SetsIsDeletedAndDecrementSortOrderAndCallsLoadReasonData()
        {
            // Arrange
            InitializePageAndControls();
            InitializeReasonTable(ReasonCommandArgument);

            // Act
            var reorderListCommandEventArgs = ReflectionHelper.CreateInstance<ReorderListCommandEventArgs>(
                new object[] { new CommandEventArgs(DeleteReasonCommandName, ReasonCommandArgument), this, null });
            var parameters = new object[] { null, reorderListCommandEventArgs };
            ReflectionHelper.ExecuteMethod(_channel, "rlReasonDropDown_ItemCommand", parameters);

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
            InitializePageAndControls();
            InitializeReasonTable(ReasonCommandArgument);
            ShimReorderList.AllInstances.PerformDataBindingIEnumerable = (_, __) => { };

            // Act
            var reorderListItemReorderEventArgs = ReflectionHelper.CreateInstance<ReorderListItemReorderEventArgs>(
                new object[] { null, oldIndex, newIndex });
            var parameters = new object[] { null, reorderListItemReorderEventArgs };
            ReflectionHelper.ExecuteMethod(_channel, "rlReasonDropDown_ItemReorder", parameters);

            // Assert
            AssertDataTableDataLength($"{FieldReasonSortOrder} = {expectedResult}", 2);
        }

        [Test]
        public void RblReasonControlTypeSelectedIndexChanged_ReasonControlTypeNotTextBox_FillDataTableWithDefaultReasons()
        {
            // Arrange
            InitializePageAndControls();
            InitializeReasonColumns();
            ReflectionHelper.SetValue(_channel, ReasonDataTableName, _dtReason);
            ReflectionHelper.SetValue(_channel, FieldRblReasonControlType, GetRadioButtonList(string.Empty));
            ShimReorderList.AllInstances.PerformDataBindingIEnumerable = (_, __) => { };

            // Act
            var parameters = new object[] { null, EventArgs.Empty };
            ReflectionHelper.ExecuteMethod(_channel, "rblReasonControlType_SelectedIndexChanged", parameters);

            // Assert
            AssertDefaultReasons();
        }

        private void AssertButtonVisibility(string buttonName)
        {
            var button = ReflectionHelper
                .GetFieldInfoFromInstanceTypeByName(typeof(BaseChannelUnsubscribe), buttonName)
                .GetValue(_channel) as Button;
            button.ShouldSatisfyAllConditions(
                () => button.ShouldNotBeNull(),
                () => button.Enabled.ShouldBeTrue(),
                () => button.Visible.ShouldBeTrue());
        }

        private RadioButtonList GetRadioButtonList(string itemValue)
        {
            var rbList = new RadioButtonList();
            rbList.Items.Add(new ListItem(NonEmptyText, itemValue));
            rbList.SelectedIndex = 0;
            return rbList;
        }

        private void InitializeReasonColumns()
        {
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
        }

        private void AssertDefaultReasons()
        {
            var actualResult = ReflectionHelper
                .GetFieldInfoFromInstanceTypeByName(typeof(BaseChannelUnsubscribe), ReasonDataTableName)
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
            var dtReason = ReflectionHelper.GetFieldInfoFromInstanceTypeByName(typeof(BaseChannelUnsubscribe), ReasonDataTableName)
                .GetValue(null) as DataTable;
            dtReason.ShouldNotBeNull();

            var columnData = dtReason.Select(columnfilter);
            columnData.ShouldSatisfyAllConditions(
                () => columnData.ShouldNotBeNull(),
                () => columnData.Length.ShouldBe(dataCount));
        }

        private void AssertButtonCommand(string buttonName, string commandArgument)
        {
            var button = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, buttonName)
                       .GetValue(_channel) as Button;
            button.ShouldSatisfyAllConditions(
                () => button.ShouldNotBeNull(),
                () => button.CommandArgument.ShouldBe(commandArgument));
        }

        private void InitializeReasonTable(string reasonID)
        {
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            AddNewRow(ReasonSortOrder);
            AddNewRow(ReasonSortOrder + 1);
            ReflectionHelper.SetValue(_channel, ReasonDataTableName, _dtReason);
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
            ReflectionHelper.SetValue(_channel, "txtReasonLabelEdit", new TextBox() { Text = reasonText });
            ReflectionHelper.SetValue(_channel, "btnSaveReason", new Button() { CommandArgument = commandArgument });
            ReflectionHelper.SetValue(_channel, "mpeEditReason", new ModalPopupExtender());
        }

        private void AssertRadioButtonVisibility(string radioButtonName, bool expectedTextBoxVisibility)
        {
            var radioButton = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, radioButtonName)
                       .GetValue(_channel) as RadioButtonList;
            radioButton.ShouldSatisfyAllConditions(
                () => radioButton.ShouldNotBeNull(),
                () => radioButton.Visible.ShouldBe(expectedTextBoxVisibility));
        }

        private void AssertPanelVisibility(string panelName, bool expectedTextBoxVisibility)
        {
            var panel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, panelName)
                       .GetValue(_channel) as Panel;
            panel.ShouldSatisfyAllConditions(
                () => panel.ShouldNotBeNull(),
                () => panel.Visible.ShouldBe(expectedTextBoxVisibility));
        }

        private void AssertTextBoxInitialized(string textboxName, bool expectedTextBoxVisibility, string initialText = "")
        {
            var textBox = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, textboxName)
                       .GetValue(_channel) as TextBox;
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
            ReflectionHelper.SetValue(_channel, radioButtonName, radioButtonList);
        }

        private void AssertDropDownListSelectedIndex(string dropDownListName, int selectedIndex)
        {
            var dropDownList = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, dropDownListName)
                       .GetValue(_channel) as DropDownList;
            dropDownList.ShouldSatisfyAllConditions(
                () => dropDownList.ShouldNotBeNull(),
                () => dropDownList.SelectedIndex.ShouldBe(selectedIndex));
        }

        private void AssertTextBox(string textBoxName, string text)
        {
            var textBox = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, textBoxName)
                       .GetValue(_channel) as TextBox;
            textBox.ShouldSatisfyAllConditions(
                () => textBox.ShouldNotBeNull(),
                () => textBox.Text.ShouldBe(text));
        }

        private void InitializeRedirectRadioAndDropDownList(string selectedValue)
        {
            InitializeRadioButton("rblRedirectThankYou", selectedValue);

            var dropDownList = new DropDownList();
            dropDownList.Items.Add(selectedValue);
            ReflectionHelper.SetValue(_channel, "ddlRedirectDelay", dropDownList);
        }

        private void SetRadioButtonListSelectedValue(string radioButtonListName, string selectedValue)
        {
            var radioButtonList = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, radioButtonListName)
                .GetValue(_channel) as RadioButtonList;

            radioButtonList.SelectedValue = selectedValue;
        }

        private void AssertTableVisibility(string tableName, bool isVisible)
        {
            var htmlTable = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, tableName)
                .GetValue(_channel) as HtmlTable;
            htmlTable.ShouldSatisfyAllConditions(
                () => htmlTable.ShouldNotBeNull(),
                () => htmlTable.Visible.ShouldBe(isVisible));
        }

        [Test]
        public void PageLoad_ForChannelAdministratorFalse_ShouldNotLoadData()
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
            ShimUser.IsChannelAdministratorUser = (x) => false;

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodPageLoad, new object[] { this, new EventArgs() });

            // Assert
            AssertPanel(PanelAccess, true);
            AssertPanel(PanelSettings, false);
        }

        private void AssertPanel(string panelName, bool visible)
        {
            var panel = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, panelName)
                .GetValue(_channel) as Panel;
            panel.ShouldSatisfyAllConditions(
                () => panel.ShouldNotBeNull(),
                () => panel.Visible.ShouldBe(visible));
        }

        [Test]
        public void ButtonPreviewClick_ForOverride_LoadCustomer()
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
            EntityAccountFakes.ShimCustomer.AllInstances.BaseChannelIDGet = (x) => Ten;
            var landingPageAssign = new LandingPageAssign();
            landingPageAssign.BaseChannelDoesOverride = false;
            landingPageAssign.CustomerCanOverride = true;
            DataLayerFakes.ShimLandingPageAssign.GetSqlCommand = (x) => landingPageAssign;
            var customer = new Customer();
            var customerList = new List<Customer> { customer };
            DataLayerFakes.ShimCustomer.GetListSqlCommandInt32 = (x, y) => customerList;
            ShimBaseDataBoundControl.AllInstances.DataBind = _ => { };

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodButtonPreviewClick, new object[] { this, new EventArgs() });

            // Assert
            AssertLabel(LabelChannel, LabelChannelValue);
            AssertLabel(LabelCustomer, LabelCustomerValue);
        }

        private void AssertLabel(string labelName, string text)
        {
            var label = ReflectionHelper.GetFieldInfoFromInstanceByName(_channel, labelName)
                       .GetValue(_channel) as Label;
            label.ShouldSatisfyAllConditions(
                () => label.ShouldNotBeNull(),
                () => label.Text.ShouldBe(text));
        }

        [Test]
        public void DdlCustomerSelectedIndexChanged_ForSelectedIndexNotZero_ChangeIndex()
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
            var landingPageAssign = new LandingPageAssign();
            landingPageAssign.BaseChannelDoesOverride = false;
            landingPageAssign.CustomerCanOverride = true;
            DataLayerFakes.ShimLandingPageAssign.GetSqlCommand = (x) => landingPageAssign;
            EntityAccountFakes.ShimLandingPageAssign.AllInstances.LPAIDGet = (x) => One;
            DataLayerFakes.ShimLandingPageAssign.GetPreviewParametersInt32Int32 = (x, y) => CreateDataTable();
            ShimListControl.AllInstances.SelectedValueGet = (x) => StringTen;

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodCustomerSelectedIndex, new object[] { this, new EventArgs() });

            // Assert
            AssertLabel(LabelUrlWarningText, string.Empty);
        }

        [Test]
        public void DdlCustomerSelectedIndexChanged_ForSelectedIndexNotZeroAndNullUrl_ChangeIndex()
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
            var landingPageAssign = new LandingPageAssign();
            landingPageAssign.BaseChannelDoesOverride = false;
            landingPageAssign.CustomerCanOverride = true;
            DataLayerFakes.ShimLandingPageAssign.GetSqlCommand = (x) => landingPageAssign;
            EntityAccountFakes.ShimLandingPageAssign.AllInstances.LPAIDGet = (x) => One;
            DataLayerFakes.ShimLandingPageAssign.GetPreviewParametersInt32Int32 = (x, y) => new DataTable();
            ShimListControl.AllInstances.SelectedValueGet = (x) => StringTen;

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodCustomerSelectedIndex, new object[] { this, new EventArgs() });

            // Assert
            AssertLabel(LabelUrlWarningText, LabelUrlWarning);
        }

        [Test]
        public void DdlCustomerSelectedIndexChanged_ForSelectedIndexZero_ChangeIndex()
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
            ShimDropDownList.AllInstances.SelectedIndexGet = (x) => Zero;

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodCustomerSelectedIndex, new object[] { this, new EventArgs() });

            // Assert
            AssertLabel(LabelUrlWarningText, string.Empty);
        }

        [Test]
        public void ThrowECNException_ForECNError_SetErrorMessage()
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodException, new object[] { Error });

            // Assert
            AssertLabel(LabelErrorMessage, LabelErrorMessageValue);
        }

        [Test]
        public void RblReasonControlType_SelectedIndexChanged_ForTextBox_SetVisibleFalse()
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
            ShimListControl.AllInstances.SelectedValueGet = (x) => TextBoxValue;

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodReasonControlType, new object[] { this, new EventArgs() });

            // Assert
            AssertPanel(PanelDropDown, false);
        }

        [Test]
        public void RblReasonControlType_SelectedIndexChanged_ForSelectedValue_SetVisibleFalse()
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
            ShimListControl.AllInstances.SelectedValueGet = (x) => string.Empty;
            _dtReason.Columns.Add(Reason, typeof(string));
            _dtReason.Columns.Add(Id, typeof(string));
            _dtReason.Columns.Add(SortOrder, typeof(int));
            _dtReason.Columns.Add(IsDeleted, typeof(bool));
            ReflectionHelper.SetValue(_channel, DropDownReason, _dtReason);
            ShimBaseDataBoundControl.AllInstances.DataBind = _ => { };

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodReasonControlType, new object[] { this, new EventArgs() });

            // Assert
            AssertTextBox(TextNewReason, string.Empty);
        }

        [Test]
        [TestCase(EditReason)]
        [TestCase(DeleteReason)]
        public void GvReasonDropDown_RowCommand_ForEditReason_DisplayEditReason(string param)
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
            var commandEventArgs = new CommandEventArgs(param, Argument);
            var gridViewCommandEventArgs = new GridViewCommandEventArgs(Argument, commandEventArgs);
            _dtReason.Columns.Add(Reason, typeof(string));
            _dtReason.Columns.Add(Id, typeof(string));
            _dtReason.Columns.Add(SortOrder, typeof(int));
            _dtReason.Columns.Add(IsDeleted, typeof(bool));
            var dataRow = _dtReason.NewRow();
            dataRow[Reason] = string.Empty;
            dataRow[Id] = Argument;
            dataRow[SortOrder] = 1;
            dataRow[IsDeleted] = false;
            _dtReason.Rows.Add(dataRow);
            ReflectionHelper.SetValue(_channel, DropDownReason, _dtReason);
            ShimBaseDataBoundControl.AllInstances.DataBind = _ => { };

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodReasonDropDown, new object[] { this, gridViewCommandEventArgs });

            // Assert
            AssertTextBox(TextLabelEdit, string.Empty);
        }

        [Test]
        [TestCase(Test)]
        [TestCase(TestDummy)]
        public void BtnAddNewReason_Click_ForNewReason_LoadGrid(string param)
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
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
            ReflectionHelper.SetValue(_channel, DropDownReason, _dtReason);
            ShimBaseDataBoundControl.AllInstances.DataBind = _ => { };
            ShimTextBox.AllInstances.TextGet = (x) => param;

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodAddNewReason, new object[] { this, new EventArgs() });

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
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
            ReflectionHelper.SetValue(_channel, DropDownReason, _dtReason);
            ShimListControl.AllInstances.SelectedValueGet = (x) => param;

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodVisibilityReason, new object[] { this, new EventArgs() });

            // Assert
            AssertTextBox(TextNewReason, string.Empty);
            AssertTextBox(TextLabel, string.Empty);
        }

        [Test]
        public void BtnHtmlPreview_Hide_ForBorderStyleNone_ShouldHidePreview()
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodHtmlPreview, new object[] { this, new EventArgs() });

            // Assert
            AssertLabel(LabelUrlWarningText, string.Empty);
        }

        [Test]
        public void GvReasonDropDown_RowDataBound()
        {
            // Arrange
            InitializePageAndControls();
            InitializeControls(NonEmptyText, NonEmptyText);
            var gridViewRow = new GridViewRow(Zero, Zero, DataControlRowType.DataRow, DataControlRowState.Alternate);
            gridViewRow.RowType = DataControlRowType.DataRow;
            var gridViewRowEventArgs = new GridViewRowEventArgs(gridViewRow);
            var keyList = new ArrayList() { Key };
            var dataKeyCollection = new DataKeyCollection(keyList);
            ShimReorderList.AllInstances.DataKeysGet = (x) => dataKeyCollection;
            var imageButton = new ImageButton();
            ShimControl.AllInstances.FindControlString = (x, y) => imageButton;

            // Act
            ReflectionHelper.ExecuteMethod(_channel, MethodReasonRowData, new object[] { this, gridViewRowEventArgs });

            // Assert
            imageButton.ShouldSatisfyAllConditions(
                () => imageButton.ShouldNotBeNull(),
                () => imageButton.CommandArgument.ShouldBe(Key));
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
