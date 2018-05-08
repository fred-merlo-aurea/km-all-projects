using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using KMPS_JF_Objects.Objects;
using System.Text.RegularExpressions;

namespace KMPS_JF.Forms
{
    public partial class PasswordHelp : System.Web.UI.Page
    {
        private int CustomerID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["CustomerId"].ToString());
                }
                catch
                {
                    return 0;
                }
            }

        }

        private int GroupID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["GroupID"].ToString());
                }
                catch
                {
                    return 0;
                }
            }

        }

        private string pubcode
        {
            get
            {
                try
                {
                    string PubCode = Request.QueryString["PubCode"].ToString();

                    if (PubCode.Length > 15)
                    {
                        PubCode = PubCode.Substring(0, 15);
                    }

                    PubCode = PubCode.Replace("'", "''");

                    return PubCode;

                }
                catch
                {
                    return "info";
                }
            }

        }

        public int EnableCS
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["enableCS"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (CustomerID > 0)
                    {
                        pnlstep1.Visible = true;
                        pnlstep2.Visible = false;
                    }
                }
                catch
                {
                    lblMessage2.Text = "Invalid Parameters. Click on the \"I can't access my account\" link from login page";
                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (rbforgot.Checked)
                btnFinish.Text = "Send Password";
            else if (EnableCS == 1)
                btnFinish.Text = "Email Customer Service";
            else
                btnFinish.Text = "Reset Password";

            pnlstep1.Visible = false;
            pnlstep2.Visible = true;
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                int EmailID = 0;
                string password = string.Empty;
                string newpassword = string.Empty;

                //DataTable dt = DataFunctions.GetDataTable("select e.emailID, isnull(password,'') as Password from emails e join emailgroups eg on e.emailID = eg.emailID where emailaddress= '" + txtemailaddress.Text.Replace("'", "''") + "' and CustomerID = " + CustomerID + " and groupID= " + GroupID, ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString);

                SqlCommand cmd = new SqlCommand("select e.emailID, isnull(password,'') as Password from emails e join emailgroups eg on e.emailID = eg.emailID where emailaddress= @email and CustomerID = @customerID and groupID= @GroupID");
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 50));
                cmd.Parameters["@email"].Value = txtemailaddress.Text.ToString();

                cmd.Parameters.Add(new SqlParameter("@customerID", SqlDbType.Int));
                cmd.Parameters["@customerID"].Value = CustomerID;

                cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
                cmd.Parameters["@GroupID"].Value = GroupID;

                DataTable dt = DataFunctions.GetDataTable("communicator", cmd);

                if (dt.Rows.Count > 0)
                {
                    EmailID = Convert.ToInt32(dt.Rows[0]["EmailID"]);
                    password = dt.Rows[0]["Password"].ToString();
                    newpassword = System.Guid.NewGuid().ToString().Substring(0, 7);

                    if (rbforgot.Checked)
                    {
                        if (password.Length == 0)
                        {
                            //DataFunctions.Execute("update ecn5_communicator..emails set password='" + newpassword + "' where emailID = " + EmailID);
                            SqlCommand Updatecmd = new SqlCommand("update ecn5_communicator..emails set password=@password, DateUpdated=getdate()  where emailID = @EmailID; update ecn5_communicator..emailgroups set LastChanged=getdate(),LastChangedSource='JFs Password Reset'  where emailID = @EmailID and groupID = @groupID;");
                            Updatecmd.CommandTimeout = 0;
                            Updatecmd.CommandType = CommandType.Text;

                            Updatecmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 50));
                            Updatecmd.Parameters["@password"].Value = newpassword;

                            Updatecmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
                            Updatecmd.Parameters["@EmailID"].Value = EmailID;

                            Updatecmd.Parameters.Add(new SqlParameter("@groupID", SqlDbType.Int));
                            Updatecmd.Parameters["@groupID"].Value = GroupID;

                            DataFunctions.Execute(Updatecmd);

                            SendMail(newpassword);
                        }
                        else
                        {
                            SendMail(password);
                        }
                    }
                    else
                    {
                        //DataFunctions.Execute("update ecn5_communicator..emails set password='" + newpassword + "' where emailID = " + EmailID);
                        SqlCommand Updatecmd = new SqlCommand("update ecn5_communicator..emails set password=@password, DateUpdated=getdate() where emailID = @EmailID; update ecn5_communicator..emailgroups set LastChanged=getdate(),LastChangedSource='JFs Password Reset'  where emailID = @EmailID and groupID = @groupID; ");
                        Updatecmd.CommandTimeout = 0;
                        Updatecmd.CommandType = CommandType.Text;

                        Updatecmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 50));
                        Updatecmd.Parameters["@password"].Value = newpassword;

                        Updatecmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
                        Updatecmd.Parameters["@EmailID"].Value = EmailID;

                        Updatecmd.Parameters.Add(new SqlParameter("@groupID", SqlDbType.Int));
                        Updatecmd.Parameters["@groupID"].Value = GroupID;

                        DataFunctions.Execute(Updatecmd);
                        SendMail(newpassword);
                    }

                    if (EnableCS == 1)
                    {
                        lblMessage.Text = "Your request has been sent. Your new password will be emailed to you shortly.";
                    }
                    else
                    {
                        lblMessage.Text = "We have sent your password to your email address.";
                    }

                    btnFinish.Visible = false;
                }
                else
                {
                    lblMessage.Text = "ERROR : We couldn't find that email address in our records ";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "ERROR : " + ex.Message;
            }
        }

        private void SendMail(string password)
        {
            string body = "";
            string subject = "";
            string receiver = "";

            if (EnableCS == 1 && !rbforgot.Checked)
            {
                body = string.Format("Please reset the password for {0}.", txtemailaddress.Text);
                subject = String.Format("{0} Password Reset", pubcode);
                receiver = ConfigurationManager.AppSettings["CustomerServiceEmail"];
            }
            else
            {
                body = string.Format("Here is your requested password : {0}<br><br>If you need further assistance, please contact customer service at <a href='mailto:{1}@KMPSGROUP.COM'>{1}@KMPSGROUP.COM</a> or call us at 1-800-869-6882 and press 5.", password, pubcode);
                
                Publication publication = Publication.GetPublicationbyID(0, pubcode);

                if (publication != null)
                {
                    if (publication.ForgotPasswordHTML != string.Empty)
                    { 
                        body = publication.ForgotPasswordHTML;

                        body = Regex.Replace(body, "%%PASSWORD%%", password, RegexOptions.IgnoreCase); 
                    }
                }

                subject = "Password Request";
                receiver = txtemailaddress.Text;
            }

            MailMessage mm = new MailMessage();
            mm.Body = body;
            mm.IsBodyHtml = true;

            mm.From = new MailAddress(pubcode + "@kmpsgroup.com");
            mm.Subject = subject;
            mm.To.Add(receiver);

            SmtpClient sc = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
            sc.Send(mm);
        }
    }
}
