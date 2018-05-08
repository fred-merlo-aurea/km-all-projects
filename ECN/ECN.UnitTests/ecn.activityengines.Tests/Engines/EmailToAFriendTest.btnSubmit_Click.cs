using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Web.Fakes;
using System.Web.SessionState.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.activityengines.Fakes;
using ecn.common.classes.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using NUnit.Framework;
using Shouldly;
using AccountsFake = ECN_Framework_BusinessLayer.Accounts.Fakes;
using ClassesFakes = ecn.communicator.classes.Fakes;
using EntitiesEnum = ECN_Framework_Common.Objects.Enums.Entity;
using MethodsEnum = ECN_Framework_Common.Objects.Enums.Method;

namespace ecn.activityengines.Tests.Engines
{
    public partial class EmailToAFriendTest
    {
        private const string BTSC_TxtEmail_1 = "TxtEmail_1";
        private const string BTSC_TxtEmail_2 = "TxtEmail_2";
        private const string BTSC_TxtEmail_3 = "TxtEmail_3";
        private const string BTSC_TxtEmail_4 = "TxtEmail_4";
        private const string BTSC_TxtEmail_5 = "TxtEmail_5";
        private const string BTSC_TxtName_1 = "TxtName_1";
        private const string BTSC_TxtName_2 = "TxtName_2";
        private const string BTSC_TxtName_3 = "TxtName_3";
        private const string BTSC_TxtName_4 = "TxtName_4";
        private const string BTSC_TxtName_5 = "TxtName_5";
        private const string BTSC_AppSettingKm_CommonKey = "KMCommon_Application";
        private const string BTSC_AppSettingKm_CommonValue = "10";
        private const string BTSC_ConfirmationMsg_PositiveAcitiveLog = "message has already been sent to";
        private const string BTSC_ConfirmationMsg_NegativeEmailId = "message cannot be sent to invalid email address";
        private const string BTSC_ConfirmationMsg_PositiveEmailId = "message has been successfully forwarded to";
        private const string BTSC_ConfirmationMsg_ExistingEmail = "message cannot be sent to existing email address";
        private string _nonCriticalErrorMsg;
        private int _logNonCriticalErrorMethodCallCount;
        private int _logCriticalErrorMethodCallCount;
        private TextBox _txtEmail1;
        private TextBox _txtEmail2;
        private TextBox _txtEmail3;
        private TextBox _txtEmail4;
        private TextBox _txtEmail5;
        private TextBox _txtName1;
        private TextBox _txtName2;
        private TextBox _txtName3;
        private TextBox _txtName4;
        private TextBox _txtName5;
        private Label _lblFrom;
        private TextBox _txtNote;
        private Panel _pnlMain;
        private Panel _pnlThankYou;
        private Label _lblConfirmation;

        [Test]
        public void BtnSubmit_Click_EcnException_Error()
        {
            // Arrange
            InitTest_btnSubmit_Click(preview: -1);
            ShimEmail.IsValidEmailAddressString = (email) =>
            {
                var errorsList = new List<ECNError>() { new ECNError(EntitiesEnum.Email, MethodsEnum.Validate, "isValidError") };
                throw new ECNException(errorsList);
            };
            var expectedError = $"<br/>{EntitiesEnum.Email}: isValidError";

            // Act
            _emailToAFriendPrivateObject.Invoke("btnSubmit_Click", new object[] { null, null });

            //Assert
            _logNonCriticalErrorMethodCallCount.ShouldSatisfyAllConditions(
                () => _logNonCriticalErrorMethodCallCount.ShouldBe(1),
                () => _nonCriticalErrorMsg.ShouldBe(expectedError));
        }

