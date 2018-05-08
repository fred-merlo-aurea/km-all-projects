using KMPS.MD.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

namespace KMPS.MD.Administration
{
    public partial class AdhocAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Adhoc";
            Master.SubMenu = "Adhoc Admin";
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                LoadAdhocGrid();
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void LoadAdhocGrid()
        {
            List<SubscriptionsExtensionMapper> extensionMappers = SubscriptionsExtensionMapper.GetAll(Master.clientconnections);
            gvAdhoc.DataSource = extensionMappers;
            gvAdhoc.DataBind();
        }

        protected void gvAdhoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    GridView grdAdhoc = (GridView)e.Row.FindControl("grdAdhoc");
            //    grdAdhoc.DataSource = Adhoc.GetByCategoryID(Convert.ToInt32(gvAdhoc.DataKeys[e.Row.RowIndex].Value.ToString()));
            //    grdAdhoc.DataBind();
            //}
        }

        protected void gvAdhoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int mapperId = Convert.ToInt32(gvAdhoc.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);

                try
                {
                    int subscriptionsExtensionMapperId = Convert.ToInt32(gvAdhoc.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                    SubscriptionsExtensionMapper extensionMapper = SubscriptionsExtensionMapper.GetByID(Master.clientconnections, subscriptionsExtensionMapperId);

                    lblMapperId.Text = subscriptionsExtensionMapperId.ToString();
                    txtName.Text = extensionMapper.CustomField;
                    ddlDataType.SelectedValue = extensionMapper.CustomFieldDataType;
                    chkActive.Checked = extensionMapper.Active;

                    btnSave.Text = "UPDATE";
                    lblpnlHeader.Text = "Edit Adhoc";
                }
                catch (Exception ex)
                {
                    DisplayError(ex.ToString());
                }
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    NameValueCollection nvc = SubscriptionsExtensionMapper.ValidationForDeleteorInActive(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));

                    if (nvc.Count > 0)
                    {
                        string errorMsg = "The selected adhoc is being used in the following scheduled exports or filters and cannot be successfully deleted until all prior uses are previously deleted.</br></br>";

                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                        foreach (var item in items)
                        {
                            errorMsg += item.key + " " + item.value + "</br>";
                        }

                        DisplayError(errorMsg);
                        return;
                    }

                    KMPS.MD.Objects.SubscriptionsExtensionMapper.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
                    LoadAdhocGrid();
                }
                catch (Exception ex)
                {
                    DisplayError(ex.ToString());
                }
            }
        }

        protected void gvAdhoc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdhoc.PageIndex = e.NewPageIndex;
            LoadAdhocGrid();
        }

        protected void gvAdhoc_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkActive.Checked && Convert.ToInt32(lblMapperId.Text) > 0)
                {
                    NameValueCollection nvc = SubscriptionsExtensionMapper.ValidationForDeleteorInActive(Master.clientconnections, Convert.ToInt32(lblMapperId.Text));

                    if (nvc.Count > 0)
                    {
                        string errorMsg = "The selected adhoc is being used in the following scheduled exports or filters and cannot be updated as inactive until all prior uses are previously deleted.</br></br>";

                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                        foreach (var item in items)
                        {
                            errorMsg += item.key + " " + item.value + "</br>";
                        }

                        DisplayError(errorMsg);
                        return;
                    }
                }

                if (SubscriptionsExtensionMapper.ExistsByIDName(Master.clientconnections, Convert.ToInt32(lblMapperId.Text),txtName.Text))
                {
                    DisplayError(" Adhoc name already exists.");
                    return;
                }

                if (KMPS.MD.Objects.Subscriber.ExistsStandardFieldName(Master.clientconnections, txtName.Text))
                {
                    DisplayError("Adhoc name cannot be the same as a Standard Field name. Please enter a different name.");
                    return;
                }

                if (MasterGroup.ExistsByName(Master.clientconnections, txtName.Text) != 0)
                {
                    DisplayError("Adhoc name cannot be the same as a Master Group. Please enter a different name.");
                    return;
                }

                SubscriptionsExtensionMapper.Save(Master.clientconnections, Convert.ToInt32(lblMapperId.Text), txtName.Text, ddlDataType.SelectedValue, chkActive.Checked);
            }
            catch (Exception ex)
            {
                DisplayError(ex.ToString());
            }

            Response.Redirect("AdhocAdmin.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdhocAdmin.aspx");
        }
    }
}