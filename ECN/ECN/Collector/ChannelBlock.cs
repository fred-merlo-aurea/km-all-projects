using System;
using System.Data;
using System.Web;
using ecn.common.classes;
using ECN.Common;

namespace ecn.collector.classes {
	
	
	
    public class ChannelBlock : ChannelBlockBase
    {

        ECN_Framework.Common.SecurityCheck sc = null;
		//ECN_Framework.Common.ChannelCheck cc = null;

		public ChannelBlock(){
			//Empty Constructor
            sc = new ECN_Framework.Common.SecurityCheck();
			//cc = new ECN_Framework.Common.ChannelCheck();
		}

		public string MainMenu(string menucode) {
            string vp = System.Configuration.ConfigurationManager.AppSettings["Collector_VirtualPath"];
            string ecnHomevp = System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"];
			string output="<table cellpadding=0 cellspacing=0><tr><td nowrap>";
			output+="<A class=menu href="+ecnHomevp+"/main/>Home</A>&nbsp;&nbsp;&nbsp;";

			switch (menucode) 
			{
				case "manageSurvey":
					output+="<A class=menu2 href="+vp+"/main/survey/default.aspx>Manage Survey</A>&nbsp;&nbsp;&nbsp;";
						//"<A class=menu href="+vp+"/main/report/survey_report.aspx?mode=report>View Reports</A>&nbsp;&nbsp;&nbsp;";
					break;
//				case "reports":
//					output+="<A class=menu href="+vp+"/main/survey/default.aspx>Manage Survey</A>&nbsp;&nbsp;&nbsp;"+
//						"<A class=menu2 href="+vp+"/main/report/survey_report.aspx?mode=report>View Reports</A>&nbsp;&nbsp;&nbsp;";
//					break;
				default:
					output+="<A class=menu href="+vp+"/main/survey/default.aspx>Manage Survey</A>&nbsp;&nbsp;&nbsp;";
						//"<A class=menu href="+vp+"/main/report/survey_report.aspx?mode=report>View Reports</A>&nbsp;&nbsp;&nbsp;";
					break;
			}
			output+="</td></tr></table>";
			return output;
		}

		private string getClass(string CurrentMenu, string ActiveMenu)
		{
			if (CurrentMenu.ToLower() == ActiveMenu.ToLower())
			{
				return "menu2";
			}
			else
			{
				return "menu1";
			}
		}

		public string SubMenu(string menucode, string ActiveMenu) {
            string vp = System.Configuration.ConfigurationManager.AppSettings["Collector_VirtualPath"];
			string output="<table cellpadding=0 cellspacing=0><tr><td nowrap>";
            switch (menucode) {
                case "index":
                    output+=" ";
                    break;
                case "manageSurvey":
                    output+="<A class=" + getClass("Survey list", ActiveMenu) + " href="+vp+"/main/survey/default.aspx>Survey list</A>&nbsp;&nbsp;&nbsp;" +
						"<A class=" + getClass("new Survey", ActiveMenu) + " href="+vp+"/main/survey/SurveyWizard.aspx>new Survey</A>&nbsp;&nbsp;&nbsp;";
                        //"<A class=" + getClass("download Survey", ActiveMenu) + " href="+vp+"/main/archive.aspx>download Survey</A>";
                    break;
                default:
                    output+=" ";
                    break;
            }
			output+="&nbsp;</td></tr></table>";
			return output;
		}
	}
}
