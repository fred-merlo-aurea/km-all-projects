namespace ecn.accounts.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	
	///		Summary description for EmailNote.
	
	public partial class EmailNotes : System.Web.UI.UserControl
	{

		public string Notes {
			get { return txtNotes.Text;}
			set { txtNotes.Text = value;}
		}

		public string InternalNotes {
			get { return txtInternalNotes.Text;}
			set { txtInternalNotes.Text = value;}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			
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
