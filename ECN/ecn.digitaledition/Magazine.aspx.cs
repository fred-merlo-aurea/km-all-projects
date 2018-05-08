using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.common.classes;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Publisher;
using ECN_Framework_Common.Objects;
using KM.Common;
using KM.Common.Utilities.Email;
using Customer = ECN_Framework_BusinessLayer.Accounts.Customer;

namespace ecn.digitaledition
{
    /// <summary>
    /// Summary description for Pages.
    /// </summary>
    public partial class Magazine : System.Web.UI.Page
    {
        private const string AdminFromEmailConfigurationKey = "Admin_FromEmail";
        private const string AdminEmailSubject = "Activate your Subscription";
        private const int MPlanet2009 = 2022;
        private const string TrueNumberString = "1";
        private const string ErrorPageName = "Error.aspx";
        private const string BlankTarget = "_blank";
        private const string FalseNumberString = "0";
        private const string ContactFormLinkHtml = "<a href=\"{0}\" target=\"_blank\" class=\"infoBtn\" id=\"infoLinkContact\"><span>&raquo;</span>&nbsp;Contact Us</a>";
        private const string StyleAttribute = "style";
        private const string DisplayNone = "display:none";
        private const string DefaultSubscribeLinkHtml = "<a href=\"javascript:infoSwitch('Subscribe');initSubscForm()\" class=\"infoBtn\" id=\"infoLinkSubscribe\"><span>&raquo;</span>&nbsp;Subscribe Today</a>";
        private const string CustomSubscribeLinkHtml = "<a href=\"{0}\" target=\"_blank\" class=\"infoBtn\" id=\"infoLinkSubscribe\"><span>&raquo;</span>&nbsp;Subscribe Today</a>";
        private const string DefaultContactFormHtml = "<a href=\"javascript:infoSwitch('Contact');\" class=\"infoBtn\" id=\"infoLinkContact\"><span>&raquo;</span>&nbsp;Contact Us</a>";
        private const string IgnoreKeysScript = "if (event.keyCode < 48 || event.keyCode > 57) event.returnValue = false;";
        private const string OnKeyPressAttribute = "onKeyPress";
        private const string PageNoStyle = "FONT-SIZE: 12px; FLOAT: left; FONT-FAMILY: Arial, Helvetica, sans-serif; TEXT-ALIGN: center;width:25px;";
        private const string IpKeyName = "IP";
        private const string RemoteAddrKeyName = "REMOTE_ADDR";
        protected PlaceHolder plMaps;
        protected PlaceHolder plLinks;

        StringBuilder sbBookmark = new StringBuilder("");
        ECN_Framework_Entities.DigitalEdition.DigitalSession uSession;
        StringBuilder sbTOC = new StringBuilder();
        KMPlatform.Entity.User user;

