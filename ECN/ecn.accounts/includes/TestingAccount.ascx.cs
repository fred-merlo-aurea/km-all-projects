namespace ecn.accounts.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	
	///		Summary description for TestingAccount.
	
	public partial class TestingAccount : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			
		}

		private string _loginUrl;
		public string LoginUrl {
			get {
				return (this._loginUrl);
			}
			set {
				this._loginUrl = value;
			}
		}

		private string _termsAndConditionLink;
		public string TermsAndConditionLink {
			set {
				this._termsAndConditionLink = value;
				lblTermsAndConditions.Text = "I have read and agreed to abide by the terms of the <br>"+_termsAndConditionLink;
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
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion

		protected void btnLogin_Click(object sender, System.EventArgs e) {
			lblErrorMessage.Visible = !chkAgreeToTerms.Checked;
			if (chkAgreeToTerms.Checked) {				
				Server.Transfer(LoginUrl);
			}
		}
	}
}
