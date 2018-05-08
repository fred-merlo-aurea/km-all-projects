using KMPS_JF_Objects.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PaidForms.Forms
{
    public partial class Cancel : System.Web.UI.Page
    {
        private int getFormID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["FormID"].ToString());
            }
            catch { return 0; }
        }

        private string getQueryString(string qs)
        {
            try { return Request.QueryString[qs].ToString(); }
            catch { return string.Empty; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (getFormID() == 0)
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "FormID is missing in the URL.";
                    return;
                }

                loadHeaderFooter();

                string basePath = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;
                hlBackToForm.NavigateUrl = ConfigurationManager.AppSettings["JointFormSubscribe"].ToString() + getQueryString("pubcode") + "&step=form";
            }
        }

        private void loadHeaderFooter()
        {
            int formID = getFormID();
            DataTable dt = GetForm(formID);

            phHeader.Controls.Add(new LiteralControl(dt.Rows[0]["HeaderHTML"].ToString()));
            phFooter.Controls.Add(new LiteralControl(dt.Rows[0]["FooterHTML"].ToString()));
        }

        private DataTable GetForm(int PaidFormID)
        {
            SqlCommand cmd = new SqlCommand("select * from PaidForm where PaidFormID=@PaidFormID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PaidFormID", SqlDbType.Int)).Value = PaidFormID;
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return dt;
        }

    }
}