        private string getPublicationCode()
        {
            try
            {
                return Request.QueryString["cd"].ToString();
            }
            catch
            {
                try
                {
                    string hostname = Request.ServerVariables["HTTP_HOST"].ToString();

                    if (hostname.ToLower().IndexOf("ecndigitaledition.com") > 0)
                    {
                        hostname = hostname.ToLower().Replace("ecndigitaledition.com", "");

                        if (hostname != "www." && hostname != string.Empty)
                            return hostname.ToLower().Replace(".", "");
                        else
                            return string.Empty;
                    }
                    else
                        return string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

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

        private string getEmailAddress()
        {
            try
            {
                return Request.QueryString["ea"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private int getBlastID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["b"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int getEmailID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["e"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int getPageNo()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["pno"].ToString());
            }
            catch
            {
                return 0;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            HideMessageLabels();

            user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"], true);

            if (!IsPostBack)
            {
                LoadPageFirstTime();
            }
            else
            {
                int editionId;
                int.TryParse(lblEditionID.Text, out editionId);
                uSession = CreateUserSession(string.Empty, editionId, user);
            }

            Session.Add(IpKeyName, Request.ServerVariables[RemoteAddrKeyName]);
        }

        public static ECN_Framework_Entities.DigitalEdition.DigitalSession CreateUserSession(string Publicationcode, int EditionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.DigitalEdition.DigitalSession obj = null;
            ECN_Framework_Entities.Publisher.Edition ed = null;

            try
            {
                if (EditionID > 0)
                    ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(EditionID, user);
                else if (Publicationcode.Length > 0)
                    ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByPublicationCode(Publicationcode, user);

                ECN_Framework_Entities.Publisher.Publication pb = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(ed.PublicationID, user);

                if (ed.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Active.ToString() || ed.Status == ECN_Framework_Common.Objects.Publisher.Enums.Status.Archieve.ToString())
                {
                    if (null == System.Web.HttpContext.Current.Session["Edition_" + ed.EditionID])
                    {
                        obj = new ECN_Framework_Entities.DigitalEdition.DigitalSession();
                        obj.EditionID = ed.EditionID;
                        obj.PublicationCode = pb.PublicationCode;
                        obj.Totalpages = ed.Pages;
                        obj.IsLoginRequired = ed.IsLoginRequired;
                        obj.ImagePath = "/ecn.images/customers/" + ed.CustomerID + "/publisher/" + ed.EditionID + '/';
                        obj.Publicationtype = pb.PublicationType;
                        System.Web.HttpContext.Current.Session["Edition_" + ed.EditionID] = obj;
                    }
                    else
                    {
                        //Retrieve the already instance that was already created
                        obj = (ECN_Framework_Entities.DigitalEdition.DigitalSession)System.Web.HttpContext.Current.Session["Edition_" + ed.EditionID];
                    }
                }
            }
            catch
            {
            }

            //Return the single instance of this class that was stored in the session
            return obj;
        }

        private void btnLogin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (uSession != null)
            {
                if (txtLogin.Text != string.Empty && txtpassword.Text != string.Empty)
                {
                    uSession.EmailID = CheckLogin(txtLogin.Text, txtpassword.Text);

                    if (uSession.EmailID > 0)
                    {
                        //Session.Add("username",txtLogin.Text);
                        //Session.Add("password", txtpassword.Text);

                        uSession.IsAuthenticated = true;
                        DisplayPage();
                    }
                    else
                    {
                        lblMessage.Text = "Invalid Username or password";
                        lblMessage.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid Username and password.";
                    lblMessage.Visible = true;
                }
            }
        }

        private int CheckLogin(string username, string password)
        {

            try
            {
                int username_gdfID = 0;
                int pwd_gdfID = 0;
                int EmailID = 0;

                ECN_Framework_Entities.Publisher.Edition ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(uSession.EditionID, user);
                if (ed != null)
                {

                    int? groupID = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(ed.PublicationID, user).GroupID;

                    List<ECN_Framework_Entities.Communicator.GroupDataFields> gdflist = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID.Value, user);

                    foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdflist)
                    {
                        if (gdf.ShortName == "username")
                        {
                            username_gdfID = gdf.GroupDataFieldsID;
                        }
                        else if (gdf.ShortName == "pwd")
                        {
                            pwd_gdfID = gdf.GroupDataFieldsID;
                        }
                    }

                    if (username_gdfID > 0 && pwd_gdfID > 0)
                    {

                        EmailID = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID(username_gdfID, user).Find(x => x.DataValue == username).EmailID;

                        if (EmailID > 0)
                        {
                            EmailID = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID(username_gdfID, EmailID, user).Find(x => x.DataValue == username).EmailID;
                        }

                    }
                }
                return EmailID;

            }
            catch
            {
                return 0;
            }
        }

        private void DisplayPage()
        {
            pnlLogin.Visible = false;
            pnlEdition.Visible = true;

            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "loadfunctions", "<script language='javascript'>onload=function(){Edition.Init();renderTree();};</script>");

            if (uSession.Publicationtype.ToLower().Equals("1flyer"))
            {
                nav2Page.Visible = false;
                nav.Visible = false;
                plsbSinglePg.Visible = true;
                plsbDoublePg.Visible = false;
            }
            else if (uSession.Publicationtype.ToLower().Equals("2flyer"))
            {
                nav1Page.Visible = false;
                nav.Visible = false;
                plsbSinglePg.Visible = false;
                plsbDoublePg.Visible = true;
            }
            else
            {
                nav1Page.Visible = false;
                nav2Page.Visible = false;
                plsbSinglePg.Visible = true;
                plsbDoublePg.Visible = true;
            }

            LoadBookmark(uSession.EditionID);
            LoadBackIssues(uSession.EditionID);
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLogin.Click += new System.Web.UI.ImageClickEventHandler(this.btnLogin_Click);

        }
        #endregion

        #region Load Edition - BackIssues & TableofContents
        private void LoadBackIssues(int editionID)
        {
            int publicationID = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(editionID, user).PublicationID;

            List<ECN_Framework_Entities.Publisher.Edition> ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByPublicationID(publicationID, user);

            var query = (from e in ed
                         where e.EditionID != editionID
                         orderby e.EnableDate descending
                         select new { e.EditionID, EditionName = e.EditionName + e.Status == "Active" ? "(Active)" : "" });


            if (query.ToList().Count > 0)
            {
                rptBackIssues.DataSource = query.ToList();
                rptBackIssues.DataBind();
            }
            else
            {
                plbackissues.Visible = false;
                plbackissues2.Visible = false;
            }
        }

        private void LoadBookmark(int editionID)
        {
            string TableofContents = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(editionID, user).xmlTOC;

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
                    XmlNode xNode = nBookmarks.Item(0);
                    if (xNode.HasChildNodes)
                    {
                        sbTOC.Append("['" + xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")',,'folder'],");
                        RecurseBookmark(xNode);
                        sbTOC.Append("]");
                    }
                    else
                    {
                        sbTOC.Append("['" + xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")']");
                    }
                }
                else
                {
                    sbTOC.Append("['Front Cover', ['javascript:Edition.gotoPage(1)'],");
                    sbTOC.Append("[");
                    for (int x = 0; x <= nBookmarks.Count - 1; x++)
                    {

                        XmlNode xNode = nBookmarks.Item(x);

                        if (xNode.HasChildNodes)
                        {
                            sbTOC.Append("['" + xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")',,'folder'],");
                            RecurseBookmark(xNode);
                        }
                        else
                        {
                            sbTOC.Append("['" + xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")',,]");
                        }

                        if (x == nBookmarks.Count - 1)
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
                pltoc.Visible = false;
                pltoc2.Visible = false;
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

                for (int x = 0; x <= xNodeList.Count - 1; x++)
                {
                    xNode = xmlNode.ChildNodes[x];

                    if (xNode.HasChildNodes)
                    {
                        sbTOC.Append("['" + xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")',,'folder'],");
                        RecurseBookmark(xNode);
                    }
                    else
                    {
                        sbTOC.Append("['" + xNode.Attributes["title"].InnerText + "', ['javascript:Edition.gotoPage(" + xNode.Attributes["pageno"].InnerText + ")',,]");
                    }

                    if (x == xNodeList.Count - 1)
                        sbTOC.Append("]");
                    else
                        sbTOC.Append("],");

                }
                sbTOC.Append("],");
            }
        }

        public static ECN_Framework_Entities.DigitalEdition.DigitalSession GetCurrentSession(int EditionID)
        {
            ECN_Framework_Entities.DigitalEdition.DigitalSession obj = null;

            if (null != System.Web.HttpContext.Current.Session["Edition_" + EditionID.ToString()])
            {
                //Retrieve the instance that was already created
                obj = (ECN_Framework_Entities.DigitalEdition.DigitalSession)System.Web.HttpContext.Current.Session["Edition_" + EditionID.ToString()];
            }

            //Return the single instance of this class that was stored in the session
            return obj;
        }

        #endregion

        #region AJAX Calls 

        [System.Web.Services.WebMethod()]
        public static string GetEditionProps(int editionID)
        {
            StringBuilder sb = new StringBuilder("");
            ECN_Framework_Entities.DigitalEdition.DigitalSession uSession = GetCurrentSession(editionID);

            string SessionID = System.Guid.NewGuid().ToString();
            if (uSession != null)
            {
                if (uSession.IsLoginRequired.Value & !uSession.IsAuthenticated)
                    sb.Append("window.location.href='http://www.ECNdigitaledition.com/error.aspx';");
                else
                {
                    sb.Append("this.tp=\"" + uSession.Totalpages.ToString() + "\";this.path=\"" + ConfigurationManager.AppSettings["ImagePath"] + uSession.ImagePath + "\";this.type=\"" + uSession.Publicationtype + "\";this.EmailID=\"" + uSession.EmailID + "\";this.BlastID=\"" + uSession.BlastID + "\";this.IP=\"" + HttpContext.Current.Session["IP"].ToString() + "\";this.SessionID=\"" + SessionID + "\";");
                }
            }
            else
            {
                sb.Append("window.location.href='http://www.ECNdigitaledition.com/error.aspx';");
            }
            return sb.ToString();
        }

        [System.Web.Services.WebMethod()]
        public static string Subscribe(int editionID, int BlastID, string firstname, string lastname, string emailaddress, string phone, string fax, string country, string address, string city, string state, string zip, string IP, string SessionID)
        {
            //int GroupID = 0;
            int CustomerID = 0;
            int EmailID = 0;
            string imgPath = string.Empty;
            string EditionName = string.Empty;
            string ErrorMessage = string.Empty;

            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);

            ECN_Framework_Entities.Publisher.Edition ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(editionID, user);
            ECN_Framework_Entities.Publisher.Publication pb = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(ed.PublicationID, user);

            if (ed != null)
            {
                CustomerID = pb.CustomerID;

                imgPath = "/ecn.images/customers/" + pb.CustomerID + "/publisher/" + editionID + "/";
                EditionName = ed.EditionName;
                try
                {
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(pb.GroupID.Value, user);

                    if (emailaddress != string.Empty)
                    {
                        ECN_Framework_Entities.Communicator.EmailGroup eg = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(emailaddress.Replace("'", "''"), pb.GroupID.Value, user);

                        if (eg != null)
                        {
                            if (eg.SubscribeTypeCode == "S")
                                EmailID = eg.EmailID;
                        }

                        if (EmailID > 0)
                            ErrorMessage = "<Error>Email Address '&lt;b&gt;" + emailaddress + "&lt;/b&gt;' is already subscribed to this Magazine.</Error>";
                        else
                        {
                            EmailID = SubscribeToGroup(group, firstname, lastname, emailaddress, phone, fax, country, address, city, state, zip, CustomerID);

                            var body = ReplaceCodeSnippets(EmailID, pb.CustomerID, pb.GroupID.Value, imgPath, EditionName);
                            var addressFrom = ConfigurationManager.AppSettings[AdminFromEmailConfigurationKey];
                            var emailService = new EmailService(new EmailClient(), new ConfigurationProvider());
                            var emailMessage = new EmailMessage
                            {
                                From = addressFrom,
                                Subject = AdminEmailSubject,
                                Body = body
                            };
                            emailMessage.To.Add(emailaddress);
                            emailService.SendEmail(emailMessage);

                            ECN_Framework_Entities.Publisher.EditionActivityLog eal = new ECN_Framework_Entities.Publisher.EditionActivityLog();
                            eal.EditionID = editionID;
                            eal.EmailID = EmailID;
                            eal.BlastID = BlastID;
                            eal.PageID = null;
                            eal.LinkID = 0;
                            eal.ActionTypeCode = "subscribe";
                            eal.ActionValue = "Subscription from Digital Edition";
                            eal.IPAddress = IP;
                            eal.SessionID = SessionID;
                            eal.IsAnonymous = EmailID > 0 ? false : true;
                            eal.PageStart = null;
                            eal.PageEnd = null;
                            eal.CreatedUserID = Convert.ToInt32(user.UserID);
                            eal.CustomerID = user.CustomerID;

                            try
                            {
                                ECN_Framework_BusinessLayer.Publisher.EditionActivityLog.Save(eal, user);
                            }
                            catch (ECN_Framework_Common.Objects.ECNException ecnex)
                            {
                                StringBuilder sb = new StringBuilder();

                                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                                {
                                    sb.Append(err.ErrorMessage + "<BR>");
                                }
                                ErrorMessage = sb.ToString();
                            }
                        }

                        //update Anonymous profile for this session with EmailID in EditionActivityLog Table

                        List<ECN_Framework_Entities.Publisher.EditionActivityLog> eActivityLog = ECN_Framework_BusinessLayer.Publisher.EditionActivityLog.GetByEditionIDSessionID(editionID, SessionID, user);

                        if (eActivityLog != null)
                        {
                            foreach (ECN_Framework_Entities.Publisher.EditionActivityLog al in eActivityLog)
                            {
                                al.EmailID = EmailID;
                                al.IsAnonymous = false;
                                al.UpdatedUserID = user.UserID;
                                ECN_Framework_BusinessLayer.Publisher.EditionActivityLog.Save(al, user);
                            }
                        }
                    }
                }
                catch (ECNException ecnEx)
                {
                    ErrorMessage = "<Error>&lt;b&gt;ERROR:&lt;/b&gt;&lt;BR&gt;&lt;BR&gt;This error has been logged.&lt;BR&gt;&lt;a href='mailto:accountmanagers@knowledgemarketing.com&subject=Need Help'&gt;Click here&lt;/a&gt; to report this error.</Error>";
                }
                catch (Exception e)
                {
                    ECNTasks.insertECNTask(CustomerID, "Digital Edition", e.Message, "Magazine.aspx", e.Message + "\n" + e.Source + "\n" + e.StackTrace + "\n" + e.InnerException, "medium", "pending", "EditionID = " + editionID + "\nEmailaddress=" + emailaddress);
                    ErrorMessage = "<Error>&lt;b&gt;ERROR:&lt;/b&gt;&lt;BR&gt;&lt;BR&gt;This error has been logged.&lt;BR&gt;&lt;a href='mailto:accountmanagers@knowledgemarketing.com&subject=Need Help'&gt;Click here&lt;/a&gt; to report this error.</Error>";
                }
            }

            return "<XML><EmailID>" + EmailID + "</EmailID>" + ErrorMessage + "</XML>";
        }

        private static string ReplaceCodeSnippets(int EmailID, int CustomerID, int GroupID, string ImgPath, string EditionName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ConfigurationManager.AppSettings["SubscribeEmailTemplate"].ToString());

            sb = sb.Replace("%%EditionName%%", EditionName);
            sb = sb.Replace("%%cover%%", ConfigurationManager.AppSettings["ImagePath"] + ImgPath + "150/1.png");

            sb = sb.Replace("%%Subscriptionlink%%", ConfigurationManager.AppSettings["Activity_DomainPath"] + "engines/subscribe.aspx?ei=" + EmailID + "&b=0&s=S&f=html&c=" + CustomerID + "&g=" + GroupID);

            return sb.ToString();
        }

        private static int SubscribeToGroup(ECN_Framework_Entities.Communicator.Group group, string firstname, string lastname, string EmailAddress, string phone, string fax, string country, string address, string city, string state, string zip, int CustomerID)
        {
            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);

            int EmailID = 0;

            ECN_Framework_Entities.Communicator.Email em = new ECN_Framework_Entities.Communicator.Email();

            ECN_Framework_Entities.Communicator.EmailGroup eg = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(EmailAddress, group.GroupID, user);

            if (eg != null && eg.EmailID > 0)
            {
                em = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailIDGroupID(eg.EmailID, eg.GroupID, user);
            }
            else
            {
                em = new ECN_Framework_Entities.Communicator.Email();
                em.EmailAddress = EmailAddress;
                em.Title = "";
                em.FirstName = firstname;
                em.LastName = lastname;
                em.FullName = firstname + " " + lastname;
                em.Company = "";
                em.Occupation = "";
                em.Address = address;
                em.Address2 = "";
                em.City = city;
                em.State = state;
                em.Zip = zip;
                em.Country = country;
                em.Voice = phone;
                em.Mobile = "";
                em.Fax = fax;
                em.Website = "";
                em.Age = "";
                em.Income = "";
                em.Gender = "";
                em.User1 = "";
                em.User2 = "";
                em.User3 = "";
                em.User4 = "";
                em.User5 = "";
                em.User6 = "";
                em.Notes = "Subscription Thru Digital Edition. DateAdded: " + DateTime.Now.ToString();
                try
                {
                    StringBuilder xmlProfile = new StringBuilder("");
                    xmlProfile.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML><Emails>");
                    xmlProfile.Append("<emailaddress>" + EmailAddress + "</emailaddress>");
                    xmlProfile.Append("<firstname>" + firstname + "</firstname>");
                    xmlProfile.Append("<lastname>" + lastname + "</lastname>");
                    xmlProfile.Append("<fullname>" + firstname + " " + lastname + "</fullname>");
                    xmlProfile.Append("<address>" + address + "</address>");
                    xmlProfile.Append("<city>" + city + "</city>");
                    xmlProfile.Append("<state>" + state + "</state>");
                    xmlProfile.Append("<zip>" + zip + "</zip>");
                    xmlProfile.Append("<country>" + country + "</country>");
                    xmlProfile.Append("<voice>" + phone + "</voice>");
                    xmlProfile.Append("<fax>" + fax + "</fax>");
                    xmlProfile.Append("<notes>" + "Subscription Thru Digital Edition. DateAdded: " + DateTime.Now.ToString() + "</notes>");
                    xmlProfile.Append("</Emails></XML>");
                    ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsOverrideCustomer(user, group.CustomerID, group.GroupID, xmlProfile.ToString(), "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "HTML", "P", false);

                    em.EmailID = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetEmailIDFromWhatEmail(group.GroupID, group.CustomerID, em.EmailAddress, user);

                }
                catch (ECNException ex)
                {
                }
            }

            return em.EmailID;
        }


        [System.Web.Services.WebMethod()]
        public static string GetLinks(int editionID, string Pageno)
        {
            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);

            string lnks = "";

            StringBuilder sblinks = new StringBuilder("");

            List<ECN_Framework_Entities.Publisher.Page> page = ECN_Framework_BusinessLayer.Publisher.Page.GetByEditionID(editionID, Pageno.ToString(), user, true);

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
        public static void LogActivity(int editionID, int EmailID, int BlastID, int PageNo, int LinkID, string ActionCode, string ActionValue, string IP, string SessionID)
        {
            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);

            if (SessionID != string.Empty)
            {
                ECN_Framework_Entities.Publisher.EditionActivityLog eal = new ECN_Framework_Entities.Publisher.EditionActivityLog();
                eal.EditionID = editionID;
                eal.EmailID = EmailID;
                eal.BlastID = BlastID;
                eal.PageNo = PageNo.ToString();
                eal.LinkID = LinkID;
                eal.ActionTypeCode = ActionCode;
                eal.ActionValue = ActionValue;
                eal.IPAddress = IP;
                eal.SessionID = SessionID;
                eal.IsAnonymous = EmailID > 0 ? false : true;
                eal.PageStart = null;
                eal.PageEnd = null;
                eal.CreatedUserID = Convert.ToInt32(user.UserID);
                eal.CustomerID = user.CustomerID;

                try
                {
                    ECN_Framework_BusinessLayer.Publisher.EditionActivityLog.Save(eal, user);
                }
                catch (ECN_Framework_Common.Objects.ECNException ecnex)
                {
                    //StringBuilder sb = new StringBuilder();

                    //foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                    //{
                    //    sb.Append(err.ErrorMessage + "<BR>");
                    //}
                    //ErrorMessage = sb.ToString();
                }
            }
        }

