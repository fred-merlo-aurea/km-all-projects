using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlTypes;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.Diagnostics;
using System.Net.Mail;
using System.Security;
using System.Text.RegularExpressions;
using ServiceStack;
using Group = ECN_Framework_Entities.Communicator.Group;

namespace ECNTools.ECN_Import
{
    public partial class FPImportNightly : Form
    {
        public FPImportNightly()
        {
            Main2.user = new KMPlatform.BusinessLogic.User().LogIn(Guid.Parse(ConfigurationManager.AppSettings["ecnAccessKey"].ToString()), false);
            InitializeComponent();
            LoadListBox();
        }

        public static readonly int[] NewSubscriptionXACTs = { 10, 13, 14 };
        public static readonly int[] NonNewSubscritionXACTs = {16, 17, 18, 19, 21, 22, 23, 25, 27, 31, 32, 33, 34, 38, 39,
            40, 41, 42, 43, 46, 47, 48, 49, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 99};
        private const string ConfigLineNumberToStart = "LinenumberToStart";
        private const string ConfigConnectionString = "ECNCommunicator";
        private const string PubCode = "PUBCODE";
        private const string Email = "EMAIL";
        private const string Sequence = "SEQUENCE";
        private const string EmailId = "EMAILID";
        private const string XACT = "XACT";
        private const string UnrecognizedXACT = "UNRECOGNIZED XACT Code";
        private const string NoMatch = "NO MATCH on EMAIL / SUBSCRIBERID";
        private const string NonExistingEmail = "NON EXISTING EMAIL / SUBID - NEED TO BE INSERTED";
        private const string BodyFormat = "<P>PUBLICATION: {0}<BR>" +
                "RECORD #: {1}<BR>" +
                "EMAILADDRESS: {2}<BR>" +
                "XACT: {3}<BR>" +
                "SEQUENCE ID: {4}<BR></P>";
        private const string Zip = "ZIP";
        private const string Plus4 = "PLUS4";
        private const string Country = "COUNTRY";
        private const string Ca = "ca";
        private const string Canada = "canada";
        private const string FirstName = "FNAME";
        private const string LastName = "LNAME";
        private const string EmailsOpenTag = "<Emails>";
        private const string Company = "COMPANY";
        private const string Title = "TITLE";
        private const string City = "CITY";
        private const string State = "STATE";
        private const string Phone = "PHONE";
        private const string Fax = "FAX";
        private const string Website = "WEBSITE";
        private const string Address = "ADDRESS";
        private const string MailStop = "MAILSTOP";
        private const string SubscribeId = "SUBSCRIBERID";
        private const string PublicationCode = "PUBLICATIONCODE";
        private const string AlternateEmailAddress = "ALTERNATE_EMAILADDRESS";
        private static string Log = string.Empty;
        private static StreamWriter Logfile = null;
        private static DataTable DBIVDataTable = null;
        private static string Pubs = string.Empty;

        private static ECN_Framework_Entities.Communicator.Email _newEmailObj = null;
        public static ECN_Framework_Entities.Communicator.Email newEmailObj
        {
            get { return _newEmailObj; }
            set { _newEmailObj = value; }
        }

        private static bool _ecnEmailID_SubIDMatch = false;
        public static bool ECNEmailID_SubIDMatch
        {
            get { return _ecnEmailID_SubIDMatch; }
            set { _ecnEmailID_SubIDMatch = value; }
        }

        private static string _ecnEmailID = "";
        public static string ECNEmailID
        {
            get { return _ecnEmailID.ToString(); }
            set { _ecnEmailID = value; }
        }

        private static string _ecnSubscriberID = "";
        public static string ECNSubscriberID
        {
            get { return _ecnSubscriberID.ToString(); }
            set { _ecnSubscriberID = value; }
        }

