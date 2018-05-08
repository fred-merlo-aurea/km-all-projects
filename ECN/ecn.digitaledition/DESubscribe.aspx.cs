using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Configuration;
using System.Text;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Data;

namespace ecn.digitaledition
{
    public partial class DESubscribe : System.Web.UI.Page
    {
        int EmailID;
        int GroupID;
        int EditionID;

        protected void Page_Load(object sender, EventArgs e)
        {
            EmailID = GetEmailID();
            GroupID = GetGroupID();
            EditionID = GetEditionID();

            if (EmailID == 0 || GroupID == 0 || EditionID == 0)
            {
                NotifyAdmin("Error in Digital Edition DESubscribe.aspx", "Invalid Parameter(s): ");
                btnSubscribe.Enabled = false;
                lblMessage.Text = "There has been a problem with your request.  Customer Support has been notified.";
            }
            else
            {
                if (!IsPostBack)
                {
                    try
                    {
                        DataTable dt = DataFunctions.GetDataTable("select GroupID, m.CustomerID, e.EditionName, '/ecn.images/customers/' + convert(varchar,c.customerID) + '/publisher/' + convert(varchar,editionID) + '/' as imgpath from Publications m join editions e on m.PublicationID = e.PublicationID join ecn5_accounts..customer c on m.customerID = c.customerID where e.editionID = " + EditionID.ToString());
                        string editionName = dt.Rows[0]["EditionName"].ToString();
                        string imgPath = dt.Rows[0]["imgpath"].ToString();

                        hl.ImageUrl = ConfigurationManager.AppSettings["ImagePath"] + imgPath + "150/1.png";
                        hl.NavigateUrl = "http://mlb.ecndigitaledition.com/Magazine.aspx?eID=" + EditionID.ToString() + "&e=" + EmailID.ToString();
                        lblName.Text = editionName;
                        lblMessage.Text = "Click to subscribe to this Mailing List.";
                    }
                    catch (Exception ex)
                    {
                        NotifyAdmin("Error in Digital Edition DESubscribe.aspx", ex.Message);
                    }
                }
            }
        }

        protected void btnSubscribe_Click(object sender, EventArgs e)
        {
            try
            {
                Groups group = new Groups(GroupID);
                Emails email = new Emails(EmailID);
                group.AttachEmail(email, "HTML", "S");
                lblMessage.Text = "You have successfully subscribed to the Mailing List.";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "There has been a problem with your request.  Customer Support has been notified.";
                NotifyAdmin("Error in Digital Edition DESubscribe.aspx", ex.Message);
            }
            btnSubscribe.Enabled = false;
            
        }

        private int GetEmailID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["e"].ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int GetEditionID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["eID"].ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int GetGroupID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["g"].ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void NotifyAdmin(string subject, string body)
        {            
            string adminEmailSubject = subject;
            StringBuilder adminEmailBody = new StringBuilder(body);
            adminEmailBody.AppendLine("<BR>EmailID: " + EmailID.ToString());
            adminEmailBody.AppendLine("<BR>GroupID: " + GroupID.ToString());
            adminEmailBody.AppendLine("<BR>EditionID: " + EditionID.ToString());
            adminEmailBody.AppendLine("<BR>Page URL: " + Request.RawUrl.ToString());

            EmailFunctions emailFunctions = new EmailFunctions();
            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], adminEmailSubject, adminEmailBody.ToString());
            
        }
    }
}