        [Test]
        public void BtnSubmit_Click_Exception_Error()
        {
            // Arrange
            InitTest_btnSubmit_Click(preview: -1);
            ShimEmail.IsValidEmailAddressString = (email) => throw new Exception();
            var expectedError = ActivityError.GetErrorMessage(Enums.ErrorMessage.HardError);

            // Act
            _emailToAFriendPrivateObject.Invoke("btnSubmit_Click", new object[] { null, null });

            //Assert
            _logCriticalErrorMethodCallCount.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => _lblErrorMessage.Text.ShouldBe(expectedError),
                () => _logCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [Test]
        public void BtnSubmit_Click_PreviewGreaterThanZero_ConfirmationMsgInitialized()
        {
            // Arrange
            InitTest_btnSubmit_Click(preview: 10);

            // Act
            _emailToAFriendPrivateObject.Invoke("btnSubmit_Click", new object[] { null, null });

            //Assert
            _lblConfirmation.ShouldSatisfyAllConditions(
                () => _lblConfirmation.Text.ShouldContain(BTSC_TxtEmail_1),
                () => _lblConfirmation.Text.ShouldContain(BTSC_TxtEmail_2),
                () => _lblConfirmation.Text.ShouldContain(BTSC_TxtEmail_3),
                () => _lblConfirmation.Text.ShouldContain(BTSC_TxtEmail_4),
                () => _lblConfirmation.Text.ShouldContain(BTSC_TxtEmail_5),
                () => _pnlMain.Visible.ShouldBeFalse());
        }

        [Test]
        public void BtnSubmit_Click_PreviewLessThanZeroTrackDataException_ExceptionLogged()
        {
            // Arrange
            InitTest_btnSubmit_Click(preview: -10, throwTrackDataException: true);

            // Act
            _emailToAFriendPrivateObject.Invoke("btnSubmit_Click", new object[] { null, null });

            //Assert
            _logNonCriticalErrorMethodCallCount.ShouldBe(1);
        }

        [TestCase(10, 10)]
        [TestCase(-10, -10)]
        [TestCase(-10, 10)]
        public void BtnSubmit_Click_PreviewLessThanZero_ConfirmationMsgInitialized(int activityLogSendCount, int emailId)
        {
            // Arrange
            InitTest_btnSubmit_Click(preview: -10, trackDataEmailId: emailId, activityLogSendCount: activityLogSendCount);
            var expectedConfirmationMsg = string.Empty;
            if (activityLogSendCount > 0)
            {
                expectedConfirmationMsg = BTSC_ConfirmationMsg_PositiveAcitiveLog;
            }
            else if (emailId < 0)
            {
                expectedConfirmationMsg = BTSC_ConfirmationMsg_NegativeEmailId;
            }
            else
            {
                expectedConfirmationMsg = BTSC_ConfirmationMsg_PositiveEmailId;
            }

            // Act
            _emailToAFriendPrivateObject.Invoke("btnSubmit_Click", new object[] { null, null });

            //Assert
            _lblConfirmation.Text.ToLower().ShouldContain(expectedConfirmationMsg);
        }

        [Test]
        public void BtnSubmit_Click_EmailGroupNotExist_ConfirmationMsgInitialized()
        {
            // Arrange
            InitTest_btnSubmit_Click(preview: -10, trackDataEmailId: 10, emailGroupExit: true, activityLogSendCount: 10);

            // Act
            _emailToAFriendPrivateObject.Invoke("btnSubmit_Click", new object[] { null, null });

            //Assert
            _lblConfirmation.Text.ToLower().ShouldContain(BTSC_ConfirmationMsg_ExistingEmail);
        }

        [Test]
        public void BtnSubmit_Click_InvalidEmail_ConfirmationMsgInitialized()
        {
            // Arrange
            InitTest_btnSubmit_Click(preview: -10, trackDataEmailId: 10, isValidEmailAddress: false, activityLogSendCount: 10);

            // Act
            _emailToAFriendPrivateObject.Invoke("btnSubmit_Click", new object[] { null, null });

            //Assert
            _lblConfirmation.Text.ToLower().ShouldContain(BTSC_ConfirmationMsg_NegativeEmailId);
        }

        private void InitTest_btnSubmit_Click(int preview = 0, bool emailGroupExit = false, int activityLogSendCount = 10, string userHostAddress = null, string userAgent = null, bool throwTrackDataException = false, int trackDataEmailId = 20, bool isValidEmailAddress = true, bool sendBlastException = false, bool shimSendBlast = true)
        {
            _nonCriticalErrorMsg = string.Empty;
            _logNonCriticalErrorMethodCallCount = 0;
            _logCriticalErrorMethodCallCount = 0;
            SetPageProperties_btnSubmit_Click();
            SetPageControls_btnSubmit_Click();
            _emailToAFriendPrivateObject.SetField("Preview", BindingFlags.Instance | BindingFlags.NonPublic, preview);
            ShimEmailGroup.ExistsStringInt32Int32 = (email, grpId, customerId) => emailGroupExit;
            ShimEmailActivityLog.GetSendCountInt32Int32 = (id, blastId) => activityLogSendCount;
            ShimEmailActivityLog.InsertEmailActivityLogUser = (log, user) => 10;
            ClassesFakes.ShimEmailFunctions.CreateEmailListAllStringInt32Int32StringString = (filter, custId, grpId, bounce, bID) => new DataTable();
            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString
                = (user, customerID, groupID, xmlProfile, xmlUDF, formatTypeCode, subscribeTypeCode, emailAddressOnly, filename, source) => new DataTable();
            if (shimSendBlast)
            {
                Shimemailtoafriend.AllInstances.SendBlastInt32Int32StringInt32 = (page, bid, emailId, email, fromEmailId) => { };
            }
            ShimEmail.IsValidEmailAddressString = (email) => isValidEmailAddress;
            ShimEmail.GetByEmailAddressStringInt32 = (email, custId) =>
            {
                if (throwTrackDataException)
                {
                    throw new Exception();
                }
                return new Email() { EmailID = trackDataEmailId };
            };
            ClassesFakes.ShimEmailFunctions.AllInstances.SendBlastForEmailsBlastStringStringStringDataTableBooleanBooleanStringBoolean = (fn, blast, vPath, host, bounce, table, Resend, tBlast, email, doSocialMedia) => { };
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 = (error, method, id, errorNote, charit, custId) =>
            {
                _logNonCriticalErrorMethodCallCount++;
                _nonCriticalErrorMsg = errorNote;
                return 0;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (exception, source, appId, note, charId, custId) => _logCriticalErrorMethodCallCount++;
            ShimControl.AllInstances.FindControlString = (control, id) =>
            {
                if (id == "lblHeader" || id == "lblFooter")
                {
                    return new Label();
                }
                return null;
            };
            AccountsFake.ShimLandingPageAssign.GetByLPAIDInt32Boolean = (view, getChildren) => new LandingPageAssign();
            AccountsFake.ShimLandingPageAssign.GetOneToUseInt32Int32Boolean = (lp, custId, getChildren) => new LandingPageAssign();
            ClassesFakes.ShimBlasts.ConstructorInt32 = (b,id)=> { };
        }

        private void SetPageControls_btnSubmit_Click()
        {
            _txtEmail1 = new TextBox();
            _txtEmail1.Text = BTSC_TxtEmail_1;
            _emailToAFriendPrivateObject.SetField("txtEmail1", BindingFlags.Instance | BindingFlags.NonPublic, _txtEmail1);
            _txtEmail2 = new TextBox();
            _txtEmail2.Text = BTSC_TxtEmail_2;
            _emailToAFriendPrivateObject.SetField("txtEmail2", BindingFlags.Instance | BindingFlags.NonPublic, _txtEmail2);
            _txtEmail3 = new TextBox();
            _txtEmail3.Text = BTSC_TxtEmail_3;
            _emailToAFriendPrivateObject.SetField("txtEmail3", BindingFlags.Instance | BindingFlags.NonPublic, _txtEmail3);
            _txtEmail4 = new TextBox();
            _txtEmail4.Text = BTSC_TxtEmail_4;
            _emailToAFriendPrivateObject.SetField("txtEmail4", BindingFlags.Instance | BindingFlags.NonPublic, _txtEmail4);
            _txtEmail5 = new TextBox();
            _txtEmail5.Text = BTSC_TxtEmail_5;
            _emailToAFriendPrivateObject.SetField("txtEmail5", BindingFlags.Instance | BindingFlags.NonPublic, _txtEmail5);
            _txtName1 = new TextBox();
            _txtName1.Text = BTSC_TxtName_1;
            _emailToAFriendPrivateObject.SetField("txtName1", BindingFlags.Instance | BindingFlags.NonPublic, _txtName1);
            _txtName2 = new TextBox();
            _txtName2.Text = BTSC_TxtName_2;
            _emailToAFriendPrivateObject.SetField("txtName2", BindingFlags.Instance | BindingFlags.NonPublic, _txtName2);
            _txtName3 = new TextBox();
            _txtName3.Text = BTSC_TxtName_3;
            _emailToAFriendPrivateObject.SetField("txtName3", BindingFlags.Instance | BindingFlags.NonPublic, _txtName3);
            _txtName4 = new TextBox();
            _txtName4.Text = BTSC_TxtName_4;
            _emailToAFriendPrivateObject.SetField("txtName4", BindingFlags.Instance | BindingFlags.NonPublic, _txtName4);
            _txtName5 = new TextBox();
            _txtName5.Text = BTSC_TxtName_5;
            _emailToAFriendPrivateObject.SetField("txtName5", BindingFlags.Instance | BindingFlags.NonPublic, _txtName5);
            _txtNote = new TextBox();
            _emailToAFriendPrivateObject.SetField("txtNote", BindingFlags.Instance | BindingFlags.NonPublic, _txtNote);
            _lblFrom = new Label();
            _lblFrom.Text = "EmailFrom";
            _emailToAFriendPrivateObject.SetField("lblFrom", BindingFlags.Instance | BindingFlags.NonPublic, _lblFrom);
            _pnlMain = new Panel();
            _emailToAFriendPrivateObject.SetField("pnlMain", BindingFlags.Instance | BindingFlags.NonPublic, _pnlMain);
            _pnlThankYou = new Panel();
            _emailToAFriendPrivateObject.SetField("pnlThankYou", BindingFlags.Instance | BindingFlags.NonPublic, _pnlThankYou);
            _lblConfirmation = new Label();
            _emailToAFriendPrivateObject.SetField("lblConfirmation", BindingFlags.Instance | BindingFlags.NonPublic, _lblConfirmation);
        }

        private void SetPageProperties_btnSubmit_Click()
        {
            var httpSession = new ShimHttpSessionState();
            ShimPage.AllInstances.SessionGet = (p) => httpSession;
            var serverVariablesCollection = new NameValueCollection();
            serverVariablesCollection.Add("HTTP_HOST", "HTTP_HOST");
            var requestHeadersCollection = new NameValueCollection();
            requestHeadersCollection.Add("header1", "dummyValue");
            ShimHttpRequest.AllInstances.HeadersGet = (r) => requestHeadersCollection;
            ShimHttpRequest.AllInstances.ServerVariablesGet = (r) => serverVariablesCollection;
            ShimHttpRequest.AllInstances.UrlReferrerGet = (r) => new Uri("http://www.dummy9999999.com");
            ShimHttpContext.AllInstances.RequestGet = (c) => new ShimHttpRequest();
            ShimHttpContext.CurrentGet = () => new ShimHttpContext();
            ShimHttpRequest.AllInstances.RawUrlGet = (r) => "RawUrl";
            ShimHttpRequest.AllInstances.UserAgentGet = (r) => "UserAgent";
            ShimHttpRequest.AllInstances.UserHostAddressGet = (r) => "UserHostAddress";
            ShimPage.AllInstances.ResponseGet = (p) => new ShimHttpResponse();
            ShimPage.AllInstances.RequestGet = (p) => new ShimHttpRequest();
            ShimPage.AllInstances.MasterGet = (p) => new MasterPages.Activity();
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (blastId, getChildren) => new BlastSMS() { TestBlast = "Y", CustomerID = 10 };
            ShimDataFunctions.ExecuteString = (q) => throw new Exception();
        }
    }
}
