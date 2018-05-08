using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Threading;
using System.Web.Caching;
using ecn.communicator.classes;
using KM.Common;
using KM.Common.Entity;
using DataFunctions = ecn.common.classes.DataFunctions;
using KMBusinessLogic = KMPlatform.BusinessLogic;
using User = KMPlatform.Entity.User;

namespace ecn.activityengines
{
    public partial class SO_multigroup_subscribe : System.Web.UI.Page
    {
        private const string HtmlScriptIncludePopup =
            "<script>javascript:window.open(" +
            "'../includes/popup.aspx','Error', 'left=100,top=100,height=330,width=615," +
            "resizable=yes,scrollbar=yes,status=no' );</script>";
        private const string HtmlExceptionTemplate = "<font color='#FF0000'>Error Occured</font> <br> {0}";
        private const string SubscriptionAddedTemplate = "SO Subscription through Website. DateAdded: {0}";
        private const int SqlTimeoutZero = 0;
        private const string SqlInsertEmail = 
            "INSERT INTO Emails " +
            "(EmailAddress,CustomerID,Title,FirstName,LastName,FullName,Company,Occupation,Address,Address2," +
            "City,State,Zip,Country,Voice,Mobile,Fax,Website,Age,Income,Gender,User1,User2,User3,User4,User5," +
            "User6,Birthdate,UserEvent1,UserEvent1Date,UserEvent2,UserEvent2Date,Notes,DateAdded, DateUpdated) " +
            "VALUES " +
            "(@emailAddress,@customer_id,@title,@first_name,@last_name,@full_name,@company,@occupation," +
            "@address,@address2,@city,@state,@zip,@country,@voice,@mobile,@fax,@website,@age,@income,@gender," +
            "@user1,@user2,@user3,@user4,@user5,@user6,@birthdate,@user_event1,@user_event1_date,@user_event2," +
            "@user_event2_date,@notes,@DateAdded,@DateUpdated) SELECT @@IDENTITY";
        private const string SqlUpdateEmail = 
            "UPDATE Emails SET " +
            "Title = @title, FirstName = @first_name, LastName = @last_name, FullName = @full_name, " +
            "Company = @company, Occupation = @occupation, Address = @address, Address2 = @address2, " +
            "City = @city, State = @state, Zip = @zip, Country = @country, Voice = @voice, " +
            "Mobile = @mobile, Fax = @fax, Website = @website, Age = @age, Income = @income, " +
            "Gender = @gender, User1 = @user1, User2 = @user2, User3 = @user3, User4 = @user4, " +
            "User5 = @user5, User6 = @user6, Birthdate = @birthdate, " +
            "UserEvent1 = @user_event1, UserEvent1Date = @user_event1_date, " +
            "UserEvent2 = @user_event2, UserEvent2Date = @user_event2_date " +
            "WHERE EmailID = @email_id;";
        private const string AppSettingEcnEngineAccessKey = "ECNEngineAccessKey";
        private const string AppSettingKmApplication = "KMCommon_Application";
        private const string PageLoadMethodName = "SO_multigroup_subscribe.Page_Load";
        private const string MultiGroupSubscribeErrorMessage = "Error in Multi-Group Subscribe.  Customer Service has been notified.";
        private const string UserCacheKeyPrefix = "cache_user_by_AccessKey_";
        private const string UrlStart = "http://";
        private const string ValueY = "Y";
        private const string SFmodeParam = "SFmode";
        private const string QsValueManage = "MANAGE";
        public KMPlatform.Entity.User User = null;
        public string Response_FromEmail = "";
        public string Response_UserMsgSubject = "";
        public string Response_UserMsgBody = "";
        public string Response_UserScreen = "";
        public string Response_AdminEmail = "";
        public string Response_AdminMsgSubject = "";
        public string Response_AdminMsgBody = "";
        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
        int SFGroupID = 0;
        string SubscriptionGroupIDs = string.Empty;

        Emails CurrentEmail;
        string EmailAddress = "";
        string Subscribe = "";
        string Format = "";
        bool double_opt_in_needed;

