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
    public partial class groupsExplorer : System.Web.UI.UserControl
    {
        int GroupsRecordCount = 0;
        private static int GroupGridPageIndex;
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

        private int getGroupID()
        {
            int theGroupID = 0;
            if (Request.QueryString["GroupID"] != null)
            {
                theGroupID = Convert.ToInt32(Request.QueryString["GroupID"].ToString());
            }
            return theGroupID;
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

        public void enableSelectMode()
        {
            GroupsGrid.Columns[2].Visible = false;
            GroupsGrid.Columns[3].Visible = false;
            GroupsGrid.Columns[4].Visible = false;
            GroupsGrid.Columns[5].Visible = false;
            GroupsGrid.Columns[6].Visible = false;
            GroupsGrid.Columns[7].Visible = false;
            GroupsGrid.Columns[8].Visible = false;
            GroupsGrid.Columns[9].Visible = true;
            pnlGroupSearch.Visible = true;
        }

        public void enableEditMode()
        {
            GroupsGrid.Columns[2].Visible = true;
            GroupsGrid.Columns[3].Visible = true;
            GroupsGrid.Columns[4].Visible = true;
            GroupsGrid.Columns[5].Visible = true;
            GroupsGrid.Columns[6].Visible = true;
            GroupsGrid.Columns[7].Visible = true;
            GroupsGrid.Columns[8].Visible = true;
            GroupsGrid.Columns[9].Visible = false;
            pnlGroupSearch.Visible = true;
        }


        protected void GroupGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupGridPageIndex = 0;
            this.GroupsGrid.PageSize = int.Parse(ddlPageSizeContent.SelectedValue);
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }

        protected void GoToPageGroup_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageContent = (TextBox)sender;

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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            int requestGroupID = getGroupID();
            GroupFolder.FolderEvent += new EventHandler(GroupFolderEvent);
            if (requestGroupID > 0)
            {
                DeleteGroup(requestGroupID);
            }

            if (!Page.IsPostBack)
            {
                GroupGridPageIndex = 0;
                LoadGroupFolder();
            }
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
            {
                GroupsGrid.Columns[0].Visible = false;
            }
        }

        public void reset()
        {
            SearchTypeDR.SelectedIndex = 0;
            SearchGrpsDR.SelectedIndex = 0;
            this.GroupsGrid.PageSize = 15;
            this.GroupsGrid.PageIndex = 0;
            SearchCriteria.Text = string.Empty;
            chkAllFolders.Checked = false;
            LoadGroupFolder();
            loadGroupsGrid(0);
        }

        private void GroupFolderEvent(object sender, EventArgs e)
        {
            TreeView tn = (TreeView)sender;
            GroupGridPageIndex = 0;
            SearchCriteria.Text = "";
            chkAllFolders.Checked = false;
            loadGroupsGrid(Convert.ToInt32(tn.SelectedNode.Value));
            chkAllFolders.Checked = false;
        }

        protected void GroupsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                GroupsGrid.PageIndex = e.NewPageIndex;
            }
            GroupsGrid.DataBind();
        }

        private void loadGroupsGrid(int groupFolderID)
        {
            if (SearchCriteria.Text == string.Empty)
            {
                DataTable groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers(groupFolderID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                if (groupList.Rows.Count > 0)
                {
                    GroupsRecordCount = int.Parse(groupList.Rows[0].ItemArray[1].ToString());
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
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName(SearchCriteria.Text.Replace("'","''"), searchCrietria, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());

                }
                else if (searchType.Equals("Profile"))
                {
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByProfileName(SearchCriteria.Text.Replace("'", "''"), searchCrietria, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                }

                GroupsGrid.DataSource = groupList;
                if (groupList.Rows.Count > 0)
                {
                    GroupsRecordCount = int.Parse(groupList.Rows[0].ItemArray[1].ToString());
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


                GroupsGrid.Columns[2].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                GroupsGrid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }
            var exactPageCount = (double)GroupsRecordCount / (double)GroupsGrid.PageSize;
            var rountUpPageCount = Math.Ceiling((double)exactPageCount);

            lblTotalRecords.Text = GroupsRecordCount.ToString();

            lblTotalNumberOfPagesGroup.Text = rountUpPageCount.ToString();
            txtGoToPageContent.Text = (GroupGridPageIndex + 1) <= 1 ? "1" : (GroupGridPageIndex + 1).ToString();

            pnlPager.Visible = true;

            ViewState["layoutGridPageCount"] = lblTotalNumberOfPagesGroup.Text;
            ddlPageSizeContent.SelectedValue = GroupsGrid.PageSize.ToString();
            GroupsGrid.ShowEmptyTable = true;
            GroupsGrid.EmptyTableRowText = "No Groups to display";

            upMain.Update();

        }

        protected void GroupsGrid_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteGroup"))
            {
                DeleteGroup(Convert.ToInt32(e.CommandArgument.ToString()));
            }
            else if (e.CommandName.Equals("SelectGroup"))
            {
                selectedGroupID = Convert.ToInt32(e.CommandArgument.ToString());
                RaiseBubbleEvent("GroupSelected", new EventArgs());
            }
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

        protected void DeleteGroup(int theGroupID)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Group.Delete(theGroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void GroupsGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteGroup(Convert.ToInt32(GroupsGrid.DataKeys[e.RowIndex].Values[0]));
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

        protected void btnPreviousGroup_Click(object sender, EventArgs e)
        {
            if (GroupGridPageIndex > 0)
            {
                GroupGridPageIndex--;
            }
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));

        }

        protected void btnNextGroup_Click(object sender, EventArgs e)
        {

            var maxPage = lblTotalNumberOfPagesGroup.Text;
            if (GroupGridPageIndex + 1 < int.Parse(maxPage))
            {

                GroupGridPageIndex++;
            }

            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
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
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            GroupGridPageIndex = 0;
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }
    }
}