        [System.Web.Services.WebMethod()]
        public static string Search(int editionID, string searchText)
        {
            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);

            StringBuilder sb = new StringBuilder("");
            StringBuilder xml = new StringBuilder("");

            string nodeVal = "";
            int start;
            int at;
            int end;
            int icount = 0;

            if (searchText != string.Empty)
            {
                List<ECN_Framework_Entities.Publisher.Page> page = ECN_Framework_BusinessLayer.Publisher.Page.GetByEditionID(editionID, user, false);

                var xEle = new XElement("xml",
                            from p in page
                            select new XElement("Page",
                                    new XAttribute("id", p.PageNumber),
                                    new XAttribute("displayno", p.DisplayNumber),
                                    new XText("<![CDATA[" + p.TextContent + "]]>")));

                xml.Append(xEle.ToString());

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml("<xml>" + xml.ToString() + "</xml>");

                XPathNavigator xNav;
                XPathNodeIterator xItr;

                xNav = xmldoc.CreateNavigator();

                string xpath = "";

                //if (getCS) 
                // xpath = "//Page[contains(., '" + searchText + "')]";
                //else
                xpath = "//Page[contains(translate(.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), translate('" + searchText + "','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))]";

                xItr = xNav.Select(xpath);

                while (xItr.MoveNext())
                {
                    nodeVal = xItr.Current.Value;
                    end = nodeVal.Length;
                    start = 0;
                    at = 0;

                    sb.Append("<span class='pageNumLink'><a href='javascript:Edition.gotoPage(" + xItr.Current.GetAttribute("id", "") + ")'> Page: " + xItr.Current.GetAttribute("displayno", "") + "</a></span>");
                    while ((start <= end) && (at > -1))
                    {
                        at = nodeVal.ToLower().IndexOf(searchText.ToLower(), start);

                        if (at == -1) break;
                        icount++;
                        int s1 = ((nodeVal.LastIndexOf(" ", at) == -1) ? at : (nodeVal.LastIndexOf(" ", at)));

                        string next50chars = nodeVal.Substring(s1, (nodeVal.Length - s1 > 50 ? 50 : nodeVal.Length - s1));

                        int e1 = next50chars.LastIndexOf(" ") > next50chars.LastIndexOf(".") ? next50chars.LastIndexOf(" ") : next50chars.LastIndexOf(".");

                        if (e1 <= 0)
                            e1 = searchText.Length;

                        sb.Append("<div class='searchList'>&bull;&nbsp;&nbsp;<a class='lbSearchText' href='javascript:Edition.gotoPage(" + xItr.Current.GetAttribute("id", "") + ")'>" + nodeVal.Substring(s1, e1).Trim() + "</a></div>");
                        start = at + 1;
                    }
                }
            }


