using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using aspNetEmail;
using ecn.common.classes;
using ecn.communicator.classes.EmailWriter;
using ecn.communicator.classes.Port25;
using ECN_Framework_BusinessLayer.Communicator;
using ECN.Communicator.EmailBuilder;

namespace ecn.communicator.classes
{

    /// The EmailBlast class sends out a blast for an already created piece of content. It holds the email messasge and the Emails table.   
    /// You create the message and add emails to it via the AddEmail method. Then call Blast() and it will send the message.                
    /// It then threads the blast and updates the DB when finished.

    public class EmailBlast
    {
        private const string Or = "||";
        private const string DoblastEndLog = "EmailBlast: DoBlast() - ended at: ";
        private const string MemoryGcComplete = "Memory: GC Complete";
        private const string Memory = "Memory: ";
        private const string BreakupHtmlMailCount = "lBreakupHTMLMail Count : ";
        private const bool ForceFullCollection = true;
        private const string BreakupTextMailCount = "this.BreakupTextMail Count : ";
        private const string BreakupSubjectCount = "this.BreakupSubject Count : ";
        private const string BreakupFromNameCount = "this.BreakupFromName Count : ";
        private const string BreakupFromEmailCount = "this.BreakupFromEmail Count : ";
        private const string BreakupReplyToEmailCount = "this.breakupReplyToEmail Count : ";
        private const string StartingEmailBlast = "Starting EmailBlast.Blast: ";
        private const string EndingEmailBlast = "Ending EmailBlast.Blast: ";
        private const string Statistics = "statistics_";
        private const bool OutputToConsole = false;
        private const string UnderScore = "_";
        private const string Slash = "/";
        private const string Dash = "-";
        private const string EmailBlastDoStartedAt = "EmailBlast: DoBlast() - started at: ";
        private const string TransnippetPattern = "<(table)([a-zA-Z0-9\"'=\\s_]*id=[\"']transnippet_[^>]*?[\"'].*?)(<tr.*?id=[\"']header[\"'].*?</tr>)(.*?<tr.*?id=[\"']detail[\"'].*?</tr>).*?</\\1>";
        private const string TextTransnippetFailed = "Building text transnippet failed.";
        private const string SociaShareUsedCountText = "this.SocialShareUsed Count : ";
        private const string LessThan = "<";
        private const string GreaterThen = ">";
        private const string NewLineEscaped = "\\n";
        private const string EndLine = "---------------------------------------------------------------------------------";
        private const string StartingLoopText = "Starting Loop ---";
        private const string DoublePercentage = "%%";
        private const string HtmlTransnippetMark = "##TRANSNIPPET|";
        private const string NewLine = "\n";
        private const string RegexPatternHtmlMark = "#{2}.*#{2}";
        private const char SingleOr = '|';
        private const string DoubleDollarSign = "$$";
        private const string DoubleHashSign = "##";
        private const char CommaSeparator = ',';
        private const string Tab = "\t";
        private const string DoubleQuote = "\"";

        #region Getters & Setters
        public int BlastID { get; set; }
        public string HTML { get; set; }
        public string TEXT { get; set; }

        public string type;

        private LicenseCheck licenseCheck;

        bool email_resend;
        
        private int count;
        private int update_count;

        bool test_blast;
        public bool TestBlast
        {
            get { return test_blast; }
            set { test_blast = value; }
        }

        //objects/arrays
        private Dictionary<int, int> _PreviousEmails;

        public Dictionary<int, string> SocialShareUsed;
       

        public List<string> DynamicTags;

        //public List<string> HTMLCodesnippets { get; set; }
        //public List<string> TextCodesnippets { get; set; }
        //public List<string> SubjectCodesnippets { get; set; }
        //public List<string> FromNameCodesnippets { get; set; }
        //public List<string> FromEmailCodesnippets { get; set; }
        //public List<string> ReplyToEmailCodesnippets { get; set; }

        public Array BreakupSubject { get; set; }
        public Array BreakupHTMLMail { get; set; }
        public Array BreakupTextMail { get; set; }

        public Array BreakupFromEmail { get; set; }
        public Array BreakupFromName { get; set; }
        public Array breakupReplyToEmail { get; set; }

        public bool HasTransnippets { get; set; }

        public int SendTotal { get; set; }

        public int LastUpdate { get; set; }

        /// The body of the HTML message we will send

        //--
        //--Added for Email Personalization - Ashok 01/12/09
        //--
        public string dynamicFromName { get; set; }
        public string dynamicFromEmail { get; set; }
        public string dynamicReplyToEmail { get; set; }


        /// The EmailMessage that we will be blasting
        EmailMessage msg;
        public EmailMessage blast_msg
        {
            get { return msg; }
            set { msg = value; }
        }

        public BlastConfig blastconfig = null;

        /// EmailsTable that we will be blasting to. Only use for non-dynmic emails.
        DataTable emails_table;
        public DataTable EmailsTable
        {
            get { return emails_table; }
            set { emails_table = value; }
        }

        public int TransnippetsCount { get; set; }
        public List<string> Transnippet;
        public List<string> TransnippetTables;
        public List<string> TransnippetTablesTxt;

        public KM.Common.Entity.Encryption KMEncryption { get; set; }

        public ECN_Framework_Entities.Communicator.Blast BlastEntity { get; set; }
        private ECN_Framework.Common.ChannelCheck _ChannelCheck { get; set; }

        private Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> _PersonalizedContent;

        #endregion

        #region Constructors
        private EmailBlast()
        {
            this.licenseCheck = new LicenseCheck();
            this.msg = new EmailMessage();
            InitializeEmailMessage();
            _PreviousEmails = new Dictionary<int, int>();
        }

