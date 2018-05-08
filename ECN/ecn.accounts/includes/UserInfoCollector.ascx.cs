namespace ecn.accounts.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;

	
	///		Summary description for UserInfoCollector.
	
	public partial class UserInfoCollector : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			
		}

		public void SetUser(User user) {
			user.UserName = txtUserName.Text;
			user.Password = txtPassword1.Text;			
		}

		public User User {
			set { 
				LoadUserInfoIntoControls(value);
			}
		}

		private void LoadUserInfoIntoControls(User user) {
			txtUserName.Text = user.UserName;
			txtPassword1.Text = user.Password;
			txtPassword2.Text = user.Password;
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
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
