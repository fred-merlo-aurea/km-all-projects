using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Services.Description;
using ecn.webservice.classes;
using ecn.common.classes;
using ecn.communicator.classes;
using ECN_Framework_BusinessLayer.Communicator;
using User = KMPlatform.Entity.User;

namespace ecn.webservice
{
    /// <summary>
    /// Summary description for ListImport_XML
    /// </summary>
    [WebService(
         Namespace = "http://tempuri.org/",
         Description = "The Web service provides access to Import Email profiles into a list in ECN.")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]

    public class ListImport_XML : System.Web.Services.WebService
    {

        #region Getters & Setters
        public string connStr { get { return ConfigurationSettings.AppSettings["connString"]; } }
        public string accountsDB { get { return ConfigurationSettings.AppSettings["accountsdb"]; } }
        public string commDB { get { return ConfigurationSettings.AppSettings["communicatordb"]; } }

        private const string XmlTemplate = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{0}</XML>";
        private const string ActionColumnName = "Action";
        private const string CountsColumnName = "Counts";
        private const string EmailAddressColumn = "emailaddress";
        private const string UserPrefix = "user_";
        private const string NewEmailAddressParam = "@NewEmailAddress";
        private const string OldEmailAddressParam = "@OldEmailAddress";
        private const string GroupIdParam = "@GroupID";
        private const int BatchLength = 10000;
        public string _emailProfilesXMLString = "";
        public string emailProfilesXMLString { set { _emailProfilesXMLString = value; } get { return _emailProfilesXMLString; } }

        public int _listID = 0;
        public int listID { set { _listID = value; } get { return _listID; } }

        public string _accessKey = "";
        public string accessKey { set { _accessKey = value; } get { return _accessKey; } }
        public int _baseChannelID = 0;
        public int baseChannelID { set { _baseChannelID = value; } get { return _baseChannelID; } }
        public int _customerID = 0;
        public int customerID { set { _customerID = value; } get { return _customerID; } }
        public int _userID = 0;
        public int userID { set { _userID = value; } get { return _userID; } }
        public string _customerName = "";
        public string customerName { set { _customerName = value; } get { return _customerName; } }

        public string _importError = "";
        public string importError { set { _importError = value; } get { return _importError; } }

        public string _importResults = "";
        public string importResults { set { _importResults = value; } get { return _importResults; } }

        public static ArrayList emailsColumnHeadings;
        public static Hashtable hUpdatedRecords = new Hashtable();

        #endregion

        [WebMethod(Description = "This method call will import the email profiles in the XML string.<br>Parameters to this method<br>* ecn_userName / ecn_password - is the ECN Useename & Password to login<br>* ecn_listID - is the ID of the list to import the email profiles.<br>* ecn_emailProfilesXMLString - is XML string that has the profile data to be imported.<br>* NOTE: the XML should have <u>no line breaks</u> OR <u>special characters</u>", EnableSession = false)]
        [SoapDocumentMethod(Action = "http://webservices.ecn5.com/communicator/listservices/ImportEmailsList_XML.asmx/importEmailProfiles",
            RequestNamespace = "ecn.webServices.communicator", RequestElementName = "ImportEmailProfilesRequest",
            ResponseNamespace = "ecn.webServices.communicator", ResponseElementName = "Response",
            Use = SoapBindingUse.Default)]

