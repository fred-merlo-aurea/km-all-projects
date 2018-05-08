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
using System.Linq;
using System.Collections.Generic;


namespace ecn.activityengines
{
    public partial class Optin : System.Web.UI.Page
    {
        Hashtable hProfileFields = new Hashtable();
        Hashtable hUDFFields = new Hashtable();

        private KMPlatform.Entity.User User = null;
        private int CustomerID = 0;
        private int GroupID = 0;
        private int SmartFormID = 0;
        private string EmailAddress = string.Empty;
        private string Format = string.Empty;
        private string SubscribeTypeCode = string.Empty;
        private int BlastID = 0;
        private string ReturnURL = string.Empty;
        private int EmailID = 0;

        private string getQS(string QSname)
        {
            return Request.QueryString[QSname].ToString();
        }
        private string fromURL
        {
            get { try { return Request.UrlReferrer.ToString(); } catch { return ""; } }
        }
        string fromIP
        {
            get { try { return Request.UserHostAddress.ToString(); } catch { return ""; } }
        }

        private void GetValuesFromQuerystring(string queryString)
        {
            ECN_Framework_Common.Objects.QueryString qs = ECN_Framework_Common.Objects.QueryString.GetECNParameters(Server.UrlDecode(QSCleanUP(queryString)));
            try{int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.CustomerID).ParameterValue, out CustomerID);}catch (Exception){}
            try{int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.GroupID).ParameterValue, out GroupID);}catch (Exception){}
            try{int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.SmartFormID).ParameterValue, out SmartFormID);}catch (Exception){}
            try{EmailAddress = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.EmailAddress).ParameterValue;}catch (Exception){}
            try{SubscribeTypeCode = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.Subscribe).ParameterValue;}catch (Exception){}
            try{Format = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.Format).ParameterValue;}catch (Exception){}
            try{int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.BlastID).ParameterValue, out BlastID);}catch (Exception){}
            try{ReturnURL = qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.URL).ParameterValue;}catch (Exception){}
            try { int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.EmailID).ParameterValue, out EmailID);}catch (Exception){}
        }

        private string QSCleanUP(string querystring)
        {
            try
            {
                querystring = querystring.Replace("&amp;", "&");
                querystring = querystring.Replace("&lt;", "<");
                querystring = querystring.Replace("&gt;", ">");
            }
            catch (Exception)
            {
            }

            return querystring.Trim();
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.Url.Query.ToString().Length > 0)
            {
                GetValuesFromQuerystring(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
            }

            ECN_Framework_Entities.Communicator.SmartFormTracking sft = new ECN_Framework_Entities.Communicator.SmartFormTracking();
            if(CustomerID != 0)
                sft.CustomerID = CustomerID;
            if (SmartFormID != 0)
                sft.SFID = SmartFormID;
            if(GroupID != 0)
                sft.GroupID = GroupID;
            if (BlastID != 0)
                sft.BlastID = BlastID;
            sft.ReferringUrl = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();
            sft.ActivityDate = DateTime.Now;
            ECN_Framework_BusinessLayer.Communicator.SmartFormTracking.Insert(sft);

            if (Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
            {
                User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                User = (KMPlatform.Entity.User)Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
            }

            //Validate 
            //  1. CustomerID && GroupID should be > than 0
            //  2. Valid Emailaddress
            bool validEmail = ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(EmailAddress);
            //  3. SFID belongs to correct Group.
            int checkGroupID = ECN_Framework_BusinessLayer.Communicator.SmartFormsHistory.GetGroupID(CustomerID, SmartFormID);

            if (GroupID > 0 && ((SmartFormID > 0 && GroupID == checkGroupID) || SmartFormID == 0) && validEmail)
            {
               
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(GroupID);
                if (group != null)
                {
                    SubscribeToGroup();
                        
                    int eID = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetEmailIDFromWhatEmail_NoAccessCheck(GroupID, CustomerID, EmailAddress);
                    ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.Insert(BlastID, eID, "subscribe", "S", "", User);
                    ECN_Framework_BusinessLayer.Communicator.EventOrganizer.Event(CustomerID, GroupID, eID, User, SmartFormID);

                    if (SmartFormID > 0 && eID > 0)
                    {
                        ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(eID);
                        //get smartForm Details
                        ECN_Framework_Entities.Communicator.SmartFormsHistory smartFormHistory = ECN_Framework_BusinessLayer.Communicator.SmartFormsHistory.GetBySmartFormID_NoAccessCheck(SmartFormID, GroupID);
                            
                        //now send the response emails confirmation to the user & admins.
                        SendUserResponseEmails(smartFormHistory, group, email);
                        SendAdminResponseEmails(smartFormHistory, group, email);

                        //Finally show the user, the Thankyou Page / redirect'em to a page that's setup.
                        if (smartFormHistory.Response_UserScreen.ToLower().StartsWith("http://"))
                        {
                            smartFormHistory.Response_UserScreen = ReplaceCodeSnippets(group, email, smartFormHistory.Response_UserScreen);
                            Response.Write("<script>document.location.href='" + smartFormHistory.Response_UserScreen + "';</script>");
                        }
                        else if (smartFormHistory.Response_UserScreen.Trim().Length > 0)
                        {
                            smartFormHistory.Response_UserScreen = ReplaceCodeSnippets(group, email, smartFormHistory.Response_UserScreen);
                            Response.Write(smartFormHistory.Response_UserScreen);
                        }
                        else
                        {

                        }
                    }                        
                }
            }
            else if (SmartFormID > 0 && validEmail)
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("Unknown GroupID: " + GroupID + " for SFID: " + SmartFormID, "OptIn.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            else if (!validEmail)
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("Bad Email Address: " + EmailAddress, "OptIn.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
        }

        

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<br><b>Blast ID:</b>&nbsp;" + BlastID);
                adminEmailVariables.AppendLine("<br><b>Group ID:</b>&nbsp;" + GroupID);
                adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + EmailID);
                adminEmailVariables.AppendLine("<br><b>Smart Form ID:</b>&nbsp;" + SmartFormID);
                adminEmailVariables.AppendLine("<br><b>Email Address:</b>&nbsp;" + EmailAddress);
                adminEmailVariables.AppendLine("<br><b>Customer ID:</b>&nbsp;" + CustomerID);
                adminEmailVariables.AppendLine("<BR>Page URL: " + Request.RawUrl.ToString());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }
        
        private void SendUserResponseEmails(ECN_Framework_Entities.Communicator.SmartFormsHistory smartFormHistory, ECN_Framework_Entities.Communicator.Group group, ECN_Framework_Entities.Communicator.Email email)
        {
            if (smartFormHistory.Response_FromEmail.Length > 5 && smartFormHistory.Response_UserMsgSubject.Trim().Length > 0 && smartFormHistory.Response_UserMsgBody.Trim().Length > 0)
            {
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                ed.CustomerID = CustomerID;
                ed.EmailAddress = email.EmailAddress;
                ed.EmailSubject = smartFormHistory.Response_UserMsgSubject;
                ed.FromName = "Activity Engine";
                ed.Process = "Activity Engine - Optin.SendUserResponseEmails";
                ed.Source = "Activity Engine";
                ed.ReplyEmailAddress = smartFormHistory.Response_FromEmail;
                ed.CreatedUserID = User.UserID;
                ed.Content = ReplaceCodeSnippets(group, email, smartFormHistory.Response_UserMsgBody);

                if (ed.Content.ToLower().IndexOf("%%unsubscribelink%%") > 0)
                {
                    ed.Content = ECN_Framework_Common.Functions.StringFunctions.Replace(ed.Content, "http://%%unsubscribelink%%", "%%unsubscribelink%%");
                    ed.Content = ECN_Framework_Common.Functions.StringFunctions.Replace(ed.Content, "%%unsubscribelink%%", ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?e=" + EmailAddress + "&g=" + GroupID + "&b=0&c=" + CustomerID + "&s=U");
                }
                else
                {
                    //Add Unsubscribe Link at the bottom of the email as per CAN-SPAM & USI requested it to be done. 
                    string unsubscribeText = "<p style=\"padding-TOP:5px\"><div style=\"font-size:8.0pt;font-family:'Arial,sans-serif'; color:#666666\"><IMG style=\"POSITION:relative; TOP:5px\" src='" + ConfigurationManager.AppSettings["Image_DomainPath"] + "/images/Sure-Unsubscribe.gif'/>&nbsp;If you feel you have received this message in error, or wish to be removed, please <a href='" + ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?e=" + EmailAddress + "&g=" + GroupID + "&b=0&c=" + CustomerID + "&s=U'>Unsubscribe</a>.</div></p>";
                    ed.Content += unsubscribeText;
                }


                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
        }

        private void SendAdminResponseEmails(ECN_Framework_Entities.Communicator.SmartFormsHistory smartFormHistory, ECN_Framework_Entities.Communicator.Group group, ECN_Framework_Entities.Communicator.Email email)
        {
            if (smartFormHistory.Response_AdminEmail.Length > 5)
            {
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                ed.CustomerID = CustomerID;
                ed.EmailAddress = smartFormHistory.Response_AdminEmail;
                ed.EmailSubject = smartFormHistory.Response_AdminMsgSubject;
                ed.FromName = "Activity Engine";
                ed.Process = "Activity Engine - Optin.SendAdminResponseEmails";
                ed.Source = "Activity Engine";
                ed.ReplyEmailAddress = "emaildirect@ecn5.com";
                ed.CreatedUserID = User.UserID;
                ed.Content = ReplaceCodeSnippets(group, email, smartFormHistory.Response_AdminMsgBody);

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
        }

        private string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text.Trim();
        }

        private void SubscribeToGroup()
        {
            LoadFields();
            hUDFFields = GetGroupDataFields(GroupID);

            StringBuilder xmlProfile = new StringBuilder("");
            StringBuilder xmlUDF = new StringBuilder("");

            IDictionaryEnumerator en = hProfileFields.GetEnumerator();

            xmlProfile.Append("<Emails>");
            xmlProfile.Append("<subscribetypecode>" + SubscribeTypeCode + "</subscribetypecode>");

            while (en.MoveNext())
            {
                try
                {
                    if (en.Key.ToString().ToLower() == "notes")
                        xmlProfile.Append("<" + en.Key.ToString() + ">" + "<![CDATA[ [" + fromIP + "] [" + fromURL + "] [" + DateTime.Now.ToString() + "] ]]> " + "</" + en.Key.ToString() + ">");
                    else
                        xmlProfile.Append("<" + en.Key.ToString() + ">" + CleanXMLString(getQS(en.Value.ToString())) + "</" + en.Key.ToString() + ">");
                }
                catch
                { }
            }

            xmlProfile.Append("</Emails>");

            if (hUDFFields.Count > 0)
            {
                xmlUDF.Append("<row>");
                xmlUDF.Append("<ea>" + CleanXMLString(EmailAddress) + "</ea>");

                IDictionaryEnumerator en1 = hUDFFields.GetEnumerator();

                while (en1.MoveNext())
                {
                    try
                    {
                        xmlUDF.Append("<udf id=\"" + en1.Key.ToString() + "\"><v><![CDATA[" + CleanXMLString(getQS(en1.Value.ToString())) + "]]></v></udf>");
                    }
                    catch
                    { }
                }
                xmlUDF.Append("</row>");
            }

            try
            {
                DataTable emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(User, CustomerID, GroupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", Format, SubscribeTypeCode, false, "", "ActivityEngine.Optin");
                    
            }
            catch (SqlException ex)
            {
                if (ex.Message.ToLower().Contains("violation of unique key"))
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "OptIn.SubscribeToGroup.UpdateToDB", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateNote());
                }
            }
        }      

        private Hashtable GetGroupDataFields(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);

            Hashtable fields = new Hashtable();
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
                fields.Add(gdf.GroupDataFieldsID, "user_" + gdf.ShortName.ToLower());

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

        private string ReplaceCodeSnippets(ECN_Framework_Entities.Communicator.Group group, ECN_Framework_Entities.Communicator.Email email, string emailbody)
        {
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%GroupID%%", group.GroupID.ToString());
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%GroupName%%", group.GroupName);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%EmailID%%", email.EmailID.ToString());
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%EmailAddress%%", email.EmailAddress);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Title%%", email.Title);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%FirstName%%", email.FirstName);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%LastName%%", email.LastName);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%FullName%%", email.FullName);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Company%%", email.Company);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Occupation%%", email.Occupation);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Address%%", email.Address);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Address2%%", email.Address2);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%City%%", email.City);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%State%%", email.State);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Zip%%", email.Zip);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Country%%", email.Country);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Voice%%", email.Voice);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Mobile%%", email.Mobile);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Fax%%", email.Fax);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Website%%", email.Website);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Age%%", email.Age);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Income%%", email.Income);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Gender%%", email.Gender);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%Notes%%", email.Notes);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%BirthDate%%", email.Birthdate == null ? "" : email.Birthdate.ToString());
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User1%%", email.User1);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User2%%", email.User2);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User3%%", email.User3);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User4%%", email.User4);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User5%%", email.User5);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%User6%%", email.User6);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%UserEvent1%%", email.UserEvent1);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%UserEvent1Date%%", email.UserEvent1Date == null ? "" : email.UserEvent1Date.ToString());
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%UserEvent2%%", email.UserEvent2);
            emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%UserEvent2Date%%", email.UserEvent2Date == null ? "" : email.UserEvent2Date.ToString());

            //UDF Data 
            ArrayList _keyArrayList = new ArrayList();
            ArrayList _UDFData = new ArrayList();

            if (hUDFFields.Count > 0)
            {
                IDictionaryEnumerator UDFHashEnumerator = hUDFFields.GetEnumerator();
                while (UDFHashEnumerator.MoveNext())
                {
                    string UDFData = "";
                    string _value = UDFHashEnumerator.Value.ToString().Contains("user_") == true ? UDFHashEnumerator.Value.ToString() : "user_" + UDFHashEnumerator.Value.ToString();
                    string _key = UDFHashEnumerator.Key.ToString();
                    try
                    {
                        UDFData = getQS(_value);
                        _keyArrayList.Add(_key);
                        _UDFData.Add(UDFData);
                        emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%" + _value + "%%", UDFData);
                    }
                    catch
                    {
                        emailbody = ECN_Framework_Common.Functions.StringFunctions.Replace(emailbody, "%%" + _value + "%%", "");
                    }
                }
            }

            return emailbody;
        }

    }
}