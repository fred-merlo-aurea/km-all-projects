using System;
using System.Data;
using System.Web.UI.WebControls;
using ECN.Common.Helpers;
using ECN_Framework_Common.Objects;

namespace ecn.editor.ckeditor.controls
{
    public partial class groupexplorer : System.Web.UI.UserControl
    {
        public Delegate hideGroupsLookupPopup;

        private string _UDFFilter { get; set; }

        public string UDFFilter
        {
            get
            {
                if (!string.IsNullOrEmpty(_UDFFilter))
                {
                    return _UDFFilter;
                }
                else
                {
                    return "standalone";
                }
            }
            set
            {
                _UDFFilter = value;
            }
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
        private int GroupsGridPageIndex
        {
            get
            {
                if (ViewState["GroupGridPageIndex"] != null)
                    return (int)ViewState["GroupGridPageIndex"];
                else
                    return 0;
            }
            set
            {
                ViewState["GroupGridPageIndex"] = value;
            }

        }

        public int selectedGroupID
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(selectedGroupID), 0); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(selectedGroupID), value); }
        }

        public void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            EcnErrorHelper.SetEcnError(phError, lblErrorMessage, ecnException);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            GroupFolder.FolderEvent += new EventHandler(GroupFolderEvent);
            phError.Visible = false;
            this.modalPopupGroupExplorer.Show();
        }

        public void LoadControl()
        {
            this.modalPopupGroupExplorer.Show();
            reset();
        }

        public void HideControl()
        {
            this.modalPopupGroupExplorer.Hide();
            upGroups.Update();
        }

        private void reset()
        {
            selectedGroupID = 0;
            SearchTypeDR.SelectedIndex = 0;
            SearchGrpsDR.SelectedIndex = 0;
            chkAllFolders.Checked = false;
            this.GroupsGrid.PageSize = 15;
            GroupsGridPageIndex = 0;
            
            SearchCriteria.Text = string.Empty;
            LoadGroupFolder();
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
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
                GroupsGridPageIndex = e.NewPageIndex;
            }
            GroupsGrid.DataBind();
        }

        private void GroupFolderEvent(object sender, EventArgs e)
        {
            TreeView tn = (TreeView)sender;
            GroupsGridPageIndex = 0;
            SearchCriteria.Text = "";
            chkAllFolders.Checked = false;
            loadGroupsGrid(Convert.ToInt32(tn.SelectedNode.Value));
            chkAllFolders.Checked = false;
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

        private void loadGroupsGrid(int groupFolderID)
        {
            if (SearchCriteria.Text == string.Empty)
            {
                DataTable groupList = new DataTable();
                if (UDFFilter.ToLower().Equals("standalone") || UDFFilter.ToLower().Equals("all"))
                {
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers_NoAccessCheck(groupFolderID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, GroupsGridPageIndex + 1, this.GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                }
                else if (UDFFilter.ToLower().Equals("transactional"))
                {
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetTransactional_NoAccessCheck(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, "group", "like", SearchCriteria.Text, GroupsGridPageIndex + 1, this.GroupsGrid.PageSize, groupFolderID, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                }
                GroupsRecordCount = groupList.Rows.Count;
                DataView dvgroup = groupList.DefaultView;
                GroupsGrid.DataSource = dvgroup;

                if (groupList.Rows.Count > 0)
                    GroupsRecordCount = Convert.ToInt32(groupList.Rows[0]["TotalCount"].ToString());
                else
                    GroupsRecordCount = groupList.Rows.Count;


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
                    if (UDFFilter.ToLower().Equals("standalone") || UDFFilter.ToLower().Equals("all"))
                    {
                        groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName_NoAccessCheck(SearchCriteria.Text, searchCrietria, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, groupFolderID, GroupsGridPageIndex + 1, this.GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                    }
                    else if (UDFFilter.ToLower().Equals("transactional"))
                    {
                        groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetTransactional_NoAccessCheck(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, searchType, searchCrietria, SearchCriteria.Text, GroupsGridPageIndex + 1, this.GroupsGrid.PageSize, groupFolderID, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                    }
                }
                else if (searchType.Equals("Profile"))
                {
                    if (UDFFilter.ToLower().Equals("standalone") || UDFFilter.ToLower().Equals("all"))
                    {
                        groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByProfileName_NoAccessCheck(SearchCriteria.Text, searchCrietria, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, groupFolderID, GroupsGridPageIndex + 1, this.GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                    }
                    else if (UDFFilter.ToLower().Equals("transactional"))
                    {
                        groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetTransactional_NoAccessCheck(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, searchType, searchCrietria, SearchCriteria.Text, GroupsGridPageIndex + 1, this.GroupsGrid.PageSize, groupFolderID, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                    }
                }

                GroupsGrid.DataSource = groupList;
                if (groupList.Rows.Count > 0)
                    GroupsRecordCount = Convert.ToInt32(groupList.Rows[0]["TotalCount"].ToString());
                else
                    GroupsRecordCount = groupList.Rows.Count;
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

            //Label lblTotalRecordsContent = (Label)e.Row.FindControl("lblTotalRecordsGroup");
            lblTotalRecordsGroup.Text = GroupsRecordCount.ToString();

            //Label lblTotalNumberOfPagesContent = (Label)e.Row.FindControl("lblTotalNumberOfPagesGroup");
            lblTotalNumberOfPagesGroup.Text = rountUpPageCount.ToString();
            txtGoToPageGroup.Text = (GroupsGridPageIndex + 1) <= 1 ? "1" : (GroupsGridPageIndex + 1).ToString();

            if (GroupsRecordCount > 0)
            {
                pnlPager.Visible = true;
            }
            else
                pnlPager.Visible = false;

            ViewState["layoutGridPageCount"] = lblTotalNumberOfPagesGroup.Text;
            ddlPageSizeGroup.SelectedValue = GroupsGrid.PageSize.ToString();


            upGroups.Update();

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
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }

        protected void GoToPageGroup_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageContent = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageContent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.GroupsGrid.PageCount)
            {
                this.GroupsGridPageIndex = pageNumber - 1;
            }
            else
            {
                this.GroupsGridPageIndex = 0;
            }
                loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
            }
        #endregion

        protected void btnNextGroup_Click(object sender, EventArgs e)
        {
            int MaxPage = Convert.ToInt32(lblTotalNumberOfPagesGroup.Text);
            if (GroupsGridPageIndex + 1 < MaxPage)
            {
                GroupsGridPageIndex++;
                loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
            }
        }

        protected void btnPreviousGroup_Click(object sender, EventArgs e)
            {
            if (GroupsGridPageIndex > 0)
            {
                GroupsGridPageIndex--;
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }

    }
}