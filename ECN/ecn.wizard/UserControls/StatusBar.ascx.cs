namespace ecn.wizard.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;
	
	/// <summary>
	///		Summary description for StatusBar.
	/// </summary>
	public partial class StatusBar : ecn.wizard.MasterControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserName"] != null)
				email.Text = Session["UserName"].ToString();
			else
			{
				string sqlQuery	=	"SELECT UserName FROM Users WHERE UserID=" + UserID;
				email.Text = DataFunctions.ExecuteScalar("accounts", sqlQuery).ToString();
			}
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

		private void btnMainMenu_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect(UrlBase + "default.aspx",true);
		}

		private void btnLogout_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect(UrlBase + "default.aspx",true);
		}
	}
}