        public string ImportEmailProfiles(string ecn_accessKey, int ecn_listID, string ecn_emailProfilesXMLString)   
        {
            accessKey = ecn_accessKey;
            listID = ecn_listID;
            emailProfilesXMLString = ecn_emailProfilesXMLString;

            if (authenticateUser())
            {
                if (isListAuthorized())
                {
                    if (emailProfilesXMLString.Length > 0)
                    {
                        extractCoumnNamesFromEmailsTable();
                        DataTable xmlDT = extractColumnNamesFromXMLString();
                        if (!(xmlDT == null))
                        {
                            bool importSuccess = importData(xmlDT, ecn_accessKey);
                            if (importSuccess)
                            {
                                return SendResponse.response("ImportEmailProfiles", "0", 0, importResults);
                            }
                            else
                            {
                                return SendResponse.response("ImportEmailProfiles", "1", 0, importError);
                            }
                        }
                        else
                        {
                            return SendResponse.response("ImportEmailProfiles", "1", 0, "INVALID XML STRING");
                        }
                    }
                    else
                    {
                        return SendResponse.response("ImportEmailProfiles", "1", 0, "INVALID XML STRING");
                    }
                }
                else
                {
                    return SendResponse.response("ImportEmailProfiles", "1", 0, "UNAUTHORIZED ACCESS TO LIST");
                }
            }
            else
            {
                return SendResponse.response("ImportEmailProfiles", "1", 0, "LOGIN AUTHENTICATION FAILED");
            }
        }

        [WebMethod(Description = "This method call will import the email profiles in the XML string.<br>Parameters to this method<br>* ecn_userName / ecn_password - is the ECN Useename & Password to login<br>* ecn_listID - is the ID of the list to import the email profiles.<br>* ecn_emailProfilesXMLString - is XML string that has the profile data to be imported.<br>* NOTE: the XML should have <u>no line breaks</u> OR <u>special characters</u>", EnableSession = false)]
        [SoapDocumentMethod(Action = "http://webservices.ecn5.com/communicator/listservices/ImportEmailsList_XML.asmx/importEmailProfilesSF",
            RequestNamespace = "ecn.webServices.communicator", RequestElementName = "ImportEmailProfilesRequestSF",
            ResponseNamespace = "ecn.webServices.communicator", ResponseElementName = "ResponseSF",
            Use = SoapBindingUse.Default)]
        public string ImportEmailProfilesSF(string ecn_accessKey, int ecn_listID, string ecn_emailProfilesXMLString, int sfID)    
        {
            accessKey = ecn_accessKey;
            listID = ecn_listID;
            emailProfilesXMLString = ecn_emailProfilesXMLString;

            if (authenticateUser())
            {
                if (isListAuthorized())
                {
                    if (emailProfilesXMLString.Length > 0)
                    {
                        extractCoumnNamesFromEmailsTable();
                        DataTable xmlDT = extractColumnNamesFromXMLString();
                        if (!(xmlDT == null))
                        {
                            bool importSuccess = importData(xmlDT, ecn_accessKey);

                            if (importSuccess)
                            {
                                SendEmailFromSF(sfID, xmlDT.Rows[0], ecn_listID);
                                return SendResponse.response("ImportEmailProfiles", "0", 0, importResults);
                            }
                            else
                            {
                                return SendResponse.response("ImportEmailProfiles", "1", 0, importError);
                            }
                        }
                        else
                        {
                            return SendResponse.response("ImportEmailProfiles", "1", 0, "INVALID XML STRING");
                        }
                    }
                    else
                    {
                        return SendResponse.response("ImportEmailProfiles", "1", 0, "INVALID XML STRING");
                    }
                }
                else
                {
                    return SendResponse.response("ImportEmailProfiles", "1", 0, "UNAUTHORIZED ACCESS TO LIST");
                }
            }
            else
            {
                return SendResponse.response("ImportEmailProfiles", "1", 0, "LOGIN AUTHENTICATION FAILED");
            }
        }


