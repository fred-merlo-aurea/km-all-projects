using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace KMPS_JF_Setup.Publisher
{
    public partial class Pub_Categories : System.Web.UI.Page
    {
        JFSession jfsess = new JFSession();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    BoxPanel2.Title = "Manage Catagories for " + Request.QueryString["PubName"] + ":";
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void grdPublisherCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "CategoryEdit")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblCategoryID = row.FindControl("lblCategoryID") as Label;
                    hfldCategoryID.Value = lblCategoryID.Text;
                    txtCategoryName.Text = row.Cells[1].Text;
                    txtDescription.Text = row.Cells[2].Text;

                    Label lblCategoryType = (Label) row.FindControl("lblCategoryType");
                    ddlCategoryType.SelectedIndex = ddlCategoryType.Items.IndexOf(ddlCategoryType.Items.FindByText(lblCategoryType.Text));
                    BoxPanel1.Title = "Edit Category";
                    btnAdd.Text = "SAVE";

                }
                else if (e.CommandName == "CategoryDelete")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblCategoryID = row.FindControl("lblCategoryID") as Label;
                    hfldCategoryID.Value = lblCategoryID.Text;
                    SqlDataSourcePCategoryConnect.SelectParameters["CategoryID"].DefaultValue = hfldCategoryID.Value;
                    SqlDataSourcePCategoryConnect.SelectParameters["iMod"].DefaultValue = "3";
                    SqlDataSourcePCategoryConnect.Select(DataSourceSelectArguments.Empty);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (hfldCategoryID.Value == "")
                {
                    SqlDataSourcePCategoryConnect.SelectParameters["AddedBy"].DefaultValue = jfsess.UserName();
                    SqlDataSourcePCategoryConnect.SelectParameters["iMod"].DefaultValue = "1";
                    SqlDataSourcePCategoryConnect.Select(DataSourceSelectArguments.Empty);
                }
                else
                {
                    SqlDataSourcePCategoryConnect.SelectParameters["CategoryID"].DefaultValue = hfldCategoryID.Value;
                    SqlDataSourcePCategoryConnect.SelectParameters["ModifiedBy"].DefaultValue = jfsess.UserName();
                    SqlDataSourcePCategoryConnect.SelectParameters["iMod"].DefaultValue = "2";
                    SqlDataSourcePCategoryConnect.Select(DataSourceSelectArguments.Empty);
                }
                ClearData();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void ClearData()
        {
            hfldCategoryID.Value = "";
            txtCategoryName.Text = "";
            txtDescription.Text = "";
            ddlCategoryType.SelectedIndex = 0;

            SqlDataSourcePCategoryConnect.SelectParameters["CategoryID"].DefaultValue = "0";
            SqlDataSourcePCategoryConnect.SelectParameters["iMod"].DefaultValue = "4";

            BoxPanel1.Title = "Add Category";
            btnAdd.Text = "SAVE";
        }
    }
}