        private void LoadListBox()
        {
            comboBox1.Items.Clear();
            string selectPubList = " SELECT FPDataImporterID, GroupID, PublicationName, LastDownloadDate " +
                                            " FROM FPDataImporter order by PublicationName ";// +
            //" WHERE PublicationName = " + ConfigurationManager.AppSettings["UpdatePubName"].ToString();
            //" WHERE DownloadFlag = 'Y' ";
            DataTable selectedPubsDT = ECN_Framework_DataLayer.DataFunctions.GetDataTable(selectPubList, "KMPSInterim");

            //This is the Master Loop for Downloading Files & importing in to ECN.
            comboBox1.Items.Add("");
            foreach (DataRow row in selectedPubsDT.Rows)
            {
                comboBox1.Items.Add(row["PublicationName"].ToString());
            }
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void ImportDataToDB()
        {
            //Put code here
            DateTime dtStartTime = System.DateTime.Now;
            string filePath = Path.GetDirectoryName(openFileDialog1.FileName) + "\\";
            string file = Path.GetFileName(txtDBFFile.Text);
            int count = 0;
            bool bcancel = false;

            string mm_dd_yyyy = DateTime.Now.ToString("MM-dd-yyyy");

            //Create the Log file 
            Log = filePath + file.ToUpper().Replace(".DBF", "_" + mm_dd_yyyy) + ".log";
            Logfile = new StreamWriter(new FileStream(Log, System.IO.FileMode.Append));
            WriteToLog("");
            WriteToLog("");
            WriteToLog("-START--------------------------------------");

            //wgh - do all pubs
            //get all list of Publications that will be downloaded Today.           

            string selectPubList = " SELECT FPDataImporterID, GroupID, PublicationName, LastDownloadDate " +
                                            " FROM FPDataImporter " +
                                            " WHERE PublicationName = " + Pubs;
            //" WHERE DownloadFlag = 'Y' ";
            DataTable selectedPubsDT = ECN_Framework_DataLayer.DataFunctions.GetDataTable(selectPubList, "KMPSInterim");

            //This is the Master Loop for Downloading Files & importing in to ECN.
            foreach (DataRow row in selectedPubsDT.Rows)
            {
                string ImporterID = row["FPDataImporterID"].ToString();
                string GroupID = row["GroupID"].ToString();
                string Publication = row["PublicationName"].ToString();
                string LastDownloadDate = row["LastDownloadDate"].ToString();

                WriteToLog("#### START DOWNLOADING DATA FOR " + Publication + " [GROUPID: " + GroupID + "] ####");

                DBIVDataTable = CreateDataTableFromDBF(filePath, file);
                WriteToLog("* ColumnNames extraced from File");
                ExtractDataFromFileAndImport(DBIVDataTable, GroupID, LastDownloadDate);

                //Update this Row as Completed & update the LastDownloadDate to "Today"
                try
                {
                    SqlConnection kmpsConn = new SqlConnection(ConfigurationManager.AppSettings["KMPS_ConnString"].ToString());
                    SqlCommand updCmd = new SqlCommand(null, kmpsConn);
                    updCmd.CommandText = " UPDATE FPDataImporter " +
                        " SET DownloadFlag = 'N', LastDownloadDate = CONVERT(VARCHAR, getDate(), 101) " +
                        " WHERE FPDataImporterID = @ImporterID ";
                    updCmd.Parameters.Add("@ImporterID", SqlDbType.Int, 4).Value = Convert.ToInt32(ImporterID.ToString());

                    kmpsConn.Open();
                    updCmd.ExecuteNonQuery();
                    kmpsConn.Close();
                }
                catch (Exception ex)
                {
                    WriteToLog("* ERROR: Unable to Update " + Publication + " Record on FPDataImporter DB Table ");
                }
                WriteToLog("#### END DOWNLOADING DATA FOR " + Publication + " ####");
                WriteToLog(" ");
            }


            //end wgh

            WriteToLog("-END----------------------------------------");
            Logfile.Close();
        }

        private void ExtractDataFromFileAndImport(DataTable dBaseDataTable, string groupID, string lastDownloadDate)
        {
            WriteToLog("Ready to Import data in to ECN: ");
            var sqldatenull = SqlDateTime.Null;
            var startTime = DateTime.Now;
            int lineNumber;
            int.TryParse(ConfigurationManager.AppSettings[ConfigLineNumberToStart], out lineNumber);

            try
            {
                //Add Code to Filter based on the LastDownloadDate. Check for XACTDATE between lastDownloadDate & TODAY
                var selectDBIVDataTableRows = dBaseDataTable.Select();
                var ecnConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigConnectionString].ConnectionString);
                int groupId;
                int.TryParse(groupID, out groupId);
                var pubGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupId);
                var custID = pubGroup.CustomerID;
                var dbLength = selectDBIVDataTableRows.Length;
                