            if (sb.ToString() != string.Empty)
            {
                return "<table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td width='90%'><span class='searchHeader'><b>Total Instance found : " + icount + "</b></span></td><td>&nbsp;</td></tr><tr><td colspan='2'><HR></td></tr><tr><td colspan='2'><font size='2'>" + sb.ToString() + "</font></td></table>";
            }
            else
            {
                return "<table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td width='90%'><span class='searchHeader'><b>Total Instance found : 0</b></span></td><td>&nbsp;</td></tr><tr><td colspan='2'><HR></td></tr><tr><td colspan='2'><span class='searchHeader'>No matches!</span></td></table>";

            }
        }

        #endregion

        private void LoadPageFirstTime()
        {
            var exists = false;
            txtpageno.Attributes.Add(OnKeyPressAttribute, IgnoreKeysScript);
            txtpageno.Attributes.Add(StyleAttribute, PageNoStyle);
            txtpageno.Text = getPageNo() > 0 ? getPageNo().ToString() : "1";

            if (getPublicationCode() != string.Empty || getEditionID() > 0)
            {
                uSession = CreateUserSession(getPublicationCode(), getEditionID(), user);

                exists = InitializePageForSession();
            }

            if (!exists)
            {
                Response.Redirect($"{ErrorPageName}?err=2");
            }
        }

