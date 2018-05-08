using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ecn.accounts.includes
{
    public partial class MultiSelectGroupExplorer : System.Web.UI.UserControl
    {
        int userID = 0;
        int customerID = 0;
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

        public static int CustomerID { get; set; }

        public void reset(int customerID)
        {
            CustomerID = customerID;
            GroupFolder.CustomerID = customerID;
            ViewState["SelectedGroups_List"] = null;
            gvSelectedGroups.DataSource = null;
            gvSelectedGroups.DataBind();
            SearchCriteria.Text = "";
            SearchGrpsDR.SelectedIndex = 0;
            SearchTypeDR.SelectedIndex = 0;
            chkAllFolders.Checked = false;
            ddlArchiveFilter.SelectedIndex = 0;
            GroupGridPageIndex = 0;
            ddlPageSizeContent.SelectedValue = "15";
            GroupsGrid.PageSize = 15;
            LoadGroupFolder();
            loadGroupsGrid(0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            GroupFolder.FolderEvent += new EventHandler(GroupFolderEvent);
            userID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            customerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            if (!Page.IsPostBack)
            {
                GroupGridPageIndex = 0;
                ViewState["SelectedGroups_List"] = null;
                ViewState["SupressionGroups_List"] = null;
                LoadGroupFolder();
            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {


            if (chkAllFolders.Checked)
            {
                GroupsGrid.Columns[0].Visible = true;
            }
            else
            {
                GroupsGrid.Columns[0].Visible = false;
            }
        }

        private void LoadGroupFolder()
        {
            GroupFolder.ID = "GroupFolder";
            GroupFolder.CustomerID = CustomerID;
            GroupFolder.FolderType = ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString();
            GroupFolder.NodesExpanded = true;
            GroupFolder.ChildrenExpanded = false;
            GroupFolder.LoadFolderTree();
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }

        private void loadGroupsGrid(int groupFolderID)
        {
            if (SearchCriteria.Text == string.Empty)
            {
                DataTable groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers_NoAccessCheck(groupFolderID, CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());



                DataView dvgroup = groupList.DefaultView;
                GroupsGrid.DataSource = dvgroup;
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
                GroupsGrid.ShowEmptyTable = true;
                GroupsGrid.EmptyTableRowText = "No Groups to display";
            }
            else
            {
                string searchCrietria = SearchGrpsDR.SelectedItem.Value.ToString();
                string searchType = SearchTypeDR.SelectedItem.Value.ToString();
                DataTable groupList = new DataTable();

                if (searchType.Equals("Group"))
                {
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName_NoAccessCheck(SearchCriteria.Text, searchCrietria, CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());

                }
                else if (searchType.Equals("Profile"))
                {
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByProfileName_NoAccessCheck(SearchCriteria.Text, searchCrietria, CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
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
                GroupsGrid.ShowEmptyTable = true;
                GroupsGrid.EmptyTableRowText = "No Groups to display";

            }

            var exactPageCount = (double)GroupsRecordCount / (double)GroupsGrid.PageSize;
            var rountUpPageCount = Math.Ceiling((double)exactPageCount);

            lblTotalRecords.Text = GroupsRecordCount.ToString();

            lblTotalNumberOfPagesGroup.Text = rountUpPageCount.ToString();
            txtGoToPageContent.Text = (GroupGridPageIndex + 1) <= 1 ? "1" : (GroupGridPageIndex + 1).ToString();
            
            pnlPager.Visible = true;

            ViewState["layoutGridPageCount"] = lblTotalNumberOfPagesGroup.Text;
            ddlPageSizeContent.SelectedValue = GroupsGrid.PageSize.ToString();

            upMain.Update();
        }

        protected void gvSelectedGroups_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("RemoveGroup"))
            {
                int selectedGroupID = Convert.ToInt32(e.CommandArgument.ToString());
                List<GroupObject> dt = (List<GroupObject>)ViewState["SelectedGroups_List"];
                foreach (GroupObject dr in dt)
                {
                    if (dr.GroupID.ToString().Equals(selectedGroupID.ToString()))
                    {
                        dt.Remove(dr);
                        break;
                    }
                }
                ViewState["SelectedGroups_List"] = dt;

                gvSelectedGroups.DataSource = dt;
                gvSelectedGroups.DataBind();
                upMain.Update();
            }

        }

        private void GroupFolderEvent(object sender, EventArgs e)
        {
            TreeView tn = (TreeView)sender;
            GroupGridPageIndex = 0;
            loadGroupsGrid(Convert.ToInt32(tn.SelectedNode.Value));
            chkAllFolders.Checked = false;
        }

        protected void GoToPageGroup_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageContent = (TextBox)sender;
            int totalPages = Convert.ToInt32(lblTotalNumberOfPagesGroup.Text);
            int pageNumber;
            if (int.TryParse(txtGoToPageContent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= totalPages)
            {
                GroupGridPageIndex = pageNumber - 1;
            }
            else
            {
                GroupGridPageIndex = 0;
            }
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }

        

        protected void GroupsGrid_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectGroup"))
            {
                selectedGroupID = Convert.ToInt32(e.CommandArgument.ToString());
                setSelectedGroup(selectedGroupID);
                upMain.Update();
            }

        }

        protected void GroupGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.GroupsGrid.PageSize = int.Parse(dropDown.SelectedValue);
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }

        protected void GroupsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Pager)
            //{
            //    Label lblTotalRecordsContent = (Label)e.Row.FindControl("lblTotalRecordsGroup");
            //    lblTotalRecordsContent.Text = GroupsRecordCount.ToString();

            //    Label lblTotalNumberOfPagesContent = (Label)e.Row.FindControl("lblTotalNumberOfPagesGroup");
            //    lblTotalNumberOfPagesContent.Text = GroupsGrid.PageCount.ToString();

            //    TextBox txtGoToPageContent = (TextBox)e.Row.FindControl("txtGoToPageGroup");
            //    txtGoToPageContent.Text = (GroupsGrid.PageIndex + 1).ToString();

            //    DropDownList ddlPageSizeContent = (DropDownList)e.Row.FindControl("ddlPageSizeGroup");
            //    ddlPageSizeContent.SelectedValue = GroupsGrid.PageSize.ToString();
            //}

        }

        protected void gvSelectedGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //GroupObject rowView = (GroupObject)e.Row.DataItem;

                //int groupID = Convert.ToInt32(gvSelectedGroups.DataKeys[e.Row.RowIndex].Value.ToString());
                ////List<ECN_Framework_Entities.Communicator.Filter> filterList =
                ////ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID(groupID, true, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                //ImageButton imgbtnAddSubs = (ImageButton)e.Row.FindControl("imgbtnAddSubs");
                //imgbtnAddSubs.CommandArgument = groupID.ToString();
                //ImageButton imgbtnImportSubs = (ImageButton)e.Row.FindControl("imgbtnImportSubs");
                //imgbtnImportSubs.CommandArgument = groupID.ToString();

                //ecn.communicator.main.ECNWizard.Group.filtergrid filterGrid = (ecn.communicator.main.ECNWizard.Group.filtergrid)e.Row.FindControl("ucFilterGrid");
                //filterGrid.GroupID = groupID;
                //filterGrid.RowIndex = e.Row.RowIndex;
                //filterGrid.SetFilters(rowView, e.Row.RowIndex);


            }
        }

        protected void GroupsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                GroupsGrid.PageIndex = e.NewPageIndex;
            }
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
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

        public void setSelectedGroup(int selectedGroupID)
        {
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(selectedGroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            List<GroupObject> dt;

            if (ViewState["SelectedGroups_List"] != null)
            {
                dt = (List<GroupObject>)ViewState["SelectedGroups_List"];
            }
            else
            {
                dt = new List<GroupObject>();

            }
            if (group != null && group.GroupID > 0)
            {
                bool exists = false;
                if (dt.Count > 0)
                {
                    foreach (GroupObject drow in dt)
                    {
                        if (drow.GroupID.Equals(group.GroupID))
                        {
                            exists = true;
                            break;
                        }
                    }
                }

                if (!exists)
                {
                    GroupObject dr = new GroupObject();
                    dr.GroupID = group.GroupID;
                    dr.GroupName = group.GroupName;


                    dt.Add(dr);
                }
                else
                {
                    GroupObject go = dt.Find(x => x.GroupID == group.GroupID);
                    //getting the group object to add the filter to

                }
                if (ViewState["SelectedGroups_List"] != null)
                    ViewState["SelectedGroups_List"] = dt;
                else
                    ViewState.Add("SelectedGroups_List", dt);

                DataBindSelected();
            }
        }

        public List<GroupObject> getSelectedGroups()
        {
            List<GroupObject> retList = new List<GroupObject>();
            if (ViewState["SelectedGroups_List"] != null)
                return (List<GroupObject>)ViewState["SelectedGroups_List"];
            else
                return retList;

        }

        private void DataBindSelected()
        {
            if (ViewState["SelectedGroups_List"] != null)
            {
                List<GroupObject> list = (List<GroupObject>)ViewState["SelectedGroups_List"];
                gvSelectedGroups.Visible = true;
                gvSelectedGroups.DataSource = list;
                gvSelectedGroups.DataBind();

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
    }
    [Serializable]
    public class GroupObject
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }
}