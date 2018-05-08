using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlClient.Fakes;
using System.Net.Mail;
using System.Web.Caching;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.activityengines.engines.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using AccountFakeDataLayer = ECN_Framework_DataLayer.Accounts.Fakes;
using CommunicatorFakeDataLayer = ECN_Framework_DataLayer.Communicator.Fakes;

namespace ecn.activityengines.Tests.Engines
{
    public partial class SubscriptionManagementTest
    {
        private const string BtnSubmitClickEmail = "email";
        private const string ButtonSubmitClickNoReasonSelectedErrorMsg = "Please select a reason from the list of available reasons";
        private const string ButtonSubmitClickNoReasonTextErrorMsg = "Please enter a reason in the text box.";
        private const int GroupId = 1;
        private string _buttonSubmitClickEmailContent;

        [Test]
        public void BtnSubmit_Click_NoSubscription_Error()
        {
            // Arrange
            InitTestForBtnSubmitClick();

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => _lblErrorMessage.Text.ShouldBe(ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink)),
                () => _logNonCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [Test]
        public void BtnSubmit_Click_SubscriptionIdMismatch_Error()
        {
            // Arrange
            InitTestForBtnSubmitClick(subscriptionManagement: new SubscriptionManagement() { SubscriptionManagementID = 1 });
            ShimSubscriptionManagement.AllInstances.getSMID = (sm) => 2;

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => _lblErrorMessage.Text.ShouldBe(ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink)),
                () => _logNonCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [Test]
        public void BtnSubmit_Click_ViewStateException_Error()
        {
            // Arrange
            InitTestForBtnSubmitClick();
            ShimSubscriptionManagement.AllInstances.GetUser = (sm) => throw new ViewStateException();

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => _lblErrorMessage.Text.ShouldBe(ActivityError.GetErrorMessage(Enums.ErrorMessage.Timeout)),
                () => _logNonCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [Test]
        public void BtnSubmit_Click_EcnException_Error()
        {
            // Arrange
            InitTestForBtnSubmitClick();
            var errors = new List<ECNError>();
            ShimSubscriptionManagement.AllInstances.GetUser = (sm) => throw new ECNException(errors);

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => _lblErrorMessage.Text.ShouldBe(ActivityError.GetErrorMessage(Enums.ErrorMessage.HardError)),
                () => _logNonCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [Test]
        public void BtnSubmit_Click_Exception_Error()
        {
            // Arrange
            InitTestForBtnSubmitClick();
            ShimSubscriptionManagement.AllInstances.GetUser = (sm) => throw new Exception();

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => _lblErrorMessage.Text.ShouldBe(ActivityError.GetErrorMessage(Enums.ErrorMessage.HardError)),
                () => _logCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [TestCase("", 10)]
        [TestCase("email", -10)]
        public void BtnSubmit_Click_InvalidSmIdOrEmail_Error(string email, int smId)
        {
            // Arrange
            InitTestForBtnSubmitClick();
            ShimSubscriptionManagement.AllInstances.getEmailAddress = (sm) => email;
            ShimSubscriptionManagement.AllInstances.getSMID = (sm) => smId;

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => _lblErrorMessage.Text.ShouldBe(ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink)),
                () => _logNonCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [TestCase(true, "U", false)]
        [TestCase(false, "S", false)]
        [TestCase(false, "U", false)]
        [TestCase(true, "S", false)]
        [TestCase(false, "S", null)]
        public void BtnSubmit_Click_WithDifferentInitialValue_NoError(bool subscriped, string initialValue, bool? reasonVisible)
        {
            // Arrange
            CreateGroupListForBtnSubmitClick(out List<SubscriptionManagementGroup> subscriptionGroupList, out List<Group> dataGroupList);
            var subscriptionManagement = new SubscriptionManagement() { SubscriptionManagementID = 10, ReasonVisible = reasonVisible, UseReasonDropDown = true };
            InitTestForBtnSubmitClick(
                subscriptionManagement: subscriptionManagement,
                smID: 10,
                subscriptionGroupList: subscriptionGroupList,
                dataGroupList: dataGroupList);
            _gvCurrentRBSubscribe.Checked = subscriped;
            _gvCurrentHFInitialId.Value = initialValue;

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _lblReasonError.Text.ShouldBeEmpty();
        }

        [TestCase(0, "")]
        [TestCase(1, "reason")]
        [TestCase(1, "")]
        [TestCase(2, "")]
        public void BtnSubmit_Click_UseReasonDropDownWithDifferentSelection_LabelErrorInitialized(int selectedIndex, string reasonText)
        {
            // Arrange
            CreateGroupListForBtnSubmitClick(out List<SubscriptionManagementGroup> subscriptionGroupList, out List<Group> dataGroupList);
            var subscriptionManagement = new SubscriptionManagement() { SubscriptionManagementID = 10, ReasonVisible = true, UseReasonDropDown = true };
            InitTestForBtnSubmitClick(
                subscriptionManagement: subscriptionManagement,
                smID: 10,
                subscriptionGroupList: subscriptionGroupList,
                dataGroupList: dataGroupList);
            _gvCurrentRBSubscribe.Checked = false;
            _gvCurrentHFInitialId.Value = "S";
            _ddlReason.SelectedIndex = selectedIndex;
            _txtReason.Text = reasonText;
            Set(GridViewAvailableId, null);
            var expectedError = string.Empty;
            if (selectedIndex == 0)
            {
                expectedError = ButtonSubmitClickNoReasonSelectedErrorMsg;
            }
            else if (selectedIndex == 1 && string.IsNullOrWhiteSpace(reasonText))
            {
                expectedError = ButtonSubmitClickNoReasonTextErrorMsg;
            }

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _lblReasonError.Text.ShouldBe(expectedError);
        }

        [TestCase("")]
        [TestCase("reason")]
        public void BtnSubmit_Click_NoUseReasonDropDownWithDifferentReasonText_LabelErrorInitialized(string reasonText)
        {
            // Arrange
            CreateGroupListForBtnSubmitClick(out List<SubscriptionManagementGroup> subscriptionGroupList, out List<Group> dataGroupList);
            var subscriptionManagement = new SubscriptionManagement() { SubscriptionManagementID = 10, ReasonVisible = true, UseReasonDropDown = false };
            InitTestForBtnSubmitClick(
                subscriptionManagement: subscriptionManagement,
                smID: 10,
                subscriptionGroupList: subscriptionGroupList,
                dataGroupList: dataGroupList);
            _gvCurrentRBSubscribe.Checked = false;
            Set(GridViewAvailableId, null);
            _gvCurrentHFInitialId.Value = "S";
            _txtReason.Text = reasonText;

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _lblReasonError.Text.ShouldBe(string.IsNullOrWhiteSpace(reasonText) ? ButtonSubmitClickNoReasonTextErrorMsg : string.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void BtnSubmit_Click_GridAvailableExist(bool isSubscribe)
        {
            // Arrange
            CreateGroupListForBtnSubmitClick(out List<SubscriptionManagementGroup> subscriptionGroupList, out List<Group> dataGroupList);
            var subscriptionManagement = new SubscriptionManagement()
            {
                SubscriptionManagementID = 10,
                ReasonVisible = true,
                UseReasonDropDown = false,
                CreatedUserID = 10
            };
            InitTestForBtnSubmitClick(
                subscriptionManagement: subscriptionManagement,
                smID: 10, subscriptionGroupList: subscriptionGroupList,
                dataGroupList: dataGroupList);
            Set(GridViewCurrentId, null);
            _gvCheckboxAvailableSubscribe.Checked = isSubscribe;
            _chkBoxSendResponse.Checked = true;
            var expectedContent = isSubscribe
                ? "<td>subscribed</td>" 
                : "<td>Unsubscribed</td>";

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _buttonSubmitClickEmailContent.ShouldSatisfyAllConditions(
                () => _buttonSubmitClickEmailContent.ShouldNotBeNullOrWhiteSpace(),
                () => _buttonSubmitClickEmailContent.ToLower().ShouldContain(expectedContent.ToLower()));
        }

        [Test]
        public void BtnSubmit_Click_SendResponseChecked_EmailSaved()
        {
            // Arrange
            CreateGroupListForBtnSubmitClick(out List<SubscriptionManagementGroup> subscriptionGroupList, out List<Group> dataGroupList);
            var subscriptionManagement = new SubscriptionManagement()
            {
                SubscriptionManagementID = 10,
                ReasonVisible = true,
                UseReasonDropDown = false,
                AdminEmail = "mail1,mail2",
                CreatedUserID = 10
            };
            InitTestForBtnSubmitClick(
                subscriptionManagement: subscriptionManagement,
                smID: 10,
                subscriptionGroupList: subscriptionGroupList,
                dataGroupList: dataGroupList);
            Set(GridViewAvailableId, null);
            _chkBoxSendResponse.Checked = true;
            var expectedSavedEmailCount = subscriptionManagement.AdminEmail.Split(",".ToCharArray()).Length + 1;

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _btc_EmailDirectSaveMethodCallCount.ShouldBe(expectedSavedEmailCount);
        }

        [Test]
        public void BtnSubmit_Click_ImportEmailToGroupsException_Error()
        {
            // Arrange
            CreateGroupListForBtnSubmitClick(out List<SubscriptionManagementGroup> subscriptionGroupList, out List<Group> dataGroupList);
            var subscriptionManagement = new SubscriptionManagement()
            {
                SubscriptionManagementID = 10,
                ReasonVisible = true,
                UseReasonDropDown = false,
                AdminEmail = "mail1,mail2",
                CreatedUserID = 10
            };
            InitTestForBtnSubmitClick(
                subscriptionManagement: subscriptionManagement,
                smID: 10,
                subscriptionGroupList: subscriptionGroupList,
                dataGroupList: dataGroupList);
            Set(GridViewAvailableId, null);
            ShimEmailGroup.ImportEmailToGroups_NoAccessCheckStringInt32 = (xml, id) => throw new Exception();

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _phError.ShouldSatisfyAllConditions(
                 () => _phError.Visible.ShouldBeTrue(),
                 () => _lblErrorMessage.Text.ShouldBe(ActivityError.GetErrorMessage(Enums.ErrorMessage.HardError)));
        }

        [Test]
        public void BtnSubmit_Click_SendResponseCheckedSaveException_ErrorLogged()
        {
            // Arrange
            CreateGroupListForBtnSubmitClick(out List<SubscriptionManagementGroup> subscriptionGroupList, out List<Group> dataGroupList);
            var subscriptionManagement = new SubscriptionManagement()
            {
                SubscriptionManagementID = 10,
                ReasonVisible = true,
                UseReasonDropDown = false,
                AdminEmail = "mail1,mail2",
                CreatedUserID = 10
            };
            InitTestForBtnSubmitClick(
                subscriptionManagement: subscriptionManagement,
                smID: 10, subscriptionGroupList: subscriptionGroupList,
                dataGroupList: dataGroupList,
                emailDirectSaveException: true);
            Set(GridViewAvailableId, null);
            _chkBoxSendResponse.Checked = true;
            var expectedErrorsCount = subscriptionManagement.AdminEmail.Split(",".ToCharArray()).Length + 1;

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            _logNonCriticalErrorMethodCallCount.ShouldBe(expectedErrorsCount);
        }

        [TestCase(null, null)]
        [TestCase(null, true)]
        [TestCase(null, false)]
        [TestCase(true, null)]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, null)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void BtnSubmit_Click_UseThankYouAndRedirectDifferentValues_ProperAction(bool? useThankYou, bool? useRedirect)
        {
            // Arrange
            CreateGroupListForBtnSubmitClick(out List<SubscriptionManagementGroup> subscriptionGroupList, out List<Group> dataGroupList);
            var subscriptionManagement = new SubscriptionManagement()
            {
                UseThankYou = useThankYou,
                UseRedirect = useRedirect,
                SubscriptionManagementID = 10,
                ReasonVisible = true,
                UseReasonDropDown = false,
                AdminEmail = "mail1,mail2",
                CreatedUserID = 10,
                RedirectDelay = 10,
                ThankYouLabel = ThankYouLabel
            };
            InitTestForBtnSubmitClick(
                subscriptionManagement: subscriptionManagement,
                smID: 10,
                subscriptionGroupList: subscriptionGroupList,
                dataGroupList: dataGroupList);
            Set(GridViewAvailableId, null);

            // Act
            _subscriptionManagementPrivateObject.Invoke("btnSubmit_Click", new object[] { null, EventArgs.Empty });

            // Assert
            if (useThankYou.HasValue && useThankYou.Value && (!useRedirect.HasValue || !useRedirect.Value))
            {
                _panelThankYou.Visible.ShouldBeTrue();
                _lblThankYouHeading.Text.ShouldBe(ThankYouLabel);
            }
            else if (useRedirect.HasValue && useRedirect.Value && (!useThankYou.HasValue || !useThankYou.Value))
            {
                _btc_ResponseRedirectCalled.ShouldBeTrue();
            }
            else if (useThankYou.HasValue && useRedirect.HasValue && useThankYou.Value && useRedirect.Value)
            {
                _panelThankYou.Visible.ShouldBeTrue();
                _lblThankYouHeading.Text.ShouldBe(ThankYouLabel);
            }
            else
            {
                _panelThankYou.Visible.ShouldBeTrue();
            }
        }

        private void InitTestForBtnSubmitClick(
            bool isValidEmail = true,
            int smID = 10,
            bool emailHasValue = true,
            SubscriptionManagement subscriptionManagement = null,
            List<SubscriptionManagementGroup> subscriptionGroupList = null,
            List<Group> dataGroupList = null,
            List<SubsriptionManagementUDF> subscriptionUDF = null,
            bool emailDirectSaveException = false)
        {
            SetPagePropertiesForBtnSubmitClick(smID);
            SetPageControlsForBtnSubmitClick();
            if (subscriptionUDF == null)
            {
                subscriptionUDF = new List<SubsriptionManagementUDF>()
                {
                    new SubsriptionManagementUDF()
                };
            }
            ShimSqlConnection.AllInstances.Open = (conn) => { };
            ShimSqlConnection.AllInstances.Close = (conn) => { };
            KM.Common.Fakes.ShimDataFunctions.GetSqlConnectionString = (connString) => new ShimSqlConnection();
            ShimDataFunctions.GetSqlConnectionString = (connString) => new ShimSqlConnection();
            var shimSqlParamCollection = new ShimSqlParameterCollection();
            ShimSqlParameterCollection.AllInstances.ItemGetString = (collection, name) =>
            {
                if (name == "@IsValid" && isValidEmail)
                {
                    return new System.Data.SqlClient.SqlParameter(name, true);
                }
                return null;
            };
            ShimSqlCommand.AllInstances.ExecuteNonQuery = (command) => 1;
            AccountFakeDataLayer.ShimSubscriptionManagement.GetSqlCommand = (cmd) => subscriptionManagement;
            AccountFakeDataLayer.ShimSubscriptionManagementGroup.GetListSqlCommand = (cmd) => subscriptionGroupList;
            AccountFakeDataLayer.ShimSubscriptionManagementUDF.GetListSqlCommand = (cmd) => subscriptionUDF;
            CommunicatorFakeDataLayer.ShimEmailGroup.GetSqlCommand = (cmd) => new EmailGroup();
            List<Group> dataGroupCopy = CopyList(dataGroupList);
            CommunicatorFakeDataLayer.ShimGroup.GetSqlCommand = (cmd) =>
            {
                if (dataGroupCopy.Count == 0)
                {
                    return new Group() { GroupID = GroupId };
                }
                var grp = dataGroupCopy[0];
                dataGroupCopy.RemoveAt(0);
                return grp;
            };
            ShimEmailDirect.SaveEmailDirect = (email) =>
            {
                _btc_EmailDirectSaveMethodCallCount++;
                _buttonSubmitClickEmailContent = email?.Content;
                if (emailDirectSaveException)
                {
                    throw new SmtpException();
                }
                return 0;
            };
        }

        private void CreateGroupListForBtnSubmitClick(out List<SubscriptionManagementGroup> subscriptionGroupList, out List<Group> dataGroupList)
        {
            subscriptionGroupList = new List<SubscriptionManagementGroup>();
            dataGroupList = new List<Group>();
            subscriptionGroupList.Add(new SubscriptionManagementGroup()
            {
                GroupID = GroupId,
                SubscriptionManagementGroupID = 1
            });
            dataGroupList.Add(new Group() { GroupID = GroupId });
        }

        private void SetPageControlsForBtnSubmitClick()
        {
            _gvCurrentRBSubscribe = new RadioButton();
            _gvCurrentHFInitialId = new HiddenField();
            _gvCheckboxAvailableSubscribe = new CheckBox();
            _gvCurrent = InitField<GridView>(GridViewCurrentId);
            _gvAvailable = InitField<GridView>(GridViewAvailableId);
            ShimGridView.AllInstances.RowsGet = (g) => new GridViewRowCollection(new ArrayList() { new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal) });
            ShimDataKey.AllInstances.ValueGet = (k) => GroupId;
            ShimDataKeyArray.AllInstances.ItemGetInt32 = (k, id) => new ShimDataKey();
            ShimGridView.AllInstances.DataKeysGet = (g) => new ShimDataKeyArray();
            ShimControl.AllInstances.FindControlString = (control, id) =>
            {
                if (control is GridViewRow)
                {
                    if (id == GVCurrentRBSubId)
                    {
                        return _gvCurrentRBSubscribe;
                    }
                    else if (id == GV_CurrentHFInitialId)
                    {
                        return _gvCurrentHFInitialId;
                    }
                    else if (id == GVAvailableChkSubId)
                    {
                        return _gvCheckboxAvailableSubscribe;
                    }
                }
                return ShimsContext.ExecuteWithoutShims(() => control.FindControl(id));
            };
            _ddlReason = InitField<DropDownList>(DropDownReasonId);
            _ddlReason.Items.Add(new ListItem("first", "first"));
            _ddlReason.Items.Add(new ListItem("other", "other"));
            _ddlReason.Items.Add(new ListItem("reason", "reason"));
            _lblReasonError = InitField<Label>(LabelReasonErrorId);
            _txtReason = InitField<TextBox>(TextReasonId);
            _chkBoxSendResponse = InitField<CheckBox>(CheckBoxSendResponseId);
            _panelContent = InitField<Panel>(PanelContentId);
            _panelThankYou = InitField<Panel>(PanelThankYouId);
            _lblThankYouHeading = InitField<Label>(LabelThankYouHeadingId);
            _panelThankYou.Visible = false;
        }

        private void SetPagePropertiesForBtnSubmitClick(int smID)
        {
            var appSettings = new NameValueCollection();
            var queryString = new NameValueCollection();
            var serverVariables = new NameValueCollection();
            var requestHeaders = new NameValueCollection();
            queryString["e"] = BtnSubmitClickEmail;
            queryString["smid"] = smID.ToString();
            queryString["embedded"] = true.ToString();
            appSettings.Add(AppSettingsEngineAccessKey, AppSettingsEngineAccessKeyValue);
            serverVariables.Add("HTTP_HOST", string.Empty);
            requestHeaders.Add("header1", "header1Value");
            var cache = new Cache();
            ShimPage.AllInstances.CacheGet = (p) => cache;
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (response, url, endResponse) => { _btc_ResponseRedirectCalled = true; };
            ShimPage.AllInstances.ResponseGet = (p) => new ShimHttpResponse();
            ShimHttpRequest.AllInstances.HeadersGet = (request) => requestHeaders;
            ShimHttpRequest.AllInstances.QueryStringGet = (request) => queryString;
            ShimHttpRequest.AllInstances.ServerVariablesGet = (request) => serverVariables;
            ShimHttpRequest.AllInstances.RawUrlGet = (request) => string.Empty;
            ShimHttpRequest.AllInstances.UrlReferrerGet = (request) => new Uri("http://fakeUrl00000fakeUrl.com");
            ShimHttpContext.CurrentGet = () => new ShimHttpContext();
            ShimHttpContext.AllInstances.RequestGet = (context) => new ShimHttpRequest();
            ShimPage.AllInstances.RequestGet = (p) => new ShimHttpRequest();
            ShimClientScriptManager.AllInstances.RegisterStartupScriptTypeStringString = (manager, type, key, script) => { };
            ShimPage.AllInstances.ClientScriptGet = (p) => new ShimClientScriptManager();
            ShimConfigurationManager.AppSettingsGet = () => appSettings;
            KMPlatform.BusinessLogic.Fakes.ShimUser.GetByAccessKeyStringBoolean = (key, getChildren) => new User() { UserID = 10 };
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 = (e, method, appId, note, charId, custId) => _logNonCriticalErrorMethodCallCount++;
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (e, method, appId, note, charId, custId) => _logCriticalErrorMethodCallCount++;
        }
    }
}