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
using System.Text;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Entities.Communicator;
using static ECN_Framework_BusinessLayer.Communicator.Filter;

namespace ecn.communicator.main.ECNWizard.Group
{
    public partial class testGroupExplorer : System.Web.UI.UserControl
    {
        private const string EditCustomFilterCommandName = "EditCustomFilter";
        private const string AddFilterCommandName = "AddFilter";
        private const string CreateCustomFilterCommandName = "createcustomfilter";
        private const string DeleteFilterCommandName = "deletessfilter";
        private const string DeleteCustomFilterCommandName = "deletecustomfilter";
        private const string RemoveGroupCommandName = "RemoveGroup";
        private const string ImportSubsCommandName = "importsubs";
        private const string AddSubsCommandName = "addsubs";
        private const string SelectedTestGroupsList = "SelectedTestGroups_List";
        private const char UnderscoreSeparator = '_';
        private const string LabelGroupId = "lblGroupID";
        private const string UcFilterGrid = "ucFilterGrid";
        private const string TestSelect = "testselect";
        private const string ShowSelectOne = "1";
        private const string ShowSelectZero = "0";
        private const int CustomFilterId = 6;
        int userID = 0;
        int customerID = 0;
        int GroupsRecordCount = 0;
        private static int GroupGridPageIndex;
        public string SuppressOrSelect
        {
            get
            {
                if (ViewState["SuppressOrSelect" + this.ClientID] != null)
                    return ViewState["SuppressOrSelect" + this.ClientID].ToString();
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["SuppressOrSelect" + this.ClientID] = value;
            }
        }
        private static int _GroupID;

        public int GroupID
        {
            get
            {
                if (_GroupID != null)
                    return _GroupID;
                else
                    return -1;
            }
            set
            {
                _GroupID = value;
            }
        }
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
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            userID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            customerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            GroupFolder.FolderEvent += new EventHandler(GroupFolderEvent);
            