        int CustomerID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingKmApplication]);
            SetUserFromCache();

            try
            {
                var smartFormId = getSmartFormID();
                if (smartFormId > 0)
                {
                    getSmartFormDetails(smartFormId);
                }
                if (smartFormId > 0 && (!string.IsNullOrWhiteSpace(getEmailAddress()) || double_opt_in_needed))
                {
                    EmailAddress = getEmailAddress();
                    Subscribe = getSubscribe();
                    Format = getFormat();

                    // If the double opt in is needed and they HAVE an emailid this is due
                    // to them clickign on the subscribe link and we should set all P status options to S
                    var returningFromEmailClick = double_opt_in_needed && getSendOptIn() != ValueY;

                    var sFgroup = new Groups(SFGroupID);
                    UpdateEmailGroups(ref CurrentEmail, returningFromEmailClick, sFgroup);

                    if (CurrentEmail == null)
                    {
                        if (Response_UserScreen.StartsWith(UrlStart, StringComparison.OrdinalIgnoreCase))
                        {
                            Response.Redirect(Response_UserScreen);
                        }
                        else
                        {
                            Response.Write(Response_UserScreen);
                        }
                    }
                    else
                    {
                        // now send the response emails confirmation to the user & admins.
                        // as long as they are not returning from their email click
                        if (!returningFromEmailClick)
                        {
                            SendUserResponseEmails(sFgroup, CurrentEmail);
                            SendAdminResponseEmails(sFgroup, CurrentEmail);
                            // Finally show the user, the Thankyou Page / redirect'em to a page that's setup.
                            if (Response_UserScreen.StartsWith(UrlStart, StringComparison.OrdinalIgnoreCase))
                            {
                                Response_UserScreen = ReplaceCodeSnippets(sFgroup, CurrentEmail, Response_UserScreen);
                                Response.Redirect(Response_UserScreen);
                            }
                            else
                            {
                                Response_UserScreen = ReplaceCodeSnippets(sFgroup, CurrentEmail, Response_UserScreen);
                                Response.Write(Response_UserScreen);
                            }
                        }
                        else
                        {
                            var finalUrLorBody = GetUrlOrBodyFromCustomerTemplate();
                            if (Response_UserScreen.StartsWith(UrlStart, StringComparison.OrdinalIgnoreCase))
                            {
                                Response_UserScreen = ReplaceCodeSnippets(sFgroup, CurrentEmail, finalUrLorBody);
                                Response.Redirect(finalUrLorBody);
                            }
                            else
                            {
                                Response_UserScreen = ReplaceCodeSnippets(sFgroup, CurrentEmail, finalUrLorBody);
                                Response.Write(finalUrLorBody);
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException exception)
            {
                var severityLevel = Severity.SeverityLevel.Non_Critical;
                ApplicationLog.LogError(exception.Message, PageLoadMethodName, applicationId, severityLevel);
            }
            catch (Exception exception)
            {
                ApplicationLog.LogCriticalError(exception, PageLoadMethodName, applicationId, CreateNote());
                Response.Write(MultiGroupSubscribeErrorMessage);
            }
        }

        private void SetUserFromCache()
        {
            var ecnEngineAccessKey = ConfigurationManager.AppSettings[AppSettingEcnEngineAccessKey];
            var cacheKeyUser = $"{UserCacheKeyPrefix}{ecnEngineAccessKey}";
            if (Cache[cacheKeyUser] == null)
            {
                User = KMBusinessLogic.User.GetByAccessKey(ecnEngineAccessKey, false);

                Cache.Add(
                    cacheKeyUser,
                    User,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(15),
                    CacheItemPriority.Normal,
                    null);
            }
            else
            {
                User = Cache[cacheKeyUser] as User;
            }
        }

        private void UpdateEmailGroups(
            ref Emails currentEmail,
            bool returingFromEmailClick,
            Groups sFgroup)
        {
            var groups = SubscriptionGroupIDs.Split(',');
            var oldProfile = sFgroup.WhatEmailForCustomer(EmailAddress);
            var removeFromGroups = string.Empty;
            if (returingFromEmailClick)
            {
                currentEmail = new Emails(getEmailID());
                oldProfile = currentEmail;
                // For all the smart form groups, if we are already in the list it is due to a pending thus reg them.
                foreach (var groupValue in groups)
                {
                    var groupToTest = new Groups(groupValue);

                    if (groupToTest.HasPendingEmail(oldProfile))
                    {
                        AddtoGroup(Convert.ToInt32(groupValue));
                    }
                }
            }
            else
            {
                foreach (var groupValue in groups)
                {
                    try
                    {
                        if (getQS($"g_{groupValue}").Equals(ValueY, StringComparison.OrdinalIgnoreCase))
                        {
                            AddtoGroup(Convert.ToInt32(groupValue));
                        }
                        else
                        {
                            removeFromGroups += string.IsNullOrWhiteSpace(removeFromGroups)
                                ? groupValue
                                : $",{groupValue}";
                        }
                    }
                    catch
                    {
                        removeFromGroups += string.IsNullOrWhiteSpace(removeFromGroups)
                            ? groupValue
                            : $",{groupValue}";
                    }
                }
            }

            if (oldProfile != null)
            {
                currentEmail = Emails.GetEmailByID(oldProfile.ID());

                // if it is "Manage my subscriptions" page.
                if (getQS(SFmodeParam).Equals(QsValueManage, StringComparison.OrdinalIgnoreCase) &&
                    removeFromGroups.Length > 0)
                {
                    var query = string.Concat(
                        "update emailgroups set subscribeTypeCode='U', LastChanged = getdate()",
                        $" where subscribeTypeCode<>'U' and EmailID = {currentEmail.ID()}",
                        $" and GroupID in ({removeFromGroups})");
                    DataFunctions.Execute(query);
                }
            }
        }

        private string GetUrlOrBodyFromCustomerTemplate()
        {
            var responseFinalUrLorBody = string.Empty;

            // The final URL or body of the message comes from a customer template 
            // Subscribe-DblOptinMutiGroupRef
            // Now that they are good to go, we can finish.
            var templateQuery = string.Concat(
                $" SELECT HeaderSource FROM {accountsdb}.dbo.CustomerTemplate",
                $" WHERE CustomerID = {CustomerID}",
                " AND TemplateTypeCode='Subscribe-DblOptinMutiGroupRef' and IsActive=1 and IsDeleted = 0");

            try
            {
                responseFinalUrLorBody = DataFunctions.ExecuteScalar(templateQuery).ToString();
            }
            catch (Exception)
            {
                templateQuery = string.Concat(
                    $" SELECT HeaderSource FROM {accountsdb}.dbo.CustomerTemplate",
                    " WHERE CustomerID = 1 AND TemplateTypeCode='Subscribe-DblOptinMutiGroupRef'",
                    " and IsActive=1 and IsDeleted = 0");
                responseFinalUrLorBody = DataFunctions.ExecuteScalar(templateQuery).ToString();
            }

            return responseFinalUrLorBody;
        }

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<br><b>Blast ID:</b>&nbsp;" + getBlastID());
                adminEmailVariables.AppendLine("<br><b>Group IDs:</b>&nbsp;" + SubscriptionGroupIDs);
                adminEmailVariables.AppendLine("<br><b>Smart Form ID:</b>&nbsp;" + getSmartFormID());
                adminEmailVariables.AppendLine("<br><b>Email Address:</b>&nbsp;" + getEmailAddress());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        //private void NotifyAdmin(Exception ex, string groupIDs, int blastID, string emailAddress, int sfID)
        //{
        //    StringBuilder adminEmailVariables = new StringBuilder();
        //    string admimEmailBody = string.Empty;

        //    adminEmailVariables.AppendLine("<br><b>Blast ID:</b>&nbsp;" + getBlastID());
        //    adminEmailVariables.AppendLine("<br><b>Group IDs:</b>&nbsp;" + SubscriptionGroupIDs);
        //    adminEmailVariables.AppendLine("<br><b>Smart Form ID:</b>&nbsp;" + getSmartFormID());
        //    adminEmailVariables.AppendLine("<br><b>Email Address:</b>&nbsp;" + emailAddress);

        //    admimEmailBody = ActivityError.CreateMessage(ex, Request, adminEmailVariables.ToString());

        //    Helper.SendMessage("Error in Activity Engine: SO Multi-Group Subscribe", admimEmailBody);
        //}
        #region Get Request Variables methods [ getEmailID, getCustomerID, getBlastID, getEmailAddress, getReturnURL, getFullName, getGroupID, getSubscribe, getFormat, getFirstName getLastName, getFullName, getStreetAddress, getCompanyName, getCity, getState, getZipCode, getPhone, getBirthdate]

        private int getEmailID()
        {
            try { return Convert.ToInt32(Request.Params["ei"].ToString()); }
            catch { return 0; }
        }

        private int getCustomerID()
        {
            try { return Convert.ToInt32(Request.Params["c"].ToString()); }
            catch { return 0; }
        }

        private string getSendOptIn()
        {
            return Request.Params["soptin"];
        }

        private int getBlastID()
        {
            try { return Convert.ToInt32(Request.Params["b"].ToString()); }
            catch { return 0; }
        }

        private string getEmailAddress()
        {
            try { return Request.Params["e"].ToString().Trim(); }
            catch { return string.Empty; }
        }

        private int getSmartFormID()
        {
            try { return Convert.ToInt32(Request.Params["sfID"]); }
            catch { return 0; }
        }

        private string getReturnURL()
        {
            try { return Request.Params["url"].ToString(); }
            catch { return string.Empty; }
        }

        private string getGroupID()
        {
            try { return Request.Params["g"].ToString(); }
            catch { return string.Empty; }
        }

        private string getSubscribe()
        {
            try { return Request.Params["s"].ToString(); }
            catch { return "S"; }
        }
        private string getFormat()
        {
            try { return Request.Params["f"].ToString(); }
            catch { return "html"; }
        }
        private string getTitle()
        {
            try { return Request.Params["t"].ToString(); }
            catch { return string.Empty; }
        }
        private string getFirstName()
        {
            try { return Request.Params["fn"].ToString(); }
            catch { return string.Empty; }
        }
        private string getLastName()
        {
            try { return Request.Params["ln"].ToString(); }
            catch { return string.Empty; }
        }
        private string getFullName()
        {
            try { return Request.Params["n"].ToString(); }
            catch { return string.Empty; }
        }
        private string getStreetAddress()
        {
            try { return Request.Params["adr"].ToString(); }
            catch { return string.Empty; }
        }
        private string getStreetAddress2()
        {
            try { return Request.Params["adr2"].ToString(); }
            catch { return string.Empty; }
        }
        private string getCompanyName()
        {
            try { return Request.Params["compname"].ToString(); }
            catch { return string.Empty; }
        }
        private string getCity()
        {
            try { return Request.Params["city"].ToString(); }
            catch { return string.Empty; }
        }
        private string getState()
        {
            try { return Request.Params["state"].ToString(); }
            catch { return string.Empty; }
        }
        private string getCountry()
        {
            try { return Request.Params["ctry"].ToString(); }
            catch { return string.Empty; }
        }
        private string getZipCode()
        {
            try { return Request.Params["zc"].ToString(); }
            catch { return string.Empty; }
        }
        private string getPhone()
        {
            try { return Request.Params["ph"].ToString(); }
            catch { return string.Empty; }
        }
        private string getMobilePhone()
        {
            try { return Request.Params["mph"].ToString(); }
            catch { return string.Empty; }
        }
        private string getFax()
        {
            try { return Request.Params["fax"].ToString(); }
            catch { return string.Empty; }
        }
        private string getWebsite()
        {
            try { return Request.Params["website"].ToString(); }
            catch { return string.Empty; }
        }
        private string getAge()
        {
            try { return Request.Params["age"].ToString(); }
            catch { return string.Empty; }
        }
        private string getIncome()
        {
            try { return Request.Params["income"].ToString(); }
            catch { return string.Empty; }
        }
        private string getGender()
        {
            try { return Request.Params["gndr"].ToString(); }
            catch { return string.Empty; }
        }
        private string getOccupation()
        {
            try { return Request.Params["occ"].ToString(); }
            catch { return string.Empty; }
        }
        private string getBirthdate()
        {
            try { return (DateTime.Parse(Request.Params["bdt"].ToString())).ToString(); }
            catch { return string.Empty; }
        }
        private string getUser1()
        {
            try { return Request.Params["usr1"].ToString(); }
            catch
            { return string.Empty; }
        }
        private string getUser2()
        {
            try { return Request.Params["usr2"].ToString(); }
            catch { return string.Empty; }
        }
        private string getUser3()
        {
            try { return Request.Params["usr3"].ToString(); }
            catch { return string.Empty; }
        }
        private string getUser4()
        {
            try { return Request.Params["usr4"].ToString(); }
            catch { return string.Empty; }
        }
        private string getUser5()
        {
            try { return Request.Params["usr5"].ToString(); }
            catch { return string.Empty; }
        }
        private string getUser6()
        {
            try { return Request.Params["usr6"].ToString(); }
            catch { return string.Empty; }
        }
        private string getUserEvent1()
        {
            try { return Request.Params["usrevt1"].ToString(); }
            catch { return string.Empty; }
        }
        private string getUserEvent1Date()
        {
            try { return (DateTime.Parse(Request.Params["usrevtdt1"].ToString())).ToString(); }
            catch { return string.Empty; }
        }
        private string getUserEvent2()
        {
            try { return Request.Params["usrevt2"].ToString(); }
            catch { return string.Empty; }
        }
        private string getUserEvent2Date()
        {
            try { return (DateTime.Parse(Request.Params["usrevtdt2"].ToString())).ToString(); }
            catch { return string.Empty; }
        }

        //Get UDF Values.. 
        private string getQS(string _value)
        {
            try {
                 return Request.Params[_value].ToString();
            } catch {
                 return string.Empty;
            }
        }
        #endregion

        #region subscribe to groups

        private void AddtoGroup(int GroupID)
        {
            Groups current_group = new Groups(GroupID);

            if (CurrentEmail == null)
                CurrentEmail = CreateEmailRecord(current_group);

            
            SubscribeToGroup(current_group, Subscribe, Format);
            
            //Check for Group Trigger Events 
            EmailActivityLog log;
            try
            {
                log = new EmailActivityLog(EmailActivityLog.InsertSubscribe(CurrentEmail.ID(), 0, Subscribe));
                log.SetGroup(current_group);
                log.SetEmail(new Emails(CurrentEmail.ID()));

                EventOrganizer eventer = new EventOrganizer();
                eventer.CustomerID(current_group.CustomerID());
                eventer.Event(log);
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
                
            
            

        }

        private Emails CreateEmailRecord(Groups groups)
        {
            Guard.NotNull(groups, nameof(groups));

            var emailId = 0;
            var oldEmail = groups.WhatEmailForCustomer(EmailAddress);

            if (null == oldEmail)
            {
                using (var dbConnection = new SqlConnection(DataFunctions.connStr))
                {
                    using (var insertCommand = new SqlCommand(SqlInsertEmail, dbConnection))
                    {
                        insertCommand.Parameters.Add("@emailAddress", SqlDbType.VarChar, 250).Value = EmailAddress;
                        insertCommand.Parameters.Add("@customer_id", SqlDbType.Int, 4).Value = CustomerID;
                        FillCommandParams(insertCommand.Parameters);
                        insertCommand.Parameters.Add("@notes", SqlDbType.Text).Value =
                            string.Format(SubscriptionAddedTemplate, DateTime.Now);
                        insertCommand.Parameters.Add("@DateAdded", SqlDbType.DateTime).Value = DateTime.Now.ToString();
                        insertCommand.Parameters.Add("@DateUpdated", SqlDbType.DateTime).Value =
                            DateTime.Now.ToString();
                        insertCommand.CommandTimeout = SqlTimeoutZero;
                        insertCommand.Connection.Open();
                        emailId = ToInt32(insertCommand.ExecuteScalar().ToString());
                        insertCommand.Connection.Close();
                    }
                }
            }
            else
            {
                try
                {
                    emailId = oldEmail.ID();
                    using (var dbConnection = new SqlConnection(DataFunctions.connStr))
                    {
                        using (var updateCommand = new SqlCommand(SqlUpdateEmail, dbConnection))
                        {
                            updateCommand.Parameters.Add("@email_id", SqlDbType.Int, 4).Value = oldEmail.ID();
                            FillCommandParams(updateCommand.Parameters);
                            updateCommand.CommandTimeout = SqlTimeoutZero;
                            updateCommand.Connection.Open();
                            updateCommand.Prepare();
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception exception)
                {
                    PopUp.PopupMsg = string.Format(HtmlExceptionTemplate, exception);
                    Response.Write(HtmlScriptIncludePopup);
                }
            }

            return Emails.GetEmailByID(emailId);
        }

        private void FillCommandParams(SqlParameterCollection sqlParams)
        {
            Guard.NotNull(sqlParams, nameof(sqlParams));

            DateTime dateTime;
            var birthDate = DateTime.TryParse(getBirthdate(), out dateTime) ? dateTime : SqlDateTime.Null;
            var userEvent1Date = DateTime.TryParse(getUserEvent1Date(), out dateTime) ? dateTime : SqlDateTime.Null;
            var userEvent2Date = DateTime.TryParse(getUserEvent1Date(), out dateTime) ? dateTime : SqlDateTime.Null;

            sqlParams.Add("@title", SqlDbType.VarChar, 50).Value = getTitle();
            sqlParams.Add("@first_name", SqlDbType.VarChar, 50).Value = getFirstName();
            sqlParams.Add("@last_name", SqlDbType.VarChar, 50).Value = getLastName();
            sqlParams.Add("@full_name", SqlDbType.VarChar, 50).Value = getFullName();
            sqlParams.Add("@company", SqlDbType.VarChar, 50).Value = getCompanyName();
            sqlParams.Add("@occupation", SqlDbType.VarChar, 50).Value = getOccupation();
            sqlParams.Add("@address", SqlDbType.VarChar, 255).Value = getStreetAddress();
            sqlParams.Add("@address2", SqlDbType.VarChar, 255).Value = getStreetAddress2();
            sqlParams.Add("@city", SqlDbType.VarChar, 50).Value = getCity();
            sqlParams.Add("@state", SqlDbType.VarChar, 50).Value = getState();
            sqlParams.Add("@zip", SqlDbType.VarChar, 50).Value = getZipCode();
            sqlParams.Add("@country", SqlDbType.VarChar, 50).Value = getCountry();
            sqlParams.Add("@voice", SqlDbType.VarChar, 50).Value = getPhone();
            sqlParams.Add("@mobile", SqlDbType.VarChar, 50).Value = getMobilePhone();
            sqlParams.Add("@fax", SqlDbType.VarChar, 50).Value = getFax();
            sqlParams.Add("@website", SqlDbType.VarChar, 50).Value = getWebsite();
            sqlParams.Add("@age", SqlDbType.VarChar, 50).Value = getAge();
            sqlParams.Add("@income", SqlDbType.VarChar, 50).Value = getIncome();
            sqlParams.Add("@gender", SqlDbType.VarChar, 50).Value = getGender();
            sqlParams.Add("@user1", SqlDbType.VarChar, 255).Value = getUser1();
            sqlParams.Add("@user2", SqlDbType.VarChar, 255).Value = getUser2();
            sqlParams.Add("@user3", SqlDbType.VarChar, 255).Value = getUser3();
            sqlParams.Add("@user4", SqlDbType.VarChar, 255).Value = getUser4();
            sqlParams.Add("@user5", SqlDbType.VarChar, 255).Value = getUser5();
            sqlParams.Add("@user6", SqlDbType.VarChar, 255).Value = getUser6();
            sqlParams.Add("@birthdate", SqlDbType.DateTime).Value = birthDate;
            sqlParams.Add("@user_event1", SqlDbType.VarChar, 50).Value = getUserEvent1();
            sqlParams.Add("@user_event1_date", SqlDbType.DateTime).Value = userEvent1Date;
            sqlParams.Add("@user_event2", SqlDbType.VarChar, 50).Value = getUserEvent2();
            sqlParams.Add("@user_event2_date", SqlDbType.DateTime).Value = userEvent2Date;
        }

        private void SubscribeToGroup(Groups group, string theSubscribe, string theFormat)
        {
            SortedList UDFHash = group.UDFHash;
            
            // No attaching emails twice.

            if (group.WhatEmail(CurrentEmail.EmailAddress()) == null || theSubscribe != "P")
                group.AttachEmail(CurrentEmail, theFormat, theSubscribe);
          
            //------ The following part is for the UDF's Insert & Update.. 
            //------ Should work with the UDF History too.. but not tested.
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
                    }
                    catch { }
                }

                string GUID = System.Guid.NewGuid().ToString();

                string dfsID = null;
                for (int i = 0; i < _UDFData.Count; i++)
                {
                    dfsID = null;
                    try
                    {
                        dfsID = DataFunctions.ExecuteScalar("SELECT DataFieldSetID FROM GroupDataFields WHERE GroupDataFieldsID = " + _keyArrayList[i].ToString() + " and IsDeleted = 0").ToString();
                    }
                    catch (Exception) { }
                    if (dfsID == null || dfsID.Length == 0)
                    {
                        group.AttachUDFToEmail(CurrentEmail, _keyArrayList[i].ToString(), _UDFData[i].ToString());
                    }
                    else
                    {
                        group.AttachUDFToEmail(CurrentEmail, _keyArrayList[i].ToString(), _UDFData[i].ToString(), GUID);
                    }
                }
            }
        }
        #endregion

        #region get smartForm Details
        private void getSmartFormDetails(int SFID)
        {
            if (SFID != 0)
            {
                string smartFormSql = " SELECT g.customerID, sf.* FROM SmartFormsHistory sf join groups g on sf.groupID = g.groupID WHERE SmartFormID= " + SFID + " and sf.IsDeleted = 0";
                DataTable dt = DataFunctions.GetDataTable(smartFormSql);
                DataRow dr = dt.Rows[0];

                Response_FromEmail = dr["Response_FromEmail"].ToString();
                Response_UserMsgSubject = dr["Response_UserMsgSubject"].ToString();
                Response_UserMsgBody = dr["Response_UserMsgBody"].ToString();
                Response_UserScreen = dr["Response_UserScreen"].ToString();
                Response_AdminEmail = dr["Response_AdminEmail"].ToString();
                Response_AdminMsgSubject = dr["Response_AdminMsgSubject"].ToString();
                Response_AdminMsgBody = dr["Response_AdminMsgBody"].ToString();
                SFGroupID = Convert.ToInt32(dr["GroupID"]);
                CustomerID = Convert.ToInt32(dr["CustomerID"]);

                if (dr.IsNull("DoubleOptIn") || Convert.ToInt32(dr["DoubleOptIn"]) == 0)
                    double_opt_in_needed = false;
                else
                    double_opt_in_needed = true;

                if (dr.IsNull("SubscriptionGroupIDs"))
                    SubscriptionGroupIDs = SFGroupID.ToString();
                else
                    SubscriptionGroupIDs = dr["GroupID"].ToString() + (dr["SubscriptionGroupIDs"].ToString().Trim() == string.Empty ? "" : "," + dr["SubscriptionGroupIDs"].ToString().Trim());
            }
        }

        #endregion

        #region Send User & Admin Emails
        //User Email
        private void SendUserResponseEmails(Groups grpObject, Emails theEmailObject)
        {
            if (Response_FromEmail.Length > 5 && Response_UserMsgSubject.Trim().Length > 0 && Response_UserMsgBody.Trim().Length > 0)
            {
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                ed.CustomerID = CustomerID;
                ed.EmailAddress = EmailAddress;
                ed.EmailSubject = Response_UserMsgSubject;
                ed.FromName = "Activity Engine";
                ed.Process = "Activity Engine - SO_multigroup_subscribe.SendUserResponseEmails";
                ed.Source = "Activity Engine";
                ed.ReplyEmailAddress = Response_FromEmail;
                ed.CreatedUserID = User.UserID;
                ed.Content = ReplaceCodeSnippets(grpObject, theEmailObject, Response_UserMsgBody);
                string unsubscribeText = "<p style=\"padding-TOP:5px\"><div style=\"font-size:8.0pt;font-family:'Arial,sans-serif'; color:#666666\"><IMG style=\"POSITION:relative; TOP:5px\" src='" + ConfigurationManager.AppSettings["Image_DomainPath"] + "/images/Sure-Unsubscribe.gif'/>&nbsp;If you feel you have received this message in error, or wish to be removed, please <a href='" + ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?e=" + EmailAddress + "&g=" + grpObject.ID().ToString() + "&b=0&c=" + CustomerID + "&s=U'>Unsubscribe</a>.</div></p>";
                ed.Content += unsubscribeText;

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
        }

        //Admin Email
        private void SendAdminResponseEmails(Groups grpObject, Emails theEmailObject)
        {
            if (Response_AdminEmail.Length > 5 && Response_FromEmail.Length > 5)
            {
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                ed.CustomerID = CustomerID;
                ed.EmailAddress = Response_AdminEmail;
                ed.EmailSubject = Response_AdminMsgSubject;
                ed.FromName = "Activity Engine";
                ed.Process = "Activity Engine - SO_multigroup_subscribe.SendUserResponseEmails";
                ed.Source = "Activity Engine";
                ed.ReplyEmailAddress = Response_FromEmail;
                ed.CreatedUserID = User.UserID;
                ed.Content = ReplaceCodeSnippets(grpObject, theEmailObject, Response_AdminMsgBody);

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
            }
        }
        #endregion

        #region Replace Code Snippets to real Value - which replaces %%**%% to real data..
        private string ReplaceCodeSnippets(Groups group, Emails emailObj, string emailbody)
        {
            try
            {

                string redirpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/SO_multigroup_subscribe.aspx";
                string thelink = redirpage + "?ei=" + emailObj.ID() +
                    "&s=S" + 
                    "&f=html" +
                    "&c=" + CustomerID +
                    "&sfID=" + getSmartFormID();

                emailbody = emailbody.Replace("%%SubscribeLink%%", thelink);

                emailbody = emailbody.Replace("%%GroupID%%", group.ID().ToString());
                emailbody = emailbody.Replace("%%GroupName%%", group.Name.ToString());
                emailbody = emailbody.Replace("%%EmailID%%", emailObj.ID().ToString());
                emailbody = emailbody.Replace("%%EmailAddress%%", emailObj.EmailAddress().ToString());
                emailbody = emailbody.Replace("%%Title%%", emailObj.Title.ToString());
                emailbody = emailbody.Replace("%%FirstName%%", emailObj.FirstName.ToString());
                emailbody = emailbody.Replace("%%LastName%%", emailObj.LastName.ToString());
                emailbody = emailbody.Replace("%%FullName%%", emailObj.FullName.ToString());
                emailbody = emailbody.Replace("%%Company%%", emailObj.Company.ToString());
                emailbody = emailbody.Replace("%%Occupation%%", emailObj.Occupation.ToString());
                emailbody = emailbody.Replace("%%Address%%", emailObj.Address.ToString());
                emailbody = emailbody.Replace("%%Address2%%", emailObj.Address2.ToString());
                emailbody = emailbody.Replace("%%City%%", emailObj.City.ToString());
                emailbody = emailbody.Replace("%%State%%", emailObj.State.ToString());
                emailbody = emailbody.Replace("%%Zip%%", emailObj.Zip.ToString());
                emailbody = emailbody.Replace("%%Country%%", emailObj.Country.ToString());
                emailbody = emailbody.Replace("%%Voice%%", emailObj.Voice.ToString());
                emailbody = emailbody.Replace("%%Mobile%%", emailObj.Mobile.ToString());
                emailbody = emailbody.Replace("%%Fax%%", emailObj.Fax.ToString());
                emailbody = emailbody.Replace("%%Website%%", emailObj.Website.ToString());
                emailbody = emailbody.Replace("%%Age%%", emailObj.Age.ToString());
                emailbody = emailbody.Replace("%%Income%%", emailObj.Income.ToString());
                emailbody = emailbody.Replace("%%Gender%%", emailObj.Gender.ToString());
                emailbody = emailbody.Replace("%%Notes%%", emailObj.Notes.ToString());
                emailbody = emailbody.Replace("%%BirthDate%%", emailObj.BirthDate.ToString());
                emailbody = emailbody.Replace("%%User1%%", emailObj.User1.ToString());
                emailbody = emailbody.Replace("%%User2%%", emailObj.User2.ToString());
                emailbody = emailbody.Replace("%%User3%%", emailObj.User3.ToString());
                emailbody = emailbody.Replace("%%User4%%", emailObj.User4.ToString());
                emailbody = emailbody.Replace("%%User5%%", emailObj.User5.ToString());
                emailbody = emailbody.Replace("%%User6%%", emailObj.User6.ToString());
                emailbody = emailbody.Replace("%%UserEvent1%%", emailObj.UserEvent1.ToString());
                emailbody = emailbody.Replace("%%UserEvent1Date%%", emailObj.UserEvent1Date.ToString());
                emailbody = emailbody.Replace("%%UserEvent2%%", emailObj.UserEvent2.ToString());
                emailbody = emailbody.Replace("%%UserEvent2Date%%", emailObj.UserEvent2Date.ToString());

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
                            emailbody = emailbody.Replace("%%" + _value + "%%", UDFData);
                        }
                        catch
                        {
                            emailbody = emailbody.Replace("%%" + _value + "%%", "");
                        }
                    }
                }
                //End UDF Data
            }
            catch
            { }

            return emailbody;
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

        private static int ToInt32(string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }
    }
}
