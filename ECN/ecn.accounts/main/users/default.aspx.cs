using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ECNCommon = ECN_Framework_Common.Objects;

namespace ecn.accounts.usersmanager
{
    public partial class users_main : ECN_Framework_BusinessLayer.Application.WebPageHelper
    {
        bool hasDepartmentFeatureEnabled = false;
        int usersGridPageIndex = 0;
        public bool IsPlatformAdministrator = false;
        public bool IsChannelAdministator = false;
        public bool IsAdministrator = false;

        public static string CreateCustomerList(KMPlatform.Entity.User user)
        {

            return "";
        }

        public int CurrentClientID
        {
            get
            {
                return String.IsNullOrWhiteSpace(ddlCustomers.SelectedValue) ? Master.UserSession.CurrentCustomer.PlatformClientID : Int32.Parse(ddlCustomers.SelectedValue);
            }
        }

        public int CurrentClientGroupID
        {
            get
            {
                return String.IsNullOrWhiteSpace(ddlBaseChannels.SelectedValue)
                     ? Master.UserSession.CurrentBaseChannel.PlatformClientGroupID
                     : Int32.Parse(ddlBaseChannels.SelectedValue);
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.USERS;
            Master.SubMenu = "users list";
            Master.HelpContent = "Users Help";
            Master.HelpTitle = "USERS MANAGER";
            Master.Heading = "Users > Users List";
            IsPlatformAdministrator = KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser);
            IsChannelAdministator = KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser);
            IsAdministrator = KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser);
            // && KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.PLATFORM, KMPlatform.Enums.ServiceFeatures.User, KMPlatform.Enums.Access.View)

            Master.MasterRegisterButtonForPostBack(btnDownloadUsers);

