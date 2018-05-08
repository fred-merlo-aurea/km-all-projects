using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using static ECN_Framework_BusinessLayer.Communicator.Filter;

namespace ecn.communicator.main.ECNWizard.Group
{
    public partial class groupExplorer : System.Web.UI.UserControl
    {
        private const string EditCustomFilter = "EditCustomFilter";
        private const string AddFilter = "AddFilter";
        private const string CreateCustomFilter = "createcustomfilter";
        private const string DeleteFilter = "deletessfilter";
        private const string DeleteCustomFilter = "deletecustomfilter";
        private const string RemoveGroup = "RemoveGroup";
        private const string ImportSubs = "importsubs";
        private const string AddSubs = "addsubs";
        private const string SelectedGroupsListKey = "SelectedGroups_List";
        private const string SupressionGroupsListKey = "SupressionGroups_List";
        private const char UnderscoreSeparator = '_';
        private const string LabelGroupId = "lblGroupID";
        private const string Select = "select";
        private const string UcFiltergrid = "ucFilterGrid";
        private const int FilterId6 = 6;
        private const string ShowSelectZero = "0";
        private const string ShowSelectOne = "1";
        private const string SaveFilter = "savefilter";
        private const string Suppress = "suppress";
        private const string SuppressionFilterGrid = "fgSuppressionFilterGrid";

        int userID = 0;
        int customerID = 0;
        int GroupsRecordCount = 0;

        private int GroupGridPageIndex
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
        private int CampaignItemID
        {
            get
            {
                if (ViewState["CampaignItemID"] != null)
                    return Convert.ToInt32(ViewState["CampaignItemID"].ToString());
                else
                {
                    return -1;
                }
            }
            set
            {
                ViewState["CampaignItemID"] = value;
            }
        }

