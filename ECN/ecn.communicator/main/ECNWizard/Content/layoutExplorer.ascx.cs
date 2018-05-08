using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard.Content
{
    public partial class layoutExplorer : System.Web.UI.UserControl
    {

        int userID = 0;
        int customerID = 0;
        int LayoutRecordCount = 0;

        int LayoutsGridPageIndex = 0;
        private int _lytFolderId
        {
            get
            {
                if (ViewState["_lytFolderId"] != null)
                    return (int)ViewState["_lytFolderId"];
                else
                    return 0;
            }
            set
            {
                ViewState["_lytFolderId"] = value;
            }
        }
        private bool IsSelect
        {
            get
            {
                if (ViewState["IsSelect"] != null)
                    return (bool)ViewState["IsSelect"];
                else
                    return false;
            }
            set
            {
                ViewState["IsSelect"] = value;
            }
        }
        public int selectedLayoutID
        {
            get
            {
                if (ViewState["selectedLayoutID"] != null)
                    return (int)ViewState["selectedLayoutID"];
                else
                    return 0;
            }
            set
            {
                ViewState["selectedLayoutID"] = value;
                if(value != 0)
                {
                    ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(value, false);
                    if (layout != null)
                        lblSelectedLayoutID.Text = layout.LayoutName;
                    else
                    {
                        ECNError error = new ECNError(Enums.Entity.Layout, Enums.Method.Get, "Message no longer exists");
                        List<ECNError> listError = new List<ECNError>();
                        listError.Add(error);
                        ECNException exc = new ECNException(listError);
                        setECNError(exc);
                    }
                }
                else
                {
                    lblSelectedLayoutID.Text = "None";
                }
            }
        }

        public void enableSelectMode()
        {
            IsSelect = true;
            LayoutsGrid.Columns[5].Visible = false;
            LayoutsGrid.Columns[6].Visible = false;
            LayoutsGrid.Columns[9].Visible = false;
            LayoutsGrid.Columns[10].Visible = false;
            LayoutsGrid.Columns[11].Visible = true;
            pnlSelectedLayout.Visible = true;
        }

        public void enableEditMode()
        {
            IsSelect = false;
            LayoutsGrid.Columns[5].Visible = true;
            LayoutsGrid.Columns[6].Visible = true;
            LayoutsGrid.Columns[9].Visible = true;
            LayoutsGrid.Columns[10].Visible = true;
            LayoutsGrid.Columns[11].Visible = false;
            pnlSelectedLayout.Visible = false;
        }

        public void enableShowArchivedOnlyMode()
        {
            IsSelect = true;
            ddlArchiveFilter.SelectedValue = "active";
            ddlArchiveFilter.Visible = false;
            LayoutsGrid.Columns[12].Visible = false;
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            EcnErrorHelper.SetEcnError(phError, lblErrorMessage, ecnException);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            LayoutsGrid.PageIndexChanging += new GridViewPageEventHandler(LayoutsGrid_PageIndexChanging);
            LayoutsGrid.Sorting += new GridViewSortEventHandler(LayoutsGrid_Sorting);
            LayoutsGrid.RowDeleting += new GridViewDeleteEventHandler(LayoutsGrid_RowDeleting);
            LayoutFolderID.FolderEvent += new EventHandler(LayoutFolderEvent);
            userID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            customerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID;

            if (!Page.IsPostBack)
            {

                LoadUserDD("%", "%");
                loadLayoutFoldersDD(customerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString());
                checkECNFeatures();
            }
            else
            {
                string contentuserID = "%";
                string layoutuserID = "%";
                try
                {
                    layoutuserID = LayoutUserID.SelectedItem.Value.ToString();
                    LoadUserDD(contentuserID, layoutuserID);

                }
                catch (System.NullReferenceException ne)
                {
                    string devnul = ne.ToString();
                    LoadUserDD("%", "%");
                }
            }

        }

        private void checkECNFeatures()
        {
            KMPlatform.Entity.User user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            if (!IsSelect)
            {
                LayoutsGrid.Columns[5].Visible = KMPlatform.BusinessLogic.Client.HasServiceFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ROIReporting);
                LayoutsGrid.Columns[6].Visible = KM.Platform.User.HasServiceFeature(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner);
                LayoutsGrid.Columns[9].Visible = KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit);
                LayoutsGrid.Columns[10].Visible = KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Delete);
            }
        }

        public void reset()
        {
            userID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            customerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            LayoutSearchCriteria.Text = "";
            cbxAllFoldersLayout.Checked = false;
            ViewState["lSortField"] = null;
            ViewState["lSortDirection"] = null;
            ViewState["layoutGridPageIndex"] = null;
            
            _lytFolderId = 0;
            
            LayoutUserID.SelectedIndex = 0;
            ddlPageSizeContent.SelectedValue = "15";
            LayoutsGrid.PageSize = 15;
            LoadUserDD("%", "%");
            loadFolders();
            loadLayoutsGrid(customerID, _lytFolderId);
        }

        public void loadFolders()
        {
            loadLayoutFoldersDD(customerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString());
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {

            if (!string.IsNullOrEmpty(LayoutFolderID.SelectedFolderID))
            {
                _lytFolderId = Convert.ToInt32(LayoutFolderID.SelectedFolderID.ToString());
            }
            if (cbxAllFoldersLayout.Checked)
            {
                LayoutsGrid.Columns[0].Visible = true;
            }
            else
                LayoutsGrid.Columns[0].Visible = false;

            KMPlatform.Entity.User user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            if(KM.Platform.User.IsAdministrator(user))
            {
                if (IsSelect)
                {
                    ddlArchiveFilter.SelectedValue = "active";
                    ddlArchiveFilter.Visible = false;
                    LayoutsGrid.Columns[12].Visible = false;
                }
                else
                {
                    LayoutsGrid.Columns[12].Visible = true;
                    ddlArchiveFilter.Visible = true;
                }
            }
            else
            {
                LayoutsGrid.Columns[12].Visible = false;
            }
            
            
            
        }

        #region Data Load
        private void LoadUserDD(string contentusermatch, string layoutusermatch)
        {
            List<KMPlatform.Entity.User> userList = KMPlatform.BusinessLogic.User.GetByCustomerID(Convert.ToInt32(customerID));

            if(!userList.Exists(x => x.UserID == ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID))
            {
                userList.Add(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }

            var result = (from src in userList
                          where src.IsActive
                          orderby src.UserName
                          select src).ToList();
            LayoutUserID.ClearSelection();
            LayoutUserID.Items.Clear();
            LayoutUserID.SelectedValue = null;
            LayoutUserID.DataSource = result;
            LayoutUserID.DataBind();
            LayoutUserID.Items.Insert(0, new ListItem("All", "*"));



            if ((contentusermatch.Equals("%")) && (layoutusermatch.Equals("%")))
            {
                List<ECN_Framework_Entities.Communicator.UserGroup> userGroupList = ECN_Framework_BusinessLayer.Communicator.UserGroup.Get(Convert.ToInt32(userID));
                bool bFilterExists = userGroupList.Count > 0 ? true : false;
                if (bFilterExists)
                {
                    LayoutUserID.Items.FindByValue(userID.ToString()).Selected = true;
                    LayoutUserID.Enabled = false;
                }
                else
                {
                    LayoutUserID.SelectedValue = "*";
                }
            }
            else
            {
                LayoutUserID.Items.FindByValue(layoutusermatch).Selected = true;
            }
            if (LayoutUserID.Items.Count == 1)
            {
                LayoutUserID.Visible = false;
            }
        }

        private void loadLayoutsGrid(int CustomerID, int folderID)
        {
            int? userID = null;
            if (!LayoutUserID.SelectedItem.Value.Equals("*"))
                userID = Convert.ToInt32(LayoutUserID.SelectedItem.Value);

            var layoutView = new DataSet();

            if (ViewState["lSortField"] == null || ViewState["lSortDirection"] == null)
            {
                ViewState["lSortField"] = "UpdatedDate";
                ViewState["lSortDirection"] = "DESC";
            }

            var sortColumn = ViewState["lSortField"].ToString();
            var sortDirection = ViewState["lSortDirection"].ToString();

            if (ViewState["layoutGridPageIndex"] != null)
            {
                LayoutsGridPageIndex = int.Parse(ViewState["layoutGridPageIndex"].ToString());

                if (IsPageOutOfBounds(LayoutsGrid.PageSize, LayoutsGridPageIndex))
                {
                    LayoutsGridPageIndex = int.Parse(lblTotalRecords.Text) / LayoutsGrid.PageSize;
                    ViewState["layoutGridPageIndex"] = LayoutsGridPageIndex;
                }
            }
            int ValidatedOnly = 1;
            if (!IsSelect)
            {
                ValidatedOnly = 0;
            }
            if (cbxAllFoldersLayout.Checked)
            {
                layoutView = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutName(LayoutSearchCriteria.Text.Trim().ToUpper(), null, userID, ValidatedOnly, null, null, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, LayoutsGridPageIndex + 1, LayoutsGrid.PageSize, sortDirection, sortColumn,ddlArchiveFilter.SelectedValue.ToString());
            }
            else
            {
                layoutView = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutName(LayoutSearchCriteria.Text.Trim().ToUpper(), folderID, userID, ValidatedOnly, null, null, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, LayoutsGridPageIndex + 1, LayoutsGrid.PageSize, sortDirection, sortColumn, ddlArchiveFilter.SelectedValue.ToString());
            }

           

            if (layoutView.Tables.Count > 0 && layoutView.Tables[0].Rows.Count > 0)
            {
                LayoutRecordCount = int.Parse(layoutView.Tables[0].Rows[0].ItemArray[1].ToString());
                DataView dvlayout = layoutView.Tables[0].DefaultView;
                dvlayout.Sort = ViewState["lSortField"].ToString() + ' ' + ViewState["lSortDirection"];

                var tblLayout = dvlayout.ToTable();
                

                LayoutsGrid.DataSource = tblLayout;
            }


            if (!ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ROIReporting))
            {
                LayoutsGrid.Columns[3].Visible = false;
            }

            try
            {
                LayoutsGrid.DataBind();
            }
            catch
            {
                LayoutsGrid.PageIndex = 0;
                LayoutsGrid.DataBind();
            }
            LayoutsGrid.ShowEmptyTable = true;
            LayoutsGrid.EmptyTableRowText = "No Messages to display";

            lblTotalRecords.Text = LayoutRecordCount.ToString();

            var exactPageCount = (double)LayoutRecordCount / (double)LayoutsGrid.PageSize;
            var rountUpPageCount = Math.Ceiling((double)exactPageCount);

            lblTotalNumberOfPagesGroup.Text = rountUpPageCount.ToString();
            txtGoToPageContent.Text = (LayoutsGridPageIndex + 1) <= 1 ? "1" : (LayoutsGridPageIndex + 1).ToString();

            pnlPager.Visible = true;

            ViewState["layoutGridPageCount"] = lblTotalNumberOfPagesGroup.Text;
            ddlPageSizeContent.SelectedValue = LayoutsGrid.PageSize.ToString();

            pnlMessage.Update();
        }

        public bool IsPageOutOfBounds(int pageSize, int gridPageIndex)
        {
            if (int.Parse(lblTotalRecords.Text) <= (pageSize * gridPageIndex))
            {
                return true;
            }
            return false;
        }

        private void loadLayoutFoldersDD(int CustomerID, string selectedFolderType)
        {
            LayoutFolderID.ID = "LayoutFolderID";
            LayoutFolderID.CustomerID = Convert.ToInt32(CustomerID);
            LayoutFolderID.FolderType = selectedFolderType;
            LayoutFolderID.LoadFolderTree();
        }
        #endregion

        #region LayoutEvents
        public void DeleteLayout(int theLayoutID)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Layout.Delete(theLayoutID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }

        }

        public void LayoutsGrid_sortCommand(Object sender, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["lSortField"].ToString())
            {
                switch (ViewState["lSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["lSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["lSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["lSortField"] = e.SortExpression;
                ViewState["lSortDirection"] = "ASC";
            }
            loadLayoutsGrid(customerID, _lytFolderId);
        }

        private void LayoutsGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int count = Convert.ToInt32(e.Item.Cells[8].Text);
                LinkButton deleteBtn = e.Item.FindControl("DeleteLayoutBtn") as LinkButton;

                if (count > 0)
                {
                    deleteBtn.Attributes.Add("style", "cursor:hand;padding:0;margin:0;");
                    deleteBtn.Attributes.Add("onclick", "return confirm('Message titled \"" + e.Item.Cells[0].Text.ToString().Replace("'", "") + "\" is currently being used in " + count + " Blast[s]." + "\\n" + "The Reference to this Message will be lost on any associated Blast." + "\\n" + "\\n" + "Press OK to continue Deleting this Message');");
                }
                else
                {
                    deleteBtn.Attributes.Add("onclick", "return confirm('Are you sure that you want to delete the Message titled \"" + e.Item.Cells[0].Text.ToString().Replace("'", "") + "\" ?')");
                }
                return;
            }
        }

        public void LayoutsGrid_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteLayout"))
            {
                DeleteLayout(Convert.ToInt32(e.CommandArgument.ToString()));
                loadLayoutsGrid(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, Convert.ToInt32(LayoutFolderID.SelectedFolderID));
            }
            else if (e.CommandName.Equals("SelectLayout"))
            {
                selectedLayoutID = Convert.ToInt32(e.CommandArgument.ToString());
                //the set for selectedLayoutID will take care of the assigning of the selected layout name to the label
                ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(selectedLayoutID, false);
                if (layout != null)
                {
                    
                    RaiseBubbleEvent(layout, null);
                }
                
            }
            pnlMessage.Update();

        }

        private void LayoutFolderEvent(object sender, EventArgs e)
        {
            cbxAllFoldersLayout.Checked = false;
            TreeView tn = (TreeView)sender;
            _lytFolderId = Convert.ToInt32(tn.SelectedNode.Value);
            LayoutsGrid.PageIndex = 0;
            ViewState["layoutGridPageIndex"] = "0";
            loadLayoutsGrid(customerID, Convert.ToInt32(tn.SelectedNode.Value));
        }

        protected void LayoutClearButton_Click(object sender, EventArgs e)
        {
            cbxAllFoldersLayout.Checked = false;
            LayoutSearchCriteria.Text = "";
            foreach (ListItem item in LayoutUserID.Items)
            {
                if (item.Selected == true)
                {
                    item.Selected = false;
                    break;
                }
            }

            LayoutUserID.Items.FindByValue("*").Selected = true;
            loadLayoutFoldersDD(customerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString());
        }

        protected void LayoutsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.LayoutsGrid.PageSize = int.Parse(dropDown.SelectedValue);
            loadLayoutsGrid(customerID, Convert.ToInt32(LayoutFolderID.SelectedFolderID));
        }

        protected void LayoutsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                LayoutsGrid.PageIndex = e.NewPageIndex;
            }
            LayoutsGrid.DataBind();

        }

        protected void GoToPageLayout_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageLayout = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageLayout.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.LayoutsGrid.PageCount)
            {
                this.LayoutsGrid.PageIndex = pageNumber - 1;
            }
            else
            {
                this.LayoutsGrid.PageIndex = 0;
            }
            loadLayoutsGrid(customerID, Convert.ToInt32(LayoutFolderID.SelectedFolderID));
        }

        protected void LayoutsGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["lSortField"].ToString())
            {
                switch (ViewState["lSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["lSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["lSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["lSortField"] = e.SortExpression;
                ViewState["lSortDirection"] = "ASC";
            }
            loadLayoutsGrid(customerID, _lytFolderId);
        }

        protected void LayoutsGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteLayout(Convert.ToInt32(LayoutsGrid.DataKeys[e.RowIndex].Values[0]));
        }

        protected void GoToPageContent_TextChanged(object sender, EventArgs e)
        {
            var txtGoToPageContent = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageContent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= int.Parse(ViewState["layoutGridPageCount"].ToString()))
            {
                LayoutsGridPageIndex = 1;
                ViewState["layoutGridPageIndex"] = LayoutsGridPageIndex = pageNumber - 1;
            }
            else
            {
                LayoutsGridPageIndex = 0;
                ViewState["layoutGridPageIndex"] = LayoutsGridPageIndex;
            }
            loadLayoutsGrid(customerID, Convert.ToInt32(LayoutFolderID.SelectedFolderID));
        }

        protected void btnPreviousGroup_Click(object sender, EventArgs e)
        {
            if (ViewState["layoutGridPageIndex"] != null)
            {
                LayoutsGridPageIndex = int.Parse(ViewState["layoutGridPageIndex"].ToString());
                if (LayoutsGridPageIndex > 0)
                {
                    ViewState["layoutGridPageIndex"] = --LayoutsGridPageIndex;
                }
                loadLayoutsGrid(customerID, Convert.ToInt32(LayoutFolderID.SelectedFolderID));
            }
        }

        protected void btnNextGroup_Click(object sender, EventArgs e)
        {
            if (ViewState["layoutGridPageIndex"] != null)
            {
                var maxPage = lblTotalNumberOfPagesGroup.Text;
                if ((int.Parse(ViewState["layoutGridPageIndex"].ToString()) + 1) < int.Parse(maxPage))
                {
                    LayoutsGridPageIndex = int.Parse(ViewState["layoutGridPageIndex"].ToString());
                    ViewState["layoutGridPageIndex"] = ++LayoutsGridPageIndex;
                }
            }
            else
            {
                LayoutsGridPageIndex = 1;
                ViewState["layoutGridPageIndex"] = LayoutsGridPageIndex;
            }
            loadLayoutsGrid(customerID, Convert.ToInt32(LayoutFolderID.SelectedFolderID));
        }

        #endregion

        protected void LayoutSearchButton_Click(object sender, EventArgs e)
        {
            loadLayoutsGrid(customerID, Convert.ToInt32(LayoutFolderID.SelectedFolderID));
        }

        protected void chkIsArchived_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkArchived = (CheckBox)sender;
            int index = Convert.ToInt32(chkArchived.Attributes["index"].ToString());
            int LayoutID = Convert.ToInt32(LayoutsGrid.DataKeys[index].Value.ToString());
            ECN_Framework_Entities.Communicator.Layout l = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(LayoutID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);            
            ECN_Framework_BusinessLayer.Communicator.Layout.Archive(l, chkArchived.Checked, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID);
            
            loadLayoutsGrid(customerID, Convert.ToInt32(LayoutFolderID.SelectedFolderID));
        }

        protected void LayoutsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                CheckBox chkArchived = (CheckBox)e.Row.FindControl("chkIsArchived");
                Label lblFolder = (Label)e.Row.FindControl("lblFolderName");
                lblFolder.Text = drv["FolderName"].ToString();
                chkArchived.Checked = drv["Archived"].ToString().ToLower().Equals("true") ? true : false;
                chkArchived.Attributes.Add("index", e.Row.RowIndex.ToString());
            }
        }
    }
}