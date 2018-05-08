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

namespace ecn.showcare.wizard.main
{
	/// <summary>
	/// Summary description for Confirmation.
	/// </summary>
	public class Confirmation : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm Form1;
		protected System.Web.UI.WebControls.ImageButton btnShowlead;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divMsg;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Check if message is present in the session if it does show it, other wise dont know what to do1
			try {
				if (Session["msg"] != null ) {
					Label lbl = new Label();
					lbl.Text = Session["msg"].ToString();
					divMsg.Controls.Add(lbl);

				}
			} catch (Exception) {}
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
			this.btnShowlead.Click += new System.Web.UI.ImageClickEventHandler(this.btnShowlead_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnNext_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			//Response.Redirect(ConfigurationManager.AppSettings["BadLogin"]+"blast.aspx");
			string redirectURL = Session["SCRedirectPage"].ToString();
			Session.Clear();
			Response.Redirect(redirectURL);

		}

		private void btnShowlead_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			//Response.Redirect(ConfigurationManager.AppSettings["BadLogin"]+"ExhibitorDetails.aspx");
			string redirectURL = Session["SCRedirectPage"].ToString();
			Session.Clear();
			Response.Redirect(redirectURL);
		}
	}
}