        [WebMethod(Description = "This method call will import the email profiles in the XML string and alos updates the emailaddress.<br>Parameters to this method<br>* ecn_userName / ecn_password - is the ECN Useename & Password to login<br>* ecn_listID - is the ID of the list to import the email profiles.<br>* ecn_emailProfilesXMLString - is XML string that has the profile data to be imported.<br>* NOTE: the XML should have <u>no line breaks</u> OR <u>special characters</u>", EnableSession = false)]
        [SoapDocumentMethod(Action = "http://www.ecn5.com/ecn.webservice/ListImport_XML.asmx?op=ImportEmailProfilesSFUpdate",   
            RequestNamespace = "ecn.webServices.communicator", RequestElementName = "ImportEmailProfilesRequestSFUpdate",
            ResponseNamespace = "ecn.webServices.communicator", ResponseElementName = "ResponseSFUpdate",
            Use = SoapBindingUse.Default)]
        public string UpdateEmailAddress(string ecn_accessKey, int ecn_listID, string ecn_emailProfilesXMLString, string oldEmailAddress, string newEmailAddress, int sfID)
        {
            accessKey = ecn_accessKey;
            listID = ecn_listID;
            emailProfilesXMLString = ecn_emailProfilesXMLString;

            if (authenticateUser())
            {
                if (isListAuthorized())
                {
                    if (emailProfilesXMLString.Length > 0)
                    {
                        extractCoumnNamesFromEmailsTable();
                        DataTable xmlDT = extractColumnNamesFromXMLString();
                        if (!(xmlDT == null))
                        {
                            bool importSuccess = importDataWithUpdate(xmlDT, oldEmailAddress, newEmailAddress, ecn_accessKey);        

                            if (importSuccess)
                            {
                                SendEmailFromSF(sfID, xmlDT.Rows[0], ecn_listID);
                                return SendResponse.response("ImportEmailProfiles", "0", 0, importResults);
                            }
                            else
                            {
                                return SendResponse.response("ImportEmailProfiles", "1", 0, importError);
                            }
                        }
                        else
                        {
                            return SendResponse.response("ImportEmailProfiles", "1", 0, "INVALID XML STRING");
                        }
                    }
                    else
                    {
                        return SendResponse.response("ImportEmailProfiles", "1", 0, "INVALID XML STRING");
                    }
                }
                else
                {
                    return SendResponse.response("ImportEmailProfiles", "1", 0, "UNAUTHORIZED ACCESS TO LIST");
                }
            }
            else
            {
                return SendResponse.response("ImportEmailProfiles", "1", 0, "LOGIN AUTHENTICATION FAILED");
            }
        }

        public bool importDataWithUpdate(
            DataTable dtFile,
            string oldEmailAddres,
            string newEmailAddress,
            string accessKey)
        {
            var xmlUdf = new StringBuilder();
            var xmlProfile = new StringBuilder();
            var ecnUser = KMPlatform.BusinessLogic.User.ECN_GetByAccessKey(accessKey);
            var startDateTime = DateTime.Now;
            try
            {
                var hGdfFields = getUDFsForList(listID);

                for (var dtFileIndex= 0; dtFileIndex < dtFile.Rows.Count; dtFileIndex++)
                {
                    AppendXmlProfileWithEmailRow(dtFile, dtFileIndex, xmlProfile, hGdfFields, xmlUdf);

                    if ((dtFileIndex == 0 || dtFileIndex % BatchLength != 0) && dtFileIndex != dtFile.Rows.Count - 1)
                    {
                        continue;
                    }

                    ExecuteEmailUpdateChunk(oldEmailAddres, newEmailAddress, ecnUser, xmlProfile, xmlUdf);

                    xmlProfile = new StringBuilder();
                    xmlUdf = new StringBuilder();
                }

                if (hUpdatedRecords.Count > 0)
                {
                    UpdateImportResultsLog(startDateTime);
                }

                return true;
            }
            catch (Exception ex)
            {
                importError = ex.ToString();
                return false;
            }
        }

        private void UpdateImportResultsLog(DateTime startDateTime)
        {
            var logBuilder = new StringBuilder();
            foreach (DictionaryEntry dictionaryEntry in hUpdatedRecords)
            {
                switch (dictionaryEntry.Key.ToString())
                {
                    case "T":
                        logBuilder.Append($"<TotalRecords>{dictionaryEntry.Value}</TotalRecords>");
                        break;
                    case "I":
                        logBuilder.Append($"<New>{dictionaryEntry.Value}</New>");
                        break;
                    case "U":
                        logBuilder.Append($"<Changed>{dictionaryEntry.Value}</Changed>");
                        break;
                    case "D":
                        logBuilder.Append($"<Duplicates>{dictionaryEntry.Value}</Duplicates>");
                        break;
                    case "S":
                        logBuilder.Append($"<Skipped>{dictionaryEntry.Value}</Skipped>");
                        break;
                    case "M":
                        logBuilder.Append($"<MSSkipped>{dictionaryEntry.Value}</MSSkipped>");
                        break;
                }
            }

            var duration = DateTime.Now - startDateTime;
            logBuilder.Append($"<ImportTime>{duration.Hours}:{duration.Minutes}:{duration.Seconds}</ImportTime>");
            importResults = logBuilder.ToString();
        }

