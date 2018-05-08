using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KMPS.MD.Objects;
using System.Net.Mail;
using System.Linq;
using System.Collections.Specialized;

namespace KMPS.MDAdmin
{
    public partial class MasterGroups : KMPS.MD.Main.WebPageHelper
    {
        private string SortField
        {
            get
            {
                return ViewState["SortField"].ToString();
            }
            set
            {
                ViewState["SortField"] = value;
            }
        }

        private string SortDirection
        {
            get
            {
                return ViewState["SortDirection"].ToString();
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            Master.Menu = "Master Groups";
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                SortField = "SortOrder";
                SortDirection = "ASC";
                LoadGrid();
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void LoadGrid()
        {
            List<KMPS.MD.Objects.MasterGroup> mg = KMPS.MD.Objects.MasterGroup.GetAll(Master.clientconnections);

            List<KMPS.MD.Objects.MasterGroup> lst = null;

            if (mg != null && mg.Count > 0)
            {
                switch (SortField.ToUpper())
                {
                    case "DISPLAYNAME":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = mg.OrderBy(o => o.DisplayName).ToList();
                        else
                            lst = mg.OrderByDescending(o => o.DisplayName).ToList();
                        break;

                    case "NAME":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = mg.OrderBy(o => o.Name).ToList();
                        else
                            lst = mg.OrderByDescending(o => o.Name).ToList();
                        break;
                    case "ISACTIVE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = mg.OrderBy(o => o.IsActive).ToList();
                        else
                            lst = mg.OrderByDescending(o => o.IsActive).ToList();
                        break;
                    case "ENABLESUBREPORTING":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = mg.OrderBy(o => o.EnableSubReporting).ToList();
                        else
                            lst = mg.OrderByDescending(o => o.EnableSubReporting).ToList();
                        break;
                    case "ENABLESEARCHING":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = mg.OrderBy(o => o.EnableSearching).ToList();
                        else
                            lst = mg.OrderByDescending(o => o.EnableSearching).ToList();
                        break;
                    case "ENABLEADHOCSEARCH":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = mg.OrderBy(o => o.EnableAdhocSearch).ToList();
                        else
                            lst = mg.OrderByDescending(o => o.EnableAdhocSearch).ToList();
                        break;
                    case "SORTORDER":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = mg.OrderBy(o => o.SortOrder).ToList();
                        else
                            lst = mg.OrderByDescending(o => o.SortOrder).ToList();
                        break;
                }
            }

            if (lst != null)
            {
                if (txtSearch.Text != string.Empty)
                {
                    lst = lst.FindAll(x => x.Name.ToLower().Contains(txtSearch.Text.ToLower()) || (x.DisplayName.ToLower().Contains(txtSearch.Text.ToLower())));
                }
            }

            gvMasterGroups.DataSource = lst;
            gvMasterGroups.DataBind();
        }

        protected void gvMasterGroups_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == SortField)
            {
                SortDirection = (SortDirection.ToUpper() == "ASC" ? "DESC" : "ASC");
            }
            else
            {
                SortField = e.SortExpression;
                SortDirection = "ASC";
            }

            LoadGrid();
        }

        protected void gvMasterGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMasterGroups.PageIndex = e.NewPageIndex;
            LoadGrid();
        }

