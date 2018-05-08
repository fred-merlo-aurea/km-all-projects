using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Text;
using System.Collections.Generic;

namespace ecn.activityengines
{


    public partial class subscribe : System.Web.UI.Page
    {

        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
        public KMPlatform.Entity.User User = null;
        int EmailID = 0;
        int CustomerID = 0;
        int BlastID = 0;
        string EmailAddress = "";
        string GroupID = "";
        string Subscribe = "";
        string Format = "";

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
            {
                User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                User = (KMPlatform.Entity.User)Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
            }

            EmailID = getEmailID();
            CustomerID = getCustomerID();
            BlastID = getBlastID();
            EmailAddress = getEmailAddress();
            GroupID = getGroupID();
            Subscribe = getSubscribe();
            Format = getFormat();

            ECN_Framework_Entities.Communicator.SmartFormTracking sft = new ECN_Framework_Entities.Communicator.SmartFormTracking();
            if (CustomerID != 0)
                sft.CustomerID = CustomerID;
            try
            {
                if(Convert.ToInt32(GroupID) > 0)
                    sft.GroupID = Convert.ToInt32(GroupID);
            }
            catch (Exception) {}
            if (BlastID != 0)
                sft.BlastID = BlastID;
            sft.ReferringUrl = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();
            sft.ActivityDate = DateTime.Now;
            ECN_Framework_BusinessLayer.Communicator.SmartFormTracking.Insert(sft);

            int rows = 0;