        private void ExecuteEmailUpdateChunk(
            string oldEmailAddres,
            string newEmailAddress,
            User ecnUser,
            StringBuilder xmlProfile,
            StringBuilder xmlUdf)
        {
            using (var cmdUpdate = new SqlCommand("sp_UpdateEmailAddress")
            {
                CommandTimeout = 0,
                CommandType = CommandType.StoredProcedure
            })
            {
                cmdUpdate.Parameters.Add(NewEmailAddressParam, SqlDbType.VarChar);
                cmdUpdate.Parameters[NewEmailAddressParam].Value = newEmailAddress;

                cmdUpdate.Parameters.Add(OldEmailAddressParam, SqlDbType.VarChar);
                cmdUpdate.Parameters[OldEmailAddressParam].Value = oldEmailAddres;

                cmdUpdate.Parameters.Add(GroupIdParam, SqlDbType.Int);
                cmdUpdate.Parameters[GroupIdParam].Value = listID;

                DataFunctions.Execute("communicator", cmdUpdate);
            }

            var dtRecords = EmailGroup.ImportEmails(ecnUser,
                customerID,
                listID,
                string.Format(XmlTemplate, xmlProfile),
                string.Format(XmlTemplate, xmlUdf),
                "HTML",
                "S",
                false,
                string.Empty,
                "Ecn.webservice.listImport_XML.importDataWithUpdate");


            if (dtRecords.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRecords.Rows)
                {
                    if (!hUpdatedRecords.Contains(dr[ActionColumnName].ToString()))
                    {
                        hUpdatedRecords.Add(
                            dr[ActionColumnName].ToString().ToUpper(),
                            Convert.ToInt32(dr[CountsColumnName]));
                    }
                    else
                    {
                        var eTotal = Convert.ToInt32(hUpdatedRecords[dr[ActionColumnName].ToString().ToUpper()]);
                        hUpdatedRecords[dr[ActionColumnName].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr[CountsColumnName]);
                    }
                }
            }
        }

