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



    public partial class howitworks : ECN_Framework.WebPageHelper
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.LOGIN;
            Master.SubMenu = "";
            Master.Heading = "Why ECN.communicator?";
            Master.HelpContent = "From simple to sophisticated campaigns, our technology is structured to manage your data as well as help you plan, act upon and analyze your email campaigns.<br /><br />The entire hosted application is highly scalable, simplifying the implementation and integration process, ensuring that no matter the size of your list or the complexity of the campaign- .communicator can manage it. <br /><br /><a href=signup.aspx><img src='/ecn.images/images/actionbutton.gif' border='0'></a>";
            Master.HelpTitle = "Complete Control Without the Agony";			
		
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
