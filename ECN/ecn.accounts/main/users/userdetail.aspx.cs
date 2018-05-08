using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Linq;
using Vladsm.Web.UI.WebControls;
using System.Text;
using System.Transactions;
using System.Data;
using KM.Common;
using KMPlatform.Entity;
using ECNCommon = ECN_Framework_Common.Objects;
using AccountsCommon = ECN_Framework_Common.Objects.Accounts;
using AccountsEntity = ECN_Framework_Entities.Accounts;
using AccountsBLL = ECN_Framework_BusinessLayer.Accounts;
using AccountsBLLView = ECN_Framework_BusinessLayer.Accounts.View;
using ApplicationBLL = ECN_Framework_BusinessLayer.Application;
using BusinessAccounts = ECN_Framework_BusinessLayer.Accounts;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using KMPlatformBusiness = KMPlatform.BusinessLogic;
using KMCommonEntity = KM.Common.Entity;
using KMPlatformEntity = KMPlatform.Entity;
using SecurityGroupAdministrativeLevel = KMPlatform.Enums.SecurityGroupAdministrativeLevel;
using KMPlatformUser = KM.Platform.User;

namespace ecn.accounts.usersmanager
{
    public partial class userdetail : ApplicationBLL.WebPageHelper
    {
        private const string CommandEditRole = "editrole";
        private const string CommandDeleteRole = "deleterole";
        private const string CommandRestrict = "restrict";

        private const char UnderscoreDelim = '_';
        private const string DashDelim = "-";

        private const string ColumnId = "ID";
        private const string ColumnIsActive = "IsActive";
        private const string ColumnIsDeleted = "IsDeleted";
        private const string ColumnInactiveReason = "InactiveReason";
        private const string ColumnDisplay = "Display";
        private const string ColumnRole = "Role";
        private const string ColumnCustomerId = "CustomerID";
        private const string ColumnCustomer = "Customer";
        private const string ColumnSecurityGroupId = "SecurityGroupID";
        private const string ColumnDoHardDelete = "DoHardDelete";
        private const string ColumnIsBcAdmin = "IsBCAdmin";
        private const string ColumnBaseChannelId = "BaseChannelID";
        private const string ColumnIsCaAdmin = "IsCAdmin";
        private const string ColumnIsChannelRole = "IsChannelRole";
        private const string ColumnBaseChannel = "BaseChannel";

        private const string RestrictedGroupsTemplate = "RestrictGroups_{0}";
        private const string SecurityGroupFilterTemplate = "SecurityGroupID = {0}";
        private const string IdFilterTemplate = "ID = '{0}'";

        private const string ValueDisabled = "Disabled";
        private const string ValueYes = "yes";
        private const string ValueNo = "no";
        private const string InactiveReasonPending = "pending";
        private const string InactiveReasonPendingCapital = "Pending";
        private const string UserCustomerIdTemplate = "{0}_{1}";
        private const string FilterBaseChannelAndGroupIdTemplate = "BaseChannelID = {0} and SecurityGroupID = {1}";

        private const string RoleChannleAdministrator = "Channel Administrator";
        private const string RoleAdministrator = "Administrator";
        private const string StatusActive = "active";
        private const string StatusDisabled = "disabled";

        private const int NoId = -1;

        private const string ErrorUserAlreadyExists = "UserName already exists";
        private const string ErrorUserHasRole = "User already has a role under this Basechannel or Customer.";
        private const string ErrorSaveUserDetails = "UserDetail.SaveUser - Error sending user opt in email";
        private const string ErrorEmailInvalid = "Email address is not valid";
        private const string ErrorNoRoleForUser = "Please assign at least one role for this user";
        private const string ErrorNoNames =
            "Before you can complete this action you need to update your own user profile with first and last name. Your first and last name is used as the \"From Name\" in the email invitation.";
        private const string ErrorOccured = "An error has occurred";

        private const string CustomerFilerTemplate =
            "(CustomerID = {0} and (IsActive = false)) or (BaseChannelID = {1} and IsActive = false)";
        private const string FilterIsActiveOrBaseChannelTemplate =
            "((IsActive = true) or (IsActive = false and InactiveReason = 'Pending')) and (BaseChannelID > 0)";
        private const string FilterIsActiveOrCustomerIdTemplate =
            "((IsActive = true) or (IsActive = false and InactiveReason = 'Pending')) and (CustomerID > 0)";
        private const string FilterIsDeletedAndCustomer = "IsDeleted = false and CustomerID > 0";

        private const string AppSettingMasterClientId = "MasterClientID";
        private const string AppSettingMasterClientGroupId = "MasterClientGroupID";
        private const string AppSettingComonApplication = "KMCommon_Application";

        private const string MethodBtnSaveClick = "UserDetail.btnSave_Click";
        private const string AttributeType = "type";
        private const string AttributeTypeValuePassword = "password";
        private const string HeadingManageUser = "Users > Manage User";
        private const string UrlSecurityAccessError = "~/main/securityAccessError.aspx";
        private const string ControlId = "hfID";
        private const string ControlBaseChannel = "hfBaseChannel";
        private const string ControlBaseChannelId = "hfBaseChannelID";
        private const string ControlCustomer = "hfCustomer";
        private const string ControlCustomerId = "hfCustomerID";
        private const string ControlRole = "hfRole";
        private const string ControlSecurityGroupId = "hfSecurityGroupID";
        private const string ControlInactiveReason = "hfInactiveReason";
        private const string ControlIsBCAdmin = "hfIsBCAdmin";
        private const string ControlIsCAdmin = "hfIsCAdmin";
        private const string ControlIsActive = "hfIsActive";
        private const string ControlIsDeleted = "hfIsDeleted";
        private const string ControlDisplay = "hfDisplay";
        private const string ControlDoHardDelete = "hfDoHardDelete";
        private const string ControlIsChannelRole = "hfIsChannelRole";

        private const string DefaultAspx = "default.aspx";

        #region get request - query string values, UserID, CustomerID, ChannelID etc.,

        KMPlatform.Entity.User user = null;
        AccountsEntity.Customer customer = null;
        DataTable dtUserRoles = null;

