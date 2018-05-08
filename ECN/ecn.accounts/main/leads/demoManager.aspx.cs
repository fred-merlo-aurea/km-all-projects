using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Net.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.accounts.classes;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.accounts.main.leads
{



    public partial class demoManager : ECN_Framework.WebPageHelper
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.LEADS;
            Master.SubMenu = "Demo Manager";
            Master.Heading = "Demo Manager";
            Master.HelpContent = "";
            Master.HelpTitle = "Demo Manager";
            
            if (!IsPostBack) {
				DateTime endDate = GetFirstDayOfCurrentWeek();
				txtStart.Text = endDate.AddDays(-7).ToShortDateString();
				txtEnd.Text = endDate.ToShortDateString();
				LoadLeadsWithDemo();
			}
		}

		#region Event Handler

		
		private void dgdLeads_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) {
			int emailID = 0;
			Emails email;
			BlastSingle blastSingle;
			switch (e.CommandName) {				 
				case "Done":					
					emailID = Convert.ToInt32(e.CommandArgument);
					email = new Emails(emailID);
					email.SetValue("UserEvent2", SqlDbType.VarChar, 50, "Done");

					blastSingle = new BlastSingle(LeadConfig.ThankyouBlastID, emailID, DateTime.Now);
					blastSingle.Save();	
					break;
				case "Absent":
					emailID = Convert.ToInt32(e.CommandArgument);
					email = new Emails(emailID);
					email.SetValue("UserEvent2", SqlDbType.VarChar, 50, "Absent");

					blastSingle = new BlastSingle(LeadConfig.AbsentBlastsID, emailID, DateTime.Now);
					blastSingle.Save();
					email = Emails.GetEmailByID(emailID);
					Staff nbd = Staff.GetStaffByID(Convert.ToInt32(email.User6));
					Email(nbd.Email, "A prospective customer has missed their demo", string.Format(@"
Hello {0},

{1} with {2} did not show up for the demo. A followup email has been sent out. Please contact him/her. The phone number is : {3}

Thank you very much,

KM
", nbd.FirstName, email.FirstName, email.Company, email.Voice));
					break;
			}

			LoadLeadsWithDemo();
		}

		protected void btnRefine_Click(object sender, System.EventArgs e) {
			LoadLeadsWithDemo();
		}

		private void dgdLeads_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) {
			if (e.Item.ItemType == ListItemType.Header ||e.Item.ItemType == ListItemType.Footer) {
				return;
			}
			
			Label lblDemoDate = e.Item.FindControl("lblDemoDate") as Label;				
			lblDemoDate.Style.Add("Font","normal");		
			
			DateTime demoDate = Convert.ToDateTime(lblDemoDate.Text);
			if (demoDate >= DateTime.Now && DateTime.Now.AddDays(7) >= demoDate) {
				e.Item.Style.Add("background-color", "yellow");				
			}
		}


		private void dgdLeads_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e) {
			IsDesc = !IsDesc;
			ArrayList leads = GetLeadsByDateRange();
			leads.Sort(new LeadDemoDateComparer(IsDesc));
			dgdLeads.DataSource = leads;
			dgdLeads.DataBind();			
		}
		#endregion


		private DateTime GetFirstDayOfCurrentWeek() {
			DateTime startDate = DateTime.Now;

			while (startDate.DayOfWeek != DayOfWeek.Monday) {
				startDate = startDate.AddDays(-1);
			}
			return startDate;
		}

		private void LoadLeadsWithDemo() {
			dgdLeads.DataSource = GetLeadsByDateRange();
			dgdLeads.DataBind();
		}

		private ArrayList GetLeadsByDateRange() {
			DateTime start, end;
			try {
				start = Convert.ToDateTime(txtStart.Text);
			} catch (Exception) {					
				start = DateTimeInterpreter.MinValue;
				txtStart.Text = "";
			}

			try {
				end = Convert.ToDateTime(txtEnd.Text);
			} catch( Exception) {
				end = DateTimeInterpreter.MaxValue;		
				txtEnd.Text = "";
			}

			return new Lead().GetLeadsWithDemo(start, end);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    
			this.dgdLeads.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgdLeads_ItemCommand);
			this.dgdLeads.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.dgdLeads_SortCommand);
			this.dgdLeads.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgdLeads_ItemDataBound);

		}
		#endregion

		
		private bool IsDesc {
			get { 
				if (Session["SortingDirection"] == null) {
					return true;
				}
				return Convert.ToBoolean(Session["SortingDirection"]);}
			set { Session["SortingDirection"] = value;}
		}	

		private string SmtpServer {
			get {
				return ConfigurationManager.AppSettings["SmtpServer"];
			}
		}

		private void Email(string toEmail, string subject, string body) {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("demo@knowledgemarketing.com");
            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = body;

            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
		}
	}
}