        private bool InitializePageForSession()
        {
            var exists = false;
            if (uSession != null)
            {
                exists = true;

                lblEditionID.Text = uSession.EditionID.ToString();
                lblIsSecured.Text = FalseNumberString;

                uSession.BlastID = getBlastID();
                uSession.EmailID = getEmailID();

                var edition = Edition.GetByEditionID(uSession.EditionID, user);
                var publication = Publication.GetByPublicationID(edition.PublicationID, user);
                lblTotalPages.Text = uSession.Totalpages.ToString();
                hlLogo.Target = BlankTarget;

                InitializePublication(publication, edition);

                InitializeSubscribeByEmail(publication);

                InitializeLoginBlock();
            }
            else
            {
                Response.Redirect($"{ErrorPageName}?err=1&eid={getEditionID()}");
            }

            return exists;
        }

        private void InitializeLoginBlock()
        {
            var loginRequired = uSession.IsLoginRequired.Value;

            phsbf2f.Visible = !loginRequired;
            phf2f.Visible = !loginRequired;

            if (loginRequired)
            {
                lblIsSecured.Text = TrueNumberString;
                download.Visible = false;
                pnlLogin.Visible = true;
                pnlEdition.Visible = false;
                imgThumbnail.ImageUrl = string.Format("{0}{1}/150/1.png",
                        ConfigurationManager.AppSettings["ImagePath"],
                        uSession.ImagePath);
            }
            else
            {
                DisplayPage();
            }
        }

