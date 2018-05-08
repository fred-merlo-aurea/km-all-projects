using ECN_Framework_Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.accounts.main.roles
{
    public partial class _default : System.Web.UI.Page
    {
        #region methods

        public bool HasAuthorized(int userID, int clientID)
        {
            if (Master.UserSession.CurrentUser.UserClientSecurityGroupMaps.Find(x => x.ClientID == clientID) != null)
                return true;

            return false;
        }

        #endregion methods
        #region properties

        public new ecn.accounts.MasterPages.Accounts Master
        {
            get
            {
                return ((ecn.accounts.MasterPages.Accounts)(base.Master));
            }
        }

        private int currentClientGroupID()
        {
            int     id = 0;
            try {   id = Convert.ToInt32(Request.QueryString["ClientGroupID"].ToString()); }
            catch { id = Master.UserSession.ClientGroupID; }
            return  id;
        }

        private int currentClientID()
        {
            int     id = 0;
            try {   id = Convert.ToInt32(Request.QueryString["ClientID"].ToString()); }
            catch { id = Master.UserSession.ClientID; }
            return  id;
        }

        #endregion methods
        #region event handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Heading = "Users > View Roles";
            if(IsPostBack)
            {
                return;
            }

            int clientGroupID = currentClientGroupID();
            int clientID = currentClientID();
            drpclientgroup.DataSource = Master.UserSession.CurrentUser.ClientGroups.Where(x => x.IsActive == true).OrderBy( x => x.ClientGroupName );
            drpclientgroup.DataBind();            
            drpclientgroup.Items.FindByValue(clientGroupID.ToString()).Selected = true;
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                drpClient.DataSource = (new KMPlatform.BusinessLogic.Client()).SelectActiveForClientGroupLite(clientGroupID).OrderBy(x => x.ClientName);
            }
            else if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                drpClient.DataSource = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(clientGroupID).OrderBy(x => x.ClientName);
            }
            else if (KMPlatform.BusinessLogic.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                drpClient.DataSource = Master.UserSession.CurrentUserClientGroupClients.OrderBy(x => x.ClientName).OrderBy(x => x.ClientName);
            }
            else
            {
                throw new SecurityException("SECURITY VIOLATION!");
            }

            drpClient.DataBind();
            drpClient.Items.Insert(0, new ListItem("-- select client --", "0"));
            
            drpClient.Items.FindByValue(clientID.ToString()).Selected = true;            
            LoadClientGroupSecurityGroupsGridView(clientGroupID);
            LoadClientSecurityGroupsGridView(clientID);
        }

        protected void drpclientgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<KMPlatform.Entity.Client> clients = new List<KMPlatform.Entity.Client>();

            int selectedClientGroup = int.Parse( drpclientgroup.SelectedValue );
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                clients = (new KMPlatform.BusinessLogic.Client()).SelectActiveForClientGroupLite(selectedClientGroup);
            }
            else if(KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                clients = (new KMPlatform.BusinessLogic.Client()).SelectActiveForClientGroupLite(selectedClientGroup);
            }
            else
            {
                clients = (new KMPlatform.BusinessLogic.Client()).SelectbyUserIDclientgroupID(Master.UserSession.CurrentUser.UserID, selectedClientGroup, false);
            }

            drpClient.DataSource = clients.OrderBy( x => x.ClientName );
            drpClient.DataBind();

            drpClient.Items.Insert(0, new ListItem("-- select client --", "0"));
            

            LoadClientGroupSecurityGroupsGridView(selectedClientGroup);
            LoadClientSecurityGroupsGridView(0);
        }

        protected void drpAccount_SelectedIndexChanged(object sender, EventArgs e)
        {            
            int drpClientSelectedItemValue = int.Parse(drpClient.SelectedItem.Value);
            LoadClientSecurityGroupsGridView(drpClientSelectedItemValue);
        }

        protected void LoadClientGroupSecurityGroupsGridView(int clientGroupID)
        {
            if(false == KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                pnlChannelRoles.Visible = false;
                pnlChannelRolesDropDown.Visible = false;
                gvClientGroupSecurityGroups.Visible = false;
                gvClientGroupSecurityGroups.Enabled = false;
                return;
            }
            
            KMPlatform.BusinessLogic.SecurityGroup sgLogic = new KMPlatform.BusinessLogic.SecurityGroup();
            gvClientGroupSecurityGroups.DataSource = sgLogic.SelectForClientGroup(clientGroupID, false);
            gvClientGroupSecurityGroups.DataBind();
        }

        protected void LoadClientSecurityGroupsGridView(int clientID)
        {
            bool allowed = KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser) 
                        || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser)
                        || (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser) && HasAuthorized(Master.UserSession.CurrentUser.UserID, clientID));
            
            if (clientID < 1)
            {
                gvClientSecurityGroups.DataSource = null;
                gvClientSecurityGroups.DataBind();
                return;
            }
            else if (false == allowed)
            {
                    gvClientSecurityGroups.Visible = false;
                    gvClientSecurityGroups.Enabled = false;
                    return;
            }

            KMPlatform.BusinessLogic.SecurityGroup sgLogic = new KMPlatform.BusinessLogic.SecurityGroup();
            gvClientSecurityGroups.DataSource = sgLogic.SelectForClient(clientID, false);
            gvClientSecurityGroups.DataBind();
        }

        protected void drpclientgroup_DataBound(object sender, EventArgs e)
        {
            if(drpclientgroup.Items.Count < 2)
            {
                drpclientgroup.Enabled = false;
            }

            if(false == KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                drpclientgroup.Visible = false;
            }
        }

        protected void gvClientSecurityGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch(e.CommandName.ToUpper())
            {
                case "DELETE":
                    // TODO: implement delete by setting IsActive = 0 /IsEnabled = 0
                    //  for 1. (Client|ClientGroup)SecurityGroupMap
                    //      2. SecurityGroup
                    break;
            }
        }

        protected void gvClientGroupSecurityGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink img = (HyperLink)e.Row.FindControl("hlEdit");
                HiddenField hfAdministrativeLevel = (HiddenField)e.Row.FindControl("hfAdministrativeLevel");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                HiddenField hfStatus = (HiddenField)e.Row.FindControl("hfStatus");
                if (hfStatus.Value.ToLower().Equals("true"))
                {
                    lblStatus.Text = "Active";
                }
                else
                    lblStatus.Text = "Disabled";
                
                if (hfAdministrativeLevel.Value.Equals("ChannelAdministrator", StringComparison.InvariantCultureIgnoreCase))
                {
                    img.Visible = false;
                }
            }
        }

        protected void gvClientSecurityGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink img = (HyperLink)e.Row.FindControl("hlEdit");
                HiddenField hfCAdministrativeLevel = (HiddenField)e.Row.FindControl("hfCAdministrativeLevel");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                HiddenField hfStatus = (HiddenField)e.Row.FindControl("hfStatus");
                if (hfStatus.Value.ToLower().Equals("true"))
                {
                    lblStatus.Text = "Active";
                }
                else
                    lblStatus.Text = "Disabled";
                if (hfCAdministrativeLevel.Value.Equals("Administrator", StringComparison.InvariantCultureIgnoreCase))
                {
                    img.Visible = false;
                }
            }
        }

        protected void btnAddNewRole_Click(object sender, EventArgs e)
        {
            Response.Redirect("roledetail.aspx");
        }

        #endregion event handlers
    }
}