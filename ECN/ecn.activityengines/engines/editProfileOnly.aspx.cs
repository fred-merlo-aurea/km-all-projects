using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.activityengines{
	
	
	
	public partial class editProfile : System.Web.UI.Page {

		protected string EmailID = "", EmailAdd="", GroupID="", Action="", panelToShow="";

		protected void Page_Load(object sender, System.EventArgs e) {
			EmailID			= getEmailID();
			EmailAdd		= getEmailAddress();
			GroupID			= getGroupID();
			Action			= getAction();
			panelToShow = getPanelToShow();

			Emails emailProfile = Emails.GetEmailByID(Convert.ToInt32(EmailID));
			string emailProfileFullName = emailProfile.FirstName +" "+emailProfile.LastName;
		
			EmailProfile_Panel.Controls.Clear();

			Control emailProfileControl = LoadControl("../includes/emailProfile_Base.ascx");
			EmailProfile_Panel.Controls.Add(emailProfileControl);
			EmailProfile_Panel.Visible=true;
		}

		#region getEmailID, getEmailAddress, getGroupID, getAction, getPanelToShow
		private string getEmailID() {
			string theEmailID = "";
			try {
				theEmailID	= Request.QueryString["eID"].ToString();
			}catch {
				MessageLabel.Visible = true;
				MessageLabel.Text = "<br>ERROR: EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received";
			}
			return theEmailID;
		}

		private string getEmailAddress() {
			string theEmailAddress = "";
			try {
				theEmailAddress	= Request.QueryString["eAD"].ToString();
			}catch{
				MessageLabel.Visible = true;
				MessageLabel.Text = "<br>ERROR: EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received";
			}
			return theEmailAddress;
		}

		private string getGroupID() {
			string theGroupID = "";
			try {
				theGroupID	= Request.QueryString["gID"].ToString();
			}catch {
				MessageLabel.Visible = true;
				MessageLabel.Text = "<br>ERROR: EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received";
			}
			return theGroupID;
		}

		private string getAction() {
			string theAction = "";
			try {
				theAction	= Request.QueryString["action"].ToString();
			}catch{
				theAction = "view";
			}
			return theAction;
		}
		private string getPanelToShow() {
			string thePanel = "";
			try {
				thePanel	= Request.QueryString["panel"].ToString();
			}catch {
			}
			return thePanel;
		}

		#endregion

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

		}
		#endregion
	}
}
