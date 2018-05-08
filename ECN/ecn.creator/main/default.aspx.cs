using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.main
{

    public partial class _default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.SubMenu = "home";
            Master.Heading = "The Enterprise Communication Network";

            Master.HelpTitle = "The Enterprise Communication Network";
            //ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            string vp = System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"];
            try
            {
                //if(false == KM.Platform.User.HasApplication(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, "creator")) {
                if (!KMPlatform.BusinessLogic.Client.HasService(Master.UserSession.CurrentUser.CurrentClient.ClientID, KMPlatform.Enums.Services.CREATOR))
                {
                    Response.Redirect(vp + "/main/default.aspx");
                }
            
            }
            catch (Exception)
            {
                Response.Redirect(vp + "/main/default.aspx");
            }
        }
	}
}
