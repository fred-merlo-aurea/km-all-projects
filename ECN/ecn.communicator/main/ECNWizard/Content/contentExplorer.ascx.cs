using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using ECN.Common.Helpers;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard.Content
{
    public partial class contentExplorer : System.Web.UI.UserControl
    {
        int userID = 0;
        int customerID = 0;
        int ContentRecordCount = 0;
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
        int contentGridPageIndex = 0;
        private bool IsSelect
        {
            get
            {
                if (ViewState["IsSelectContent"] != null)
                    return (bool)ViewState["IsSelectContent"];
                else
                    return false;
            }
            set
            {
                ViewState["IsSelectContent"] = value;
            }
        }

        public int selectedContentID
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(selectedContentID), 0); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(selectedContentID), value); }
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            EcnErrorHelper.SetEcnError(phError, lblErrorMessage, ecnException);
        }

        public void enableSelectMode()
        {
            IsSelect = true;
            ContentGrid.Columns[5].Visible = false;
            ContentGrid.Columns[6].Visible = false;
            ContentGrid.Columns[7].Visible = false;
            ContentGrid.Columns[10].Visible = false;
            ContentGrid.Columns[11].Visible = false;
            ContentGrid.Columns[12].Visible = true;
            pnlSelectedContent.Visible = true;
        }

        public void enableEditMode()
        {
            IsSelect = false;
            ContentGrid.Columns[5].Visible = true;
            ContentGrid.Columns[6].Visible = true;
            ContentGrid.Columns[7].Visible = true;
            ContentGrid.Columns[10].Visible = true;
            ContentGrid.Columns[11].Visible = true;
            ContentGrid.Columns[12].Visible = false;
            pnlSelectedContent.Visible = false;
        }

        public void Reset()
        {
            selectedContentID = 0;
            lblSelectedContentID.Text = "None";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ContentGrid.Sorting += new GridViewSortEventHandler(ContentGrid_Sorting);
            ContentGrid.RowDeleting += new GridViewDeleteEventHandler(ContentGrid_RowDeleting);
            ContentGrid.PageIndexChanging += new GridViewPageEventHandler(ContentGrid_PageIndexChanging);
            userID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            customerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            ContentFolderID.FolderEvent += new EventHandler(ContentFolderEvent);

            if (!Page.IsPostBack)
            {

                LoadUserDD("%", "%");
                loadContentFoldersDD(customerID);
                checkECNFeatures();
            }
            else
            {
                string contentuserID = "%";
                string layoutuserID = "%";
                try
                {
                    contentuserID = ContentUserID.SelectedItem.Value.ToString();
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
            if (!IsSelect)
            {
                KMPlatform.Entity.User user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
                ContentGrid.Columns[7].Visible = KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner, KMPlatform.Enums.Access.View);
                ContentGrid.Columns[10].Visible = KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit);
                ContentGrid.Columns[11].Visible = KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Delete);
            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            if (ShowArchiveFilter)
            {
                ddlArchiveFilter.Visible = true;
            }
            else
            {
                ddlArchiveFilter.SelectedValue = "active";
                ddlArchiveFilter.Visible = false;
            }

            int CntFolderID = 0;
            if (ContentFolderID.SelectedFolderID != null)
            {
                CntFolderID = Convert.ToInt32(ContentFolderID.SelectedFolderID.ToString());
            }
            loadContentGrid(customerID, CntFolderID);

            if(cbxAllFoldersContent.Checked)
            {
                ContentGrid.Columns[0].Visible = true;
            }
            else
                ContentGrid.Columns[0].Visible = false;

            if(IsSelect)
            {
                ddlArchiveFilter.SelectedValue = "active";
                ddlArchiveFilter.Visible = false;
            }
            //if(KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.IsAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
            {
                if(IsSelect)
                    ContentGrid.Columns[13].Visible = false;
                else
                    ContentGrid.Columns[13].Visible = true;

            }
            else
            {
                ContentGrid.Columns[13].Visible = false;
            }

            
        }

        private void LoadUserDD(string contentusermatch, string layoutusermatch)
        {
            List<KMPlatform.Entity.User> userList = KMPlatform.BusinessLogic.User.GetByCustomerID(Convert.ToInt32(customerID));
            var result = (from src in userList
                          where src.IsActive
                          orderby src.UserName
                          select src).ToList();
            ContentUserID.DataSource = result;
            ContentUserID.DataBind();
            ContentUserID.Items.Insert(0, new ListItem("All", "*"));

            if ((contentusermatch.Equals("%")) && (layoutusermatch.Equals("%")))
            {
                List<ECN_Framework_Entities.Communicator.UserGroup> userGroupList = ECN_Framework_BusinessLayer.Communicator.UserGroup.Get(Convert.ToInt32(userID));

                bool bFilterExists = userGroupList.Count > 0 ? true : false;

                if (bFilterExists)
                {
                    ContentUserID.Items.FindByValue(userID.ToString()).Selected = true;
                    ContentUserID.Enabled = false;
                }
                else
                {
                    ContentUserID.SelectedValue = "*";
                }
            }
            else
            {
                ContentUserID.Items.FindByValue(contentusermatch).Selected = true;
            }

            if (ContentUserID.Items.Count == 1)
            {
                ContentUserID.Visible = false;
            }
        }

        public void loadContentGrid(int CustomerID, int folderID)
        {
            int? userID = null;
            if (ContentUserID.SelectedItem != null)
            {
                if (ContentUserID.SelectedItem.Value != "*")
                {
                    userID = Convert.ToInt32(ContentUserID.SelectedItem.Value);
                }
            }

            var contentView = new DataSet();

            if (ViewState["cSortField"] == null || ViewState["cSortDirection"] == null)
            {
                ViewState["cSortField"] = "UpdatedDate";
                ViewState["cSortDirection"] = "DESC";
            }

            var sortColumn = ViewState["cSortField"].ToString();
            var sortDirection = ViewState["cSortDirection"].ToString();

            if (ViewState["contentGridPageIndex"] != null)
            {
                contentGridPageIndex = int.Parse(ViewState["contentGridPageIndex"].ToString());

                if (IsPageOutOfBounds(ContentGrid.PageSize, contentGridPageIndex))
                {
                    contentGridPageIndex = int.Parse(lblTotalRecords.Text) / ContentGrid.PageSize;
                    ViewState["contentGridPageIndex"] = contentGridPageIndex;
                }
            }
            int ValidatedOnly = 1;
            if (!IsSelect)
            {
                ValidatedOnly = 0;
            }
            if (cbxAllFoldersContent.Checked)
            {
                contentView = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentTitle(ContentSearchCriteria.Text.Trim().ToUpper(), null, ValidatedOnly, userID, null, null, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, contentGridPageIndex + 1, ContentGrid.PageSize, sortDirection, sortColumn,ddlArchiveFilter.SelectedValue.ToString());
            }
            else
            {

                contentView = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentTitle(ContentSearchCriteria.Text.Trim().ToUpper(), folderID, ValidatedOnly, userID, null, null, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, contentGridPageIndex + 1, ContentGrid.PageSize, sortDirection, sortColumn,ddlArchiveFilter.SelectedValue.ToString());
            }

            if (contentView.Tables.Count > 0 && contentView.Tables[0].Rows.Count > 0)
            {
                ContentRecordCount = int.Parse(contentView.Tables[0].Rows[0].ItemArray[1].ToString());
                DataView dvContent = contentView.Tables[0].DefaultView;

                dvContent.Sort = ViewState["cSortField"].ToString() + ' ' + ViewState["cSortDirection"].ToString();
                
                var tblContent = dvContent.ToTable();
                

                ContentGrid.DataSource = tblContent;
            }

            try
            {
                ContentGrid.DataBind();
            }
            catch
            {
                ContentGrid.PageIndex = 0;
                ContentGrid.DataBind();
            }
            ContentGrid.ShowEmptyTable = true;
            ContentGrid.EmptyTableRowText = "No content to display";

            lblTotalRecords.Text = ContentRecordCount.ToString();

            var exactPageCount = (double)ContentRecordCount / (double)ContentGrid.PageSize;
            var roundUpPageCount = Math.Ceiling((double)exactPageCount);

            lblTotalNumberOfPagesGroup.Text = roundUpPageCount.ToString();
            ViewState["contentGridPageCount"] = lblTotalNumberOfPagesGroup.Text;

            txtGoToPageContent.Text = (contentGridPageIndex + 1) <= 1 ? "1" : (contentGridPageIndex + 1).ToString();

            pnlPager.Visible = true;
            ddlPageSizeContent.SelectedValue = ContentGrid.PageSize.ToString();
        }

        public bool IsPageOutOfBounds(int pageSize, int gridPageIndex)
        {
            return int.Parse(lblTotalRecords.Text) <= (pageSize * gridPageIndex);
        }

        public void loadContentFoldersDD(int CustomerID)
        {
            ContentFolderID.ID = "ContentFolderID";
            ContentFolderID.CustomerID = Convert.ToInt32(CustomerID);
            ContentFolderID.FolderType = ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString(); ;
            ContentFolderID.LoadFolderTree();
        }

        #region ContentEvents
        public void DeleteContent(int theContentID)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Content.Delete(theContentID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        public void ContentGrid_sortCommand(Object sender, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["cSortField"].ToString())
            {
                switch (ViewState["cSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["cSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["cSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["cSortField"] = e.SortExpression;
                ViewState["cSortDirection"] = "ASC";
            }
        }

        public void ContentGrid_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteContent"))
            {
                DeleteContent(Convert.ToInt32(e.CommandArgument.ToString()));
            }
            else if (e.CommandName.Equals("SelectContent"))
            {
                selectedContentID = Convert.ToInt32(e.CommandArgument.ToString());
                ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(selectedContentID, false);
                lblSelectedContentID.Text = content.ContentTitle;
                RaiseBubbleEvent("ContentSelected", new EventArgs());
            }
        }



        protected void ContentClearButton_Click(object sender, EventArgs e)
        {
            cbxAllFoldersContent.Checked = false;
            ContentSearchCriteria.Text = "";
            foreach (ListItem item in ContentUserID.Items)
            {
                if (item.Selected == true)
                {
                    item.Selected = false;
                    break;
                }
            }
            ContentUserID.Items.FindByValue("*").Selected = true;
            loadContentFoldersDD(customerID);
        }

        private void ContentFolderEvent(object sender, EventArgs e)
        {
            TreeView tn = (TreeView)sender;
            cbxAllFoldersContent.Checked = false;
            ContentGrid.PageIndex = 0;
            ViewState["contentGridPageIndex"] = "0";
            loadContentGrid(customerID, Convert.ToInt32(tn.SelectedNode.Value));
        }

        protected void ContentGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.ContentGrid.PageSize = int.Parse(dropDown.SelectedValue);
        }

        protected void ContentGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                ContentGrid.PageIndex = e.NewPageIndex;
            }
            ContentGrid.DataBind();
        }

        protected void ContentGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var tmp = ViewState["cSortField"].ToString();
            if (e.SortExpression.ToString() == ViewState["cSortField"].ToString())
            {
                switch (ViewState["cSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["cSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["cSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["cSortField"] = e.SortExpression;
                ViewState["cSortDirection"] = "ASC";
            }
        }

        protected void ContentGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteContent(Convert.ToInt32(ContentGrid.DataKeys[e.RowIndex].Values[0]));
        }

        //New
        protected void GoToPageContent_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageContent = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageContent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= int.Parse(ViewState["contentGridPageCount"].ToString()))
            {
                contentGridPageIndex = 1;
                ViewState["contentGridPageIndex"] = contentGridPageIndex = pageNumber - 1;
            }
            else
            {
                contentGridPageIndex = 0;
                ViewState["contentGridPageIndex"] = contentGridPageIndex;
            }
        }

        protected void btnPreviousGroup_Click(object sender, EventArgs e)
        {
            if (ViewState["contentGridPageIndex"] != null)
            {
                contentGridPageIndex = int.Parse(ViewState["contentGridPageIndex"].ToString());
                if (contentGridPageIndex > 0)
                {
                    ViewState["contentGridPageIndex"] = --contentGridPageIndex;
                }
            }
        }

        protected void btnNextGroup_Click(object sender, EventArgs e)
        {

            if (ViewState["contentGridPageIndex"] != null)
            {
                var maxPage = lblTotalNumberOfPagesGroup.Text;
                if ((int.Parse(ViewState["contentGridPageIndex"].ToString()) + 1) < int.Parse(maxPage))
                {
                    contentGridPageIndex = int.Parse(ViewState["contentGridPageIndex"].ToString());
                    ViewState["contentGridPageIndex"] = ++contentGridPageIndex;
                }
            }
            else
            {
                contentGridPageIndex = 1;
                ViewState["contentGridPageIndex"] = contentGridPageIndex;
            }
        }

        protected void ContentSearchButton_Click(object sender, EventArgs e)
        {
            ContentGrid.PageIndex = 0;
            ViewState["contentGridPageIndex"] = "0";
            //loadContentGrid(customerID, Convert.ToInt32(tn.SelectedNode.Value));
        }

        protected void chkIsArchived_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkArchive = (CheckBox)sender;
            int index = Convert.ToInt32(chkArchive.Attributes["index"].ToString());

            int datakey = Convert.ToInt32(ContentGrid.DataKeys[index].Value.ToString());

            ECN_Framework_Entities.Communicator.Content g = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(datakey, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser,false);
            
            ECN_Framework_BusinessLayer.Communicator.Content.ArchiveContent(g, chkArchive.Checked, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID);
            

            loadContentGrid(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID,Convert.ToInt32(ContentFolderID.SelectedFolderID));
        }

        protected void chkIsValidated_CheckedChanged(object sender, EventArgs e)
        {
            phError.Visible = false;
            CheckBox chkValidate = (CheckBox)sender;
            int index = Convert.ToInt32(chkValidate.Attributes["vindex"].ToString());

            int datakey = Convert.ToInt32(ContentGrid.DataKeys[index].Value.ToString());
            try
            { 
                ECN_Framework_Entities.Communicator.Content g = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(datakey, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                if(ECN_Framework_BusinessLayer.Communicator.Content.ValidateHTMLContent(g.ContentSource))
                {
                    g.IsValidated = true;
                    ECN_Framework_BusinessLayer.Communicator.Content.Save(g,ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                }  
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }

            loadContentGrid(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, Convert.ToInt32(ContentFolderID.SelectedFolderID));
        }

        protected void ContentGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drGroup = (DataRowView)e.Row.DataItem;
                CheckBox chkArchive = (CheckBox)e.Row.FindControl("chkIsArchived");
                CheckBox chkValidate = (CheckBox)e.Row.FindControl("chkIsValidated");
                Label lblFolder = (Label)e.Row.FindControl("lblFolderName");
                lblFolder.Text = drGroup["FolderName"].ToString();
                chkArchive.Attributes.Add("index", e.Row.RowIndex.ToString());
                chkArchive.Checked = drGroup["Archived"].ToString().ToLower().Equals("true") ? true : false;
                chkValidate.Attributes.Add("vindex", e.Row.RowIndex.ToString());
                chkValidate.Checked = drGroup["Validated"].ToString().ToLower().Equals("true") ? true : false;
                chkValidate.Enabled = drGroup["Validated"].ToString().ToLower().Equals("true") ? false : true;
            }
        }
        //End

        #endregion
    }
}