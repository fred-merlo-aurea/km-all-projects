using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.activityengines
{
    public partial class ATHB_SO_subscribe : System.Web.UI.Page
    {
        Groups group;

        Hashtable hProfileFields = new Hashtable();
        Hashtable hUDFFields = new Hashtable();

        public KMPlatform.Entity.User User = null;

        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];

        public static string Response_FromEmail = "";
        public static string Response_UserMsgSubject = "";
        public static string Response_UserMsgBody = "";
        public static string Response_UserScreen = "";
        public static string Response_AdminEmail = "";
        public static string Response_AdminMsgSubject = "";
        public static string Response_AdminMsgBody = "";

        int CustomerID = 0;
        int GroupID = 0;
        int SmartFormID = 0;

        string EmailAddress = "";
        string Subscribe = "";
        string Format = "";

        bool isNewSubscriber = false;

        #region Get Request Variables

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
        private int getGroupID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["g"].ToString());
            }
            catch (Exception E)
            {
                return 0;
            }
        }
        private int getSmartFormID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["sfID"].ToString());
            }
            catch
            {
                return 0;
            }
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
            if (theEmailAddress.Equals("") || theEmailAddress.Equals(string.Empty))
                theEmailAddress = Guid.NewGuid().ToString() + "@kmpsgroup.com";
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

        private string getSubscribeTypeCode()
        {
            try
            {
                return Request.QueryString["s"].ToString();
            }
            catch
            {
                return "S";
            }
        }
        private string getFormat()
        {
            try
            {
                return Request.QueryString["f"].ToString();
            }
            catch (Exception E)
            {
                return "html";
            }
        }

        private string getQS(string QSname)
        {
            if (!QSname.Equals("e"))
                return Request.QueryString[QSname].ToString();
            else
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
                if (theEmailAddress.Equals("") || theEmailAddress.Equals(string.Empty))
                    theEmailAddress = Guid.NewGuid().ToString() + "@kmpsgroup.com";
                return theEmailAddress.Trim();
            }
        }
        private string fromURL
        {
            get { try { return Request.UrlReferrer.ToString(); } catch { return ""; } }
        }

        string fromIP
        {
            get { try { return Request.UserHostAddress.ToString(); } catch { return ""; } }
        }
        #endregion

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

            CustomerID = getCustomerID();
            GroupID = getGroupID();
            SmartFormID = getSmartFormID();
            EmailAddress = getEmailAddress();

            //Validate 
            //  1. CustomerID && GroupID should be > than 0
            //  2. Valid Emailaddress
            //  3. SFID belongs to correct Group.

            try
            {
                if (GroupID > 0)
                {
                    group = new Groups(GroupID);
                    int tempCustID = 0;
                    try
                    {
                        tempCustID = group.CustomerID();
                    }
                    catch (Exception)
                    {
                    }

                    if (group != null && tempCustID > 0)
                    {
                        if (EmailAddress.Trim().Length > 0 && group.WhatEmail(EmailAddress) == null)
                            isNewSubscriber = true;

                        Emails email = SubscribeToGroup();

                        //Check for Group Trigger Events 
                        if (email != null)
                        {
                            int eID = email.ID();
                            int id = EmailActivityLog.InsertSubscribe(eID, 0, "S");
                            EmailActivityLog log = new EmailActivityLog(id);

                            log.SetGroup(group);
                            log.SetEmail(new Emails(eID));

                            EventOrganizer eventer = new EventOrganizer();
                            eventer.CustomerID(group.CustomerID());
                            eventer.Event(log);

                            if (SmartFormID > 0)
                            {
                                //get smartForm Details
                                getSmartFormDetails(SmartFormID);

                                if (CustomerID != 2553) // DO NOT SEND NOTIFICATION/THANKYOU EMAIL FOR EXISTING SUBSCRIBERS IN THE GROUP - ADDED 06/11/2010
                                {
                                    //now send the response emails confirmation to the user & admins.
                                    SendUserResponseEmails(group, email);
                                    SendAdminResponseEmails(group, email);
                                }
                                else
                                {
                                    if (isNewSubscriber)
                                    {
                                        SendUserResponseEmails(group, email);
                                    }
                                }

                                //Finally show the user, the Thankyou Page / redirect'em to a page that's setup.
                                if (Response_UserScreen.ToLower().StartsWith("http://"))
                                {
                                    Response_UserScreen = ReplaceCodeSnippets(group, email, Response_UserScreen);
                                    Response.Write("<script>document.location.href='" + Response_UserScreen + "';</script>");
                                }
                                else
                                {
                                    Response_UserScreen = ReplaceCodeSnippets(group, email, Response_UserScreen);
                                    Response.Write(Response_UserScreen);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "OptIn.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                //Helper.LogCriticalError(ex, "OptIn.Page_Load");
                //NotifyAdmin(ex);
                Response.Write("Error in Optin.  Customer Service has been notified.");
            }
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
                adminEmailVariables.AppendLine("<br><b>Smart Form ID:</b>&nbsp;" + getSmartFormID());
                adminEmailVariables.AppendLine("<br><b>Email Address:</b>&nbsp;" + getEmailAddress());
                adminEmailVariables.AppendLine("<br><b>Customer ID:</b>&nbsp;" + getCustomerID());
                adminEmailVariables.AppendLine("<BR>Page URL: " + Request.RawUrl.ToString());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        #region get smartForm Details
        private void getSmartFormDetails(int sfID)
        {
            if (sfID > 0)
            {
                DataTable dt = DataFunctions.GetDataTable(" SELECT * FROM SmartFormsHistory WHERE IsDeleted = 0 and SmartFormID=" + sfID);
                DataRow dr = dt.Rows[0];

                Response_FromEmail = dr["Response_FromEmail"].ToString();
                Response_UserMsgSubject = dr["Response_UserMsgSubject"].ToString();
                Response_UserMsgBody = dr["Response_UserMsgBody"].ToString();
                Response_UserScreen = dr["Response_UserScreen"].ToString();
                Response_AdminEmail = dr["Response_AdminEmail"].ToString();
                Response_AdminMsgSubject = dr["Response_AdminMsgSubject"].ToString();
                Response_AdminMsgBody = dr["Response_AdminMsgBody"].ToString();
            }
        }

        #endregion

        #region Send User & Admin Emails
        private void SendUserResponseEmails(Groups grpObject, Emails theEmailObject)
        {
            if (Response_FromEmail.Length > 5 && Response_UserMsgSubject.Trim().Length > 0 && Response_UserMsgBody.Trim().Length > 0)
            {
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                ed.CustomerID = CustomerID;
                ed.EmailAddress = theEmailObject.EmailAddress();
                ed.EmailSubject = Response_UserMsgSubject;
                ed.FromName = "Activity Engine";
                ed.Process = "Activity Engine - ATHB_SO_subscribe.SendUserResponseEmails";
                ed.Source = "Activity Engine";
                ed.ReplyEmailAddress = Response_FromEmail;
                ed.CreatedUserID = User.UserID;
                ed.Content = ReplaceCodeSnippets(grpObject, theEmailObject, Response_UserMsgBody);

                if (ed.Content.ToLower().IndexOf("%%unsubscribelink%%") > 0)
                {
                    ed.Content = StringFunctions.Replace(ed.Content, "http://%%unsubscribelink%%", "%%unsubscribelink%%");
                    ed.Content = StringFunctions.Replace(ed.Content, "%%unsubscribelink%%", ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?e=" + EmailAddress + "&g=" + GroupID + "&b=0&c=" + CustomerID + "&s=U");
                }
                else
                {
                    string unsubscribeText = "<p style=\"padding-TOP:5px\"><div style=\"font-size:8.0pt;font-family:'Arial,sans-serif'; color:#666666\"><IMG style=\"POSITION:relative; TOP:5px\" src='" + ConfigurationManager.AppSettings["Image_DomainPath"] + "/images/Sure-Unsubscribe.gif'/>&nbsp;If you feel you have received this message in error, or wish to be removed, please <a href='" + ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?e=" + EmailAddress + "&g=" + GroupID + "&b=0&c=" + CustomerID + "&s=U'>Unsubscribe</a>.</div></p>";
                    ed.Content += unsubscribeText;
                }

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
        }

        private void SendAdminResponseEmails(Groups grpObject, Emails theEmailObject)
        {
            if (Response_AdminEmail.Length > 5 && Response_FromEmail.Length > 5)
            {
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                ed.CustomerID = CustomerID;
                ed.EmailAddress = Response_AdminEmail;
                ed.EmailSubject = Response_AdminMsgSubject;
                ed.FromName = "Activity Engine";
                ed.Process = "Activity Engine - ATHB_SO_subscribe.SendUserResponseEmails";
                ed.Source = "Activity Engine";
                ed.ReplyEmailAddress = Response_FromEmail;
                ed.CreatedUserID = User.UserID;
                ed.Content = ReplaceCodeSnippets(grpObject, theEmailObject, Response_AdminMsgBody);

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
        }
        #endregion

        #region Subscribe email to the Group

        private Emails SubscribeToGroup()
        {
            try
            {
                LoadFields();
                hUDFFields = GetGroupDataFields(GroupID);

                StringBuilder xmlProfile = new StringBuilder("");
                StringBuilder xmlUDF = new StringBuilder("");

                IDictionaryEnumerator en = hProfileFields.GetEnumerator();
                string firstname = string.Empty;
                string lastname = string.Empty;
                string state = string.Empty;
                xmlProfile.Append("<Emails>");
                while (en.MoveNext())
                {
                    try
                    {
                        if (en.Key.ToString().ToLower() == "firstname")
                            firstname = ECN_Framework_Common.Functions.StringFunctions.CleanXMLString(getQS(en.Value.ToString()));
                        if (en.Key.ToString().ToLower() == "lastname")
                            lastname = ECN_Framework_Common.Functions.StringFunctions.CleanXMLString(getQS(en.Value.ToString()));
                        if (en.Key.ToString().ToLower() == "state")
                            state = ECN_Framework_Common.Functions.StringFunctions.CleanXMLString(getQS(en.Value.ToString()));


                        if (en.Key.ToString().ToLower() == "notes")
                            xmlProfile.Append("<" + en.Key.ToString() + ">" + "<![CDATA[ [" + fromIP + "] [" + fromURL + "] [" + DateTime.Now.ToString() + "] ]]> " + "</" + en.Key.ToString() + ">");
                        else if (!en.Key.ToString().ToLower().Equals("user1"))
                            xmlProfile.Append("<" + en.Key.ToString() + ">" + ECN_Framework_Common.Functions.StringFunctions.CleanXMLString(getQS(en.Value.ToString())) + "</" + en.Key.ToString() + ">");
                    }
                    catch
                    { }
                }
                xmlProfile.Append("<user1>" + firstname +" "+lastname+" "+state + "</user1>");

                xmlProfile.Append("</Emails>");

                if (hUDFFields.Count > 0)
                {
                    xmlUDF.Append("<row>");
                    xmlUDF.Append("<ea kv=\"" + firstname + " " + lastname + " " + state + "\">" + ECN_Framework_Common.Functions.StringFunctions.CleanXMLString(getEmailAddress()) + "</ea>");
                    IDictionaryEnumerator en1 = hUDFFields.GetEnumerator();

                    while (en1.MoveNext())
                    {
                        try
                        {
                            xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v>" + ECN_Framework_Common.Functions.StringFunctions.CleanXMLString(getQS(en1.Value.ToString())) + "</v></udf>");
                        }
                        catch
                        { }
                    }
                    xmlUDF.Append("</row>");
                }

                UpdateToDB(xmlProfile.ToString(), xmlUDF.ToString());

                Emails retEmails = null;

                try
                {
                    retEmails = Emails.GetEmailByID(group.WhatEmail(getEmailAddress()).ID());
                }
                catch (Exception)
                {
                }

                return retEmails;

            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "OptIn.SubscribeToGroup", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                Response.Write("Error in Optin.  Customer Service has been notified.");
                return null;
            }
        }

        private KMPlatform.Entity.User getSuperUser(int customerID)
        {
            List<KMPlatform.Entity.User> userList = KMPlatform.BusinessLogic.User.GetByCustomerID(customerID);
            KMPlatform.Entity.User superUser = new KMPlatform.Entity.User();
            foreach (KMPlatform.Entity.User user in userList)
            {
                if (KM.Platform.User.IsAdministrator(user))
                {
                    superUser = user;
                    break;
                }
                //if (ECN_Framework_BusinessLayer.Communicator.EmailGroup.HasPermission(ECN_Framework_Common.Objects.Communicator.Enums.EntityRights.Edit, user))
                if (KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.Edit))
                {
                    superUser = user;
                    break;
                }
            }
            return superUser;
        }

        private void UpdateToDB(string xmlProfile, string xmlUDF)
        {

            DataTable dt = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsWithDupes(getSuperUser(CustomerID), Convert.ToInt32(getGroupID()), "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", getFormat(), getSubscribeTypeCode(), false, "user1", false,"ecn.activityengines.ATHB_SO_Subscribe");
        }

        #endregion


        private Hashtable GetGroupDataFields(int groupID)
        {
            string sqlstmt = " SELECT * FROM GroupDatafields WHERE GroupID=" + groupID + " and IsDeleted = 0";

            DataTable emailstable = DataFunctions.GetDataTable(sqlstmt);

            Hashtable fields = new Hashtable();
            foreach (DataRow dr in emailstable.Rows)
                fields.Add(Convert.ToInt32(dr["GroupDataFieldsID"]), "user_" + dr["ShortName"].ToString().ToLower());

            return fields;
        }

        private void LoadFields()
        {
            hProfileFields.Add("emailaddress", "e");
            hProfileFields.Add("title", "t");
            hProfileFields.Add("firstname", "fn");
            hProfileFields.Add("lastname", "ln");
            hProfileFields.Add("fullname", "n");
            hProfileFields.Add("company", "compname");
            hProfileFields.Add("occupation", "occ");
            hProfileFields.Add("address", "adr");
            hProfileFields.Add("address2", "adr2");
            hProfileFields.Add("city", "city");
            hProfileFields.Add("state", "state");
            hProfileFields.Add("zip", "zc");
            hProfileFields.Add("country", "ctry");
            hProfileFields.Add("voice", "ph");
            hProfileFields.Add("mobile", "mph");
            hProfileFields.Add("fax", "fax");
            hProfileFields.Add("website", "website");
            hProfileFields.Add("age", "age");
            hProfileFields.Add("income", "income");
            hProfileFields.Add("gender", "gndr");
            hProfileFields.Add("user1", "usr1");
            hProfileFields.Add("user2", "usr2");
            hProfileFields.Add("user3", "usr3");
            hProfileFields.Add("user4", "usr4");
            hProfileFields.Add("user5", "usr5");
            hProfileFields.Add("user6", "usr6");
            hProfileFields.Add("birthdate", "bdt");
            hProfileFields.Add("userevent1", "usrevt1");
            hProfileFields.Add("userevent1date", "usrevtdt1");
            hProfileFields.Add("userevent2", "usrevt2");
            hProfileFields.Add("userevent2date", "usrevtdt2");
            hProfileFields.Add("notes", "notes");
            hProfileFields.Add("password", "password");
        }


        #region Replace Code Snippets to real Value - which replaces %%**%% to real data..
        private string ReplaceCodeSnippets(Groups group, Emails emailObj, string emailbody)
        {
            emailbody = StringFunctions.Replace(emailbody, "%%GroupID%%", group.ID().ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%GroupName%%", group.Name.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%EmailID%%", emailObj.ID().ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%EmailAddress%%", emailObj.EmailAddress().ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Title%%", emailObj.Title.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%FirstName%%", emailObj.FirstName.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%LastName%%", emailObj.LastName.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%FullName%%", emailObj.FullName.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Company%%", emailObj.Company.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Occupation%%", emailObj.Occupation.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Address%%", emailObj.Address.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Address2%%", emailObj.Address2.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%City%%", emailObj.City.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%State%%", emailObj.State.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Zip%%", emailObj.Zip.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Country%%", emailObj.Country.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Voice%%", emailObj.Voice.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Mobile%%", emailObj.Mobile.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Fax%%", emailObj.Fax.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Website%%", emailObj.Website.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Age%%", emailObj.Age.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Income%%", emailObj.Income.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Gender%%", emailObj.Gender.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Notes%%", emailObj.Notes.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%BirthDate%%", emailObj.BirthDate.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User1%%", emailObj.User1.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User2%%", emailObj.User2.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User3%%", emailObj.User3.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User4%%", emailObj.User4.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User5%%", emailObj.User5.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User6%%", emailObj.User6.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent1%%", emailObj.UserEvent1.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent1Date%%", emailObj.UserEvent1Date.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent2%%", emailObj.UserEvent2.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent2Date%%", emailObj.UserEvent2Date.ToString());

            //UDF Data 
            SortedList UDFHash = group.UDFHash;
            ArrayList _keyArrayList = new ArrayList();
            ArrayList _UDFData = new ArrayList();

            if (UDFHash.Count > 0)
            {
                IDictionaryEnumerator UDFHashEnumerator = UDFHash.GetEnumerator();
                while (UDFHashEnumerator.MoveNext())
                {
                    string UDFData = "";
                    string _value = "user_" + UDFHashEnumerator.Value.ToString();
                    string _key = UDFHashEnumerator.Key.ToString();
                    try
                    {
                        UDFData = getQS(_value);
                        _keyArrayList.Add(_key);
                        _UDFData.Add(UDFData);
                        emailbody = StringFunctions.Replace(emailbody, "%%" + _value + "%%", UDFData);
                    }
                    catch
                    {
                        emailbody = StringFunctions.Replace(emailbody, "%%" + _value + "%%", "");
                    }
                }
            }
            //End UDF Data
            return emailbody;
        }
        #endregion
    }
}