namespace ecn.wizard.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;

	/// <summary>
	///		Summary description for Footer.
	/// </summary>
	public partial class phfooter : ecn.wizard.MasterControl
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//delete EmailID from EmailGroups for that Group.	
			string sqlQuery	=	" select Footersource from Channel where baseChannelID = " + ChannelID + " and Active = 'Y' and ChannelTypeCode = 'wizard'";

			try
			{
				phFooter.Controls.Add(new LiteralControl(DataFunctions.ExecuteScalar("accounts", sqlQuery).ToString()));
			}
			catch
			{
				throw;
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
			this.ID = "phfooter";

		}
		#endregion
	}
}
