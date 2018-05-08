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
using AjaxControlToolkit;

using System.Data.SqlClient;

namespace KMPS_JF_Setup.Publisher
{
    public partial class Pub_Forms : System.Web.UI.Page
    {
        JFSession jfsess = new JFSession();
        DataTable dtFormField = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string link1 = "http://" + Request.ServerVariables["HTTP_HOST"] + "/jointforms/Forms/Subscription.aspx" + "?pubcode=" + DataFunctions.ExecuteScalar("select pubCode from publications where pubID = " + Request.QueryString["PubID"]).ToString();
                    string link2 = "http://" + Request.ServerVariables["HTTP_HOST"] + "/jointforms/Forms/Subscription.aspx" + "?pubID=" + Request.QueryString["PubID"];
                    hLink1.NavigateUrl = link1;
                    hLink1.Text = link1;

                    hLink2.NavigateUrl = link2;
                    hLink2.Text = link2;

                    BoxPanel2.Title = "Manage Subscription for " + Request.QueryString["PubName"] + ":";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void grdPublisherForms_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "FormDelete")
                {
                    SqlCommand cmdFormDelete = new SqlCommand("sp_deletePubForms"); 
                    cmdFormDelete.CommandType = CommandType.StoredProcedure;
                    cmdFormDelete.Parameters.Add(new SqlParameter("@PFID", e.CommandArgument.ToString()));  
                    DataFunctions.Execute(cmdFormDelete);  
                    Response.Redirect("Pub_Forms.aspx?PubId=" + Request.QueryString["PubID"] + "&PubName=" + Request.QueryString["PubName"]);
                }
                if (e.CommandName == "FormCopy")
                {
                    JFSession jfsess = new JFSession();
                    int PFID = Convert.ToInt32(e.CommandArgument.ToString());
                    SqlCommand cmdFormDelete = new SqlCommand("sp_PubFormCopy");
                    cmdFormDelete.CommandType = CommandType.StoredProcedure;
                    cmdFormDelete.Parameters.Add(new SqlParameter("@PFID", e.CommandArgument.ToString()));
                    cmdFormDelete.Parameters.Add(new SqlParameter("@UserID", jfsess.UserID()));
                    DataFunctions.Execute(cmdFormDelete);
                    Response.Redirect("Pub_Forms.aspx?PubId=" + Request.QueryString["PubID"] + "&PubName=" + Request.QueryString["PubName"]);
                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pub_FormsCreate.aspx?PubId=" + Request.QueryString["PubID"] + "&PFID=0&PubName=" + Request.QueryString["PubName"]);
        }
    }
}
