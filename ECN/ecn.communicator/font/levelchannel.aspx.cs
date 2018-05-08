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
	public partial class levelchannel : ECN_Framework.WebPageHelper
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.LOGIN;
            Master.SubMenu = "";
            Master.Heading = "Channel Partners";
            Master.HelpContent = "<P>Are your clients interested in email marketing? Are they looking to you for solutions? Would you like to easily offer them a comprehensive, robust email marketing tools at an affordable price?<P>If so, you should consider becoming an Channel Partner ---This program allows you to own and manage the relationship with your customers and resell .communicator technology as your own service. <br /><br /><a href=signup.aspx><img src='/ecn.images/images/actionbutton.gif' border='0'></a><br /><br /><br /><a href='levelsmallbiz.aspx'>&gt;&gt;Small Business</a><br /><a href='levelenterprise.aspx'>&gt;&gt;Enterprise</a><br /><a href='levelchannel.aspx'>&gt;&gt;Channel Partner</a>";
            Master.HelpTitle = "Becoming an Channel Partner";			
		}
	}
}
