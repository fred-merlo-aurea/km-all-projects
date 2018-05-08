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
using System.Web.Security;

namespace ecn.wizard.LoginHandlers
{
	/// <summary>
	/// Summary description for Logout.
	/// </summary>
	public partial class Logout : ecn.wizard.MasterPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string _redirectURL = "";

			if (Session["redirectURL"]!= null)
				_redirectURL = Session["redirectURL"].ToString();
			else
				_redirectURL= "LoginPages/wizChannel_" + ChannelID.ToString() + "_login.aspx";

			HttpContext.Current.Session.Abandon();
			FormsAuthentication.SignOut();

			Response.Redirect(_redirectURL,false);
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
		}
		#endregion
	}
}