            if (!Page.IsPostBack)
            {
                //LoadGroupFolder();
                ViewState["SelectedTestGroups_List"] = null;
                GroupGridPageIndex = 0;
            }
        }
        private void GroupFolderEvent(object sender, EventArgs e)
        {
            TreeView tn = (TreeView)sender;
            GroupGridPageIndex = 0;
            SearchCriteria.Text = "";
            SearchGrpsDR.SelectedIndex = 0;
            SearchTypeDR.SelectedIndex = 0;
            loadGroupsGrid(Convert.ToInt32(tn.SelectedNode.Value));
        }
        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("Upload"))
                {
                    modalPopupImport.Show();
                }
            }
            catch { }
            return true;
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            if (ViewState["SelectedTestGroups_List"] != null)
            {
                lblEmptyGrid_Selected.Visible = false;
                gvSelectedGroups.Visible = true;
                List<GroupObject> selectedDT = (List<GroupObject>)ViewState["SelectedTestGroups_List"];
                if (selectedDT.Count == 0)
                {
                    lblEmptyGrid_Selected.Visible = true;
                    gvSelectedGroups.Visible = false;
                }
            }
            //int grpFolderID = 0;
            //if (GroupFolder.SelectedFolderID != null)
            //{
            //    grpFolderID = Convert.ToInt32(GroupFolder.SelectedFolderID.ToString());
            //}
            //loadGroupsGrid(grpFolderID);
            blastLicenseCount_Update();
        }

        protected void GroupsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                GroupsGrid.PageIndex = e.NewPageIndex;
            }
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }

        private int getTestBlast_AllowedCount()
        {
            int count = 0;

            if (ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.TestBlastLimit.HasValue)
            {
                count = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.TestBlastLimit.Value;
            }
            else if (ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.TestBlastLimit.HasValue)
            {
                count = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.TestBlastLimit.Value;
            }
            else
            {
                count = 10;
            }
            //count = Convert.ToInt32(ConfigurationManager.AppSettings["CH_" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID + "_TestBlastEmails"].ToString());


            BlastLicensed.Text = count.ToString();
            return count;
        }

        private void blastLicenseCount_Update()
        {
            hfLicenseExceed.Value = "";
            int AllowedCount = getTestBlast_AllowedCount();
            int blastCount = 0;
            List<GroupObject> dtSelected = getSelectedGroups();
            StringBuilder xmlGroups = new StringBuilder();

            if (dtSelected != null)
            {
                int thisgroupcount = 0;

                foreach (GroupObject dr in dtSelected)
                {
                    thisgroupcount = 0;

                    xmlGroups.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    xmlGroups.Append("<NoBlast>");

                    int selectedGroupID = dr.GroupID;
                    //int filterID = Convert.ToInt32(dr["FilterID"].ToString());
                    //string refBlastList = dr.ToString();
                    //ECN_Framework_Entities.Communicator.SmartSegment ss = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegmentByID(filterID);

                    xmlGroups.Append("<Group id=\"" + selectedGroupID.ToString() + "\">");
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in dr.filters.Where(x => x.SmartSegmentID != null).ToList())
                    {
                        xmlGroups.Append("<SmartSegmentID id=\"" + cibf.SmartSegmentID + "\">");
                        xmlGroups.Append("<RefBlastIDs>" + cibf.RefBlastIDs + "</RefBlastIDs></SmartSegmentID>");
                    }

                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in dr.filters.Where(x => x.FilterID != null).ToList())
                    {
                        xmlGroups.Append("<FilterID id=\"" + cibf.FilterID.ToString() + "\" />");
                    }

                    xmlGroups.Append("</Group>");

                    xmlGroups.Append("<SuppressionGroups></SuppressionGroups></NoBlast>");

                    DataTable dt = ECN_Framework_BusinessLayer.Communicator.Blast.GetEstimatedSendsCount(xmlGroups.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
                    thisgroupcount = Convert.ToInt32(dt.Rows[0][0].ToString());
                    blastCount = blastCount + thisgroupcount;
                    if (blastCount > AllowedCount)
                    {
                        hfLicenseExceed.Value = hfLicenseExceed.Value + "'" + dr.GroupName + "',";
                    }



                    xmlGroups.Clear();
                }
            }

            hfLicenseExceed.Value = hfLicenseExceed.Value.Trim(',');

            if (blastCount >= 0)
            {
                BlastThis.Text = blastCount.ToString();
            }
            else if (blastCount < 0)
            {
                BlastThis.Text = "0";
            }

            if (dtSelected.Count > 0)
            {
                BlastLicensed.Text = AllowedCount.ToString();
                //BlastLicensed.Text = (AllowedCount * dtSelected.Rows.Count).ToString();
            }

            //if (!hfLicenseExceed.Value.Equals(string.Empty))
            //{
            //    throwECNException("ERROR - License limit exceeded for " + hfLicenseExceed.Value);
            //    return;
            //}

            upMain.Update();
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Blast, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        private void loadGroupsGrid(int groupFolderID, string archiveFilter = "active")
        {
            if (SearchCriteria.Text == string.Empty)
            {
                DataTable groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers_NoAccessCheck(groupFolderID,ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, GroupGridPageIndex + 1, GroupsGrid.PageSize, false, archiveFilter: archiveFilter);

                if (groupList.Rows.Count > 0)
                {
                    GroupsRecordCount = int.Parse(groupList.Rows[0].ItemArray[1].ToString());
                }

                List<Group> grpList = new List<Group>();
                foreach (DataRow dr in groupList.AsEnumerable())
                {
                    Group myGroup = new Group();
                    myGroup.GroupID = Convert.ToInt32(dr["GroupID"]);
                    myGroup.GroupName = dr["GroupName"].ToString();
                    myGroup.Subscribers = Convert.ToInt32(dr["Subscribers"]);
                    grpList.Add(myGroup);
                }
                var result = (from src in grpList
                              orderby src.GroupName
                              select new
                              {
                                  GroupID = src.GroupID,
                                  GroupName = src.GroupName,
                                  Subscribers = src.Subscribers
                              }).ToList();
                //GroupsRecordCount = result.Count;
                GroupsGrid.DataSource = result;
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
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName_NoAccessCheck(SearchCriteria.Text, searchCrietria,ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, Convert.ToInt32(GroupFolder.SelectedFolderID), GroupGridPageIndex + 1, GroupsGrid.PageSize, true, archiveFilter: archiveFilter);

                }
                else if (searchType.Equals("Profile"))
                {
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByProfileName_NoAccessCheck(SearchCriteria.Text, searchCrietria,ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, Convert.ToInt32(GroupFolder.SelectedFolderID), GroupGridPageIndex + 1, GroupsGrid.PageSize, true, archiveFilter: archiveFilter);
                }
                if (groupList.Rows.Count > 0)
                {
                    GroupsRecordCount = int.Parse(groupList.Rows[0].ItemArray[1].ToString());
                }

                List<Group> grpList = new List<Group>();
                foreach (DataRow dr in groupList.AsEnumerable())
                {
                    Group myGroup = new Group();
                    myGroup.GroupID = Convert.ToInt32(dr["GroupID"]);
                    myGroup.GroupName = dr["GroupName"].ToString();
                    myGroup.Subscribers = Convert.ToInt32(dr["Subscribers"]);
                    grpList.Add(myGroup);
                }
                var result = (from src in grpList
                              orderby src.GroupName
                              select new
                              {
                                  GroupID = src.GroupID,
                                  GroupName = src.GroupName,
                                  Subscribers = src.Subscribers
                              }).ToList();
                //GroupsRecordCount = result.Count;
                GroupsGrid.DataSource = result;
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

        protected void btnPreviousGroup_Click(object sender, EventArgs e)
        {
            if (GroupGridPageIndex > 0)
            {
                GroupGridPageIndex--;
                loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID), "active");
            }
        }

        protected void btnNextGroup_Click(object sender, EventArgs e)
        {
            var maxPage = lblTotalNumberOfPagesGroup.Text;
            if (GroupGridPageIndex + 1 < int.Parse(maxPage))
            {
                GroupGridPageIndex++;
                loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID), "active");
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
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID), "active");
        }

        protected void GroupsGrid_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectGroup"))
            {
                selectedGroupID = Convert.ToInt32(e.CommandArgument.ToString());
                setSelectedGroup(selectedGroupID, 0, null);
            }
            else if (e.CommandName.Equals("AddFilter"))
            {
                selectedGroupID = Convert.ToInt32(e.CommandArgument.ToString());
                filterEdit1.selectedGroupID = selectedGroupID;
                filterEdit1.selectedFilterID = 0;
                filterEdit1.loadData();
                modalPopupFilterEdit.Show();
            }
        }

        private void setSelectedGroup(int selectedGroupID, int? filterID, string refBlastList)
        {
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(selectedGroupID);
            List<GroupObject> dt;
            if (ViewState["SelectedTestGroups_List"] != null)
            {
                dt = (List<GroupObject>)ViewState["SelectedTestGroups_List"];
            }
            else
            {
                dt = new List<GroupObject>();
            }
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
                dr.filters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
                if (filterID != null && filterID > 0)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();

                    if (!string.IsNullOrEmpty(refBlastList))
                    {
                        cibf.RefBlastIDs = refBlastList;
                        cibf.SmartSegmentID = filterID.Value;

                    }
                    else
                    {
                        cibf.FilterID = filterID;
                    }
                    cibf.IsDeleted = false;
                    dr.filters.Add(cibf);
                }
                dt.Add(dr);
            }
            else
            {
                GroupObject go = dt.Find(x => x.GroupID == group.GroupID);
                //getting the group object to add the filter to
                if (go != null)
                {
                    //check to see the filter doesn't already exist for the group object
                    //if it does exist we don't need to do anything
                    if (filterID != null && filterID > 0 && go.filters.Count(x => x.FilterID == filterID) == 0)
                    {
                        //remove group object from the original list so we can replace it
                        dt.Remove(go);
                        ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();

                        if (!string.IsNullOrEmpty(refBlastList))
                        {
                            cibf.RefBlastIDs = refBlastList;
                            cibf.SmartSegmentID = filterID.Value;

                        }
                        else
                        {
                            cibf.FilterID = filterID;
                        }
                        cibf.IsDeleted = false;
                        //add the new filter to the filter list and add the group object back into the original list
                        go.filters.Add(cibf);
                        dt.Add(go);
                    }

                }
            }
            ViewState["SelectedTestGroups_List"] = dt;
            DataBindSelected();
        }

        public void LoadGroupFolder()
        {
            GroupFolder.ID = "GroupFolder";
            GroupFolder.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            GroupFolder.FolderType = ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString();
            GroupFolder.NodesExpanded = true;
            GroupFolder.ChildrenExpanded = false;
            GroupFolder.LoadFolderTree();
            loadGroupsGrid(0);
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

        protected void gvSelectedGroups_Command(object sender, GridViewCommandEventArgs eventArgs)
        {
            var commandName = eventArgs.CommandName;

            if (commandName.Equals(EditCustomFilterCommandName))
            {
                EditCustomFilterCommand(eventArgs);
            }
            else if (commandName.Equals(AddFilterCommandName))
            {
                AddFilterCommand(eventArgs);
            }
            else if (commandName.Equals(CreateCustomFilterCommandName, StringComparison.OrdinalIgnoreCase))
            {
                CreateCustomFilterCommand(eventArgs);
            }
            else if (commandName.Equals(DeleteFilterCommandName, StringComparison.OrdinalIgnoreCase))
            {
                DeleteFilterCommand(eventArgs);
            }
            else if (commandName.Equals(DeleteCustomFilterCommandName, StringComparison.OrdinalIgnoreCase))
            {
                DeleteCustomFilterCommand(eventArgs);
            }
            else if (commandName.Equals(RemoveGroupCommandName))
            {
                RemoveGroupCommand(eventArgs);
            }
            else if (commandName.Equals(ImportSubsCommandName))
            {
                ImportSubsCommand(eventArgs);
            }
            else if (commandName.Equals(AddSubsCommandName))
            {
                AddSubsCommand(eventArgs);
            }
        }

        private void AddSubsCommand(CommandEventArgs eventArgs)
        {
            var selectedGroupId = Convert.ToInt32(eventArgs.CommandArgument.ToString());
            SelectedGroupID.Value = selectedGroupId.ToString();
            addSubscribers1.GroupID = selectedGroupId;
            addSubscribers1.loadDropDowns(ECNSession.CurrentSession().CurrentUser.CustomerID);
            modalPopupAddSubscribers.Show();
        }

        private void ImportSubsCommand(CommandEventArgs eventArgs)
        {
            var selectedGroupId = Convert.ToInt32(eventArgs.CommandArgument.ToString());
            SelectedGroupID.Value = selectedGroupId.ToString();
            importSubscribers1.GroupID = selectedGroupId;
            modalPopupImport.Show();
        }

        private void RemoveGroupCommand(CommandEventArgs eventArgs)
        {
            var selectedGroupId = Convert.ToInt32(eventArgs.CommandArgument.ToString());
            var groupObjects = (List<GroupObject>)ViewState[SelectedTestGroupsList];
            foreach (var groupObject in groupObjects)
            {
                if (groupObject.GroupID.ToString().Equals(selectedGroupId.ToString()))
                {
                    groupObjects.Remove(groupObject);
                    break;
                }
            }

            ViewState[SelectedTestGroupsList] = groupObjects;
            if (groupObjects.Count == 0)
            {
                lblEmptyGrid_Selected.Visible = true;
                gvSelectedGroups.Visible = false;
            }

            gvSelectedGroups.DataSource = groupObjects;
            gvSelectedGroups.DataBind();
        }

        private void DeleteCustomFilterCommand(CommandEventArgs eventArgs)
        {
            var split = eventArgs.CommandArgument.ToString().Split(UnderscoreSeparator);
            var groupObjects = (List<GroupObject>)ViewState[SelectedTestGroupsList];
            var filterId = Convert.ToInt32(split[0]);
            var groupId = Convert.ToInt32(split[1]);

            var currentGroup = groupObjects.Find(x => x.GroupID == groupId);
            var currentIndex = groupObjects.IndexOf(currentGroup);
            groupObjects.Remove(currentGroup);
            var filter = currentGroup?.filters.Find(x => x.FilterID == filterId);
            if (filter != null)
            {
                currentGroup.filters.Remove(filter);
            }

            groupObjects.Insert(currentIndex, currentGroup);

            ViewState[SelectedTestGroupsList] = groupObjects;
            DataBindSelected();
        }

        private void DeleteFilterCommand(CommandEventArgs eventArgs)
        {
            var split = eventArgs.CommandArgument.ToString().Split(UnderscoreSeparator);
            var groupObjects = (List<GroupObject>)ViewState[SelectedTestGroupsList];
            var filterId = Convert.ToInt32(split[0]);
            var groupId = Convert.ToInt32(split[1]);

            var currentGroup = groupObjects.Find(x => x.GroupID == groupId);
            var currentIndex = groupObjects.IndexOf(currentGroup);
            groupObjects.Remove(currentGroup);
            var filter = currentGroup?.filters.Find(x => x.SmartSegmentID == filterId);
            if (filter != null)
            {
                currentGroup.filters.Remove(filter);
            }

            groupObjects.Insert(currentIndex, currentGroup);

            ViewState[SelectedTestGroupsList] = groupObjects;
            DataBindSelected();
        }

        private void CreateCustomFilterCommand(CommandEventArgs eventArgs)
        {
            var gridViewRow = gvSelectedGroups.Rows[Convert.ToInt32(eventArgs.CommandArgument.ToString())];
            var labelGroupId = (Label)gridViewRow.FindControl(LabelGroupId);
            var groupId = Convert.ToInt32(labelGroupId.Text);
            var filtergrid = (filtergrid)gridViewRow.FindControl(UcFilterGrid);
            filtergrid.SuppressOrSelect = TestSelect;
            filterEdit1.selectedGroupID = groupId;
            filterEdit1.selectedFilterID = 0;
            filterEdit1.loadData();
            hfShowSelect.Value = ShowSelectOne;
            modalPopupFilterEdit.Show();
        }

        private void AddFilterCommand(CommandEventArgs eventArg)
        {
            var gridViewRow = gvSelectedGroups.Rows[Convert.ToInt32(eventArg.CommandArgument.ToString())];
            var labelGroupId = (Label)gridViewRow.FindControl(LabelGroupId);
            var groupId = Convert.ToInt32(labelGroupId.Text);
            imgbtnCreateFilter.CommandArgument = gridViewRow.RowIndex.ToString();
            btnFilterEdit_Close.CommandArgument = gridViewRow.RowIndex.ToString();
            btnFilterEdit_Close.CommandName = TestSelect;
            hfShowSelect.Value = ShowSelectOne;
            ResetAddFilter(TestSelect, groupId, true, false, "true");
        }

        private void EditCustomFilterCommand(CommandEventArgs eventArg)
        {
            var filterId = Convert.ToInt32(eventArg.CommandArgument.ToString());

            if (filterId <= CustomFilterId)
            {
                return;
            }

            var filter = GetByFilterID_NoAccessCheck(filterId);
            filterEdit1.selectedGroupID = filter.GroupID.Value;
            filterEdit1.selectedFilterID = filterId;
            filterEdit1.loadData();
            hfShowSelect.Value = ShowSelectZero;
            modalPopupFilterEdit.Show();
        }

        private void DataBindSelected()
        {
            List<GroupObject> listGroups = (List<GroupObject>)ViewState["SelectedTestGroups_List"];
            gvSelectedGroups.DataSource = listGroups;
            gvSelectedGroups.DataBind();
        }

        protected void FilterEdit_Hide(object sender, EventArgs e)
        {
            filterEdit1.reset();
            Button closeButton = (Button)sender;
            if (closeButton.CommandName.ToLower().Equals("testselect"))
            {
                GridViewRow gvr = gvSelectedGroups.Rows[Convert.ToInt32(closeButton.CommandArgument.ToString())];
                Label lblGroupID = (Label)gvr.FindControl("lblGroupID");
                int groupID = Convert.ToInt32(lblGroupID.Text);
                filtergrid fg = (filtergrid)gvr.FindControl("ucFilterGrid");
                bool showSelect = false;
                if (hfShowSelect.Value.Equals("1"))
                    showSelect = true;
                ResetAddFilter("testselect", groupID, showSelect, true, "true");

            }
            this.modalPopupFilterEdit.Hide();
        }

        protected void ImportGroup_Hide(object sender, EventArgs e)
        {
            importSubscribers1.reset();
            this.modalPopupImport.Hide();
            blastLicenseCount_Update();
        }

        protected void AddSubscribers_Save(object sender, EventArgs e)
        {
            if (addSubscribers1.Save())
            {
                addSubscribers1.Reset();
                this.modalPopupAddSubscribers.Hide();
                blastLicenseCount_Update();
            }
            //TODO:ROHIT updateLicense();
        }

        protected void AddSubscribers_Close(object sender, EventArgs e)
        {
            addSubscribers1.Reset();
            this.modalPopupAddSubscribers.Hide();
            //blastLicenseCount_Update();
        }

        protected void AddGroup_Save(object sender, EventArgs e)
        {
            if (addGroup1.Save())
            {
                addGroup1.Reset();
                this.modalPopupAddGroup.Hide();
            }
        }

        protected void AddGroup_Close(object sender, EventArgs e)
        {
            addGroup1.Reset();
            this.modalPopupAddGroup.Hide();
        }

        protected void gvSelectedGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GroupObject rowView = (GroupObject)e.Row.DataItem;

                int groupID = Convert.ToInt32(gvSelectedGroups.DataKeys[e.Row.RowIndex].Value.ToString());
                List<ECN_Framework_Entities.Communicator.Filter> filterList =
                GetByGroupID_NoAccessCheck(groupID, true);
                ImageButton imgbtnAddSubs = (ImageButton)e.Row.FindControl("imgbtnAddSubs");
                imgbtnAddSubs.CommandArgument = groupID.ToString();
                ImageButton imgbtnImportSubs = (ImageButton)e.Row.FindControl("imgbtnImportSubs");
                imgbtnImportSubs.CommandArgument = groupID.ToString();

                ecn.communicator.main.ECNWizard.Group.filtergrid filterGrid = (ecn.communicator.main.ECNWizard.Group.filtergrid)e.Row.FindControl("ucFilterGrid");
                filterGrid.GroupID = groupID;
                filterGrid.RowIndex = e.Row.RowIndex;
                filterGrid.SetFilters(rowView, e.Row.RowIndex);
            }
        }


        private string getRefBlasts()
        {
            string refBlastIDs = "";
            if (lstboxBlast.Items.Count > 0)
            {
                foreach (ListItem item in lstboxBlast.Items)
                {
                    if (item.Selected)
                    {
                        if (refBlastIDs == string.Empty)
                        {
                            refBlastIDs = item.Value.ToString();
                        }
                        else
                        {
                            refBlastIDs += "," + item.Value.ToString();
                        }
                    }
                }
            }
            return refBlastIDs;
        }

        protected void GroupsGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteGroup(Convert.ToInt32(GroupsGrid.DataKeys[e.RowIndex].Values[0]));
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

        public List<GroupObject> getSelectedGroups()
        {
            List<GroupObject> retList = new List<GroupObject>();
            if (ViewState["SelectedTestGroups_List"] != null)
                return (List<GroupObject>)ViewState["SelectedTestGroups_List"];
            else
                return retList;
        }

        private void ResetAddFilter(string suppressorselect, int groupID, bool showSelect, bool custom = false, string istestblast = "false")
        {
            lblRefBlastError.Visible = false;
            rblFilterType.Enabled = true;
            if (!custom)
            {
                rblFilterType.SelectedValue = "smart";
                pnlSmartSegment.Visible = true;
                pnlCustomFilter.Visible = false;
            }
            else
            {
                rblFilterType.SelectedValue = "custom";
                pnlSmartSegment.Visible = false;
                pnlCustomFilter.Visible = true;
            }
            SuppressOrSelect = suppressorselect;

            GroupID = groupID;
            List<GroupObject> dt = new List<GroupObject>();

            if (suppressorselect.Equals("testselect"))
            {
                dt = (List<GroupObject>)ViewState["SelectedTestGroups_List"];
            }

            List<ECN_Framework_Entities.Communicator.Filter> filterList =
GetByGroupID_NoAccessCheck(groupID, true);
            lbAvailableFilters.DataSource = filterList;
            lbAvailableFilters.DataTextField = "FilterName";
            lbAvailableFilters.DataValueField = "FilterID";
            lbAvailableFilters.DataBind();

            if (istestblast.ToLower().Equals("false"))
            {
                List<ECN_Framework_Entities.Communicator.SmartSegment> listSS = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegments().Where(x => x.SmartSegmentID != 2).ToList();
                var ssIDs = new List<int?>();
                if (dt.Find(x => x.GroupID == groupID).filters != null)
                    ssIDs = dt.Find(x => x.GroupID == groupID).filters.Select(y => y.SmartSegmentID).ToList();
                ddlSmartSegment.DataSource = listSS.Where(x => !ssIDs.Contains(x.SmartSegmentID));
                ddlSmartSegment.DataTextField = "SmartSegmentName";
                ddlSmartSegment.DataValueField = "SmartSegmentID";
                ddlSmartSegment.DataBind();

                if (_GroupID != null)
                {
                    List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList =
                        ECN_Framework_BusinessLayer.Communicator.Blast.GetByGroupID_NoAccessCheck(groupID,  false);
                    var result = (from src in blastList
                                  orderby src.BlastID descending
                                  select new
                                  {
                                      BlastID = src.BlastID,
                                      EmailSubject = "[" + src.BlastID.ToString() + "] " + src.EmailSubject
                                  }).ToList();
                    lstboxBlast.DataSource = result;
                    lstboxBlast.DataTextField = "EmailSubject";
                    lstboxBlast.DataValueField = "BlastID";
                    lstboxBlast.DataBind();


                }
            }
            else
            {
                rblFilterType.SelectedValue = "custom";
                rblFilterType.Enabled = false;
                pnlSmartSegment.Visible = false;
                pnlCustomFilter.Visible = true;
            }
            pnlFilterConfig.Update();
            if (showSelect)
                mpeAddFilter.Show();
        }
        protected void btnSaveFilter_Click(object sender, EventArgs e)
        {
            lblRefBlastError.Visible = false;
            List<GroupObject> dt = new List<GroupObject>();
            if (SuppressOrSelect.ToLower().Equals("testselect"))
            {
                if (ViewState["SelectedTestGroups_List"] != null)
                {
                    dt = (List<GroupObject>)ViewState["SelectedTestGroups_List"];
                }
            }

            if (dt != null && dt.Count > 0)
            {
                GroupObject current = dt.Find(x => x.GroupID == GroupID);
                if (current != null)
                {
                    int currentIndex = dt.IndexOf(current);
                    if (rblFilterType.SelectedValue.ToLower().Equals("smart"))
                    {


                        if (current.filters.Count(x => x.SmartSegmentID == Convert.ToInt32(ddlSmartSegment.SelectedValue.ToString())) == 0)
                        {
                            ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                            cibf.SmartSegmentID = Convert.ToInt32(ddlSmartSegment.SelectedValue.ToString());
                            cibf.IsDeleted = false;
                            cibf.RefBlastIDs = getRefBlasts();
                            if (string.IsNullOrEmpty(cibf.RefBlastIDs.ToString()))
                            {
                                lblRefBlastError.Text = "Please select a blast for the smart segment";
                                lblRefBlastError.Visible = true;
                                mpeAddFilter.Show();
                                return;
                            }
                            dt.Remove(current);
                            current.filters.Add(cibf);
                        }

                    }
                    else if (rblFilterType.SelectedValue.ToLower().Equals("custom"))
                    {
                        dt.Remove(current);
                        foreach (ListItem li in lbAvailableFilters.Items)
                        {
                            if (li.Selected)
                            {
                                if (current.filters.Count(x => x.FilterID == Convert.ToInt32(li.Value.ToString())) == 0)
                                {
                                    ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                                    cibf.FilterID = Convert.ToInt32(li.Value.ToString());
                                    cibf.IsDeleted = false;

                                    current.filters.Add(cibf);
                                }
                            }
                        }
                    }
                    dt.Insert(currentIndex, current);
                    if (SuppressOrSelect.ToLower().Equals("testselect"))
                    {
                        if (ViewState["SelectedTestGroups_List"] != null)
                            ViewState["SelectedTestGroups_List"] = dt;
                        else
                            ViewState.Add("SelectedTestGroups_List", dt);
                        DataBindSelected();
                    }

                    mpeAddFilter.Hide();
                }
                else
                {
                    mpeAddFilter.Hide();
                }
            }
        }

        protected void rblFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblFilterType.SelectedValue.ToLower().Equals("smart"))
            {
                pnlSmartSegment.Visible = true;
                pnlCustomFilter.Visible = false;
            }
            else
            {
                pnlSmartSegment.Visible = false;
                pnlCustomFilter.Visible = true;
            }
            mpeAddFilter.Show();
        }

        protected void btnCancelFilter_Click(object sender, EventArgs e)
        {
            mpeAddFilter.Hide();
        }

        protected void imgbtnCreateFilter_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ImageButton imgbtnCreateFil = (ImageButton)sender;
            GridViewRow gvr = gvSelectedGroups.Rows[Convert.ToInt32(imgbtnCreateFil.CommandArgument.ToString())];
            Label lblGroupID = (Label)gvr.FindControl("lblGroupID");
            int GroupID = Convert.ToInt32(lblGroupID.Text);
            filtergrid fg = (filtergrid)gvr.FindControl("ucFilterGrid");
            fg.SuppressOrSelect = "testselect";
            SuppressOrSelect = "testselect";
            filterEdit1.selectedGroupID = GroupID;
            filterEdit1.selectedFilterID = 0;
            filterEdit1.loadData();
            hfShowSelect.Value = "1";
            modalPopupFilterEdit.Show();
            mpeAddFilter.Hide();
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            GroupGridPageIndex = 0;
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }
    }

    class Group
    {
        public int GroupID;
        public string GroupName;
        public int Subscribers;

    }
}