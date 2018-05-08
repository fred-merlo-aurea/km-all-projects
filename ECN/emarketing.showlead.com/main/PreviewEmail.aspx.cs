using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

using System.Web.Mail;
using ecn.common.classes;
using ecn.showcare.wizard.main;

namespace ecn.showcare.wizard
{
	/// <summary>
	/// Summary description for PreviewEmail.
	/// </summary>
	public class PreviewEmail : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.ImageButton btnBack;
		protected Steps steps = new Steps();
		protected System.Web.UI.WebControls.ImageButton btnNext;
		protected System.Web.UI.WebControls.Label previewLbl;
		protected System.Web.UI.WebControls.TextBox txtTestEmail;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvEmail;
		protected System.Web.UI.WebControls.RegularExpressionValidator valEmailAddress;
		protected System.Web.UI.WebControls.Button btnTestEmail;
		protected System.Web.UI.WebControls.Label lblSentEmail;
		protected Step3 step3;

		private void BuildPreview () {
			Step2 step2 = (Step2) Session["step2"];
			Step1 step1 = (Step1) Session["step1"];


			string sql = "SELECT TemplateSource FROM Templates WHERE TemplateID="+step1.TemplateID;
			string body = Convert.ToString(DataFunctions.ExecuteScalar(sql));

			string csrc = step2.ContentSource;
			
			// Before sshowing the preview we need to add code-snippet and footer information to content-source
			if (step2.Salutation.Equals("firstname")) {
				csrc = "<P align=left>Dear %%FirstName%%,</P>" + csrc;
			} else {
				csrc = "<P align=left>Dear %%FirstName%% %%LastName%%,</P>" + csrc;
			}
			// In any case we need to insert Footer-Information in the end
			csrc += "<BR><P align=left>"+
				step2.FooterName+"<BR>"+
				step2.FooterTitle+"<BR>"+
				step2.FooterCompany+"<BR>"+
				step2.FooterPhone+"<BR>";

			if (step1.IsCustomHeader)
			{
				string headerImage = "";
				if (step2.HeaderImage != string.Empty)
				{
					ChannelCheck cc = new ChannelCheck();

					headerImage = "<IMG src='"+ cc.getAssetsPath("accounts")+"/"+"channelID_"+ ConfigurationManager.AppSettings["ChannelID"] +"/customers/"+step1.CustomerID+"/images/" + step2.HeaderImage + "' border=0>";
				}
				else
					headerImage = "&nbsp;";

				csrc = "<TR><TD style='HEIGHT: 100px;border:2px solid #afc2d5;' valign='middle' ><center>" + headerImage + "</center></TD></TR><TR><TD style='PADDING-RIGHT: 30px; PADDING-LEFT: 30px; FONT-SIZE: 12px; PADDING-BOTTOM: 30px; PADDING-TOP: 30px'>" + csrc + "</TD></TR>";
			}

			body = StringFunctions.Replace(body,"%%slot1%%",csrc);
			body=StringFunctions.Replace(body,"®","&reg;");
			body=StringFunctions.Replace(body,"©","&copy;");
			body=StringFunctions.Replace(body,"™","&trade;");
			body=StringFunctions.Replace(body,"…","...");
			body=StringFunctions.Replace(body,((char)0).ToString(),"");
			body="<table><tr><td align=left>"+body+"</td></tr></table>";

			if (step1.IsCustomHeader)
			{
				  body = "<DIV style='FONT-WEIGHT: bold; FONT-SIZE: 12px; COLOR: red'>If your image is wider than the borders on the screen below, your image is more than 650px(pixels) wide.<BR>For better results, reduce the width of your image to 650px..</DIV><bR>" + body;
			}
			previewLbl.Text = body;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Set Last visited step to current
			if (!IsPostBack && (steps.Current > steps.LastVisited)) {
				steps.nextStep();
			}
			lblSentEmail.Visible=false;
			// Generate preview of the message and display
			BuildPreview();
			
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnTestEmail.Click += new System.EventHandler(this.btnTestEmail_Click);
			this.btnBack.Click += new System.Web.UI.ImageClickEventHandler(this.btnBack_Click);
			this.btnNext.Click += new System.Web.UI.ImageClickEventHandler(this.btnNext_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnBack_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			// Decreament the current step
			steps.decreamentCurrentStep();
			// Redirect to previous Step
			Response.Redirect("UpdateMessage.aspx");
		}

		private void btnNext_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			// Goto the next and final step
			Response.Redirect("CreditCardSend.aspx");
		}

		private void btnTestEmail_Click(object sender, System.EventArgs e)
		{
			Step2 step2 = (Step2) Session["step2"];
			string body = previewLbl.Text;
			SmtpMail.SmtpServer = ConfigurationManager.AppSettings["SMTPServer"];
			MailMessage mm = new MailMessage();
			mm.Body = body;
			mm.BodyFormat = MailFormat.Html;
			mm.From = ConfigurationManager.AppSettings["fromemail"];				
			mm.Subject = "TEST CAMPAIGN - " + step2.EmailSubject;	
			mm.To = this.txtTestEmail.Text.ToString();
			SmtpMail.Send(mm);
			lblSentEmail.Visible=true;
		}

	}
}