        private static bool IsSelect;

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
            IsSelect = true;
            GroupsGrid.Columns[2].Visible = false;
            GroupsGrid.Columns[3].Visible = false;
            GroupsGrid.Columns[4].Visible = false;
            GroupsGrid.Columns[5].Visible = false;
            GroupsGrid.Columns[6].Visible = false;
            GroupsGrid.Columns[7].Visible = false;
            GroupsGrid.Columns[8].Visible = false;
            GroupsGrid.Columns[9].Visible = false;
            GroupsGrid.Columns[10].Visible = true;
            GroupsGrid.Columns[11].Visible = ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastSuppression);
            pnlDownload.Visible = false;
            lnkbtnAddGroup.Visible = true;
            pnlSelectedGroup.Visible = true;
            pnlGroupSearch.Visible = true;
            lnkbtnAddGroup.Visible = true;
            ddlArchiveFilter.SelectedValue = "active";
            ddlArchiveFilter.Enabled = false;
        }

        public void enableEditMode()
        {
            IsSelect = false;
            lnkbtnAddGroup.Visible = false;
            GroupsGrid.Columns[2].Visible = true;
            GroupsGrid.Columns[3].Visible = true;
            GroupsGrid.Columns[4].Visible = true;
            GroupsGrid.Columns[5].Visible = true;
            GroupsGrid.Columns[6].Visible = KMPlatform.BusinessLogic.User.IsSystemAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit) ||
                KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.View)
                )
            {
                GroupsGrid.Columns[7].Visible = true;
            }
            else
            {
                GroupsGrid.Columns[7].Visible = false;
            }

            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Delete))
            {
                GroupsGrid.Columns[8].Visible = true;
            }
            else
            {
                GroupsGrid.Columns[8].Visible = false;
            }


            GroupsGrid.Columns[9].Visible = false;
            GroupsGrid.Columns[10].Visible = false;
            GroupsGrid.Columns[11].Visible = false;
            pnlDownload.Visible = true;
            pnlGroupSearch.Visible = true;
            lnkbtnAddGroup.Visible = false;
            pnlSelectedGroup.Visible = false;
        }


        protected void GroupGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupGridPageIndex = 0;
            this.GroupsGrid.PageSize = int.Parse(ddlPageSizeContent.SelectedValue);
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
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
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
        }


        protected void btnAddGroup_Click(object sender, System.EventArgs e)
        {
            addGroup1.LoadFolders(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
            this.modalPopupAddGroup.Show();
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;

            userID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            customerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            int requestGroupID = getGroupID();
            GroupFolder.FolderEvent += new EventHandler(GroupFolderEvent);
            DoTwemoji();
            if (requestGroupID > 0)
            {
                DeleteGroup(requestGroupID);
            }

            if (!Page.IsPostBack)
            {
                GroupGridPageIndex = 0;
                //ViewState["SelectedGroups_List"] = null;
                //ViewState["SupressionGroups_List"] = null;
                LoadGroupFolder();
                loadLicense();

                btnAddGroup.Visible = KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit);
            }
            checkForCustomerECNFeatures();
        }

        private void AssignTwemoji()
        {
            foreach (GridViewRow gvr in gvSelectedGroups.Rows)
            {
                ecn.communicator.main.ECNWizard.Group.filtergrid fg = (ecn.communicator.main.ECNWizard.Group.filtergrid)gvr.FindControl("ucFilterGrid");
                fg.EmojiEvent += new EventHandler(DoTwemojiOnGridHandler);
            }
        }

        private void DoTwemojiOnGridHandler(object sender, EventArgs e)
        {
            DoTwemoji();
        }

        private void DoTwemoji()
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "pageloaded();", true);
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

        private void loadLicense()
        {
            if (IsSelect)
            {
                ECN_Framework_Entities.Accounts.License lic =
                ECN_Framework_BusinessLayer.Accounts.License.GetCurrentLicensesByCustomerID(customerID, ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeCode.emailblock10k);

                if (lic.LicenseOption == ECN_Framework_Common.Objects.Accounts.Enums.LicenseOption.unlimited)
                {
                    BlastLicensed.Text = "UNLIMITED";
                    BlastAvailable.Text = "N/A";
                }
                else
                {
                    BlastLicensed.Text = lic.Allowed.ToString();
                    BlastAvailable.Text = lic.Available.ToString();
                }
                BlastUsed.Text = lic.Used.ToString();
            }
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
            DoTwemoji();
            return true;
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            if (IsSelect)
            {
                if (ViewState["SelectedGroups_List"] != null)
                {
                    lblEmptyGrid_Selected.Visible = false;
                    gvSelectedGroups.Visible = true;
                    List<GroupObject> selectedDT = (List<GroupObject>)ViewState["SelectedGroups_List"];

                    if (selectedDT.Count == 0)
                    {
                        lblEmptyGrid_Selected.Visible = true;
                        gvSelectedGroups.Visible = false;
                    }
                }
                if (ViewState["SupressionGroups_List"] != null)
                {
                    lblEmptyGrid_Supression.Visible = false;
                    gvSupression.Visible = true;
                    List<GroupObject> suppressionDT = (List<GroupObject>)ViewState["SupressionGroups_List"];

                    if (suppressionDT.Count == 0)
                    {
                        lblEmptyGrid_Supression.Visible = true;
                        gvSupression.Visible = false;
                    }
                    pnlBlastSuppression.Visible = ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastSuppression);

                }

                GroupsGrid.Columns[12].Visible = false;
            }
            else
            {

                //if (KM.Platform.User.IsAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                if (KM.Platform.User.IsAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                {
                    GroupsGrid.Columns[12].Visible = true;
                }
                else
                {
                    GroupsGrid.Columns[12].Visible = false;
                }
            }

            if (chkAllFolders.Checked)
            {
                GroupsGrid.Columns[0].Visible = true;
            }
            else
            {
                GroupsGrid.Columns[0].Visible = false;
            }

            //removing this, handling folder event elsewhere
            //int grpFolderID = 0;
            //if (GroupFolder.SelectedFolderID != null)
            //{
            //    grpFolderID = Convert.ToInt32(GroupFolder.SelectedFolderID.ToString());
            //}
            //loadGroupsGrid(grpFolderID);
            //if(IsSelect)
            //    blastLicenseCount_Update();

        }

        private void checkForCustomerECNFeatures()
        {
            if (!IsSelect)
            {
                KMPlatform.Entity.User user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
                //GroupsGrid.Columns[8].Visible = KMPlatform.BusinessLogic.User.HasPermission(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, "addgroup");
                GroupsGrid.Columns[7].Visible = KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit);
                GroupsGrid.Columns[4].Visible = KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.View);
                GroupsGrid.Columns[5].Visible = KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupUDFs, KMPlatform.Enums.Access.View);
                GroupsGrid.Columns[6].Visible = KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SmartForm, KMPlatform.Enums.Access.View);
                GroupsGrid.Columns[8].Visible = KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Delete);
            }
            else
            {
                KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
                gvSelectedGroups.Columns[2].Visible = gvSupression.Columns[2].Visible = KMPlatform.BusinessLogic.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.AddEmails);
                gvSelectedGroups.Columns[3].Visible = gvSupression.Columns[3].Visible = KMPlatform.BusinessLogic.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.ImportEmails);
            }
        }

        public void reset()
        {
            LoadGroupFolder();
            loadLicense();
            ViewState["SelectedGroups_List"] = null;
            ViewState["SupressionGroups_List"] = null;
            chkAllFolders.Checked = false;
            gvSelectedGroups.DataBind();
            gvSupression.DataBind();
            loadGroupsGrid(0);
            blastLicenseCount_Update();

        }

        protected void GroupsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                GroupsGrid.PageIndex = e.NewPageIndex;
            }
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
            DoTwemoji();
        }



        private void loadGroupsGrid(int groupFolderID)
        {
            if (SearchCriteria.Text == string.Empty)
            {

                DataTable groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers_NoAccessCheck(groupFolderID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());

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
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupName_NoAccessCheck(SearchCriteria.Text, searchCrietria, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());

                }
                else if (searchType.Equals("Profile"))
                {
                    groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByProfileName_NoAccessCheck(SearchCriteria.Text, searchCrietria, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, groupFolderID, GroupGridPageIndex + 1, GroupsGrid.PageSize, chkAllFolders.Checked, ddlArchiveFilter.SelectedValue.ToString());
                }

                if (groupList.Rows.Count > 0)
                {
                    GroupsRecordCount = int.Parse(groupList.Rows[0].ItemArray[1].ToString());
                }

                GroupsGrid.DataSource = groupList;

                try
                {
                    GroupsGrid.DataBind();
                }
                catch
                {
                    GroupsGrid.PageIndex = 0;
                    GroupsGrid.DataBind();
                }


                GroupsGrid.Columns[3].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                GroupsGrid.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
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
                loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
            }
            DoTwemoji();

        }

        protected void btnNextGroup_Click(object sender, EventArgs e)
        {

            var maxPage = lblTotalNumberOfPagesGroup.Text;
            if (GroupGridPageIndex + 1 < int.Parse(maxPage))
            {
                GroupGridPageIndex++;
                loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
            }
            DoTwemoji();

        }

        protected void GroupsGrid_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteGroup"))
            {
                DeleteGroup(Convert.ToInt32(e.CommandArgument.ToString()));
                loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
            }
            else if (e.CommandName.Equals("SelectGroup"))
            {
                selectedGroupID = Convert.ToInt32(e.CommandArgument.ToString());
                setSelectedGroup(selectedGroupID, 0, null, 0);
                blastLicenseCount_Update();
            }
            else if (e.CommandName.Equals("SuppressGroup"))
            {
                selectedGroupID = Convert.ToInt32(e.CommandArgument.ToString());
                setSuppressionGroup(selectedGroupID, null, "", 0);
                blastLicenseCount_Update();
            }

            DoTwemoji();
            //else if (e.CommandName.Equals("AddFilter"))
            //{
            //    selectedGroupID = Convert.ToInt32(e.CommandArgument.ToString());
            //    filterEdit1.selectedGroupID = selectedGroupID;
            //    filterEdit1.selectedFilterID = 0;
            //    filterEdit1.loadData();
            //    modalPopupFilterEdit.Show();
            //}
        }

        private void DataBindSelected()
        {
            if (ViewState["SelectedGroups_List"] != null)
            {
                List<GroupObject> list = (List<GroupObject>)ViewState["SelectedGroups_List"];
                gvSelectedGroups.DataSource = list;
                gvSelectedGroups.DataBind();

            }
        }

        private void DataBindSuppression()
        {
            if (ViewState["SupressionGroups_List"] != null)
            {
                List<GroupObject> list = (List<GroupObject>)ViewState["SupressionGroups_List"];
                gvSupression.DataSource = list;
                gvSupression.DataBind();
            }
        }
        private void blastLicenseCount_Update()
        {
            if (IsSelect)
            {
                List<GroupObject> dtSelected = getSelectedGroups();
                StringBuilder xmlGroups = new StringBuilder();
                xmlGroups.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xmlGroups.Append("<NoBlast>");

                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                if (dtSelected != null)
                {
                    foreach (GroupObject dr in dtSelected)
                    {
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
                    }
                    List<GroupObject> dtSuppression = getSuppressionGroups();

                    if (dtSuppression != null)
                    {
                        xmlGroups.Append("<SuppressionGroup>");
                        foreach (GroupObject dr in dtSuppression)
                        {
                            xmlGroups.Append("<Group id=\"" + dr.GroupID + "\">");
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in dr.filters.Where(x => x.SmartSegmentID != null))
                            {

                                xmlGroups.Append("<SmartSegmentID id=\"" + cibf.SmartSegmentID + "\">");
                                xmlGroups.Append("<RefBlastIDs>" + cibf.RefBlastIDs + "</RefBlastIDs></SmartSegmentID>");

                            }

                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in dr.filters.Where(x => x.FilterID != null))
                            {
                                xmlGroups.Append("<FilterID id=\"" + cibf.FilterID + "\" />");
                            }
                            xmlGroups.Append("</Group>");
                        }
                        xmlGroups.Append("</SuppressionGroup>");
                    }

                    xmlGroups.Append("</NoBlast>");

                    if (dtSelected.Count > 0)
                    {
                        DataTable dt = ECN_Framework_BusinessLayer.Communicator.Blast.GetEstimatedSendsCount(xmlGroups.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, ci.IgnoreSuppression.HasValue ? ci.IgnoreSuppression.Value : false);
                        BlastThis.Text = dt.Rows[0][0].ToString();
                    }
                    else
                    {
                        BlastThis.Text = "0";
                    }
                }
                else
                {
                    BlastThis.Text = "0";
                }
                upMain.Update();
            }
        }

        public void setSelectedGroup(int selectedGroupID, int? filterID, string refBlastList, int cibfID)
        {
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(selectedGroupID);
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
                    dr.filters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
                    if (filterID != null && filterID > 0)
                    {
                        ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                        cibf.CampaignItemBlastFilterID = cibfID;
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
                        if (filterID != null && filterID > 0 && go.filters.Count(x => x.FilterID == filterID) == 0 && go.filters.Count(x => x.SmartSegmentID == filterID) == 0)
                        {
                            //remove group object from the original list so we can replace it
                            dt.Remove(go);
                            ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                            cibf.CampaignItemBlastFilterID = cibfID;
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
                if (ViewState["SelectedGroups_List"] != null)
                    ViewState["SelectedGroups_List"] = dt;
                else
                    ViewState.Add("SelectedGroups_List", dt);

                DataBindSelected();
            }
        }

        public void setSuppressionGroup(int selectedGroupID, int? filterID, string refBlastList, int cibfID)
        {

            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(selectedGroupID);
            List<GroupObject> dt;
            if (ViewState["SupressionGroups_List"] != null)
            {
                dt = (List<GroupObject>)ViewState["SupressionGroups_List"];
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
                    dr.filters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
                    if (filterID != null && filterID > 0)
                    {
                        ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                        cibf.CampaignItemBlastFilterID = cibfID;
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
                            cibf.CampaignItemBlastFilterID = cibfID;
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
                if (ViewState["SupressionGroups_List"] != null)
                    ViewState["SupressionGroups_List"] = dt;
                else
                    ViewState.Add("SupressionGroups_List", dt);
                DataBindSuppression();
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
            DoTwemoji();
        }

        protected void gvSupression_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals(ImportSubs, StringComparison.OrdinalIgnoreCase))
            {
                SuppressCommandImportSubs(e);
            }
            else if (e.CommandName.Equals(AddSubs, StringComparison.OrdinalIgnoreCase))
            {
                SuppressCommandAddSubs(e);
            }
            else if (e.CommandName.Equals(RemoveGroup, StringComparison.OrdinalIgnoreCase))
            {
                SuppressCommandRemoveGroup(e);
            }
            else if (e.CommandName.Equals(AddFilter))
            {
                SuppressCommandAddFilter(e);
            }
            else if (e.CommandName.Equals(EditCustomFilter, StringComparison.OrdinalIgnoreCase))
            {
                SuppressCommandEditCustomFilter(e);
            }
            else if (e.CommandName.Equals(DeleteFilter, StringComparison.OrdinalIgnoreCase))
            {
                DeleteFilterCommand(e, SupressionGroupsListKey);
            }
            else if (e.CommandName.Equals(DeleteCustomFilter, StringComparison.OrdinalIgnoreCase))
            {
                DeleteCustomFilterCommand(e, SupressionGroupsListKey);
            }

            DoTwemoji();
        }

        private void SuppressCommandEditCustomFilter(CommandEventArgs e)
        {
            var filterId = Convert.ToInt32(e.CommandArgument.ToString());

            if (filterId <= FilterId6)
            {
                return;
            }

            var filter = GetByFilterID(filterId, ECNSession.CurrentSession().CurrentUser);
            filterEdit1.selectedGroupID = filter.GroupID.Value;
            filterEdit1.selectedFilterID = filterId;
            filterEdit1.loadData();
            hfShowSelect.Value = ShowSelectZero;
            modalPopupFilterEdit.Show();
        }

        private void SuppressCommandAddFilter(CommandEventArgs e)
        {
            var gridViewRow = gvSupression.Rows[Convert.ToInt32(e.CommandArgument.ToString())];
            var labelGroupId = (Label)gridViewRow.FindControl(LabelGroupId);
            var groupId = Convert.ToInt32(labelGroupId.Text);

            imgbtnCreateFilter.CommandArgument = gridViewRow.RowIndex.ToString();
            btnFilterEdit_Close.CommandArgument = gridViewRow.RowIndex.ToString();
            btnFilterEdit_Close.CommandName = Suppress;
            SuppressOrSelect = Suppress;
            hfShowSelect.Value = ShowSelectOne;
            ResetAddFilter(Suppress, groupId, true, true);
        }

        private void SuppressCommandRemoveGroup(CommandEventArgs e)
        {
            var removeGroupId = Convert.ToInt32(e.CommandArgument.ToString());
            var groupObjects = (List<GroupObject>)ViewState[SupressionGroupsListKey];
            foreach (var groupObject in groupObjects)
            {
                if (groupObject.GroupID != removeGroupId)
                {
                    continue;
                }

                groupObjects.Remove(groupObject);
                break;
            }

            ViewState[SupressionGroupsListKey] = groupObjects;
            if (!groupObjects.Any())
            {
                lblEmptyGrid_Supression.Visible = true;
                gvSupression.Visible = false;
            }

            gvSupression.DataSource = groupObjects;
            gvSupression.DataBind();
            blastLicenseCount_Update();
        }

        private void SuppressCommandAddSubs(CommandEventArgs e)
        {
            var selectedGroupId = Convert.ToInt32(e.CommandArgument.ToString());
            SelectedGroupID.Value = selectedGroupId.ToString();
            addSubscribers1.GroupID = selectedGroupId;
            addSubscribers1.loadDropDowns(
                ECNSession.CurrentSession()
                    .CurrentUser.CustomerID);
            modalPopupAddSubscribers.Show();
        }

        private void SuppressCommandImportSubs(CommandEventArgs e)
        {
            var selectedGroupId = Convert.ToInt32(e.CommandArgument.ToString());
            SelectedGroupID.Value = selectedGroupId.ToString();
            importSubscribers1.GroupID = selectedGroupId;
            modalPopupImport.Show();
        }

        protected void gvSelectedGroups_Command(object sender, GridViewCommandEventArgs e)
        {
            var lowerCommandName = e.CommandName.ToLower();

            if (e.CommandName.Equals(EditCustomFilter))
            {
                EditCustomFilterCommand(e);
            }
            else if (e.CommandName.Equals(AddFilter))
            {
                AddFilterCommand(e);
            }
            else if (lowerCommandName.Equals(CreateCustomFilter))
            {
                CreateCustomFilterCommand(e);
            }
            else if (lowerCommandName.Equals(DeleteFilter))
            {
                DeleteFilterCommand(e, SelectedGroupsListKey);
            }
            else if (lowerCommandName.Equals(DeleteCustomFilter))
            {
                DeleteCustomFilterCommand(e, SelectedGroupsListKey);
            }
            else if (e.CommandName.Equals(RemoveGroup))
            {
                RemoveGroupCommand(e);
            }
            else if (e.CommandName.Equals(ImportSubs))
            {
                ImportSubsCommand(e);
            }
            else if (e.CommandName.Equals(AddSubs))
            {
                AddSubsCommand(e);
            }

            DoTwemoji();
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
            var groupObjects = (List<GroupObject>)ViewState[SelectedGroupsListKey];
            foreach (var groupObject in groupObjects)
            {
                if (groupObject.GroupID.ToString().Equals(selectedGroupId.ToString()))
                {
                    groupObjects.Remove(groupObject);
                    break;
                }
            }

            ViewState[SelectedGroupsListKey] = groupObjects;
            if (groupObjects.Count == 0)
            {
                lblEmptyGrid_Selected.Visible = true;
                gvSelectedGroups.Visible = false;
            }

            gvSelectedGroups.DataSource = groupObjects;
            gvSelectedGroups.DataBind();
            blastLicenseCount_Update();
        }

        private void DeleteCustomFilterCommand(CommandEventArgs eventArgs, string viewStateKey)
        {
            DeleteFilterCommandWithSelectFunction(eventArgs, filter => filter.FilterID, viewStateKey);
        }

        private void DeleteFilterCommand(CommandEventArgs eventArgs, string viewStateKey)
        {
            DeleteFilterCommandWithSelectFunction(eventArgs, filter => filter.SmartSegmentID, viewStateKey);
        }

        private void DeleteFilterCommandWithSelectFunction(CommandEventArgs eventArgs, Func<CampaignItemBlastFilter, int?> selectFunction, string viewStateKey)
        {
            var split = eventArgs.CommandArgument.ToString().Split(UnderscoreSeparator);
            var listGroups = (List<GroupObject>)ViewState[viewStateKey];
            var filterId = Convert.ToInt32(split[0]);
            var groupId = Convert.ToInt32(split[1]);

            var currentGroup = listGroups.Find(x => x.GroupID == groupId);
            var currentIndex = listGroups.IndexOf(currentGroup);
            listGroups.Remove(currentGroup);
            var campaignItemBlastFilter = currentGroup?.filters.Find(x => selectFunction(x) == filterId);
            if (campaignItemBlastFilter != null)
            {
                currentGroup.filters.Remove(campaignItemBlastFilter);
            }

            listGroups.Insert(currentIndex, currentGroup);

            ViewState[viewStateKey] = listGroups;
            DataBindSelected();
            blastLicenseCount_Update();
        }

        private void CreateCustomFilterCommand(CommandEventArgs eventArgs)
        {
            var gridViewRows = gvSelectedGroups.Rows[Convert.ToInt32(eventArgs.CommandArgument.ToString())];
            var labelGroupId = (Label)gridViewRows.FindControl(LabelGroupId);
            var groupId = Convert.ToInt32(labelGroupId.Text);
            var filtergrid = (filtergrid)gridViewRows.FindControl(UcFiltergrid);
            filtergrid.SuppressOrSelect = Select;
            filterEdit1.selectedGroupID = groupId;
            filterEdit1.selectedFilterID = 0;
            filterEdit1.loadData();
            hfShowSelect.Value = ShowSelectOne;
            modalPopupFilterEdit.Show();
        }

        private void AddFilterCommand(CommandEventArgs eventArgs)
        {
            var gridViewRows = gvSelectedGroups.Rows[Convert.ToInt32(eventArgs.CommandArgument.ToString())];
            var labelGroupId = (Label)gridViewRows.FindControl(LabelGroupId);
            var groupId = Convert.ToInt32(labelGroupId.Text);

            imgbtnCreateFilter.CommandArgument = gridViewRows.RowIndex.ToString();
            btnFilterEdit_Close.CommandArgument = gridViewRows.RowIndex.ToString();
            btnFilterEdit_Close.CommandName = Select;
            hfShowSelect.Value = ShowSelectOne;
            SuppressOrSelect = Select;
            ResetAddFilter(Select, groupId, true, true);
        }

        private void EditCustomFilterCommand(CommandEventArgs eventArgs)
        {
            var filterId = Convert.ToInt32(eventArgs.CommandArgument.ToString());

            if (filterId > FilterId6)
            {
                var filter = GetByFilterID(
                    filterId,
                    ECNSession.CurrentSession().CurrentUser);

                filterEdit1.selectedGroupID = filter.GroupID.Value;
                filterEdit1.selectedFilterID = filterId;
                filterEdit1.loadData();
                hfShowSelect.Value = ShowSelectZero;
                modalPopupFilterEdit.Show();
            }
        }

        protected void FilterEdit_Hide(object sender, EventArgs e)
        {
            filterEdit1.reset();
            Button closeButton = (Button)sender;
            if (closeButton.CommandName.ToLower().Equals("select"))
            {
                GridViewRow gvr = gvSelectedGroups.Rows[Convert.ToInt32(closeButton.CommandArgument.ToString())];
                Label lblGroupID = (Label)gvr.FindControl("lblGroupID");
                int groupID = Convert.ToInt32(lblGroupID.Text);
                filtergrid fg = (filtergrid)gvr.FindControl("ucFilterGrid");
                bool showSelect = false;
                if (hfShowSelect.Value.Equals("1"))
                    showSelect = true;
                ResetAddFilter("select", groupID, showSelect, true, "false");

            }
            else if (closeButton.CommandName.ToLower().Equals("suppress"))
            {
                GridViewRow gvr = gvSupression.Rows[Convert.ToInt32(closeButton.CommandArgument.ToString())];
                Label lblGroupID = (Label)gvr.FindControl("lblGroupID");
                int groupID = Convert.ToInt32(lblGroupID.Text);
                filtergrid fg = (filtergrid)gvr.FindControl("fgSuppressionFilterGrid");
                bool showSelect = false;
                if (hfShowSelect.Value.Equals("1"))
                    showSelect = true;
                ResetAddFilter("suppress", groupID, showSelect, true, "false");

            }
            this.modalPopupFilterEdit.Hide();
            DoTwemoji();
        }

        protected void ImportGroup_Hide(object sender, EventArgs e)
        {
            importSubscribers1.reset();
            this.modalPopupImport.Hide();
            blastLicenseCount_Update();
            DoTwemoji();
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
            DoTwemoji();
        }

        protected void AddSubscribers_Close(object sender, EventArgs e)
        {
            addSubscribers1.Reset();
            this.modalPopupAddSubscribers.Hide();
            DoTwemoji();
        }

        protected void AddGroup_Save(object sender, EventArgs e)
        {
            if (addGroup1.Save())
            {
                addGroup1.Reset();
                this.modalPopupAddGroup.Hide();
            }
            DoTwemoji();
        }

        protected void AddGroup_Close(object sender, EventArgs e)
        {
            addGroup1.Reset();
            this.modalPopupAddGroup.Hide();
            DoTwemoji();
        }

        protected void gvSelectedGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GroupObject rowView = (GroupObject)e.Row.DataItem;

                int groupID = Convert.ToInt32(gvSelectedGroups.DataKeys[e.Row.RowIndex].Value.ToString());
                //List<ECN_Framework_Entities.Communicator.Filter> filterList =
                //ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID(groupID, true, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
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

        protected void btndownload_Click(object sender, System.EventArgs e)
        {
            DataTable emailGroupView = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByUserID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID);

            string filePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID + "/downloads/");
            string fileName = string.Format("subscribercounts_{0}_{1}_{2}_{3}.xml", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, DateTime.Now.Second);


            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fileDestination = Path.Combine(filePath, fileName);
            XmlTextWriter xmlWriter = null;
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(fileDestination, FileMode.OpenOrCreate, FileAccess.Write);
                xmlWriter = new XmlTextWriter(fileStream, System.Text.Encoding.Unicode);

                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteStartDocument(true);
                xmlWriter.WriteStartElement("Lists");

                string oFolderID = string.Empty;
                int count = 0;
                foreach (DataRow emailGroup in emailGroupView.AsEnumerable())
                {
                    if (oFolderID != emailGroup["FolderID"].ToString())
                    {
                        if (oFolderID != string.Empty)
                            xmlWriter.WriteEndElement();

                        oFolderID = emailGroup["FolderID"].ToString();
                        xmlWriter.WriteStartElement("Folder");
                        xmlWriter.WriteAttributeString("name", emailGroup["FolderName"].ToString());
                    }

                    xmlWriter.WriteStartElement("Group");
                    xmlWriter.WriteAttributeString("name", emailGroup["GroupName"].ToString());
                    xmlWriter.WriteRaw(emailGroup["Subscribers"].ToString());
                    xmlWriter.WriteEndElement();

                    if (count == emailGroupView.Rows.Count - 1)
                    {
                        xmlWriter.WriteEndElement();
                    }
                    count++;
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
            catch
            {
            }
            finally
            {
                if (xmlWriter != null)
                {
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

            Response.ContentType = "application/xml";
            Response.AddHeader("content-disposition", "attachment; filename=subscribercount.xml");
            Response.WriteFile(Path.Combine(filePath, fileName));
            Response.Flush();
            Response.End();

            DoTwemoji();
        }

        protected void GroupsGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteGroup(Convert.ToInt32(GroupsGrid.DataKeys[e.RowIndex].Values[0]));
            DoTwemoji();
        }

        protected void GroupsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink hlGroupEditor = (System.Web.UI.WebControls.HyperLink)e.Row.FindControl("hlGroupEditor");

                if (string.Equals(SearchTypeDR.SelectedValue, "Group", StringComparison.OrdinalIgnoreCase))
                {
                    hlGroupEditor.NavigateUrl = "~/main/lists/groupeditor.aspx?GroupID=" + GroupsGrid.DataKeys[e.Row.RowIndex].Values[0].ToString();
                }
                else
                {
                    hlGroupEditor.NavigateUrl = "~/main/lists/groupeditor.aspx?GroupID=" + GroupsGrid.DataKeys[e.Row.RowIndex].Values[0].ToString() + "&Comparator=" + SearchGrpsDR.SelectedValue + "&Value=" + Server.UrlEncode(SearchCriteria.Text);
                }
                DataRowView drGroup = (DataRowView)e.Row.DataItem;

                Label lblFolder = (Label)e.Row.FindControl("lblFolderName");
                lblFolder.Text = drGroup["FolderName"].ToString();
                CheckBox chkArchive = (CheckBox)e.Row.FindControl("chkIsArchived");
                chkArchive.Attributes.Add("index", e.Row.RowIndex.ToString());

                chkArchive.Checked = drGroup["Archived"].ToString().ToLower().Equals("true") ? true : false;
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


        public List<GroupObject> getSuppressionGroups()
        {
            List<GroupObject> retList = new List<GroupObject>();
            if (ViewState["SupressionGroups_List"] != null)
                return (List<GroupObject>)ViewState["SupressionGroups_List"];
            else
                return retList;

        }



        public void setDataFromCampaignItem(ECN_Framework_Entities.Communicator.CampaignItem ci)
        {
            CampaignItemID = ci.CampaignItemID;
            LoadGroupFolder();
            loadGroupsGrid(0);
            loadLicense();
            ViewState["SelectedGroups_List"] = null;
            ViewState["SupressionGroups_List"] = null;
            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ci.BlastList)
            {
                List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> listCIBF = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemBlastID(ciBlast.CampaignItemBlastID);
                if (listCIBF.Count > 0)
                {

                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in listCIBF)
                    {
                        string refBlastIDs = "";
                        if (cibf.SmartSegmentID != null)
                        {
                            refBlastIDs = cibf.RefBlastIDs;
                            setSelectedGroup(ciBlast.GroupID.Value, cibf.SmartSegmentID.Value, refBlastIDs, cibf.CampaignItemBlastFilterID);
                        }
                        else if (cibf.FilterID != null)
                        {
                            setSelectedGroup(ciBlast.GroupID.Value, cibf.FilterID.Value, "", cibf.CampaignItemBlastFilterID);
                        }

                    }


                }
                else
                    setSelectedGroup(ciBlast.GroupID.Value, null, null, 0);
            }
            foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppression in ci.SuppressionList)
            {
                if (ciSuppression.Filters.Count > 0)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciSuppression.Filters.Where(x => x.FilterID != null))
                    {

                        setSuppressionGroup(ciSuppression.GroupID.Value, cibf.FilterID.HasValue ? cibf.FilterID : null, "", cibf.CampaignItemBlastFilterID);
                    }
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciSuppression.Filters.Where(x => x.SmartSegmentID != null))
                    {
                        setSuppressionGroup(ciSuppression.GroupID.Value, cibf.SmartSegmentID.Value, cibf.RefBlastIDs, cibf.CampaignItemBlastFilterID);
                    }
                }
                else
                {
                    setSuppressionGroup(ciSuppression.GroupID.Value, null, "", 0);
                }
            }
            blastLicenseCount_Update();



        }

        protected void gvSupression_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GroupObject rowView = (GroupObject)e.Row.DataItem;

                int groupID = Convert.ToInt32(gvSupression.DataKeys[e.Row.RowIndex].Value.ToString());
                List<ECN_Framework_Entities.Communicator.Filter> filterList =
                GetByGroupID_NoAccessCheck(groupID, true);

                ImageButton imgbtnAddSubs = (ImageButton)e.Row.FindControl("imgbtnAddSubs");
                imgbtnAddSubs.CommandArgument = groupID.ToString();
                ImageButton imgbtnImportSubs = (ImageButton)e.Row.FindControl("lbImportSubs");
                imgbtnImportSubs.CommandArgument = groupID.ToString();

                ecn.communicator.main.ECNWizard.Group.filtergrid filterGrid = (ecn.communicator.main.ECNWizard.Group.filtergrid)e.Row.FindControl("fgSuppressionFilterGrid");
                filterGrid.GroupID = groupID;
                filterGrid.RowIndex = e.Row.RowIndex;
                filterGrid.SetFilters(rowView, e.Row.RowIndex);
            }
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

            if (suppressorselect.Equals("select"))
            {
                dt = (List<GroupObject>)ViewState["SelectedGroups_List"];
            }
            else if (suppressorselect.Equals("suppress"))
            {
                dt = (List<GroupObject>)ViewState["SupressionGroups_List"];
            }
            else if (suppressorselect.Equals("testselect"))
            {
                dt = (List<GroupObject>)ViewState["SelectedTestGroups_List"];
            }
            pnlCreateFilterButton.Visible = KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.Edit);

            List<ECN_Framework_Entities.Communicator.Filter> filterList =