                for (var i = 0; i < selectDBIVDataTableRows.Length; i++)
                {
                    var progress = ((double)i / dbLength) * 100;
                    if (backgroundWorker1.CancellationPending)
                    {
                        backgroundWorker1.ReportProgress(100, " Exiting..");
                        return;
                    }

                    backgroundWorker1.ReportProgress((int)progress, $"Importing row {i} of {selectDBIVDataTableRows.Length}({DateTime.Now.Subtract(startTime).TotalSeconds.ToString("F2")} secs)");
                    startTime = DateTime.Now;

                    if ((i + 1) > lineNumber)
                    {
                        WriteToLog($"--- RECORD {(i + 1)} START ---");
                        var rowPubCode = selectDBIVDataTableRows[i][PubCode].ToString().Trim();
                        var rowEmailAddress = selectDBIVDataTableRows[i][Email].ToString().Trim();
                        var rowSubscriberId = selectDBIVDataTableRows[i][Sequence].ToString().Trim();
                        int rowEmailId;
                        int.TryParse(selectDBIVDataTableRows[i][EmailId].ToString().Trim(), out rowEmailId);
                        int rowXACT;
                        int.TryParse(selectDBIVDataTableRows[i][XACT].ToString(), out rowXACT);

                        if (rowEmailAddress.Length < 5)
                        {
                            rowEmailAddress = getDummyEmailAddress(rowPubCode);
                            WriteToLog($"Getting DUMMY EmailAddress: {rowEmailAddress}");
                        }

                        newEmailObj = null;
                        if (rowEmailId != 0)
                        {
                            newEmailObj = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailIDGroupID_NoAccessCheck(rowEmailId, pubGroup.GroupID);
                        }

                        //Check for EmailID & get SubscriberID depending on that.
                        CheckEmailAndSubscriberID(rowEmailId.ToString(), rowSubscriberId, groupId.ToString());
                        WriteToLog($"Match on Email / SubID ? : {ECNEmailID_SubIDMatch}");

                        if (NewSubscriptionXACTs.Contains(rowXACT))
                        {
                            ProcessNewSubscriptionXACT(custID, selectDBIVDataTableRows[i], pubGroup, i, rowPubCode, rowEmailAddress, rowEmailId, rowXACT);
                        }
                        else if (NonNewSubscritionXACTs.Contains(rowXACT))
                        {
                            ProcessNonNewSubscriptionXACT(custID, selectDBIVDataTableRows[i], pubGroup, i, rowPubCode, rowEmailAddress, rowXACT);
                        }
                        else
                        {
                            NotifyAdminUnrecognizedXACT(UnrecognizedXACT, selectDBIVDataTableRows[i], i, rowPubCode, rowEmailAddress, rowXACT);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                backgroundWorker1.ReportProgress(100, "***Error occured");
                WriteToLog("* [ERROR]:Error occured while fetching data from the File" + ex);
            }
        }

        private void ProcessNewSubscriptionXACT(int custID, DataRow row, ECN_Framework_Entities.Communicator.Group pubGroup, int rowIndex, string rowPubCode, string rowEmailAddress, int rowEmailId, int rowXACT)
        {
            WriteToLog($"XACTCODE = {rowXACT}. NEW SUBSCRIPTION");
            var rowAltEMailAddress = string.Empty;

            //Check if there was a match on EmailID / SubID in ECN.
            if (ECNEmailID_SubIDMatch)
            {
                //YES MATCH. 
                //There is atleast a Sub / EmailID Match for sure.
                //Check to see if the EmailAddress Entered exists in a the Group.
                if (newEmailObj == null)
                {
                    // Do an Update 'cos we have the EmailID already
                    ReportNotExistRow(custID, row, pubGroup, rowIndex, rowPubCode, rowEmailAddress, rowAltEMailAddress);
                }
                else
                {
                    //There is a record of this EmailAddress.. Might be a different record / the EmailAddress has not been Changed / updated.
                    //Check to see if the EmailID's are the same !! If they are then its just an Update on other information. 
                    if (newEmailObj.EmailID == rowEmailId)
                    {
                        UpdateRow(custID, row, pubGroup, rowIndex, rowPubCode, rowEmailAddress, rowAltEMailAddress);
                    }
                    else
                    {
                        // Here the EmailID for the newEmailObj belongs to some one else. 
                        // so Let it go thru by using a Dummy Email Address & store the Duplicate emailAddress entered in the 
                        // user_ALTERNATE_EMAILADDRESS UDF
                        rowEmailAddress = getDummyEmailAddress(rowPubCode);
                        rowAltEMailAddress = rowEmailAddress;
                        InsertRow(custID, row, pubGroup, rowIndex, rowPubCode, rowEmailAddress, rowXACT, rowAltEMailAddress);
                    }
                }
            }
            else
            {
                //There is no Match on either SubID / EmailID so its an Insert.. 
                if (newEmailObj == null)
                {
                    // Do an INSERT 'cos we DON'T have the EmailID / SUBID
                    InsertRow(custID, row, pubGroup, rowIndex, rowPubCode, rowEmailAddress, rowXACT, rowAltEMailAddress);
                }
                else
                {
                    // There is a record of this EmailAddress.. Might be a different record / the EmailAddress has not been Changed / updated.
                    // Here the EmailID for the newEmailObj belongs to some one else. 
                    // so Let it go thru by using a Dummy Email Address & store the Duplicate emailAddress entered in the 
                    // user_ALTERNATE_EMAILADDRESS UDF
                    rowEmailAddress = getDummyEmailAddress(rowPubCode);
                    rowAltEMailAddress = rowEmailAddress;
                    InsertRow(custID, row, pubGroup, rowIndex, rowPubCode, rowEmailAddress, rowXACT, rowAltEMailAddress);
                }
            }
        }

        private void ProcessNonNewSubscriptionXACT(int custID, DataRow row, ECN_Framework_Entities.Communicator.Group pubGroup, int rowIndex, string rowPubCode, string rowEmailAddress, int rowXACT)
        {
            WriteToLog($"XACTCODE = {rowXACT}. RENEW / ADDCHG / CANCEL SUBSCRIPTION");
            var rowAltEMailAddress = string.Empty;

            //Check if there was a match on EmailID / SubID in ECN.
            if (ECNEmailID_SubIDMatch)
            {
                //YES MATCH. 
                //There is atleast a Sub / EmailID Match for sure.
                //Check to see if the EmailAddress Entered exists in a the Group.
                if (newEmailObj == null)
                {
                    // Do an Update 'cos we have the EmailID already
                    ReportNotExistRow(custID, row, pubGroup, rowIndex, rowPubCode, rowEmailAddress, rowAltEMailAddress);
                }
                else
                {
                    //There is a record of this EmailAddress.. Might be a different record / the EmailAddress has not been Changed / updated.
                    //Check to see if the EmailID's are the same !! If they are then its just an Update on other information. 
                    if (newEmailObj.EmailID > 0)
                    {
                        UpdateRow(custID, row, pubGroup, rowIndex, rowPubCode, rowEmailAddress, rowAltEMailAddress);
                    }
                    else
                    {
                        // Here the EmailID for the newEmailObj belongs to some one else. 
                        // so Let it go thru by using a Dummy Email Address & store the Duplicate emailAddress entered in the 
                        // user_ALTERNATE_EMAILADDRESS UDF
                        rowEmailAddress = getDummyEmailAddress(rowPubCode);
                        rowAltEMailAddress = rowEmailAddress;
                        InsertRow(custID, row, pubGroup, rowIndex, rowPubCode, rowEmailAddress, rowXACT, rowAltEMailAddress);
                    }
                }
            }
            else
            {
                NotifyAdminUnrecognizedXACT(NoMatch, row, rowIndex, rowPubCode, rowEmailAddress, rowXACT);
            }
        }

        private void InsertRow(int custID, DataRow row, ECN_Framework_Entities.Communicator.Group pubGroup, int rowIndex, string rowPubCode, string rowEmailAddress, int rowXACT, string rowAltEMailAddress)
        {
            WriteToLog($"EmailAddress: {rowEmailAddress} DOES NOT Exist in this group. REPORT ON NON EXISTING EMAIL / SUBID");
            ReportNonExistEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, row);
            var subject = NonExistingEmail;
            var body = string.Format(BodyFormat, rowPubCode, rowIndex + 1, rowEmailAddress, rowXACT, row[Sequence].ToString().Trim());
            NotifyAdmin(subject, body);
            WriteToLog("PUBLICATION: " + rowPubCode + "; CUSTOMERID: " + custID + "; EMAIL: " + rowEmailAddress + "");
            WriteToLog($"--- RECORD {rowIndex + 1} END ---");
        }