            try
            {
              
                if (GroupID.Trim().Length > 0)
                {
                    if (EmailID > 0)
                    {
                        rows = ClickVerified(EmailID, BlastID, GroupID, Subscribe, Format);

                        if (rows > 0)
                        {
                            if ("U" == Subscribe)
                            {
                                string unSubscribe_Response_pg = TemplateFunctions.SelectTemplate(CustomerID.ToString(), "UnSubscribe-DblOptinVerConfirmPage/URL", "");

                                if (unSubscribe_Response_pg.ToLower().StartsWith("http://"))
                                {
                                    Response.Redirect(unSubscribe_Response_pg);
                                }
                                else
                                {
                                    string unSubscribe_Response_pg_Hdr = TemplateFunctions.SelectTemplate(CustomerID.ToString(), "UnSubscribe-DblOptinVerPageHdr", "");
                                    string unSubscribe_Response_pg_Ftr = TemplateFunctions.SelectTemplate(CustomerID.ToString(), "UnSubscribe-DblOptinVerPageFtr", "");
                                    Response.Write(unSubscribe_Response_pg_Hdr + unSubscribe_Response_pg + unSubscribe_Response_pg_Ftr);
                                }
                            }
                            else
                            {
                                string subscribe_Response_page = TemplateFunctions.SelectTemplate(CustomerID.ToString(), "Subscribe-DblOptinResponsePage/URL", "");
                                if (subscribe_Response_page.ToLower().StartsWith("http://"))
                                {
                                    Response.Redirect(subscribe_Response_page);
                                }
                                else
                                {
                                    Response.Write(subscribe_Response_page);
                                }
                            }
                            Response.End();
                        }
                        else
                        {
                            Response.Write("ERROR! Record Not Found. Verify the link you entered. ");
                            Response.End();
                        }
                    }
                    else
                    {
                        SendEmail(EmailAddress, BlastID, GroupID, CustomerID, Subscribe, Format);

                    }
                    string body = TemplateFunctions.SelectTemplate(CustomerID.ToString(), "Subscribe-DblOptinVerPage/URL", "");
                    if (getReturnURL() != "")
                    {
                        Response.Write("<script>document.location.href='" + getReturnURL() + "';</script>");
                    }
                    else
                    {
                        //Response.Write(body);
                        if (body.ToLower().StartsWith("http://"))
                        {
                            Response.Redirect(body);
                        }
                        else
                        {
                            Response.Write(body);
                        }
                    }

                }
                else
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError("Unknown GroupID: " + GroupID, "Subscribe.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                }

              
            }
            catch (System.Threading.ThreadAbortException)
            {
                //ignore
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Subscribe.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                //Helper.LogCriticalError(ex, "Subscribe.Page_Load");
                //NotifyAdmin(ex, getEmailID(), getGroupID(), getBlastID(), getEmailAddress());
                Response.Write("Error in Subscribe.  Customer Service has been notified.");
            }
            //Response.Redirect(Request.UrlReferrer.ToString());            
        }


        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<br><b>Blast ID:</b>&nbsp;" + getBlastID());
                adminEmailVariables.AppendLine("<br><b>Group ID:</b>&nbsp;" + getGroupID());
                adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + getEmailID());
                adminEmailVariables.AppendLine("<br><b>Email Address:</b>&nbsp;" + getEmailAddress());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }


        //private void NotifyAdmin(Exception ex, int emailID, string groupID, int blastID, string emailAddress)
        //{
        //    StringBuilder adminEmailVariables = new StringBuilder();
        //    string admimEmailBody = string.Empty;

        //    adminEmailVariables.AppendLine("<br><b>Blast ID:</b>&nbsp;" + blastID);
        //    adminEmailVariables.AppendLine("<br><b>Group ID:</b>&nbsp;" + groupID);
        //    adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + emailID);
        //    adminEmailVariables.AppendLine("<br><b>Email Address:</b>&nbsp;" + emailAddress);

        //    admimEmailBody = ActivityError.CreateMessage(ex, Request, adminEmailVariables.ToString());

        //    Helper.SendMessage("Error in Activity Engine: Subscribe", admimEmailBody);
        //}



        #region Get Request Variables methods [ getEmailID, getCustomerID, getBlastID, getEmailAddress, getReturnURL, getFullName, getGroupID, getSubscribe, getFormat, getFirstName getLastName, getFullName, getStreetAddress, getCompanyName, getCity, getState, getZipCode, getPhone, getBirthdate]

        private int getEmailID()
        {
            int theEmailID = 0;
            try
            {
                theEmailID = Convert.ToInt32(Request.QueryString["ei"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theEmailID;
        }

        private int getCustomerID()
        {
            int theCustomerID = 0;
            try
            {
                theCustomerID = Convert.ToInt32(Request.QueryString["c"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCustomerID;
        }

        private int getBlastID()
        {
            int theBlastID = 0;
            try
            {
                theBlastID = Convert.ToInt32(Request.QueryString["b"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theBlastID;
        }

        private string getEmailAddress()
        {
            string theEmailAddress = "";
            try
            {
                theEmailAddress = Request.QueryString["e"].ToString().Trim();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theEmailAddress.Trim();
        }

        private string getReturnURL()
        {
            string theReturnURL = "";
            try
            {
                theReturnURL = Request.QueryString["url"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theReturnURL;
        }

        private string getGroupID()
        {
            string theGroupID = "";
            try
            {
                theGroupID = Request.QueryString["g"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theGroupID;
        }

        private string getSubscribe()
        {
            string theSubscribe = "";
            try
            {
                theSubscribe = Request.QueryString["s"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theSubscribe;
        }
        private string getFormat()
        {
            string theFormat = "";
            try
            {
                theFormat = Request.QueryString["f"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theFormat;
        }
        private string getTitle()
        {
            string theTitle = "";
            try
            {
                theTitle = Request.QueryString["t"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theTitle;
        }
        private string getFirstName()
        {
            string theFirstName = "";
            try
            {
                theFirstName = Request.QueryString["fn"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theFirstName;
        }
        private string getLastName()
        {
            string theLastName = "";
            try
            {
                theLastName = Request.QueryString["ln"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theLastName;
        }
        private string getFullName()
        {
            string theFullName = "";
            try
            {
                theFullName = Request.QueryString["n"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theFullName;
        }
        private string getStreetAddress()
        {
            string theStreetAddress = "";
            try
            {
                theStreetAddress = Request.QueryString["adr"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theStreetAddress;
        }
        private string getStreetAddress2()
        {
            string theStreetAddress2 = "";
            try
            {
                theStreetAddress2 = Request.QueryString["adr2"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theStreetAddress2;
        }
        private string getCompanyName()
        {
            string theCompanyName = "";
            try
            {
                theCompanyName = Request.QueryString["compname"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCompanyName;
        }
        private string getCity()
        {
            string theCity = "";
            try
            {
                theCity = Request.QueryString["city"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCity;
        }
        private string getState()
        {
            string theState = "";
            try
            {
                theState = Request.QueryString["state"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theState;
        }
        private string getCountry()
        {
            string theCountry = "";
            try
            {
                theCountry = Request.QueryString["ctry"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCountry;
        }
        private string getZipCode()
        {
            string theZipCode = "";
            try
            {
                theZipCode = Request.QueryString["zc"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theZipCode;
        }
        private string getPhone()
        {
            string thePhone = "";
            try
            {
                thePhone = Request.QueryString["ph"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return thePhone;
        }
        private string getMobilePhone()
        {
            string theMobilePhone = "";
            try
            {
                theMobilePhone = Request.QueryString["mph"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theMobilePhone;
        }
        private string getFax()
        {
            string theFax = "";
            try
            {
                theFax = Request.QueryString["fax"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theFax;
        }
        private string getWebsite()
        {
            string theWebsite = "";
            try
            {
                theWebsite = Request.QueryString["website"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theWebsite;
        }
        private string getAge()
        {
            string theAge = "";
            try
            {
                theAge = Request.QueryString["age"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theAge;
        }
        private string getIncome()
        {
            string theIncome = "";
            try
            {
                theIncome = Request.QueryString["income"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theIncome;
        }
        private string getGender()
        {
            string theGender = "";
            try
            {
                theGender = Request.QueryString["gndr"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theGender;
        }
        private string getOccupation()
        {
            string theOccupation = "";
            try
            {
                theOccupation = Request.QueryString["occ"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theOccupation;
        }
        private string getBirthdate()
        {
            string theBirthdate = "";
            try
            {
                theBirthdate = (DateTime.Parse(Request.QueryString["bdt"].ToString())).ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }

            if (theBirthdate.Length == 0)
            {
                //theBirthdate = DateTime.Now.Date.ToString();
            }
            return theBirthdate;
        }
        private string getUser1()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr1"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUser2()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr2"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUser3()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr3"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUser4()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr4"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUser5()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr5"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUser6()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usr6"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUserEvent1()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usrevt1"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUserEvent1Date()
        {
            string theUserEvent1Date = "";
            try
            {
                theUserEvent1Date = (DateTime.Parse(Request.QueryString["usrevtdt1"].ToString())).ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }

            if (theUserEvent1Date.Length == 0)
            {
                //theUserEvent1Date = DateTime.Now.Date.ToString();
            }
            return theUserEvent1Date;
        }
        private string getUserEvent2()
        {
            string userstuff = "";
            try
            {
                userstuff = Request.QueryString["usrevt2"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return userstuff;
        }
        private string getUserEvent2Date()
        {
            string theUserEvent2Date = "";
            try
            {
                theUserEvent2Date = (DateTime.Parse(Request.QueryString["usrevtdt2"].ToString())).ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }

            if (theUserEvent2Date.Length == 0)
            {
                //theUserEvent2Date = DateTime.Now.Date.ToString();
            }
            return theUserEvent2Date;
        }

        //Get UDF Values.. 
        private string getUDF(string _value)
        {
            //try
            // {
            return Request.QueryString[_value].ToString();
            // }
            // catch
            // {
            //    return string.Empty;
            // }
        }
        #endregion

        #region Click Verified
        private int ClickVerified(int theEmailID, int theBlastID, string theGroupID, string theSubscribe, string theFormat)
        {
            string group_id;
            if (theBlastID > 0)
            {
                ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(theBlastID,  false);
                group_id = blast.GroupID.ToString();
            }
            else
            {
                group_id = theGroupID;
            }
            ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(theEmailID);

            string xmlProfile = "<Emails><emailaddress>" + email.EmailAddress + "</emailaddress></Emails>";

            DataTable dtResults = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(User, email.CustomerID, Convert.ToInt32(theGroupID), "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", theFormat, theSubscribe, true, "", "ActivityEngine.Subscribe");

            Hashtable hUpdatedRecords = new Hashtable();
            foreach (DataRow dr in dtResults.Rows)
            {
                if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                {
                    hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                }
                else
                {
                    int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                    hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                }
            }
            int UpdatedRecords = Convert.ToInt32(hUpdatedRecords["U"].ToString());
            if (UpdatedRecords > 0)
            {
                ECN_Framework_Entities.Communicator.EmailActivityLog eal = new ECN_Framework_Entities.Communicator.EmailActivityLog();
                eal.BlastID = theBlastID;
                eal.EmailID = theEmailID;
                eal.ActionTypeCode = "subscribe";
                eal.ActionValue = theSubscribe;//need to be more descriptive
                eal.ActionNotes = theSubscribe + " - BlastID: " + theBlastID.ToString() + " - GroupID: " + theGroupID.ToString();
                int id = ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.Insert(eal, User);
                //EmailActivityLog.InsertSubscribe(theEmailID, theBlastID, theSubscribe);


                //Commenting out because the EmailActivityLog.Insert statement above will trigger the EventOrganizer

                //EmailActivityLog log = new EmailActivityLog(id);
                //Groups group = new Groups(theGroupID);
                //log.SetGroup(group);
                //log.SetEmail(new Emails(theEmailID));
                //EventOrganizer eventer = new EventOrganizer();
                //eventer.CustomerID(group.CustomerID());
                //eventer.Event(log);
            }
            return UpdatedRecords;
        }
        #endregion

        #region Send Email
        private void SendEmail(string theEmailAddress, int theBlastID, string theGroupID, int theCustomerID, string theSubscribe, string theFormat)
        {
            ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck(theCustomerID);
            ECN_Framework_Entities.Communicator.Group current_group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(Convert.ToInt32(theGroupID));
            ECN_Framework_Entities.Communicator.EmailGroup old_email = SubscribeToGroup(current_group, theEmailAddress, theBlastID, theGroupID, theCustomerID, theSubscribe, theFormat);

            string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
            string GroupName = current_group.GroupName;
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(theCustomerID, false);

            string CompanyName = customer.CustomerName;
            string CompanyAddress = customer.Address + "<br />" + customer.City + " , " + customer.State + " " + customer.Zip;
            string subscriptionsEmail = customer.SubscriptionsEmail;
            string bounceDomain = cc.getBounceDomain();

            string body = CreateEmailBody(theCustomerID, theGroupID, old_email, CompanyName, CompanyAddress, GroupName, bounceDomain, theBlastID, theSubscribe, theFormat);

            if (subscriptionsEmail.Length < 0)
            {
                subscriptionsEmail = "subscriptions@" + bounceDomain;
            }

            //Add Unsubscribe Link at the bottom of the email as per CAN-SPAM & USI requested it to be done. 
            string unsubscribeText = "<p style=\"padding-TOP:5px\"><div style=\"font-size:8.0pt;font-family:'Arial,sans-serif'; color:#666666\"><IMG style=\"POSITION:relative; TOP:5px\" src='" + ConfigurationManager.AppSettings["Image_DomainPath"] + "/images/Sure-Unsubscribe.gif'/>&nbsp;If you feel you have received this message in error, or wish to be removed, please <a href='" + ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?e=" + EmailAddress + "&g=" + GroupID + "&b=0&c=" + CustomerID + "&s=U'>Unsubscribe</a>.</div></p>";
            body += unsubscribeText;


            EmailFunctions emailFunctions = new EmailFunctions();

            ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
            ed.CustomerID = theCustomerID;
            ed.EmailAddress = theEmailAddress;
            ed.EmailSubject = GroupName + " Subscription Confirmation";

            ed.FromName = "Activity Engine";
            ed.Process = "Activity Engine - subscribe.SendEmail";
            ed.Source = "Activity Engine";
            ed.ReplyEmailAddress = subscriptionsEmail;
            ed.CreatedUserID = User.UserID;
            ed.Content = body;

            try
            {
                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "Subscribe.PageLoad.SendEmail", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateNote());
            }

            //Send CC Notification to Admin
            string ccNotifyBody = CreateAdminEmailBody(theCustomerID, GroupName);

            ed.Content = ccNotifyBody;
            ed.EmailAddress = customer.Email;
            ed.EmailSubject = "New Subscription to : " + GroupName;

            if (ccNotifyBody.Length > 0)
            {
                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
        }

        private Hashtable GetGroupDataFields(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);
            Hashtable fields = new Hashtable();
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in groupDataFieldsList)
                fields.Add("user_" + groupDataFields.ShortName.ToLower(), groupDataFields.GroupDataFieldsID);
            return fields;
        }
        private string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }

        private string getProfileString(string theEmailAddress)
        {
            StringBuilder sbProfile = new StringBuilder();
            sbProfile.Append("<Emails><emailaddress>" + theEmailAddress + "</emailaddress>");
            sbProfile.Append("<title>" + getTitle() + "</title>");
            sbProfile.Append("<firstname>" + getFirstName() + "</firstname>");
            sbProfile.Append("<lastname>" + getLastName() + "</lastname>");
            sbProfile.Append("<fullname>" + getFullName() + "</fullname>");
            sbProfile.Append("<company>" + getCompanyName() + "</company>");
            sbProfile.Append("<occupation>" + getOccupation() + "</occupation>");
            sbProfile.Append("<address>" + getStreetAddress() + "</address>");
            sbProfile.Append("<address2>" + getStreetAddress2() + "</address2>");
            sbProfile.Append("<city>" + getCity() + "</city>");
            sbProfile.Append("<state>" + getState() + "</state>");
            sbProfile.Append("<zip>" + getZipCode() + "</zip>");
            sbProfile.Append("<country>" + getCountry() + "</country>");
            sbProfile.Append("<voice>" + getPhone() + "</voice>");
            sbProfile.Append("<mobile>" + getMobilePhone() + "</mobile>");
            sbProfile.Append("<fax>" + getFax() + "</fax>");
            sbProfile.Append("<website>" + getWebsite() + "</website>");
            sbProfile.Append("<age>" + getAge() + "</age>");
            sbProfile.Append("<income>" + getIncome() + "</income>");
            sbProfile.Append("<gender>" + getGender() + "</gender>");
            sbProfile.Append("<user1>" + getUser1() + "</user1>");
            sbProfile.Append("<user2>" + getUser2() + "</user2>");
            sbProfile.Append("<user3>" + getUser3() + "</user3>");
            sbProfile.Append("<user4>" + getUser4() + "</user4>");
            sbProfile.Append("<user5>" + getUser5() + "</user5>");
            sbProfile.Append("<user6>" + getUser6() + "</user6>");
            try
            {
                sbProfile.Append("<birthdate>" + DateTime.Parse(getBirthdate()) + "</birthdate>");
            }
            catch { }

            try
            {
                sbProfile.Append("<userevent1date>" + DateTime.Parse(getUserEvent1Date()) + "</userevent1date>");
            }
            catch { }

            try
            {
                sbProfile.Append("<userevent2date>" + DateTime.Parse(getUserEvent2Date()) + "</userevent2date>");
            }
            catch { }

            sbProfile.Append("<userevent1>" + getUserEvent1() + "</userevent1>");
            sbProfile.Append("<userevent2>" + getUserEvent2() + "</userevent2>");
            sbProfile.Append("<notes>" + "Subscription through Website. DateAdded: " + DateTime.Now.ToString() + "</notes>");

            sbProfile.Append("</Emails>");
            return sbProfile.ToString();

        }
        private string getUDFString(Hashtable hGDFFields, string theEmailAddress)
        {
            StringBuilder xmlUDF = new StringBuilder();

            if (hGDFFields.Count > 0)
            {
                bool bRowCreated = false;
                foreach (DictionaryEntry kvp in hGDFFields)
                {
                    string UDFData = "";
                    if (!bRowCreated)
                    {
                        xmlUDF.Append("<row>");
                        xmlUDF.Append("<ea>" + CleanXMLString(theEmailAddress) + "</ea>");
                        bRowCreated = true;
                    }
                    try
                    {
                        UDFData = getUDF(kvp.Key.ToString());
                        if (UDFData.Trim().Length > 0)
                        {



                            xmlUDF.Append("<udf id=\"" + kvp.Value.ToString() + "\">");

                            xmlUDF.Append("<v><![CDATA[" + CleanXMLString(UDFData) + "]]></v>");

                            xmlUDF.Append("</udf>");


                        }
                    }
                    catch { }
                }
                xmlUDF.Append("</row>");
            }
            return xmlUDF.ToString();
        }
        private ECN_Framework_Entities.Communicator.EmailGroup SubscribeToGroup(ECN_Framework_Entities.Communicator.Group group, string theEmailAddress, int theBlastID, string theGroupID, int theCustomerID, string theSubscribe, string theFormat)
        {
            //Commented - this method was allowing duplication of email profiles in the same customer 
            //which is not the design of ECN5. Using the logic from Import Objects - ashok 1/16/2005
            //Emails old_email =  group.WhatEmail(theEmailAddress);
            ECN_Framework_Entities.Communicator.EmailGroup old_email = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(theEmailAddress, group.GroupID);
            Hashtable UDFHash = GetGroupDataFields(group.GroupID);

            string xmlProfile = getProfileString(theEmailAddress);
            string xmlUDFs = getUDFString(UDFHash, theEmailAddress);
            if (null == old_email)
            {
                #region View commented old code
                /* NOT USING this 'cos inserts fail when there's a single quote (') in the text.. 
					string InsertQuery=
					" INSERT INTO Emails ("+
					" EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, "+
					" Address, Address2, City, State, Zip, Country, "+
					" Voice, Mobile, Fax, Website, Age, Income, Gender, "+
					" Occupation, User1 , User2, User3, User4, User5, User6, "+
					" Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, "+
					" DateAdded, DateUpdated "+
					") VALUES ("+
					"'"+theEmailAddress+"', " +theCustomerID+", '"+getTitle()+"', '"+getFirstName()+"', '"+getLastName()+"', '"+getFullName()+"', '"+getCompanyName()+"', "+
					"'"+getStreetAddress()+"', '"+getStreetAddress2()+"', '"+getCity()+"', '"+getState()+"', '"+getZipCode()+"', '"+getCountry()+"', "+
					"'"+getPhone() +"', '"+getMobilePhone()+"', '"+getFax()+"', '"+getWebsite()+"', '"+getAge()+"', '"+getIncome()+"', '"+getGender()+"', "+
					"'"+getOccupation()+"', '"+getUser1()+"', '"+getUser2()+"', '"+getUser3()+"', '"+getUser4()+"', '"+getUser5()+"', '"+getUser6()+"', "+
					"'"+getBirthdate()+"', '"+getUserEvent1()+"', '"+getUserEvent1Date()+"', '"+getUserEvent2()+"', '"+getUserEvent2Date()+"', "+
					"'"+DateTime.Now.ToString()+"' , '"+ DateTime.Now.ToString()+"'"+
					" ); SELECT @@IDENTITY; ";
				EmailID = Convert.ToInt32(DataFunctions.ExecuteScalar(InsertQuery));
				old_email= new Emails(EmailID);
				*/
                #endregion


                if (IsSingleOptin(theCustomerID))
                {
                    ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(User, Convert.ToInt32(theCustomerID), group.GroupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDFs + "</XML>", theFormat, theSubscribe, false, "", "ActivityEngine.Subscribe");
                    ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(theEmailAddress, group.CustomerID);
                    // theBlastID always 0, no customer is assoicate with it, so it won't trigger any event.
                    if (email != null)
                    {
                        ECN_Framework_Entities.Communicator.EmailActivityLog eal = new ECN_Framework_Entities.Communicator.EmailActivityLog();
                        eal.BlastID = theBlastID;
                        eal.EmailID = email.EmailID;
                        eal.ActionTypeCode = "subscribe";
                        eal.ActionValue = "S";
                        eal.ActionNotes = "S - " + HttpContext.Current.Request.UrlReferrer.ToString();
                        ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.Insert(eal, User);
                    }
                    //int id = EmailActivityLog.InsertSubscribe(EmailID, theBlastID, "S");

                    //Commenting out below, it's handled in the EmailActivityLog.Insert() above

                    // If it's single optin, it triggers the event from here.
                    //EmailActivityLog log = new EmailActivityLog(id);
                    //log.SetGroup(new Groups(theGroupID));
                    //log.SetEmail(new Emails(EmailID));

                    //EventOrganizer eventer = new EventOrganizer();
                    //eventer.CustomerID(theCustomerID);
                    //eventer.Event(log);
                }
                else
                {
                    ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(User, Convert.ToInt32(theCustomerID), group.GroupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDFs + "</XML>", theFormat, "P", false, "", "ActivityEngine.Subscribe");
                }

            }
            else
            {
                //Code added by Ashok 4-10-06. Ziegler & RMC Proj Mgmt were complaining that the subscription confirmations wren't working properly. 
                //Looked at the Code & found that this part is missing.. !
                ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(User, Convert.ToInt32(theCustomerID), group.GroupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDFs + "</XML>", theFormat, "P", false, "", "ActivityEngine.Subscribe");
            }

            old_email = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(theEmailAddress, group.GroupID);

            return old_email;
        }

        private string CreateEmailBody(int theCustomerID, string theGroupID, ECN_Framework_Entities.Communicator.EmailGroup email, string companyName, string companyAddress, string groupName, string bounceDomain, int theBlastID, string theSubscribe, string theFormat)
        {
            ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck(theCustomerID);
            if (IsSingleOptin(theCustomerID))
            {
                ECN_Framework_Entities.Accounts.CustomerTemplate ctSingleOptInBody = null;
                string singleOptinBody;

                try
                {
                    ctSingleOptInBody = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByTypeID_NoAccessCheck(Convert.ToInt32(theCustomerID), "Subscribe-DblOptinResponsePage/URL");
                    if (ctSingleOptInBody == null)
                    {
                        ctSingleOptInBody = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByTypeID_NoAccessCheck(1, "Subscribe-DblOptinResponsePage/URL");
                    }
                    singleOptinBody = ctSingleOptInBody.HeaderSource;
                }
                catch (Exception)
                {
                    ctSingleOptInBody = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByTypeID_NoAccessCheck(1, "Subscribe-DblOptinResponsePage/URL");
                    singleOptinBody = ctSingleOptInBody.HeaderSource;
                }

                return singleOptinBody;
            }

            ECN_Framework_Entities.Accounts.CustomerTemplate ctDblOptin = null;

            string body;
            try
            {
                ctDblOptin = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByTypeID_NoAccessCheck(theCustomerID, "Subscribe-DblOptinVerEmail");
                body = ctDblOptin.HeaderSource;

            }
            catch (Exception)
            {
                ctDblOptin = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByTypeID_NoAccessCheck(1, "Subscribe-DblOptinVerEmail");
                body = ctDblOptin.HeaderSource;
            }

            string virtualPath = System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"];

            string server = cc.getHostName();
            string redirpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/subscribe.aspx";
            string thelink = redirpage + "?ei=" + email.EmailID +
                "&b=" + theBlastID +
                "&s=" + theSubscribe +
                "&f=" + theFormat +
                "&c=" + theCustomerID +
                "&g=" + theGroupID;

            /*	string body="This is to confirm your subscription change request for "+GroupName+".<br> "+
                    " If you made this request follow the included link to confirm the change.<br> "+
                    " &nbsp;&nbsp;<a href="+thelink+">"+thelink+"</a>";
            */
            body = body.Replace("%%SubscribeLink%%", thelink);
            body = body.Replace("%%GroupName%%", groupName);
            body = body.Replace("%%CompanyName%%", companyName);
            body = body.Replace("%%CompanyAddress%%", companyAddress);
            return body;
        }

        private string CreateAdminEmailBody(int theCustomerID, string groupName)
        {
            ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
            ECN_Framework_Entities.Accounts.CustomerTemplate ct = null;
            string ccEmailBody = "";
            string ccEmailNotifyFlag = "N";

            try
            {
                ct = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByTypeID_NoAccessCheck(theCustomerID, "Subscribe-DblOptinVerEmail");
                ccEmailNotifyFlag = ct.CCNotifyEmail;

                if (ccEmailNotifyFlag.Equals("Y"))
                {
                    ECN_Framework_Entities.Accounts.CustomerTemplate ctHeader = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByTypeID_NoAccessCheck(theCustomerID, "Subscribe-DblOptinNotifyEmail");

                    ccEmailBody = ctHeader.HeaderSource;

                    ccEmailBody = ccEmailBody.Replace("%%EmailAddress%%", getEmailAddress());
                    ccEmailBody = ccEmailBody.Replace("%%GroupName%%", groupName);
                    ccEmailBody = ccEmailBody.Replace("%%Voice%%", getPhone());
                    ccEmailBody = ccEmailBody.Replace("%%Name%%", getFirstName() + " " + getLastName());
                    ccEmailBody = ccEmailBody.Replace("%%Zip%%", getZipCode());
                }
            }
            catch (Exception)
            {
                ccEmailBody = "";
            }

            return ccEmailBody;
        }
        #endregion

        #region Single Optin Helpers
        private bool IsSingleOptin(int customerID)
        {
            return customerID == 202 || customerID == 5;
        }
        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
        }
        #endregion
    }
}
