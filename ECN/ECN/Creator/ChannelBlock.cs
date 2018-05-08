using System;
using System.Data;
using System.Web;
using ecn.common.classes;
using ECN.Common;

namespace ecn.creator.classes {
	
	
	
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
            string vp = System.Configuration.ConfigurationManager.AppSettings["Creator_VirtualPath"];
            string ecnHomevp = System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"];

			string output="<table cellpadding=0 cellspacing=0><tr><td nowrap>";
			output+="<A class=menu href="+ecnHomevp+"/main/>Home</A>&nbsp;&nbsp;&nbsp;";
			switch (menucode) {
				case "pages":
					//output+="<A class=menu href="+vp+"/main/>.Creator Home</A>&nbsp;&nbsp;&nbsp;";
					output+="<A class=menu href="+vp+"/main/headerfooter>Header & Footer</A>&nbsp;&nbsp;&nbsp; "+
						"<A class=menu2 href="+vp+"/main/pages>Content & Pages</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/folders>Folders</A>&nbsp;&nbsp;&nbsp;"+
						//"<A class=menu href="+vp+"/main/menus>Menus</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/events>Events</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/media>Media</A>&nbsp;&nbsp;&nbsp;";
				break;
				case "headerfooter":
					//output+="<A class=menu href="+vp+"/main/>.Creator Home</A>&nbsp;&nbsp;&nbsp;";
					output+="<A class=menu2 href="+vp+"/main/headerfooter>Header & Footer</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/pages>Content & Pages</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/folders>Folders</A>&nbsp;&nbsp;&nbsp;"+
						//"<A class=menu href="+vp+"/main/menus>Menus</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/events>Events</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/media>Media</A>&nbsp;&nbsp;&nbsp;";
					break;
				case "folders":
					//output+="<A class=menu href="+vp+"/main/>.Creator Home</A>&nbsp;&nbsp;&nbsp;";
					output+="<A class=menu href="+vp+"/main/headerfooter>Header & Footer</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/pages>Content & Pages</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu2 href="+vp+"/main/folders>Folders</A>&nbsp;&nbsp;&nbsp;"+
						//"<A class=menu href="+vp+"/main/menus>Menus</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/events>Events</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/media>Media</A>&nbsp;&nbsp;&nbsp;";
					break;
				case "events":
					//output+="<A class=menu href="+vp+"/main/>.Creator Home</A>&nbsp;&nbsp;&nbsp;";
					output+="<A class=menu href="+vp+"/main/headerfooter>Header & Footer</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/pages>Content & Pages</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/folders>Folders</A>&nbsp;&nbsp;&nbsp;"+
						//"<A class=menu href="+vp+"/main/menus>Menus</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu2 href="+vp+"/main/events>Events</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/media>Media</A>&nbsp;&nbsp;&nbsp;";
				break;
				case "media":
					//output+="<A class=menu href="+vp+"/main/>.Creator Home</A>&nbsp;&nbsp;&nbsp;";
					output+="<A class=menu href="+vp+"/main/headerfooter>Header & Footer</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/pages>Content & Pages</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/folders>Folders</A>&nbsp;&nbsp;&nbsp;"+
						//"<A class=menu href="+vp+"/main/menus>Menus</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/events>Events</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu2 href="+vp+"/main/media>Media</A>&nbsp;&nbsp;&nbsp;";
				break;
				case "menu":
					//output+="<A class=menu href="+vp+"/main/>.Creator Home</A>&nbsp;&nbsp;&nbsp;";
					output+="<A class=menu href="+vp+"/main/headerfooter>Header & Footer</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/pages>Content & Pages</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/folders>Folders</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu2 href="+vp+"/main/menus>Menus</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/events>Events</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/media>Media</A>&nbsp;&nbsp;&nbsp;";
					break;
				default:
					//output+="<A class=menu2 href="+vp+"/main/>.Creator Home</A>&nbsp;&nbsp;&nbsp;";
					output+="<A class=menu href="+vp+"/main/headerfooter>Header & Footer</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/pages>Content & Pages</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/folders>Folders</A>&nbsp;&nbsp;&nbsp;"+
						//"<A class=menu href="+vp+"/main/menus>Menus</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/events>Events</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=menu href="+vp+"/main/media>Media</A>&nbsp;&nbsp;&nbsp;";
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

		public string SubMenu(string menucode, string ActiveMenu) 
		{
            string vp = System.Configuration.ConfigurationManager.AppSettings["Creator_VirtualPath"];

			string output="<table cellpadding=0 cellspacing=0><tr><td nowrap>";
			switch (menucode) {
				case "index":
					output+=" ";
					break;
				case "pages":
					output+=
						"<A class=" + getClass("content and page list", ActiveMenu) + " href=default.aspx>content and page list</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=" + getClass("add new content", ActiveMenu) + " href=contentdetail.aspx?chID="+sc.BasechannelID()+"&cuID="+sc.CustomerID()+">add new content</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=" + getClass("add new page", ActiveMenu) + " href=pagedetail.aspx>add new page</A>";
					break;
				case "headerfooter":
					output+=
						"<A class=" + getClass("Header-Footer list", ActiveMenu) + " href=default.aspx>Header-Footer list</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=" + getClass("add new Header-Footer", ActiveMenu) + " href=headerfooterdetail.aspx?chID="+sc.BasechannelID()+"&cuID="+sc.CustomerID()+">add new Header-Footer</A>";
					break;
				case "menu":
					output+=
						"<A class=" + getClass("menu list", ActiveMenu) + " href=default.aspx>menu list</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=" + getClass("add new menu", ActiveMenu) + " href=menudetail.aspx>add new menu</A>";
					break;	
				case "events":
					output+=
						"<A class=" + getClass("event list", ActiveMenu) + " href=default.aspx>event list</A>&nbsp;&nbsp;&nbsp;"+
						"<A class=" + getClass("add new event", ActiveMenu) + " href=eventdetail.aspx>add new event</A>";
					break;											
				case "login":
					output+="";
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
