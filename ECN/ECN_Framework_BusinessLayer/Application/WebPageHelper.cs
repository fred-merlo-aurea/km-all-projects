using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO;


namespace ECN_Framework_BusinessLayer.Application
{
    public class WebPageHelper : Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            //ecnSession es = ecnSession.CurrentSession();
            ECNSession UserSession = ECNSession.CurrentSession();

            if (UserSession != null)
            {
                try
                {
                    if (Directory.Exists(Server.MapPath("~/App_Themes/" + UserSession.CurrentBaseChannel.BaseChannelID.ToString())))
                    {
                        this.Theme = UserSession.CurrentBaseChannel.BaseChannelID.ToString();
                    }
                    else
                    {
                        this.Theme = "1";
                    }
                }
                catch { this.Theme = "1"; }
            }

            base.OnPreInit(e);
        }
    }
}