        private void InitializeSubscribeByEmail(ECN_Framework_Entities.Publisher.Publication publication)
        {
            if (uSession.EmailID == 0)
            {
                return;
            }

            phSubscribe.Visible = publication.SubscriptionFormLink != string.Empty;
        }

        private void InitializePublication(ECN_Framework_Entities.Publisher.Publication publication, ECN_Framework_Entities.Publisher.Edition edition)
        {
            if (publication == null)
            {
                return;
            }

            //Create User Profile for email address
            if (uSession.EmailID == 0 && !string.IsNullOrWhiteSpace(getEmailAddress()))
            {
                try
                {
                    var groupObj = Group.GetByGroupID(publication.GroupID.Value, user);
                    uSession.EmailID = SubscribeToGroup(groupObj,
                        string.Empty,
                        string.Empty,
                        getEmailAddress().Trim(),
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        Convert.ToInt32(publication.CustomerID));
                }
                catch
                {
                    // POSSIBLE BUG: add exception logging there
                }
            }

            _Theme = edition.Theme == string.Empty ? "default" : edition.Theme;
            var downloadUrl = $"/ecn.images/customers/{edition.CustomerID}/publisher/{edition.EditionID}/{edition.FileName}";
            download.HRef = downloadUrl;

            var kmpsLogoChannels = ConfigurationManager.AppSettings["KMPSLogoChannels"].Split(',');
            var customerId = publication.CustomerID;
            var channelId = Customer.GetByCustomerID(customerId, false).BaseChannelID.Value;

            InitializeCustomLogo(kmpsLogoChannels, channelId, customerId);

            SetLogoProperties(publication);

            InitializeSubscriptionLogic(publication);

            if (publication.ContactFormLink == string.Empty)
            {
                InitializeContactForm(publication);
            }
            else
            {
                phContact.Controls.Add(
                    new LiteralControl(string.Format(ContactFormLinkHtml, publication.ContactFormLink)));
            }
        }

