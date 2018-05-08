using System;
using System.Web;
using System.Web.UI.WebControls;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Linq;

namespace ecn.activityengines
{
    public partial class Click : System.Web.UI.Page
    {
        int BlastID = 0;
        int EmailID = 0;
        int BlastLinkID = 0;
        public string Link = string.Empty;

        public KMPlatform.Entity.User User = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);

            string decrypted = Helper.DeCrypt_DeCode_EncryptedQueryString(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
            GetValuesFromQuerystring(decrypted);
            Link = getLinkFromBlastLinkID(BlastLinkID);

            //must have EmailID and BlastID
            if (BlastID == 0 || EmailID == 0 || BlastLinkID == 0 || string.IsNullOrEmpty(Link))
            {
                errorMsgPanel.Visible = true;
            }
            else
            {

                errorMsgPanel.Visible = false;
                mailSenderFormPanel.Visible = false;

                int BlastIDToCheckWith = BlastID;
                try
                {
                    ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID(BlastID, User, false);
                    if (blast.BlastType.ToUpper() == "LAYOUT" || blast.BlastType.ToUpper() == "NOOPEN")
                    {
                        int refBlastID = ECN_Framework_BusinessLayer.Communicator.BlastSingle.GetRefBlastID(BlastID, EmailID, blast.CustomerID.Value, blast.BlastType);
                        BlastIDToCheckWith = refBlastID;
                    }
                }
                catch (Exception) { }

                if (ECN_Framework_BusinessLayer.Communicator.EmailGroup.ValidForTracking(BlastIDToCheckWith, EmailID))
                {
                    int eaid = TrackData();
                    if (eaid > 0)
                    {
                        //track topics in trans UDF
                        if (ContainsTopics(Link))
                        {
                            LogTransactionalUDF(BlastID, EmailID, Link);
                        }
                    }
                }

                if (Link.ToLower().ToString().StartsWith("mailto:"))
                {
                    // The following code will open a blank send email window when some one clicks on the mailto link in an email. But it will open a blank page. To 
                    // avoid it, we are redirecting back to www.ecn5.com. We can put some sensible text in this same page as well.. 
                    mailSenderFormPanel.Visible = true;
                }
                else
                {
                    mailSenderFormPanel.Visible = false;

                    //The following is a patch for BlackBerry users.. it didn't support the java script method of recirecting the URL's. the following will do a straight redirect to the link.
                    string useragent = Request.UserAgent.ToString();
                    if (useragent.ToLower().Contains("blackberry"))
                    {
                        Response.Redirect(Link, false);
                    }
                    else
                    {
                        Response.Write("<script language='javascript'>window.location.href='" + Link.Replace("'", "\\'") + "'</script>");
                    }
                }
            }
        }

