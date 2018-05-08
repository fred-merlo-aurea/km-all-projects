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
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.communicator.front {




    public partial class login : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {

            string ap = System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"];

            string calltoaction = "<HR><BR><A href=# onclick=ja vascript:window.open('demo.aspx','demo','width=415,height=420'); ><img src=" + ap + "/images/sidebaraction1.gif border=0></a>";
            calltoaction += "<BR><A href=# onclick=javascript:window.open('call.aspx','demo','width=370,height=190'); ><img src=" + ap + "/images/sidebaraction2.gif border=0></a>";
            
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.LOGIN;
            Master.SubMenu = "";
            Master.Heading = "Powerful eMail Communication for Relationship Marketing.";
            Master.HelpContent = "<center>" + LoginBlock() + calltoaction + "</center>";
            Master.HelpTitle = "Login";	

            //pageheader.divHelpBox = "<center>" + pageheader.loginBlock + calltoaction;
        }

        public string LoginBlock()
        {
            string output =
                "<table border=0 cellspacing=1 cellpadding=1>" +
                "<form action='" + System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "/includes/login.aspx" + "' method=post><tr>" +
                "<td class=tableContent align=center>username</td>" +
                "</tr><tr>" +
                "<td><input name=user value='' size=15 maxlength=100 class=formfield type=text></td>" +
                "</tr><tr>" +
                "<td class=tableContent align=center>password</td>" +
                "</tr><tr>" +
                "<td><input name=password type=Password size=15 maxlength=50 class=formfield></td>" +
                "</tr><tr>" +
                "<td class=tableContent align=middle><input type=checkbox name=persist> remember me</td>" +
                "</tr><tr>" +
                "<td align=middle><input type=submit name=cmdLogin value='Login' class=formbutton><br>" +
                "<div id=ErrorMessage><br><b><font color=darkRed> </font></b></div></td>" +
                "</tr></form>" +
                "</table>";
            return output;
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    

		}
		#endregion
	}
}
