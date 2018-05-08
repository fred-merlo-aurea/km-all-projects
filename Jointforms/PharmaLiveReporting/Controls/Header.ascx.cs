	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;
	using ECN_Framework.Common;

public partial class Header : System.Web.UI.UserControl
	{

		SecurityCheck sc = new SecurityCheck();
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				try
				{
					lblUser.Text= DataFunctions.GetUser(Convert.ToInt32(sc.UserID()));
				}
				catch
				{
					//Response.Redirect("/ecn.accounts/main/");
				}
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


	}
