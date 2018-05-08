using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using KMPS.MD.Objects;

namespace KMPS.MD.Administration
{
    public partial class PubCustomField : System.Web.UI.Page
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
            Master.Menu = "Products";
            Master.SubMenu = "Product Custom Field";
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                SortField = "CustomField";
                SortDirection = "ASC";

                rddlProducts.DataSource = Pubs.GetActive(Master.clientconnections) ;
                rddlProducts.DataBind();
                try
                {
                    rddlProducts.SelectedIndex = 0;
                }
                catch
                {
                }

                LoadCustomFieldGrid();
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void rddlProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomFieldGrid();
        }

        protected void LoadCustomFieldGrid()
        {
            List<PubSubscriptionsExtensionMapper> lem = PubSubscriptionsExtensionMapper.GetByPubID(Master.clientconnections, Convert.ToInt32(rddlProducts.SelectedValue));

            List<PubSubscriptionsExtensionMapper> lst = null;

            if (lem != null && lem.Count > 0)
            {
                switch (SortField.ToUpper())
                {
                    case "CUSTOMFIELD":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = lem.OrderBy(o => o.CustomField).ToList();
                        else
                            lst = lem.OrderByDescending(o => o.CustomField).ToList();
                        break;
                }
            }

            gvPubCustomFields.DataSource = lst;
            gvPubCustomFields.DataBind();
        }

        protected void gvPubCustomFields_Sorting(object sender, GridViewSortEventArgs e)
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

            LoadCustomFieldGrid();
        }

        protected void gvPubCustomFields_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void gvPubCustomFields_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int mapperId = Convert.ToInt32(gvPubCustomFields.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);

                try
                {
                    int subscriptionsExtensionMapperId = Convert.ToInt32(gvPubCustomFields.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                    PubSubscriptionsExtensionMapper extensionMapper = PubSubscriptionsExtensionMapper.GetByID(Master.clientconnections, subscriptionsExtensionMapperId);

                    hfMapperID.Value = subscriptionsExtensionMapperId.ToString();
                    txtName.Text = extensionMapper.CustomField;
                    ddlDataType.SelectedValue = extensionMapper.CustomFieldDataType;
                    chkActive.Checked = extensionMapper.Active;
                    hfCreatedDate.Value = extensionMapper.DateCreated.ToString();
                    hfCreatedUserID.Value = extensionMapper.CreatedByUserID.ToString();
                    btnSave.Text = "UPDATE";
                    lblpnlHeader.Text = "Edit Custom Fields";
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
                    string[] args = e.CommandArgument.ToString().Split('|');

                    NameValueCollection nvc = PubSubscriptionsExtensionMapper.ValidationForDeleteorInActive(Master.clientconnections, Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));

                    if (nvc.Count > 0)
                    {
                        string errorMsg = "The selected custom field is being used in the following scheduled exports or filters and cannot be successfully deleted until all prior uses are previously deleted.</br></br>";

                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                        foreach (var item in items)
                        {
                            errorMsg += item.key + " " + item.value + "</br>";
                        }

                        DisplayError(errorMsg);
                        return;
                    }

                    KMPS.MD.Objects.PubSubscriptionsExtensionMapper.Delete(Master.clientconnections, Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
                    LoadCustomFieldGrid();
                }
                catch (Exception ex)
                {
                    DisplayError(ex.ToString());
                }
            }
        }

        protected void gvPubCustomFields_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPubCustomFields.PageIndex = e.NewPageIndex;
            LoadCustomFieldGrid();
        }

        protected void gvPubCustomFields_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkActive.Checked && Convert.ToInt32(hfMapperID.Value) > 0)
                {
                    NameValueCollection nvc = PubSubscriptionsExtensionMapper.ValidationForDeleteorInActive(Master.clientconnections, Convert.ToInt32(hfMapperID.Value), Convert.ToInt32(rddlProducts.SelectedValue));

                    if (nvc.Count > 0)
                    {
                        string errorMsg = "The selected custom field is being used in the following scheduled exports or filters and cannot be updated as inactive until all prior uses are previously deleted.</br></br>";

                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                        foreach (var item in items)
                        {
                            errorMsg += item.key + " " + item.value + "</br>";
                        }

                        DisplayError(errorMsg);
                        return;
                    }
                }

                if (PubSubscriptionsExtensionMapper.ExistsByIDCustomField(Master.clientconnections, Convert.ToInt32(hfMapperID.Value), txtName.Text, Convert.ToInt32(rddlProducts.SelectedValue)))
                {
                    DisplayError("Custom Field Name already exists.");
                    return;
                }

                if (KMPS.MD.Objects.Subscriber.ExistsStandardFieldName(Master.clientconnections, txtName.Text))
                {
                    DisplayError("Custom Field name cannot be the same as a Standard Field name. Please enter a different name.");
                    return;
                }

                if (ResponseGroup.ExistsByName(Master.clientconnections, txtName.Text))
                {
                    DisplayError("Custom Field name cannot be the same as a Response Group. Please enter a different name.");
                    return;
                }

                PubSubscriptionsExtensionMapper m = new PubSubscriptionsExtensionMapper();
                m.PubSubscriptionsExtensionMapperId = Convert.ToInt32(hfMapperID.Value);
                m.PubID = Convert.ToInt32(rddlProducts.SelectedValue);
                m.CustomField = txtName.Text;
                m.CustomFieldDataType = ddlDataType.SelectedValue;
                m.Active = chkActive.Checked;

                if (m.PubSubscriptionsExtensionMapperId > 0)
                {
                    m.UpdatedByUserID = Master.LoggedInUser;
                    m.DateUpdated = DateTime.Now;
                    m.DateCreated = Convert.ToDateTime(hfCreatedDate.Value);
                    m.CreatedByUserID = Convert.ToInt32(hfCreatedUserID.Value);
                }
                else
                {
                    m.CreatedByUserID = Master.LoggedInUser;
                    m.DateCreated = DateTime.Now;
                    m.UpdatedByUserID = Master.LoggedInUser;
                    m.DateUpdated = DateTime.Now;
                }

                PubSubscriptionsExtensionMapper.Save(Master.clientconnections, m);

                LoadCustomFieldGrid();
                hfMapperID.Value = "0";
                txtName.Text = string.Empty;
                ddlDataType.SelectedIndex = -1;
                chkActive.Checked = false;
                hfCreatedDate.Value = string.Empty;
                hfCreatedUserID.Value = string.Empty;
            }
            catch (Exception ex)
            {
                DisplayError(ex.ToString());
                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("PubCustomField.aspx");
        }
    }
}