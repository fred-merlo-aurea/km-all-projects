using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard.Group
{
    public partial class groupsLookup : System.Web.UI.UserControl
    {
        public Delegate hideGroupsLookupPopup;
        private static int GroupGridPageIndex;
        
        public bool isQTB
        {
            get {
                if (ViewState["IsQTB"] != null)
                    return (bool)ViewState["IsQTB"];
                else
                    return false;
            }
            set { ViewState["IsQTB"] = value; }
        }
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            this.modalPopupGroupExplorer.Show();
            GroupFolder.FolderEvent += new EventHandler(GroupFolderEvent);
            if (!Page.IsPostBack)
            {
                GroupGridPageIndex = 0;
                loadGroupsGrid(0, isQTB);
            }
        }

        private void GroupFolderEvent(object sender, EventArgs e)
        {
            TreeView tn = (TreeView)sender;
            GroupGridPageIndex = 0;
            SearchCriteria.Text = string.Empty;
            chkAllFolders.Checked = false;
            loadGroupsGrid(Convert.ToInt32(tn.SelectedNode.Value), isQTB);

        }


        public void LoadControl(bool isQuickTestBlast = false)
        {
            this.modalPopupGroupExplorer.Show();
            this.isQTB = isQuickTestBlast;
            reset(this.isQTB);
        }

        private void reset(bool isQuickTestBlast)
        {
            selectedGroupID = 0;
            GroupGridPageIndex = 0;
            SearchTypeDR.SelectedIndex = 0;
            SearchGrpsDR.SelectedIndex = 0;
            chkAllFolders.Checked = false;
            this.GroupsGrid.PageSize = 15;
            this.GroupsGrid.PageIndex = 0;
            SearchCriteria.Text = string.Empty;

            LoadGroupFolder();
            loadGroupsGrid(0, isQuickTestBlast);
        }


        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            int grpFolderID = 0;
            if (GroupFolder.SelectedFolderID != null)
            {
                grpFolderID = Convert.ToInt32(GroupFolder.SelectedFolderID.ToString());
            }
            //loadGroupsGrid(grpFolderID);

            if (chkAllFolders.Checked)
            {
                GroupsGrid.Columns[0].Visible = true;
            }
            else
                GroupsGrid.Columns[0].Visible = false;

            if (ShowArchiveFilter)
            {
                ddlArchiveFilter.Visible = true;
            }
            else
            {
                ddlArchiveFilter.SelectedValue = "active";
                ddlArchiveFilter.Visible = false;
            }

        }

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
            GroupFolder.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            GroupFolder.FolderType = ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString();
            GroupFolder.NodesExpanded = true;
            GroupFolder.ChildrenExpanded = false;
            GroupFolder.LoadFolderTree();
        }

        private void loadGroupsGrid(int groupFolderID, bool isQuickTestBlast = false)
        {
            int testBlastLimit = 10;
            if(isQuickTestBlast)
            {
                if(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.TestBlastLimit.HasValue && ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.TestBlastLimit.Value > 0)
                {
                    testBlastLimit = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.TestBlastLimit.Value;
                }

                if(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.TestBlastLimit.HasValue && ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.TestBlastLimit.Value > 0)
                {
                    testBlastLimit = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.TestBlastLimit.Value;
                }
            }


            if (SearchCriteria.Text == string.Empty)
            {
                DataTable groupList = new DataTable();
                if (isQuickTestBlast)
                {
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers_NoAccessCheck(groupFolderID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString(), testBlastLimit);
                }
                else
                {
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers_NoAccessCheck(groupFolderID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                }
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
                string searchType = SearchTypeDR.SelectedItem.Value.ToString();
                DataTable groupList = new DataTable();

                if (searchType.Equals("Group"))
                {
                    if (isQuickTestBlast)
                    {
                        groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName_NoAccessCheck(SearchCriteria.Text, searchCrietria, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString(), testBlastLimit);
                    }
                    else
                    {
                        groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName_NoAccessCheck(SearchCriteria.Text, searchCrietria, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                    }
                }
                else if (searchType.Equals("Profile"))
                {
                    if (isQuickTestBlast)
                    {
                        groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByProfileName_NoAccessCheck(SearchCriteria.Text, searchCrietria, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString(), testBlastLimit);
                    }
                    else
                    {
                        groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByProfileName_NoAccessCheck(SearchCriteria.Text, searchCrietria, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                    }
                }

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

                //GroupsGrid.Columns[2].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //GroupsGrid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
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
            GroupsGrid.ShowEmptyTable = true;
            GroupsGrid.EmptyTableRowText = "No Groups to display";


            UpdatePanel1.Update();
        }

        protected void btnPreviousGroup_Click(object sender, EventArgs e)
        {
            if (GroupGridPageIndex > 0)
            {
                GroupGridPageIndex--;
                loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID), isQTB);
            }
        }

        protected void btnNextGroup_Click(object sender, EventArgs e)
        {

            var maxPage = lblTotalNumberOfPagesGroup.Text;
            if (GroupGridPageIndex + 1 < int.Parse(maxPage))
            {

                GroupGridPageIndex++;
                loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID), isQTB);
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
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID), isQTB);
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
                Label lblFolder = (Label)e.Row.FindControl("lblFolderName");
                DataRowView drv = (DataRowView)e.Row.DataItem;
                lblFolder.Text = drv["FolderName"].ToString();

            }
        }

        protected void groupExplorer_Hide(object sender, EventArgs e)
        {
            hideGroupsLookupPopup.DynamicInvoke();
        }

        #region Group Grid Pager Evetns
        protected void GroupGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.GroupsGrid.PageSize = int.Parse(dropDown.SelectedValue);
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
            GroupGridPageIndex = 0;
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID), isQTB);
        }

    }
}