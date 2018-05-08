using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ecn.publisher.main.Edition
{
	public partial class Preview : System.Web.UI.Page
	{
		StringBuilder sbBookmark = new StringBuilder("");
		string Publicationtype = string.Empty;
		private int getEditionID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["eID"].ToString());
			}
			catch
			{
				return 0;
			}
		}

        private string _Theme = "default";
        public string DETheme
        {
            get
            {
                return "Themes/" + _Theme;
            }

            set { _Theme = value; }
        }

        private ECN_Framework_BusinessLayer.Application.ECNSession _usersession = null;
        public ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                if (_usersession == null)
                    _usersession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

                return _usersession;
            }
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
            int CustomerID = 0;
            int ChannelID = 0;

			//Ajax.Utility.RegisterTypeForAjax(typeof(Preview)); 

			lblEditionID.Attributes.Add("style","display:none");
            lblIsSecured.Attributes.Add("style", "display:none");
            lblIsSecured.Text = "0";
			if (!IsPostBack)
			{

				txtpageno.Attributes.Add("onKeyPress","if (event.keyCode < 48 || event.keyCode > 57) event.returnValue = false;");
				txtpageno.Attributes.Add("style","FONT-SIZE: 12px; FLOAT: left; FONT-FAMILY: Arial, Helvetica, sans-serif; TEXT-ALIGN: center;width:25px;");
				txtpageno.Text = "1" ;

				if ( getEditionID() > 0)
				{

                    ECN_Framework_Entities.Publisher.Edition ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(getEditionID(), UserSession.CurrentUser);

                    if (ed != null)
                    {
                        ECN_Framework_Entities.Publisher.Publication pb = new ECN_Framework_Entities.Publisher.Publication();

                        pb = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(ed.PublicationID, UserSession.CurrentUser);

                        CustomerID = pb.CustomerID;
                        ChannelID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CustomerID, false).BaseChannelID.Value;
                        _Theme = ed.Theme == string.Empty ? "default" : ed.Theme;
                        download.HRef = ConfigurationManager.AppSettings["ImagePath"] + "/ecn.images/customers/" + pb.CustomerID + "/publisher/" + ed.EditionID + '/' + ed.FileName;

                        string[] KMPSLogoChannels = ConfigurationManager.AppSettings["KMPSLogoChannels"].ToString().Split(',');

                        //if (ChannelID == 16 || ChannelID == 39)
                        foreach (string id in KMPSLogoChannels)
                        {
                            if (ChannelID.ToString() == id)
                                hlpowered.ImageUrl = "http://www.ecndigitaledition.com/images/powered-by-KMPS.jpg";
                        }

                        if (CustomerID == 2022)
                        {
                            hlpowered.ImageUrl = "http://www.ecndigitaledition.com/themes/blue/powered-by-AMA-blue.jpg";
                            hlpowered.NavigateUrl = "http://www.mplanet2009.com/home.shtml";
                        }


                        lblEditionID.Text = getEditionID().ToString();
                        lblTotalPages.Text = ed.Pages.ToString();
                        Publicationtype = pb.PublicationType == string.Empty ? "Magazine" : pb.PublicationType;


                        hlLogo.Target = "_blank";
                        if (pb.LogoURL == string.Empty)
                        {

                            hlLogo.Text = "<img src='http://www.ecndigitaledition.com/images/km-logo-plate.gif' border='0'>";
                            hlLogo.NavigateUrl = "http://www.knowledgemarketing.com";
                        }
                        else
                        {
                            hlLogo.Text = "<img src='" + pb.LogoURL + "' border='0'>";
                            if (pb.LogoLink != string.Empty)
                                hlLogo.NavigateUrl = pb.LogoLink;

                        }

                        if (pb.EnableSubscription.Value)
                            phSubscribe.Visible = true;
                        else
                            phSubscribe.Visible = false;


                        if (pb.ContactFormLink == string.Empty)
                        {
                            phContact.Controls.Add(new LiteralControl("<a href=\"javascript:infoSwitch('Contact');\" class=\"infoBtn\" id=\"infoLinkContact\"><span>&raquo;</span>&nbsp;Contact Us</a>"));


                            if (pb.ContactEmail != string.Empty)
                            {
                                lnkContactEmail.NavigateUrl = "mailto:" + pb.ContactEmail;
                                lnkContactEmail.Text = pb.ContactEmail;
                            }

                            if (pb.ContactPhone != String.Empty)
                            {
                                pnlContactPhone.Visible = true;
                                lblContactPhone.Text = pb.ContactPhone;
                            }
                            else
                            {
                                pnlContactPhone.Visible = false;
                            }

                            if (pb.ContactAddress1 != String.Empty || pb.ContactAddress2 != String.Empty)
                            {
                                pnlContactAddress.Visible = true;
                                lblContactAddress1.Text = pb.ContactAddress1;
                                lblContactAddress2.Text = pb.ContactAddress2;
                            }
                            else
                            {
                                pnlContactAddress.Visible = false;
                            }


                            if (pb.ContactFormLink != string.Empty)
                            {
                                imgContactLogo.ImageUrl = pb.ContactFormLink;
                                imgContactLogo.Visible = true;
                            }
                        }
                        else
                        {
                            phContact.Controls.Add(new LiteralControl("<a href=\"" + pb.ContactFormLink + "\" target=\"_blank\" class=\"infoBtn\" id=\"infoLinkContact\"><span>&raquo;</span>&nbsp;Contact Us</a>"));
                        }
                        DisplayPage();
                    }
				}
				else
				{
                    throw new ECN_Framework_Common.Objects.SecurityException();
				}
			}
		}
        
        private void DisplayPage()
		{
			pnlEdition.Visible = true;

			ClientScript.RegisterStartupScript( typeof(System.Web.UI.Page), "loadfunctions","<script language='javascript'>onload=function(){Edition.Init();renderTree();};</script>");

			if (Publicationtype.ToLower().Equals("1flyer"))
			{
				nav2Page.Visible = false;
				nav.Visible=false;
				plsbSinglePg.Visible=true;
				plsbDoublePg.Visible=false;
			}
			else if (Publicationtype.ToLower().Equals("2flyer"))
			{
				nav1Page.Visible = false;
				nav.Visible=false;
				plsbSinglePg.Visible=false;
				plsbDoublePg.Visible=true;
			}
			else
			{
				nav1Page.Visible = false;
				nav2Page.Visible = false;
				plsbSinglePg.Visible=true;
				plsbDoublePg.Visible=true;
			}
							
			LoadBookmark(getEditionID());
		}

		StringBuilder sbTOC = new StringBuilder();

		private void LoadBookmark(int editionID)
		{
            string TableofContents = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(editionID, UserSession.CurrentUser).xmlTOC;
                
			if (TableofContents != string.Empty)
			{
				sbTOC.Append("var arrNodes =[");

				//load XML
				XmlDocument xmldoc = new XmlDocument();
				xmldoc.LoadXml("<xml>" + TableofContents + "</xml>");
			
				// Load Bookmarks
				XmlNodeList nBookmarks = xmldoc.SelectNodes("//xml/bookmark");


				if (nBookmarks.Count == 1)
				{
					XmlNode  xNode = nBookmarks.Item(0);
					if (xNode.HasChildNodes)
					{
						sbTOC.Append("['" +  xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")',,'folder'],");
						RecurseBookmark(xNode);
						sbTOC.Append("]");
					}
					else
					{
						sbTOC.Append("['" +  xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")']");
					}
				}
				else
				{
					sbTOC.Append("['Front Cover', ['javascript:Edition.gotoPage(1)'],");
					sbTOC.Append("[");
					for(int x=0; x<=nBookmarks.Count-1; x++) 
					{
						
						XmlNode xNode = nBookmarks.Item(x);

						if (xNode.HasChildNodes)
						{
							sbTOC.Append("['" +  xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")',,'folder'],");
							RecurseBookmark(xNode);
						}
						else
						{
							sbTOC.Append("['" +  xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")',,]");
						}

						if (x == nBookmarks.Count-1) 
							sbTOC.Append("]");
						else
							sbTOC.Append("],");
					}
					sbTOC.Append("]]");
				}
				sbTOC.Append("]");
			}
			else
			{
				sbTOC.Append("var arrNodes = '';");
				pltoc.Visible =false;
			}

			Response.Write("<script language=\"JavaScript\" type=\"text/javascript\">" + sbTOC.ToString() + "</script>");

		}

		
		public void RecurseBookmark(XmlNode xmlNode)
		{
			XmlNode xNode;
			XmlNodeList xNodeList;

			if (xmlNode.HasChildNodes) //The current node has children
			{
				sbTOC.Append("[");
				xNodeList = xmlNode.ChildNodes;

				for(int x=0; x<=xNodeList.Count-1; x++) 
				{
					xNode = xmlNode.ChildNodes[x];

					if (xNode.HasChildNodes)
					{
						sbTOC.Append("['" +  xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")',,'folder'],");
						RecurseBookmark(xNode);
					}
					else
					{
						sbTOC.Append("['" +  xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")',,]");
					}

					if (x==xNodeList.Count-1)
						sbTOC.Append("]");
					else
						sbTOC.Append("],");

				}
				sbTOC.Append("],");
			}
		}


		# region AJAX Calls

        [System.Web.Services.WebMethod()]
		public static string GetEditionProps(int editionID)
		{
			StringBuilder sb = new StringBuilder("");

            ECN_Framework_Entities.Publisher.Edition ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(editionID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            ECN_Framework_Entities.Publisher.Publication pb = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(ed.PublicationID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

			sb.Append("this.tp=\""+ ed.Pages + "\";this.path=\"" +ConfigurationManager.AppSettings["ImagePath"] + "/ecn.images/customers/" + pb.CustomerID + "/publisher/" + editionID + '/' + "\";this.type=\""+  pb.PublicationType.ToLower() + "\";this.EmailID=\"\";this.BlastID=\"\";this.IP=\"\";this.SessionID=\"\";");
			
			return sb.ToString();
		}

        [System.Web.Services.WebMethod()]
        public static string GetLinks(int editionID, string Pageno)
		{
			string lnks = "";

			StringBuilder sblinks = new StringBuilder("");

            List<ECN_Framework_Entities.Publisher.Page> page = ECN_Framework_BusinessLayer.Publisher.Page.GetByEditionID(editionID, Pageno.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);

            if (page.Count > 0)
            {
                var xEle = new XElement("Links",
                            from p in page
                            select new XElement("Image",

                                    new XAttribute("PageID", p.PageID),
                                    new XAttribute("Height", p.Height),
                                    new XAttribute("PageNo", p.PageNumber),

                            from l in p.LinkList
                            select new XElement("Link",

                                    new XAttribute("LinkID", l.LinkID),
                                    new XAttribute("type", l.LinkType),
                                    new XAttribute("Alias", l.Alias),
                                    new XAttribute("x1", l.x1),
                                    new XAttribute("y1", l.y1),
                                    new XAttribute("x2", l.x2),
                                    new XAttribute("y2", l.y2),
                                    new XText(l.LinkURL)
                                    )));
                lnks = xEle.ToString();
            }
            else
            {

                lnks = "<Links></Links>";
            }

			return lnks;
		}

        [System.Web.Services.WebMethod()]
        public static string Search(int editionID, string searchText)
		{
			StringBuilder sb = new StringBuilder("");
			StringBuilder xml = new StringBuilder("");
			
			string nodeVal ="";
			int start;
			int at;
			int end;
			int icount = 0;

			if (searchText != string.Empty)
			{
                List<ECN_Framework_Entities.Publisher.Page> page = ECN_Framework_BusinessLayer.Publisher.Page.GetByEditionID(editionID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                
                var xEle = new XElement("xml",
                            from p in page
                            select new XElement("Page",
                                    new XAttribute("id", p.PageNumber),
                                    new XAttribute("displayno", p.DisplayNumber),
                                    new XText("<![CDATA[" + p.TextContent + "]]>")));

                xml.Append(xEle.ToString());

				XmlDocument xmldoc = new XmlDocument();
				xmldoc.LoadXml(xml.ToString());

				XPathNavigator xNav;
				XPathNodeIterator xItr;

				xNav = xmldoc.CreateNavigator();

				string xpath = "";
				
				//if (getCS) 
				//	xpath = "//Page[contains(., '" + searchText + "')]";
				//else
				xpath = "//Page[contains(translate(.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), translate('" + searchText + "','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))]";

				xItr = xNav.Select(xpath);

				while (xItr.MoveNext())
				{
					nodeVal = xItr.Current.Value;	
					end = nodeVal.Length;
					start = 0;
					at = 0;

					sb.Append("<span class='pageNumLink'><a href='javascript:Edition.gotoPage("+ xItr.Current.GetAttribute("id", "") + ")' style='text-decoration:underline;'> Page: " + xItr.Current.GetAttribute("displayno", "") + "</a></span><BR>");
					while((start <= end) && (at > -1))
					{
						at = nodeVal.ToLower().IndexOf(searchText.ToLower(), start);

						if (at == -1) break;
						icount++;
						int s1 = ((nodeVal.LastIndexOf(" ", at) == -1)?at:(nodeVal.LastIndexOf(" ", at)));

						string next50chars = nodeVal.Substring(s1, (nodeVal.Length-s1>50?50:nodeVal.Length-s1));

						int e1 = next50chars.LastIndexOf(" ")>next50chars.LastIndexOf(".")?next50chars.LastIndexOf(" "):next50chars.LastIndexOf(".");
							
						if (e1<=0)
							e1 = searchText.Length;

						sb.Append("<div class='searchList'><a class='lbSearchText' href='javascript:Edition.gotoPage("+ xItr.Current.GetAttribute("id", "") + ")'>&bull;&nbsp;" + nodeVal.Substring(s1, e1) + "</a></div>");
						start = at+1;
					}
				}
				
			}

			if (sb.ToString() != string.Empty)
			{
				return "<table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td width='90%'><span class='searchHeader'><b>Total Instance found : " + icount+ "</b></span></td><td>&nbsp;</td></tr><tr><td colspan='2'><HR></td></tr><tr><td colspan='2'><font size='2'>" + sb.ToString() + "</font></td></table>";
			}
			else
			{
				return "<table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td width='90%'><span class='searchHeader'><b>Total Instance found : 0</b></span></td><td>&nbsp;</td></tr><tr><td colspan='2'><HR></td></tr><tr><td colspan='2'><span class='searchHeader'>No matches!</span></td></table>";

			}
		}		

		#endregion
	}
}

