using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.listsmanager;
using ecn.communicator.listsmanager.Fakes;
using ECN_Framework.Common.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    /// UT for <see cref="ReferralProgram"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ReferralProgramTest : BaseListsTest<ReferralProgram>
    {
        private const int ReferralProgramId = 20;
        private const string ReferralProgramIdPropertyName = "ReferralProgramId";
        private const string ReferralProgramIdQueryStringKey = "rpid";
        private const int GroupId = 40;
        private const string GroupIdPropertyName = "GroupId";
        private const string GroupIdQueryStringKey = "GroupID";
        private const string Action = "Edit";
        private const string ActionPropertyName = "RequestedAction";
        private const string ActionQueryStringKey = "action";

        [Test]
        public void Page_Load_HasAccess_WithRPID_ActionDelete_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitializePageLoadObjects();
            var deleteExecuted = false;
            ecn.common.classes.Fakes.ShimDataFunctions.ExecuteString = (p) => { deleteExecuted = true; return 0; };
            QueryString["action"] = "delete";

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            deleteExecuted.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_HasAccess_WithRPID_ActionDelete_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            InitializePageLoadObjects();
            ecn.common.classes.Fakes.ShimDataFunctions.ExecuteString = (p) => throw new Exception("Test Exception");
            QueryString["action"] = "delete";

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            responseText.ShouldBe("<script>javascript:window.open('../../includes/popup.aspx','Error', 'left=100,top=100,height=300,width=615,resizable=yes,scrollbar=yes,status=no' );</script>");
        }

        [Test]
        public void Page_Load_HasAccess_WithRPID_ActionDefault_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitializePageLoadObjects();
            var programName = privateObject.GetFieldOrProperty("programName") as TextBox;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            programName.Text.ShouldBe("TestName");
        }

        [Test]
        public void Page_Load_HasAccess_WithGroupID_WithCommunicatorLevel_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitializePageLoadObjects();
            var programName = privateObject.GetFieldOrProperty("programName") as TextBox;
            QueryString["GroupID"] = "1";
            QueryString["rpid"] = "0";
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentCustomer = new Customer { CommunicatorLevel = "2" };
                return session;
            };

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            responseText.ShouldBeEmpty();
        }

        [Test]
        public void Page_Load_HasAccess_WithGroupID_WithoutCommunicatorLevel_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitializePageLoadObjects();
            var programName = privateObject.GetFieldOrProperty("programName") as TextBox;
            QueryString["GroupID"] = "1";
            QueryString["rpid"] = "0";
            var txtBx_HTMLCode = privateObject.GetFieldOrProperty("TxtBx_HTMLCode") as TextBox;
            txtBx_HTMLCode.Visible = false;
            ecn.common.classes.Fakes.ShimDataFunctions.ExecuteScalarString = (p) => string.Empty;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            txtBx_HTMLCode.Visible.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_HasAccess_WithGroupID_PostBack_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitializePageLoadObjects();
            ShimPage.AllInstances.IsPostBackGet = (p) => true;
            QueryString["GroupID"] = "1";
            QueryString["rpid"] = "0";

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            responseText.ShouldBeEmpty();
        }

        [Test]
        public void Page_Load_HasAccess_Default_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitializePageLoadObjects();
            ShimPage.AllInstances.IsPostBackGet = (p) => true;
            QueryString["GroupID"] = "0";
            QueryString["rpid"] = "0";

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("../default.aspx");
        }

        [Test]
        public void GetHTMLCode_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var RP_OptinFieldSelection = privateObject.GetFieldOrProperty("RP_OptinFieldSelection") as ListBox;
            RP_OptinFieldSelection.Items.Add("1");
            RP_OptinFieldSelection.SelectedValue = "1";
            var SFFieldSet = privateObject.GetFieldOrProperty("SFFieldSet") as TextBox;
            SFFieldSet.Text = "1";
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection { { "Activity_DomainPath", "Activity_DomainPath" } };

            // Act
            var result = (string)privateObject.Invoke("GetHTMLCode", new object[] { null, "RP" });

            // Assert
            result.ShouldBe("<form action=Activity_DomainPath/engines/ReferralProgram.aspx><table border=1><tr><tr><td>Email Address:</td><td>1</td></tr><tr><td><INPUT id=EmailAddress_1 type=text name=EmailAddress_1 size=25></td><td><INPUT id=1_1 type=text name=1_1 size=15></td></tr><tr><td colspan=2 align=center>\t<INPUT type=hidden value=S name=s>\t<INPUT type=hidden value=html name=f>\t<input type=hidden name=g value='0'>\t<input type=hidden name=c value=0>\t<input type=hidden name=rpid value='0'>\t<INPUT name=reID type=hidden value=Submit name=Submit>  <INPUT name=user_Referred_By type=hidden>  <INPUT name=user_Referred_On type=hidden>  <INPUT id=Submit type=submit value=Submit name=Submit></td></tr></table></form><!-- Do NOT MODIFY THE SCRIPT BELOW --><!-- REFERRAL PROGRAM WILL NOT WORK PROPERLY IF THE SCRIPT IS MODIFIED --><SCRIPT LANGUAGE='JavaScript' src='Image_DomainPath/channels/1/js/referralProgram.js'></ENDSCRIPT><!-- END SCRIPT -->");
        }

        [Test]
        public void RefreshHTML_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var RP_OptinFieldSelection = privateObject.GetFieldOrProperty("RP_OptinFieldSelection") as ListBox;
            RP_OptinFieldSelection.Items.Add("1");
            RP_OptinFieldSelection.SelectedValue = "1";
            var SFFieldSet = privateObject.GetFieldOrProperty("SFFieldSet") as TextBox;
            SFFieldSet.Text = "1";
            var RP_HTMLCode = privateObject.GetFieldOrProperty("RP_HTMLCode") as CKEditor.NET.CKEditorControl;
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection { { "Activity_DomainPath", "Activity_DomainPath" } };

            // Act
            privateObject.Invoke("RefreshHTML_Click", new object[] { null, null });

            // Assert
            RP_HTMLCode.Text.ShouldBe("<form action=Activity_DomainPath/engines/ReferralProgram.aspx><table border=1><tr><tr><td>Email Address:</td><td>1</td></tr><tr><td><INPUT id=EmailAddress_1 type=text name=EmailAddress_1 size=25></td><td><INPUT id=1_1 type=text name=1_1 size=15></td></tr><tr><td colspan=2 align=center>\t<INPUT type=hidden value=S name=s>\t<INPUT type=hidden value=html name=f>\t<input type=hidden name=g value='0'>\t<input type=hidden name=c value=0>\t<input type=hidden name=rpid value='0'>\t<INPUT name=reID type=hidden value=Submit name=Submit>  <INPUT name=user_Referred_By type=hidden>  <INPUT name=user_Referred_On type=hidden>  <INPUT id=Submit type=submit value=Submit name=Submit></td></tr></table></form><!-- Do NOT MODIFY THE SCRIPT BELOW --><!-- REFERRAL PROGRAM WILL NOT WORK PROPERLY IF THE SCRIPT IS MODIFIED --><SCRIPT LANGUAGE='JavaScript' src='Image_DomainPath/channels/1/js/referralProgram.js'></ENDSCRIPT><!-- END SCRIPT -->");
        }

        [Test]
        public void SaveOptinHTML_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            QueryString.Add("rpid", "1");
            SqlCommand sqlCommand = null;
            ShimSqlConnection.ConstructorString = (p1, p2) =>  { };
            ShimSqlConnection.AllInstances.Open = (p) => { };
            ShimSqlConnection.AllInstances.Close = (p) => { };
            ShimSqlCommand.ConstructorStringSqlConnection = (p1, p2, p3) =>  { };
            ShimSqlCommand.AllInstances.ExecuteNonQuery = (p) => { sqlCommand = p; return 0; };

            // Act
            privateObject.Invoke("SaveOptinHTML_Click", new object[] { null, null });

            // Assert
            sqlCommand.ShouldSatisfyAllConditions(
                () => sqlCommand.ShouldNotBeNull(),
                () => sqlCommand.Parameters.Count.ShouldBe(14));
        }

        [Test]
        public void RP_OptinHTMLSaveNew_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            QueryString.Add("GroupID", "1");
            SqlCommand sqlCommand = null;
            ShimSqlConnection.ConstructorString = (p1, p2) => { };
            ShimSqlConnection.AllInstances.Open = (p) => { };
            ShimSqlConnection.AllInstances.Close = (p) => { };
            ShimSqlCommand.ConstructorStringSqlConnection = (p1, p2, p3) => { };
            ShimSqlCommand.AllInstances.ExecuteNonQuery = (p) => { sqlCommand = p; return 0; };
            ShimReferralProgram.AllInstances.LoadSmartFormGridInt32 = (p1,p2) => { };

            // Act
            privateObject.Invoke("RP_OptinHTMLSaveNew_Click", new object[] { null, null });

            // Assert
            sqlCommand.ShouldSatisfyAllConditions(
                () => sqlCommand.ShouldNotBeNull(),
                () => sqlCommand.Parameters.Count.ShouldBe(13));
        }

        [Test]
        public void ReferralProgramIdGetter_IfQueryStringContainsReferralProgramId_ReturnsReferralProgramId()
        {
            // Arrange
            QueryString.Add(ReferralProgramIdQueryStringKey, ReferralProgramId.ToString());

            // Act
            var returnedValue = (int)privateObject.GetProperty(ReferralProgramIdPropertyName);

            // Assert
            returnedValue.ShouldBe(ReferralProgramId);
        }

        [Test]
        public void ReferralProgramIdGetter_IfQueryStringDoesNotContainReferralProgramId_ReturnsDefaultValue()
        {
            // Arrange
            // set nothing to query string

            // Act
            var returnedValue = (int)privateObject.GetProperty(ReferralProgramIdPropertyName);

            // Assert
            returnedValue.ShouldBe(default(int));
        }

        [Test]
        public void GroupIdGetter_IfQueryStringContainsGroupId_ReturnsGroupId()
        {
            // Arrange
            QueryString.Add(GroupIdQueryStringKey, GroupId.ToString());

            // Act
            var returnedValue = (int)privateObject.GetProperty(GroupIdPropertyName);

            // Assert
            returnedValue.ShouldBe(GroupId);
        }

        [Test]
        public void GroupIdGetter_IfQueryStringDoesNotContainGroupId_ReturnsDefaultValue()
        {
            // Arrange
            // set nothing to query string

            // Act
            var returnedValue = (int)privateObject.GetProperty(GroupIdPropertyName);

            // Assert
            returnedValue.ShouldBe(default(int));
        }

        [Test]
        public void RequestedActionGetter_IfQueryStringContainsAction_ReturnsAction()
        {
            // Arrange
            QueryString.Add(ActionQueryStringKey, Action);

            // Act
            var returnedValue = (string)privateObject.GetProperty(ActionPropertyName);

            // Assert
            returnedValue.ShouldBe(Action);
        }

        [Test]
        public void RequestedActionGetter_IfQueryStringDoesNotContainAction_ReturnsDefaultValue()
        {
            // Arrange
            // set no action to query string

            // Act
            var returnedValue = (string)privateObject.GetProperty(ActionPropertyName);

            // Assert
            returnedValue.ShouldBeNull();
        }

        private void InitializePageLoadObjects()
        {
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (p1, p2, p3, p4) => true;
            ShimSecurityAccess.canIStringString = (p1, p2) => { };
            ShimHttpResponse.AllInstances.WriteString = (p1, p2) => responseText += p2;
            ecn.common.classes.Fakes.ShimDataFunctions.GetDataTableString = (p) =>
            {
                if (p.Contains("from Layouts"))
                {
                    return new DataTable { Columns = { "LayoutID", "LayoutName" }, Rows = { { "1", "TestName" } } };
                }
                else if (p.Contains("FROM GroupDatafields"))
                {
                    return new DataTable { Columns = { "ShortName" }, Rows = { { "1" } } };
                }
                else if (p.Contains("FROM ReferralProgram"))
                {
                    return new DataTable
                    {
                        Columns = { "ReferralProgramName", "SmartFormHtml", "SmartFormFields", "Referee_Lead_MsgSubject", "Referer_Response_Screen", "Referer_Response_FromEmail", "Referer_Response_MsgSubject", "Referer_Response_MsgID", "Referee_Lead_MsgID", "SmartFormFieldset" },
                        Rows = { { "TestName", "1", "1", "1", "1", "1", "1", "1", "1", "1" } }
                    };
                }

                return new DataTable { Columns = { "id" }, Rows = { { "1" } } };
            };
            QueryString.Add("GroupID", "1");
            QueryString.Add("rpid", "1");
            QueryString.Add("action", "default");
            QueryString.Add("cuID", "1");
            QueryString.Add("chID", "1");
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer { CommunicatorLevel = "1"};
                session.CurrentBaseChannel = new BaseChannel();
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