        protected void gvMasterGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    NameValueCollection nvc = MasterGroup.ValidationForDeleteorInActive(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));

                    if (nvc.Count > 0)
                    {
                        string errorMsg = "The selected master group is being used in the following scheduled exports, filters or Download Templates and cannot be successfully deleted until all prior uses are previously deleted.</br></br>";

                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new {key = k, value = v});
                        foreach (var item in items)
                        {
                            errorMsg += item.key + " " + item.value + "</br>";
                        }

                        DisplayError(errorMsg);
                        return;
                    }

                    KMPS.MD.Objects.MasterGroup.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
                    LoadGrid();
                }
                catch (Exception ex)
                {
                    DisplayError(ex.ToString());
                }
            }
        }

        protected void gvMasterGroups_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            ResetControls();
            lblMasterGroup.Text = "Edit MasterGroup";
            MasterGroup mg = MasterGroup.GetByID(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
            hfMasterGroupID.Value = mg.MasterGroupID.ToString();
            txtDisplayName.Text = mg.DisplayName;
            txtName.Text = mg.Name;
            ddlActive.SelectedValue = mg.IsActive.ToString();
            ddlSubReporting.SelectedValue = mg.EnableSubReporting.ToString();
            ddlSearching.SelectedValue = mg.EnableSearching.ToString();
            ddlAdhocSearch.SelectedValue = mg.EnableAdhocSearch.ToString();
            hfSortOrder.Value = mg.SortOrder.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(ddlActive.SelectedValue) == false && Convert.ToInt32(hfMasterGroupID.Value) > 0)
                {
                    NameValueCollection nvc = MasterGroup.ValidationForDeleteorInActive(Master.clientconnections, Convert.ToInt32(hfMasterGroupID.Value));

                    if (nvc.Count > 0)
                    {
                        string errorMsg = "The selected master group is being used in the following scheduled exports, filters  or Download Templates and cannot be updated as inactive until all prior uses are previously deleted.</br></br>";

                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                        foreach (var item in items)
                        {
                            errorMsg += item.key + " " + item.value + "</br>";
                        }

                        DisplayError(errorMsg);
                        return;
                    }
                }

                if (MasterGroup.ExistsByIDDisplayName(Master.clientconnections, Convert.ToInt32(hfMasterGroupID.Value), txtDisplayName.Text) != 0)
                {
                    DisplayError("Display Name already exists.");
                    return;
                }

                if (MasterGroup.ExistsByIDName(Master.clientconnections, Convert.ToInt32(hfMasterGroupID.Value), txtName.Text) != 0)
                {
                    DisplayError("Name already exists.");
                    return;
                }

                if (KMPS.MD.Objects.Subscriber.ExistsStandardFieldName(Master.clientconnections, txtDisplayName.Text))
                {
                    DisplayError("Display Name cannot be the same as a Standard Field name. Please enter a different name.");
                    return;
                }

                if (SubscriptionsExtensionMapper.ExistsByName(Master.clientconnections, txtDisplayName.Text))
                {
                    DisplayError("Display Name cannot be the same as a Adhoc name. Please enter a different name.");
                    return;
                }

                MasterGroup mg = new MasterGroup();
                mg.MasterGroupID = Convert.ToInt32(hfMasterGroupID.Value);
                mg.DisplayName = txtDisplayName.Text;
                mg.Name = txtName.Text;
                mg.IsActive = Convert.ToBoolean(ddlActive.SelectedValue); 
                mg.EnableSubReporting = Convert.ToBoolean(ddlSubReporting.SelectedValue); 
                mg.EnableSearching = Convert.ToBoolean(ddlSearching.SelectedValue); 
                mg.EnableAdhocSearch = Convert.ToBoolean(ddlAdhocSearch.SelectedValue);
                mg.SortOrder = Convert.ToInt32(hfSortOrder.Value);
                if(mg.MasterGroupID > 0)
                {
                    mg.DateUpdated = DateTime.Now;
                    mg.UpdatedByUserID = Master.LoggedInUser;
                }
                else
                {
                    mg.DateCreated = DateTime.Now;
                    mg.CreatedByUserID = Master.LoggedInUser;
                }

                MasterGroup.Save(Master.clientconnections, mg);

                Response.Redirect("MasterGroups.aspx");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("MasterGroups.aspx");
        }

        private void ResetControls()
        {
            lblMasterGroup.Text = "Add MasterGroup";
            txtDisplayName.Text = string.Empty;
            txtName.Text = string.Empty;
            ddlActive.SelectedIndex = -1;
            ddlSubReporting.SelectedIndex = -1;
            ddlSearching.SelectedIndex = -1;
            ddlAdhocSearch.SelectedIndex = -1;
            hfSortOrder.Value = "0";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadGrid();
        }
    }
}