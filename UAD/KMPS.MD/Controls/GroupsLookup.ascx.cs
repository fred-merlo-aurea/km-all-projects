using KMPS.MD.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KMPS.MD.Controls
{
    public partial class GroupsLookup : System.Web.UI.UserControl
    {
        public Delegate hideGroupsLookupPopup;
        private static int GroupGridPageIndex;
        public bool ShowArchiveFilter
        {
            get
            {
                if (ViewState["ShowArchive"] != null)
                {
                    return ViewState["ShowArchive"].ToString().ToLower().Equals("true") ? true : false;
                }
                else
                    return false;
            }
            set
            {
                ViewState["ShowArchive"] = value;
            }
        }

        int GroupsRecordCount = 0;

        public int selectedGroupID
        {
            get
            {
                if (ViewState["selectedGroupID"] != null)
                    return (int)ViewState["selectedGroupID"];
                else
                    return 0;

            }
            set
            {
                ViewState["selectedGroupID"] = value;
            }
        }

        public void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            this.modalPopupGroupExplorer.Show();
            GroupFolder.FolderEvent += new EventHandler(GroupFolderEvent);
            GroupFolder.FolderType = ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString();

            if (!Page.IsPostBack)
            {
                GroupGridPageIndex = 0;

                if(KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
                {
                    btnAddNewGroup.Visible = true;
                }
            }
        }

        private void GroupFolderEvent(object sender, EventArgs e)
        {
            TreeView tn = (TreeView)sender;
            GroupGridPageIndex = 0;
            SearchCriteria.Text = string.Empty;
            chkAllFolders.Checked = false;

            if (tn.SelectedNode.Parent == null)
            {
                int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(Convert.ToInt32(tn.SelectedNode.Value), false).CustomerID;
                hfCustomerID.Value = CustomerID.ToString();
                hfFolderID.Value = "0";
                loadGroupsGrid(0, CustomerID);
            }
            else
            {
                string[] id = tn.SelectedNode.Value.Split('|');
                hfCustomerID.Value = id[1];
                hfFolderID.Value = id[0];
                loadGroupsGrid(Convert.ToInt32(hfFolderID.Value), Convert.ToInt32(hfCustomerID.Value));
            }
        }


        public void LoadControl()
        {
            this.modalPopupGroupExplorer.Show();
            reset();
        }

        private void reset()
        {
            selectedGroupID = 0;
            GroupGridPageIndex = 0;
            SearchGrpsDR.SelectedIndex = 0;
            chkAllFolders.Checked = false;
            this.GroupsGrid.PageSize = 10;
            this.GroupsGrid.PageIndex = 0;
            SearchCriteria.Text = string.Empty;

            LoadGroupFolder();
            //loadGroupsGrid(0,0);
        }


        //protected void Page_PreRender(object sender, System.EventArgs e)
        //{
        //    int grpFolderID = 0;

        //    string[] id = GroupFolder.SelectedFolderID.Split('|');

        //    if (GroupFolder.SelectedFolderID != null)
        //    {
        //        grpFolderID = Convert.ToInt32(id[0]);
        //    }
        //    //loadGroupsGrid(grpFolderID);

        //    if (chkAllFolders.Checked)
        //    {
        //        GroupsGrid.Columns[0].Visible = true;
        //    }
        //    else
        //        GroupsGrid.Columns[0].Visible = false;

        //    if (ShowArchiveFilter)
        //    {
        //        ddlArchiveFilter.Visible = true;
        //    }
        //    else
        //    {
        //        ddlArchiveFilter.SelectedValue = "active";
        //        ddlArchiveFilter.Visible = false;
        //    }

        //}

        protected void GroupsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                GroupsGrid.PageIndex = e.NewPageIndex;
            }
            GroupsGrid.DataBind();
        }

        private void LoadGroupFolder()
        {
            GroupFolder.ID = "GroupFolder";
            GroupFolder.FolderType = ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString();
            GroupFolder.NodesExpanded = true;
            GroupFolder.ChildrenExpanded = false;
            GroupFolder.LoadFolderTree();
        }

        private void loadGroupsGrid(int groupFolderID, int customerID)
        {
            if (SearchCriteria.Text == string.Empty)
            {
                DataTable groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers_NoAccessCheck(groupFolderID, customerID, UserSession.CurrentUser.UserID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked);

                if (groupList.Rows.Count > 0)
                {
                    GroupsRecordCount = Convert.ToInt32(groupList.Rows[0]["TotalCount"].ToString());
                }

                DataView dvgroup = groupList.DefaultView;
                GroupsGrid.DataSource = dvgroup;
                try
                {
                    GroupsGrid.DataBind();
                }
                catch
                {
                    GroupsGrid.PageIndex = 0;
                    GroupsGrid.DataBind();
                }
            }
            else
            {
                string searchCrietria = SearchGrpsDR.SelectedItem.Value.ToString();
                DataTable groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName_NoAccessCheck(SearchCriteria.Text, searchCrietria, customerID, UserSession.CurrentUser.UserID, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked);

                GroupsGrid.DataSource = groupList;

                if (groupList.Rows.Count > 0)
                {
                    GroupsRecordCount = Convert.ToInt32(groupList.Rows[0]["TotalCount"].ToString());
                }
                try
                {
                    GroupsGrid.DataBind();
                }
                catch
                {
                    GroupsGrid.PageIndex = 0;
                    GroupsGrid.DataBind();
                }
            }
            var exactPageCount = (double)GroupsRecordCount / (double)GroupsGrid.PageSize;
            var rountUpPageCount = Math.Ceiling((double)exactPageCount);

            lblTotalRecords.Text = GroupsRecordCount.ToString();

            lblTotalNumberOfPagesGroup.Text = rountUpPageCount.ToString();
            txtGoToPageContent.Text = (GroupGridPageIndex + 1) <= 1 ? "1" : (GroupGridPageIndex + 1).ToString();

            if (GroupsRecordCount > 0)
            {
                pnlPager.Visible = true;
            }
            else
                pnlPager.Visible = false;

            ViewState["layoutGridPageCount"] = lblTotalNumberOfPagesGroup.Text;
            ddlPageSizeContent.SelectedValue = GroupsGrid.PageSize.ToString();
            GroupsGrid.ShowHeaderWhenEmpty = true;
            //GroupsGrid.EmptyTableRowText = "No Groups to display";
            UpdatePanel1.Update();
        }

        protected void btnPreviousGroup_Click(object sender, EventArgs e)
        {
            if (GroupGridPageIndex > 0)
            {
                GroupGridPageIndex--;
                string[] id = GroupFolder.SelectedFolderID.Split('|');
                loadGroupsGrid(Convert.ToInt32(hfFolderID.Value), Convert.ToInt32(hfCustomerID.Value));
            }
        }

        protected void btnNextGroup_Click(object sender, EventArgs e)
        {
            var maxPage = lblTotalNumberOfPagesGroup.Text;
            if (GroupGridPageIndex + 1 < int.Parse(maxPage))
            {
                GroupGridPageIndex++;
                string[] id = GroupFolder.SelectedFolderID.Split('|');
                loadGroupsGrid(Convert.ToInt32(hfFolderID.Value), Convert.ToInt32(hfCustomerID.Value));
            }
        }

        protected void GoToPageContent_TextChanged(object sender, EventArgs e)
        {
            int pageNumber;
            if (int.TryParse(txtGoToPageContent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= int.Parse(ViewState["layoutGridPageCount"].ToString()))
            {
                GroupGridPageIndex = 1;
                ViewState["layoutGridPageIndex"] = GroupGridPageIndex = pageNumber - 1;
            }
            else
            {
                GroupGridPageIndex = 0;
                ViewState["layoutGridPageIndex"] = GroupGridPageIndex;
            }
            string[] id = GroupFolder.SelectedFolderID.Split('|');
            loadGroupsGrid(Convert.ToInt32(hfFolderID.Value), Convert.ToInt32(hfCustomerID.Value));
        }

        protected void GroupsGrid_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectGroup"))
            {
                selectedGroupID = Convert.ToInt32(e.CommandArgument.ToString());
                RaiseBubbleEvent("GroupSelected", new EventArgs());
            }
        }

        protected void GroupsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label lblFolder = (Label)e.Row.FindControl("lblFolderName");
                //DataRowView drv = (DataRowView)e.Row.DataItem;
                //lblFolder.Text = drv["FolderName"].ToString();
            }
        }

        protected void groupExplorer_Hide(object sender, EventArgs e)
        {
            hfCustomerID.Value = "0";
            hfFolderID.Value = "0";
            hideGroupsLookupPopup.DynamicInvoke();
        }

        #region Group Grid Pager Evetns
        protected void GroupGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.GroupsGrid.PageSize = int.Parse(dropDown.SelectedValue);
            loadGroupsGrid(Convert.ToInt32(hfFolderID.Value), Convert.ToInt32(hfCustomerID.Value));
        }

        protected void GoToPageGroup_TextChanged(object sender, EventArgs e)
        {
            int pageNumber;
            if (int.TryParse(txtGoToPageContent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.GroupsGrid.PageCount)
            {
                this.GroupsGrid.PageIndex = pageNumber - 1;
            }
            else
            {
                this.GroupsGrid.PageIndex = 0;
            }
        }
        #endregion

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (GroupFolder.SelectedFolderID != "0")
            {
                GroupGridPageIndex = 0;
                string[] id = GroupFolder.SelectedFolderID.Split('|');
                loadGroupsGrid(Convert.ToInt32(hfFolderID.Value), Convert.ToInt32(hfCustomerID.Value));
            }
        }

        protected void btnAddNewGroup_Click(object sender, EventArgs e)
        {
            txtGroupName.Text = string.Empty;
            lblErrGroup.Visible = false;

            if (Convert.ToInt32(hfCustomerID.Value) > 0)
            {
                mpeNewGroup.Show();
            }
            else
            {
                phError.Visible = true;
                lblErrorMessage.Text = "Please select customer";
            }
        }

        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            lblErrGroup.Visible = false;

            if (txtGroupName.Text.ToUpper() == "MASTER SUPPRESSION")
            {
                lblErrGroup.Visible = true;
                lblErrGroup.Text = "Can’t name group as Master Suppression.";
                mpeNewGroup.Show();
                return;
            }

            if (Groups.ExistsGroupNameByFolderID(txtGroupName.Text, Convert.ToInt32(hfCustomerID.Value), Convert.ToInt32(hfFolderID.Value)))
            {
                lblErrGroup.Visible = true;
                lblErrGroup.Text = "Group Name already exists in this folder.";
                mpeNewGroup.Show();
                return;
            }
            else
            {
                Utilities.InsertGroup(txtGroupName.Text, Convert.ToInt32(hfCustomerID.Value), Convert.ToInt32(hfFolderID.Value));
                loadGroupsGrid(Convert.ToInt32(hfFolderID.Value), Convert.ToInt32(hfCustomerID.Value));
                txtGroupName.Text = string.Empty;
            }
        }

        protected void btnCancelGroup_Click(object sender, EventArgs e)
        {
            lblErrGroup.Visible = false;
            lblErrGroup.Text = "";
            txtGroupName.Text = string.Empty;
        }
    }
}