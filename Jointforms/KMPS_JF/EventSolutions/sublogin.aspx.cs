using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KMPS_JF_Objects.Objects;
using System.Text;

namespace KMPS_JF.EventSolutions
{
    public partial class sublogin : System.Web.UI.Page
    {
        private Publication _Pub = null;

        private string _PubCode
        {
            get
            {
                try { return ConfigurationManager.AppSettings["PubCode_" + _PublicationID.ToString()].ToString(); }
                catch { return string.Empty; }
            }
        }
        private int _PublisherID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["btx_pub_id"].ToString()); }
                catch { return 0; }
            }
        }
        private int _PublicationID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["btx_m"].ToString()); }
                catch { return 0; }
            }
        }
        private int _IssueID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["btx_i"].ToString()); }
                catch { return 0; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (_PublisherID == 0 || _PublicationID == 0 || _IssueID == 0 || _PubCode == string.Empty)
                {
                    lblError.Text = "One or more invalid parameters.";
                    lblError.Visible = true;
                    pnlError.Visible = true;
                    pnlSubmit.Visible = false;
                    return;
                }
                else
                {
                    lblError.Text = string.Empty;
                    lblError.Visible = false;
                    pnlError.Visible = false;
                    pnlSubmit.Visible = true;
                }
            }


            _Pub = Publication.GetPublicationbyID(0, _PubCode);

            if (_Pub == null)
                Response.Redirect("Error.asx");

            SetupPage();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //get the emailid if one exists
            string sqlSelect = string.Empty;
            sqlSelect = "SELECT top 1 e.EmailID " +
                        "FROM ecn5_communicator..Emails e " +
                            "JOIN ecn5_communicator..EmailGroups eg ON e.EmailID = eg.EmailID " +
                            "JOIN KMPSJointForms..Publications p ON eg.GroupID = p.ECNDefaultGroupID " +
                        "WHERE " +
                            "p.PubCode = @PubCode AND " +
                            "e.EmailAddress = @EmailAddress";
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.Add(new SqlParameter("@PubCode", _PubCode));
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", txtEmailAddress.Text.Trim()));
            int emailID = 0;
            try
            {
                emailID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
            }
            catch (Exception)
            {
            }
            if (emailID > 0)
            {
                Response.Redirect(string.Format("redirecttoQG.aspx?btx_pub_id={0}&btx_m={1}&btx_i={2}&email_id={3}&add=false", _PublisherID, _PublicationID, _IssueID, emailID));
            }
            else
            {
                Response.Redirect(ConfigurationManager.AppSettings["JointFormSubscribe"].ToString() + _PubCode + string.Format("&btx_pub_id={0}&btx_m={1}&btx_i={2}", _PublisherID, _PublicationID, _IssueID));//need to pass other parameters as well
            }
        }

        private void SetupPage()
        {
            try { phHeader.Controls.Add(new LiteralControl(_Pub.HeaderHTML)); }
            catch { }

            try { phFooter.Controls.Add(new LiteralControl(_Pub.FooterHTML)); }
            catch { }

        }        
    }
}
