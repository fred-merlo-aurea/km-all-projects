namespace ecn.wizard.wizard
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;

	/// <summary>
	///		Summary description for Confirmation.
	/// </summary>
	public partial class Confirmation : ecn.wizard.MasterControl, IWizard 
	{

		public void Initialize() 
		{
			string msg = "";
			if (WizardSession.ProcessCC)
				msg = ConfigurationManager.AppSettings["successmessageCC"];
			else
				msg = ConfigurationManager.AppSettings["successmessage"];

			msg = msg.Replace("%%emailaddress%%", WizardSession.EmailAddress);
			
			lblMessage.Text = msg;

			ecn.wizard.Session.Dispose();

		}

		public bool Save() 
		{
				return true;
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
