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



    public partial class features : ECN_Framework.WebPageHelper
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.LOGIN;
            Master.SubMenu = "";
            Master.Heading = "Features List";
            Master.HelpContent = "<b>List Builders</b><br>Our web forms and viral marketing tools grow your list of permission-based email addresses quickly.<br><br><b>Libraries</b><br>ECN.communicator provides simple management of your contacts, content, images, surveys and documents in our online libraries. <br><br><b>Precision Target Marketing Tools</b><br />Our Direct Marketing tools help you target, personalize and optimize your emails.<br><br><a href='signup.aspx'><img src='/ecn.images/images/actionbutton.gif' border='0'></a>";
            Master.HelpTitle = "The Power of the Latest eMarketing Tools";			
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
