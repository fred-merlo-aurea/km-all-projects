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
    public partial class Pub_CustomPage : System.Web.UI.Page
    {
        JFSession jfsess = new JFSession();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    BoxPanel2.Title = "Manage Pages for " + Request.QueryString["PubName"] + ":";
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void grdPubCustom_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "CustomEdit")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblPubCustomID = row.FindControl("lblPubCustomID") as Label;
                    hfldPubCustomId.Value = lblPubCustomID.Text;
                    txtPageName.Text = row.Cells[1].Text;
                    Label lblPubCustomPageHTML = row.FindControl("lblPubCustomPageHTML") as Label;

                    RadEditorPageHTML.Content = lblPubCustomPageHTML.Text;

                    Label lblIsActive = (Label)row.FindControl("lblIsActive");
                    rbtlstIsActive.SelectedValue = (lblIsActive.Text.ToUpper() == "YES" ? "1" : "0");

                    BoxPanel1.Title = "Edit Custom Page";
                    btnAdd.Text = "SAVE";
                }
                else if (e.CommandName == "CustomDelete")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblPubCustomID = row.FindControl("lblPubCustomID") as Label;
                    hfldPubCustomId.Value = lblPubCustomID.Text;
                    SqlDataSourcePCustomConnect.SelectParameters["PCPID"].DefaultValue = hfldPubCustomId.Value;
                    SqlDataSourcePCustomConnect.SelectParameters["iMod"].DefaultValue = "3";
                    SqlDataSourcePCustomConnect.Select(DataSourceSelectArguments.Empty);

                    ClearData();
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
                if (hfldPubCustomId.Value == "")
                {
                    //SqlDataSourcePCustomConnect.SelectParameters["PageHTML"].DefaultValue = CKeditorPageHTML.Text;
                    SqlDataSourcePCustomConnect.SelectParameters["AddedBy"].DefaultValue = jfsess.UserName();
                    SqlDataSourcePCustomConnect.SelectParameters["iMod"].DefaultValue = "1";
                    SqlDataSourcePCustomConnect.Select(DataSourceSelectArguments.Empty);
                }
                else
                {
                    SqlDataSourcePCustomConnect.SelectParameters["PCPID"].DefaultValue = hfldPubCustomId.Value;
                    //SqlDataSourcePCustomConnect.SelectParameters["PageHTML"].DefaultValue = CKeditorPageHTML.Text;
                    SqlDataSourcePCustomConnect.SelectParameters["ModifiedBy"].DefaultValue = jfsess.UserName();
                    SqlDataSourcePCustomConnect.SelectParameters["iMod"].DefaultValue = "2";
                    SqlDataSourcePCustomConnect.Select(DataSourceSelectArguments.Empty);
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
            hfldPubCustomId.Value = "";
            txtPageName.Text = "";

            RadEditorPageHTML.Content = "";
            rbtlstIsActive.SelectedIndex = 1;

            SqlDataSourcePCustomConnect.SelectParameters["PCPID"].DefaultValue = "0";
            SqlDataSourcePCustomConnect.SelectParameters["iMod"].DefaultValue = "4";

            BoxPanel1.Title = "Add Custom Page";
            btnAdd.Text = "SAVE";
        }        
    }
}