        private void UpdateRow(int custID, DataRow row, ECN_Framework_Entities.Communicator.Group pubGroup, int rowIndex, string rowPubCode, string rowEmailAddress, string rowAltEMailAddress)
        {
            WriteToLog($"EmailAddress: {rowEmailAddress} DOES Exist in this group. EmailID : {newEmailObj.EmailID} - UPDATING");
            UpdateEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, row);
            WriteToLog($"PUBLICATION: {rowPubCode}; CUSTOMERID: {custID}; EMAIL: {rowEmailAddress}");
            WriteToLog($"--- RECORD {rowIndex + 1} END ---");
        }

        private void ReportNotExistRow(int custID, DataRow row, ECN_Framework_Entities.Communicator.Group pubGroup, int rowIndex, string rowPubCode, string rowEmailAddress, string rowAltEMailAddress)
        {
            WriteToLog($"EmailAddress: {rowEmailAddress} DOES Exist in this group. EmailID : {ECNEmailID} - UPDATING");
            ReportNonExistEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, row);
            WriteToLog($"PUBLICATION: {rowPubCode}; CUSTOMERID: {custID}; EMAIL: {rowEmailAddress}");
            WriteToLog($"--- RECORD {rowIndex + 1} END ---");
        }

        private void NotifyAdminUnrecognizedXACT(string subject, DataRow row, int rowIndex, string rowPubCode, string rowEmailAddress, int rowXACT)
        {
            //Notify Admin about an UNRECOGNIZED XACT Code.
            WriteToLog($">>>>> {subject} for this Record. Email Sent to Admins <<<<< ");
            var body = string.Format(BodyFormat, rowPubCode, rowIndex + 1, rowEmailAddress, rowXACT, row[Sequence].ToString().Trim());
            NotifyAdmin(subject, body);
        }

        private void UpdateEmailProfile(string EmailAddress, ECN_Framework_Entities.Communicator.Group pubGroup, int custID, string AlternateEmail, DataRow selectDBIVDataTableRows)
        {
            var xmlProfile = new StringBuilder();
            var xmlUdf = new StringBuilder();
            var lineFeed = new Regex(@"\\n|\\r\\n");
            int tempInt;

            if (!int.TryParse(selectDBIVDataTableRows[EmailId].ToString().Trim(), out tempInt))
            {
                Trace.TraceError("selectDBIVDataTableRows[EmailId] cannot parse to integer");
            }

            if (newEmailObj.EmailID > 0)
            {
                var zip = selectDBIVDataTableRows[Zip].ToString().Trim();
                var zipPlus = selectDBIVDataTableRows[Plus4].ToString().Trim();
                var country = selectDBIVDataTableRows[Country].ToString().Trim();

                if (country.EqualsIgnoreCase(Ca) || country.EqualsIgnoreCase(Canada))
                {
                    zip = $"{zip} {zipPlus}";
                }
                else if (string.IsNullOrWhiteSpace(country))
                {
                    if (zipPlus.Length > 0)
                    {
                        zip = $"{zip}-{zipPlus}";
                    }
                }
                else if (zipPlus.Length > 0)
                {
                    zip = $"{zip}{zipPlus}";
                }

                xmlProfile.Append(EmailsOpenTag)
                    .Append($"<emailaddress>{CleanXMLString(EmailAddress)}</emailaddress>")
                    .Append($"<firstname><![CDATA[{SecurityElement.Escape(selectDBIVDataTableRows[FirstName].ToString().Trim())}]]></firstname>")
                    .Append($"<lastname><![CDATA[{SecurityElement.Escape(selectDBIVDataTableRows[LastName].ToString().Trim())}]]></lastname>")
                    .Append($"<company><![CDATA[{SecurityElement.Escape(selectDBIVDataTableRows[Company].ToString().Trim())}]]></company>")
                    .Append($"<occupation><![CDATA[{SecurityElement.Escape(selectDBIVDataTableRows[Title].ToString().Trim())}]]></occupation>")
                    .Append($"<address><![CDATA[{lineFeed.Replace(SecurityElement.Escape(selectDBIVDataTableRows[Address].ToString().Trim()), " ")}]]></address>")
                    .Append($"<address2><![CDATA[{lineFeed.Replace(SecurityElement.Escape(selectDBIVDataTableRows[MailStop].ToString().Trim()), " ")}]]></address2>")
                    .Append($"<city><![CDATA[{SecurityElement.Escape(selectDBIVDataTableRows[City].ToString().Trim())}]]></city>")
                    .Append($"<state><![CDATA[{SecurityElement.Escape(selectDBIVDataTableRows[State].ToString().Trim())}]]></state>")
                    .Append($"<zip><![CDATA[{SecurityElement.Escape(zip)}]]></zip>")
                    .Append($"<country><![CDATA[{SecurityElement.Escape(selectDBIVDataTableRows[Country].ToString().Trim())}]]></country>")
                    .Append($"<voice><![CDATA[{SecurityElement.Escape(selectDBIVDataTableRows[Phone].ToString().Trim())}]]></voice>")
                    .Append($"<fax><![CDATA[{SecurityElement.Escape(selectDBIVDataTableRows[Fax].ToString().Trim())}]]></fax>")
                    .Append($"<website><![CDATA[{SecurityElement.Escape(selectDBIVDataTableRows[Website].ToString().Trim())}]]></website>")
                    .Append("</Emails>");

                WriteToLog($"{EmailAddress} UPDATED. ");

                ProcessUdfHashGroup(EmailAddress, pubGroup, AlternateEmail, selectDBIVDataTableRows, xmlUdf, xmlProfile);
            }
            else
            {
                WriteToLog(" >>>>>> EMAILID = 0 in Update <<<<<<<");
                WriteToLog($"PUBCODE: {selectDBIVDataTableRows[PubCode].ToString().Trim()}");
                WriteToLog($"EMAIL: {selectDBIVDataTableRows[Email].ToString().Trim()}");
                WriteToLog($"SEQUENCE: {selectDBIVDataTableRows[Sequence].ToString().Trim()}");
                WriteToLog($"EMAILID: {selectDBIVDataTableRows[EmailId].ToString().Trim()}");
                WriteToLog(" >>>>>>>>>>>>>>><<<<<<<<<<<<<<<<");
            }
        }

        private void ProcessUdfHashGroup(string emailAddress, Group pubGroup, string alternateEmail, DataRow selectDbivDataTableRows, StringBuilder xmlUdf, StringBuilder xmlProfile)
        {
            var udfHash = GetGroupUDFHash(pubGroup.GroupID);

            if (udfHash.Count > 0)
            {
                var udfHashEnumerator = udfHash.GetEnumerator();

                while (udfHashEnumerator.MoveNext())
                {
                    ProcessUdfHash(emailAddress, alternateEmail, selectDbivDataTableRows, xmlUdf, udfHashEnumerator);
                }

                ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(
                    Main2.user,
                    pubGroup.CustomerID,
                    pubGroup.GroupID,
                    $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlProfile}</XML>",
                    $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlUdf}</XML>",
                    "HTML",
                    "S",
                    false,
                    string.Empty,
                    "ECNTools.ECN_Import.FPImportNightly");
            }
        }

        private void ProcessUdfHash(string emailAddress, string alternateEmail, DataRow selectDbivDataTableRows, StringBuilder xmlUdf, IDictionaryEnumerator udfHashEnumerator)
        {
            if (udfHashEnumerator.Key != null)
            {
                var key = udfHashEnumerator.Key.ToString();

                string udfData;

                try
                {
                    string userUdfValue;

                    if (udfHashEnumerator.Value.ToString().EqualsIgnoreCase(SubscribeId))
                    {
                        userUdfValue = selectDbivDataTableRows[Sequence].ToString().Trim();
                    }
                    else if (udfHashEnumerator.Value.ToString().EqualsIgnoreCase(PublicationCode))
                    {
                        userUdfValue = selectDbivDataTableRows[PubCode].ToString().Trim();
                    }
                    else if (udfHashEnumerator.Value.ToString().EqualsIgnoreCase(AlternateEmailAddress))
                    {
                        userUdfValue = alternateEmail;
                    }
                    else
                    {
                        userUdfValue = selectDbivDataTableRows[udfHashEnumerator.Value.ToString()].ToString().Trim();
                    }

                    udfData = userUdfValue.Length > 0
                                  ? userUdfValue
                                  : string.Empty;

                    WriteToLog($"{udfHashEnumerator.Value.ToString().ToUpper()}: {udfData}");
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    WriteToLog($"{udfHashEnumerator.Value}: >>>>> NOT IN DBASE FILE <<<<< ");
                    udfData = string.Empty;
                }

                if (!udfHashEnumerator.Value.ToString().Trim().EqualsIgnoreCase("DEMO39"))
                {
                    xmlUdf.Append("<row>").Append($"<ea>{SecurityElement.Escape(emailAddress)}</ea>").Append($"<udf id=\"{key}\">").Append($"<v><![CDATA[{udfData}]]></v>").Append("</udf>").Append("</row>");
                }
                else
                {
                    WriteToLog($"::{udfHashEnumerator.Value.ToString().Trim().ToUpper()}:: >>>>> IS IGNORED <<<<<");
                }
            }
        }

        private Hashtable GetGroupUDFHash(int groupID)
        {
            Hashtable retHash = new Hashtable();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);

            foreach (ECN_Framework_Entities.Communicator.GroupDataFields g in gdf)
            {
                retHash.Add(g.GroupDataFieldsID, g.ShortName);
            }
            return retHash;
        }

        private void ReportNonExistEmailProfile(string EmailAddress, ECN_Framework_Entities.Communicator.Group pubGroup, int custID, string AlternateEmail, DataRow selectDBIVDataTableRows)
        {
            WriteToLog(" >>>>>> REPORT NON EXISTANT EMAIL PROFILE <<<<<<<");
            WriteToLog("PUBCODE: " + selectDBIVDataTableRows["PUBCODE"].ToString().Trim());
            WriteToLog("EMAIL: " + selectDBIVDataTableRows["EMAIL"].ToString().Trim());
            WriteToLog("SEQUENCE: " + selectDBIVDataTableRows["SEQUENCE"].ToString().Trim());
            WriteToLog("EMAILID: " + selectDBIVDataTableRows["EMAILID"].ToString().Trim());
            WriteToLog(" >>>>>>>>>>>>>>><<<<<<<<<<<<<<<<");
        }

        private void CheckEmailAndSubscriberID(string rowEmailID, string rowSubscriberID, string groupID)
        {
            //Check for EmailID & get SubscriberID depending on that.
            if (Convert.ToInt32(rowEmailID.ToString()) > 0)
            {
                //Get EmailID, SubID from the current Row Using EmailID from the ECNDB.
                getECNSubscriberID(rowEmailID, groupID, "EmailSearch");
            }

            if (newEmailObj == null && Convert.ToInt32(rowSubscriberID.ToString()) > 0)
            {
                //Get SubscriptionID from the current Row Using a subscriberID from the ECNDB.
                getECNSubscriberID(rowSubscriberID, groupID, "SubIDSearch");
            }
        }

        private void getECNSubscriberID(string ID, string groupID, string searchType)
        {

            ECN_Framework_Entities.Communicator.EmailDataValues edv = null;
            try
            {
                if (searchType.Equals("EmailSearch"))
                {
                    //get id's by email id
                    ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(Convert.ToInt32(groupID)).First(x => x.ShortName.ToLower().Equals("subscriberid"));
                    try
                    {
                        edv = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID_NoAccessCheck(gdf.GroupDataFieldsID, Convert.ToInt32(ID)).First();
                    }
                    catch { }

                }
                else if (searchType.Equals("SubIDSearch"))
                {
                    //get id's by subscriberid datavalue
                    ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(Convert.ToInt32(groupID)).First(x => x.ShortName.ToLower().Equals("subscriberid"));
                    try
                    {
                        edv = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID_NoAccessCheck(gdf.GroupDataFieldsID).First(x => x.DataValue == ID);
                    }
                    catch { }

                }

                if (edv != null)
                {
                    ECNEmailID_SubIDMatch = true;

                    //ECNEmailID = dr["EmailID"].ToString();
                    //ECNSubscriberID = dr["DataValue"].ToString();
                    ECNEmailID = edv.EmailID.ToString();
                    ECNSubscriberID = edv.DataValue;
                }
                else
                {
                    //Added The following Check 'cos we purge the EmailDataValues Table records during the daily Clean up Process.
                    // - ashok 10/25/06
                    if (searchType.Equals("EmailSearch"))
                    {
                        if (Convert.ToInt32(ID) > 0)
                        {
                            ECNEmailID = ID;
                            ECNSubscriberID = "";
                            ECNEmailID_SubIDMatch = true;
                            WriteToLog("##### SUBID IS BLANK & got Deleted during ECN Cleanup Process #####");
                        }
                    }
                    else
                    {
                        ECNEmailID_SubIDMatch = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ECNEmailID = "";
                ECNSubscriberID = "";
            }

            if (ECNEmailID.Length > 0)
            {
                newEmailObj = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(Convert.ToInt32(ECNEmailID));
            }
        }

        private string getDummyEmailAddress(string pubCde)
        {
            string GUID = "", dummyEmail = "";
            GUID = System.Guid.NewGuid().ToString();
            dummyEmail = DateTime.Now.ToString("yyyyMMdd-HHmmss.fff") + "-" + GUID.Substring(0, 6) + "@" + pubCde + ".kmpsgroup.com";

            return dummyEmail;
        }

        private void NotifyAdmin(string subject, string body)
        {
            try
            {
                MailMessage msg = new MailMessage(ConfigurationManager.AppSettings["Admin_FromEmail"].ToString(), ConfigurationManager.AppSettings["Admin_ToEmail"].ToString());
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString());
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                WriteToLog(ex.Message);
                WriteToLog(ex.StackTrace);
            }
        }

        private DataTable CreateDataTableFromDBF(string filePath, string file)
        {
            WriteToLog("* Filepath: " + filePath + file);
           

            string connString = "";
            DataTable dt = null;
            try
            {
                DataSet dataset;
                //connString = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=dBASE IV;Data Source=" + filePath + ";";

                string fileNameNoExt = Path.GetFileNameWithoutExtension(file);
                connString = "Provider=VFPOLEDB.1;Data Source=" + filePath + "\\" + file;

                OleDbDataAdapter oleAdapter = new OleDbDataAdapter("SELECT * FROM [" + fileNameNoExt + "]", connString);
                dataset = new DataSet(file);
                oleAdapter.Fill(dataset); //fill the adapter with rows only from the linenumber specified				
                oleAdapter.Dispose();

                int countColumns = dataset.Tables[0].Columns.Count;
                int totalRecs = dataset.Tables[0].Rows.Count;
                dt = dataset.Tables[0];

                WriteToLog("* # of Columns in DBIV file : " + countColumns);
                WriteToLog("* # of Records in DBIV file : " + totalRecs);
            }
            catch (Exception ex)
            {
                string exx = ex.ToString();
                WriteToLog("[ERROR]: Error when Creating DataTable from DBASE File. \n" + exx);
            }

            return dt;
        }

        private void WriteToLog(string text)
        {
            Logfile.AutoFlush = true;
            Logfile.WriteLine(DateTime.Now + " >> " + text);
            Logfile.Flush();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ImportDataToDB();

            backgroundWorker1.ReportProgress(100, "End Time : " + System.DateTime.Now);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                lstMessage.Items.Add(e.UserState);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnImport.Enabled = true;
            btnCancel.Enabled = false;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            lstMessage.Items.Clear();

            if (comboBox1.SelectedItem.ToString() != "")
            {
                Pubs = "'" + comboBox1.SelectedItem.ToString() + "'";

                //for (int x = 0; x < listBox1.Items.Count; x++)
                //{
                //  // Determine if the item is selected.
                //  if(listBox1.GetSelected(x) == true)
                //     Pubs += ("'" + listBox1.Items[x].ToString() + "',");
                //}
                //if (Pubs.Length > 0)
                //{
                //Pubs = Pubs.Remove(Pubs.Length - 1, 1);

                if (txtDBFFile.Text != string.Empty && txtDBFFile.Text.EndsWith("dbf", StringComparison.OrdinalIgnoreCase))
                {
                    btnImport.Enabled = false;
                    btnCancel.Enabled = true;

                    lstMessage.Items.Add("Start Time : " + System.DateTime.Now);

                    backgroundWorker1.RunWorkerAsync();

                }
                else
                {
                    MessageBox.Show("Select DBF File");
                }
            }
            else
            {
                MessageBox.Show("Select Pub(s)");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChooseDBF_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtDBFFile.Text = Path.GetFileName(openFileDialog1.FileName);
                lstMessage.Items.Add("* Filepath: " + openFileDialog1.FileName);
            }
        }

        private void FPImportNitely_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main2 frm = (Main2)this.MdiParent;
            frm.ToggleMenus(true);
        }

        private string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }
    }
}