        private void InitializeContactForm(ECN_Framework_Entities.Publisher.Publication publication)
        {
            phContact.Controls.Add(new LiteralControl(DefaultContactFormHtml));

            if (publication.ContactEmail != string.Empty)
            {
                lnkContactEmail.NavigateUrl = $"mailto:{publication.ContactEmail}";
                lnkContactEmail.Text = publication.ContactEmail;
            }

            if (publication.ContactPhone != string.Empty)
            {
                pnlContactPhone.Visible = true;
                lblContactPhone.Text = publication.ContactPhone;
            }
            else
            {
                pnlContactPhone.Visible = false;
            }

            if (publication.ContactAddress1 != string.Empty ||
                publication.ContactAddress2 != string.Empty)
            {
                pnlContactAddress.Visible = true;
                lblContactAddress1.Text = publication.ContactAddress1;
                lblContactAddress2.Text = publication.ContactAddress2;
            }
            else
            {
                pnlContactAddress.Visible = false;
            }

            if (publication.ContactFormLink != string.Empty)
            {
                imgContactLogo.ImageUrl = publication.ContactFormLink;
                imgContactLogo.Visible = true;
            }
        }

        private void InitializeSubscriptionLogic(ECN_Framework_Entities.Publisher.Publication publication)
        {
            if (publication.EnableSubscription.Value)
            {
                phSubscribe.Visible = true;
                if (publication.SubscriptionFormLink == string.Empty)
                {
                    phSubscribe.Controls.Add(new LiteralControl(DefaultSubscribeLinkHtml));
                }
                else
                {
                    phSubscribe.Controls.Add(
                        new LiteralControl(string.Format(CustomSubscribeLinkHtml, publication.SubscriptionFormLink)));
                }
            }
            else
            {
                phSubscribe.Visible = false;
            }
        }