        private void InitializeEmailMessage()
        {
            //msg.BodyFormat = MailFormat.Html;
            msg.CharSet = "ISO-8859-1"; //international standard encoding
            msg.CharSetHeader = "ISO-8859-1"; //international standard encoding
            msg.XMailer = "Microsoft Outlook, Build 10.0.2627"; //fake it into thinking we are Outlook
            msg.Organization = "Microsoft"; //add this to prevent rejection on lack of org
            msg.MMPrefix = "%%";
            msg.MMSuffix = "%%";
            msg.ReversePath = "%%BounceAddress%%";
            msg.MSPickupDirectory = @ConfigurationManager.AppSettings["mailPickupDirectory"];
            msg.AddTo("%%EmailAddress%%");
        }

        // Hold the data for a Dynamic email until we are ready to blast it.
        public EmailBlast(int BlastID, DataTable email_clone, string html_body, string text_body, bool Resend, bool TestBlast, string dynamicFromName, string dynamicFromEmail, string dynamicReplyToEmail, bool omitDocType, ECN_Framework_Entities.Communicator.Blast blast)
            : this()
        {

            this.BlastID = BlastID;
            this.BlastEntity = blast;
            this.emails_table = email_clone;
            this.TEXT = text_body;

            this._ChannelCheck = new ECN_Framework.Common.ChannelCheck(this.BlastEntity.CustomerID.Value);

            MessageObject mo = new MessageObject();
            mo.KMHEAD = ("<HEAD><TITLE>%%EmailSubject%%</TITLE><META http-equiv=\"Content-Type content=text/html; charset=ISO-8859-1\"></HEAD>").Replace("%%EmailSubject%%", this.BlastEntity.EmailSubject);
            mo.KMBODY = ("<BODY style=\"PADDING-RIGHT:0px;PADDING-LEFT:0px;PADDING-TOP:0px;PADDING-BOTTOM:0px;MARGIN-TOP:0px;MARGIN-LEFT:0px;MARGIN-RIGHT: 0px;\">%%BODY%%</BODY>").Replace("%%BODY%%", html_body);

            if (!omitDocType)
            {
                html_body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                "<HTML><HEAD><TITLE>" + this.BlastEntity.EmailSubject + "</TITLE>" +
                "<META http-equiv=\"Content-Type content=text/html; charset=ISO-8859-1\">" +
                "</HEAD>" + "<BODY style=\"PADDING-RIGHT:0px;PADDING-LEFT:0px;PADDING-TOP:0px;PADDING-BOTTOM:0px;MARGIN-TOP:0px;MARGIN-LEFT:0px;MARGIN-RIGHT: 0px;\">" + html_body + "</BODY></HTML>";
            }
            else
            {
                bool useKMDOC = false;
                bool useKMHTML = false;
                bool useKMHEAD = false;
                bool useKMBODY = false;
                bool foundTemplateStart = false;
                string tempHTML = html_body.ToLower();
                if (tempHTML.Contains("<!doctype"))
                {
                    useKMDOC = false;
                    string DocType = html_body.Substring(tempHTML.IndexOf("<!doctype"), tempHTML.IndexOf(">", tempHTML.IndexOf("<!doctype")) - tempHTML.IndexOf("<!doctype") + 1);

                    mo.EmailDOC = DocType;
                    mo.EmailTemplateStart = html_body.Substring(0, tempHTML.IndexOf("<!doctype"));
                    foundTemplateStart = true;
                    //html_body = html_body.Replace(DocType, "");

                    //html_body = DocType +
                    //"<HTML><HEAD><TITLE>" + blast_info["EmailSubject"].ToString() + "</TITLE>" +
                    //"<META http-equiv=\"Content-Type content=text/html; charset=ISO-8859-1\">" +
                    //"</HEAD>" + "<BODY style=\"PADDING-RIGHT:0px;PADDING-LEFT:0px;PADDING-TOP:0px;PADDING-BOTTOM:0px;MARGIN-TOP:0px;MARGIN-LEFT:0px;MARGIN-RIGHT: 0px;\">" + html_body + "</BODY></HTML>";
                }
                else
                {
                    useKMDOC = true;
                    //html_body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                    //"<HTML><HEAD><TITLE>" + blast_info["EmailSubject"].ToString() + "</TITLE>" +
                    //"<META http-equiv=\"Content-Type content=text/html; charset=ISO-8859-1\">" +
                    //"</HEAD>" + "<BODY style=\"PADDING-RIGHT:0px;PADDING-LEFT:0px;PADDING-TOP:0px;PADDING-BOTTOM:0px;MARGIN-TOP:0px;MARGIN-LEFT:0px;MARGIN-RIGHT: 0px;\">" + html_body + "</BODY></HTML>";
                }

                if (mo.rgHTMLStart.IsMatch(tempHTML))
                {
                    useKMHTML = false;
                    Match htmlStart = mo.rgHTMLStart.Match(tempHTML);
                    int indexStart = mo.rgHTMLStart.Match(tempHTML).Index;
                    int indexEnd = htmlStart.Length;
                    string htmlTag = html_body.Substring(indexStart, indexEnd);
                    mo.EmailHTMLTAG = htmlTag;

                    if(!foundTemplateStart)
                    {
                        mo.EmailTemplateStart = html_body.Substring(0, indexStart);
                        foundTemplateStart = true;
                    }
                }
                else
                {
                    useKMHTML = true;
                }

                if(mo.rgHeadStart.IsMatch(tempHTML))
                {
                    //need to grab everything in the head tags
                    useKMHEAD = false;
                    Match headEnd = mo.rgHeadEnd.Match(tempHTML);
                    int indexStart = mo.rgHeadStart.Match(tempHTML).Index;
                    int indexEnd = headEnd.Index + headEnd.Length;
                    string headTag = html_body.Substring(indexStart, indexEnd - indexStart);
                    mo.EmailHEAD = headTag;

                    if(!foundTemplateStart)
                    {
                        mo.EmailTemplateStart = html_body.Substring(0, indexStart);
                        foundTemplateStart = true;
                    }
                }
                else
                {
                    useKMHEAD = true;
                }

                if (mo.rgBodyStart.IsMatch(tempHTML))
                {
                    //Need to grab everything in the body
                    useKMBODY = false;
                    Match bodyEnd = mo.rgBodyEnd.Match(tempHTML);
                    Match bodyStart = mo.rgBodyStart.Match(tempHTML);
                    int indexStart = bodyStart.Index;
                    int indexEnd = bodyEnd.Index + bodyEnd.Length;
                    string bodyTag = html_body.Substring(indexStart, indexEnd - indexStart);

                    if(!foundTemplateStart)
                    {
                        mo.EmailTemplateStart = html_body.Substring(0,indexStart);
                        foundTemplateStart = true;
                    }

                    string templateFooter = "";
                    if(!useKMHTML)
                    {
                        Match HTMLEnd = mo.rgHTMLEnd.Match(tempHTML);
                        templateFooter = html_body.Substring(HTMLEnd.Index + HTMLEnd.Length);
                        
                    }
                    else if(!useKMBODY)
                    {
                        Match BodyEnd = mo.rgBodyEnd.Match(tempHTML);
                        templateFooter = html_body.Substring(BodyEnd.Index + BodyEnd.Length);
                    }

                    bodyTag = bodyTag.Insert(mo.rgBodyStart.Match(bodyTag).Length, mo.EmailTemplateStart);
                    bodyTag = bodyTag.Insert(mo.rgBodyEnd.Match(bodyTag).Index, templateFooter);

                   
                    
                    mo.EmailBODY = bodyTag;
                }
                else
                {
                    useKMBODY = true;
                }

                html_body = mo.BuildHTML(useKMDOC, useKMHTML, useKMHEAD, useKMBODY);
            }

            if (TestBlast)
            {
                //Insert "TEST BLAST" at top of body
                int firstBodyBracketIndex = mo.rgBodyStart.Match(html_body).Index;
                int firstCloseBodyBracketIndex = html_body.Substring(firstBodyBracketIndex).IndexOf(">");
                html_body = html_body.Insert(firstBodyBracketIndex + firstCloseBodyBracketIndex + 1, string.Format("<div align=\"center\"><br /><span style=\"color:black;font-size:15pt\">TEST EMAIL {0}-{1}</span></div>", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
                //html_body = html_body.Insert(firstBodyBracketIndex + firstCloseBodyBracketIndex + 1, "<div align=\"center\"><br /><span style=\"color:black;font-size:15pt\">TEST EMAIL</span></div>");
                //Insert "TEST BLAST" at bottom
                int lastBodyBracketIndex = mo.rgBodyEnd.Match(html_body).Index;
                html_body = html_body.Insert(lastBodyBracketIndex, string.Format("<div align=\"center\"><br /><span style=\"color:black;font-size:15pt\">TEST EMAIL {0}-{1}</span></div>", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
                //html_body = html_body.Insert(lastBodyBracketIndex, "<div align=\"center\"><br /><span style=\"color:black;font-size:15pt\">TEST EMAIL</span></div>");

                text_body = string.Format("TEST EMAIL {0}-{1} \n {2} \n TEST EMAIL {0}-{1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString() ,text_body );

            }

            #region Social Share (new - Append the enable social media icons after the body)
            try
            {
                //New social media subscriber share stuff
                KMPlatform.Entity.User userSM = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);
                ECN_Framework_Entities.Communicator.CampaignItem ci = new ECN_Framework_Entities.Communicator.CampaignItem();
                if (TestBlast)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemTestBlast citb = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByBlastID_NoAccessCheck(BlastID, false);
                    ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemTestBlastID_NoAccessCheck(citb.CampaignItemTestBlastID,  false);
                }
                else
                {
                    ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(BlastID, false);
                }

                List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> listCISM = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.GetByCampaignItemID(ci.CampaignItemID).OrderBy(x => x.SocialMediaID).ToList();
                StringBuilder socialString = new StringBuilder();
                if (listCISM.FindAll(x => x.SimpleShareDetailID == null).Count > 0)
                {
                    socialString.Append("<table style=\"background-color:#FFFFFF;\"><tr style=\"background-color:#FFFFFF;text-decoration:none;border:none;\">");

                    foreach (ECN_Framework_Entities.Communicator.CampaignItemSocialMedia cism in listCISM)
                    {

                        if (cism.SimpleShareDetailID == null)
                        {
                            ECN_Framework_Entities.Communicator.SocialMedia sm = ECN_Framework_BusinessLayer.Communicator.SocialMedia.GetSocialMediaByID(cism.SocialMediaID);
                            if (cism.SocialMediaID == 1)
                            {
                                socialString.Append("<td><a href=\"" + sm.MatchString + "\"><img width=\"30px\" style=\"text-decoration:none;border:none;height:30px;width:30px;\" src=\"" + ConfigurationManager.AppSettings["Image_DomainPath"].ToString() + sm.ImagePath + "\" alt=\"Facebook\" /></a></td>");
                            }
                            else if (cism.SocialMediaID == 2)
                            {
                                socialString.Append("<td><a href=\"" + sm.MatchString + "\"><img style=\"text-decoration:none;border:none;height:30px;width:30px;\" src=\"" + ConfigurationManager.AppSettings["Image_DomainPath"].ToString() + sm.ImagePath + "\" alt=\"Twitter\" /></a></td>");
                            }
                            else if (cism.SocialMediaID == 3)
                            {
                                socialString.Append("<td><a href=\"" + sm.MatchString + "\"><img style=\"text-decoration:none;border:none;height:30px;width:30px;\" src=\"" + ConfigurationManager.AppSettings["Image_DomainPath"].ToString() + sm.ImagePath + "\" alt=\"LinkedIn\" /></a></td>");
                            }
                            else if (cism.SocialMediaID == 4)
                            {
                                try
                                {

                                    if (!string.IsNullOrEmpty(cism.PageID))
                                    {
                                        StringBuilder sbFBLike = new StringBuilder();
                                        sbFBLike.Append("<a href=\"" + sm.ShareLink.ToString().Replace("|link|", "https://www.facebook.com/" + cism.PageID) + "\">");
                                        sbFBLike.Append("<img src=\"" + ConfigurationManager.AppSettings["Image_DomainPath"].ToString() + sm.ImagePath + "\" alt=\"Facebook Like\" style=\"border:none;height:30px;width:30px;text-decoration:none;\" /></a>");
                                        socialString.Append("<td>" + sbFBLike.ToString() + "</td>");
                                    }
                                }
                                catch { }
                            }
                            else if (cism.SocialMediaID == 5)
                            {
                                socialString.Append("<td><a href=\"" + sm.MatchString + "\"><img style=\"text-decoration:none;border:none;height:30px;width:30px;\" src=\"" + ConfigurationManager.AppSettings["Image_DomainPath"].ToString() + sm.ImagePath + "\" alt=\"Forward\" /></a></td>");
                            }
                        }
                    }
                    socialString.Append("</tr></table>");
                    html_body = html_body.Insert(html_body.ToString().IndexOf(">", html_body.ToString().ToLower().IndexOf("<body")) + 1, socialString.ToString());

                }

            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "EmailBlast().SubscriberShare", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
            }

            #endregion

            #region RSSFEED

            
            int CustomerID = this.BlastEntity.CustomerID.Value;
            ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck(CustomerID);
            ECN_Framework_BusinessLayer.Communicator.ContentReplacement.RSSFeed.ReplaceWithHandler(ref html_body, ref text_body, CustomerID, BlastID, (eventArgs) =>
            {
                if (!ECN_Framework_BusinessLayer.Communicator.BlastRSS.ExistsByBlastID_FeedID(BlastID, eventArgs.Context.RSSFeed.FeedID))
                {
                    ECN_Framework_Entities.Communicator.BlastRSS rssToSave = new ECN_Framework_Entities.Communicator.BlastRSS();
                    rssToSave.BlastID = BlastID;
                    rssToSave.FeedHTML = eventArgs.HTML;
                    rssToSave.FeedTEXT = eventArgs.Text;
                    rssToSave.FeedID = eventArgs.Context.RSSFeed.FeedID;
                    rssToSave.Name = eventArgs.Context.RSSFeed.Name;
                    ECN_Framework_BusinessLayer.Communicator.BlastRSS.Save(rssToSave);
                }
                string redirpage = ConfigurationManager.AppSettings["Activity_DomainPath"].ToString();
                if (ConfigurationManager.AppSettings["OpenClick_UseOldSite"].Equals("false"))
                    redirpage = ConfigurationManager.AppSettings["MVCActivity_DomainPath"].ToString();

                eventArgs.HTML = TemplateFunctions.LinkReWriter(eventArgs.HTML, this.BlastEntity, CustomerID.ToString(), ConfigurationManager.AppSettings["Communicator_VirtualPath"].ToString(), cc.getHostName());
                eventArgs.HTML = Regex.Replace(eventArgs.HTML, redirpage + "[^\"']*?lid=(\\d+)(?:&ulid=(\\d+))?&l=.*?([\"'])", (m) =>
                {
                    return string.IsNullOrEmpty(m.Groups[2].Value) ? String.Format(@"{0}%%ECN_Encrypt_{1}%%{2}", redirpage, m.Groups[1].Value, m.Groups[3].Value) : String.Format(@"{0}%%ECN_Encrypt_{1}_{2}%%{3}", redirpage, m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value);
                }, RegexOptions.Singleline | RegexOptions.Compiled);

                eventArgs.Text = TemplateFunctions.LinkReWriterText(eventArgs.Text, this.BlastEntity, CustomerID.ToString(), ConfigurationManager.AppSettings["Communicator_VirtualPath"].ToString(), cc.getHostName(), "");
                eventArgs.Text = Regex.Replace(eventArgs.Text, "<" + redirpage + "[^>]*?lid=(\\d+)&l=.*?>", String.Format(@"<{0}%%ECN_Encrypt_$1%%>", redirpage), RegexOptions.Singleline | RegexOptions.Compiled);
            });

            #endregion

            //this.msg.HtmlBodyPart = html_body;
            this.HTML = html_body;
            this.TEXT = text_body;
            this.msg.Subject = this.BlastEntity.EmailSubject;
            this.msg.FromName = this.BlastEntity.EmailFromName;
            this.msg.FromAddress = this.BlastEntity.EmailFrom;
            this.msg.ReplyTo = this.BlastEntity.ReplyTo;
            this.email_resend = Resend;
            this.SendTotal = 0;
            this.test_blast = TestBlast;
            this.type = this.BlastEntity.BlastType;
            this.KMEncryption = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
            //this.sample_count = sample_number;
            this.dynamicFromEmail = System.Web.HttpUtility.HtmlDecode(dynamicFromEmail);
            this.dynamicFromName = System.Web.HttpUtility.HtmlDecode(dynamicFromName);
            this.dynamicReplyToEmail = System.Web.HttpUtility.HtmlDecode(dynamicReplyToEmail);
            this.blastconfig = new BlastConfig(this.BlastEntity.CustomerID.Value);
        }
        #endregion

        /// Add an Email from the Emails table to this blast

        /// <param name="email"> A DataRow from the Emails Table that we want to get this dynamic message</param>
        public void AddEmail(DataRow email)
        {
            emails_table.ImportRow(email);
        }

        public void Blast()
        {
            LogBegin();
            var totalEmailCount = ProcessEmailsTable();
            LogEnd();
            SetCounts(totalEmailCount);
            WriteEndStatus();
        }

        #region DOBLAST()

        private int DoBlast(Array row_holder, int current_email_pos)
        {
            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, string.Format("Starting EmailBlast.DoBlast: {0}", BlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

            Dictionary<int, DataRow> listDataRows = new Dictionary<int, DataRow>();

            StringBuilder sbXMLPersonalizedContentID = new StringBuilder();

            int total_emails = row_holder.Length;
            int go_up_to = current_email_pos + 1000; //wgh - for testing make this 2
            if (go_up_to > total_emails) go_up_to = total_emails;

            count = 0;
            update_count = 0;
            Console.WriteLine(string.Format("Processing {0} - {1} / {2} ", current_email_pos, go_up_to, DateTime.Now));
            try
            {
                int currentRowEmailID = 0;

                for (int main_counter = current_email_pos; main_counter < go_up_to; main_counter++)
                {
                    DataRow dr = (DataRow)row_holder.GetValue(main_counter);

                    currentRowEmailID = Convert.ToInt32(dr["EmailID"].ToString());

                    if (!_PreviousEmails.ContainsKey(currentRowEmailID))
                    {
                        count++;
                        update_count++;

                        _PreviousEmails.Add(currentRowEmailID, currentRowEmailID);

                        if (this.type.Equals(ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Personalization.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            if (!(string.IsNullOrEmpty(dr["PersonalizedContentID"].ToString()) || int.Parse(dr["PersonalizedContentID"].ToString()) == 0))
                            {
                                sbXMLPersonalizedContentID.Append(string.Format("<id>{0}</id>", dr["PersonalizedContentID"].ToString()));
                            }
                        }

                        listDataRows.Add(currentRowEmailID, dr);
                    }
                }

                var starttick1 = Environment.TickCount;

                if (sbXMLPersonalizedContentID.ToString() != string.Empty)
                {
                    _PersonalizedContent = ECN_Framework_BusinessLayer.Content.PersonalizedContent.GetDictionaryByPersonalizedContentIDs("<xml>" + sbXMLPersonalizedContentID.ToString() + "</xml>");
                }

                var endtick1 = Environment.TickCount;

                Console.WriteLine(string.Format(" Get _PersonalizedContent Dictionary // ProcessTime : {0} // {1} ", (endtick1 - starttick1), DateTime.Now));

                ParallelOptions options = new ParallelOptions();

                try
                {
                    options.MaxDegreeOfParallelism = int.Parse(ConfigurationManager.AppSettings["ParallelThreads"].ToString());
                }
                catch
                {
                    options.MaxDegreeOfParallelism = 1;
                }
                RecipientProvider rp = new RecipientProvider();
                //MatchCollection mcOpen = Regex.Matches(this.HTML, string.Format(@"<(img)[^>]+src\s*=\s*'({0}/Opens[^']*)'[^>]*>", ConfigurationManager.AppSettings["MVCActivity_DomainPath"].ToString()), RegexOptions.Singleline | RegexOptions.Compiled);

                //MatchCollection mc = Regex.Matches(this.HTML, @"<(?:a|link|area)\s[^>]*?href\s*=\s*(['""])([^>]*?&l=[^>]*?)\1", RegexOptions.Singleline | RegexOptions.Compiled);
                //MatchCollection mcText = Regex.Matches(this.TEXT, @"<\s*(http[^>]+)>", RegexOptions.Singleline | RegexOptions.Compiled);

                //MatchCollection mcOpen = Regex.Matches(this.HTML, string.Format(@"<(img)[^>]+src\s*=\s*'({0}/Opens[^']*)'[^>]*>", ConfigurationManager.AppSettings["MVCActivity_DomainPath"].ToString()), RegexOptions.Singleline | RegexOptions.Compiled);

                starttick1 = Environment.TickCount;

                // Process message creation for each email.  //CPU spikes up..
                Parallel.ForEach(listDataRows, options, kvp =>
                {
                    //Personalize EmailMessage for each email & send to SMTP writer to inject into MTA

                    string msg = GetMIMEMessage(kvp.Value, (HasTransnippets ? EmailsTable.Select(" EmailID = " + kvp.Key) : null));
                    rp.Add(kvp.Key, ((DataRow)kvp.Value)["BounceAddress"].ToString().Trim(), ((DataRow)kvp.Value)["EmailAddress"].ToString().Trim(), ((DataRow)kvp.Value)["mailRoute"].ToString().Trim(), msg);
                });

                endtick1 = Environment.TickCount;

                Console.WriteLine(string.Format(" Prep GetMIMEMessage (thread) // {0} records // ProcessTime : {1} // {2} ", listDataRows.Count, (endtick1 - starttick1), DateTime.Now));

                starttick1 = Environment.TickCount;

                ThreadCoordinator coord = new ThreadCoordinator(this.BlastID, this.TestBlast, this.blastconfig.SMTPServer, this.blastconfig.MTAReset);
                coord.RecipientProvider = rp;
                try
                {
                    coord.ThreadsPerConnection = int.Parse(ConfigurationManager.AppSettings["ThreadsPerConnection"].ToString());
                }
                catch
                { }
                try
                {
                    coord.ConnectionsPerServer = int.Parse(ConfigurationManager.AppSettings["ConnectionsPerServer"].ToString());
                }
                catch
                { }

                coord.RunThreads();

                endtick1 = Environment.TickCount;
                Console.WriteLine(string.Format("Completed coord.RunThreads() -  {0} ms // {1}   ", (endtick1 - starttick1), DateTime.Now));
                coord = null;

                rp = null;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
            finally
            {
                if (_PersonalizedContent != null)
                    _PersonalizedContent.Clear();
                listDataRows.Clear();
            }

            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, string.Format("Ending EmailBlast.DoBlast: {0}", BlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

            return go_up_to;
        }
        #endregion
        private string GetMIMEMessage(DataRow dr, DataRow[] emailProfileDataSet)
        {
            var mimeMessageBuilder = new MessageBuilder(this, dr, _PersonalizedContent, emailProfileDataSet);
            return mimeMessageBuilder.GetMIMEMessage();
        }
    
        #region ENDBLAST()

        /// Aysnc call at the end of the blast. It updates the Blasts table and adds this thread's work to the general work queue.

        /// <param name="sendTotal"> the send total from this blast.</param>
        /// <param name="attemptTotal"> the attempt total from this blast excluding the dups.</param>
        private void EndBlast(int sendTotal, int attemptTotal)
        {
            int real_bytes = (this.HTML.Length + this.TEXT.Length) * sendTotal;

            string my_cust_id = DataFunctions.ExecuteScalar("select CustomerID from Blast where BlastID =" + BlastID.ToString()).ToString();

            if (!email_resend)
            {

                // Finish time probably shouldn't keep moving, but no reason to link it to status right now.
                DataFunctions.Execute(" UPDATE Blast SET " +
                    //status_update + finish_time +
                    " FinishTime= getDate(), " +
                    //Had to use attemptTotal instead of sendtotal 'cos of the discrepency in the license#'s sendTotal variable has only the # of emails that was sent in that 10K block dump. 
                    //We need the whole count in the successtotal, attempttotal variable has the right #'s. Using that from now on - ashok 06/12/2008
                    //" SuccessTotal = ISNULL(SuccessTotal,0) + " + sendTotal.ToString() +", "+
                    " SuccessTotal = ISNULL(SuccessTotal,0) + " + attemptTotal.ToString() + ", " +
                    " AttemptTotal = ISNULL(AttemptTotal,0) + " + attemptTotal.ToString() + ", " +
                    " SendBytes = ISNULL(SendBytes,0) + " + real_bytes.ToString() +
                    " WHERE BlastID=" + BlastID.ToString());
                if (!test_blast)
                    //Had to use attemptTotal instead of sendtotal 'cos of the discrepency in the license#'s sendTotal variable has only the # of emails that was sent in that 10K block dump. 
                    //We need the whole count in the successtotal, attempttotal variable has the right #'s. Using that from now on - ashok 06/12/2008
                    //licenseCheck.UpdateUsed(Convert.ToInt32(my_cust_id),"emailblock10k",SendTotal);
                    licenseCheck.UpdateUsed(Convert.ToInt32(my_cust_id), "emailblock10k", attemptTotal);
            }
        }
        #endregion

        private int ProcessEmailsTable()
        {
            var totalEmailCount = 0;

            if (EmailsTable.Rows.Count <= 0)
            {
                return totalEmailCount;
            }

            var emailBlast = this;
            var rowHolder = CreateRowHolder();
            totalEmailCount = rowHolder.Length;
            SetNewTransnippet();
            ProcessTransnippet();
            SetBreakupTexts( msg);
            SetHasTransnippets();
            WriteCounts();
            ReplaceSocialMedia();
            WriteSocialSharedCount();
            ProcessBlast(rowHolder);

            return totalEmailCount;
        }

        private void SetCounts(int totalEmailCount)
        {
            SendTotal = count;
            LastUpdate = update_count;
            EndBlast(count, totalEmailCount);
        }

        private void ProcessBlast( Array rowHolder)
        {
            WriteStartingLoop();
            var totalEmailCount = rowHolder.Length;
            var currentEmailPos = 0;
            while (currentEmailPos < totalEmailCount)
            {
                var start1 = Environment.TickCount;
                currentEmailPos = DoBlast(rowHolder, currentEmailPos);
                var end1 = Environment.TickCount;

                WriteBlastSucceedLog(currentEmailPos, end1, start1);
            }

            ClearEmailsTable();
        }

        private void ReplaceSocialMedia()
        {

            SocialShareUsed = new Dictionary<int, string>();

            var socialshareList =
                ECN_Framework_BusinessLayer.Communicator.SocialMedia.GetSocialMediaCanShare();

            if (socialshareList == null || socialshareList.Count <= 0)
            {
                return;
            }

            foreach (var socialShare in socialshareList)
            {
                var regex = new Regex(socialShare.MatchString, RegexOptions.IgnoreCase);

                var htmlCount = regex.Matches(HTML).Count;
                var textCount = regex.Matches(TEXT).Count;

                if (htmlCount > 0 || textCount > 0)
                {
                    SocialShareUsed.Add(socialShare.SocialMediaID, socialShare.MatchString);
                }
            }
        }

        private static void WriteEndStatus()
        {
            Console.WriteLine($"{DoblastEndLog}{DateTime.Now}");
            Console.WriteLine($"{Memory}{GC.GetTotalMemory(ForceFullCollection)}");
            GC.Collect();
            Console.WriteLine(MemoryGcComplete);
            Console.WriteLine($"{Memory}{GC.GetTotalMemory(ForceFullCollection)}");
        }

        private void WriteCounts()
        {
            Console.WriteLine($"{BreakupHtmlMailCount}{BreakupHTMLMail.Length} ");
            Console.WriteLine($"{BreakupTextMailCount}{BreakupTextMail.Length} ");
            Console.WriteLine($"{BreakupSubjectCount}{BreakupSubject.Length} ");
            Console.WriteLine($"{BreakupFromNameCount}{BreakupFromName.Length} ");
            Console.WriteLine($"{BreakupFromEmailCount}{BreakupFromEmail.Length} ");
            Console.WriteLine($"{BreakupReplyToEmailCount}{breakupReplyToEmail.Length} ");
        }

        private void WriteSocialSharedCount()
        {
            Console.WriteLine($"{SociaShareUsedCountText}{SocialShareUsed.Count} ");
        }

        private void SetHasTransnippets()
        {
            var hasSnippets = TransnippetsCount > 0
                   || ContentTransnippet.CheckForTransnippet(HTML) > 0
                   || ContentTransnippet.CheckForTransnippet(TEXT) > 0
                   || Content.CheckForDynamicTags(HTML);

            HasTransnippets = hasSnippets;
        }

        private void SetBreakupTexts(EmailMessage emailMessage)
        {
            var regex = new Regex(DoublePercentage); // Split on percents.

            BreakupHTMLMail = regex.Split(HTML);
            BreakupTextMail = regex.Split(TEXT);
            BreakupSubject = regex.Split(emailMessage.Subject);
            BreakupFromName = regex.Split(dynamicFromName);
            BreakupFromEmail = regex.Split(dynamicFromEmail);
            breakupReplyToEmail = regex.Split(dynamicReplyToEmail);
        }

        private void ClearEmailsTable()
        {
            if (EmailsTable != null)
            {
                EmailsTable.Dispose();
                EmailsTable = null;
            }
        }

        private static void WriteStartingLoop()
        {
            Console.WriteLine($"{StartingLoopText}{DateTime.Now}");
        }

        private static void WriteBlastSucceedLog(int currentEmailPos, int end1, int start1)
        {
            Console.WriteLine(
                $"Blast from {currentEmailPos - 1000} - " +
                $"{currentEmailPos} : completed in {(end1 - start1)}" +
                $" ms ({((end1 - start1) / 1000)} sec) / Finish Time {DateTime.Now} ");

            Console.WriteLine(EndLine);
        }

        private Array CreateRowHolder()
        {
            var rowHolder = Array.CreateInstance(
                EmailsTable.Rows[0].GetType(),
                EmailsTable.Rows.Count);

            EmailsTable.Rows.CopyTo(rowHolder, 0);

            return rowHolder;
        }

        private void ProcessTransnippet()
        {
            if (HTML.IndexOf(HtmlTransnippetMark) > 0)
            {
                ProcessHtmlMarkExist();
            }
            else
            {
                ProcessNoHtmlMark();
            }
        }

        private void SetNewTransnippet()
        {
            var newTranRegex = new Regex(TransnippetPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var newTransnippetMatchs = newTranRegex.Matches(HTML);

            if (newTransnippetMatchs.Count <= 0)
            {
                return;
            }

            foreach (Match match in newTransnippetMatchs)
            {
                var outputHtml = GetNewTransnippetOutputHtml(match);
                HTML = HTML.Replace(match.Value, outputHtml);

                var txtToReplace = match.Value.Replace(LessThan, Or).Replace(GreaterThen, Or).Replace(NewLineEscaped, string.Empty);
                TEXT = TEXT.Replace(txtToReplace, outputHtml);
            }
        }

        private void LogBegin()
        {
            LogStatistics(StartingEmailBlast);
            Console.WriteLine($"{EmailBlastDoStartedAt}{DateTime.Now}");
        }

        private void LogEnd()
        {
            LogStatistics(EndingEmailBlast);
        }

        private void LogStatistics(string message)
        {
            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
            {
                KM.Common.FileFunctions.LogActivity(
                    OutputToConsole,
                    $"{message}{BlastID}",
                    $"{Statistics}{GetFileName()}{UnderScore}{GetDate()}");
            }
        }

        private void ProcessNoHtmlMark()
        {
            var checkForTransnippet = ContentTransnippet.CheckForTransnippet(HTML);
            var checkForDynamicTags = Content.CheckForDynamicTags(HTML);

            if (checkForTransnippet > 0 || checkForDynamicTags)
            {
                return;
            }

            EmailsTable.Dispose();
            EmailsTable = null;
            TransnippetsCount = 0;
            TransnippetHolder.TransnippetsCount = 0;
        }

        private string GetDate()
        {
            return DateTime.Now.ToShortDateString().ToString().Replace(Slash, Dash);
        }

        private string GetFileName()
        {
            var mainModuleFileName = Process.GetCurrentProcess().MainModule.FileName;
            return Path.GetFileNameWithoutExtension(mainModuleFileName);
        }

        private string GetNewTransnippetOutputHtml(Match m)
        {
            var outputHtmlBuilder = new StringBuilder();

            outputHtmlBuilder.Append("<transnippet ");
            outputHtmlBuilder.Append(m.Groups[2].Value);
            outputHtmlBuilder.Append($"<table><tbody>{m.Groups[3].Value}");
            outputHtmlBuilder.Append($"<transnippet_detail>");
            outputHtmlBuilder.Append(m.Groups[4].Value);
            outputHtmlBuilder.Append("</transnippet_detail></tbody></table>");
            outputHtmlBuilder.Append("</transnippet>");

            return outputHtmlBuilder.ToString();
        }

        private void ProcessHtmlMarkExist()
        {
            var transnippetRegEx = new Regex(RegexPatternHtmlMark);
            var transnippetMatchs = transnippetRegEx.Matches(HTML);

            if (transnippetMatchs.Count > 0)
            {
                Transnippet = new List<string>();

                for (var index = 0; index < transnippetMatchs.Count; index++)
                {
                    Transnippet.Add(transnippetMatchs[index].Value);
                }

                TransnippetTables = new List<string>();
                TransnippetTablesTxt = new List<string>();

                foreach (var transnippet in Transnippet)
                {
                    ProcessTransnippet(transnippet);
                }
            }

            SetTransnippetsValues();
        }

        private void ProcessTransnippet(string transnippet)
        {
            var outputHtml = new StringBuilder();
            var outputTxt = new StringBuilder();

            var transnippetSplits = transnippet
                .Replace(DoubleHashSign, string.Empty)
                .Replace(DoubleDollarSign, string.Empty)
                .Split(SingleOr);

            if (transnippetSplits.Length <= 0)
            {
                return;
            }

            var transnippetUdfList = transnippetSplits[2].Split(CommaSeparator);

            outputHtml.Append(
                $"<table cellpadding=1 cellspacing=1 style=" +
                $"{DoubleQuote}{transnippetSplits[4].Replace("TBL-STYLE=", string.Empty)}{DoubleQuote}>");

            outputTxt.Append(NewLine);

            if (transnippetUdfList.Length > 0)
            {
                outputHtml.Append($"<tr style={DoubleQuote}" +
                                  $"{transnippetSplits[3].Replace("HDR-STYLE=", string.Empty)}" +
                                  $"{DoubleQuote}>");

                outputTxt.Append(NewLine);


                foreach (var transnippetUdf in transnippetUdfList)
                {
                    outputHtml.Append($"<td><b>{transnippetUdf}</b></td>");
                    outputTxt.Append($"{transnippetUdf}{Tab}");
                }

                outputHtml.Append("</tr>");

                outputHtml.Append(transnippet);
                outputHtml.Append("</table>");
                outputTxt.Append(transnippet);
            }

            try
            {
                TransnippetTables.Add(outputHtml.ToString());
                TransnippetTablesTxt.Add(outputTxt.ToString());
            }
            catch (Exception ex)
            {
                Trace.TraceError(TextTransnippetFailed, ex);
            }
        }

        private void SetTransnippetsValues()
        {
            TransnippetsCount = Transnippet.Count;

            TransnippetHolder.Transnippet = Transnippet;
            TransnippetHolder.TransnippetsCount = TransnippetsCount;
            TransnippetHolder.TransnippetTablesHTML = TransnippetTables;
            TransnippetHolder.TransnippetTablesTxt = TransnippetTablesTxt;
        }
    }

    class MessageObject
    {
        string KMDOC = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        string KMHTMLTAG = "<HTML>";
       //  string _KMHEAD = "<HEAD><TITLE>%%EmailSubject%%</TITLE><META http-equiv=\"Content-Type content=text/html; charset=ISO-8859-1\"></HEAD>";
       // string _KMBODY = "<BODY style=\"PADDING-RIGHT:0px;PADDING-LEFT:0px;PADDING-TOP:0px;PADDING-BOTTOM:0px;MARGIN-TOP:0px;MARGIN-LEFT:0px;MARGIN-RIGHT: 0px;\">%%BODY%%</BODY>";

        public MessageObject()
        {
            KMHEAD = "";
            KMBODY = "";
            
            EmailBODY = "";
            EmailDOC = "";
            EmailHEAD = "";
            EmailHTMLTAG = "";

            rgBodyEnd = new Regex("</body.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            rgBodyStart = new Regex("<body.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            rgHeadStart = new Regex("<head.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            rgHeadEnd = new Regex("</head.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            rgHTMLEnd = new Regex("</html.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            rgHTMLStart = new Regex("<html.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public string BuildHTML(bool useKMDOC, bool useKMHTML, bool useKMHEAD, bool useKMBODY)
        {
            StringBuilder sbHTML = new StringBuilder();
            if (useKMDOC)
                sbHTML.Append(KMDOC);
            else
                sbHTML.Append(EmailDOC);

            if (useKMHTML)
                sbHTML.Append(KMHTMLTAG);
            else
                sbHTML.Append(EmailHTMLTAG);

            if (useKMHEAD)
                sbHTML.Append(KMHEAD);
            else
                sbHTML.Append(EmailHEAD);

            if (useKMBODY)
                sbHTML.Append(KMBODY);
            else
                sbHTML.Append(EmailBODY);

            sbHTML.Append("</HTML>");
            return sbHTML.ToString();
        }

        public Regex rgBodyEnd{get;set;}
        public Regex rgBodyStart { get; set; }
        public Regex rgHeadStart { get; set; }
        public Regex rgHeadEnd { get; set; }
        public Regex rgHTMLStart { get; set; }
        public Regex rgHTMLEnd { get; set; }

        public string EmailDOC { get; set; }

        

        public string EmailHTMLTAG { get; set; }

        public string KMHEAD
        {
            get;
            set;

        }

        public string EmailHEAD { get; set; }
        public string KMBODY
        {
            get;
            set;
        }

        public string EmailBODY { get; set; }

        public string EmailTemplateStart { get; set; }
        public string EMailTemplateEnd { get; set; }

        

    }
}