            if (IsPlatformAdministrator || (IsChannelAdministator || IsAdministrator))
            {
                if (!Page.IsPostBack)
                {
                    pnlBaseChannel.Visible = IsPlatformAdministrator;
                    pnlCustomer.Visible = IsChannelAdministator;

                    LoadChannels();
                    LoadCustomers(Master.UserSession.CurrentBaseChannel.PlatformClientGroupID, Master.UserSession.CurrentCustomer.PlatformClientID);

                    LoadUsers();

                    if (IsPlatformAdministrator)
                    {
                        chkShowSysAdmin.Visible = true;
                    }
                    else
                        chkShowSysAdmin.Visible = false;

                    if (!IsChannelAdministator)
                        ddlCustomers.Enabled = false;
                    if (!IsPlatformAdministrator)
                        ddlBaseChannels.Enabled = false;

                }
            }
            else
            {
                Response.Redirect("~/main/securityAccessError.aspx", true);
            }
        }


        private void LoadChannels()
        {
            KMPlatform.BusinessLogic.ClientGroup cgWorker = new KMPlatform.BusinessLogic.ClientGroup();
            if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                List<ECN_Framework_Entities.Accounts.BaseChannel> listC = new List<ECN_Framework_Entities.Accounts.BaseChannel>();
                KMPlatform.BusinessLogic.SecurityGroup sgWorker = new KMPlatform.BusinessLogic.SecurityGroup();
                KMPlatform.Entity.SecurityGroup sg = sgWorker.Select(Master.UserSession.CurrentUser.CurrentSecurityGroup.SecurityGroupID, false);
                ECN_Framework_Entities.Accounts.Customer c = new ECN_Framework_Entities.Accounts.Customer();
                ECN_Framework_Entities.Accounts.BaseChannel bc = new ECN_Framework_Entities.Accounts.BaseChannel();
                if (sg.SecurityGroupName.ToLower().Equals("administrator"))
                {
                    c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(sg.ClientID, false);
                    bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(c.BaseChannelID.Value);

                }
                else if (sg.SecurityGroupName.ToLower().Equals("channel administrator"))
                {
                    bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByPlatformClientGroupID(sg.ClientGroupID);

                }

                KMPlatform.Entity.ClientGroup cg = cgWorker.Select(bc.PlatformClientGroupID, false);
                if (!listC.Any(x => x.BaseChannelID == bc.BaseChannelID) && cg.IsActive)
                {
                    listC.Add(bc);
                }
                //foreach (KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in Master.UserSession.CurrentUser.UserClientSecurityGroupMaps)
                //{
                    
                //    if (ucsgm.SecurityGroupID == Master.UserSession.CurrentUser.CurrentSecurityGroup.SecurityGroupID)
                //    {
                //        KMPlatform.Entity.SecurityGroup sg = sgWorker.Select(ucsgm.SecurityGroupID, false);
                //        ECN_Framework_Entities.Accounts.Customer c = new ECN_Framework_Entities.Accounts.Customer();
                //        ECN_Framework_Entities.Accounts.BaseChannel bc = new ECN_Framework_Entities.Accounts.BaseChannel();
                //        if (sg.SecurityGroupName.ToLower().Equals("administrator"))
                //        {
                //            c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(sg.ClientID, false);
                //            bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(c.BaseChannelID.Value);

                //        }
                //        else if (sg.SecurityGroupName.ToLower().Equals("channel administrator"))
                //        {
                //            bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByPlatformClientGroupID(sg.ClientGroupID);

                //        }

                //        KMPlatform.Entity.ClientGroup cg = cgWorker.Select(bc.PlatformClientGroupID, false);
                //        if (!listC.Any(x => x.BaseChannelID == bc.BaseChannelID) && cg.IsActive)
                //        {
                //            listC.Add(bc);
                //        }
                //    }
                //}
                ddlBaseChannels.DataSource = listC;
                ddlBaseChannels.DataBind();
                ddlBaseChannels.Items.FindByValue(Master.UserSession.CurrentBaseChannel.PlatformClientGroupID.ToString()).Selected = true;
            }
            else
            {
                List<KMPlatform.Entity.ClientGroup> cgList = cgWorker.Select(false).Where(x => x.IsActive == true).ToList();
                ddlBaseChannels.DataSource = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll().Where(x => cgList.Exists(y => y.ClientGroupID == x.PlatformClientGroupID)).OrderBy(x => x.BaseChannelName);
                ddlBaseChannels.DataBind();
                ddlBaseChannels.Items.FindByValue(Master.UserSession.CurrentBaseChannel.PlatformClientGroupID.ToString()).Selected = true;
            }
        }

        private void LoadCustomers(int platformClientGroupID, int customerID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> listC = new List<ECN_Framework_Entities.Accounts.Customer>();
            KMPlatform.BusinessLogic.SecurityGroup sgWorker = new KMPlatform.BusinessLogic.SecurityGroup();

            ECN_Framework_Entities.Accounts.BaseChannel bcAdmin = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByPlatformClientGroupID(platformClientGroupID);
            KMPlatform.Entity.SecurityGroup sgBCAdmin = sgWorker.SelectForClientGroup(bcAdmin.PlatformClientGroupID).First(x => x.SecurityGroupName.ToLower().Equals("channel administrator"));
            bool isChannelAdmin = false;
            if (Master.UserSession.CurrentUser.UserClientSecurityGroupMaps.Count(x => x.SecurityGroupID == sgBCAdmin.SecurityGroupID) > 0 || KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                listC = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(bcAdmin.BaseChannelID).Where(x => x.ActiveFlag.ToLower().Equals("y")).ToList();
                isChannelAdmin = true;
            }
            else
            {
                foreach (KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in Master.UserSession.CurrentUser.UserClientSecurityGroupMaps)
                {
                    KMPlatform.Entity.SecurityGroup sg = sgWorker.Select(ucsgm.SecurityGroupID, false);
                    ECN_Framework_Entities.Accounts.Customer c = new ECN_Framework_Entities.Accounts.Customer();
                    ECN_Framework_Entities.Accounts.BaseChannel bc = new ECN_Framework_Entities.Accounts.BaseChannel();
                    if (sg.SecurityGroupName.ToLower().Equals("administrator"))
                    {
                        c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(sg.ClientID, false);

                        if (!listC.Any(x => x.CustomerID == c.CustomerID) && c.BaseChannelID.Value == bcAdmin.BaseChannelID && c.ActiveFlag.ToLower().Equals("y"))
                        {
                            listC.Add(c);
                        }
                    }
                }
            }


            ddlCustomers.DataSource = listC;
            ddlCustomers.DataBind();

            if (customerID != 0 && listC.Any(x => x.CustomerID == customerID))
            {
                if (isChannelAdmin)
                    ddlCustomers.Items.Insert(0, new ListItem("-- Include All --", "0"));
                ddlCustomers.Items.FindByValue(customerID.ToString()).Selected = true;
            }
            else
            {
                switch (ddlCustomers.Items.Count)
                {
                    case 0:
                        ddlCustomers.Enabled = false;
                        return;
                    case 1:
                        ddlCustomers.Enabled = false;
                        ddlCustomers.SelectedIndex = 0;
                        return;
                    default:
                        ddlCustomers.Enabled = true;
                        ddlCustomers.Items.Insert(0, new ListItem("-- Include All --", "0"));
                        if (Master.UserSession.CurrentCustomer.CustomerID == customerID)
                        {
                            ddlCustomers.Items.FindByValue(customerID.ToString()).Selected = true;
                        }
                        else
                        {
                            ddlCustomers.SelectedIndex = 0;
                        }
                        return;
                }
            }
        }



        void LoadUsers()
        {
            KMPlatform.Entity.User requestingUser = Master.UserSession.CurrentUser;
            KMPlatform.BusinessLogic.User userBusinessLogic = new KMPlatform.BusinessLogic.User();
            DataTable users = new DataTable();
            bool includeBCAdmins = false;
            int clientGroupID = 0; Int32.TryParse(ddlBaseChannels.SelectedValue, out clientGroupID);
            int clientID = -1; Int32.TryParse(ddlCustomers.SelectedValue, out clientID);
            int? finalCLientGroup = null;
            if (clientGroupID > 0)
                finalCLientGroup = clientGroupID;

            if (KM.Platform.User.IsChannelAdministrator(requestingUser))
            {
                includeBCAdmins = true;

            }

            users = userBusinessLogic.SelectUserForGrid(clientID <= 0 ? -1 : clientID, finalCLientGroup, grdUsers.PageSize, grdUsers.PageIndex + 1, chkShowSysAdmin.Checked, KM.Platform.User.IsAdministrator(requestingUser), clientID == 0 ? true : false, includeBCAdmins, Master.UserSession.CurrentUser.IsKMStaff, txtSearch.Text, chkShowDisabledUsers.Checked, chkShowDisabledUserRoles.Checked);



            //if (KM.Platform.User.IsChannelAdministrator(requestingUser))
            //{
            //    int channelAdminSecurityGroupID = GetChannelAdministratorSecurityGroupID(clientGroupID);
            //    bool removeChannelAdminUsers = false == Master.UserSession.CurrentUser.UserClientSecurityGroupMaps.Any(
            //        x => x.SecurityGroupID == channelAdminSecurityGroupID);
            //    bool removeSysAdmin = !chkShowSysAdmin.Checked;
            //    users.AddRange(LoadUsers_ChannelAdministrator(removeSysAdmin,removeChannelAdminUsers));
            //}

            //if (KM.Platform.User.IsAdministrator(requestingUser))
            //{
            //    int adminSecurityGroupID = GetAdministratorSecurityGroupID(clientID);
            //    bool removeAdminUsers = false == Master.UserSession.CurrentUser.UserClientSecurityGroupMaps.Any(
            //        x => x.SecurityGroupID == adminSecurityGroupID);
            //    users.AddRange(LoadUsers_Administrator(removeAdminUsers));
            //}

            #region old ECN_Accounts.Users table stuff
            /*KM.Platform.User.IsSystemAdministrator(requestingUser) 
                                                      ? userBusinessLogic.Select()
                                                      : (userBusinessLogic.());*/

            //IEnumerable<ECN_Framework_Entities.Accounts.User> users = 
            //    from u in ECN_Framework_BusinessLayer.Accounts.User.GetByCustomerID(customerID)
            //    where KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser) 
            //       || (( false == u.IsSysAdmin)
            //          && (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser) 
            //          || (false == u.IsChannelAdmin)))
            //        select u;

            //if (hasDepartmentFeatureEnabled && departmentID > 0)
            //{
            //    List<ECN_Framework_Entities.Accounts.UserDepartment> departments = ECN_Framework_BusinessLayer.Accounts.UserDepartment.GetByDepartmentID(departmentID, Convert.ToInt32(customerID));
            //    var usersdeparments = from us in users
            //                          join dt in departments on us.UserID equals dt.UserID
            //                          orderby us
            //                          select us;
            //    grdUsers.DataSource = usersdeparments.ToList();
            //}
            //else
            //{
            //    grdUsers.DataSource = users.OrderBy(x => x.UserName).ToList<ECN_Framework_Entities.Accounts.User>();
            //}
            #endregion old ECN_Accounts.Users table stuff

            grdUsers.DataSource = users;
            grdUsers.DataBind();

            if (users.Rows.Count > 0)
            {
                pnlPager.Visible = true;
                int totalCount = Convert.ToInt32(users.Rows[0]["TotalCount"].ToString());
                var exactPageCount = (double)totalCount / (double)grdUsers.PageSize;
                var roundUpPageCount = Math.Ceiling((double)exactPageCount);
                lblTotalRecords.Text = totalCount.ToString();
                lblTotalNumberOfPagesGroup.Text = roundUpPageCount.ToString();
                ViewState["contentGridPageCount"] = lblTotalNumberOfPagesGroup.Text;

                txtGoToPageContent.Text = (grdUsers.PageIndex + 1).ToString();

                pnlPager.Visible = true;

                ddlPageSize.SelectedValue = grdUsers.PageSize.ToString();
            }
            else
            {
                pnlPager.Visible = false;
            }
        }

        private void DownloadUsers()
        {
            KMPlatform.Entity.User requestingUser = Master.UserSession.CurrentUser;
            KMPlatform.BusinessLogic.User userBusinessLogic = new KMPlatform.BusinessLogic.User();
            DataTable users = new DataTable();
            bool includeBCAdmins = false;
            int clientGroupID = 0; Int32.TryParse(ddlBaseChannels.SelectedValue, out clientGroupID);
            int clientID = -1; Int32.TryParse(ddlCustomers.SelectedValue, out clientID);
            int? finalCLientGroup = null;
            if (clientGroupID > 0)
                finalCLientGroup = clientGroupID;

            if (KM.Platform.User.IsChannelAdministrator(requestingUser))
            {
                includeBCAdmins = true;

            }
            int totalUsers = Convert.ToInt32(lblTotalRecords.Text);

            users = userBusinessLogic.DownloadUserGrid(clientID <= 0 ? -1 : clientID, finalCLientGroup, chkShowSysAdmin.Checked, KM.Platform.User.IsAdministrator(requestingUser), clientID == 0 ? true : false, includeBCAdmins, Master.UserSession.CurrentUser.IsKMStaff, txtSearch.Text, chkShowDisabledUsers.Checked, chkShowDisabledUserRoles.Checked);


            string tab = ECN_Framework_Common.Functions.DataTableFunctions.ToTabDelimited(users);

            ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToTab(tab, "Users");



        }

        #region old code
        //KMPlatform.BusinessLogic.User userLogic = new KMPlatform.BusinessLogic.User();
        //KMPlatform.BusinessLogic.SecurityGroup securityGroupLogic = new KMPlatform.BusinessLogic.SecurityGroup();

        //List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel> LoadUsers_SystemAdministrator(bool removeSysAdmin = true)
        //{
        //    int clientID = String.IsNullOrWhiteSpace(ddlCustomers.SelectedValue) ? Master.UserSession.CurrentCustomer.PlatformClientID : Int32.Parse(ddlCustomers.SelectedValue);
        //    int clientGroupID = Int32.Parse(ddlBaseChannels.SelectedValue);

        //    _allUsers = FilterUsers(userLogic.SelectRoles(clientGroupID, clientID), removeSysAdmin, false, false);

        //    return _allUsers;
        //}
        //List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel> _allUsers = null;

        //List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel> LoadUsers_ChannelAdministrator(bool removeSysAdmin = true, bool removeChannelAdmin = true)
        //{
        //    int clientGroupID = Int32.Parse(ddlBaseChannels.SelectedValue);

        //    clientGroupUsers = FilterUsers(userLogic.SelectRoles(clientGroupID) ?? new List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel>(), removeSysAdmin, removeChannelAdmin, false);

        //    return clientGroupUsers;
        //}
        //List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel> clientGroupUsers = new List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel>();

        //List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel> LoadUsers_Administrator(bool removeAdmin = true, bool removeChannelAdmin = true)
        //{
        //    int clientID = String.IsNullOrWhiteSpace(ddlCustomers.SelectedValue) ? Master.UserSession.CurrentCustomer.PlatformClientID : Int32.Parse(ddlCustomers.SelectedValue);

        //    clientUsers = FilterUsers(userLogic.SelectRoles(CurrentClientGroupID, clientID) ?? new List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel>(), true, removeChannelAdmin, removeAdmin);

        //    return clientUsers;
        //}
        //List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel> clientUsers = new List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel>();

        //List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel> FilterUsers(List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel> users, bool removeSystemAdmin, bool removeChannelAdmin, bool removeAdmin)
        //{
        //    //int clientID = CurrentClientID; int adminSecurityGroupID = GetAdministratorSecurityGroupID(clientID);            
        //    //int clientGroupID = CurrentClientGroupID; int channelAdminSecurityGroupID = GetChannelAdministratorSecurityGroupID(clientGroupID);            
        //    return users.Where(x => false == (removeSystemAdmin && x.IsPlatformAdministrator)
        //                    && false == (removeAdmin && x.AdministrativeLevel != KMPlatform.Entity.SecurityGroup.SecurityGroupAdministrativeLevel.None)
        //                   && false == (removeChannelAdmin && x.AdministrativeLevel == KMPlatform.Entity.SecurityGroup.SecurityGroupAdministrativeLevel.ChannelAdministrator)).ToList();
        //    //from x in users.AsEnumerable()
        //    //where false == (removeSystemAdmin  && x.IsPlatformAdministrator)
        //    //   && false == (removeAdmin        && x.AdministrativeLevel != KMPlatform.Entity.SecurityGroup.SecurityGroupAdministrativeLevel.None)
        //    //   && false == (removeChannelAdmin && x.AdministrativeLevel == KMPlatform.Entity.SecurityGroup.SecurityGroupAdministrativeLevel.ChannelAdministrator)
        //    //select x;
        //}

        //Dictionary<int, int> clientGroupIDToAdminSecurityGroupIDLookupTable = new Dictionary<int, int>();
        //int GetChannelAdministratorSecurityGroupID(int clientGroupID)
        //{
        //    if (false == clientGroupIDToAdminSecurityGroupIDLookupTable.ContainsKey(clientGroupID))
        //    {
        //        clientGroupIDToAdminSecurityGroupIDLookupTable[clientGroupID] = (securityGroupLogic.SelectForClientGroup(clientGroupID, KMPlatform.Entity.SecurityGroup.SecurityGroupAdministrativeLevel.Administrator)
        //                                                              .FirstOrDefault() ?? new KMPlatform.Entity.SecurityGroup { }).SecurityGroupID;
        //    }
        //    return clientGroupIDToAdminSecurityGroupIDLookupTable[clientGroupID];
        //}

        //Dictionary<int, int> clientIDToAdminSecurityGroupIDLookupTable = new Dictionary<int, int>();
        //int GetAdministratorSecurityGroupID(int clientID)
        //{
        //    if (false == clientIDToAdminSecurityGroupIDLookupTable.ContainsKey(clientID))
        //    {
        //        clientIDToAdminSecurityGroupIDLookupTable[clientID] = (securityGroupLogic.SelectForClient(clientID, KMPlatform.Entity.SecurityGroup.SecurityGroupAdministrativeLevel.Administrator)
        //                                                              .FirstOrDefault() ?? new KMPlatform.Entity.SecurityGroup { }).SecurityGroupID;
        //    }
        //    return clientIDToAdminSecurityGroupIDLookupTable[clientID];
        //}
        #endregion
        public void ddlBaseChannels_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadCustomers(int.Parse(ddlBaseChannels.SelectedValue), 0);
            grdUsers.DataSource = null;
            grdUsers.DataBind();
            grdUsers.PageIndex = 0;


            LoadUsers();

        }

        public void ddlCustomers_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            grdUsers.PageIndex = 0;
            LoadUsers();
        }

        public void ddlDepartments_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadUsers();
        }

        protected void grdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {

                    //bool isContentCreator = ECN_Framework_BusinessLayer.Accounts.User.bIsContentCreator(int.Parse(e.CommandArgument.ToString()));
                    //if (isContentCreator)
                    //{
                    //    throwECNException("User cannot be deleted because the user is a current content creator", phError, lblErrorMessagePhError);
                    //}

                    //ECN_Framework_BusinessLayer.Accounts.User.Delete(int.Parse(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                    LoadUsers();
                }
                catch (ECN_Framework_Common.Objects.ECNException ecnex)
                {
                    setECNError(ecnex, phError, lblErrorMessagePhError);

                }

            }
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

        protected void grdUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnAddNewUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("userdetail.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdUsers.PageIndex = 0;
            LoadUsers();
        }

        protected void btnResendInvite_Click(object sender, EventArgs e)
        {
            //TODO write code for resending the invite email to the user

            LinkButton lbResend = (LinkButton)sender;
            string[] param = lbResend.CommandArgument.ToString().Split('_');

            if (param.Length == 2 && !string.IsNullOrEmpty(param[1]))
            {
                int userid = Convert.ToInt32(param[0]);

                Guid SetID = new Guid();
                Guid.TryParse(param[1], out SetID);

                KMPlatform.Entity.User currentUser = (new KMPlatform.BusinessLogic.User()).SelectUser(userid, false);

                List<KMPlatform.Entity.SecurityGroupOptIn> listSGOI = new KMPlatform.BusinessLogic.SecurityGroupOptIn().GetBySetID(SetID);

                ResendInvite(SetID, currentUser, listSGOI);
            }

        }

        private void ResendInvite(Guid setIds, KMPlatform.Entity.User currentUser, List<KMPlatform.Entity.SecurityGroupOptIn> roles)
        {
            KMPlatform.Entity.User AdminUser = Master.UserSession.CurrentUser;

            if (!string.IsNullOrWhiteSpace(AdminUser.FirstName + " " + AdminUser.LastName))
            {
                KM.Common.Entity.Encryption ecLink = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));

                string query = "?setID=" + setIds.ToString() + "&userID=" + currentUser.UserID.ToString() + "&existing=" + currentUser.IsActive.ToString();
                string encryptedQuery = KM.Common.Encryption.Base64Encrypt(query, ecLink);

                string redirectURL = ConfigurationManager.AppSettings["MVCActivity_DomainPath"].ToString() + "/User/Accept/" + encryptedQuery;

                string fromName = "";
                if (string.IsNullOrEmpty(AdminUser.FirstName + " " + AdminUser.LastName))
                    fromName = AdminUser.EmailAddress;
                else
                    fromName = AdminUser.FirstName + " " + AdminUser.LastName;

                //Send the email to the user
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                ed.CreatedDate = DateTime.Now;
                ed.CreatedUserID = AdminUser.UserID;
                ed.EmailAddress = currentUser.EmailAddress;
                ed.EmailSubject = fromName + " has added you to KM Platform";
                ed.FromName = fromName;
                ed.Content = GetConfirmationContent(currentUser, AdminUser, redirectURL, roles);
                ed.SendTime = DateTime.Now;
                ed.ReplyEmailAddress = "info@knowledgemarketing.com";
                ed.CustomerID = Master.UserSession.CurrentCustomer.CustomerID;
                ed.Process = "New User Initial Email";
                ed.Source = "ECN Accounts";

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
            else
            {
                throwECNException("Before you can complete this action you need to update your own user profile with first and last name. Your first and last name is used as the \"From Name\" in the email invitation.", phError, lblErrorMessagePhError);
                return;
            }
        }

        private string GetConfirmationContent(KMPlatform.Entity.User newUser, KMPlatform.Entity.User adminUser, string redirectURL, List<KMPlatform.Entity.SecurityGroupOptIn> roles)
        {
            string content = "";
            if (!newUser.IsActive)
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
                        <p>Dear %%FirstName%%,<br /><br />%%CreatedByUserName%% has granted you access to the Knowledge Marketing Platform with the following roles,<br /><br />
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
                foreach (KMPlatform.Entity.SecurityGroupOptIn ucsgm in roles)
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


        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btnEdit = (LinkButton)sender;
            Response.Redirect("userdetail.aspx?UserID=" + btnEdit.CommandArgument);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton btnDelete = (LinkButton)sender;
            string[] IDS = btnDelete.CommandArgument.ToString().Split('_');
            int userID = Convert.ToInt32(IDS[0]);
            int securityGroupID = Convert.ToInt32(IDS[1]);
            KMPlatform.Entity.User userToDelete = (new KMPlatform.BusinessLogic.User()).SelectUser(userID, true);
            KMPlatform.Entity.SecurityGroup sgToDelete = new KMPlatform.BusinessLogic.SecurityGroup().Select(securityGroupID, false);
            KMPlatform.BusinessLogic.UserClientSecurityGroupMap ucsgmWorker = new KMPlatform.BusinessLogic.UserClientSecurityGroupMap();
            if (userToDelete != null)
            {
                //Sys Admin - disable
                if (KM.Platform.User.IsSystemAdministrator(userToDelete))
                {
                    //removing code here because if its a sys admin we have no roles to disable
                }
                else if (sgToDelete.ClientGroupID != null && sgToDelete.ClientGroupID > 0)//Channel Admin - disable UserClientSecurityGroupMap
                {
                    List<KMPlatform.Entity.UserClientSecurityGroupMap> lstUCSGM = ucsgmWorker.SelectForUser(userID).Where(x => x.SecurityGroupID == securityGroupID).ToList();
                    foreach (KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in lstUCSGM)
                    {
                        ucsgm.IsActive = false;
                        ucsgm.InactiveReason = "Disabled";
                        ucsgm.UpdatedByUserID = Master.UserSession.CurrentUser.UserID;
                        ucsgm.DateUpdated = DateTime.Now;
                        ucsgmWorker.Save(ucsgm);
                    }
                }
                else//Customer Admin or regular User - disable UserClientSecurityGroupMap
                {
                    List<KMPlatform.Entity.UserClientSecurityGroupMap> lstUCSGM = ucsgmWorker.SelectForUser(userID).Where(x => x.SecurityGroupID == securityGroupID).ToList();
                    foreach (KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in lstUCSGM)
                    {
                        ucsgm.IsActive = false;
                        ucsgm.InactiveReason = "Disabled";
                        ucsgm.UpdatedByUserID = Master.UserSession.CurrentUser.UserID;
                        ucsgm.DateUpdated = DateTime.Now;
                        ucsgmWorker.Save(ucsgm);
                    }
                }

            }

            LoadUsers();

        }

        protected void grdUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView userModel = (DataRowView)e.Row.DataItem;
                LinkButton lbEdit = (LinkButton)e.Row.FindControl("btnEdit");
                LinkButton lbDelete = (LinkButton)e.Row.FindControl("btnDelete");
                LinkButton lbResend = (LinkButton)e.Row.FindControl("btnResendInvite");
                Label lblUserRole = (Label)e.Row.FindControl("lblUserRole");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                System.Web.UI.HtmlControls.HtmlControl liResend = (System.Web.UI.HtmlControls.HtmlControl)e.Row.FindControl("liResend");
                System.Web.UI.HtmlControls.HtmlControl liDeleteRole = (System.Web.UI.HtmlControls.HtmlControl)e.Row.FindControl("liDeleteRole");
                int userID = Convert.ToInt32(userModel["UserID"].ToString());
                string setID = userModel["SetID"].ToString();
                bool status = (bool)userModel["Status"];
                string userStatus = userModel["UserStatus"].ToString();
                string inactiveReason = userModel["InactiveReason"].ToString();
                bool noName = false;
                if(string.IsNullOrWhiteSpace(Master.UserSession.CurrentUser.FirstName + " " + Master.UserSession.CurrentUser.LastName))
                {
                    noName = true;
                }
                if ((new KMPlatform.BusinessLogic.SecurityGroupOptIn()).SelectPendingForUser(userID).Count > 0 && inactiveReason.ToLower().Equals("pending") && status == false)
                {
                    if (!userStatus.ToLower().Equals("locked"))
                    {
                        liResend.Visible = true;
                        lbResend.CommandArgument = userID.ToString() + "_" + setID;
                        if(noName)
                        {
                            lbResend.Attributes.Add("onclick", "SetScrollEvent();");
                        }
                    }
                    else
                    {
                        liResend.Visible = false;
                    }
                }
                else
                {
                    liResend.Visible = false;
                }

                if (userModel["IsPlatformAdministrator"].ToString().ToLower().Equals("true"))
                {
                    lblUserRole.Text = "System Administrator";
                    liDeleteRole.Visible = false;
                    lblStatus.Text = "";
                }
                else
                {
                    liDeleteRole.Visible = true;
                    switch (userModel["SecurityGroupName"].ToString().ToLower())
                    {
                        case "administrator":
                            lblUserRole.Text = "Customer Admin";
                            break;
                        case "channel administrator":
                            lblUserRole.Text = "BaseChannel Admin";
                            break;
                        default:
                            lblUserRole.Text = userModel["SecurityGroupName"].ToString();
                            break;
                    }
                    //If role status = true then active else if inactive reason is empty then disabled else inactive reason
                    lblStatus.Text = status ? "Active" : string.IsNullOrEmpty(inactiveReason) ? "Disabled" : inactiveReason;
                }

                lbEdit.CommandArgument = userID.ToString();
                lbDelete.CommandArgument = userID.ToString() + "_" + userModel["SecurityGroupID"].ToString();

            }

        }

        protected void chkShowSysAdmin_CheckedChanged(object sender, EventArgs e)
        {
            grdUsers.PageIndex = 0;
            LoadUsers();
        }

        protected void UserGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdUsers.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue.ToString());
            LoadUsers();
        }

        protected void GoToPageUser_TextChanged(object sender, EventArgs e)
        {
            int temp;
            if  (int.TryParse(txtGoToPageContent.Text.ToString(), out temp))
            {
                int newIndex = Convert.ToInt32(txtGoToPageContent.Text);
                if (newIndex <= 0)
                {
                    grdUsers.PageIndex = 0;
                    LoadUsers();
                }
                else if (newIndex + 1 > Convert.ToInt32(lblTotalNumberOfPagesGroup.Text))
                {
                    grdUsers.PageIndex = Convert.ToInt32(lblTotalNumberOfPagesGroup.Text) - 1;
                    LoadUsers();
                }
                else
                {
                    grdUsers.PageIndex = newIndex - 1;
                    LoadUsers();
                }
            }
            else
            {
                txtGoToPageContent.Text = (grdUsers.PageIndex + 1).ToString();
            }

        }

        protected void btnPreviousGroup_Click(object sender, EventArgs e)
        {
            if (grdUsers.PageIndex > 0)
            {
                grdUsers.PageIndex--;
                LoadUsers();
            }
        }

        protected void btnNextGroup_Click(object sender, EventArgs e)
        {
            if (grdUsers.PageIndex + 1 < Convert.ToInt32(lblTotalNumberOfPagesGroup.Text))
            {
                grdUsers.PageIndex++;
                LoadUsers();
            }

        }

        protected void chkShowDisabledUsers_CheckedChanged(object sender, EventArgs e)
        {
            grdUsers.PageIndex = 0;
            LoadUsers();
        }

        protected void chkShowDisabledUserRoles_CheckedChanged(object sender, EventArgs e)
        {
            grdUsers.PageIndex = 0;
            LoadUsers();
        }

        protected void chkShowKMStaff_CheckedChanged(object sender, EventArgs e)
        {
            grdUsers.PageIndex = 0;
            LoadUsers();
        }

        protected void btnDownloadUsers_Click(object sender, EventArgs e)
        {
            DownloadUsers();
        }

    }
}