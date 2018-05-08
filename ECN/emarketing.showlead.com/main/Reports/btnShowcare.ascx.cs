namespace ecn.showcare.wizard.main.Reports
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for btnShowcare.
	/// </summary>
	public class btnShowcare : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.ImageButton showcare;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
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
			this.showcare.Click += new System.Web.UI.ImageClickEventHandler(this.showcare_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void showcare_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			string redirectURL = Session["SCRedirectPage"].ToString();
			Session.Clear();
			Response.Redirect(redirectURL);
		}
	}
}
