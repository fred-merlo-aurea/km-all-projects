using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using ecn.common.classes;
using ecn.communicator.classes;

namespace PaidPub
{
    public partial class forgotpassword : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ValidateEmailAddress(txtE.Text))
            {
                phError.Visible = true;
                lblErrorMessage.Text = "Please enter a valid emailaddress.";
                lblErrorMessage.Visible = true;
                return;
            }
            try
            {
                DataTable dtEmail = DataFunctions.GetDataTable("select top 1 e.password from emails e join emailgroups eg on e.emailID = eg.emailID where e.customerID = " + ConfigurationManager.AppSettings["Pharmalive_CustomerID"].ToString() + " and subscribetypecode = 'S' and e.emailaddress = '" + txtE.Text.Replace("'", "''") + "'");

                if (dtEmail.Rows.Count > 0)
                {
                    SendEmail(dtEmail.Rows[0]["password"].ToString());
                    lblSuccessmessage.Text = "We have emailed you the password.";
                    lblSuccessmessage.Visible = true;
                }
                else
                {
                    lblErrorMessage.Text = "Email address not exists in the system";
                    phError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                phError.Visible = true;
            }

        }

        private void SendEmail(string password)
        {
            string msgBody = string.Empty;
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["Pharmalive_FromEmail"].ToString());
            message.To.Add(txtE.Text);
            message.Subject = "Password Request.";

            msgBody += "<font face='arial' size='2'>Dear %%firstname%%:<BR><BR>Your Password for accessing XTRA content is <u>%%password%%</u>. If you require assistance, please contact Subscription Services at <a href='mailto:circulation_pa@cancom.com'>circulation_pa@cancom.com</a> or call 888-354-3544, extension 9814.<BR><br>Thank you.<BR><BR>Fran Mehesz<br>Subscription Services Representative<font>";
            
            msgBody = msgBody.Replace("%%firstname%%", txtFirstName.Text);
            msgBody = msgBody.Replace("%%password%%", password);

            message.Body = msgBody;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
        }

        private bool ValidateEmailAddress(string emailaddress)
        {
            string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
       + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
       + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

            if (emailaddress != null) return Regex.IsMatch(emailaddress, MatchEmailPattern);
            else return false;

        }


    }
}
