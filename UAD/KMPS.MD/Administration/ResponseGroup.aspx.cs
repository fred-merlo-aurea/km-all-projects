using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.SqlTypes;
using KMPS.MD.Objects;
using System.Collections.Specialized;

namespace KMPS.MDAdmin
{
    public partial class ResponseGroup : KMPS.MD.Main.WebPageHelper
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

        private int PubID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["PubID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        //public static string connStr = DataFunctions.GetClientSqlConnection(Master.clientconnections);
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Products";
            Master.SubMenu = "Response Groups";
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                gvGroup.PageSize = 10;
                SortField = "ResponseGroupName";
                SortDirection = "ASC";
                LoadPubs();

                try
                {
                    drpPubs.Items.FindByValue(PubID().ToString()).Selected = true;
                }
                catch
                {
                }

                drpResponseGroupType.DataSource = Code.GetResponseGroup();
                drpResponseGroupType.DataBind();
                drpResponseGroupType.Items.Insert(0, new ListItem("", ""));

                LoadGrid();               
            }
        }
        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void drpPubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }


        protected void LoadPubs()
        {
            drpPubs.DataSource = Pubs.GetActive(Master.clientconnections);
            drpPubs.DataBind();
        }

        protected void gvGroup_Sorting(object sender, GridViewSortEventArgs e)
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
        
        protected void LoadGrid()
        {
            var rg = KMPS.MD.Objects.ResponseGroup.GetAllByPubID(Master.clientconnections, Convert.ToInt32(drpPubs.SelectedItem.Value));

            List<KMPS.MD.Objects.ResponseGroup> lst = null;

            if (rg != null && rg.Count > 0)
            {
                switch (SortField.ToUpper())
                {
                    case "RESPONSEGROUPNAME":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = rg.OrderBy(o => o.ResponseGroupName).ToList();
                        else
                            lst = rg.OrderByDescending(o => o.ResponseGroupName).ToList();
                        break;

                    case "DISPLAYNAME":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = rg.OrderBy(o => o.DisplayName).ToList();
                        else
                            lst = rg.OrderByDescending(o => o.DisplayName).ToList();
                        break;

                    case "ISMULTIPLEVALUE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = rg.OrderBy(o => o.IsMultipleValue).ToList();
                        else
                            lst = rg.OrderByDescending(o => o.IsMultipleValue).ToList();
                        break;

                    case "ISREQUIRED":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = rg.OrderBy(o => o.IsRequired).ToList();
                        else
                            lst = rg.OrderByDescending(o => o.IsRequired).ToList();
                        break;

                    case "ISACTIVE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = rg.OrderBy(o => o.IsActive).ToList();
                        else
                            lst = rg.OrderByDescending(o => o.IsActive).ToList();
                        break;
                }
            }

            gvGroup.DataSource = lst;
            gvGroup.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!cbIsActive.Checked && Convert.ToInt32(hfResponseGroupID.Value) > 0)
            {
                NameValueCollection nvc = MD.Objects.ResponseGroup.ValidationForDeleteorInActive(Master.clientconnections, Convert.ToInt32(hfResponseGroupID.Value), Convert.ToInt32(drpPubs.SelectedValue));

                if (nvc.Count > 0)
                {
                    string errorMsg = "The selected response group is being used in the following scheduled exports, filters or Download Templates and cannot be updated as inactive until all prior uses are previously deleted.</br></br>";

                    var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                    foreach (var item in items)
                    {
                        errorMsg += item.key + " " + item.value + "</br>";
                    }

                    DisplayError(errorMsg);
                    return;
                }
            }

            if (KMPS.MD.Objects.ResponseGroup.ExistsByIDNamePubID(Master.clientconnections, Convert.ToInt32(drpPubs.SelectedItem.Value), txtResponseGroupName.Text.Trim(), Convert.ToInt32(hfResponseGroupID.Value)))
            {
                DisplayError("Group Name already exists or Group Name contains illegal spaces.");
                return;
            }

            if (KMPS.MD.Objects.Subscriber.ExistsStandardFieldName(Master.clientconnections, txtResponseGroupName.Text.Trim()))
            {
                DisplayError("Group Name cannot be same as Standard Field Name. Please enter a different name.");
                return;
            }

            if (KMPS.MD.Objects.Subscriber.ExistsStandardFieldName(Master.clientconnections, txtDisplayName.Text.Trim()))
            {
                DisplayError("Display Name cannot be same as Standard Field Name. Please enter a different dislay name.");
                return;
            }

            if (PubSubscriptionsExtensionMapper.ExistsCustomField(Master.clientconnections, txtResponseGroupName.Text.Trim()))
            {
                DisplayError("Group Name cannot be the same as a Pub Custom Field name. Please enter a different name.");
                return;
            }

            if (PubSubscriptionsExtensionMapper.ExistsCustomField(Master.clientconnections, txtDisplayName.Text.Trim()))
            {
                DisplayError("Display Name cannot be the same as a Pub Custom Field name. Please enter a different name.");
                return;
            }

            try
            {
                KMPS.MD.Objects.ResponseGroup rg = new KMPS.MD.Objects.ResponseGroup();
                rg.ResponseGroupID = Convert.ToInt32(hfResponseGroupID.Value);
                rg.PubID = Convert.ToInt32(drpPubs.SelectedValue);
                rg.ResponseGroupName = txtResponseGroupName.Text.Trim();
                rg.DisplayName = txtDisplayName.Text;
                rg.IsMultipleValue = cbIsMultipleValue.Checked;
                rg.IsRequired = cbIsRequired.Checked;
                rg.IsActive = cbIsActive.Checked;
                rg.ResponseGroupTypeID = Convert.ToInt32(drpResponseGroupType.SelectedValue);
                KMPS.MD.Objects.ResponseGroup.Save(Master.clientconnections, rg);

                if (btnSave.Text == "UPDATE")
                {
                    KMPS.MD.Objects.CodeSheet.Update(Master.clientconnections, Convert.ToInt32(hfResponseGroupID.Value), txtResponseGroupName.Text.Trim());
                }

                LoadGrid();
                ResetControls();
            }
            catch (Exception ex)
            {
                DisplayError(ex.ToString());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        protected void gvGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    ResetControls();

                    KMPS.MD.Objects.ResponseGroup rg = KMPS.MD.Objects.ResponseGroup.GetByResponseGroupID(Master.clientconnections, Convert.ToInt32(gvGroup.DataKeys[Convert.ToInt32(e.CommandArgument)].Value));

                    hfResponseGroupID.Value = rg.ResponseGroupID.ToString();
                    txtPubID.Text = rg.PubID.ToString();
                    txtResponseGroupName.Text = rg.ResponseGroupName;
                    txtDisplayName.Text = rg.DisplayName;
                    cbIsMultipleValue.Checked = rg.IsMultipleValue;
                    cbIsRequired.Checked = rg.IsRequired;
                    cbIsActive.Checked = rg.IsActive;
                    try
                    {
                        drpResponseGroupType.SelectedValue = rg.ResponseGroupTypeID.ToString();
                    }
                    catch
                    {
                    }
                    btnSave.Text = "UPDATE";
                    lblpnlHeader.Text = "Edit Response Group";

                }
                else if (e.CommandName == "Delete")
                {

                    NameValueCollection nvc = MD.Objects.ResponseGroup.ValidationForDeleteorInActive(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(drpPubs.SelectedValue));

                    if (nvc.Count > 0)
                    {
                        string errorMsg = "The selected response group is being used in the following scheduled exports, filters or Download Templates and cannot be successfully deleted until all prior uses are previously deleted.</br></br>";

                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                        foreach (var item in items)
                        {
                            errorMsg += item.key + " " + item.value + "</br>";
                        }

                        DisplayError(errorMsg);
                        return;
                    }

                    KMPS.MD.Objects.ResponseGroup.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(drpPubs.SelectedItem.Value));
                    LoadGrid();
                    ResetControls();
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.ToString());
            }
        }

        protected void gvGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
           
        }

        private void ResetControls()
        {
            txtResponseGroupName.Text = "";
            txtDisplayName.Text = "";
            cbIsMultipleValue.Checked = false;
            cbIsRequired.Checked = false;
            cbIsActive.Checked = true;
            drpResponseGroupType.ClearSelection();
            btnSave.Text = "SAVE";
            lblpnlHeader.Text = "Add Response Group";
        }
        protected void gvGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGroup.PageIndex = e.NewPageIndex;
            LoadGrid();
        }
    }
}