GetByGroupID_NoAccessCheck(groupID, true, "active");
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
                        ECN_Framework_BusinessLayer.Communicator.Blast.GetBySearch(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, "", null, groupID, false, "", null, null, null, "", "", ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                    if (blastList != null)
                    {
                        var result = (from src in blastList
                                      orderby src.BlastID descending
                                      select new
                                      {
                                          BlastID = src.BlastID,
                                          EmailSubject = "[" + src.BlastID.ToString() + "] " + ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(src.EmailSubject)
                                      }).ToList();
                        lstboxBlast.DataSource = result;
                        lstboxBlast.DataTextField = "EmailSubject";
                        lstboxBlast.DataValueField = "BlastID";
                        lstboxBlast.DataBind();
                    }

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

        private string CheckAndConvertUnicode(string dynamicSubject)
        {
            Regex UnicodeSplit = new Regex("([\\\\][u][a-zA-Z0-9]{4}[\\\\][u][a-zA-Z0-9]{4})", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = UnicodeSplit.Matches(dynamicSubject);
            if (matches != null && matches.Count > 0)
            {

                Encoding enc = Encoding.UTF8;
                foreach (Match m in matches)
                {
                    string uniValue = m.Groups[1].Value;
                    uniValue = uniValue.Replace("\\u", "");
                    char[] chars = new char[2];
                    int charIndex = 0;
                    for (int i = 0; i < uniValue.Length; i += 4)
                    {
                        chars[charIndex] = (char)Int16.Parse(uniValue.Substring(i, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                        charIndex++;
                    }

                    string hex = "";
                    if (!char.IsSurrogatePair(chars[0], chars[1]))
                    {


                        hex = StringFunctions.ToUnicode(uniValue.Substring(0, 4) + "-" + uniValue.Substring(4));

                    }
                    else
                    {

                        hex = char.ConvertFromUtf32(Char.ConvertToUtf32(chars[0], chars[1]));
                    }

                    dynamicSubject = dynamicSubject.Replace(m.Value, hex.ToString());
                }

            }
            return dynamicSubject;
        }
        
        protected void btnSaveFilter_Click(object sender, EventArgs e)
        {
            lblRefBlastError.Visible = false;
            List<GroupObject> dt = new List<GroupObject>();
            if (SuppressOrSelect.ToLower().Equals("select"))
            {
                if (ViewState["SelectedGroups_List"] != null)
                {

                    dt = (List<GroupObject>)ViewState["SelectedGroups_List"];
                }
            }
            else if (SuppressOrSelect.ToLower().Equals("suppress"))
            {
                if (ViewState["SupressionGroups_List"] != null)
                {
                    dt = (List<GroupObject>)ViewState["SupressionGroups_List"];
                }
            }
            else if (SuppressOrSelect.ToLower().Equals("testselect"))
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
                    if (SuppressOrSelect.ToLower().Equals("select"))
                    {
                        if (ViewState["SelectedGroups_List"] != null)
                            ViewState["SelectedGroups_List"] = dt;
                        else
                            ViewState.Add("SelectedGroups_List", dt);
                        DataBindSelected();
                    }
                    else if (SuppressOrSelect.ToLower().Equals("suppress"))
                    {
                        if (ViewState["SupressionGroups_List"] != null)
                            ViewState["SupressionGroups_List"] = dt;
                        else
                            ViewState.Add("SupressionGroups_List", dt);

                        DataBindSuppression();
                    }


                    mpeAddFilter.Hide();
                }
                else
                {
                    mpeAddFilter.Hide();
                }

                blastLicenseCount_Update();
            }
            DoTwemoji();
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
            DoTwemoji();
        }

        protected void btnCancelFilter_Click(object sender, EventArgs e)
        {
            mpeAddFilter.Hide();
            DoTwemoji();
        }

        protected void imgbtnCreateFilter_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ImageButton imgbtnCreateFil = (ImageButton)sender;

            if (SuppressOrSelect.Equals("select"))
            {

                GridViewRow gvr = gvSelectedGroups.Rows[Convert.ToInt32(imgbtnCreateFil.CommandArgument.ToString())];
                Label lblGroupID = (Label)gvr.FindControl("lblGroupID");
                int GroupID = Convert.ToInt32(lblGroupID.Text);
                filtergrid fg = (filtergrid)gvr.FindControl("ucFilterGrid");
                fg.SuppressOrSelect = "select";
                filterEdit1.selectedGroupID = GroupID;
                filterEdit1.selectedFilterID = 0;
                filterEdit1.loadData();
                hfShowSelect.Value = "1";
                modalPopupFilterEdit.Show();
                mpeAddFilter.Hide();
            }
            else if (SuppressOrSelect.Equals("suppress"))
            {
                GridViewRow gvr = gvSupression.Rows[Convert.ToInt32(imgbtnCreateFil.CommandArgument.ToString())];
                Label lblGroupID = (Label)gvr.FindControl("lblGroupID");
                int GroupID = Convert.ToInt32(lblGroupID.Text);
                filtergrid fg = (filtergrid)gvr.FindControl("fgSuppressionFilterGrid");
                fg.SuppressOrSelect = "suppress";
                filterEdit1.selectedGroupID = GroupID;
                filterEdit1.selectedFilterID = 0;
                filterEdit1.loadData();
                hfShowSelect.Value = "1";
                modalPopupFilterEdit.Show();
                mpeAddFilter.Hide();
            }
            DoTwemoji();
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            GroupGridPageIndex = 0;
            loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
            DoTwemoji();
        }

        protected void chkIsArchived_CheckedChanged(object sender, EventArgs e)
        {
            bool initialState = false;
            CheckBox chkArchive = (CheckBox)sender;
            try
            {

                initialState = !chkArchive.Checked;
                int index = Convert.ToInt32(chkArchive.Attributes["index"].ToString());

                int datakey = Convert.ToInt32(GroupsGrid.DataKeys[index].Value.ToString());

                ECN_Framework_BusinessLayer.Communicator.Group.Archive(datakey, chkArchive.Checked, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                loadGroupsGrid(Convert.ToInt32(GroupFolder.SelectedFolderID));
            }
            catch (ECNException ecn)
            {
                chkArchive.Checked = initialState;
                setECNError(ecn);
            }
            DoTwemoji();
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
            DoTwemoji();
        }




    }

    [Serializable]
    public class GroupObject
    {

        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> filters { get; set; }

    }
}