        private void SetLogoProperties(ECN_Framework_Entities.Publisher.Publication publication)
        {
            if (publication.LogoURL == string.Empty)
            {
                hlLogo.Text = "<img src='http://www.ecndigitaledition.com/images/km-logo-plate.gif' border='0'>";
                hlLogo.NavigateUrl = "http://www.knowledgemarketing.com";
            }
            else
            {
                hlLogo.Text = $"<img src='{publication.LogoURL}' border='0'>";
                if (publication.LogoLink != string.Empty)
                {
                    hlLogo.NavigateUrl = publication.LogoLink;
                }
            }
        }

        private void InitializeCustomLogo(IEnumerable<string> kmpsLogoChannels, int channelId, int customerId)
        {
            if (kmpsLogoChannels.Any(id => id == channelId.ToString()))
            {
                hlpowered.ImageUrl = "images/powered-by-KMPS.png";
            }

            // Does this code still needed. The link is outdated.
            if (customerId == MPlanet2009)
            {
                hlpowered.ImageUrl = "themes/blue/powered-by-AMA-blue.jpg";
                hlpowered.NavigateUrl = "http://www.mplanet2009.com/home.shtml";
            }
        }

        private void HideMessageLabels()
        {
            lblMessage.Text = string.Empty;
            lblMessage.Visible = false;
            lblEditionID.Attributes.Add(StyleAttribute, DisplayNone);
            lblIsSecured.Attributes.Add(StyleAttribute, DisplayNone);
        }
    }
}