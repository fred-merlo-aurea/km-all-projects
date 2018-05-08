using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_Common.Functions;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using AccountsCustomer = ECN_Framework_Entities.Accounts.Customer;
using CommonStringFunctions = KM.Common.StringFunctions;
using CommunicatorBlastLink = ECN_Framework_BusinessLayer.Communicator.BlastLink;
using CommunicatorGroup = ECN_Framework_BusinessLayer.Communicator.Group;
using CommunicatorLayout = ECN_Framework_BusinessLayer.Communicator.Layout;
using CommunicatorUniqueLink = ECN_Framework_BusinessLayer.Communicator.UniqueLink;
using EnumsServices = KMPlatform.Enums.Services;
using EnumsServiceFeatures = KMPlatform.Enums.ServiceFeatures;
using static ECN_Framework_BusinessLayer.Accounts.Customer;

namespace ecn.common.classes
{
    public class TemplateFunctions
    {
        private const string SlashChar = "/";
        private const string DashChar = "-";
        private const string DotChar = ".";
        private const string Placeholder = "{0}";
        private const string LinkWriterStatsStartHeader = "Starting TemplateFunctions.LinkReWriter: ";
        private const string LinkWriterStatsEndHeader = "Ending TemplateFunctions.LinkReWriter: ";
        private const string LinkWriterTextStatsStartHeader = "Starting TemplateFunctions.LinkReWriterText: ";
        private const string LinkWriterTextStatsEndHeader = "Ending TemplateFunctions.LinkReWriterText: ";
        private const string OpenClickUseOldSiteConfigurationKey = "OpenClick_UseOldSite";
        private const string MvcActivityDomainPathConfigurationKey = "MVCActivity_DomainPath";
        private const string ImageDomainPathConfigurationKey = "Image_DomainPath";
        private const string SubscriptionManagementKey = "ECN.SUBSCRIPTIONMGMT";
        private const string OpenClickOldSiteRedirectPage = "/engines/linkfrom.aspx";
        private const string MvcActivityRedirectPage = "/Clicks/";
        private const string ActivityDomainPathConfigurationKey = "Activity_DomainPath";
        private const string HrefMailToTag = "href=\"mailto:";
        private const string HrefMailToTagVariation1 = "href='mailto:";
        private const string HrefMailToTagVariation2 = "href=mailto:";
        private const string GroupNamePlaceholder = "%%groupname%%";
        private const string HostNamePlaceholder = "%%hostname%%";
        private const string UnsubscribeLinkPlaceholder = "%%unsubscribelink%%";
        private const string PublicViewPlaceholder = "%%publicview%%";
        private const string ReportAbuseLinkPlaceholder = "%%reportabuselink%%";
        private const string EmailToFriendPlaceholder = "%%emailtofriend%%";
        private const string CompanyAddressPlaceholder = "%%company_address%%";
        private const string EmailFromAddressPlaceholder = "%%EmailFromAddress%%";
        private const string BlastStartTimePlaceholder = "%%blast_start_time%%";
        private const string BlastEndTimePlaceholder = "%%blast_end_time%%";
        private const string BlastIdPlaceholder = "%%blast_id%%";
        private const string SubscribeLinkPlaceholder = "%%sub_link%%";
        private const string EmailFriendPlaceholder = "%%email_friend%%";   
        private const string F2FNotesPlaceholder = "%%F2FNotes%%";
        private const string ProfilePreferencesPlaceholder = "%%profilepreferences%%";
        private const string UserProfilePreferencesPlaceholder = "%%userprofilepreferences%%";
        private const string ListProfilePreferencesPlaceholder = "%%listprofilepreferences%%";
        private const string F2FnotesSessionKey = "F2FNotes";
        private const string HttpTag = "<http://";
        private const string HttpsTag = "<https://";
        private const string LinksMatchPatternTemplate = "([\"']){0}[^\"']*?lid=(\\d+)(?:&ulid=(\\d+))?&l=.*?\\1";
        private const string LinksMatchPattern = @"(<[^link][^>]*?href\s*?=\s*?[""']?)(https?://)";
        private const string RedirectLinkMatchPattern = "<{redirectPageLink}[^>]*?lid=(\\d+)&l=.*?>";
        private const string LinkReplacementTemplate = @"$1{0}?b={1}&e=ECN_EmailID&l=$2";
        private const string RedirectLinkReplacementTemplate = @"<{0}%%ECN_Encrypt_$1%%>";
        private const string HttpReplacementTemplate = "<{0}?b={1}&e=ECN_EmailID&l=http";
        private const string MailToReplacementTemplate = "href=\"{0}?b={1}&e=%%EmailID%%&l=mailto:";
        private const string PublicViewPageLinkTemplate = 
            "{0}/engines/publicPreview.aspx?blastID={1}&emailID=%%EmailID%%";
        private const string ReportAbusePageLinkTemplate = 
            "{0}/engines/reportspam.aspx?p=%%EmailAddress%%,%%EmailID%%,{1},{2},{3}";
        private const string UnsubscribePageLinkTemplate = 
            "{0}/engines/Unsubscribe.aspx?e=%%EmailAddress%%&g=%%GroupID%%&b={1}&c={2}&s=U&f=html";
        private const string EmailToFriendPageLinkTemplate = 
            "{0}/engines/emailtofriend.aspx?e=%%EmailID%%&b={1}";
        private const string SubscribeLinkTemplate =
            "{0}/engines/subscribe.aspx?&e=%%EmailAddress%%&g=%%GroupID%%&b={1}&c={2}&s=S&f=html";
        private const string ImagePathTemplate = "{0}/channels/{1}/images/";
        private const string ListProfilePreferencesPageTemplate =
                    "<a href='{0}/engines/managesubscriptions.aspx?e=%%EmailAddress%%,%%EmailID%%&prefrence=both'>" +
                    "<img border=\"0\" src=\"{1}/list_email_pref.gif\"></a>&nbsp;";
        private const string ListProfilePreferencesTextPageTemplate =
                    "Email List and Profile Preferences: {0}/engines/managesubscriptions.aspx?" +
                    "e=%%EmailAddress%%,%%EmailID%%&prefrence=both";
        private const string UserProfilePreferencesPageTemplate =
                    "<a href='{0}/engines/managesubscriptions.aspx?e=%%EmailAddress%%,%%EmailID%%,{1}&" +
                    "prefrence=email'><img border=\"0\" src=\"{2}/email_pref.gif\"></a>&nbsp;";
        private const string UserProfilePreferencesTextPageTemplate =
                    "Email Profile Preferences:{0}/engines/managesubscriptions.aspx?" +
                    "e=%%EmailAddress%%,%%EmailID%%,{1}&prefrence=email";
        private const string ProfilePreferencesPageTemplate =
                    "<a href='{0}/engines/managesubscriptions.aspx?e=%%EmailAddress%%,%%EmailID%%" +
                    "&prefrence=list'>Manage my Subscriptions</a>&nbsp;";
        private const string ProfilePreferencesTextPageTemplate = 
                    "List Preferences: {0}/engines/managesubscriptions.aspx?" +
                    "e=%%EmailAddress%%,%%EmailID%%&prefrence=list";
        private const string SubManagementPageTemplate = 
                    "<a href='{0}/engines/subscriptionmanagement.aspx?smid={1}&e=%%EmailAddress%%'>Unsubscribe</a>";
        private const string SubManagementTextPageTemplate = 
                    "Unsubscribe: {0}/engines/subscriptionmanagement.aspx?smid={1}&e=%%EmailAddress%%";
        private const string EcnIdMatchingPattern = @"ecn_id=([""']?)(.*?)\1";
        private const string LinkReplacePattern = @"<$1 $2""$4"" $6";
        private const string UniqueLinkParamsTemplate = "&lid={0}&ulid={1}&l={2}";
        private const string LinkParamsTemplate = "&lid={0}&l={1}";
        private const string LinkParamTemplate = "&l={0}";
        private const string LinkIdParam = "&lid=";
        private const string LinkMatchingPattern =
            @"<\s*               # any whitespace character
            ([^>]*)              # any character that doesn't close the tag
            (href\s*=\s*)        # followed by href=, with spaces allowed on either side of the equal sign
            ([""'])              # followed by open single or double quote
            ([^>]*?&l=([^>]*?))  # followed by some text that doesn't close the tag and does contain &l=<stuff>
            \3                   # followed by a close quote matching the prior open quote type
            ([^>]*>)             # followed by some more stuff and then a close tag";

        private static readonly string HeaderQueryTemplate =
            $"SELECT HeaderSource FROM {accountsdb}.dbo.CustomerTemplate WHERE CustomerID={0} " +
            $"AND TemplateTypeCode='F2FIntroEmailHdr' and IsActive=1 and IsDeleted = 0";

        private static readonly IReadOnlyDictionary<string, Func<AccountsCustomer, string>> LinkRewriteMappings =
            new Dictionary<string, Func<AccountsCustomer, string>>
            {
                ["%%customer_name%%"] = (customer) => customer.CustomerName,
                ["%%customer_address%%"] = (customer) => 
                    $"{customer.Address}, {customer.City}, {customer.State} - {customer.Zip}",
                ["%%customer_webaddress%%"] = (customer) => customer.WebAddress,
                ["%%customer_udf1%%"] = (customer) => customer.customer_udf1,
                ["%%customer_udf2%%"] = (customer) => customer.customer_udf2,
                ["%%customer_udf3%%"] = (customer) => customer.customer_udf3,
                ["%%customer_udf4%%"] = (customer) => customer.customer_udf4,
                ["%%customer_udf5%%"] = (customer) => customer.customer_udf5,
                ["http://%%unsubscribelink%%/"] = (customer) => "%%unsubscribelink%%",
                ["http://%%emailtofriend%%/"] = (customer) => "%%emailtofriend%%",
                ["http://%%publicview%%/"] = (customer) => "%%publicview%%",
                ["http://%%reportabuselink%%/"] = (customer) => "%%reportabuselink%%",
                ["%%publicview%%/"] = (customer) => "%%publicview%%",
                ["http://%%unsubscribelink%%"] = (customer) => "%%unsubscribelink%%",
                ["http://%%emailtofriend%%"] = (customer) => "%%emailtofriend%%",
                ["http://%%publicview%%"] = (customer) => "%%publicview%%",
                ["http://%%reportabuselink%%"] = (customer) => "%%reportabuselink%%"
            };

        public static ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
        public static ArrayList menuIDs = new ArrayList();        

        public static string getWebFeed(string feedurl)
        {
            string body = "";
            try
            {
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(feedurl);
                HttpWebResponse ws = (HttpWebResponse)wr.GetResponse();
                Stream str = ws.GetResponseStream();
                StreamReader sr = new StreamReader(str);
                string line = " ";
                while (line != null)
                {
                    body += line;
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception: " + ex);
            }
            return body;
        }

        public static string SelectTemplate(string CustomerID, string template_type, string column)
        {
            string column_source;

            // Figure out if they want the header or the footer
            if ("footer" == column)
            {
                column_source = "FooterSource";
            }
            else if ("header" == column)
            {
                column_source = "HeaderSource";
            }
            else
            {
                // default header
                column_source = "HeaderSource";
            }
            // Get the template for this item...
            string header = GetCustomerTemplate(CustomerID, template_type, column_source);
            if ("" == header)
            {
                object ptr = DataFunctions.ExecuteScalar("select MasterCustomerID from " +
                    accountsdb + ".dbo.BaseChannel bc, " +
                    accountsdb + ".dbo.Customer cu where" +
                    " cu.CustomerID = " + CustomerID + " and bc.BaseChannelID = cu.BaseChannelID and cu.IsDeleted = 0 and bc.IsDeleted = 0");
                if (null != ptr)
                {
                    CustomerID = ptr.ToString();
                    header = GetCustomerTemplate(CustomerID, template_type, column_source);
                }
            }
            // No chanel set.. use Teckman default
            if ("" == header)
            {
                header = GetCustomerTemplate("1", template_type, column_source);
            }
            return header;
        }

        public static string GetCustomerTemplate(string CustomerID, string template_type, string column)
        {
            string TemplateQuery =
                "SELECT " + column + " FROM " + accountsdb + ".dbo.CustomerTemplate WHERE CustomerID = " + CustomerID + " AND TemplateTypeCode='" + template_type + "' and IsActive=1 and IsDeleted = 0";
            object ptr = DataFunctions.ExecuteScalar(TemplateQuery);
            if (null == ptr) return "";
            return ptr.ToString();
        }

        public static string GetCustomerTemplateHeader(string CustomerID, string template_type)
        {
            string TemplateQuery =
                "SELECT HeaderSource FROM " + accountsdb + ".dbo.CustomerTemplate WHERE CustomerID = " + CustomerID + " AND TemplateTypeCode='" + template_type + "' and IsActive=1 and IsDeleted = 0";
            object ptr = DataFunctions.ExecuteScalar(TemplateQuery);
            if (null == ptr) return "";
            return ptr.ToString();
        }
        public static string GetCustomerTemplateFooter(string CustomerID, string template_type)
        {
            string TemplateQuery =
                "SELECT FooterSource FROM " + accountsdb + ".dbo.CustomerTemplate WHERE CustomerID = " + CustomerID + " AND TemplateTypeCode='" + template_type + "' and IsActive=1 and IsDeleted = 0";
            object ptr = DataFunctions.ExecuteScalar(TemplateQuery);
            if (null == ptr) return "";
            return ptr.ToString();
        }
        public static string BreadCrumb(string MenuPage, string ContextPath)
        {
            string displayOut = "";
            string front = "";
            string back = "";

            displayOut += "<font face=verdana size=-1 color=darkBlue><b>";
            try
            {
                string sqlQuery =
                    " SELECT m.MenuName, m.MenuTarget, m.MenuName AS ParentName, p.MenuTarget as ParentTarget " +
                    " FROM Menus s, Menus p " +
                    " WHERE m.MenuCode = '" + MenuPage + "' " +
                    " AND m.ParentID *= p.MenuID; ";
                DataTable dt = DataFunctions.GetDataTable(sqlQuery);
                foreach (DataRow dr in dt.Rows)
                {
                    string SectionName = dr["MenuName"].ToString();
                    string SectionTarget = ContextPath + dr["MenuTarget"].ToString();
                    string ParentName = dr["ParentName"].ToString();
                    string ParentTarget = ContextPath + dr["ParentTarget"].ToString();
                    if (ParentName != null)
                    {
                        front = "> <a class=sidemenu href='" + ParentTarget + "'>";
                        back = "</a>";
                        displayOut += front + ParentName + back;
                    }
                    front = "> <a class=sidemenu href='" + SectionTarget + "'>";
                    back = "</a>";
                    displayOut += front + SectionName + back;
                }
            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            return displayOut;
        }

        public static string channelTemplate(
            string sourcecode, string CurrentUser, string AssetsPath,
            string PageTitle, string MainTitle, string HelpBoxTitle,
            string HelpBoxContent, string MainMenu, string SubMenu, string ChannelName
            )
        {
            string body = sourcecode;
            body = StringFunctions.Replace(body, "%%ChannelName%%", ChannelName);
            body = StringFunctions.Replace(body, "%%HelpBoxTitle%%", HelpBoxTitle);
            body = StringFunctions.Replace(body, "%%HelpBoxContent%%", HelpBoxContent);
            body = StringFunctions.Replace(body, "%%MainTitle%%", MainTitle);
            body = StringFunctions.Replace(body, "%%PageTitle%%", PageTitle);
            body = StringFunctions.Replace(body, "%%MainMenu%%", MainMenu);
            body = StringFunctions.Replace(body, "%%SubMenu%%", SubMenu);
            body = StringFunctions.Replace(body, "%%CurrentUser%%", CurrentUser);
            body = StringFunctions.Replace(body, "%%AssetsPath%%", AssetsPath);
            return body;
        }

        public static string EmailHTMLBody(
            string TemplateSource, string TableOptions,
            int Slot1, int Slot2, int Slot3, int Slot4, int Slot5, int Slot6, int Slot7, int Slot8, int Slot9)
        {
            string body = TemplateSource;
            if (Slot1 > 0)
                body = StringFunctions.Replace(body, "%%slot1%%", DataFunctions.GetContent(Slot1));
            if (Slot2 > 0)
                body = StringFunctions.Replace(body, "%%slot2%%", DataFunctions.GetContent(Slot2));
            if (Slot3 > 0)
                body = StringFunctions.Replace(body, "%%slot3%%", DataFunctions.GetContent(Slot3));
            if (Slot4 > 0)
                body = StringFunctions.Replace(body, "%%slot4%%", DataFunctions.GetContent(Slot4));
            if (Slot5 > 0)
                body = StringFunctions.Replace(body, "%%slot5%%", DataFunctions.GetContent(Slot5));
            if (Slot6 > 0)
                body = StringFunctions.Replace(body, "%%slot6%%", DataFunctions.GetContent(Slot6));
            if (Slot7 > 0)
                body = StringFunctions.Replace(body, "%%slot7%%", DataFunctions.GetContent(Slot7));
            if (Slot8 > 0)
                body = StringFunctions.Replace(body, "%%slot8%%", DataFunctions.GetContent(Slot8));
            if (Slot9 > 0)
                body = StringFunctions.Replace(body, "%%slot9%%", DataFunctions.GetContent(Slot9));
            body = StringFunctions.Replace(body, "®", "&reg;");
            body = StringFunctions.Replace(body, "©", "&copy;");
            body = StringFunctions.Replace(body, "™", "&trade;");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");
            if (TableOptions.Length < 1)
            {
                TableOptions = " cellpadding=0 cellspacing=0 width='100%'";
            }
            else if (!TableOptions.ToLower().Contains("width"))
            {
                TableOptions += " width='100%'";
            }
            body = "<table " + TableOptions + "><tr><td>" + body + "</td></tr></table>";
            return body;
        }

        //WGH - For mobile support
        public static string EmailHTMLBody(
            string TemplateSource, string TableOptions,
            int Slot1, int Slot2, int Slot3, int Slot4, int Slot5, int Slot6, int Slot7, int Slot8, int Slot9, bool IsMobile)
        {
            string body = TemplateSource;
            body = StringFunctions.Replace(body, "%%slot1%%", DataFunctions.GetContent(Slot1, IsMobile));
            body = StringFunctions.Replace(body, "%%slot2%%", DataFunctions.GetContent(Slot2, IsMobile));
            body = StringFunctions.Replace(body, "%%slot3%%", DataFunctions.GetContent(Slot3, IsMobile));
            body = StringFunctions.Replace(body, "%%slot4%%", DataFunctions.GetContent(Slot4, IsMobile));
            body = StringFunctions.Replace(body, "%%slot5%%", DataFunctions.GetContent(Slot5, IsMobile));
            body = StringFunctions.Replace(body, "%%slot6%%", DataFunctions.GetContent(Slot6, IsMobile));
            body = StringFunctions.Replace(body, "%%slot7%%", DataFunctions.GetContent(Slot7, IsMobile));
            body = StringFunctions.Replace(body, "%%slot8%%", DataFunctions.GetContent(Slot8, IsMobile));
            body = StringFunctions.Replace(body, "%%slot9%%", DataFunctions.GetContent(Slot9, IsMobile));
            body = StringFunctions.Replace(body, "®", "&reg;");
            body = StringFunctions.Replace(body, "©", "&copy;");
            body = StringFunctions.Replace(body, "™", "&trade;");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");
            if (TableOptions.Length < 1)
            {
                TableOptions = " cellpadding=0 cellspacing=0 width='100%'";
            }
            else if (!TableOptions.ToLower().Contains("width"))
            {
                TableOptions += " width='100%'";
            }
            body = "<table " + TableOptions + "><tr><td>" + body + "</td></tr></table>";
            return body;
        }

        public static string HTMLPagePreview(string QueryValue, string HeaderCode, string FooterCode, string TemplateSource,
            int Slot1, int Slot2, int Slot3, int Slot4, int Slot5, int Slot6, int Slot7, int Slot8, int Slot9, int customerID)
        {
            string page = "";
            /*	"<html>"+
                "<head>"+
                "<meta http-equiv='Content-Type' content='text/html; charset=windows-1252'>"+
                "<meta name='Keywords' content="+Keywords+">"+
                "</head><body>";*/
            page += "<table border='0' width='100%' cellpadding='0' cellspacing='0'><tr><td>" + HeaderCode + "</td></tr>";
            page += "<tr><td>";

            string body = TemplateSource;
            body = StringFunctions.Replace(body, "%%slot1%%", DataFunctions.GetContent(Slot1));
            body = StringFunctions.Replace(body, "%%slot2%%", DataFunctions.GetContent(Slot2));
            body = StringFunctions.Replace(body, "%%slot3%%", DataFunctions.GetContent(Slot3));
            body = StringFunctions.Replace(body, "%%slot4%%", DataFunctions.GetContent(Slot4));
            body = StringFunctions.Replace(body, "%%slot5%%", DataFunctions.GetContent(Slot5));
            body = StringFunctions.Replace(body, "%%slot6%%", DataFunctions.GetContent(Slot6));
            body = StringFunctions.Replace(body, "%%slot7%%", DataFunctions.GetContent(Slot7));
            body = StringFunctions.Replace(body, "%%slot8%%", DataFunctions.GetContent(Slot8));
            body = StringFunctions.Replace(body, "%%slot9%%", DataFunctions.GetContent(Slot9));
            body = StringFunctions.Replace(body, "®", "&reg;");
            body = StringFunctions.Replace(body, "©", "&copy;");
            body = StringFunctions.Replace(body, "™", "&trade;");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");
            //body = StringFunctions.Replace(body,"%%eventlist%%",EventsList(customerID,"event"));
            //body = StringFunctions.Replace(body,"%%showlist%%",EventsList(customerID,"show"));
            body = StringFunctions.Replace(body, "%%calendareventslist%%", CalendarEventsList(customerID));
            body = StringFunctions.Replace(body, "%%newslist%%", EventsList(customerID, "news"));
            body = StringFunctions.Replace(body, "%%articlelist%%", EventsList(customerID, "article"));
            body = StringFunctions.Replace(body, "%%whatsnewlist%%", EventsList(customerID, "whatsnew"));
            body = StringFunctions.Replace(body, "%%testimoniallist%%", EventsList(customerID, "testimonial"));
            body = StringFunctions.Replace(body, "%%otherlist%%", EventsList(customerID, "other"));

            page += body + "</td></tr>";
            page += "<tr><td>";
            page += FooterCode;
            page += "</td></tr></table>";
            return page;
        }
        public static string HeaderFooterPreview(string HeaderCode, string FooterCode, int customerID)
        {
            string page = "";
            page += "<table border='1' width='100%' cellpadding='0' cellspacing='0'><tr><td align=left><font class='errormsg'>--- HEADER START---</font></td></tr>";
            page += "<tr><td>" + HeaderCode + "</td></tr>";
            page += "<tr><td align=left><font class='errormsg'>--- HEADER END ---</font></td></tr>";

            string body = "<tr><td align=center height=50><font class='errormsg'>--- BODY CONTENT goes here ---</font></td></tr>";

            page += "<tr><td>" + body + "</td></tr>";
            page += "<tr><td align=left><font class='errormsg'>--- FOOTER START---</font></td></tr>";
            page += "<tr><td>" + FooterCode + "</td></tr>";
            page += "<tr><td align=left><font class='errormsg'>--- FOOTER END---</font></td></tr></table>";
            return page;
        }

        public static string MenuPreview(string Source, int customerID)
        {
            string page = "";
            page += "<table border='0' width='100%' cellpadding='0' cellspacing='0'>";
            page += "<tr><td>";

            string body = Source;
            body = StringFunctions.Replace(body, "%%vertical-menu%%", MenuList("", "", customerID));

            page += body;
            page += "</td></tr></table>";
            return page;
        }

        public static string EmailTextBody(string TemplateText,
            int Slot1, int Slot2, int Slot3, int Slot4, int Slot5, int Slot6, int Slot7, int Slot8, int Slot9)
        {
            string body = TemplateText;
            body = StringFunctions.Replace(body, "%%slot1%%", DataFunctions.GetTextContent(Slot1));
            body = StringFunctions.Replace(body, "%%slot2%%", DataFunctions.GetTextContent(Slot2));
            body = StringFunctions.Replace(body, "%%slot3%%", DataFunctions.GetTextContent(Slot3));
            body = StringFunctions.Replace(body, "%%slot4%%", DataFunctions.GetTextContent(Slot4));
            body = StringFunctions.Replace(body, "%%slot5%%", DataFunctions.GetTextContent(Slot5));
            body = StringFunctions.Replace(body, "%%slot6%%", DataFunctions.GetTextContent(Slot6));
            body = StringFunctions.Replace(body, "%%slot7%%", DataFunctions.GetTextContent(Slot7));
            body = StringFunctions.Replace(body, "%%slot8%%", DataFunctions.GetTextContent(Slot8));
            body = StringFunctions.Replace(body, "%%slot9%%", DataFunctions.GetTextContent(Slot9));
            body = StringFunctions.Replace(body, "®", "");
            body = StringFunctions.Replace(body, "©", "");
            body = StringFunctions.Replace(body, "™", "");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");
            return body;
        }

        private static string GetString(object obj)
        {
            string output = "";
            try
            {
                if ((obj == System.DBNull.Value) || (obj == null))
                {
                    output = "<br>&nbsp;<br>";
                }
                else
                {
                    if (Convert.ToString(obj) == "")
                    {
                        output = "<br>&nbsp;<br>";
                    }
                    else
                    {
                        output = Convert.ToString(obj);
                    }
                }
            }
            catch (Exception)
            {
                output = "<br>&nbsp;<br>";
            }
            return output;
        }

        #region Events List methods
        private static string EventListTable(DataRow[] dr, string eventType)
        {
            int i = 1;
            if (dr.Length > 0)
            {
                string output =
                    "<div style=\"clear:both;\"><TABLE id=\"NonCalTable\" borderColor=\"#000033\" cellSpacing=\"1\" borderColorDark=\"#000066\" cellPadding=\"1\" width=\"300\" align=\"left\" bgColor=\"#ffffcc\" border=\"1\"" +
                    " style=\"FONT-SIZE: 9pt; FONT-FAMILY: Verdana; HEIGHT: 55px\">" +
                    "<TR><TD><P align=\"center\"><STRONG>#</STRONG></P></TD>" +
                    "<TD><P align=\"center\"><STRONG>Event Name</STRONG></P></TD>" +
                    "<TD><P align=\"center\"><STRONG>Start Date</STRONG></P></TD>" +
                    "<TD><P align=\"center\"><STRONG>End Date</STRONG></P></TD>" +
                    "<TD><P align=\"center\"><STRONG>Times</STRONG></P></TD>" +
                    "<TD><P align=\"center\"><STRONG>Location</STRONG></P></TD>" +
                    "<TD><P align=\"center\"><STRONG>Description</STRONG></P></TD></TR>";

                string startDate = "";
                string endDate = "";
                foreach (DataRow r in dr)
                {
                    output += "<TR>";
                    output += "<TD>" + i + "</TD>";
                    output += "<TD>" + (string)r["EventName"] + "</TD>";

                    if (r["StartDate"] == System.DBNull.Value)
                        startDate = "<br>&nbsp;<br>";
                    else
                        startDate = Convert.ToDateTime(r["StartDate"]).ToShortDateString();
                    if (r["EndDate"] == System.DBNull.Value)
                        endDate = "<br>&nbsp;<br>";
                    else
                        endDate = Convert.ToDateTime(r["EndDate"]).ToShortDateString();

                    output += "<TD>" + startDate + "</TD>";
                    output += "<TD>" + endDate + "</TD>";
                    output += "<TD>" + StringFunctions.DBString(r["Times"], "<br>&nbsp;<br>") + "</TD>";
                    output += "<TD>" + StringFunctions.DBString(r["Location"], "<br>&nbsp;<br>") + "</TD>";
                    output += "<TD>" + StringFunctions.DBString(r["Description"], "<br>&nbsp;<br>") + "</TD>";
                    output += "</TR>";
                    i++;
                }
                return (output + "</TABLE></div>");
            }
            else
                return ("<div style=\"clear:both;\">There are no entries for <b>" + eventType + " list</b> at this moment</div>");
        }

        public static string CalendarEventsList(int CustomerID)
        {
            try
            {
                string query = "SELECT * FROM events WHERE Displayflag='Y' AND (EventTypeCode='training' OR EventTypeCode='show' OR EventTypeCode='event') AND CustomerID='" + CustomerID + "'";
                DataTable dt = DataFunctions.GetDataTable(query);
                return (EventListCalendar(dt));
            }
            catch (Exception err)
            {
                string dvnull = err.Message;
                return (" ");
            }
        }

        public static string EventListCalendar(DataTable dt)
        {
            HttpContext.Current.Session["dr"] = dt.Select();
            //if (dt.Select("EndDate>='"+Convert.ToString(DateTime.Today)+"' AND EventTypeCode='"+eventType+"'").Length <= 0)
            //	return ("<div style=\"clear:both;\">There are no entries for <b>"+eventType+" list</b> at this moment</div>");
            //else
            return ("<br><a href=\"#\" onclick=javascript:window.open('EventsCalendar.aspx','EventsCalendar','resizable=yes,width=825,height=720,scrollbars=yes') > Click here to see calendar events list</a><br>");
        }

        public static string EventsList(int CustomerID, string EventType)
        {
            try
            {
                string query = "SELECT * FROM events WHERE Displayflag='Y' AND EventTypeCode='" + EventType + "' AND CustomerID='" + CustomerID + "'";
                DataTable dt = DataFunctions.GetDataTable(query);
                return (EventListTable(dt.Select(), EventType));
            }
            catch (Exception err)
            {
                string dvnull = err.Message;
                return (" ");
            }
        }

        public static string EventsList(string EventType)
        {
            string displayOut = "";
            string theClass = "";

            displayOut += "<table class='eventsContent' width='100%'>";
            try
            {
                string sqlQuery =
                    "SELECT * FROM events " +
                    "WHERE DisplayFlag = 'Y' " +
                    "AND EndDate > getDate() " +
                    "ORDER BY StartDate, EventName;";
                DataTable dt = DataFunctions.GetDataTable(sqlQuery);
                foreach (DataRow dr in dt.Rows)
                {
                    int EventID = Convert.ToInt32(dr["Event"]);
                    string EventTypeCode = dr["EventTypeCode"].ToString();
                    string EventName = dr["EventName"].ToString();
                    string StartDate = dr["StartDate"].ToString();
                    string EndDate = dr["EndDate"].ToString();
                    string Times = dr["Times"].ToString();
                    string Location = dr["Location"].ToString();
                    string Description = dr["Description"].ToString();
                    if (theClass.Equals("tableRow"))
                    {
                        theClass = "tableRowAlt";
                    }
                    else
                    {
                        theClass = "tableRow";
                    }
                    displayOut += "<tr class='tableRowHeader'>";
                    displayOut += "<td align=left colspan=2>" + DataFunctions.CodeValue("EventType", EventTypeCode) + "</td>";
                    displayOut += "</tr>";
                    displayOut += "<tr class='" + theClass + "'><td valign=top align=right width='25%'>";
                    displayOut += "<table class='" + theClass + "'>";
                    displayOut += "<tr><td align=right>Start:</td><td align=right nowrap><b>" + StartDate + "</b></td></tr>";
                    if (!StartDate.Equals(EndDate))
                    {
                        displayOut += "<tr><td align=right>End:</td><td align=right nowrap><b>" + EndDate + "</b></td></tr>";
                    }
                    displayOut += "<tr><td colspan=2 nowrap><i>" + Times + "</i></td></tr>";
                    displayOut += "</table></td>";
                    displayOut += "<td valign=top><b>" + EventName + "</b> [" + Location + "]<BR>" + DataFunctions.MediaValue("EventID", EventID, "link") + "<BR>" + Description + "<BR>" + DataFunctions.MediaValue("EventID", EventID, "logo") + "</td></tr>";
                }
            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            displayOut += "</table>";
            return displayOut;
        }
        #endregion

        public static string LinkReWriter(
            string text, 
            Blast blast, 
            string customerId, 
            string virtualPath, 
            string hostName)
        {
            return LinkReWriter(text, blast, customerId, virtualPath, hostName, string.Empty);
        }

        private static String TextReplaceWithLinkID(String strText, int blastID)
        {
            String strReturn = "";

            ECN_Framework_Entities.Communicator.BlastLink blastLink = null;
            int blastLinkID = 0;
            Regex r;
            Match m;
            r = new Regex(@"((&l=http|&l=https):((//)|(\\\\))+[^>]*)",

            //r = new Regex(@"((www\.|(http|https)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string linkFound = "";
            for (m = r.Match(strText); m.Success; m = m.NextMatch())
            {
                linkFound = m.Groups[1].ToString();

                linkFound = linkFound.Substring(linkFound.IndexOf("&l=") + 3, linkFound.Length - linkFound.IndexOf("&l=") - 3);
                blastLink = ECN_Framework_BusinessLayer.Communicator.BlastLink.GetByLinkURL(blastID, linkFound);
                if (blastLink == null || blastLink.BlastLinkID <= 0)
                {
                    blastLink = new ECN_Framework_Entities.Communicator.BlastLink();
                    blastLink.BlastID = blastID;
                    blastLink.LinkURL = linkFound;
                    blastLinkID = ECN_Framework_BusinessLayer.Communicator.BlastLink.Insert(blastLink);
                    //string findText = "&l=" + linkFound + @"""";
                    //string replaceText = "&lid=" + blastLinkID.ToString() + "&l=" + linkFound + @"""";
                    //strText = strText.Replace(findText, replaceText);
                }
                else
                {
                    blastLinkID = blastLink.BlastLinkID;
                }
                //wgh creating dupe lid entries
                string findText = string.Empty;
                string replaceText = string.Empty;
                if ((strText.IndexOf(("&lid=" + blastLinkID.ToString() + "&l=" + linkFound) + " ") < 0) && (strText.IndexOf(("&lid=" + blastLinkID.ToString() + "&l=" + linkFound) + ">") < 0))
                {
                    findText = "&l=" + linkFound + ">";
                    replaceText = "&lid=" + blastLinkID.ToString() + "&l=" + linkFound + ">";
                    strText = strText.Replace(findText, replaceText);
                }
                if ((strText.IndexOf(("&lid=" + blastLinkID.ToString() + "&l=" + linkFound) + " ") < 0) && (strText.IndexOf(("&lid=" + blastLinkID.ToString() + "&l=" + linkFound) + " ") < 0))
                {
                    findText = "&l=" + linkFound + " ";
                    replaceText = "&lid=" + blastLinkID.ToString() + "&l=" + linkFound + " ";
                    strText = strText.Replace(findText, replaceText);
                }
                if ((strText.IndexOf(("&lid=" + blastLinkID.ToString() + "&l=" + linkFound) + " ") < 0) && (strText.IndexOf(("&lid=" + blastLinkID.ToString() + "&l=" + linkFound) + "\r") < 0))
                {
                    findText = "&l=" + linkFound + "\r";
                    replaceText = "&lid=" + blastLinkID.ToString() + "&l=" + linkFound + "\r";
                    strText = strText.Replace(findText, replaceText);
                }

            }
            strReturn = strText;

            return strReturn;
        }

        private static string HtmlReplaceWithLinkId(string text, int blastId)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var links = GetLinksFromText(ref text);
            if(links == null)
            {
                return text;
            }

            var blastLinks = CommunicatorBlastLink.GetDictionaryByBlastID(blastId);
            var uniqueLinks = CommunicatorUniqueLink.GetDictionaryByBlastID(blastId);
            for (var index = 0; index < links.Count; index++)
            {                
                var linkFound = links[index];
                if (linkFound == null)
                {
                    continue;
                }

                linkFound.lFound = linkFound.lFound?.Trim();
                linkFound.Guid = linkFound.Guid?.Trim();

                var blastLinkId = GetBlastLinkId(blastLinks, linkFound.lFound, blastId);
                var uniqueLinkId = GetUniqueLinkId(uniqueLinks, linkFound.Guid, blastId, blastLinkId);                

                if (uniqueLinkId >= 0)
                {
                    var findText = string.Format(LinkParamTemplate, linkFound.lFound);
                    var replaceText = string.Format(UniqueLinkParamsTemplate, blastLinkId, uniqueLinkId, linkFound.lFound);

                    linkFound.originalA = linkFound.originalA.Replace(findText, replaceText);
                }
                else if (!linkFound.link.Contains(LinkIdParam))
                {
                    var findText = string.Format(LinkParamTemplate, linkFound.lFound);
                    var replaceText = string.Format(LinkParamsTemplate, blastLinkId, linkFound.lFound);

                    linkFound.originalA = linkFound.originalA.Replace(findText, replaceText);
                }

                text = text.Replace(linkFound.aToReplace, linkFound.originalA);
            }

            return text;
        }

        private static IList<LinkWithGuid> GetLinksFromText(ref string text)
        {
            var linkList = new List<LinkWithGuid>();
            if (string.IsNullOrWhiteSpace(text))
            {
                return linkList;
            }

            var linkmatchingRegex = new Regex(
                LinkMatchingPattern,
                RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var index = 0;
            var numberOfGroups = 7;            
            foreach (var match in linkmatchingRegex.Matches(text).OfType<Match>())
            {
                if (match.Groups.Count < numberOfGroups)
                {
                    continue;
                }

                var ecnId = GetEcnId(match);
                var originalTag = match.Groups[0].Value;
                var link = match.Groups[4].Value;
                var editedTag = linkmatchingRegex.Replace(originalTag, LinkReplacePattern);
                var linkFound = match.Groups[5].Value;

                try
                {
                    var href = new LinkWithGuid();
                    href.link = link;
                    href.originalA = editedTag;
                    if (!string.IsNullOrWhiteSpace(ecnId))
                    {
                        href.Guid = ecnId;
                        var toReplace = originalTag.Insert(0, index.ToString());
                        text = text.Replace(originalTag, toReplace);
                        href.aToReplace = toReplace;
                    }
                    else
                    {
                        href.aToReplace = originalTag;
                    }

                    href.lFound = linkFound;

                    if (!linkList.Contains(href))
                    {
                        linkList.Add(href);
                    }
                }
                // Exceptions have been intentionally swallowed because 
                // this should be a non-braking process
                catch
                {
                }

                index++;
            }

            return linkList;
        }

        private static string GetEcnId(Match match)
        {
            Guard.NotNull(match, nameof(match));

            const int numberOfGroups = 7;
            if (match.Groups.Count < numberOfGroups)
            {
                return null;
            }

            var ecnIdMatchingRegex = new Regex(
                EcnIdMatchingPattern, 
                RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var toSearch = $"{(match.Groups[1].Value ?? string.Empty)} {(match.Groups[6].Value ?? string.Empty)}";
            var ecnIdMatch = ecnIdMatchingRegex.Match(toSearch);

            if (ecnIdMatch.Success && ecnIdMatch.Groups.Count >= 3)
            {
                return ecnIdMatch.Groups[2].Value;
            }

            return null;
        }

        private static int GetBlastLinkId(IDictionary<string, int> blastLinks, string link, int blastId)
        {
            var blastLinkId = 0;
            if (blastLinks == null)
            {
                return blastLinkId;
            }

            var blastLinksContainsFoundLink = blastLinks.ContainsKey(link);
            if (blastLinks != null && blastLinksContainsFoundLink)
            {
                blastLinkId = blastLinks[link];
            }

            if (blastLinkId <= 0)
            {
                var blastLink = new BlastLink();
                blastLink.BlastID = blastId;
                blastLink.LinkURL = link;
                blastLinkId = CommunicatorBlastLink.Insert(blastLink);

                if (!blastLinksContainsFoundLink)
                {
                    blastLinks.Add(link, blastLinkId);
                }
            }

            return blastLinkId;
        }

        private static int GetUniqueLinkId(IDictionary<string, int> uniqueLinks, string linkId, int blastId, int blastLinkId)
        {
            var uniqueLinkId = -1;
            if (uniqueLinks == null || string.IsNullOrWhiteSpace(linkId))
            {
                return uniqueLinkId;
            }

            if (!uniqueLinks.ContainsKey(linkId))
            {
                var uniqueLink = new UniqueLink();
                uniqueLink.BlastLinkID = blastLinkId;
                uniqueLink.UniqueID = linkId;
                uniqueLink.BlastID = blastId;
                uniqueLinkId = CommunicatorUniqueLink.Save(uniqueLink);

                if (!uniqueLinks.ContainsKey(uniqueLink.UniqueID))
                {
                    uniqueLinks.Add(uniqueLink.UniqueID, uniqueLinkId);
                }
            }
            else
            {
                uniqueLinkId = uniqueLinks[linkId];
            }                
           
            return uniqueLinkId;
        }

        public static void Encrypt_EncodeLinks_HTML(ref StringBuilder strHTML, KM.Common.Entity.Encryption KMEncryption, MatchCollection mc, string emailID)
        {
            if ((ConfigurationManager.AppSettings["OpenClick_UseOldSite"] ?? "").ToString().ToLower().Equals("false"))
            {
                foreach (Match m in mc)
                {

                    string link = m.Groups[2].Value;//.Replace("ECN_EmailID", emailID);

                    int indexOfQ = link.IndexOf("?");

                    string queryParams = link.Substring(indexOfQ + 1);
                    int indexOfL = queryParams.IndexOf("&l=");
                    if (indexOfL != -1)
                    {
                        queryParams = queryParams.Substring(0, indexOfL);
                    }
                    queryParams = queryParams.Replace("ECN_EmailID", emailID);
                    string encryptedQuery = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Base64Encrypt(queryParams, KMEncryption));
                    StringBuilder newLink = new StringBuilder(link.Remove(indexOfQ));
                    newLink.Append(encryptedQuery);
                    strHTML = strHTML.Replace(link, newLink.ToString());

                }
            }

        }

        public static void Encrypt_EncodeLinks_TEXT(ref StringBuilder str_Text, KM.Common.Entity.Encryption KMEncryption, MatchCollection textLinks, string emailID)
        {

            //TEXT

            foreach (Match m in textLinks)
            {

                string link = m.Value;
                int indexOfQ = link.IndexOf("?");
                string queryParams = link.Substring(indexOfQ + 1);
                int indexOfl = queryParams.IndexOf("&l=");
                if (indexOfl != -1)
                {
                    queryParams = queryParams.Substring(0, indexOfl).Replace("ECN_EmailID", emailID);

                    string encryptedQuery = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Base64Encrypt(queryParams, KMEncryption));

                    string newLink = link.Remove(indexOfQ);
                    newLink += encryptedQuery + ">";
                    str_Text = str_Text.Replace(link, newLink);
                }

            }
        }

        public static string LinkReWriter(
            string body, 
            Blast blast, 
            string customerId, 
            string virtualPath, 
            string hostName, 
            string forwardFriend)
        {
            return FormatLinks(
                body, 
                blast, 
                customerId, 
                hostName, 
                forwardFriend, 
                true, 
                LinkWriterStatsStartHeader,
                LinkWriterStatsEndHeader);
        }

        public static string LinkReWriterText(
            string body,
            Blast blast,
            string customerId,
            string virtualPath,
            string hostName,
            string forwardFriend)
        {
            return FormatLinks(
                body,
                blast,
                customerId,
                hostName,
                forwardFriend,
                false,
                LinkWriterTextStatsStartHeader,
                LinkWriterTextStatsEndHeader);
        }

        private static string FormatLinks(
            string body,
            Blast blast,
            string customerId,
            string hostName,
            string forwardFriend,
            bool isHtml,
            string statsStartHeader,
            string statsEndHeader)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return body;
            }

            Guard.NotNull(blast, nameof(blast));

            LogStatistics(blast.BlastID, statsStartHeader);
            
            var customer = GetByCustomerID(blast.CustomerID ?? 0, false);
            foreach (var mapping in LinkRewriteMappings)
            {
                body = CommonStringFunctions.Replace(body, mapping.Key, mapping.Value(customer));
            }

            var activityDomainPath = ConfigurationManager.AppSettings[ActivityDomainPathConfigurationKey];
            var redirectPageLink = GetRedirectPageLink(activityDomainPath);
            body = ReplaceProfilePreferences(
                body,
                blast.BlastID,
                blast.CustomerID ?? 0,
                blast.GroupID ?? 0,
                customer.BaseChannelID ?? 0,
                activityDomainPath,
                redirectPageLink,
                isHtml);

            customerId = blast.CustomerID?.ToString() ?? customerId;
            body = AddHeader(body, blast.BlastID, customerId, forwardFriend, activityDomainPath);
            body = ReplaceSubscriptionManagementContent(body, customer.BaseChannelID ?? 0, activityDomainPath, isHtml);
            body = ReplacePlaceholders(body, blast, customerId, hostName, activityDomainPath, redirectPageLink, isHtml);            
            body = ReplaceLinks(body, redirectPageLink, blast.BlastID, isHtml);

            LogStatistics(blast.BlastID, statsEndHeader);

            return body;
        }

        private static void LogStatistics(int blastId, string header)
        {
            var logStatistics = LoggingFunctions.LogStatistics();
            if (!logStatistics)
            {
                return;
            }

            var currentProcess = Process.GetCurrentProcess();
            var currentFileName = Path.GetFileNameWithoutExtension(currentProcess.MainModule.FileName);
            var currentDate = DateTime.Now.ToShortDateString().Replace(SlashChar, DashChar);
            FileFunctions.LogActivity(
                false,
                $"{header}{blastId}",
                $"statistics_{currentFileName}_{currentDate}");
        }

        private static string GetRedirectPageLink(string activityDomainPath)
        {
            var openClickUseOldSite = ConfigurationManager.AppSettings[OpenClickUseOldSiteConfigurationKey];
            var redirpage = activityDomainPath + OpenClickOldSiteRedirectPage;

            if ((openClickUseOldSite ?? string.Empty).Equals(Boolean.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                var mvcActivityDomainPath = ConfigurationManager.AppSettings[MvcActivityDomainPathConfigurationKey];
                redirpage = mvcActivityDomainPath + MvcActivityRedirectPage;
            }

            return redirpage;
        }

        private static string GetGroupName(int blastGroupId)
        {
            try
            {
                var communicatorGroup = CommunicatorGroup.GetByGroupID_NoAccessCheck(blastGroupId);
                return communicatorGroup.GroupName;
            }
            // Exception MUST NOT propagate in the upstream, since 
            // this is a fail-silently process, with no breaking changes
            catch
            {
                return string.Empty;
            }
        }

        private static string ReplaceLinks(string text, string redirectPageLink, int blastId, bool isHtml)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            text = Regex.Replace(
               text,
               LinksMatchPattern,
               string.Format(LinkReplacementTemplate, redirectPageLink, blastId),
               RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (isHtml)
            {
                text = HtmlReplaceWithLinkId(text, blastId);   
                return Regex.Replace(
                    text,
                    string.Format(LinksMatchPatternTemplate, redirectPageLink),
                    match =>
                    {
                        var formattedText =
                            $@"{match.Groups[1].Value}{redirectPageLink}%%ECN_Encrypt_{match.Groups[2].Value}";
                        if (string.IsNullOrWhiteSpace(match.Groups[3].Value))
                        {
                            return $@"{formattedText}%%{match.Groups[1].Value}";
                        }

                        return $@"{formattedText}_{match.Groups[3].Value}%%{match.Groups[1].Value}";
                    },
                    RegexOptions.Singleline | RegexOptions.Compiled);
            }

            text = TextReplaceWithLinkID(text, blastId);
            return Regex.Replace(
                text,
                RedirectLinkMatchPattern,
                string.Format(RedirectLinkReplacementTemplate, redirectPageLink),
                RegexOptions.Singleline | RegexOptions.Compiled);
        }

        private static string GetListProfilePreferencesPage(string activityDomainPath, int baseChannelId, bool isHtml)
        {
            if (isHtml)
            {
                var imgPath = GetImagePath(baseChannelId);
                return string.Format(ListProfilePreferencesPageTemplate, activityDomainPath, imgPath);
            }
            else
            {
                return string.Format(ListProfilePreferencesTextPageTemplate, activityDomainPath);
            }
        }

        private static string GetUserProfilePreferencesPage(
            string activityDomainPath,
            int groupId,
            int baseChannelId,
            bool isHtml)
        {
            if (isHtml)
            {
                var imgPath = GetImagePath(baseChannelId);
                return string.Format(UserProfilePreferencesPageTemplate, activityDomainPath, groupId, imgPath);
            }
            else
            {
                return string.Format(UserProfilePreferencesTextPageTemplate, activityDomainPath, groupId);
            }
        }

        private static string GetImagePath(int baseChannelId)
        {
            var imageDomainPath = ConfigurationManager.AppSettings[ImageDomainPathConfigurationKey];
            return string.Format(ImagePathTemplate, imageDomainPath, baseChannelId);
        }

        private static string GetProfilePreferencesPage(string activityDomainPath, bool isHtml)
        {
            if (isHtml)
            {
                return string.Format(ProfilePreferencesPageTemplate, activityDomainPath);
            }
            else
            {
                return string.Format(ProfilePreferencesTextPageTemplate, activityDomainPath);
            }
        }

        private static string ReplacePlaceholders(
            string text, 
            Blast blast, 
            string customerId, 
            string hostName, 
            string activityDomainPath,
            string redirectPageLink,
            bool isHtml)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            var groupName = GetGroupName(blast.GroupID ?? 0);
            var layout = CommunicatorLayout.GetByLayoutID_NoAccessCheck(blast.LayoutID ?? 0, false);
            var publicViewPageLink = string.Format(PublicViewPageLinkTemplate, activityDomainPath, blast.BlastID);
            var emailToFriendPageLink = string.Format(EmailToFriendPageLinkTemplate, activityDomainPath, blast.BlastID);
            var reportAbusePageLink = string.Format(
                ReportAbusePageLinkTemplate,
                activityDomainPath,
                blast.GroupID ?? 0, customerId,
                blast.BlastID);
            var unsubscribePageLink = string.Format(
                UnsubscribePageLinkTemplate, 
                activityDomainPath, 
                blast.BlastID, 
                customerId);

            text = CommonStringFunctions.Replace(text, GroupNamePlaceholder, groupName);
            text = CommonStringFunctions.Replace(text, HostNamePlaceholder, hostName);
            text = CommonStringFunctions.Replace(text, UnsubscribeLinkPlaceholder, unsubscribePageLink);
            text = CommonStringFunctions.Replace(text, PublicViewPlaceholder, publicViewPageLink);
            text = CommonStringFunctions.Replace(text, ReportAbuseLinkPlaceholder, reportAbusePageLink);
            text = CommonStringFunctions.Replace(text, EmailToFriendPlaceholder, emailToFriendPageLink);
            text = CommonStringFunctions.Replace(text, CompanyAddressPlaceholder, layout.DisplayAddress);
            text = CommonStringFunctions.Replace(text, EmailFromAddressPlaceholder, blast.EmailFrom);
            text = CommonStringFunctions.Replace(text, BlastStartTimePlaceholder, blast.SendTime.ToString());
            text = CommonStringFunctions.Replace(text, BlastEndTimePlaceholder, blast.FinishTime.ToString());
            text = CommonStringFunctions.Replace(text, BlastIdPlaceholder, blast.BlastID.ToString());

            if (!isHtml)
            {
                var httpReplacement = string.Format(HttpReplacementTemplate, redirectPageLink, blast.BlastID);
                text = CommonStringFunctions.Replace(text, HttpTag, $"{httpReplacement}://");
                text = CommonStringFunctions.Replace(text, HttpsTag, $"{httpReplacement}s://");
            }

            return text;
        }

        private static string ReplaceProfilePreferences(
            string text, 
            int blastId,
            int customerId,
            int groupId,
            int baseChannelId, 
            string activityDomainPath,
            string redirectPageLink, 
            bool isHtml)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            
            var profilePreferencesPage = string.Empty;
            var userprofilepreferencespage = string.Empty;
            var listprofilepreferencespage = string.Empty;

            if (HasProductFeature(customerId, EnumsServices.EMAILMARKETING, EnumsServiceFeatures.TrackmailtoClickThru))
            {
                var mailTo = string.Format(MailToReplacementTemplate, redirectPageLink, blastId);
                text = CommonStringFunctions.Replace(text, HrefMailToTag, mailTo);
                text = CommonStringFunctions.Replace(text, HrefMailToTagVariation1, mailTo);
                text = CommonStringFunctions.Replace(text, HrefMailToTagVariation2, mailTo);
            }

            if (HasProductFeature(
                customerId, 
                EnumsServices.EMAILMARKETING, 
                EnumsServiceFeatures.EmailListPreferences))
            {
                profilePreferencesPage = GetProfilePreferencesPage(activityDomainPath, isHtml);
            }
            
            if (HasProductFeature(
                customerId, 
                EnumsServices.EMAILMARKETING, 
                EnumsServiceFeatures.EmailProfilePreferences))
            {
                userprofilepreferencespage = 
                    GetUserProfilePreferencesPage(activityDomainPath, groupId, baseChannelId, isHtml);
            }

            if (HasProductFeature(
                customerId, 
                EnumsServices.EMAILMARKETING, 
                EnumsServiceFeatures.EmailListandProfilePreferences))
            {
                listprofilepreferencespage = GetListProfilePreferencesPage(activityDomainPath, baseChannelId, isHtml);
            }

            text = CommonStringFunctions.Replace(text, ProfilePreferencesPlaceholder, profilePreferencesPage);
            text = CommonStringFunctions.Replace(text, UserProfilePreferencesPlaceholder, userprofilepreferencespage);
            text = CommonStringFunctions.Replace(text, ListProfilePreferencesPlaceholder, listprofilepreferencespage);

            return text;
        }
        
        private static string ReplaceSubscriptionManagementContent(
            string text, 
            int baseChannelId, 
            string activityDomainPath,
            bool isHtml)
        {
            var subscriptionManagementIdentifier = SubscriptionManagementKey + DotChar;
            if (string.IsNullOrWhiteSpace(text) ||
                text.IndexOf(subscriptionManagementIdentifier, StringComparison.OrdinalIgnoreCase) <= 0)
            {
                return text;
            }

            var subManagementPageTemplate = string.Format(SubManagementPageTemplate, activityDomainPath, Placeholder);
            if(!isHtml)
            {
                subManagementPageTemplate = string.Format(SubManagementTextPageTemplate, activityDomainPath, Placeholder);
            }
            var emailbody = Regex.Split(text, SubscriptionManagementKey);
            for (var i = 0; i < emailbody.Length; i++)
            {                
                var lineData = emailbody[i];
                if (i % 2 == 0)
                {
                    continue;
                }

                var tempSubMLink = lineData.Remove(lineData.Length - 1, 1).Remove(0, 1);
                var subscriptionManagements = SubscriptionManagement.GetByBaseChannelID(baseChannelId);
                var subscriptionManagement = subscriptionManagements.Find(x => x.Name == tempSubMLink);

                text = CommonStringFunctions.Replace(
                    text,
                    $"{SubscriptionManagementKey}.{tempSubMLink}.{SubscriptionManagementKey}", 
                    string.Format(subManagementPageTemplate, subscriptionManagement.SubscriptionManagementID));
            }

            return text;
        }

        private static string AddHeader(
            string text,
            int blastId,
            string customerId,
            string forwardFriend,
            string activityDomainPath)
        {
            if (string.IsNullOrWhiteSpace(text) ||
                string.IsNullOrWhiteSpace(forwardFriend))
            {
                return text;
            }

            var header = string.Empty;
            try
            {
                header = DataFunctions.ExecuteScalar(string.Format(HeaderQueryTemplate, customerId)).ToString();
            }
            // Generic Exception is intentionately caught here in order to retry the call with different inputs
            // The second call is left to propagate.
            catch
            {
                header = DataFunctions.ExecuteScalar(string.Format(HeaderQueryTemplate, 1)).ToString();
            }

            var f2fNotes = string.Empty;
            try
            {
                f2fNotes = HttpContext.Current.Session[F2FnotesSessionKey].ToString();
            }
            // Exception MUST NOT propagate because a potential error here 
            // should not cause the process to fail
            catch
            {
                f2fNotes = string.Empty;
            }

            var subscribeLink = string.Format(SubscribeLinkTemplate, activityDomainPath, blastId, customerId);
            header = header.Replace(SubscribeLinkPlaceholder, subscribeLink);
            header = header.Replace(EmailFriendPlaceholder, forwardFriend);
            header = header.Replace(F2FNotesPlaceholder, f2fNotes);

            return $"{header}{text}";
        }

        #region Link ReWriter - WITHOUT BLAST
        public static string LinkReWriterWithoutBlast(string strText, int LayoutID, int GroupID, string CustomerID, string VirtualPath, string HostName, string ForwardFriend)
        {
            string redirpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/linkfrom.aspx";
            string body = strText;

            ECN_Framework_Entities.Accounts.Customer customer = GetByCustomerID(int.Parse(CustomerID), false);

            body = StringFunctions.Replace(body, "%%customer_name%%", customer.CustomerName);
            body = StringFunctions.Replace(body, "%%customer_address%%", string.Format("{0}, {1}, {2} - {3}", customer.Address, customer.City, customer.State, customer.Zip));
            body = StringFunctions.Replace(body, "%%customer_webaddress%%", customer.WebAddress);

            body = StringFunctions.Replace(body, "%%customer_udf1%%", customer.customer_udf1);
            body = StringFunctions.Replace(body, "%%customer_udf2%%", customer.customer_udf2);
            body = StringFunctions.Replace(body, "%%customer_udf3%%", customer.customer_udf3);
            body = StringFunctions.Replace(body, "%%customer_udf4%%", customer.customer_udf4);
            body = StringFunctions.Replace(body, "%%customer_udf5%%", customer.customer_udf5);


            // set the hostname
            body = StringFunctions.Replace(body, "%%hostname%%", HostName);

            //catch to remove unsub from link track 
            body = StringFunctions.Replace(body, "http://%%unsubscribelink%%/", "%%unsubscribelink%%");
            body = StringFunctions.Replace(body, "http://%%emailtofriend%%/", "%%emailtofriend%%");

            //body=StringFunctions.Replace(body, "href=\"http://", "href=\""+redirpage+"?b=0&e=%%EmailID%%&l=http://");
            //body=StringFunctions.Replace(body, "href=http://", "href="+redirpage+"?b=0&e=%%EmailID%%&l=http://");
            //body=StringFunctions.Replace(body, "href=\"https://", "href=\""+redirpage+"?b=0&e=%%EmailID%%&l=https://");
            //body=StringFunctions.Replace(body, "href=https://", "href="+redirpage+"?b=0&e=%%EmailID%%&l=https://");

            body = StringFunctions.Replace(body, "href=http://", "href=" + redirpage + "?b=0&e=%%EmailID%%&l=http://");
            body = StringFunctions.Replace(body, "href=https://", "href=" + redirpage + "?b=0&e=%%EmailID%%&l=https://");

            body = StringFunctions.Replace(body, "href=\"http://", "href=\"" + redirpage + "?b=0&e=%%EmailID%%&l=http://");
            body = StringFunctions.Replace(body, "href=\"https://", "href=\"" + redirpage + "?b=0&e=%%EmailID%%&l=https://");

            body = StringFunctions.Replace(body, "href='http://", "href='" + redirpage + "?b=0&e=%%EmailID%%&l=http://");
            body = StringFunctions.Replace(body, "href='https://", "href='" + redirpage + "?b=0&e=%%EmailID%%&l=https://");


            //rewrite mailto links If they have the Featre Turned ON
            //if (sc.hasProductFeature("ecn.communicator", "Track mailto: Click Thru", CustomerID.ToString()))
            if (HasProductFeature(int.Parse(CustomerID), KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.TrackmailtoClickThru))
            {
                body = StringFunctions.Replace(body, "href=\"mailto:", "href=\"" + redirpage + "?b=0&e=%%EmailID%%&l=mailto:");
                body = StringFunctions.Replace(body, "href='mailto:", "href=\"" + redirpage + "?b=0&e=%%EmailID%%&l=mailto:");
                body = StringFunctions.Replace(body, "href=mailto:", "href=\"" + redirpage + "?b=0&e=%%EmailID%%&l=mailto:");
            }

            if ("" != ForwardFriend)
            {
                // Add the "message" at the top of the email.
                string subscribe_link = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/subscribe.aspx?e=%%EmailAddress%%&g=" + GroupID + "&b=0&c=" + CustomerID + "&s=S&f=html";
                string unsubscribe_link = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/subscribe.aspx?e=%%EmailAddress%%&g=" + GroupID + "&b=0&c=" + CustomerID + "&s=U&f=html";

                // Remove the unsubscribe link
                body = StringFunctions.Replace(body, "%%unsubscribelink%%", unsubscribe_link);

                // get the message to put on top
                string TemplateQuery = " SELECT HeaderSource FROM " + accountsdb + ".dbo.CustomerTemplate " +
                    " WHERE CustomerID = " + CustomerID + " AND TemplateTypeCode='F2FIntroEmailHdr' and IsActive=1 and IsDeleted = 0";
                string header;
                try
                {
                    header = DataFunctions.ExecuteScalar(TemplateQuery).ToString();
                }
                catch
                {
                    TemplateQuery = " SELECT HeaderSource FROM " + accountsdb + ".dbo.CustomerTemplate " +
                        " WHERE CustomerID = 1 AND TemplateTypeCode='F2FIntroEmailHdr' and IsActive=1 and IsDeleted = 0";
                    header = DataFunctions.ExecuteScalar(TemplateQuery).ToString();
                }

                header = header.Replace("%%sub_link%%", subscribe_link);
                header = header.Replace("%%email_friend%%", ForwardFriend);
                body = header + body;
            }
            else
            {
                //added for unsubscribe tag
                string unsubpagepage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?e=%%EmailAddress%%&g=" + GroupID + "&b=0&c=" + CustomerID + "&s=U&f=html";
                body = StringFunctions.Replace(body, "%%unsubscribelink%%", unsubpagepage);
            }
            string emailtofriendpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/emailtofriend.aspx?e=%%EmailID%%&b=0";
            body = StringFunctions.Replace(body, "%%emailtofriend%%", emailtofriendpage);
            body = StringFunctions.Replace(body, "%%hostname%%", HostName);

            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(LayoutID, false);
            body = StringFunctions.Replace(body, "%%company_address%%", layout.DisplayAddress);

            return body;
        }
        #endregion

        #region IMG rewriter
        public static string imgRewriter(string strText, int BlastID)
        {
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(strText);
            List<string> imgSrcList = new List<string>();
            HtmlAgilityPack.HtmlNodeCollection imgs = document.DocumentNode.SelectNodes("//img[@src]");
            if (imgs != null)
            {
                foreach (var node in imgs)
                {
                    HtmlAgilityPack.HtmlAttribute imgSRC = node.Attributes["src"];

                    if (imgSRC != null)
                    {
                        if (!imgSRC.Value.Contains("ecn5.com/engines/open.aspx") && !imgSRC.Value.Contains("ecn5.com/engines/read.aspx") && !imgSRC.Value.Contains("ecn5.com/Opens/"))
                        {
                            if (!imgSrcList.Contains(imgSRC.Value))
                            {
                                if (imgSRC.Value.Length > 0)
                                {
                                    imgSrcList.Add(imgSRC.Value.Trim());
                                }
                            }
                        }
                    }
                }
            }

            foreach (string s in imgSrcList)
            {
                if (s.Contains("?"))
                {
                    strText = strText.Replace(s, s + "&bid=" + BlastID.ToString() + "&eid=%%EmailID%%");
                }
                else
                {
                    strText = strText.Replace(s, s + "?bid=" + BlastID.ToString() + "&eid=%%EmailID%%");
                }
            }


            return strText;
        }
        #endregion

        public static string MenuList(string MenuPage, string ContextPath, int customerID)
        {
            string displayOut = "";
            string front = ""; string sub_front = "";
            string back = ""; string sub_back = "";
            int selectedParent = 0;
            string sub_menuQuery = "";
            //ArrayList menuIDs = new ArrayList();
            ArrayList menuBuild = new ArrayList();

            try
            {
                string sqlQuery = "SELECT ParentID, MenuID FROM Menus WHERE MenuCode = '" + MenuPage + "'";
                DataTable dt = DataFunctions.GetDataTable(sqlQuery);
                foreach (DataRow dr in dt.Rows)
                {
                    selectedParent = Convert.ToInt32(dr["ParentID"]);
                }
                sqlQuery = "SELECT * FROM Menus WHERE MenuTypeCode = 'website' and CustomerID = " + customerID + " ORDER BY  SortOrder ";
                dt = DataFunctions.GetDataTable(sqlQuery);
                int MenuID = 0; int sub_MenuID = 0;
                string MenuCode = ""; string sub_MenuCode = "";
                string MenuName = ""; string sub_MenuName = "";
                string MenuTarget = ""; string sub_MenuTarget = "";
                int ParentID = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    MenuID = Convert.ToInt32(dr["MenuID"]);
                    MenuCode = dr["MenuCode"].ToString();
                    MenuName = dr["MenuName"].ToString();
                    MenuTarget = dr["MenuTarget"].ToString();
                    ParentID = Convert.ToInt32(dr["ParentID"]);

                    if (!(menuIDs.Contains(MenuID)))
                    {
                        if (!(MenuTarget.Equals("")))
                        {
                            MenuTarget = "pageDetail.aspx?pgID=" + MenuTarget;
                        }
                        else { }

                        front = "&nbsp;<img src='images/icoBullet.gif' border=0>&nbsp;<a href='" + MenuTarget + "'>";
                        back = "</a><br>";

                        if (MenuCode.Equals(MenuPage))
                        {
                            front = "&nbsp;<a href='" + MenuTarget + "'><img src='iamges/icoBullet.gif' border=0>&nbsp;";
                            back = "</a><br>";
                        }

                        displayOut += front + MenuName + back;
                        menuIDs.Add(MenuID);
                        menuBuild.Add(displayOut);

                        //displayOut += checkMenuHasSubMenus(MenuID, customerID);
                        sub_menuQuery = "SELECT * FROM Menus WHERE MenuTypeCode = 'website' and CustomerID = " + customerID + " and ParentID = " + MenuID + " ORDER BY  SortOrder ";
                        dt = DataFunctions.GetDataTable(sub_menuQuery);
                        foreach (DataRow dr2 in dt.Rows)
                        {
                            sub_MenuID = Convert.ToInt32(dr2["MenuID"]);
                            sub_MenuCode = dr2["MenuCode"].ToString();
                            sub_MenuName = dr2["MenuName"].ToString();
                            sub_MenuTarget = dr2["MenuTarget"].ToString();
                            sub_front = "<img src='images/icoNode.gif' border=0>&nbsp;<img src='images/icoBullet.gif' border=0>&nbsp;<a class=sidemenu href='" + sub_MenuTarget + "'>";
                            sub_back = "</a><br>";
                            displayOut += sub_front + sub_MenuName + sub_back;
                            menuIDs.Add(sub_MenuID);
                            menuBuild.Add(displayOut);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            return displayOut;
        }

        public static string checkMenuHasSubMenus(int menuID, int customerID)
        {
            int sub_MenuID = 0;
            string sub_MenuCode = "";
            string sub_MenuName = "";
            string sub_MenuTarget = "";
            string sub_displayOut = "";
            string sub_menuQuery = "";
            string sub_front = "";
            string sub_back = "";

            sub_menuQuery = "SELECT * FROM Menus WHERE MenuTypeCode = 'website' and CustomerID = " + customerID + " and ParentID = " + menuID + " ORDER BY  SortOrder ";
            DataTable dt = DataFunctions.GetDataTable(sub_menuQuery);
            foreach (DataRow dr2 in dt.Rows)
            {
                sub_MenuID = Convert.ToInt32(dr2["MenuID"]);
                sub_MenuCode = dr2["MenuCode"].ToString();
                sub_MenuName = dr2["MenuName"].ToString();
                sub_MenuTarget = dr2["MenuTarget"].ToString();
                sub_front = "<img src='images/icoNode.gif' border=0>&nbsp;<img src='images/icoBullet.gif' border=0>&nbsp;<a class=sidemenu href='" + sub_MenuTarget + "'>";
                sub_back = "</a><br>";
                sub_displayOut += sub_front + sub_MenuName + sub_back;
                menuIDs.Add(sub_MenuID);
                //menuBuild.Add(sub_displayOut);

                checkMenuHasSubMenus(sub_MenuID, customerID);
            }
            return sub_displayOut;
        }

        public static string NewsList(string EventType)
        {
            string displayOut = "";

            displayOut += "<table class='newsContentOuter' width='100%'>";
            try
            {
                string sqlQuery =
                    "SELECT * FROM events " +
                    " WHERE DisplayFlag='Y' " +
                    " AND EndDate > getDate() " +
                    " ORDER BY StartDate, EventName";
                DataTable dt = DataFunctions.GetDataTable(sqlQuery);
                foreach (DataRow dr in dt.Rows)
                {
                    int EventID = Convert.ToInt32(dr["EventID"]);
                    string EventTypeCode = dr["EventTypeCode"].ToString();
                    string EventName = dr["EventName"].ToString();
                    string StartDate = dr["StartDate"].ToString();
                    string Description = dr["Description"].ToString(); ;
                    displayOut += "<tr>";
                    displayOut += "<td class='newsContentInner' valign=top><b>" + StartDate + "</b> / " + EventName + "<BR><ul>" +
                        DataFunctions.MediaValue("EventID", EventID, "logo") + " " + Description + "<BR>" + DataFunctions.MediaValue("EventID", EventID, "link") + "<BR></ul></td>";
                    displayOut += "</tr>";
                }
            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            displayOut += "</table>";
            return displayOut;
        }
        public static string prlist(string EventType)
        {
            String displayOut = "";

            displayOut += "<table class='prContentOuter' width='100%'>";
            try
            {
                string sqlQuery =
                    "SELECT * FROM events " +
                    "WHERE DisplayFlag='Y' " +
                    "ORDER BY StartDate DESC, EventName";
                DataTable dt = DataFunctions.GetDataTable(sqlQuery);
                foreach (DataRow dr in dt.Rows)
                {
                    int EventID = Convert.ToInt32(dr["EventID"]);
                    string EventTypeCode = dr["EventTypeCode"].ToString();
                    string EventName = dr["EventName"].ToString();
                    string StartDate = dr["StartDate"].ToString();
                    string Location = dr["Location"].ToString();
                    string Description = dr["Description"].ToString(); ;
                    displayOut += "<tr>";
                    displayOut += "<td class='prContentInner' valign=top>" + EventName + " / " + StartDate + "<BR>" + DataFunctions.MediaValue("EventID", EventID, "logo") + Location + " " + Description + "<BR>" + DataFunctions.MediaValue("EventID", EventID, "link") + "<BR></td>";
                    displayOut += "</tr>";
                }
            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            displayOut += "</table>";
            return displayOut;
        }
        public static string addOpensImage(string strText, int BlastID, string VirtualPath, string HostName)
        {
            string redirpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/open.aspx";
            if (ConfigurationManager.AppSettings["OpenClick_UseOldSite"].ToString().ToLower().Equals("false"))
                redirpage = System.Configuration.ConfigurationManager.AppSettings["MVCActivity_DomainPath"] + "/Opens/";

            int indexOfBody = -1;
            if (strText.ToLower().Contains("</body>"))
            {
                 indexOfBody = strText.ToLower().IndexOf("</body>");
            }

            string body = indexOfBody == -1 ? strText + "<img src='" + redirpage + "%%ECN_Encrypt_Open%%' height=1 width=1 border=0>" : strText.Insert(indexOfBody,"<img src='" + redirpage + "%%ECN_Encrypt_Open%%' height=1 width=1 border=0>");
            return body;
        }

        public static string addOpensImageForDirectEmail(string strContent, int EmailDirectID)
        {
            string redirPage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/EmailDirectOpen.aspx";
            int indexOfLastBody = strContent.ToLower().IndexOf("</body>");

            string body = strContent.Insert(indexOfLastBody, "<img src='" + redirPage + "?edid" + EmailDirectID + "' height=1 width=1 border=0 />");
            return body;
        }

        public static string addReadsImage(string strText, int BlastID, string VirtualPath, string HostName)
        {
            string redirpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/read.aspx";
            string body = strText + "<img src='" + redirpage + "?b=" + BlastID + "&e=%%EmailID%%' height=1 width=1 border=0>";
            return body;
        }
    }

    public class LinkWithGuid
    {
        public LinkWithGuid() { link = string.Empty; Guid = string.Empty; aToReplace = string.Empty; }
        public string link { get; set; }

        public string Guid { get; set; }

        public string aToReplace { get; set; }

        public string originalA { get; set; }

        public string lFound { get; set; }
    }
}