        string UserID = "-1";
        private int getRequestUserID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["UserID"].ToString());

            }
            catch
            {
                return -1;
            }
        }

        #endregion

        private void SetHelpContent()
        {
            Master.HelpTitle = "USERS MANAGER";
            Master.HelpContent = "<b>Unsent Emails</b><p>These are the emails you wrote or started writing but have not sent. You can also edit an email before you send it, Click the edit link. To send the email, first set the groups you want to recieve this Blast.</p>" +
                                "<b>Sent Emails</b><p>These emails are stored in your database and are available to view and/or send again.</p>" +
                                "<b>Helpful Hint</b><p>To send an email again, first 'view' the email and while viewing the email click 'write new email' link in the navigation. All you have to do is select the layout you want, rename it and click the preview email button.</p>";
        }









        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = AccountsCommon.Enums.MenuCode.USERS;
            txtPassword.Attributes[AttributeType] = AttributeTypeValuePassword;
            lblErrorMessage.Text = string.Empty;
            lblEmailSent.Visible = false;
            lblErrorMessage.Visible = false;
            Master.MasterRegisterButtonForPostBack(btnSaveRole);
            Master.Heading = HeadingManageUser;
            phError.Visible = false;
            SetHelpContent();
            if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                if (!IsPostBack)
                {
                    PageLoadNotPostBack();
                }
                else
                {
                    if (dtUserRoles == null)
                    {
                        CreateUserRolesTable();
                    }
                    if (gvUserRoles.Rows.Count > 0)
                    {
                        FillRolesTable();
                    }
                }
            }
            else
            {
                Response.Redirect(UrlSecurityAccessError);
            }
        }
        private void PageLoadNotPostBack()
        {
            btnResetPassword.Visible = false;
            txtPassword.Visible = false;
            dtUserRoles = null;
            if (KMPlatformUser.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                SetupControlsForSystemAdmin();
            }
            else if (KMPlatformUser.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                SetupControlForChannelAdmin();
            }
            else
            {
                SetupControlForNonAdmin();
            }
            if (getRequestUserID() > 0)
            {
                UserID = getRequestUserID().ToString();
                LoadUser();
                if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    ddlStatus.Enabled = true;
                }
                else
                {
                    ddlStatus.Enabled = false;
                }
            }
            else
            {
                UserID = Guid.NewGuid().ToString();
                BuildUserRoleDT(new List<KMPlatform.Entity.UserClientSecurityGroupMap>());
            }
        }
        private void SetupControlForNonAdmin()
        {
            txtAccessKey.Visible = false;
            txtAccessKey.Enabled = false;
            trKMStaff.Visible = false;
            if (getRequestUserID() > 0)
            {
                txtUserName.Enabled = false;
                btnResetPassword.Visible = true;
                txtPassword.Visible = true;
            }
            else
            {
                btnResetPassword.Visible = false;
                txtPassword.Visible = false;
            }
            ddlStatus.Enabled = false;
        }
        private void SetupControlForChannelAdmin()
        {
            txtAccessKey.Visible = false;
            txtAccessKey.Enabled = false;
            trKMStaff.Visible = false;
            if (getRequestUserID() > 0)
            {
                txtUserName.Enabled = false;
                btnResetPassword.Visible = true;
                txtPassword.Visible = true;
            }
            else
            {
                btnResetPassword.Visible = false;
                txtPassword.Visible = false;
            }
            ddlStatus.Enabled = false;
        }
        private void SetupControlsForSystemAdmin()
        {
            txtAccessKey.Visible = true;
            txtAccessKey.Enabled = true;
            if (getRequestUserID() > 0)
            {
                btnResetPassword.Visible = true;
                txtPassword.Visible = true;
                txtUserName.Enabled = false;
                ddlStatus.Enabled = true;
            }
            else
            {
                btnResetPassword.Visible = false;
                txtPassword.Visible = false;
            }
            trKMStaff.Visible = true;
        }
        private void FillRolesTable()
        {
            for (var i = 0; i < gvUserRoles.Rows.Count; i++)
            {
                if (gvUserRoles.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    var hfID = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlId);
                    var hfBaseChannel = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlBaseChannel);
                    var hfBaseChannelID = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlBaseChannelId);
                    var hfCustomer = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlCustomer);
                    var hfCustomerID = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlCustomerId);
                    var hfRole = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlRole);
                    var hfSecurityGroupID = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlSecurityGroupId);
                    var hfInactiveReason = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlInactiveReason);
                    var hfIsBCAdmin = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlIsBCAdmin);
                    var hfIsCAdmin = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlIsCAdmin);
                    var hfIsActive = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlIsActive);
                    var hfIsDeleted = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlIsDeleted);
                    var hfDisplay = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlDisplay);
                    var hfDoHardDelete = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlDoHardDelete);
                    var hfIsChannelRole = (HiddenField)gvUserRoles.Rows[i].FindControl(ControlIsChannelRole);
                    var row = dtUserRoles.NewRow();
                    row[ColumnId] = hfID.Value;
                    row[ColumnBaseChannel] = hfBaseChannel.Value;
                    row[ColumnBaseChannelId] = ParseInt32(hfBaseChannelID.Value);
                    row[ColumnCustomer] = hfCustomer.Value;
                    row[ColumnCustomerId] = ParseInt32(hfCustomerID.Value);
                    row[ColumnRole] = hfRole.Value;
                    row[ColumnSecurityGroupId] = ParseInt32(hfSecurityGroupID.Value);
                    row[ColumnInactiveReason] = hfInactiveReason.Value;
                    row[ColumnIsBcAdmin] = ParseBool(hfIsBCAdmin.Value);
                    row[ColumnIsCaAdmin] = ParseBool(hfIsCAdmin.Value);
                    row[ColumnIsActive] = ParseBool(hfIsActive.Value);
                    row[ColumnIsDeleted] = ParseBool(hfIsDeleted.Value);
                    row[ColumnDisplay] = ParseBool(hfDisplay.Value);
                    row[ColumnDoHardDelete] = ParseBool(hfDoHardDelete.Value);
                    row[ColumnIsChannelRole] = ParseBool(hfIsChannelRole.Value);
                    dtUserRoles.Rows.Add(row);
                }
            }
        }
        private void CreateUserRolesTable()
        {
            dtUserRoles = new DataTable();
            var dcRoleID = new DataColumn(ColumnId);
            var dcBaseChannel = new DataColumn(ColumnBaseChannel);
            var dcBaseChannelID = new DataColumn(ColumnBaseChannelId, typeof(int));
            var dcCustomer = new DataColumn(ColumnCustomer);
            var dcCustomerID = new DataColumn(ColumnCustomerId, typeof(int));
            var dcRole = new DataColumn(ColumnRole);
            var dcSecurityGroupID = new DataColumn(ColumnSecurityGroupId, typeof(int));
            var dcStatus = new DataColumn(ColumnInactiveReason);
            var dcIsBCAdmin = new DataColumn(ColumnIsBcAdmin, typeof(bool));
            var dcIsCAdmin = new DataColumn(ColumnIsCaAdmin, typeof(bool));
            var dcIsActive = new DataColumn(ColumnIsActive, typeof(bool));
            var dcIsDeleted = new DataColumn(ColumnIsDeleted, typeof(bool));
            var dcDisplay = new DataColumn(ColumnDisplay, typeof(bool));
            var dcDoHardDelete = new DataColumn(ColumnDoHardDelete, typeof(bool));
            var dcIsChannelRole = new DataColumn(ColumnIsChannelRole, typeof(bool));
            dtUserRoles.Columns.Add(dcRoleID);
            dtUserRoles.Columns.Add(dcBaseChannel);
            dtUserRoles.Columns.Add(dcBaseChannelID);
            dtUserRoles.Columns.Add(dcCustomer);
            dtUserRoles.Columns.Add(dcCustomerID);
            dtUserRoles.Columns.Add(dcRole);
            dtUserRoles.Columns.Add(dcSecurityGroupID);
            dtUserRoles.Columns.Add(dcStatus);
            dtUserRoles.Columns.Add(dcIsBCAdmin);
            dtUserRoles.Columns.Add(dcIsCAdmin);
            dtUserRoles.Columns.Add(dcIsDeleted);
            dtUserRoles.Columns.Add(dcIsActive);
            dtUserRoles.Columns.Add(dcDisplay);
            dtUserRoles.Columns.Add(dcDoHardDelete);
            dtUserRoles.Columns.Add(dcIsChannelRole);
        }









        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (dtUserRoles != null && dtUserRoles.Select("Display = true").Count() > 0)
            {
                gvUserRoles.Visible = true;
            }
            else
            {
                gvUserRoles.Visible = false;
            }
        }

        #region Data Load

        private void LoadBaseChannels()
        {
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                List<KMPlatform.Entity.ClientGroup> listBC = (new KMPlatform.BusinessLogic.ClientGroup()).Select(false);// AccountsBLL.BaseChannel.GetAll().OrderBy(x => x.BaseChannelName).ToList();


                ddlBaseChannel.DataSource = listBC.Where(x => x.IsActive == true).OrderBy(x => x.ClientGroupName);
                ddlBaseChannel.DataTextField = "ClientGroupName";
                ddlBaseChannel.DataValueField = "ClientGroupID";
                ddlBaseChannel.DataBind();
                ddlBaseChannel.Items.FindByValue(Master.UserSession.ClientGroupID.ToString()).Selected = true;
            }
            else
            {
                List<KMPlatform.Entity.ClientGroup> listBC = (new KMPlatform.BusinessLogic.ClientGroup()).SelectForAtLeastCustomerAdmin(Master.UserSession.CurrentUser.UserID);

                ddlBaseChannel.DataSource = listBC.OrderBy(x => x.ClientGroupName);
                ddlBaseChannel.DataTextField = "ClientGroupName";
                ddlBaseChannel.DataValueField = "ClientGroupID";
                ddlBaseChannel.DataBind();
                ddlBaseChannel.Items.FindByValue(Master.UserSession.ClientGroupID.ToString()).Selected = true;
            }
        }

        private void LoadCustomers()
        {
            List<KMPlatform.Entity.Client> listC = new List<KMPlatform.Entity.Client>();
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                listC = (new KMPlatform.BusinessLogic.Client()).SelectActiveForClientGroupLite(Convert.ToInt32(ddlBaseChannel.SelectedValue.ToString()));
            }
            else if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                listC = (new KMPlatform.BusinessLogic.Client()).SelectActiveForClientGroupLite(Convert.ToInt32(ddlBaseChannel.SelectedValue.ToString()));
            }
            else
            {
                listC = (new KMPlatform.BusinessLogic.Client()).SelectForAtLeastCustAdmin(Convert.ToInt32(ddlBaseChannel.SelectedValue.ToString()), Master.UserSession.CurrentUser.UserID);
            }
            //get current basechannel from dllBaseChannels 
            ddlCustomer.DataSource = listC.OrderBy(x => x.ClientName);
            ddlCustomer.DataTextField = "ClientName";
            ddlCustomer.DataValueField = "ClientID";
            ddlCustomer.DataBind();

            if (listC.Count > 0)
            {
                try
                {
                    ddlCustomer.Items.FindByValue(Master.UserSession.ClientID.ToString()).Selected = true;
                }
                catch
                {
                    if (ddlCustomer.Items.Count > 0)
                        ddlCustomer.Items[0].Selected = true;
                }
            }
        }

        private void LoadUser()
        {
            user = new KMPlatform.BusinessLogic.User().SelectUser(getRequestUserID(), true);

            if (KMPlatform.BusinessLogic.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {

            }
            else
            {
                if (!Master.UserSession.CurrentUserClientGroupClients.Exists(x => user.UserClientSecurityGroupMaps.Any(y => y.ClientID == x.ClientID)))
                {
                    throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = ECNCommon.Enums.SecurityExceptionType.RoleAccess };
                }
            }




            if (user != null)
            {
                int passLength = user.Password.Length;
                string maskedPass = string.Empty;
                for (int i = 0; i < passLength; i++)
                {
                    maskedPass += "*";
                }

                txtUserName.Text = user.UserName;
                txtPassword.Text = maskedPass;
                txtPhone.Text = user.Phone;
                txtAccessKey.Text = user.AccessKey.ToString();
                txtEmailAddress.Text = user.EmailAddress;
                txtFirstName.Text = user.FirstName;
                txtLastName.Text = user.LastName;
                ddlStatus.SelectedValue = user.Status.ToString().ToLower();
                rblKMStaff.SelectedValue = user.IsKMStaff ? ValueYes : ValueNo;
                btnAddRole.Visible = !user.IsPlatformAdministrator;
                //rblSysAdmin.SelectedValue = user.IsPlatformAdministrator ? "yes" : "no";
                BuildUserRoleDT(user.UserClientSecurityGroupMaps);
                LoadUserGroups(user.UserID);
                BindRolesGrid();

                if (!user.IsPlatformAdministrator)
                    btnAddRole.Visible = true;
                else
                    btnAddRole.Visible = false;

                if (user.Status == KMPlatform.Enums.UserStatus.Locked)
                {
                    btnAddRole.Visible = false;
                }
                else
                {
                    btnAddRole.Visible = true;
                }
            }
            else
            {
                throwECNException("User does not exist", phError, lblErrorMessagePhError);
            }
        }

        private void LoadUserGroups(int userID)
        {
            List<ECN_Framework_Entities.Communicator.UserGroup> userGroups = ECN_Framework_BusinessLayer.Communicator.UserGroup.Get(userID);
            IEnumerable<int> groupIDs = userGroups.Select(x => x.GroupID).Distinct();
            Dictionary<int, int> groupsForCust = new Dictionary<int, int>();
            List<int> custIDs = new List<int>();
            foreach (int i in groupIDs)
            {
                ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(i);
                if (g != null && g.GroupID > 0)
                {
                    if (!groupsForCust.ContainsKey(g.GroupID))
                        groupsForCust.Add(g.GroupID, g.CustomerID);
                    if (!custIDs.Contains(g.CustomerID))
                        custIDs.Add(g.CustomerID);
                }
            }
            foreach (int i in custIDs)
            {
                ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(i, false);

                var groups = groupsForCust.Where(p => p.Value == i).Select(p => p.Key).ToList();

                AddToViewState("RestrictGroups_" + c.PlatformClientID.ToString(), groups);
            }


        }

        private void BuildUserRoleDT(List<KMPlatform.Entity.UserClientSecurityGroupMap> roles)
        {
            if (dtUserRoles == null)
            {
                dtUserRoles = new DataTable();

                DataColumn dcRoleID = new DataColumn(ColumnId);
                DataColumn dcBaseChannel = new DataColumn("BaseChannel");
                DataColumn dcBaseChannelID = new DataColumn(ColumnBaseChannelId, typeof(int));
                //DataColumn dcClientGroupID = new DataColumn("ClientGroupID", typeof(int));
                DataColumn dcCustomer = new DataColumn("Customer");
                DataColumn dcCustomerID = new DataColumn(ColumnCustomerId, typeof(int));
                //DataColumn dcClientID = new DataColumn("ClientID", typeof(int));
                DataColumn dcRole = new DataColumn("Role");
                DataColumn dcSecurityGroupID = new DataColumn(ColumnSecurityGroupId, typeof(int));
                DataColumn dcStatus = new DataColumn(ColumnInactiveReason);
                DataColumn dcIsBCAdmin = new DataColumn(ColumnIsBcAdmin, typeof(bool));
                DataColumn dcIsCAdmin = new DataColumn(ColumnIsCaAdmin, typeof(bool));
                DataColumn dcIsActive = new DataColumn(ColumnIsActive, typeof(bool));
                DataColumn dcIsDeleted = new DataColumn(ColumnIsDeleted, typeof(bool));
                DataColumn dcDisplay = new DataColumn(ColumnDisplay, typeof(bool));
                DataColumn dcDoHardDelete = new DataColumn(ColumnDoHardDelete, typeof(bool));
                DataColumn dcIsChannelRole = new DataColumn(ColumnIsChannelRole, typeof(bool));

                dtUserRoles.Columns.Add(dcRoleID);
                dtUserRoles.Columns.Add(dcBaseChannel);
                dtUserRoles.Columns.Add(dcBaseChannelID);
                //dtUserRoles.Columns.Add(dcClientGroupID);
                dtUserRoles.Columns.Add(dcCustomer);
                dtUserRoles.Columns.Add(dcCustomerID);
                //dtUserRoles.Columns.Add(dcClientID);
                dtUserRoles.Columns.Add(dcRole);
                dtUserRoles.Columns.Add(dcSecurityGroupID);
                dtUserRoles.Columns.Add(dcStatus);
                dtUserRoles.Columns.Add(dcIsBCAdmin);
                dtUserRoles.Columns.Add(dcIsCAdmin);
                dtUserRoles.Columns.Add(dcIsDeleted);
                dtUserRoles.Columns.Add(dcIsActive);
                dtUserRoles.Columns.Add(dcDisplay);
                dtUserRoles.Columns.Add(dcDoHardDelete);
                dtUserRoles.Columns.Add(dcIsChannelRole);
            }

            Dictionary<string, List<int>> clientGroupClients = new Dictionary<string, List<int>>();//T1 = ClientGroupID, T2 = SecurityGroupID, T3 = ClientIDs

            if (KMPlatform.BusinessLogic.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {

            }
            else if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                List<KMPlatform.Entity.Client> clients = new KMPlatform.BusinessLogic.Client().SelectForClientGroupLite(Master.UserSession.ClientGroupID, false);
                roles = roles.Where(x => clients.Exists(y => y.ClientID == x.ClientID)).ToList();
            }
            else if (KMPlatform.BusinessLogic.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                roles = roles.Where(x => x.ClientID == Master.UserSession.ClientID).ToList();
            }

            List<KMPlatform.Entity.ClientGroup> lgp = new List<KMPlatform.Entity.ClientGroup>();
            List<KMPlatform.Entity.SecurityGroup> sgp = new List<KMPlatform.Entity.SecurityGroup>();
            Dictionary<int, List<KMPlatform.Entity.ClientGroupClientMap>> dictCGCM = new Dictionary<int, List<KMPlatform.Entity.ClientGroupClientMap>>();
            foreach (KMPlatform.Entity.UserClientSecurityGroupMap uc in roles)
            {
                KMPlatform.Entity.Client c = (new KMPlatform.BusinessLogic.Client()).Select(uc.ClientID);
                List<KMPlatform.Entity.ClientGroupClientMap> cgcm = (new KMPlatform.BusinessLogic.ClientGroupClientMap()).SelectForClientID(uc.ClientID);
                KMPlatform.Entity.ClientGroup cg = lgp.SingleOrDefault(g => g.ClientGroupID == cgcm.First(x => x.ClientID == uc.ClientID).ClientGroupID);
                if (cg == null)
                {
                    cg = (new KMPlatform.BusinessLogic.ClientGroup()).Select(cgcm.First(x => x.ClientID == uc.ClientID).ClientGroupID);
                    lgp.Add(cg);
                }
                KMPlatform.Entity.SecurityGroup sg = sgp.SingleOrDefault(s => s.SecurityGroupID == uc.SecurityGroupID);
                if (sg == null)
                {
                    sg = (new KMPlatform.BusinessLogic.SecurityGroup()).Select(uc.SecurityGroupID, false);
                    sgp.Add(sg);
                }
                //ECN_Framework_Entities.Accounts.Customer listCust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(c.ClientID, false);
                //ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByPlatformClientGroupID(cg.ClientGroupID);

                DataRow dr = dtUserRoles.NewRow();
                dr[ColumnId] = uc.UserClientSecurityGroupMapID.ToString();
                dr["BaseChannel"] = cg.ClientGroupName;
                dr[ColumnBaseChannelId] = sg.ClientGroupID;
                dr[ColumnIsChannelRole] = sg.ClientGroupID != 0;
                dr["Customer"] = c.ClientName;
                dr[ColumnCustomerId] = uc.ClientID;
                dr["Role"] = sg.SecurityGroupName;
                dr[ColumnSecurityGroupId] = sg.SecurityGroupID;
                dr[ColumnInactiveReason] = uc.InactiveReason;
                dr[ColumnIsBcAdmin] = sg.AdministrativeLevel == SecurityGroupAdministrativeLevel.ChannelAdministrator;
                dr[ColumnIsCaAdmin] = sg.AdministrativeLevel == SecurityGroupAdministrativeLevel.Administrator;
                dr[ColumnIsActive] = uc.IsActive;
                dr[ColumnIsDeleted] = false;
                dr[ColumnDoHardDelete] = false;
                if (sg.ClientGroupID > 0)
                {
                    List<KMPlatform.Entity.ClientGroupClientMap> clients;
                    if (dictCGCM.ContainsKey(sg.ClientGroupID))
                        clients = dictCGCM[sg.ClientGroupID];
                    else
                    {
                        clients = new KMPlatform.BusinessLogic.ClientGroupClientMap().SelectForClientGroup(sg.ClientGroupID);
                        dictCGCM.Add(sg.ClientGroupID, clients);
                    }
                    if (!clientGroupClients.ContainsKey(sg.ClientGroupID.ToString() + "_" + sg.SecurityGroupID.ToString()))
                    {
                        if (sg.AdministrativeLevel == KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator)
                        {
                            if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                            {
                                dr[ColumnDisplay] = true;
                                dtUserRoles.Rows.Add(dr);
                                clientGroupClients.Add(sg.ClientGroupID.ToString() + "_" + sg.SecurityGroupID.ToString(), clients.Select(x => x.ClientID).ToList());
                            }
                        }
                        else
                        {
                            dr[ColumnDisplay] = true;
                            dtUserRoles.Rows.Add(dr);
                            clientGroupClients.Add(sg.ClientGroupID.ToString() + "_" + sg.SecurityGroupID.ToString(), clients.Select(x => x.ClientID).ToList());
                        }
                    }
                    else
                    {
                        dr[ColumnDisplay] = false;
                        dtUserRoles.Rows.Add(dr);
                    }
                }
                else
                {
                    dr[ColumnDisplay] = true;
                    dtUserRoles.Rows.Add(dr);

                }

            }

        }



        #endregion

        public bool IsContentCreator(int userID)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNCommon.ECNError> errorList = new List<ECNCommon.ECNError>();
            using (TransactionScope supressscope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (ECN_Framework_BusinessLayer.Communicator.Content.CreatedUserExists(userID))
                {
                    errorList.Add(new ECNCommon.ECNError(ECNCommon.Enums.Entity.Content, Method, "User is assigned to Content"));
                }
                if (ECN_Framework_BusinessLayer.Communicator.Layout.CreatedUserExists(userID))
                {
                    errorList.Add(new ECNCommon.ECNError(ECNCommon.Enums.Entity.Layout, Method, "User is assigned to Layout"));
                }
                if (ECN_Framework_BusinessLayer.Communicator.Blast.CreatedUserExists(userID))
                {
                    errorList.Add(new ECNCommon.ECNError(ECNCommon.Enums.Entity.Blast, Method, "User is assigned to Blast"));
                }
                supressscope.Complete();
            }

            if (errorList.Count > 0)
            {
                throwECNException("User Name cannot be updated because the user is a current content creator", phError, lblErrorMessagePhError);
                return true;
            }
            return false;
        }

        private void throwECNException(string message, PlaceHolder phError, Label lblErrorMessage)
        {
            ECNCommon.ECNError ecnError = new ECNCommon.ECNError(ECNCommon.Enums.Entity.User, ECNCommon.Enums.Method.Get, message);
            List<ECNCommon.ECNError> errorList = new List<ECNCommon.ECNError> { ecnError };
            setECNError(new ECNCommon.ECNException(errorList, ECNCommon.Enums.ExceptionLayer.WebSite), phError, lblErrorMessage);
        }

        private void setECNError(ECNCommon.ECNException ecnException, PlaceHolder phError, Label lblErrorMessage)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECNCommon.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private string GenerateRandomPassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }





        public void Save(object sender, EventArgs e)
        {
            try
            {
                var fullName =
                    string.Format("{0} {1}",
                                    Master.UserSession.CurrentUser.FirstName,
                                    Master.UserSession.CurrentUser.LastName);
                if (!string.IsNullOrWhiteSpace(fullName))
                {
                    if (SaveNoFullName())
                    {
                        return;
                    }
                }
                else
                {
                    throwECNException(ErrorNoNames, phError, lblErrorMessagePhError);
                }
            }
            catch (ECNCommon.ECNException ecn)
            {
                setECNError(ecn, phError, lblErrorMessage);
            }
            catch (Exception ex)
            {
                KMCommonEntity.ApplicationLog.LogCriticalError(
                    ex,
                    MethodBtnSaveClick,
                    Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingComonApplication]));
                throwECNException(ErrorOccured, phError, lblErrorMessagePhError);
            }
        }
        private bool SaveNoFullName()
        {
            var currentUser = new User();
            var uWorker = new KMPlatformBusiness.User();
            var isUpdate = false;
            if (getRequestUserID() > 0)
            {
                currentUser = uWorker.SelectUser(getRequestUserID(), true);
            }
            if (dtUserRoles.Rows.Count > 0 || currentUser.IsPlatformAdministrator)
            {
                if (FillUserId(currentUser, uWorker, ref isUpdate))
                {
                    return true;
                }
                FillUserFields(currentUser);
                var filterExpression =
                    string.Format(
                        CustomerFilerTemplate,
                        currentUser.DefaultClientID,
                        currentUser.DefaultClientGroupID);
                if ((dtUserRoles.Select(filterExpression).Length > 0 || currentUser.DefaultClientGroupID <= 0) &&
                    !currentUser.IsPlatformAdministrator)
                {
                    FillUserClientId(currentUser);
                }
                else if (currentUser.IsPlatformAdministrator && getRequestUserID() < 0)
                {
                    currentUser.DefaultClientID =
                        Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingMasterClientId]);
                    currentUser.DefaultClientGroupID =
                        Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingMasterClientGroupId]);
                }
                if (BusinessCommunicator.Email.IsValidEmailAddress(currentUser.EmailAddress))
                {
                    SaveUserInGroups(currentUser, uWorker, isUpdate);
                    Response.Redirect(DefaultAspx, false);
                }
                else
                {
                    throwECNException(ErrorEmailInvalid, phError, lblErrorMessagePhError);
                }
            }
            else
            {
                throwECNException(ErrorNoRoleForUser, phError, lblErrorMessagePhError);
            }
            return false;
        }
        private void FillUserClientId(User currentUser)
        {
            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }
            var defaultClientGroupId = NoId;
            var defaultClientId = NoId;
            try
            {
                var channelId = dtUserRoles.Select(FilterIsActiveOrBaseChannelTemplate)[0][ColumnBaseChannelId];
                defaultClientGroupId = Convert.ToInt32(channelId.ToString());
            }
            catch
            {
                try
                {
                    var customerId = dtUserRoles.Select(FilterIsActiveOrCustomerIdTemplate)[0][ColumnCustomerId];
                    defaultClientId = Convert.ToInt32(customerId.ToString());
                }
                catch
                {
                    try
                    {
                        //catch here to try to assign default client/clientgroupid from roles that the editing user can't see
                        var listHiddenRoles =
                            new KMPlatformBusiness.UserClientSecurityGroupMap().SelectForUser(
                                currentUser.UserID);
                        if (listHiddenRoles?.Count > 0)
                        {
                            defaultClientId = listHiddenRoles.First(x => x.IsActive).ClientID;
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.Write(ex.Message);
                    }
                }
            }
            if (defaultClientGroupId > 0)
            {
                currentUser.DefaultClientGroupID = defaultClientGroupId;
                var cWorker = new KMPlatformBusiness.ClientGroupClientMap();
                var defaultClient =
                    cWorker.SelectForClientGroup(defaultClientGroupId).Where(x => x.IsActive).ToList();
                currentUser.DefaultClientID = defaultClient[0].ClientID;
            }
            else if (defaultClientId > 0)
            {
                currentUser.DefaultClientID = defaultClientId;
                var cWorker = new KMPlatformBusiness.ClientGroupClientMap();
                var defaultClient =
                    cWorker.SelectForClientID(defaultClientId).Where(x => x.IsActive).ToList();
                currentUser.DefaultClientGroupID = defaultClient[0].ClientGroupID;
            }
        }
        private void SaveUserInGroups(User currentUser, KMPlatformBusiness.User uWorker, bool isUpdate)
        {
            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }
            if (uWorker == null)
            {
                throw new ArgumentNullException(nameof(uWorker));
            }
            //Wrapping all save calls in a transaction so we can rollback if we have an issue
            var userId = NoId;
            List<UserClientSecurityGroupMap> listForDblOptIn = null;
            ECN_Framework_Entities.Communicator.EmailDirect urlForDBLOptinLink = null;
            var userSaveComplete = false;
            var existingUser = currentUser.UserClientSecurityGroupMaps.Any(x => x.IsActive);
            try
            {
                using (var scope = new TransactionScope())
                {
                    userId = uWorker.Save(currentUser);
                    currentUser.UserID = userId;
                    listForDblOptIn = DoUserClientSecurityGroups(currentUser, isUpdate);
                    userSaveComplete = true;
                    scope.Complete();
                }
                SaveUserGroups(userId);
                if (listForDblOptIn.Count > 0)
                {
                    urlForDBLOptinLink = DoDoubleOptinEmail(listForDblOptIn, currentUser, existingUser);
                }
            }
            catch (Exception ex1)
            {
                if (userSaveComplete)
                {
                    KMCommonEntity.ApplicationLog.LogCriticalError(
                        ex1,
                        ErrorSaveUserDetails,
                        Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingComonApplication]),
                        CreateNote(currentUser, listForDblOptIn, urlForDBLOptinLink));
                }
                else
                {
                    throw;
                }
            }
        }
        private void SaveUserGroups(int userId)
        {
            foreach (var ucsgm in dtUserRoles.Select(FilterIsDeletedAndCustomer))
            {
                if (!(bool)ucsgm[ColumnIsBcAdmin] && !(bool)ucsgm[ColumnIsChannelRole])
                {
                    DoUserGroupSave(
                        userId,
                        Convert.ToInt32(ucsgm[ColumnCustomerId].ToString()),
                        Convert.ToInt32(ucsgm[ColumnBaseChannelId].ToString()));
                }
            }
        }
        private void FillUserFields(User currentUser)
        {
            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }
            currentUser.UserName = txtUserName.Text.Trim();
            currentUser.EmailAddress = txtEmailAddress.Text.Trim();
            currentUser.FirstName = txtFirstName.Text.Trim();
            currentUser.LastName = txtLastName.Text.Trim();
            currentUser.Phone = txtPhone.Text.Trim();
            currentUser.IsAccessKeyValid = true;
            currentUser.IsKMStaff = string.Equals(rblKMStaff.SelectedValue, ValueYes, StringComparison.OrdinalIgnoreCase);
        }
        private bool FillUserId(User currentUser, KMPlatformBusiness.User uWorker, ref bool isUpdate)
        {
            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }
            if (uWorker == null)
            {
                throw new ArgumentNullException(nameof(uWorker));
            }
            if (getRequestUserID() > 0)
            {
                currentUser.UpdatedByUserID = Master.UserSession.CurrentUser.UserID;
                currentUser.DateUpdated = DateTime.Now;
                currentUser.Status =
                    string.Equals(ddlStatus.SelectedValue, StatusActive, StringComparison.OrdinalIgnoreCase)
                        ? KMPlatform.Enums.UserStatus.Active
                        : string.Equals(ddlStatus.SelectedValue, StatusDisabled, StringComparison.OrdinalIgnoreCase) ?
                            KMPlatform.Enums.UserStatus.Disabled :
                            KMPlatform.Enums.UserStatus.Locked;
                currentUser.IsActive = currentUser.Status.Equals(KMPlatform.Enums.UserStatus.Active);
                isUpdate = true;
                if (!currentUser.UserName.Equals(txtUserName.Text.Trim()))
                {
                    if (uWorker.Validate_UserName(txtUserName.Text.Trim(), currentUser.UserID))
                    {
                        throwECNException(ErrorUserAlreadyExists, phError, lblErrorMessagePhError);
                        return true;
                    }
                }
            }
            else
            {
                currentUser.CreatedByUserID = Master.UserSession.CurrentUser.UserID;
                currentUser.DateCreated = DateTime.Now;
                currentUser.AccessKey = Guid.NewGuid();
                currentUser.Password = GenerateRandomPassword();
                currentUser.Status = KMPlatform.Enums.UserStatus.Disabled;
                currentUser.IsActive = false;
                if (uWorker.Validate_UserName(txtUserName.Text.Trim(), NoId))
                {
                    throwECNException(ErrorUserAlreadyExists, phError, lblErrorMessagePhError);
                    return true;
                }
            }
            return false;
        }






        private string CreateNote(KMPlatform.Entity.User user, List<KMPlatform.Entity.UserClientSecurityGroupMap> roles, ECN_Framework_Entities.Communicator.EmailDirect ed)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----EmailDirect Object");
            if (ed != null)
            {
                foreach (System.Reflection.PropertyInfo pi in ed.GetType().GetProperties())
                {
                    try
                    {
                        sb.AppendLine("--" + pi.Name + " - " + (pi.GetValue(ed) != null ? pi.GetValue(ed) : "NULL").ToString());
                    }
                    catch
                    {
                        sb.AppendLine("--" + pi.Name + " - No Value");
                    }
                }
            }
            else
            {
                sb.AppendLine("-- Email Direct Object Null");
            }


            return sb.ToString();
        }

        private List<UserClientSecurityGroupMap> DoUserClientSecurityGroups(User user, bool isUpdate)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var ucsgmWorker = new KMPlatformBusiness.UserClientSecurityGroupMap();
            var listForDblOptIn = new List<UserClientSecurityGroupMap>();

            if (!user.IsPlatformAdministrator)//if not a platform user, then do security group stuff
            {
                var idsToUpdate = new List<string>();

                var sgoiWorker = new KMPlatformBusiness.SecurityGroupOptIn();

                if (isUpdate)//Have to do updates as well as inserts for new roles
                {
                    var sGsDone = new List<int>();
                    //Loop through current userclientsecuritygroups for updates
                    foreach (var ucsgm in user.UserClientSecurityGroupMaps)
                    {
                        var filterExpression = string.Format(IdFilterTemplate, ucsgm.UserClientSecurityGroupMapID);
                        var drArray = dtUserRoles.Select(filterExpression);
                        if (drArray.Length > 0)
                        {
                            var dr = drArray[0];
                            if (!(bool)dr[ColumnIsActive] && (bool)dr[ColumnIsDeleted])
                            {
                                //Do hard delete if we have to
                                if ((bool)dr[ColumnDoHardDelete])
                                {
                                    ucsgmWorker.Delete(ucsgm.UserClientSecurityGroupMapID);
                                }
                                else
                                {
                                    if (ucsgm.InactiveReason.ToLower().Equals(InactiveReasonPending))
                                    {
                                        sgoiWorker.Delete(ucsgm.SecurityGroupID, ucsgm.UserID);
                                    }
                                    ucsgm.IsActive = false;
                                    ucsgm.InactiveReason = ValueDisabled;
                                    ucsgm.DateUpdated = DateTime.Now;
                                    ucsgm.UpdatedByUserID = Master.UserSession.CurrentUser.UserID;
                                    ucsgmWorker.Save(ucsgm);
                                }
                                if (!sGsDone.Contains(ucsgm.SecurityGroupID))
                                {
                                    sGsDone.Add(ucsgm.SecurityGroupID);
                                    var sgoiList =
                                        new KMPlatformBusiness
                                                .SecurityGroupOptIn()
                                                .SelectBySecurityGroup_UserID(ucsgm.SecurityGroupID, ucsgm.UserID);
                                    foreach (var sgoi in sgoiList)
                                    {
                                        sgoiWorker.Delete(sgoi.SecurityGroupOptInID);
                                    }
                                }
                            }
                            else
                            {
                                ucsgm.SecurityGroupID = (int)dr[ColumnSecurityGroupId];
                                ucsgm.ClientID = (int)dr[ColumnCustomerId];
                                ucsgm.DateUpdated = DateTime.Now;
                                ucsgm.UpdatedByUserID = Master.UserSession.CurrentUser.UserID;
                                ucsgmWorker.Save(ucsgm);
                            }

                            idsToUpdate.Add(ucsgm.UserClientSecurityGroupMapID.ToString());
                        }
                    }

                }
                ProcessGroupsTable(user, idsToUpdate, ucsgmWorker, listForDblOptIn);
            }
            else
            {
                ProcessPlatformAdmin(ucsgmWorker);
            }
            return listForDblOptIn;
        }

        private void ProcessGroupsTable(
            User user,
            ICollection<string> idsToUpdate,
            KMPlatformBusiness.UserClientSecurityGroupMap ucsgmWorker,
            ICollection<UserClientSecurityGroupMap> listForDblOptIn)
        {
            if (idsToUpdate == null)
            {
                throw new ArgumentNullException(nameof(idsToUpdate));
            }

            //Now loop through our datatable, filter out what's already been updated and do inserts
            foreach (DataRow dr in dtUserRoles.Rows)
            {
                if (!idsToUpdate.Contains(dr[ColumnId].ToString()) && !(bool)dr[ColumnIsDeleted])
                {
                    if ((bool)dr[ColumnIsBcAdmin]) //Base Channel Admin
                    {
                        ProcessBaseChannelAdmin(user, ucsgmWorker, listForDblOptIn, dr);
                    }
                    else if ((bool)dr[ColumnIsCaAdmin]) //Customer Admin
                    {
                        ProcessCustomerAdmin(user, ucsgmWorker, listForDblOptIn, dr);
                    }
                    else if ((int)dr[ColumnBaseChannelId] > 0 && (bool)dr[ColumnIsChannelRole])
                    {
                        ProcessClient(user, ucsgmWorker, listForDblOptIn, dr);
                    }
                    else //Any other role
                    {
                        ProcessOtherRole(user, ucsgmWorker, listForDblOptIn, dr);
                    }
                }
            }
        }

        private void ProcessOtherRole(
            User user,
            KMPlatformBusiness.UserClientSecurityGroupMap ucsgmWorker,
            ICollection<UserClientSecurityGroupMap> listForDblOptIn,
            DataRow row)
        {
            Guard.NotNull(user, nameof(user));
            Guard.NotNull(ucsgmWorker, nameof(ucsgmWorker));
            Guard.NotNull(listForDblOptIn, nameof(listForDblOptIn));
            Guard.NotNull(row, nameof(row));

            var ucsgmCheck = new KMPlatformBusiness.UserClientSecurityGroupMap().SelectForUser(user.UserID);
            var ucsgm = new UserClientSecurityGroupMap();
            if (ucsgmCheck
                .Any(
                    x => x.ClientID == (int)row[ColumnCustomerId] &&
                         x.SecurityGroupID == (int)row[ColumnSecurityGroupId]))
            {
                ucsgm = ucsgmCheck
                    .First(
                        x => x.ClientID == (int)row[ColumnCustomerId] &&
                             x.SecurityGroupID == (int)row[ColumnSecurityGroupId]);
            }
            else
            {
                ucsgm = new UserClientSecurityGroupMap();
            }

            ucsgm.SecurityGroupID = (int)row[ColumnSecurityGroupId];
            ucsgm.ClientID = (int)row[ColumnCustomerId];
            ucsgm.CreatedByUserID = Master.UserSession.CurrentUser.UserID;
            ucsgm.DateCreated = DateTime.Now;
            ucsgm.InactiveReason = row[ColumnInactiveReason].ToString();
            ucsgm.IsActive = (bool)row[ColumnIsActive];
            ucsgm.UserID = user.UserID;
            ucsgm.UserClientSecurityGroupMapID = ucsgmWorker.Save(ucsgm);
            if (ucsgm.InactiveReason.ToLower().Equals(InactiveReasonPending))
            {
                listForDblOptIn.Add(ucsgm);
            }
        }

        private void ProcessClient(
            User user,
            KMPlatformBusiness.UserClientSecurityGroupMap ucsgmWorker,
            ICollection<UserClientSecurityGroupMap> listForDblOptIn,
            DataRow dr)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (ucsgmWorker == null)
            {
                throw new ArgumentNullException(nameof(ucsgmWorker));
            }

            if (listForDblOptIn == null)
            {
                throw new ArgumentNullException(nameof(listForDblOptIn));
            }

            if (dr == null)
            {
                throw new ArgumentNullException(nameof(dr));
            }

            var sgWorker = new KMPlatformBusiness.SecurityGroup();
            var ucsgmCheck = new KMPlatformBusiness.UserClientSecurityGroupMap().SelectForUser(user.UserID);
            var ucsgm = new UserClientSecurityGroupMap();
            var sgCurrent =
                sgWorker
                    .SelectForClientGroup((int)dr[ColumnBaseChannelId], false)
                    .First(x => x.SecurityGroupID == (int)dr[ColumnSecurityGroupId]);
            var listClients =
                (new KMPlatformBusiness.Client()).SelectForClientGroup(sgCurrent.ClientGroupID, false);
            foreach (Client c in listClients)
            {
                if (ucsgmCheck.Any(x => x.ClientID == c.ClientID && x.SecurityGroupID == sgCurrent.SecurityGroupID))
                {
                    ucsgm = ucsgmCheck.First(x =>
                        x.ClientID == c.ClientID && x.SecurityGroupID == sgCurrent.SecurityGroupID);
                }
                else
                {
                    ucsgm = new UserClientSecurityGroupMap();
                }

                ucsgm.SecurityGroupID = sgCurrent.SecurityGroupID;
                ucsgm.ClientID = c.ClientID;
                ucsgm.CreatedByUserID = Master.UserSession.CurrentUser.UserID;
                ucsgm.DateCreated = DateTime.Now;
                ucsgm.InactiveReason = dr[ColumnInactiveReason].ToString();
                ucsgm.IsActive = (bool)dr[ColumnIsActive];
                ucsgm.UserID = user.UserID;
                ucsgm.UserClientSecurityGroupMapID = ucsgmWorker.Save(ucsgm);
                if (string.Equals(ucsgm.InactiveReason, InactiveReasonPending, StringComparison.OrdinalIgnoreCase))
                {
                    listForDblOptIn.Add(ucsgm);
                }
            }
        }

        private void ProcessCustomerAdmin(
            User user,
            KMPlatformBusiness.UserClientSecurityGroupMap ucsgmWorker,
            ICollection<UserClientSecurityGroupMap> listForDblOptIn,
            DataRow dr)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (ucsgmWorker == null)
            {
                throw new ArgumentNullException(nameof(ucsgmWorker));
            }

            if (listForDblOptIn == null)
            {
                throw new ArgumentNullException(nameof(listForDblOptIn));
            }

            if (dr == null)
            {
                throw new ArgumentNullException(nameof(dr));
            }

            var sgWorker = new KMPlatformBusiness.SecurityGroup();
            var ucsgmCheck = new KMPlatformBusiness.UserClientSecurityGroupMap().SelectForUser(user.UserID);
            var ucsgm = new UserClientSecurityGroupMap();
            var sgCurrent =
                sgWorker
                    .SelectForClient((int)dr[ColumnCustomerId], false)
                    .First(x => x.AdministrativeLevel == SecurityGroupAdministrativeLevel.Administrator);

            if (ucsgmCheck.Any(
                x => x.ClientID == (int)dr[ColumnCustomerId] &&
                     x.SecurityGroupID == sgCurrent.SecurityGroupID))
            {
                ucsgm = ucsgmCheck.First(
                    x => x.ClientID == (int)dr[ColumnCustomerId] &&
                         x.SecurityGroupID == sgCurrent.SecurityGroupID);
            }
            else
            {
                ucsgm = new UserClientSecurityGroupMap();
            }

            ucsgm.SecurityGroupID = sgCurrent.SecurityGroupID;
            ucsgm.ClientID = sgCurrent.ClientID;
            ucsgm.DateCreated = DateTime.Now;
            ucsgm.CreatedByUserID = Master.UserSession.CurrentUser.UserID;
            ucsgm.InactiveReason = dr[ColumnInactiveReason].ToString();
            ucsgm.IsActive = (bool)dr[ColumnIsActive];
            ucsgm.UserID = user.UserID;
            ucsgm.UserClientSecurityGroupMapID = ucsgmWorker.Save(ucsgm);
            if (ucsgm.InactiveReason.ToLower().Equals(InactiveReasonPending))
            {
                listForDblOptIn.Add(ucsgm);
            }
        }

        private void ProcessBaseChannelAdmin(
            User user,
            KMPlatformBusiness.UserClientSecurityGroupMap ucsgmWorker,
            ICollection<UserClientSecurityGroupMap> listForDblOptIn,
            DataRow dr)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (ucsgmWorker == null)
            {
                throw new ArgumentNullException(nameof(ucsgmWorker));
            }

            if (listForDblOptIn == null)
            {
                throw new ArgumentNullException(nameof(listForDblOptIn));
            }

            if (dr == null)
            {
                throw new ArgumentNullException(nameof(dr));
            }

            var sgWorker = new KMPlatformBusiness.SecurityGroup();
            var ucsgmCheck = new KMPlatformBusiness.UserClientSecurityGroupMap().SelectForUser(user.UserID);
            var ucsgm = new UserClientSecurityGroupMap();
            var sgCurrent = sgWorker
                .SelectForClientGroup((int)dr[ColumnBaseChannelId], false)
                .First(x => x.AdministrativeLevel == SecurityGroupAdministrativeLevel.ChannelAdministrator);
            var listClients =
                (new KMPlatformBusiness.Client()).SelectForClientGroupLite(sgCurrent.ClientGroupID, false);
            foreach (var client in listClients)
            {
                if (ucsgmCheck.Any(x =>
                    x.ClientID == client.ClientID && x.SecurityGroupID == sgCurrent.SecurityGroupID))
                {
                    ucsgm =
                        ucsgmCheck.First(
                            x =>
                                x.ClientID == client.ClientID &&
                                x.SecurityGroupID == sgCurrent.SecurityGroupID);
                }
                else
                {
                    ucsgm = new UserClientSecurityGroupMap();
                }

                ucsgm.SecurityGroupID = sgCurrent.SecurityGroupID;
                ucsgm.ClientID = client.ClientID;
                ucsgm.DateCreated = DateTime.Now;
                ucsgm.CreatedByUserID = Master.UserSession.CurrentUser.UserID;
                ucsgm.InactiveReason = dr[ColumnInactiveReason].ToString();
                ucsgm.IsActive = (bool)dr[ColumnIsActive];
                ucsgm.UserID = user.UserID;
                ucsgm.UserClientSecurityGroupMapID = ucsgmWorker.Save(ucsgm);
                if (ucsgm.InactiveReason.ToLower().Equals(InactiveReasonPending))
                {
                    listForDblOptIn.Add(ucsgm);
                }
            }
        }

        private void ProcessPlatformAdmin(KMPlatformBusiness.UserClientSecurityGroupMap ucsgmWorker)
        {
            if (ucsgmWorker == null)
            {
                throw new ArgumentNullException(nameof(ucsgmWorker));
            }

            //Platform admins don't need to be in any security groups, so hard delete them to avoid confusion
            foreach (DataRow dr in dtUserRoles.Rows)
            {
                if (!dr[ColumnId].ToString().Contains(DashDelim))
                {
                    var securityGroupMapId = Convert.ToInt32(dr[ColumnId].ToString());
                    ucsgmWorker.Delete(securityGroupMapId);
                }
            }
        }

        private ECN_Framework_Entities.Communicator.EmailDirect DoDoubleOptinEmail(List<KMPlatform.Entity.UserClientSecurityGroupMap> listForDblOptIn, KMPlatform.Entity.User currentUser, bool existingUser)
        {
            Guid newSetID = Guid.NewGuid();
            KMPlatform.BusinessLogic.SecurityGroupOptIn sgoiWorker = new KMPlatform.BusinessLogic.SecurityGroupOptIn();
            KMPlatform.BusinessLogic.SecurityGroup sgWorker = new KMPlatform.BusinessLogic.SecurityGroup();
            bool sendEmail = false;
            ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
            try
            {
                bool isSysAdmin = currentUser.IsPlatformAdministrator;
                foreach (KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in listForDblOptIn)
                {
                    KMPlatform.Entity.SecurityGroupOptIn sgoi = new KMPlatform.Entity.SecurityGroupOptIn();
                    KMPlatform.Entity.SecurityGroup sgCurrent = sgWorker.Select(ucsgm.SecurityGroupID, false);
                    sgoi.ClientID = ucsgm.ClientID;
                    if (sgCurrent.ClientGroupID.Equals(-1))
                        sgoi.ClientGroupID = null;
                    else
                        sgoi.ClientGroupID = sgCurrent.ClientGroupID;
                    sgoi.CreatedByUserID = Master.UserSession.CurrentUser.UserID;
                    sgoi.HasAccepted = false;
                    sgoi.SecurityGroupID = ucsgm.SecurityGroupID;
                    sgoi.SendTime = DateTime.Now;
                    sgoi.SetID = newSetID;
                    sgoi.UserClientSecurityGroupMapID = ucsgm.UserClientSecurityGroupMapID;
                    sgoi.UserID = currentUser.UserID;
                    sgoiWorker.Save(sgoi);
                    sendEmail = true;
                }


                if (sendEmail || isSysAdmin)
                {

                    KM.Common.Entity.Encryption ecLink = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));

                    string query = "?setID=" + newSetID.ToString() + "&userID=" + currentUser.UserID.ToString() + "&existing=" + existingUser.ToString();
                    string encryptedQuery = KM.Common.Encryption.Base64Encrypt(query, ecLink);

                    string redirectURL = ConfigurationManager.AppSettings["MVCActivity_DomainPath"].ToString() + "/User/Accept/" + encryptedQuery;
                    KMPlatform.Entity.User AdminUser = Master.UserSession.CurrentUser;
                    //Send the email to the user

                    string fromName = "";
                    if (string.IsNullOrEmpty(AdminUser.FirstName + " " + AdminUser.LastName))
                        fromName = AdminUser.EmailAddress;
                    else
                        fromName = AdminUser.FirstName + " " + AdminUser.LastName;
                    ed.CreatedDate = DateTime.Now;
                    ed.CreatedUserID = AdminUser.UserID;
                    ed.EmailAddress = currentUser.EmailAddress;
                    ed.EmailSubject = fromName + " has added you to KM Platform";
                    ed.FromName = fromName;
                    ed.Content = GetConfirmationContent(currentUser, AdminUser, redirectURL, listForDblOptIn, existingUser);
                    ed.SendTime = DateTime.Now;
                    ed.ReplyEmailAddress = "info@knowledgemarketing.com";
                    ed.CustomerID = Master.UserSession.CurrentCustomer.CustomerID;
                    ed.Process = "New User Initial Email";
                    ed.Source = "ECN Accounts";

                    ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
                }
            }
            catch (Exception ex)
            {
                return ed;
            }

            return ed;

        }

        private string GetConfirmationContent(KMPlatform.Entity.User newUser, KMPlatform.Entity.User adminUser, string redirectURL, List<KMPlatform.Entity.UserClientSecurityGroupMap> roles, bool existingUser)
        {
            string content = "";
            if (!existingUser)
            {
                content = @"<div style='text-align:center;'>
    <div style='width:60%; margin:0 auto;'>
        <table style='width:100%;'>
            <tbody>
                <tr>
                    <td colspan='3' style='text-align:center;'><img alt='' src='http://images.ecn5.com/KMNew/KMNewWebLogo.jpg' /></td>
                </tr>
                <tr>
                    <td>
                        <p>Dear %%FirstName%%,<br /><br />%%CreatedByUserName%% has created a temporary username and granted you access to the Knowledge Marketing Platform with the following role(s):<br /><br />
                         %%Roles%%
                        <br />The newly created temporary username for this access is %%UserName%%.<br /><br />To accept this new role, click on the link below where you can continue to create this new username, 
                        create a different new username or you can associate with an existing username</p>
                        <br />
                        <div id='redirectbutton' style='margin-right:auto;margin-left:auto;width:200px;text-align:center; border-radius:8px; background-color: #FFFFFF;  moz-border-radius: 8px;  -webkit-border-radius: 8px;  border: 2px solid #000000;  '>
                            <p style='width:200px;height:100%;'><a href='%%RedirectLink%%' style='color:black;text-decoration:none;width:100%;height:100%;'>Accept Invitation</a></p>
                        </div>
                        <br />
                        <p>This is an automated notification. Please do not reply to this email. If you have questions or need assistance in setting up your user account, please contact %%CreatedByUserName%% at %%CreatedByUserEmailAddress%%</p>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>";
            }
            else
            {
                content = @"<div style='text-align:center;'>
    <div style='width:60%; margin:0 auto;'>
        <table style='width:100%;'>
            <tbody>
                <tr>
                    <td colspan='3' style='text-align:center;'><img alt='' src='http://images.ecn5.com/KMNew/KMNewWebLogo.jpg' /></td>
                </tr>
                <tr>
                    <td>
                        <p>Dear %%FirstName%%,<br /><br />%%CreatedByUserName%% has granted you access to the Knowledge Marketing Platform with the following role(s):<br /><br />
                         %%Roles%%
                        <br />The username for this access is %%UserName%%.<br /><br />To accept this new role, click on the link below where you can continue to associate with this existing username
                         or you can create a new username.
                        </p>
                        <br />
                        <div id='redirectbutton' style='margin-right:auto;margin-left:auto;width:200px;text-align:center; border-radius:8px; background-color: #FFFFFF;  moz-border-radius: 8px;  -webkit-border-radius: 8px;  border: 2px solid #000000;  '>
                            <p style='width:200px;height:100%;'><a href='%%RedirectLink%%' style='color:black;text-decoration:none;width:100%;height:100%;'>Accept Invitation</a></p>
                        </div>
                        <br />
                        <p>This is an automated notification. Please do not reply to this email. If you have questions or need assistance in setting up your user account, please contact %%CreatedByUserName%% at %%CreatedByUserEmailAddress%%</p>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>";
            }


            content = content.Replace("%%FirstName%%", newUser.FirstName);
            content = content.Replace("%%CreatedByUserName%%", adminUser.FirstName + " " + adminUser.LastName);
            content = content.Replace("%%UserName%%", newUser.UserName);
            content = content.Replace("%%CreatedByUserEmailAddress%%", adminUser.EmailAddress);
            content = content.Replace("%%RedirectLink%%", redirectURL);
            KMPlatform.BusinessLogic.SecurityGroup sgWorker = new KMPlatform.BusinessLogic.SecurityGroup();
            List<int> clientGroupClients = new List<int>();
            StringBuilder sbRoles = new StringBuilder();
            //For replacing roles
            if (!newUser.IsPlatformAdministrator)
            {
                foreach (KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in roles)
                {
                    KMPlatform.Entity.SecurityGroup sg = sgWorker.Select(ucsgm.SecurityGroupID, false);
                    if (sg.ClientGroupID > 0)
                    {
                        List<KMPlatform.Entity.ClientGroupClientMap> clients = new KMPlatform.BusinessLogic.ClientGroupClientMap().SelectForClientGroup(sg.ClientGroupID);
                        if (!clientGroupClients.Contains(sg.ClientGroupID))
                        {
                            sbRoles.AppendLine(sg.SecurityGroupName + "<br />");
                            clientGroupClients.Add(sg.ClientGroupID);
                        }
                    }
                    else
                    {
                        sbRoles.AppendLine(sg.SecurityGroupName);
                    }
                }
            }
            else
            {
                sbRoles.Append("System Administrator");
            }
            content = content.Replace("%%Roles%%", sbRoles.ToString());
            return content;
        }

        private void DoUserGroupSave(int userID, int clientID, int clientGroupID)
        {
            if (clientID > 0)
            {
                ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(clientID, false);
                ECN_Framework_BusinessLayer.Communicator.UserGroup.DeleteByUserID_CustomerID(userID, c.CustomerID, Master.UserSession.CurrentUser);
                List<int> userGroups = (List<int>)GetFromViewState("RestrictGroups_" + clientID.ToString());
                if (userGroups != null)
                {

                    foreach (int i in userGroups)
                    {
                        ECN_Framework_Entities.Communicator.UserGroup ug = new ECN_Framework_Entities.Communicator.UserGroup();

                        ug.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                        ug.CustomerID = c.CustomerID;
                        ug.GroupID = i;
                        ug.IsDeleted = false;
                        ug.UserID = userID;

                        ECN_Framework_BusinessLayer.Communicator.UserGroup.Save(ug, Master.UserSession.CurrentUser);
                    }
                }
            }
            else if (clientGroupID > 0)
            {
                List<KMPlatform.Entity.Client> lstC = new KMPlatform.BusinessLogic.Client().SelectForClientGroupLite(clientGroupID, false);
                foreach (KMPlatform.Entity.Client c in lstC)
                {
                    ECN_Framework_Entities.Accounts.Customer cust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(c.ClientID, false);
                    ECN_Framework_BusinessLayer.Communicator.UserGroup.DeleteByUserID_CustomerID(userID, cust.CustomerID, Master.UserSession.CurrentUser);

                }
            }
        }

        protected void ddlBaseChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRolesForBaseChannel();
            LoadCustomers();
            LoadRolesForCustomer();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRolesForCustomer();
        }

        private void LoadRolesForBaseChannel()
        {
            List<KMPlatform.Entity.SecurityGroup> listSG = new List<KMPlatform.Entity.SecurityGroup>();
            int bcID = -1;
            if (int.TryParse(ddlBaseChannel.SelectedValue.ToString(), out bcID))
            {
                listSG = (new KMPlatform.BusinessLogic.SecurityGroup()).SelectForClientGroup(bcID, false).Where(x => x.AdministrativeLevel != KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator && x.IsActive == true).ToList();
            }

            ddlBCRoles.DataSource = listSG.OrderBy(x => x.SecurityGroupName);
            ddlBCRoles.DataTextField = "SecurityGroupName";
            ddlBCRoles.DataValueField = ColumnSecurityGroupId;
            ddlBCRoles.DataBind();
        }

        private void LoadRolesForCustomer()
        {
            List<KMPlatform.Entity.SecurityGroup> listSG = new List<KMPlatform.Entity.SecurityGroup>();
            int customerID = -1;
            if (int.TryParse(ddlCustomer.SelectedValue.ToString(), out customerID))
            {
                listSG = (new KMPlatform.BusinessLogic.SecurityGroup()).SelectForClient(customerID, false).Where(x => x.AdministrativeLevel != KMPlatform.Enums.SecurityGroupAdministrativeLevel.Administrator && x.IsActive == true).ToList();
            }
            ddlRole.DataSource = listSG.OrderBy(x => x.SecurityGroupName);
            ddlRole.DataTextField = "SecurityGroupName";
            ddlRole.DataValueField = ColumnSecurityGroupId;
            ddlRole.DataBind();
        }






        protected void btnSaveRole_Click(object sender, EventArgs e)
        {
            trBaseChannelError.Visible = false;
            trCustomerError.Visible = false;
            lblBCRoleError.Visible = false;
            if (dtUserRoles == null)
            {
                BuildUserRoleDT(new List<UserClientSecurityGroupMap>());
            }

            var currentRoleID = btnSaveRole.CommandArgument;
            var row = dtUserRoles.NewRow();
            var didReplace = false;
            if (!string.IsNullOrWhiteSpace(currentRoleID))
            {
                row = GetRoleRowOnCurrentEmpty(currentRoleID, row);
            }
            else
            {
                row = GetRoleRowOnCureentNonEmpty();
            }

            if (FillRoleRow(row))
            {
                return;
            }

            if (!DRExists(row))
            {
                if (string.IsNullOrWhiteSpace(currentRoleID))
                {
                    row[ColumnInactiveReason] = InactiveReasonPendingCapital;

                }
                dtUserRoles.Rows.Add(row);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(currentRoleID))
                {
                    throwECNException(ErrorUserHasRole, phError, lblErrorMessagePhError);
                }
                else
                {
                    dtUserRoles.Rows.Add(row);
                }
            }
            BindRolesGrid();
            mpeUserPerms.Hide();
        }

        private bool FillRoleRow(DataRow row)
        {
            //Base Channel Admin
            if (rblIsBCAdmin.SelectedValue.Equals(ValueYes) && !trIsBCRole.Visible)
            {
                if (FillBaseChannelAdminRow(row))
                {
                    return true;
                }
            } //Client Group Role
            else if (rblBCRoles.SelectedValue.Equals(ValueYes) && rblIsBCAdmin.SelectedValue.Equals(ValueNo))
            {
                if (FillClientGroupRow(row))
                {
                    return true;
                }
            } //Customer Admin
            else if (rblIsCAdmin.SelectedValue.Equals(ValueYes))
            {
                if (FillCustomerAdminRow(row))
                {
                    return true;
                }
            } //Client Role
            else
            {
                if (FillClientRow(row))
                {
                    return true;
                }
            }

            return false;
        }

        private bool FillClientRow(DataRow row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            if (ddlBaseChannel.SelectedIndex > NoId)
            {
                if (ddlCustomer.SelectedIndex > NoId)
                {
                    row[ColumnIsCaAdmin] = false;
                    row[ColumnIsBcAdmin] = false;
                    row[ColumnSecurityGroupId] = ddlRole.SelectedValue;
                    row[ColumnBaseChannel] = ddlBaseChannel.SelectedItem.Text;
                    row[ColumnBaseChannelId] = 0;
                    row[ColumnCustomer] = ddlCustomer.SelectedItem.Text;
                    row[ColumnCustomerId] = ddlCustomer.SelectedValue;
                    row[ColumnRole] = ddlRole.SelectedItem.Text;
                    row[ColumnIsChannelRole] = false;
                }
                else
                {
                    trCustomerError.Visible = true;
                    mpeUserPerms.Show();
                    return true;
                }
            }
            else
            {
                trBaseChannelError.Visible = true;
                mpeUserPerms.Show();
                return true;
            }

            return false;
        }

        private bool FillCustomerAdminRow(DataRow row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            var sgWorker = new KMPlatformBusiness.SecurityGroup();

            if (ddlCustomer.SelectedIndex > NoId)
            {
                var securityGroup =
                    sgWorker
                        .SelectForClient(Convert.ToInt32(ddlCustomer.SelectedValue), false)
                        .First(x => x.AdministrativeLevel == SecurityGroupAdministrativeLevel.Administrator);
                row[ColumnIsCaAdmin] = true;
                row[ColumnIsBcAdmin] = false;
                row[ColumnBaseChannel] = ddlBaseChannel.SelectedItem.Text;
                row[ColumnBaseChannelId] = 0;
                row[ColumnCustomer] = ddlCustomer.SelectedItem.Text;
                row[ColumnCustomerId] = ddlCustomer.SelectedValue;
                row[ColumnSecurityGroupId] = securityGroup.SecurityGroupID;
                row[ColumnRole] = RoleAdministrator;
                row[ColumnIsChannelRole] = false;
            }
            else
            {
                trCustomerError.Visible = true;
                mpeUserPerms.Show();
                return true;
            }

            return false;
        }

        private bool FillClientGroupRow(DataRow row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            if (ddlBaseChannel.SelectedIndex > NoId)
            {
                if (ddlBCRoles.SelectedIndex > NoId)
                {
                    row[ColumnIsCaAdmin] = false;
                    row[ColumnIsBcAdmin] = false;
                    row[ColumnSecurityGroupId] = ddlBCRoles.SelectedValue;
                    row[ColumnBaseChannel] = ddlBaseChannel.SelectedItem.Text;
                    row[ColumnBaseChannelId] = ddlBaseChannel.SelectedValue;
                    row[ColumnCustomer] = string.Empty;
                    row[ColumnCustomerId] = 0;
                    row[ColumnRole] = ddlBCRoles.SelectedItem.Text;
                    row[ColumnIsChannelRole] = true;
                }
                else
                {
                    lblBCRoleError.Visible = true;
                    mpeUserPerms.Show();
                    return true;
                }
            }
            else
            {
                trBaseChannelError.Visible = true;
                mpeUserPerms.Show();
                return true;
            }

            return false;
        }

        private bool FillBaseChannelAdminRow(DataRow row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            var sgWorker = new KMPlatformBusiness.SecurityGroup();

            if (ddlBaseChannel.SelectedIndex > NoId)
            {
                var securityGroup =
                    sgWorker
                        .SelectForClientGroup(Convert.ToInt32(ddlBaseChannel.SelectedValue), false)
                        .First(x => x.AdministrativeLevel == SecurityGroupAdministrativeLevel.ChannelAdministrator);
                row[ColumnIsBcAdmin] = true;
                row[ColumnIsCaAdmin] = false;
                row[ColumnBaseChannel] = ddlBaseChannel.SelectedItem.Text;
                row[ColumnBaseChannelId] = ddlBaseChannel.SelectedValue;
                row[ColumnCustomerId] = 0;
                row[ColumnSecurityGroupId] = securityGroup.SecurityGroupID;
                row[ColumnRole] = RoleChannleAdministrator;
                row[ColumnIsChannelRole] = true;
            }
            else
            {
                trBaseChannelError.Visible = true;
                mpeUserPerms.Show();
                return true;
            }

            return false;
        }

        private DataRow GetRoleRowOnCureentNonEmpty()
        {
            var row = dtUserRoles.NewRow();
            row[ColumnInactiveReason] = InactiveReasonPendingCapital;
            row[ColumnId] = Guid.NewGuid().ToString();
            row[ColumnIsActive] = false;
            row[ColumnIsDeleted] = false;
            row[ColumnDisplay] = true;
            return row;
        }

        private DataRow GetRoleRowOnCurrentEmpty(string currentRoleId, DataRow row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            var currentRoleFilter = string.Format(IdFilterTemplate, currentRoleId);
            var drToCheck = dtUserRoles.Select(currentRoleFilter)[0];
            drToCheck.ItemArray = dtUserRoles.Select(currentRoleFilter)[0].ItemArray;
            row[ColumnInactiveReason] = drToCheck[ColumnInactiveReason];
            row[ColumnId] = Guid.NewGuid().ToString();
            row[ColumnIsActive] = drToCheck[ColumnIsActive];
            row[ColumnIsDeleted] = drToCheck[ColumnIsDeleted];
            row[ColumnDisplay] = drToCheck[ColumnDisplay];
            if ((int)drToCheck[ColumnBaseChannelId] > 0) //it's a client group role
            {
                MarkRowsAsDeletedInClientGroup(drToCheck);
            }
            else
            {
                row = MarkRowsAsDeletedInAdminGroup(row, currentRoleFilter);
            }

            return row;
        }

        private DataRow MarkRowsAsDeletedInAdminGroup(DataRow row, string currentRoleFilter)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            var rowForIndex = dtUserRoles.Select(currentRoleFilter)[0];
            row.ItemArray = dtUserRoles.Select(currentRoleFilter)[0].ItemArray;
            var indexOf = dtUserRoles.Rows.IndexOf(rowForIndex);
            if (rblIsBCAdmin.SelectedValue.Equals(ValueYes) || rblBCRoles.SelectedValue.Equals(ValueYes))
            {
                row = dtUserRoles.NewRow();
                row[ColumnInactiveReason] = dtUserRoles.Rows[indexOf][ColumnInactiveReason];
                row[ColumnId] = Guid.NewGuid().ToString();
                row[ColumnIsActive] = dtUserRoles.Rows[indexOf][ColumnIsActive];
                row[ColumnIsDeleted] = dtUserRoles.Rows[indexOf][ColumnIsDeleted];
                row[ColumnDisplay] = dtUserRoles.Rows[indexOf][ColumnDisplay];
                row[ColumnIsChannelRole] = true;
                dtUserRoles.Rows[indexOf][ColumnIsDeleted] = true;
                dtUserRoles.Rows[indexOf][ColumnIsActive] = false;
                dtUserRoles.Rows[indexOf][ColumnInactiveReason] = ValueDisabled;
                dtUserRoles.Rows[indexOf][ColumnDisplay] = false;
                dtUserRoles.Rows[indexOf][ColumnDoHardDelete] = true;
            }
            else
            {
                row = dtUserRoles.NewRow();
                row[ColumnInactiveReason] = dtUserRoles.Rows[indexOf][ColumnInactiveReason];
                row[ColumnId] = Guid.NewGuid().ToString();
                row[ColumnIsActive] = dtUserRoles.Rows[indexOf][ColumnIsActive];
                row[ColumnIsDeleted] = dtUserRoles.Rows[indexOf][ColumnIsDeleted];
                row[ColumnDisplay] = dtUserRoles.Rows[indexOf][ColumnDisplay];
                row[ColumnIsChannelRole] = false;

                dtUserRoles.Rows[indexOf][ColumnIsDeleted] = true;
                dtUserRoles.Rows[indexOf][ColumnIsActive] = false;
                dtUserRoles.Rows[indexOf][ColumnInactiveReason] = ValueDisabled;
                dtUserRoles.Rows[indexOf][ColumnDisplay] = false;
                dtUserRoles.Rows[indexOf][ColumnDoHardDelete] = true;
            }

            return row;
        }

        private void MarkRowsAsDeletedInClientGroup(DataRow rowToCheck)
        {
            if (rowToCheck == null)
            {
                throw new ArgumentNullException(nameof(rowToCheck));
            }

            var rolesFilter =
                string.Format(FilterBaseChannelAndGroupIdTemplate, rowToCheck[ColumnBaseChannelId],
                    rowToCheck[ColumnSecurityGroupId]);
            var drsToDelete = dtUserRoles.Select(rolesFilter);
            foreach (var rowToDelete in drsToDelete)
            {
                //just doing hard deletes to make things simpler
                var indexToDelete = dtUserRoles.Rows.IndexOf(rowToDelete);

                dtUserRoles.Rows[indexToDelete][ColumnIsDeleted] = true;
                dtUserRoles.Rows[indexToDelete][ColumnIsActive] = false;
                dtUserRoles.Rows[indexToDelete][ColumnInactiveReason] = ValueDisabled;
                dtUserRoles.Rows[indexToDelete][ColumnDisplay] = false;
                dtUserRoles.Rows[indexToDelete][ColumnDoHardDelete] = true;
            }
        }





        private void BindRolesGrid()
        {
            gvUserRoles.DataSource = dtUserRoles;
            //(gvUserRoles.DataSource as DataTable).DefaultView.RowFilter = "Display = true";
            gvUserRoles.DataBind();
            upMain.Update();
        }

        private bool DRExists(DataRow dr)
        {
            //if (dtUserRoles.Select("(BaseChannelID = " + row["BaseChannelID"].ToString() + " or CustomerID = " + (string.IsNullOrEmpty(row["CustomerID"].ToString()) ? "-1" : row["CustomerID"].ToString()) + ")  and IsActive = true and IsDeleted = false and ID <> '" + row["ID"].ToString() + "'").Length == 0)
            //{
            if ((bool)dr[ColumnIsBcAdmin] == false && (bool)dr[ColumnIsCaAdmin] == false && (int)dr[ColumnBaseChannelId] <= 0)//client role
            {
                if (dtUserRoles.Select("(BaseChannelID = " + ddlBaseChannel.SelectedValue.ToString() + " or CustomerID = " + ddlCustomer.SelectedValue.ToString() + ") and ((IsActive = true) or (IsActive = false and InactiveReason = 'Pending')) and Display = true").Length > 0)
                {
                    return true;
                }
            }
            else if ((bool)dr[ColumnIsBcAdmin] == false && (bool)dr[ColumnIsCaAdmin] == true)//customer admin
            {
                if (dtUserRoles.Select("(BaseChannelID = " + ddlBaseChannel.SelectedValue.ToString() + " or CustomerID = " + ddlCustomer.SelectedValue.ToString() + ") and ((IsActive = true) or (IsActive = false and InactiveReason = 'Pending')) and Display = true").Length > 0)
                {
                    return true;
                }
            }
            else if ((bool)dr[ColumnIsBcAdmin] == true && (bool)dr[ColumnIsCaAdmin] == false || (int)dr[ColumnBaseChannelId] > 0)//channel admin or client group role
            {
                string custIDs = "";
                KMPlatform.BusinessLogic.Client cWorker = new KMPlatform.BusinessLogic.Client();
                foreach (KMPlatform.Entity.Client c in cWorker.SelectForClientGroup(Convert.ToInt32(ddlBaseChannel.SelectedValue.ToString()), false))
                {
                    custIDs += c.ClientID + ",";
                }
                custIDs = custIDs.Substring(0, custIDs.Length - 1);
                if (dtUserRoles.Select("(BaseChannelID = " + ddlBaseChannel.SelectedValue.ToString() + " or CustomerID in (" + custIDs + ")) and ((IsActive = true) or (IsActive = false and InactiveReason = 'Pending')) and Display = true").Length > 0)
                {
                    return true;
                }

            }

            //Do Check on roles that a customer admin wouldn't see
            if (!KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser) && user != null && user.UserID > 0)
            {
                int selectedClientID = Convert.ToInt32(ddlCustomer.SelectedValue.ToString());
                int selectedClientGroupID = Convert.ToInt32(ddlBaseChannel.SelectedValue.ToString());
                List<KMPlatform.Entity.UserClientSecurityGroupMap> existingRoles = new KMPlatform.BusinessLogic.UserClientSecurityGroupMap().SelectForUser(user.UserID);
                KMPlatform.BusinessLogic.SecurityGroup sgWorker = new KMPlatform.BusinessLogic.SecurityGroup();
                foreach (KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in existingRoles)
                {
                    KMPlatform.Entity.SecurityGroup sg = sgWorker.Select(ucsgm.SecurityGroupID, false);

                    if (sg.AdministrativeLevel == KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator || sg.ClientGroupID > 0)
                    {
                        //BCAdmin/client group role check
                        if (sg.ClientGroupID == selectedClientGroupID)
                            return true;

                        //Customer check
                        List<KMPlatform.Entity.Client> clientList = new KMPlatform.BusinessLogic.Client().SelectForClientGroupLite(sg.ClientGroupID, false);
                        if (clientList.Any(x => x.ClientID == selectedClientID))
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        protected void gvUserRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                if ((bool)dr[ColumnDisplay] == true)
                {
                    ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDelete");
                    ImageButton imgbtnRestrict = (ImageButton)e.Row.FindControl("imgbtnRestrict");
                    Label lblRoleStatus = (Label)e.Row.FindControl("lblRoleStatus");
                    Label lblRoleName = (Label)e.Row.FindControl("lblRoleName");
                    Label lblCustomerName = (Label)e.Row.FindControl("lblCustomerName");

                    Master.MasterRegisterButtonForPostBack(imgbtnDelete);
                    Master.MasterRegisterButtonForPostBack(imgbtnRestrict);

                    lblRoleStatus.Text = dr[ColumnInactiveReason].ToString();
                    if (lblRoleStatus.Text.ToLower().Equals(InactiveReasonPending))
                    {
                        lblRoleStatus.ForeColor = System.Drawing.Color.Orange;
                    }
                    else if (string.IsNullOrWhiteSpace(lblRoleStatus.Text) && (bool)dr[ColumnIsActive])
                    {
                        lblRoleStatus.Text = "Active";
                    }


                    imgbtnDelete.CommandArgument = UserID.ToString() + "_" + dr[ColumnId].ToString();

                    lblRoleName.Text = dr["Role"].ToString();
                    if ((bool)dr[ColumnIsBcAdmin] == true || (bool)dr[ColumnIsCaAdmin] == true || ((bool)dr[ColumnIsBcAdmin] == false && (bool)dr[ColumnIsChannelRole] == true))
                    {
                        imgbtnRestrict.Visible = false;
                        if ((bool)dr[ColumnIsBcAdmin] == true)
                            lblRoleName.Text = "BaseChannel Admin";
                        else if ((bool)dr[ColumnIsCaAdmin] == true)
                        {
                            lblRoleName.Text = "Customer Admin";
                            lblCustomerName.Text = dr["Customer"].ToString();
                        }
                        else
                        {
                            lblRoleName.Text = dr["Role"].ToString();
                        }
                    }
                    else
                    {
                        imgbtnRestrict.CommandArgument = string.Format(UserCustomerIdTemplate, UserID, dr[ColumnCustomerId]);
                        lblCustomerName.Text = dr["Customer"].ToString();
                    }
                }
                else
                {
                    e.Row.Visible = false;
                }
            }
        }

        protected void gvUserRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.CommandName == null)
            {
                throw new ArgumentNullException(nameof(e.CommandName));
            }

            if (e.CommandName.Equals(CommandEditRole))
            {
                EditRole(e);
            }
            else if (e.CommandName.Equals(CommandDeleteRole))
            {
                DeleteRole(e);
            }
            else if (e.CommandName.Equals(CommandRestrict))
            {
                Restrict(e);
            }
        }

        private void Restrict(GridViewCommandEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.CommandArgument == null)
            {
                throw new ArgumentNullException(nameof(e.CommandArgument));
            }

            var args = e.CommandArgument.ToString().Split(UnderscoreDelim);
            var userId = args[0];
            var customerId = args[1];
            var c = BusinessAccounts.Customer.GetByClientID(Convert.ToInt32(customerId), false);
            groupExplorer.reset(c.CustomerID);
            var restrictGroups = new List<int>();
            var groupsName = string.Format(RestrictedGroupsTemplate, customerId);
            if (GetFromViewState(groupsName) == null && !userId.Contains(DashDelim))
            {
                var listGroups = new List<ECN_Framework_Entities.Communicator.UserGroup>();
                listGroups = BusinessCommunicator.UserGroup.Get(Convert.ToInt32(userId));
                foreach (var ug in listGroups)
                {
                    groupExplorer.setSelectedGroup(ug.GroupID);
                    restrictGroups.Add(ug.GroupID);
                }

                AddToViewState(groupsName, restrictGroups);
            }
            else
            {
                var groups = (List<int>)GetFromViewState(groupsName) ?? new List<int>();
                foreach (var ug in groups)
                {
                    groupExplorer.setSelectedGroup(ug);
                }
            }

            btnSaveRestrictGroups.CommandArgument = e.CommandArgument.ToString();
            mpeRestrictGroups.Show();
            upMain.Update();
        }

        private void DeleteRole(GridViewCommandEventArgs e)
        {
            var argSplit = e.CommandArgument.ToString().Split(UnderscoreDelim);
            var ucsgmID = argSplit[1];
            var filterExpression = string.Format(IdFilterTemplate, ucsgmID);
            var drForIndex = dtUserRoles.Select(filterExpression)[0];
            var dr = dtUserRoles.NewRow();
            dr.ItemArray = dtUserRoles.Select(filterExpression)[0].ItemArray;
            var indexOf = dtUserRoles.Rows.IndexOf(drForIndex);
            if (ucsgmID.Contains(DashDelim))
            {
                dtUserRoles.Rows[indexOf][ColumnIsActive] = false;
                dtUserRoles.Rows[indexOf][ColumnIsDeleted] = true;
                dtUserRoles.Rows[indexOf][ColumnInactiveReason] = ValueDisabled;
                dtUserRoles.Rows[indexOf][ColumnDisplay] = false;

                var restrictGroupsKey = string.Format(RestrictedGroupsTemplate, dr[ColumnCustomerId]);
                ViewState.Remove(restrictGroupsKey);
            }
            else
            {
                var sgId = Convert.ToInt32(dtUserRoles.Rows[indexOf][ColumnSecurityGroupId]);
                dtUserRoles.Rows[indexOf][ColumnIsActive] = false;
                dtUserRoles.Rows[indexOf][ColumnIsDeleted] = true;
                dtUserRoles.Rows[indexOf][ColumnInactiveReason] = ValueDisabled;
                dtUserRoles.Rows[indexOf][ColumnDisplay] = true;
                var rolesFilter = string.Format(SecurityGroupFilterTemplate, sgId);
                foreach (var drToDelete in dtUserRoles.Select(rolesFilter))
                {
                    var indexToDelete = dtUserRoles.Rows.IndexOf(drToDelete);
                    dtUserRoles.Rows[indexToDelete][ColumnIsActive] = false;
                    dtUserRoles.Rows[indexToDelete][ColumnIsDeleted] = true;
                    dtUserRoles.Rows[indexToDelete][ColumnInactiveReason] = ValueDisabled;
                }
            }

            BindRolesGrid();
        }

        private void EditRole(GridViewCommandEventArgs e)
        {
            var argSplit = e.CommandArgument.ToString().Split(UnderscoreDelim);
            var ucsgmId = argSplit[1];

            var filterExpression = string.Format(IdFilterTemplate, ucsgmId);
            var currentSg = dtUserRoles.Select(filterExpression)[0];
            var sg = (new KMPlatformBusiness.SecurityGroup()).Select(
                Convert.ToInt32(currentSg[ColumnSecurityGroupId].ToString()), false, false);
            btnSaveRole.CommandArgument = currentSg[ColumnId].ToString();
            ResetRolePopup();
            //Client Group Role or Channel Admin
            if (sg.ClientGroupID > 0 && sg.ClientID <= 0)
            {
                EditGroupRole(sg);
            } //Customer Admin
            else if (sg.AdministrativeLevel == SecurityGroupAdministrativeLevel.Administrator)
            {
                EditAdministratorRole(sg);
            } //Client Role
            else
            {
                EditClientRole(sg);
            }

            mpeUserPerms.Show();
            pnlUserPerms.Update();
        }

        private void EditClientRole(SecurityGroup securityGroup)
        {
            trBaseChannel.Visible = true;
            trCustomer.Visible = true;
            trRole.Visible = true;
            trBCRole.Visible = false;
            trIsBCRole.Visible = true;
            rblBCRoles.SelectedValue = ValueNo;
            rblIsBCAdmin.SelectedValue = ValueNo;
            rblIsCAdmin.SelectedValue = ValueNo;
            var cgcm = (new KMPlatformBusiness.ClientGroupClientMap()).SelectForClientID(securityGroup.ClientID);
            LoadBaseChannels();
            ddlBaseChannel.SelectedValue = cgcm.First(x => x.ClientID == securityGroup.ClientID).ClientGroupID.ToString();
            LoadCustomers();

            ddlCustomer.SelectedValue = securityGroup.ClientID.ToString();

            LoadRolesForCustomer();
            LoadRolesForBaseChannel();
            ddlRole.SelectedValue = securityGroup.SecurityGroupID.ToString();
        }

        private void EditAdministratorRole(SecurityGroup securityGroup)
        {
            trBaseChannel.Visible = true;
            trCustomer.Visible = true;
            trRole.Visible = false;
            trIsBCRole.Visible = true;
            trBCRole.Visible = false;
            rblBCRoles.SelectedValue = ValueNo;
            rblIsBCAdmin.SelectedValue = ValueNo;
            rblIsCAdmin.SelectedValue = ValueYes;
            var cgcm = (new KMPlatformBusiness.ClientGroupClientMap()).SelectForClientID(securityGroup.ClientID);
            ddlBaseChannel.SelectedValue = cgcm.First(x => x.ClientID == securityGroup.ClientID).ClientGroupID.ToString();
            LoadCustomers();

            ddlCustomer.SelectedValue = securityGroup.ClientID.ToString();
            LoadRolesForCustomer();
            LoadRolesForBaseChannel();
        }

        private void EditGroupRole(SecurityGroup securityGroup)
        {
            trBaseChannel.Visible = true;
            ddlBaseChannel.SelectedValue = securityGroup.ClientGroupID.ToString();
            LoadCustomers();
            LoadRolesForCustomer();
            LoadRolesForBaseChannel();
            if (securityGroup.AdministrativeLevel == SecurityGroupAdministrativeLevel.ChannelAdministrator)
            {
                trBCRole.Visible = false;
                trIsBCRole.Visible = false;
                rblIsBCAdmin.SelectedValue = ValueYes;
                rblBCRoles.SelectedValue = ValueNo;
                trCustomer.Visible = false;
                trRole.Visible = false;

                if (KMPlatformBusiness.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                {
                    rblIsBCAdmin.Enabled = true;
                }
                else
                {
                    rblIsBCAdmin.Enabled = false;
                }
            }
            else
            {
                trBCRole.Visible = true;
                trIsBCRole.Visible = true;
                rblIsBCAdmin.SelectedValue = ValueNo;
                rblBCRoles.SelectedValue = ValueYes;
                trCustomer.Visible = false;
                trRole.Visible = false;
                ddlBCRoles.SelectedValue = securityGroup.SecurityGroupID.ToString();
            }
        }

        protected void btnAddRole_Click(object sender, EventArgs e)
        {
            btnSaveRole.CommandArgument = "";
            ResetRolePopup();
            mpeUserPerms.Show();
        }

        protected void rblIsBCAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblIsBCAdmin.SelectedValue.Equals(ValueYes))
            {
                trCustomer.Visible = false;
                trRole.Visible = false;
                trIsBCRole.Visible = false;
                trBCRole.Visible = false;
            }
            else if (rblIsBCAdmin.SelectedValue.Equals(ValueNo))
            {
                trIsBCRole.Visible = true;
                if (rblBCRoles.SelectedValue.Equals(ValueYes))
                {
                    trBCRole.Visible = true;
                    trCustomer.Visible = false;
                    trRole.Visible = false;

                }
                else
                {
                    trBCRole.Visible = false;
                    trCustomer.Visible = true;

                    if (rblIsCAdmin.SelectedValue.Equals(ValueYes))
                        trRole.Visible = false;
                    else
                        trRole.Visible = true;
                }
            }
        }

        protected void rblIsCAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblIsCAdmin.SelectedValue.Equals(ValueYes))
            {
                trRole.Visible = false;
            }
            else
            {
                trRole.Visible = true;
            }
        }

        protected void btnCancelAddRole_Click(object sender, EventArgs e)
        {
            mpeUserPerms.Hide();
        }

        protected void btnCancelRestrictGroups_Click(object sender, EventArgs e)
        {
            mpeRestrictGroups.Hide();
        }

        protected void btnSaveRestrictGroups_Click(object sender, EventArgs e)
        {
            string[] strArray = btnSaveRestrictGroups.CommandArgument.ToString().Split(UnderscoreDelim);
            string userID = strArray[0];
            string customeriD = strArray[1];

            List<includes.GroupObject> groupsToRestrict = groupExplorer.getSelectedGroups();
            List<int> finalList = new List<int>();
            foreach (includes.GroupObject go in groupsToRestrict)
            {
                if (!finalList.Contains(go.GroupID))
                {
                    finalList.Add(go.GroupID);
                }
            }
            AddToViewState("RestrictGroups_" + customeriD, finalList);


            mpeRestrictGroups.Hide();
        }

        private void AddToViewState(string name, object value)
        {
            if (ViewState[name] == null)
            {
                ViewState.Add(name, value);
            }
            else
            {
                ViewState[name] = value;
            }
        }

        private object GetFromViewState(string name)
        {
            if (ViewState[name] == null)
                return null;
            else
                return ViewState[name];
        }

        protected void rblSysAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rblSysAdmin.SelectedValue.ToString().ToLower().Equals("yes"))
            //    btnAddRole.Visible = false;
            //else
            //    btnAddRole.Visible = true;
        }

        private void ResetRolePopup()
        {
            LoadBaseChannels();
            rblIsBCAdmin.SelectedValue = ValueNo;
            trIsBCRole.Visible = true;
            rblBCRoles.SelectedValue = ValueNo;
            trCustomer.Visible = true;
            trBaseChannelError.Visible = false;
            trCustomerError.Visible = false;
            LoadCustomers();
            rblIsCAdmin.SelectedValue = ValueNo;
            trRole.Visible = true;
            trBCRole.Visible = false;
            LoadRolesForCustomer();
            LoadRolesForBaseChannel();

            //Permission check
            if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                rblIsBCAdmin.Enabled = true;
                rblBCRoles.Enabled = true;
            }
            else
            {
                rblIsBCAdmin.Enabled = false;
                rblBCRoles.Enabled = false;
            }


            pnlUserPerms.Update();


        }

        protected void rblBCRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblBCRoles.SelectedValue.Equals(ValueNo))
            {
                trBCRole.Visible = false;
                trCustomer.Visible = true;
                if (rblIsCAdmin.SelectedValue.Equals(ValueNo))
                    trRole.Visible = true;
                else
                    trRole.Visible = false;
            }
            else
            {
                trBCRole.Visible = true;
                trCustomer.Visible = false;
                trRole.Visible = false;
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedValue.Equals("locked"))
            {
                btnAddRole.Visible = false;
            }
            else
            {
                btnAddRole.Visible = true;
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (getRequestUserID() > 0)
            {
                KMPlatform.Entity.User currentUser = KMPlatform.BusinessLogic.User.GetByUserID(getRequestUserID(), false);
                string html = GetResestHTMLandReplace(currentUser);
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();

                KMPlatform.Entity.User AdminUser = Master.UserSession.CurrentUser;
                //Send the email to the user

                ed.CreatedDate = DateTime.Now;
                ed.CreatedUserID = AdminUser.UserID;
                ed.EmailAddress = currentUser.EmailAddress;
                ed.EmailSubject = "KM Platform Password Reset";
                ed.FromName = "KM Platform";
                ed.Content = html;
                ed.SendTime = DateTime.Now;
                ed.ReplyEmailAddress = "info@knowledgemarketing.com";
                ed.CustomerID = Master.UserSession.CurrentCustomer.CustomerID;
                ed.Process = "Reset My Password Email";
                ed.Source = "ECN Accounts";

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
                lblEmailSent.Visible = true;
            }
        }

        private string GetResestHTMLandReplace(KMPlatform.Entity.User currentUser)
        {
            KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
            string html = uWorker.ResetPasswordHTML();
            html = html.Replace("%%FirstName%%", currentUser.FirstName);
            string tempPassword = uWorker.GenerateTempPassword();
            currentUser.Password = tempPassword;
            currentUser.RequirePasswordReset = true;
            html = html.Replace("%%TempPassword%%", tempPassword);
            string redirectUrl = ConfigurationManager.AppSettings["ResetPassword_URL"].ToString();
            redirectUrl += "?id=" + currentUser.UserID.ToString();
            html = html.Replace("%%RedirectLink%%", redirectUrl);

            uWorker.Save(currentUser);
            return html;
        }

        private static bool ParseBool(string str)
        {
            bool bValue;
            bool.TryParse(str, out bValue);
            return bValue;
        }

        private static int ParseInt32(string str)
        {
            int nValue;
            int.TryParse(str, out nValue);
            return nValue;
        }
    }
}