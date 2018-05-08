using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.listsmanager
{
    public partial class groupeditor : ECN_Framework.WebPageHelper
    {
        private const string SubscribeTypeAll = " 'S', 'U', 'D', 'P', 'B', 'M' ";
        protected System.Web.UI.WebControls.Label GroupNameDisplay;
        protected System.Web.UI.WebControls.Label MsgLabel;


        int customerID = 0;
        int userID = 0;

        int _pagerCurrentPage = 1;
        public int pagerCurrentPage
        {
            set
            {
                _pagerCurrentPage = value;
            }
            get
            {
                return _pagerCurrentPage - 1;
            }
        }

        public string searchString
        {
            get
            {
                string emailAdd = ECN_Framework_Common.Functions.StringFunctions.CleanString(EmailFilter.Text);
                emailAdd = emailAdd.Replace("_", "[_]").Replace("'","''");
                string searchEmailLike = SearchEmailLike.SelectedItem.Value.ToString();
                string filter = "";

                if (emailAdd.Length > 0)
                {
                    if (searchEmailLike.Equals("like"))
                    {
                        filter = " AND EmailAddress like '%" + emailAdd + "%'";
                    }
                    else if (searchEmailLike.Equals("equals"))
                    {
                        filter = " AND EmailAddress like '" + emailAdd + "'";
                    }
                    else if (searchEmailLike.Equals("ends"))
                    {
                        filter = " AND EmailAddress like '%" + emailAdd + "'";
                    }
                    else if (searchEmailLike.Equals("starts"))
                    {
                        filter = " AND EmailAddress like '" + emailAdd + "%'";
                    }
                    else
                    {
                        filter = " AND EmailAddress like '%" + emailAdd + "%'";
                    }
                }

                string subscribeType = SubscribeTypeFilter.SelectedItem.Value.ToString();
                if (!(subscribeType.Equals("*")))
                {
                    filter += " AND SubscribeTypeCode = '" + subscribeType + "'";
                }

                return filter;
            }
        }

        public groupeditor()
        {
            Page.Init += new System.EventHandler(Page_Init);
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;
            Master.SubMenu = "add group";
            Master.Heading = "Groups > Manage Groups > Group Editor";
            Master.HelpContent = "<B>Adding a Group</B><div id='par1'><ul><li>Add a Name and a Description</li><li>Choose which folder you would like the group to appear in.</li><li>Click <em>Create</em></li></ul></div><b>Viewing/Editing Email Profiles (including adding notes to profile)</b><div id='par1'><ul><li>Click the <em>pencil</em> for the specific email address you want to view.</li><li>Edit information within the fields as needed; click <em>Update</em> when finished.</li><li>To enter or view notes on this subscriber, click the <em>View Notes</em> button.</li><li>To see the history of what messages have been sent to this subscriber, click the <em>View Logs</em> button. To see what message was sent, click on the blast title and a preview will appear.</li><li>To view the number of opens or pages the person has clicked on, click the <em>Profile Manager</em>, then click on <em>Email Opens Activity</em> or <em>Email Clicks Activity</em>.</li></ul></div><B> Exporting Your Customer List</B><div='par2'><ul><li>On the right side of the screen above the list of email addresses, you will find Export this view to… use the dropdown list to select the type of file you want to use (XML, Excel or CSV), then click <em>Export</em>.</li><li>Save the file to your computer.(Default file name is emails.xxx)</li></ul></div>";
            Master.HelpTitle = "Groups Manager";

            ddlFilteredDownloadOnly.CanDownloadTrans = true;
            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "grouppriv") || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
            {

                customerID = Master.UserSession.CurrentUser.CustomerID;
                userID = Master.UserSession.CurrentUser.UserID;

                chID_Hidden.Value = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
                custID_Hidden.Value = customerID.ToString();
                grpID_Hidden.Value = getGroupID().ToString();

                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SeedList))
                {
                    SeedListPanel.Visible = true;
                }
                else
                {
                    SeedListPanel.Visible = false;
                }

                if (getGroupID() > 0)
                {
                    if (!(Page.IsPostBack))
                    {
                        LoadSubscribeTypeCodes(getGroupID());

                        if (getSearchValue() != string.Empty)
                        {
                            SearchEmailLike.SelectedValue = getComparator();
                            EmailFilter.Text = getSearchValue();
                            ViewState["searchFilterVS"] = searchString;
                        }
                        else
                        {
                            ViewState["searchFilterVS"] = "";
                        }

                        SetUpdateInfo();
                        if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.View))
                        {
                            FilterPanel.Visible = true;
                            DownloadPanel.Visible = true;
                            LoadEmailsGrid();
                            BodyPanel.Visible = true;
                            EmailsGrid.Columns[5].Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.Edit);
                            EmailsGrid.Columns[6].Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.Edit);
                            EmailsGrid.Columns[7].Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.Delete);
                        }
                        else
                        {
                            FilterPanel.Visible = false;
                            DownloadPanel.Visible = false;
                            BodyPanel.Visible = false;
                        }
                        LoadFolders(customerID.ToString());
                        LoadFormData(getGroupID());
                    }
                }
                else
                {
                    //if (!KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "addgroup"))
                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
                    {
                        Response.Redirect("../securityAccessError.aspx", true);
                    }

                    if (Page.IsPostBack == false)
                    {
                        LoadFolders(Master.UserSession.CurrentUser.CustomerID.ToString());
                    }

                    DownloadPanel.Visible = false;
                    FilterPanel.Visible = false;
                    BodyPanel.Visible = false;
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        #region Getters
        public int getGroupID()
        {
            int groupID = 0;
            if (Request.QueryString["GroupID"] != null)
            {
                groupID = Convert.ToInt32(Request.QueryString["GroupID"].ToString());
            }
            return groupID;
        }
        public string getComparator()
        {
            string Comparator = string.Empty;
            if (Request.QueryString["Comparator"] != null)
            {
                Comparator = Request.QueryString["Comparator"].ToString();
            }
            return Comparator;
        }
        public string getSearchValue()
        {
            string SearchValue = string.Empty;
            if (Request.QueryString["Value"] != null)
            {
                SearchValue = Request.QueryString["Value"].ToString();
            }
            return SearchValue;
        }
        #endregion

        #region Form Prep
        private void SetUpdateInfo()
        {
            GroupID.Text = getGroupID().ToString();
            SaveButton.Text = "Update";
            SaveButton.Visible = false;
            UpdateButton.Visible = true;
            xsdDownloadLbl.Text = "<img src='" + System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/images/note.gif'>&nbsp;<font face='verdana' color='orange' size=1>For Export as XML,&nbsp;<a href='" + System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/emails.xsd' target='_blank'>Click here</a> to Download XML Schema</font>";
        }

        private void LoadFolders(string custID)
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(Master.UserSession.CurrentUser.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString(), Master.UserSession.CurrentUser);

            FolderID.DataSource = folderList;
            FolderID.DataBind();
            FolderID.Items.Insert(0, "root");
            FolderID.Items.FindByValue("root").Value = "0";
        }
        #endregion

        #region Data Load

        private void LoadEmailsGrid()
        {
            DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(Master.UserSession.CurrentUser.CustomerID, getGroupID(), EmailsPager.CurrentPage, EmailsPager.PageSize, ViewState["searchFilterVS"].ToString());
            DataTable emailstable = emailsListDS.Tables[1];
            EmailsGrid.DataSource = emailstable.DefaultView;


            try
            {
                EmailsGrid.DataBind();
            }
            catch { }

            EmailsPager.RecordCount = Convert.ToInt32(emailsListDS.Tables[0].Rows[0][0]);

            if (emailsListDS.Tables[1].Rows.Count > 0)
            {
                DownloadPanel.Visible = true;
            }
            else
            {
                DownloadPanel.Visible = false;
            }
        }

        private static DataSet LoadXMLFile(string filename)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(filename);
            return ds;
        }

        private void LoadSubscribeTypeCodes(int setGroupID)
        {
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(setGroupID, Master.UserSession.CurrentUser);
            if (group != null)
            {
                if (group.MasterSupression == null || group.MasterSupression.Value == 0)
                {
                    SubscribeTypeFilter.Items.Clear();
                    SubscribeTypeFilter.Items.Add(new ListItem("All Types", "*"));
                    SubscribeTypeFilter.Items.Add(new ListItem("Subscribes", "S"));
                    SubscribeTypeFilter.Items.Add(new ListItem("UnSubscribes", "U"));
                    SubscribeTypeFilter.Items.Add(new ListItem("Master Suppressed", "M"));
                    SubscribeTypeFilter.Items.Add(new ListItem("Marked as Bad Records", "D"));
                    SubscribeTypeFilter.Items.Add(new ListItem("Pending Subscribes", "P"));
                    SubscribeTypeFilter.Items.FindByValue("*").Selected = true;
                }
                else
                {
                    SubscribeTypeFilter.Items.Clear();
                    SubscribeTypeFilter.Items.Add(new ListItem("All Types", "*"));
                    SubscribeTypeFilter.Items.Add(new ListItem("UnSubscribes", "U"));
                    SubscribeTypeFilter.Items.Add(new ListItem("Bounce", "B"));
                    SubscribeTypeFilter.Items.Add(new ListItem("Abuse Complaint", "A"));
                    SubscribeTypeFilter.Items.Add(new ListItem("Manual Upload", "M"));
                    SubscribeTypeFilter.Items.Add(new ListItem("Feedback Loop(or Spam Complaint)", "F"));
                    SubscribeTypeFilter.Items.Add(new ListItem("Email Address Change", "E"));
                    SubscribeTypeFilter.Items.Add(new ListItem("Unknown User", "?"));
                    SubscribeTypeFilter.Items.FindByValue("*").Selected = true;
                }
            }
        }

        private void LoadFormData(int setGroupID)
        {
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(setGroupID, Master.UserSession.CurrentUser);


            if (group != null)
            {
                GroupName.Text = group.GroupName;
                GroupDescription.Text = group.GroupDescription;
                try
                {
                    rbSeedList.ClearSelection();

                    if (group.IsSeedList.Value)
                        rbSeedList.Items.FindByValue("True").Selected = true;
                    else
                        rbSeedList.Items.FindByValue("False").Selected = true;
                }
                catch
                {
                    rbSeedList.Items.FindByValue("False").Selected = true;
                }

                try
                {
                    FolderID.Items.FindByValue(group.FolderID.ToString()).Selected = true;
                }
                catch
                {
                    FolderID.Items.FindByValue("0").Selected = true;
                }
                PublicFolder.Checked = true;
                try
                {
                    if (group.PublicFolder == 0)
                    {
                        PublicFolder.Checked = false;
                    }
                }
                catch
                {
                    PublicFolder.Checked = false;
                }

                if (GroupName.Text.ToString().Trim().Equals("Master Suppression"))
                {
                    GroupName.ReadOnly = true;
                    GroupDescription.ReadOnly = true;
                    FolderID.Enabled = false;
                    PublicFolder.Enabled = false;
                    UpdateButton.Enabled = false;
                }
                else
                {

                    //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "addgroup"))
                    if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
                    {
                        GroupName.ReadOnly = false;
                        GroupDescription.ReadOnly = false;
                        FolderID.Enabled = true;
                        PublicFolder.Enabled = true;
                        UpdateButton.Enabled = true;
                    }
                    else
                    {
                        GroupName.ReadOnly = true;
                        GroupDescription.ReadOnly = true;
                        FolderID.Enabled = false;
                        PublicFolder.Enabled = false;
                        UpdateButton.Enabled = false;
                    }
                }
            }
            else
            {
                Response.Redirect("default.aspx");
            }
        }
        #endregion

        #region Data Handlers

        public void DeleteEmail(int theEmailID, int theGroupID)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.EmailGroup.Delete(theGroupID, theEmailID, Master.UserSession.CurrentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        public void CreateGroup(object sender, System.EventArgs e)
        {
            Page.Validate("GroupAddUpdate");

            if (Page.IsValid)
            {
                int pub_folder = 0;
                if (PublicFolder.Checked)
                {
                    pub_folder = 1;
                }
                string gname = ECN_Framework_Common.Functions.StringFunctions.CleanString(GroupName.Text.ToString().Trim());
                string gdesc = ECN_Framework_Common.Functions.StringFunctions.CleanString(GroupDescription.Text.ToString().Trim());

                try
                {
                    ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
                    group.GroupName = gname;
                    group.GroupDescription = gdesc;
                    group.FolderID = Convert.ToInt32(FolderID.SelectedItem.Value);
                    group.PublicFolder = pub_folder;
                    group.IsSeedList = Convert.ToBoolean(rbSeedList.SelectedItem.Value);
                    group.AllowUDFHistory = "N";
                    group.OwnerTypeCode = "customer";
                    group.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    group.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                    ECN_Framework_BusinessLayer.Communicator.Group.Save(group, Master.UserSession.CurrentUser);
                    Response.Redirect("default.aspx");
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
        }

        public void UpdateGroup(object sender, System.EventArgs e)
        {
            Page.Validate("GroupAddUpdate");

            if (Page.IsValid)
            {
                int pub_folder = 0;
                if (PublicFolder.Checked)
                {
                    pub_folder = 1;
                }
                string gname = ECN_Framework_Common.Functions.StringFunctions.CleanString(GroupName.Text.ToString().Trim());
                string gdesc = ECN_Framework_Common.Functions.StringFunctions.CleanString(GroupDescription.Text.ToString().Trim());
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(Convert.ToInt32(GroupID.Text), Master.UserSession.CurrentUser);
                group.GroupName = gname;
                group.GroupDescription = gdesc;
                group.FolderID = Convert.ToInt32(FolderID.SelectedItem.Value);
                group.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                group.PublicFolder = pub_folder;
                group.IsSeedList = Convert.ToBoolean(rbSeedList.SelectedItem.Value);
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.Group.Save(group, Master.UserSession.CurrentUser);
                    Response.Redirect("default.aspx");
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
        }
        #endregion

        #region DataGrid Item Commands
        private void EmailsGrid_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton deleteBtn = e.Row.FindControl("DeleteEmailBtn") as LinkButton;
                deleteBtn.Attributes.Add("onclick", "return confirm('Are you sure that you want to delete this Email Profile \"" + e.Row.Cells[0].Text.ToString() + "\" ?')");
                return;
            }
        }

        protected void EmailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteEmail")
            {
                int emailID = Convert.ToInt32(e.CommandArgument.ToString());
                DeleteEmail(emailID, getGroupID());
                LoadEmailsGrid();
            }
        }
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

        #region Web Form Designer generated code

        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
            EmailsGrid.RowDataBound += new GridViewRowEventHandler(EmailsGrid_ItemDataBound);
        }
        #endregion

        #region Events
        protected void EmailsPager_IndexChanged(object sender, EventArgs e)
        {
            pagerCurrentPage = EmailsPager.CurrentPage;
            LoadEmailsGrid();
        }

        protected void EmailFilterButton_Click(object sender, EventArgs e)
        {
            EmailsPager.CurrentPage = 1;
            EmailsPager.CurrentIndex = 0;

            ViewState["searchFilterVS"] = searchString;
            LoadEmailsGrid();
        }
        #endregion

        protected void btnExportGroup_Click(object sender, EventArgs e)
        {
            int FilterID = 0;
            string subscribeType = SubscribeTypeFilter.SelectedItem.Value;
            string emailAddr = EmailFilter.Text;
            string searchType = SearchEmailLike.Text;
            string channelID = chID_Hidden.Value.ToString();
            string customerID = custID_Hidden.Value.ToString();
            string groupID = grpID_Hidden.Value.ToString();
            string downloadType = FilteredDownloadType.SelectedItem.Value;

            string profFilter = ddlFilteredDownloadOnly.selected;
            string delimiter;

            var filter = PopulateFilter(downloadType, emailAddr, searchType, FilterID, SubscribeTypeAll, ref subscribeType, out delimiter);
            DataTable emailProfilesDT = new DataTable();

            emailProfilesDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetGroupEmailProfilesWithUDF(Convert.ToInt32(groupID), Convert.ToInt32(customerID), filter, subscribeType, profFilter);


            string OSFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/downloads/");
            String tfile = customerID + "-" + groupID + "emails" + downloadType;
            string outfileName = OSFilePath + tfile;

            PopulateResponse(OSFilePath, outfileName, downloadType, emailProfilesDT, delimiter, tfile, new HttpResponseWrapper(Response));
        }
    }
}