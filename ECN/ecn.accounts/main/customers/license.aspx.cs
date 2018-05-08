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

namespace ecn.accounts.main.customers
{
    public partial class license : ECN_Framework.WebPageHelper
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS;
            Master.SubMenu = "";
            Master.Heading = "Create New License";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icocustomers.gif><b>Unsent Emails</b><br />These are the emails you wrote or started writing but have not sent. You can also edit an email before you send it, Click the edit link. To send the email, first set the groups you want to recieve this Blast. </p>&#13;&#10;&#9;&#9;<p><b>Sent Emails</b><br />These emails are stored in your database and are available to view and/or send again. </p>&#13;&#10;&#9;&#9;<p><b>Helpful Hint</b><br />To send an email again, first 'view' the email and while viewing the email click 'write new email' link in the navigation. All you have to do is select the layout you want, rename it and click the preview email button.</p>&#13;&#10;&#9;&#9;";
            Master.HelpTitle = "Customer Manager";
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
