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

namespace ecn.communicator.front
{
    public partial class services : ECN_Framework.WebPageHelper
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.LOGIN;
            Master.SubMenu = "";
            Master.Heading = "Flexible Offerings";
            Master.HelpContent = "From do-it-yourself to fully-managed strategic blasts, ECN.communicator ensures your email marketing efforts realize maximum ROI.<br /><br /><a href=signup.aspx><img src='/ecn.images/images/actionbutton.gif' border='0'></a>";
            Master.HelpTitle = "Campaigns of All Sizes.";				
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
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