        #region Get Request Variables
        private void GetValuesFromQuerystring(string queryString)
        {
            KM.Common.QueryString qs = KM.Common.QueryString.GetECNParameters(queryString);
            int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.BlastID).ParameterValue, out BlastID);
            int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.EmailID).ParameterValue, out EmailID);
            int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.BlastLinkID).ParameterValue, out BlastLinkID);
        }        
        #endregion

        private bool ContainsTopics(string link)
        {
            bool found = false;

            if ((!string.IsNullOrEmpty(Link)) && link.ToLower().Contains("topic="))
            {
                found = true;
            }

            return found;
        }

        private string getLinkFromBlastLinkID(int BlastLinkID)
        {
            string theLink = string.Empty;            
            if (BlastLinkID != 0)
            {
                ECN_Framework_Entities.Communicator.BlastLink blastLink = ECN_Framework_BusinessLayer.Communicator.BlastLink.GetByBlastLinkID(BlastID, BlastLinkID);
                if (blastLink != null && (!string.IsNullOrEmpty(blastLink.LinkURL)))
                {
                    theLink = Server.UrlDecode(LinkCleanUP(blastLink.LinkURL));

                    //if UDFs exist
                    if (theLink.IndexOf("%%") >= 0)
                    {
                        theLink = replaceUDFWithValue(theLink);
                    }
                }
            }
            return theLink.Trim();
        }

        private string replaceUDFWithValue(string theLink)
        {
            string modifiedLink = "";
            string udfName = "";

            try
            {
                DataTable dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.FilterEmailsAllWithSmartSegment(EmailID, BlastID);

                if (dt.Rows.Count > 0)
                {
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("%%.+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    System.Text.RegularExpressions.MatchCollection matchList = reg.Matches(theLink);
                    foreach (System.Text.RegularExpressions.Match m in matchList)
                    {
                        udfName = m.Value.ToString().Replace("%%", string.Empty);
                        theLink = theLink.Replace("%%" + udfName + "%%", dt.Rows[0][udfName].ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
            modifiedLink = theLink;

            return modifiedLink;
        }

        private void LogTransactionalUDF(int BlastID, int EmailID, string link)
        {
            try
            {
                int start = link.ToLower().IndexOf("topic=") + 6;
                int totalLength = link.Length;
                if (totalLength - start > 0)
                {
                    string udfValue = link.Substring(link.ToLower().IndexOf("topic=") + 6, link.Length - (link.ToLower().IndexOf("topic=") + 6));

                    if (ECN_Framework_BusinessLayer.Communicator.EmailDataValues.RecordTopicsValue(BlastID, EmailID, udfValue) == 0)
                    {
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("UDF of Topics not found in db", "Click.LogTransactionalUDF", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                        NotifyOfMissingTopicUDF();
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Click.LogTransactionalUDF(Unknown Issue)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                }
                catch (Exception)
                {
                }
            }
        }

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<BR><BR>BlastID: " + BlastID.ToString());
                adminEmailVariables.AppendLine("<BR>EmailID: " + EmailID.ToString());
                adminEmailVariables.AppendLine("<BR>BlastLinkID: " + BlastLinkID.ToString());
                adminEmailVariables.AppendLine("<BR>Page URL: " + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + Request.RawUrl.ToString());
                adminEmailVariables.AppendLine("<BR>SPY Info:&nbsp;[" + Request.UserHostAddress + "] / [" + Request.UserAgent + "]");
                if (Request.UrlReferrer != null)
                {
                    adminEmailVariables.AppendLine("<BR>Referring URL: " + Request.UrlReferrer.ToString());
                }
                adminEmailVariables.AppendLine("<BR>HEADERS");
                var headers = String.Empty;
                foreach (var key in Request.Headers.AllKeys)
                    headers += "<BR>" + key + ":" + Request.Headers[key];
                adminEmailVariables.AppendLine(headers);
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        private void NotifyOfMissingTopicUDF()
        {
            if (ConfigurationManager.AppSettings["Admin_Notify"] == "true")
            {
                string adminEmailSubject = "Missing 'Topics' UDF for Blast";
                StringBuilder adminEmailBody = new StringBuilder("Activity Engines encountered a link with a 'topics' code but none was found in the group.");
                adminEmailBody.AppendLine("<BR><BR>BlastID: " + BlastID.ToString());
                adminEmailBody.AppendLine("<BR>EmailID: " + EmailID.ToString());
                adminEmailBody.AppendLine("<BR>BlastLinkID: " + BlastLinkID.ToString());
                adminEmailBody.AppendLine("<BR>Page URL: " + Request.RawUrl.ToString());

                EmailFunctions emailFunctions = new EmailFunctions();
                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], adminEmailSubject, adminEmailBody.ToString());


            }
        }

        private int TrackData()
        {
            int eaid = 0;
            string spyinfo = string.Empty;

            //spyinfo = Request.UserHostAddress + " | " + Request.UserAgent;
            try
            {
                if (Request.UserHostAddress != null)
                {
                    spyinfo = Request.UserHostAddress;
                }
                if (Request.UserAgent != null)
                {
                    if (spyinfo.Length > 0)
                    {
                        spyinfo += " | ";
                    }
                    spyinfo += Request.UserAgent;
                }
                eaid = EmailActivityLog.InsertClick(EmailID, BlastID, Link.Replace("'", "''").Replace("'", "''"), spyinfo);
                //new framework doesn't have code for the eventer
                //eaid = ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertClick(EmailID, BlastID, Link.Replace("'", "''"), spyinfo);
            }
            catch (Exception ex)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Click.TrackData(Unknown Issue)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                }
                catch (Exception)
                {
                }
            }
            return eaid;        
        }

        private string LinkCleanUP(string link)
        {            
            link = link.Replace("&amp;", "&");
            link = link.Replace("&lt;", "<");
            link = link.Replace("&gt;", ">");            

            return link;
        }

        //private string DeCrypt_DeCode_EncryptedQueryString(string encrypted_Qstring)
        //{
        //    KM.Common.Encryption ec = new KM.Common.Encryption();// KM.Common.Encryption.Get();
        //    ec.PassPhrase = ConfigurationManager.AppSettings["ECN_Pass"];
        //    ec.SaltValue = ConfigurationManager.AppSettings["ECN_Salt"];
        //    ec.HashAlgorithm = ConfigurationManager.AppSettings["ECN_Hash"];
        //    ec.PasswordIterations = Convert.ToInt32(ConfigurationManager.AppSettings["ECN_Iter"]);
        //    ec.InitVector = ConfigurationManager.AppSettings["ECN_Vector"];
        //    ec.KeySize = Convert.ToInt32(ConfigurationManager.AppSettings["ECN_KeySize"]);

        //    //ec.PassPhrase = "p$yaQat3?U@r5truX6Vepra++8?&68t8-uB9CuW?UtHaZapUJ-2e8&!3-du2AMA*";
        //    //ec.SaltValue = "7emAha2hEdrUCephekas3uzuje6uGasab5Axu5t64u8a*HEyUtr9pr+bra4uJeXE";
        //    //ec.HashAlgorithm = "SHA1";
        //    //ec.PasswordIterations = 2;
        //    //ec.InitVector = "d3EdrEp=ucR-cAwr";
        //    //ec.KeySize = 256;

        //    string encryptedQuery = System.Web.HttpContext.Current.Server.UrlDecode(encrypted_Qstring);
        //    string decryptedQuery = ec.Decrypt(encryptedQuery, ec.PassPhrase, ec.SaltValue, ec.HashAlgorithm, ec.PasswordIterations, ec.InitVector, ec.KeySize);
        //    return decryptedQuery;
        //}

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