        private void AppendXmlProfileWithEmailRow(
            DataTable dtFile,
            int cnt,
            StringBuilder xmlProfile,
            Hashtable hGdfFields,
            StringBuilder xmlUdf)
        {
            var drFile = dtFile.Rows[cnt];
            var bRowCreated = false;
            xmlProfile.Append("<Emails>");

            foreach (DataColumn dcFile in dtFile.Columns)
            {
                var cleanColumnValue = cleanXMLString(drFile[dcFile.ColumnName].ToString());
                if (dcFile.ColumnName.IndexOf(UserPrefix, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    var columnName = dcFile.ColumnName.ToLower();
                    xmlProfile.Append($"<{columnName}>{cleanColumnValue}</{columnName}>");
                }

                if (hGdfFields.Count > 0 &&
                    dcFile.ColumnName.IndexOf(UserPrefix, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    if (!bRowCreated)
                    {
                        xmlUdf.Append("<row>");
                        xmlUdf.Append($"<ea>{cleanXMLString(drFile[EmailAddressColumn].ToString())}</ea>");
                        bRowCreated = true;
                    }

                    xmlUdf.Append($"<udf id=\"{hGdfFields[dcFile.ColumnName.ToLower()]}\">");
                    xmlUdf.Append($"<v>{cleanColumnValue}</v>");
                    xmlUdf.Append("</udf>");
                }
            }

            xmlProfile.Append("</Emails>");

            if (bRowCreated)
            {
                xmlUdf.Append("</row>");
            }
        }

        private void SendEmailFromSF(int sfID, DataRow xmlDR, int groupID)
        {
            if (sfID > 0)
            {
                string emailAddress = xmlDR["EmailAddress"].ToString();
                int emailID = Convert.ToInt32(SQLHelper.executeScalar("Select EmailID from Emails where EmailAddress = '" + emailAddress + "' AND CustomerID = " + customerID.ToString(), ConfigurationManager.AppSettings["com"]));

                if (emailID > 0)
                {
                    Emails emailObj = Emails.GetEmailByID(emailID);
                    Groups group = new Groups(groupID);
                    ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();

                    ed.CustomerID = customerID;
                    ed.EmailAddress = emailAddress;
                    ed.FromName = "Webservice";
                    ed.Process = "Webservice - ListImport_XML.SendEmailFromSF";
                    ed.Source = "Webservice";
                    ed.ReplyEmailAddress = "emaildirect@ecn5.com";
                    ed.SendTime = DateTime.Now;
                    ed.CreatedUserID = userID;

                    string sql = "SELECT * from SmartFormsHistory where SmartFormID = " + sfID;
                    DataTable dt = SQLHelper.getDataTable(sql, ConfigurationManager.AppSettings["com"]);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        ed.ReplyEmailAddress = dr.IsNull("Response_FromEmail") ? "" : dr["Response_FromEmail"].ToString().Trim();

                        /* Send Response Email to user*/

                        ed.EmailSubject = dr.IsNull("Response_UserMsgSubject") ? "" : dr["Response_UserMsgSubject"].ToString().Trim();
                        ed.Content = dr.IsNull("Response_UserMsgBody") ? "" : dr["Response_UserMsgBody"].ToString().Trim();
                        ed.Content = ReplaceCodeSnippets(group, emailObj, ed.Content, xmlDR);

                        if (ed.ReplyEmailAddress.Length > 5 && ed.EmailAddress.Length > 5)
                        {
                            ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
                        }




                        /* Send Response Email to Admin*/

                        ed.EmailAddress = dr.IsNull("Response_AdminEmail") ? "" : dr["Response_AdminEmail"].ToString().Trim();
                        ed.EmailSubject = dr.IsNull("Response_AdminMsgSubject") ? "" : dr["Response_AdminMsgSubject"].ToString().Trim();
                        ed.Content = dr.IsNull("Response_AdminMsgBody") ? "" : dr["Response_AdminMsgBody"].ToString().Trim();
                        ed.Content = ReplaceCodeSnippets(group, emailObj, ed.Content, xmlDR);

                        if (ed.ReplyEmailAddress.Length > 5 && ed.EmailAddress.Length > 5)
                        {
                            ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
                        }
                    }
                }
            }
        }        

        
        private string ReplaceCodeSnippets(Groups group, Emails emailObj, string emailbody, DataRow ECNPostParams)
        {
            emailbody = emailbody.Replace("%%GroupID%%", group.ID().ToString());
            emailbody = emailbody.Replace("%%GroupName%%", group.Name.ToString());
            emailbody = emailbody.Replace("%%EmailAddress%%", emailObj.Email.ToString());
            emailbody = emailbody.Replace("%%FirstName%%", emailObj.FirstName.ToString());
            emailbody = emailbody.Replace("%%LastName%%", emailObj.LastName.ToString());
            emailbody = emailbody.Replace("%%FullName%%", emailObj.FullName.ToString());
            emailbody = emailbody.Replace("%%Address%%", emailObj.Address.ToString());
            emailbody = emailbody.Replace("%%Address2%%", emailObj.Address2.ToString());
            emailbody = emailbody.Replace("%%City%%", emailObj.City.ToString());
            emailbody = emailbody.Replace("%%State%%", emailObj.State.ToString());
            emailbody = emailbody.Replace("%%Zip%%", emailObj.Zip.ToString());
            emailbody = emailbody.Replace("%%Voice%%", emailObj.Voice.ToString());
            emailbody = emailbody.Replace("%%User1%%", emailObj.User1.ToString());
            emailbody = emailbody.Replace("%%User2%%", emailObj.User2.ToString());

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
                    string _value = UserPrefix + UDFHashEnumerator.Value.ToString();
                    string _key = UDFHashEnumerator.Key.ToString();
                    try
                    {
                        UDFData = Convert.ToString(ECNPostParams[_value]);
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

            return emailbody;
        }   

        #region Authenticate AccessID
        private bool authenticateUser()
        {
            try
            {
                string sql = " SELECT c.BaseChannelID, u.CustomerID, c.CustomerName, u.UserID " +
                    " FROM " + accountsDB + ".dbo.Users u JOIN " + accountsDB + ".dbo.Customer c ON u.CustomerID = c.CustomerID " +
                    " WHERE u.AccessKey = '" + accessKey + "'";
                try
                {
                    DataTable dt = SQLHelper.getDataTable(sql, ConfigurationManager.AppSettings["com"]);
                    baseChannelID = Convert.ToInt32(dt.Rows[0]["BaseChannelID"].ToString());
                    customerID = Convert.ToInt32(dt.Rows[0]["CustomerID"].ToString());
                    customerName = dt.Rows[0]["CustomerName"].ToString();
                    userID = Convert.ToInt32(dt.Rows[0]["UserID"].ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Authorize ListID to Customer account
        private bool isListAuthorized()
        {
            try
            {
                int countList = Convert.ToInt32(SQLHelper.executeScalar(" SELECT COUNT(GroupID) FROM Groups WHERE GroupID = " + listID + " AND CustomerID = " + customerID, ConfigurationManager.AppSettings["com"]).ToString());
                if (countList > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region extract Column Names from From EmailsTable
        //extract Column Names from From XLSFile / Table
        private void extractCoumnNamesFromEmailsTable()
        {
            DataTable dataTable = SQLHelper.getDataTable("select TOP 1 * from Emails", ConfigurationManager.AppSettings["com"]);
            emailsColumnHeadings = DataFunctions.GetDataTableColumns(dataTable);

            for (int i = 0; i < emailsColumnHeadings.Count; i++)
            {
                emailsColumnHeadings[i] = emailsColumnHeadings[i].ToString().ToLower();
            }
        }
        #endregion

        #region extract Data From XML File and Import in to ECN
        private DataTable extractColumnNamesFromXMLString()
        {
            string xmlFilePath = ConfigurationManager.AppSettings["XMLPath"].ToString() + "_" + customerID + "_" + listID + "_" + DateTime.Now.ToString("MMddyyyy_hhmmss_ffff") + "_" + System.Guid.NewGuid().ToString().Substring(1, 5) + ".xml";

            try
            {
                StreamWriter file = new StreamWriter(xmlFilePath);  
                file.Write(emailProfilesXMLString);
                file.Flush();
                file.Close();
            }
            catch { }

            DataSet dataset = new DataSet();
            dataset.ReadXml(xmlFilePath);
            DataTable dt = dataset.Tables[0];
            File.Delete(xmlFilePath);

            return dt;
        }
        #endregion     

        #region Import Data from Dataset
        public bool importData(DataTable dtFile, string accessKey)
        {
            StringBuilder xmlUDF = new StringBuilder("");
            StringBuilder xmlProfile = new StringBuilder("");
            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.ECN_GetByAccessKey(accessKey, false);
            DateTime startDateTime = DateTime.Now;
            try
            {
                Hashtable hGDFFields = getUDFsForList(listID);

                bool bRowCreated = false;
                for (int cnt = 0; cnt < dtFile.Rows.Count; cnt++)
                {

                    DataRow drFile = dtFile.Rows[cnt];
                    bRowCreated = false;
                    xmlProfile.Append("<Emails>");

                    foreach (DataColumn dcFile in dtFile.Columns)
                    {
                        if (dcFile.ColumnName.ToLower().IndexOf(UserPrefix) == -1)
                        {
                            xmlProfile.Append("<" + dcFile.ColumnName.ToLower() + ">" + cleanXMLString(drFile[dcFile.ColumnName].ToString()) + "</" + dcFile.ColumnName.ToLower() + ">");
                        }

                        if (hGDFFields.Count > 0)
                        {
                            if (dcFile.ColumnName.ToLower().IndexOf(UserPrefix) > -1)
                            {
                                if (!bRowCreated)
                                {
                                    xmlUDF.Append("<row>");
                                    xmlUDF.Append("<ea>" + cleanXMLString(drFile["emailaddress"].ToString()) + "</ea>");
                                    bRowCreated = true;
                                }

                                xmlUDF.Append("<udf id=\"" + hGDFFields[dcFile.ColumnName.ToLower()].ToString() + "\">");
                                xmlUDF.Append("<v>" + cleanXMLString(drFile[dcFile.ColumnName.ToLower()].ToString()) + "</v>");
                                xmlUDF.Append("</udf>");
                            }
                        }
                    }
                    xmlProfile.Append("</Emails>");

                    if (bRowCreated)
                        xmlUDF.Append("</row>");

                    if ((cnt != 0) && (cnt % 10000 == 0) || (cnt == dtFile.Rows.Count - 1))
                    {


                        DataTable dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(user, customerID, listID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", "HTML", "S", false, "", "Ecn.webservice.listImport_XML.importData");


                        if (dtRecords.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRecords.Rows)
                            {
                                if (!hUpdatedRecords.Contains(dr[ActionColumnName].ToString()))
                                    hUpdatedRecords.Add(dr[ActionColumnName].ToString().ToUpper(), Convert.ToInt32(dr[CountsColumnName]));
                                else
                                {
                                    int eTotal = Convert.ToInt32(hUpdatedRecords[dr[ActionColumnName].ToString().ToUpper()]);
                                    hUpdatedRecords[dr[ActionColumnName].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr[CountsColumnName]);
                                }
                            }
                        }


                        xmlProfile = new StringBuilder("");
                        xmlUDF = new StringBuilder("");
                    }
                }
                hGDFFields.Clear();

                if (hUpdatedRecords.Count > 0)
                {
                    importResults = "";
                    foreach (DictionaryEntry de in hUpdatedRecords)
                    {
                        if (de.Key.ToString() == "T")
                            importResults += "<TotalRecords>" + de.Value.ToString() + "</TotalRecords>";
                        else if (de.Key.ToString() == "I")
                            importResults += "<New>" + de.Value.ToString() + "</New>";
                        else if (de.Key.ToString() == "U")
                            importResults += "<Changed>" + de.Value.ToString() + "</Changed>";
                        else if (de.Key.ToString() == "D")
                            importResults += "<Duplicates>" + de.Value.ToString() + "</Duplicates>";
                        else if (de.Key.ToString() == "S")
                            importResults += "<Skipped>" + de.Value.ToString() + "</Skipped>";
                        else if (de.Key.ToString() == "M")
                        {
                            importResults += "<MSSkipped>" + de.Value.ToString() + "</MSSkipped>";
                        }
                    }

                    TimeSpan duration = DateTime.Now - startDateTime;
                    importResults += "<ImportTime>" + duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds + "</ImportTime>";
                }

                return true;
            }
            catch (Exception ex)
            {
                importError = ex.ToString();
                return false;
            }
        }
        #endregion

        #region Other Methods
        private Hashtable getUDFsForList(int groupId)
        {
            var emailstable = SQLHelper.getDataTable(
                $" SELECT * FROM GroupDatafields WHERE GroupID={groupId}",
                ConfigurationManager.AppSettings["com"]);

            var fields = new Hashtable();
            foreach (DataRow dataRow in emailstable.Rows)
            {
                fields.Add(
                    $"{UserPrefix}{dataRow["ShortName"].ToString().ToLower()}",
                    Convert.ToInt32(dataRow["GroupDataFieldsID"]));
            }

            return fields;
        }

        private string cleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }
        #endregion

    }
}
