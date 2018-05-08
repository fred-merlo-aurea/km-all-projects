using System;
using System.IO;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace ecn.kmps.ECN2FPImtrimDataExport
{
    /// <summary>
    /// Summary description for ECN2FPIntrimDataExport. 
    /// </summary>
    public class ECN2FPIntrimDataExport
    {
        public static string log = "";
        public static string LogFile = "";

        public static IEnumerator DBIVColListEnum = null;
        public static DataTable pubGroupsDataTable = null;
        public static string ecn_connString = "";
        public static string groups = "";
        private static SortedList _udfHash = new SortedList();
        public static SortedList customers_groups_UDFHash
        {
            get { return _udfHash; }
            set { _udfHash = value; }
        }

        public static string BATCH = "", BUSINESS = "", BUSINESS1 = "", BUSINESS2 = "", BUSINESS3 = "", BUSINESS4 = "", BUSINESS5 = "", BUSINESS6 = "", BUSINESS7 = "", BUSINESS8 = "", BUSINESS9 = "", BUSINESS10 = "", BUSINESS8TEXT = "", BUSINESS9TEXT = "", BUSINESS3TEXT = "", BUSINESSTEXT = "", CAT = "", DEMO1 = "", DEMO10 = "", DEMO10TEXT = "", DEMO11 = "", DEMO11TEXT = "", DEMO11TEXTHERBS = "", DEMO11TEXTINGR = "", DEMO11TEXTPCKG = "", DEMO12 = "", DEMO13 = "", DEMO13TEXT = "", DEMO14 = "", DEMO14TEXT = "", DEMO15 = "", DEMO2 = "", DEMO20 = "", DEMO21 = "", DEMO22 = "", DEMO23 = "", DEMO24 = "", DEMO25 = "", DEMO26 = "", DEMO27 = "", DEMO28 = "", DEMO29 = "", DEMO3 = "", DEMO31 = "", DEMO32 = "", DEMO33 = "", DEMO34 = "", DEMO35 = "", DEMO36 = "", DEMO37 = "", DEMO38 = "", DEMO38TEXT = "", DEMO39 = "", DEMO4 = "", DEMO40 = "", DEMO41 = "", DEMO42 = "", DEMO43 = "", DEMO44 = "", DEMO45 = "", DEMO46 = "", DEMO5 = "", DEMO6 = "", DEMO6TEXT = "", DEMO7 = "", DEMO8 = "", DEMO9 = "", DEMO9TEXT = "", EMPLOY = "", FORZIP = "", FUNCTION = "", FUNCTIONTEXT = "", HISTBATCH = "", PAR3C = "", PLASTICS = "", PLASTICSTEXT = "", PRODUCTS = "", PRODUCTSTEXT = "", QSOURCE = "", SALES = "", SUBSCRIPTION = "", SUBSRC = "", VERIFY = "", XACT = "", ALTERNATE_EMAILADDRESS = "";
        public static string DEMO16 = "", SEC_ADDRESS = "", SEC_ADDRESS2 = "", SEC_CITY = "", SEC_STATE = "", SEC_POSTALCODE = "";
        public static string FUNCTION1 = "", FUNCTION1TEXT = "", FUNCTION2 = "", FUNCTION2TEXT = "", FUNCTION3 = "", FUNCTION3TEXT = "", FUNCTION4 = "", FUNCTION4TXT = "", FUNCTION5 = "", FUNCTION5TXT = "";
        public static string MOBILE = "", PAYMENTSTATUS = "", STATE_INT = "";

        public static string PA1FNAME = "", PA1LNAME = "", PA1EMAIL = "", PA1FUNCTION = "", PA1FUNCTXT = "", PA1TITLE = "";
        public static string PA2FNAME = "", PA2LNAME = "", PA2EMAIL = "", PA2FUNCTION = "", PA2FUNCTXT = "", PA2TITLE = "";
        public static string PA3FNAME = "", PA3LNAME = "", PA3EMAIL = "", PA3FUNCTION = "", PA3FUNCTXT = "", PA3TITLE = "";
        public static string COMPANYTEXT = "", ADDRESS2 = "", MAILSTOP = "";
        public static string JOBT1 = "", JOBT2 = "", JOBT3 = "", TOE1 = "", TOE2 = "", AOI1 = "", AOI2 = "", AOI3 = "", PROD1 = "";
        public static string MEX_STATE = "", BUYAUTH = "", JOBT1TEXT = "", PROD1TEXT = "", TOE1TEXT = "";
        public static string IND1 = "", JOBT2TEXT = "", DEMO35A = "", DEMO35B = "", DEMO36A = "", DEMO36B = "";
        public static string DEMO34A = "", DEMO34B = "";
        public static string BUSNTEXT1 = "", BUSNTEXT5 = "", FUNCTEXT6 = "", FUNCTEXT7 = "", FUNCTEXT8 = "", FUNCTEXT9 = "";
        public static string SOURCE = "", CAMPAIGN = "", MEDIUM = "", WEBPGURL = "", FORMTYPE = "", HEADLINEID = "", URLOI = "", URLSU = "", SALUTATION = "", MARKETFACL = "";
        public static string MARKETCLN = "", MARKETRAIL = "";
        public static string TWITTER = "", TWITTER_HANDLE = "";
        public static string BUSINESS1_6 = "";
        public static SqlDateTime QDATE = new SqlDateTime();
        public static SqlDateTime sqldatenull = SqlDateTime.Null;
        public static StreamWriter logFile;
        private static KMPlatform.Entity.User kmUser = new KMPlatform.Entity.User();
        
        private static DateTime from;
        private static DateTime to;
        private static bool useDateRange;
        static void Main(string[] args)
        {
            try
            {
            string mm_dd_yyyy = DateTime.Now.ToString("MM-dd-yyyy");
            kmUser = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNUserAccessKey"].ToString(), false);
            int fromDateMinus = Convert.ToInt32(ConfigurationManager.AppSettings["FromDateMinus"].ToString());
                useDateRange = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDateRange"].ToString());
                Dictionary<int, int> countReport = new Dictionary<int, int>();
            try
            {
                    if (useDateRange)
                    {
                        DateTime.TryParse(ConfigurationManager.AppSettings["FromDate"].ToString(), out from);
                        DateTime.TryParse(ConfigurationManager.AppSettings["ToDate"].ToString(), out to);
                    }
                    else
                    {
                        //from = new DateTime(2015, 7, 3, 11, 30, 0);
                        from = DateTime.Now.AddDays(fromDateMinus);
                    }
            }
                catch { }

            //get global values.. connString & the Filemap Path to the DBIV file
            ecn_connString = ConfigurationManager.AppSettings["ecn_ConnString"].ToString();
            groups = ConfigurationManager.AppSettings["groups"].ToString();

            //Create the Log file 
            log = "logFiles/" + mm_dd_yyyy + ".log";
            logFile = new StreamWriter(new FileStream(log, System.IO.FileMode.Append));
            writeToLog(""); writeToLog("");
            writeToLog("-START--------------------------------------");


            // Create a HashTable with Groups & Corresponding CustomerID's
            writeToLog("Creating HashTable with Groups & Corresponding CustomerID's");
            CreateGroupsCustomerIDHash(groups);

            //Use the HashTable to loop thru the Data in each Group
            if (customers_groups_UDFHash.Count > 0)
            {
                IDictionaryEnumerator _customers_groups_HashEnumerator = customers_groups_UDFHash.GetEnumerator();
                while (_customers_groups_HashEnumerator.MoveNext())
                { //This the Outer most Loop
                    string _groupID = "", _customerID = "";
                    _groupID = _customers_groups_HashEnumerator.Key.ToString();
                    _customerID = _customers_groups_HashEnumerator.Value.ToString();
                        //Moving try up to here so we can keep processing groups if one fails. JWelter 11/19/2015
                        try
                        {
                    //start - testing block only 2850 --> 1053
                    //_groupID			= "2912";
                    //_customerID	= "1053";
                    //end - testing block
                    writeToLog("------ READY TO EXPORT RECORDS FOR - GroupID: " + _groupID + " CustomerID: " + _customerID + " -----");
                    DataTable currentlySelectedDT = new DataTable();
                    currentlySelectedDT = getSelectedRecordsToExport(_groupID, _customerID);
                    int totalSelected = 0;


                        totalSelected = currentlySelectedDT.Rows.Count;
                        writeToLog("Number of Records selected: " + totalSelected);
                            countReport.Add(Convert.ToInt32(_groupID), totalSelected);

                    if (totalSelected > 0)
                    {
                        extractDataToExportINTOIntrimDB(currentlySelectedDT, _groupID, _customerID);
                    }
                    else
                    {
                        writeToLog("Number of Records selected: 0");
                    }
                    //break; //THIS IS FOR TESTING ONLY
                }
                        catch (Exception ex)
                        {
                            //Adding error log here for individual groups JWelter 11/19/2015
                            KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "ECN2FPIntrimDataExport.Main", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                            writeToLog("ERROR FOR GROUP : " + _groupID.ToString());
                            writeToLog("ERROR: In Main - \n" + ex.ToString());
                            if (countReport.ContainsKey(Convert.ToInt32(_groupID)))
                            {
                                countReport[Convert.ToInt32(_groupID)] = -1;

                            }
                            else
                            {
                                countReport.Add(Convert.ToInt32(_groupID), -1);
            }
            }
                    }

                    //Send report to admins
                    try
                    {
                        SendReport(countReport);
                    }
                    catch (Exception ex)
                    {
                        writeToLog("ERROR SENDING REPORT------");
                    }
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "ECN2FPIntrimDataExport.Main", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                writeToLog("ERROR: In Main - \n" + ex.ToString());
            }
            finally
            {
            writeToLog("-END-----------------------------------------");
            logFile.Close();
        }
        }

        private static void SendReport(Dictionary<int, int> report)
        {
            StringBuilder sbReport = new StringBuilder();
            sbReport.AppendLine("Processed records report");
            foreach (KeyValuePair<int, int> kvp in report)
            {

                int groupID = kvp.Key;
                int count = kvp.Value;

                string pubCode = getPubCodeForGroupID(groupID);

                if (count == -1)
                {
                    sbReport.AppendLine("***ERROR OCCURRED for Pub: " + pubCode + " --- GroupID: " + groupID.ToString() + " --- No Records processed");
                    sbReport.AppendLine();
                }
                else
                {
                    sbReport.AppendLine("Pub: " + pubCode + " --- GroupID: " + groupID.ToString() + " --- Records processed: " + count.ToString());
                    sbReport.AppendLine();
                }
            }

            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["NOTIFY_ADMIN_FROM"].ToString());
            string[] emailTo = ConfigurationManager.AppSettings["NOTIFY_ADMIN_EMAIL"].ToString().Split(';');
            foreach (string s in emailTo)
            {
                if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(s))
                {
                    message.To.Add(s);
                }
            }

            message.Subject = "ECN to Interim DB export report";
            message.Body = sbReport.ToString();

            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString());
            try
            {
                smtp.Send(message);
            }
            catch
            {
                smtp.Send(message);
            }
        }

        private static string getPubCodeForGroupID(int groupID)
        {
            try
            {
                string query = "SELECT PublicationName FROM FPDataImporter WHERE GroupID = " + groupID.ToString();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;

                string pubcode = ECN_Framework_DataLayer.DataFunctions.ExecuteScalar(cmd, "IntrimPubDB").ToString();
                return pubcode;
            }
            catch
            {
                return "Unable to get PubCode";
            }
        }

        #region Create a HashTable with Groups & Corresponding CustomerID's
        public static void CreateGroupsCustomerIDHash(string groups)
        {
            try
            {
            List<int> st = groups.Split(',').Select(int.Parse).ToList();
            List<ECN_Framework_Entities.Communicator.Group> lstGroups = new List<ECN_Framework_Entities.Communicator.Group>();

                foreach (int i in st)
                {
                    ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(i);
                    if (g != null)
                        lstGroups.Add(g);
                }

                if (lstGroups.Count == 0)
                {
                    writeToLog("WARNING: While Creating HashTable with Groups & Corresponding CustomerID's - NO ROWS Returned ");
                    return;
                }
                else
                {
                    System.Collections.SortedList ht = new System.Collections.SortedList();

                    foreach (ECN_Framework_Entities.Communicator.Group g in lstGroups)
                    {
                        ht.Add(g.GroupID.ToString(), g.CustomerID.ToString());
                    }
                    _udfHash = ht;
                    writeToLog("HashTable Created SUCCESSFULLY");
                }
            }
            catch (Exception ex)
            {
                writeToLog("ERROR: While Creating HashTable with Groups & Corresponding CustomerID's \n" + ex.ToString());
                throw;
            }
        }
        #endregion

        #region Method to Extract Data from ECN from Each Group & Export to IntrimDB
        public static void extractDataToExportINTOIntrimDB(DataTable currentlySelectedDT, string _groupID, string _customerID)
        {
            SqlConnection intrimDBConn = new SqlConnection(ConfigurationManager.AppSettings["IntrimPubDB"]);
            int count = 0;
            string insertStmt = getInsertStatement();

            foreach (DataRow selectEmailListRow in currentlySelectedDT.Rows)
            {
                count++;

                //Set the Values for the UDF Variables from the CurrentRow by passing the same to the following method.
                setUDFDataFromDataRow(selectEmailListRow);

                try
                {
                    SqlCommand emailsInsertCommand = new SqlCommand(null, intrimDBConn);
                    emailsInsertCommand.CommandText = insertStmt;

                    //Set the Default Parameters
                    emailsInsertCommand.Parameters.Add("@CUSTOMERID", SqlDbType.Int, 4).Value = _customerID;
                    emailsInsertCommand.Parameters.Add("@GROUPID", SqlDbType.Int, 4).Value = _groupID;
                    emailsInsertCommand.Parameters.Add("@EMAILID", SqlDbType.Int, 4).Value = selectEmailListRow["EmailID"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@SUBSCRIBERID", SqlDbType.VarChar, 50).Value = selectEmailListRow["SUBSCRIBERID"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@TRANSACTIONTYPE", SqlDbType.VarChar, 10).Value = selectEmailListRow["TRANSACTIONTYPE"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@PUBLICATIONCODE", SqlDbType.VarChar, 15).Value = selectEmailListRow["PUBLICATIONCODE"].ToString().Trim();

                    if (ALTERNATE_EMAILADDRESS.Length > 5)
                    {
                        emailsInsertCommand.Parameters.Add("@EMAILADDRESS", SqlDbType.VarChar, 255).Value = ALTERNATE_EMAILADDRESS.Trim();
                    }
                    else
                    {
                        emailsInsertCommand.Parameters.Add("@EMAILADDRESS", SqlDbType.VarChar, 255).Value = selectEmailListRow["EmailAddress"].ToString().Trim();
                    }

                    emailsInsertCommand.Parameters.Add("@FNAME", SqlDbType.VarChar, 50).Value = selectEmailListRow["FirstName"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@LNAME", SqlDbType.VarChar, 50).Value = selectEmailListRow["LastName"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@COMPANY", SqlDbType.VarChar, 50).Value = selectEmailListRow["Company"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@TITLE", SqlDbType.VarChar, 255).Value = selectEmailListRow["Occupation"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@ADDRESS", SqlDbType.VarChar, 100).Value = selectEmailListRow["Address"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@CITY", SqlDbType.VarChar, 50).Value = selectEmailListRow["City"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@STATE", SqlDbType.VarChar, 50).Value = selectEmailListRow["State"].ToString().Trim();

                    string fullzip = selectEmailListRow["Zip"].ToString().Trim();
                    string zip = "", plus4 = "";
                    int index1 = fullzip.IndexOf("-");
                    int fulllength = fullzip.Length;
                    int index2 = (fullzip.IndexOf("-") + 1);
                    try
                    {
                        if (fullzip.IndexOf("-") > 0)
                        {
                            zip = fullzip.Substring(0, (fullzip.Length - fullzip.IndexOf("-")));
                            plus4 = fullzip.Substring(fullzip.IndexOf("-") + 1, (fullzip.Length - (fullzip.IndexOf("-") + 1)));
                        }
                        else
                        {
                            zip = fullzip;
                            plus4 = "";
                        }
                    }
                    catch (System.NullReferenceException nre)
                    {
                        // ZipCode might not be present for some SF's. so just Ignore it. 
                    }

                    emailsInsertCommand.Parameters.Add("@ZIP", SqlDbType.VarChar, 10).Value = zip;
                    emailsInsertCommand.Parameters.Add("@PLUS4", SqlDbType.VarChar, 10).Value = plus4;
                    emailsInsertCommand.Parameters.Add("@COUNTRY", SqlDbType.VarChar, 50).Value = selectEmailListRow["Country"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@PHONE", SqlDbType.VarChar, 50).Value = selectEmailListRow["Voice"].ToString().Trim();
                    emailsInsertCommand.Parameters.Add("@FAX", SqlDbType.VarChar, 50).Value = selectEmailListRow["Fax"].ToString().Trim();

                    //Set the Other UDF Parameters
                    emailsInsertCommand.Parameters.Add("@BATCH", SqlDbType.VarChar, 50).Value = BATCH;
                    emailsInsertCommand.Parameters.Add("@BUSINESS", SqlDbType.VarChar, 50).Value = BUSINESS;
                    emailsInsertCommand.Parameters.Add("@BUSINESS1", SqlDbType.VarChar, 50).Value = BUSINESS1;
                    emailsInsertCommand.Parameters.Add("@BUSINESS2", SqlDbType.VarChar, 50).Value = BUSINESS2;
                    emailsInsertCommand.Parameters.Add("@BUSINESS3", SqlDbType.VarChar, 50).Value = BUSINESS3;
                    emailsInsertCommand.Parameters.Add("@BUSINESS4", SqlDbType.VarChar, 50).Value = BUSINESS4;
                    emailsInsertCommand.Parameters.Add("@BUSINESS5", SqlDbType.VarChar, 50).Value = BUSINESS5;
                    emailsInsertCommand.Parameters.Add("@BUSINESS6", SqlDbType.VarChar, 50).Value = BUSINESS6;
                    emailsInsertCommand.Parameters.Add("@BUSINESS7", SqlDbType.VarChar, 50).Value = BUSINESS7;
                    emailsInsertCommand.Parameters.Add("@BUSINESS8", SqlDbType.VarChar, 50).Value = BUSINESS8;
                    emailsInsertCommand.Parameters.Add("@BUSINESS9", SqlDbType.VarChar, 50).Value = BUSINESS9;
                    emailsInsertCommand.Parameters.Add("@BUSINESS10", SqlDbType.VarChar, 50).Value = BUSINESS10;
                    emailsInsertCommand.Parameters.Add("@BUSINESS8TEXT", SqlDbType.VarChar, 50).Value = BUSINESS8TEXT;
                    emailsInsertCommand.Parameters.Add("@BUSINESS9TEXT", SqlDbType.VarChar, 50).Value = BUSINESS9TEXT;
                    emailsInsertCommand.Parameters.Add("@BUSINESS3TEXT", SqlDbType.VarChar, 50).Value = BUSINESS3TEXT;

                    emailsInsertCommand.Parameters.Add("@BUSINESSTEXT", SqlDbType.VarChar, 50).Value = BUSINESSTEXT;
                    emailsInsertCommand.Parameters.Add("@CAT", SqlDbType.VarChar, 50).Value = CAT;
                    emailsInsertCommand.Parameters.Add("@DEMO1", SqlDbType.VarChar, 500).Value = DEMO1;
                    emailsInsertCommand.Parameters.Add("@DEMO10", SqlDbType.VarChar, 500).Value = DEMO10;
                    emailsInsertCommand.Parameters.Add("@DEMO10TEXT", SqlDbType.VarChar, 500).Value = DEMO10TEXT;
                    emailsInsertCommand.Parameters.Add("@DEMO11", SqlDbType.VarChar, 500).Value = DEMO11;
                    emailsInsertCommand.Parameters.Add("@DEMO11TEXT", SqlDbType.VarChar, 500).Value = DEMO11TEXT;
                    emailsInsertCommand.Parameters.Add("@DEMO11TEXTHERBS", SqlDbType.VarChar, 500).Value = DEMO11TEXTHERBS;
                    emailsInsertCommand.Parameters.Add("@DEMO11TEXTINGR", SqlDbType.VarChar, 500).Value = DEMO11TEXTINGR;
                    emailsInsertCommand.Parameters.Add("@DEMO11TEXTPCKG", SqlDbType.VarChar, 500).Value = DEMO11TEXTPCKG;
                    emailsInsertCommand.Parameters.Add("@DEMO12", SqlDbType.VarChar, 500).Value = DEMO12;
                    emailsInsertCommand.Parameters.Add("@DEMO13", SqlDbType.VarChar, 500).Value = DEMO13;
                    emailsInsertCommand.Parameters.Add("@DEMO13TEXT", SqlDbType.VarChar, 500).Value = DEMO13TEXT;
                    emailsInsertCommand.Parameters.Add("@DEMO14", SqlDbType.VarChar, 500).Value = DEMO14;
                    emailsInsertCommand.Parameters.Add("@DEMO14TEXT", SqlDbType.VarChar, 500).Value = DEMO14TEXT;
                    emailsInsertCommand.Parameters.Add("@DEMO15", SqlDbType.VarChar, 500).Value = DEMO15;
                    emailsInsertCommand.Parameters.Add("@DEMO2", SqlDbType.VarChar, 500).Value = DEMO2;
                    emailsInsertCommand.Parameters.Add("@DEMO20", SqlDbType.VarChar, 500).Value = DEMO20;

                    //--new fields added 12/04/2006 - ashok

                    emailsInsertCommand.Parameters.Add("@DEMO21", SqlDbType.VarChar, 500).Value = DEMO21;
                    emailsInsertCommand.Parameters.Add("@DEMO22", SqlDbType.VarChar, 500).Value = DEMO22;
                    emailsInsertCommand.Parameters.Add("@DEMO23", SqlDbType.VarChar, 500).Value = DEMO23;
                    emailsInsertCommand.Parameters.Add("@DEMO24", SqlDbType.VarChar, 500).Value = DEMO24;
                    emailsInsertCommand.Parameters.Add("@DEMO25", SqlDbType.VarChar, 500).Value = DEMO25;
                    emailsInsertCommand.Parameters.Add("@DEMO26", SqlDbType.VarChar, 500).Value = DEMO26;
                    emailsInsertCommand.Parameters.Add("@DEMO27", SqlDbType.VarChar, 500).Value = DEMO27;
                    emailsInsertCommand.Parameters.Add("@DEMO28", SqlDbType.VarChar, 500).Value = DEMO28;
                    emailsInsertCommand.Parameters.Add("@DEMO29", SqlDbType.VarChar, 500).Value = DEMO29;

                    emailsInsertCommand.Parameters.Add("@DEMO3", SqlDbType.VarChar, 500).Value = DEMO3;
                    emailsInsertCommand.Parameters.Add("@DEMO31", SqlDbType.VarChar, 500).Value = DEMO31;
                    emailsInsertCommand.Parameters.Add("@DEMO32", SqlDbType.VarChar, 500).Value = DEMO32;
                    emailsInsertCommand.Parameters.Add("@DEMO33", SqlDbType.VarChar, 500).Value = DEMO33;
                    emailsInsertCommand.Parameters.Add("@DEMO34", SqlDbType.VarChar, 500).Value = DEMO34;
                    emailsInsertCommand.Parameters.Add("@DEMO35", SqlDbType.VarChar, 500).Value = DEMO35;

                    emailsInsertCommand.Parameters.Add("@DEMO36", SqlDbType.VarChar, 500).Value = DEMO36;
                    emailsInsertCommand.Parameters.Add("@DEMO37", SqlDbType.VarChar, 500).Value = DEMO37;

                    emailsInsertCommand.Parameters.Add("@DEMO38", SqlDbType.VarChar, 500).Value = DEMO38;
                    emailsInsertCommand.Parameters.Add("@DEMO38TEXT", SqlDbType.VarChar, 500).Value = DEMO38TEXT;
                    emailsInsertCommand.Parameters.Add("@DEMO39", SqlDbType.VarChar, 500).Value = DEMO39;
                    emailsInsertCommand.Parameters.Add("@DEMO4", SqlDbType.VarChar, 500).Value = DEMO4;

                    emailsInsertCommand.Parameters.Add("@DEMO40", SqlDbType.VarChar, 500).Value = DEMO40;
                    emailsInsertCommand.Parameters.Add("@DEMO41", SqlDbType.VarChar, 500).Value = DEMO41;
                    emailsInsertCommand.Parameters.Add("@DEMO42", SqlDbType.VarChar, 500).Value = DEMO42;
                    emailsInsertCommand.Parameters.Add("@DEMO43", SqlDbType.VarChar, 500).Value = DEMO43;
                    emailsInsertCommand.Parameters.Add("@DEMO44", SqlDbType.VarChar, 500).Value = DEMO44;
                    emailsInsertCommand.Parameters.Add("@DEMO45", SqlDbType.VarChar, 500).Value = DEMO45;
                    emailsInsertCommand.Parameters.Add("@DEMO46", SqlDbType.VarChar, 500).Value = DEMO46;

                    emailsInsertCommand.Parameters.Add("@DEMO5", SqlDbType.VarChar, 500).Value = DEMO5;
                    emailsInsertCommand.Parameters.Add("@DEMO6", SqlDbType.VarChar, 500).Value = DEMO6;
                    emailsInsertCommand.Parameters.Add("@DEMO6TEXT", SqlDbType.VarChar, 500).Value = DEMO6TEXT;
                    emailsInsertCommand.Parameters.Add("@DEMO7", SqlDbType.VarChar, 500).Value = DEMO7;
                    emailsInsertCommand.Parameters.Add("@DEMO8", SqlDbType.VarChar, 500).Value = DEMO8;
                    emailsInsertCommand.Parameters.Add("@DEMO9", SqlDbType.VarChar, 500).Value = DEMO9;
                    emailsInsertCommand.Parameters.Add("@DEMO9TEXT", SqlDbType.VarChar, 500).Value = DEMO9TEXT;
                    emailsInsertCommand.Parameters.Add("@EMPLOY", SqlDbType.VarChar, 500).Value = EMPLOY;
                    emailsInsertCommand.Parameters.Add("@FORZIP", SqlDbType.VarChar, 500).Value = FORZIP;

                    emailsInsertCommand.Parameters.Add("@FUNCTION", SqlDbType.VarChar, 500).Value = FUNCTION;
                    emailsInsertCommand.Parameters.Add("@FUNCTIONTEXT", SqlDbType.VarChar, 500).Value = FUNCTIONTEXT;

                    emailsInsertCommand.Parameters.Add("@FUNCTION1", SqlDbType.VarChar, 500).Value = FUNCTION1;
                    emailsInsertCommand.Parameters.Add("@FUNCTION1TEXT", SqlDbType.VarChar, 500).Value = FUNCTION1TEXT;

                    emailsInsertCommand.Parameters.Add("@FUNCTION2", SqlDbType.VarChar, 500).Value = FUNCTION2;
                    emailsInsertCommand.Parameters.Add("@FUNCTION2TEXT", SqlDbType.VarChar, 500).Value = FUNCTION2TEXT;

                    emailsInsertCommand.Parameters.Add("@FUNCTION3", SqlDbType.VarChar, 500).Value = FUNCTION3;
                    emailsInsertCommand.Parameters.Add("@FUNCTION3TEXT", SqlDbType.VarChar, 500).Value = FUNCTION3TEXT;

                    emailsInsertCommand.Parameters.Add("@FUNCTION4", SqlDbType.VarChar, 50).Value = FUNCTION4;
                    emailsInsertCommand.Parameters.Add("@FUNCTION4TXT", SqlDbType.VarChar, 50).Value = FUNCTION4TXT;
                    ;
                    emailsInsertCommand.Parameters.Add("@FUNCTION5", SqlDbType.VarChar, 50).Value = FUNCTION5;
                    emailsInsertCommand.Parameters.Add("@FUNCTION5TXT", SqlDbType.VarChar, 50).Value = FUNCTION5TXT;

                    emailsInsertCommand.Parameters.Add("@HISTBATCH", SqlDbType.VarChar, 500).Value = HISTBATCH;
                    emailsInsertCommand.Parameters.Add("@PAR3C", SqlDbType.VarChar, 50).Value = PAR3C;
                    emailsInsertCommand.Parameters.Add("@PLASTICS", SqlDbType.VarChar, 500).Value = PLASTICS;
                    emailsInsertCommand.Parameters.Add("@PLASTICSTEXT", SqlDbType.VarChar, 500).Value = PLASTICSTEXT;
                    emailsInsertCommand.Parameters.Add("@PRODUCTS", SqlDbType.VarChar, 500).Value = PRODUCTS;
                    emailsInsertCommand.Parameters.Add("@PRODUCTSTEXT", SqlDbType.VarChar, 500).Value = PRODUCTSTEXT;
                    emailsInsertCommand.Parameters.Add("@QDATE", SqlDbType.DateTime).Value = QDATE;
                    emailsInsertCommand.Parameters.Add("@QSOURCE", SqlDbType.VarChar, 50).Value = QSOURCE;
                    emailsInsertCommand.Parameters.Add("@SALES", SqlDbType.VarChar, 500).Value = SALES;
                    emailsInsertCommand.Parameters.Add("@SUBSCRIPTION", SqlDbType.VarChar, 10).Value = SUBSCRIPTION;
                    emailsInsertCommand.Parameters.Add("@SUBSRC", SqlDbType.VarChar, 50).Value = SUBSRC;
                    emailsInsertCommand.Parameters.Add("@VERIFY", SqlDbType.VarChar, 500).Value = VERIFY;
                    emailsInsertCommand.Parameters.Add("@XACT", SqlDbType.VarChar, 50).Value = XACT;

                    emailsInsertCommand.Parameters.Add("@APPDATE", SqlDbType.DateTime).Value = sqldatenull;
                    emailsInsertCommand.Parameters.Add("@PROCESSED", SqlDbType.Char).Value = 'N';

                    emailsInsertCommand.Parameters.Add("@DEMO16", SqlDbType.VarChar).Value = DEMO16;
                    emailsInsertCommand.Parameters.Add("@SEC_ADDRESS", SqlDbType.VarChar).Value = SEC_ADDRESS;
                    emailsInsertCommand.Parameters.Add("@SEC_ADDRESS2", SqlDbType.VarChar).Value = SEC_ADDRESS2;
                    emailsInsertCommand.Parameters.Add("@SEC_CITY", SqlDbType.VarChar).Value = SEC_CITY;
                    emailsInsertCommand.Parameters.Add("@SEC_STATE", SqlDbType.VarChar).Value = SEC_STATE;
                    emailsInsertCommand.Parameters.Add("@SEC_POSTALCODE", SqlDbType.VarChar).Value = SEC_POSTALCODE;
                    emailsInsertCommand.Parameters.Add("@MOBILE", SqlDbType.VarChar).Value = MOBILE;
                    emailsInsertCommand.Parameters.Add("@PAYMENTSTATUS", SqlDbType.VarChar).Value = PAYMENTSTATUS;
                    emailsInsertCommand.Parameters.Add("@STATE_INT", SqlDbType.VarChar).Value = STATE_INT;

                    emailsInsertCommand.Parameters.Add("@PA1FNAME", SqlDbType.VarChar).Value = PA1FNAME;
                    emailsInsertCommand.Parameters.Add("@PA1LNAME", SqlDbType.VarChar).Value = PA1LNAME;
                    emailsInsertCommand.Parameters.Add("@PA1EMAIL", SqlDbType.VarChar).Value = PA1EMAIL;
                    emailsInsertCommand.Parameters.Add("@PA1FUNCTION", SqlDbType.VarChar).Value = PA1FUNCTION;
                    emailsInsertCommand.Parameters.Add("@PA1FUNCTXT", SqlDbType.VarChar).Value = PA1FUNCTXT;
                    emailsInsertCommand.Parameters.Add("@PA1TITLE", SqlDbType.VarChar).Value = PA1TITLE;

                    emailsInsertCommand.Parameters.Add("@PA2FNAME", SqlDbType.VarChar).Value = PA2FNAME;
                    emailsInsertCommand.Parameters.Add("@PA2LNAME", SqlDbType.VarChar).Value = PA2LNAME;
                    emailsInsertCommand.Parameters.Add("@PA2EMAIL", SqlDbType.VarChar).Value = PA2EMAIL;
                    emailsInsertCommand.Parameters.Add("@PA2FUNCTION", SqlDbType.VarChar).Value = PA2FUNCTION;
                    emailsInsertCommand.Parameters.Add("@PA2FUNCTXT", SqlDbType.VarChar).Value = PA2FUNCTXT;
                    emailsInsertCommand.Parameters.Add("@PA2TITLE", SqlDbType.VarChar).Value = PA2TITLE;

                    emailsInsertCommand.Parameters.Add("@PA3FNAME", SqlDbType.VarChar).Value = PA3FNAME;
                    emailsInsertCommand.Parameters.Add("@PA3LNAME", SqlDbType.VarChar).Value = PA3LNAME;
                    emailsInsertCommand.Parameters.Add("@PA3EMAIL", SqlDbType.VarChar).Value = PA3EMAIL;
                    emailsInsertCommand.Parameters.Add("@PA3FUNCTION", SqlDbType.VarChar).Value = PA2FUNCTION;
                    emailsInsertCommand.Parameters.Add("@PA3FUNCTXT", SqlDbType.VarChar).Value = PA3FUNCTXT;
                    emailsInsertCommand.Parameters.Add("@PA3TITLE", SqlDbType.VarChar).Value = PA3TITLE;

                    emailsInsertCommand.Parameters.Add("@COMPANYTEXT", SqlDbType.VarChar).Value = COMPANYTEXT;
                    emailsInsertCommand.Parameters.Add("@ADDRESS2", SqlDbType.VarChar).Value = ADDRESS2;
                    emailsInsertCommand.Parameters.Add("@MAILSTOP", SqlDbType.VarChar, 50).Value = MAILSTOP;

                    emailsInsertCommand.Parameters.Add("@JOBT1", SqlDbType.VarChar).Value = JOBT1;
                    emailsInsertCommand.Parameters.Add("@JOBT2", SqlDbType.VarChar).Value = JOBT2;
                    emailsInsertCommand.Parameters.Add("@JOBT3", SqlDbType.VarChar).Value = JOBT3;
                    emailsInsertCommand.Parameters.Add("@TOE1", SqlDbType.VarChar).Value = TOE1;
                    emailsInsertCommand.Parameters.Add("@TOE2", SqlDbType.VarChar).Value = TOE2;
                    emailsInsertCommand.Parameters.Add("@AOI1", SqlDbType.VarChar).Value = AOI1;
                    emailsInsertCommand.Parameters.Add("@AOI2", SqlDbType.VarChar).Value = AOI2;
                    emailsInsertCommand.Parameters.Add("@AOI3", SqlDbType.VarChar).Value = AOI3;
                    emailsInsertCommand.Parameters.Add("@PROD1", SqlDbType.VarChar).Value = PROD1;

                    emailsInsertCommand.Parameters.Add("@MEX_STATE", SqlDbType.VarChar).Value = MEX_STATE;

                    emailsInsertCommand.Parameters.Add("@JOBT1TEXT", SqlDbType.VarChar).Value = JOBT1TEXT;
                    emailsInsertCommand.Parameters.Add("@BUYAUTH", SqlDbType.VarChar).Value = BUYAUTH;
                    emailsInsertCommand.Parameters.Add("@PROD1TEXT", SqlDbType.VarChar).Value = PROD1TEXT;
                    emailsInsertCommand.Parameters.Add("@TOE1TEXT", SqlDbType.VarChar).Value = TOE1TEXT;
                    emailsInsertCommand.Parameters.Add("@IND1", SqlDbType.VarChar).Value = IND1;
                    emailsInsertCommand.Parameters.Add("@JOBT2TEXT", SqlDbType.VarChar).Value = JOBT2TEXT;
                    emailsInsertCommand.Parameters.Add("@DEMO35A", SqlDbType.VarChar).Value = DEMO35A;
                    emailsInsertCommand.Parameters.Add("@DEMO35B", SqlDbType.VarChar).Value = DEMO35B;
                    emailsInsertCommand.Parameters.Add("@DEMO36A", SqlDbType.VarChar).Value = DEMO36A;
                    emailsInsertCommand.Parameters.Add("@DEMO36B", SqlDbType.VarChar).Value = DEMO36B;
                    emailsInsertCommand.Parameters.Add("@DEMO34A", SqlDbType.VarChar).Value = DEMO34A;
                    emailsInsertCommand.Parameters.Add("@DEMO34B", SqlDbType.VarChar).Value = DEMO34B;
                    emailsInsertCommand.Parameters.Add("@BUSNTEXT1", SqlDbType.VarChar).Value = BUSNTEXT1;
                    emailsInsertCommand.Parameters.Add("@BUSNTEXT5", SqlDbType.VarChar).Value = BUSNTEXT5;
                    emailsInsertCommand.Parameters.Add("@FUNCTEXT6", SqlDbType.VarChar).Value = FUNCTEXT6;
                    emailsInsertCommand.Parameters.Add("@FUNCTEXT7", SqlDbType.VarChar).Value = FUNCTEXT7;
                    emailsInsertCommand.Parameters.Add("@FUNCTEXT8", SqlDbType.VarChar).Value = FUNCTEXT8;
                    emailsInsertCommand.Parameters.Add("@FUNCTEXT9", SqlDbType.VarChar).Value = FUNCTEXT9;

                    emailsInsertCommand.Parameters.Add("@SOURCE", SqlDbType.VarChar).Value = SOURCE;
                    emailsInsertCommand.Parameters.Add("@CAMPAIGN", SqlDbType.VarChar).Value = CAMPAIGN;
                    emailsInsertCommand.Parameters.Add("@MEDIUM", SqlDbType.VarChar).Value = MEDIUM;
                    emailsInsertCommand.Parameters.Add("@WEBPGURL", SqlDbType.VarChar).Value = WEBPGURL;
                    emailsInsertCommand.Parameters.Add("@FORMTYPE", SqlDbType.VarChar).Value = FORMTYPE;
                    emailsInsertCommand.Parameters.Add("@HEADLINEID", SqlDbType.VarChar).Value = HEADLINEID;
                    emailsInsertCommand.Parameters.Add("@URLOI", SqlDbType.VarChar).Value = URLOI;
                    emailsInsertCommand.Parameters.Add("@URLSU", SqlDbType.VarChar).Value = URLSU;
                    emailsInsertCommand.Parameters.Add("@SALUTATION", SqlDbType.VarChar).Value = SALUTATION;
                    emailsInsertCommand.Parameters.Add("@MARKETFACL", SqlDbType.VarChar).Value = MARKETFACL;

                    emailsInsertCommand.Parameters.Add("@MARKETCLN", SqlDbType.VarChar).Value = MARKETCLN;
                    emailsInsertCommand.Parameters.Add("@MARKETRAIL", SqlDbType.VarChar).Value = MARKETRAIL;

                    emailsInsertCommand.Parameters.Add("@TWITTER", SqlDbType.VarChar).Value = TWITTER;
                    emailsInsertCommand.Parameters.Add("@TWITTER_HANDLE", SqlDbType.VarChar).Value = TWITTER_HANDLE;

                    emailsInsertCommand.Parameters.Add("@BUSINESS1_6", SqlDbType.VarChar).Value = BUSINESS1_6;

                    intrimDBConn.Open();
                    emailsInsertCommand.ExecuteNonQuery();
                    intrimDBConn.Close();
                }
                catch (Exception ex)
                {
                    writeToLog("ERROR: Exporting Record# - " + count + "\n" + ex.ToString());
                    intrimDBConn.Close();
                    throw;
                }
            }

            writeToLog("----- SUCCESSFULLY EXPORTED " + count + " ROWS FOR - GroupID: " + _groupID + " CustomerID: " + _customerID + " -----");
        }
        #endregion

        #region Method to set the Data For all UDF's from the Current DataRow
        public static void setUDFDataFromDataRow(DataRow selectEmailListRow)
        {
            //ReInitialize the Values for all Fields.
            BATCH = ""; BUSINESS = ""; BUSINESSTEXT = ""; CAT = ""; DEMO1 = ""; DEMO10 = ""; DEMO10TEXT = ""; DEMO11 = ""; DEMO11TEXT = ""; DEMO11TEXTHERBS = "";
            DEMO11TEXTINGR = ""; DEMO11TEXTPCKG = ""; DEMO12 = ""; DEMO13 = ""; DEMO13TEXT = ""; DEMO14 = ""; DEMO14TEXT = ""; DEMO15 = ""; DEMO2 = ""; DEMO20 = "";
            DEMO21 = ""; DEMO22 = ""; DEMO23 = ""; DEMO24 = ""; DEMO25 = ""; DEMO26 = ""; DEMO27 = ""; DEMO28 = ""; DEMO29 = ""; DEMO3 = ""; DEMO31 = ""; DEMO32 = "";
            DEMO33 = ""; DEMO34 = ""; DEMO38 = ""; DEMO38TEXT = ""; DEMO39 = ""; DEMO4 = ""; DEMO40 = ""; DEMO41 = ""; DEMO42 = ""; DEMO43 = ""; DEMO44 = ""; DEMO45 = "";
            DEMO46 = ""; DEMO5 = ""; DEMO6 = ""; DEMO6TEXT = ""; DEMO7 = ""; DEMO8 = ""; DEMO9 = ""; DEMO9TEXT = ""; EMPLOY = ""; FORZIP = ""; FUNCTION = "";
            FUNCTIONTEXT = ""; HISTBATCH = ""; PAR3C = ""; PLASTICS = ""; PLASTICSTEXT = ""; PRODUCTS = ""; PRODUCTSTEXT = ""; QSOURCE = ""; SALES = "";
            SUBSCRIPTION = ""; SUBSRC = ""; VERIFY = ""; XACT = ""; ALTERNATE_EMAILADDRESS = ""; DEMO16 = ""; SEC_ADDRESS = ""; SEC_ADDRESS2 = ""; SEC_CITY = "";
            SEC_STATE = ""; SEC_POSTALCODE = ""; QDATE = sqldatenull; BUSINESS1 = ""; BUSINESS2 = ""; BUSINESS3 = ""; BUSINESS4 = ""; BUSINESS5 = "";
            BUSINESS6 = ""; BUSINESS7 = ""; BUSINESS8 = ""; BUSINESS9 = ""; BUSINESS10 = ""; BUSINESS8TEXT = ""; BUSINESS9TEXT = ""; BUSINESS3TEXT = "";
            FUNCTION1 = ""; FUNCTION1TEXT = ""; FUNCTION2 = ""; FUNCTION2TEXT = ""; FUNCTION3 = ""; FUNCTION3TEXT = ""; FUNCTION4 = ""; FUNCTION4TXT = ""; FUNCTION5 = ""; FUNCTION5TXT = "";
            MOBILE = ""; PAYMENTSTATUS = ""; STATE_INT = ""; PA1FNAME = ""; PA1LNAME = ""; PA1EMAIL = ""; PA1FUNCTION = ""; PA1FUNCTXT = ""; PA1TITLE = "";
            PA2FNAME = ""; PA2LNAME = ""; PA2EMAIL = ""; PA2FUNCTION = ""; PA2FUNCTXT = ""; PA2TITLE = ""; PA3FNAME = ""; PA3LNAME = ""; PA3EMAIL = "";
            PA3FUNCTION = ""; PA3FUNCTXT = ""; PA3TITLE = ""; COMPANYTEXT = ""; ADDRESS2 = ""; DEMO35 = ""; MAILSTOP = ""; DEMO36 = ""; DEMO37 = "";
            JOBT1 = ""; JOBT2 = ""; JOBT3 = ""; TOE1 = ""; TOE2 = ""; AOI1 = ""; AOI2 = ""; AOI3 = ""; PROD1 = ""; MEX_STATE = ""; BUYAUTH = ""; JOBT1TEXT = ""; PROD1TEXT = ""; TOE1TEXT = "";
            IND1 = ""; JOBT2TEXT = ""; DEMO35A = ""; DEMO35B = ""; DEMO36A = ""; DEMO36B = ""; DEMO34A = ""; DEMO34B = "";
            BUSNTEXT1 = ""; BUSNTEXT5 = ""; FUNCTEXT6 = ""; FUNCTEXT7 = ""; FUNCTEXT8 = ""; FUNCTEXT9 = ""; 
            SOURCE = ""; CAMPAIGN = ""; MEDIUM = ""; WEBPGURL = ""; FORMTYPE = ""; HEADLINEID = ""; URLOI = ""; URLSU = ""; SALUTATION = ""; MARKETFACL = "";
            MARKETCLN = ""; MARKETRAIL = "";
            TWITTER = ""; TWITTER_HANDLE = ""; 
            BUSINESS1_6 = "";
            //get data for each column in to temp Variables. 
            //If they throw Exception, that means that group doesn't have that UDF so ignore & make it blank. 
            try { BATCH = selectEmailListRow["BATCH"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS = selectEmailListRow["BUSINESS"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS1 = selectEmailListRow["BUSINESS1"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS2 = selectEmailListRow["BUSINESS2"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS3 = selectEmailListRow["BUSINESS3"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS4 = selectEmailListRow["BUSINESS4"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS5 = selectEmailListRow["BUSINESS5"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS6 = selectEmailListRow["BUSINESS6"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS7 = selectEmailListRow["BUSINESS7"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS8 = selectEmailListRow["BUSINESS8"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS9 = selectEmailListRow["BUSINESS9"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS10 = selectEmailListRow["BUSINESS10"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS8TEXT = selectEmailListRow["BUSINESS8TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS9TEXT = selectEmailListRow["BUSINESS9TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS3TEXT = selectEmailListRow["BUSINESS3TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESSTEXT = selectEmailListRow["BUSINESSTEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { CAT = selectEmailListRow["CAT"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO1 = selectEmailListRow["DEMO1"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO10 = selectEmailListRow["DEMO10"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO10TEXT = selectEmailListRow["DEMO10TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO11 = selectEmailListRow["DEMO11"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO11TEXT = selectEmailListRow["DEMO11TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO11TEXTHERBS = selectEmailListRow["DEMO11TEXTHERBS"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO11TEXTINGR = selectEmailListRow["DEMO11TEXTINGR"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO11TEXTPCKG = selectEmailListRow["DEMO11TEXTPCKG"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO12 = selectEmailListRow["DEMO12"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO13 = selectEmailListRow["DEMO13"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO13TEXT = selectEmailListRow["DEMO13TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO14 = selectEmailListRow["DEMO14"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO14TEXT = selectEmailListRow["DEMO14TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO15 = selectEmailListRow["DEMO15"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO2 = selectEmailListRow["DEMO2"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO20 = selectEmailListRow["DEMO20"].ToString().Trim(); }
            catch (Exception) { }
            //--new fields added 12/04/2006 - ashok
            try { DEMO21 = selectEmailListRow["DEMO21"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO22 = selectEmailListRow["DEMO22"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO23 = selectEmailListRow["DEMO23"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO24 = selectEmailListRow["DEMO24"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO25 = selectEmailListRow["DEMO25"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO26 = selectEmailListRow["DEMO26"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO27 = selectEmailListRow["DEMO27"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO28 = selectEmailListRow["DEMO28"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO29 = selectEmailListRow["DEMO29"].ToString().Trim(); }
            catch (Exception) { }
            //-- ---
            try { DEMO3 = selectEmailListRow["DEMO3"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO31 = selectEmailListRow["DEMO31"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO32 = selectEmailListRow["DEMO32"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO33 = selectEmailListRow["DEMO33"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO34 = selectEmailListRow["DEMO34"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO38 = selectEmailListRow["DEMO38"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO38TEXT = selectEmailListRow["DEMO38TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO39 = selectEmailListRow["DEMO39"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO4 = selectEmailListRow["DEMO4"].ToString().Trim(); }
            catch (Exception) { }
            //--new fields added 12/04/2006 - ashok
            try { DEMO40 = selectEmailListRow["DEMO40"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO41 = selectEmailListRow["DEMO41"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO42 = selectEmailListRow["DEMO42"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO43 = selectEmailListRow["DEMO43"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO44 = selectEmailListRow["DEMO44"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO45 = selectEmailListRow["DEMO45"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO46 = selectEmailListRow["DEMO46"].ToString().Trim(); }
            catch (Exception) { }
            //-- ---
            try { DEMO5 = selectEmailListRow["DEMO5"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO6 = selectEmailListRow["DEMO6"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO6TEXT = selectEmailListRow["DEMO6TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO7 = selectEmailListRow["DEMO7"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO8 = selectEmailListRow["DEMO8"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO9 = selectEmailListRow["DEMO9"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO9TEXT = selectEmailListRow["DEMO9TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { EMPLOY = selectEmailListRow["EMPLOY"].ToString().Trim(); }
            catch (Exception) { }
            try { FORZIP = selectEmailListRow["FORZIP"].ToString().Trim(); }
            catch (Exception) { }

            try { FUNCTION = selectEmailListRow["FUNCTION"].ToString().Trim(); }
            catch (Exception) { }
            try { FUNCTIONTEXT = selectEmailListRow["FUNCTIONTEXT"].ToString().Trim(); }
            catch (Exception) { }

            try { FUNCTION1 = selectEmailListRow["FUNCTION1"].ToString().Trim(); }
            catch (Exception) { }
            try { FUNCTION1TEXT = selectEmailListRow["FUNCTION1TXT"].ToString().Trim(); }
            catch (Exception) { }

            try { FUNCTION2 = selectEmailListRow["FUNCTION2"].ToString().Trim(); }
            catch (Exception) { }
            try { FUNCTION2TEXT = selectEmailListRow["FUNCTION2TXT"].ToString().Trim(); }
            catch (Exception) { }

            try { FUNCTION3 = selectEmailListRow["FUNCTION3"].ToString().Trim(); }
            catch (Exception) { }
            try { FUNCTION3TEXT = selectEmailListRow["FUNCTION3TXT"].ToString().Trim(); }
            catch (Exception) { }

            try { FUNCTION4 = selectEmailListRow["FUNCTION4"].ToString().Trim(); }
            catch (Exception) { }
            try { FUNCTION4TXT = selectEmailListRow["FUNCTION4TXT"].ToString().Trim(); }
            catch (Exception) { }

            try { FUNCTION5 = selectEmailListRow["FUNCTION5"].ToString().Trim(); }
            catch (Exception) { }
            try { FUNCTION5TXT = selectEmailListRow["FUNCTION5TXT"].ToString().Trim(); }
            catch (Exception) { }

            try { HISTBATCH = selectEmailListRow["HISTBATCH"].ToString().Trim(); }
            catch (Exception) { }
            try { PAR3C = selectEmailListRow["PAR3C"].ToString().Trim(); }
            catch (Exception) { }
            try { PLASTICS = selectEmailListRow["PLASTICS"].ToString().Trim(); }
            catch (Exception) { }
            try { PLASTICSTEXT = selectEmailListRow["PLASTICSTEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { PRODUCTS = selectEmailListRow["PRODUCTS"].ToString().Trim(); }
            catch (Exception) { }
            try { PRODUCTSTEXT = selectEmailListRow["PRODUCTSTEXT"].ToString().Trim(); }
            catch (Exception) { }

            try
            {
                if (useDateRange)
                {
                    QDATE = DateTime.Parse(!string.IsNullOrEmpty(selectEmailListRow["LastChanged"].ToString().Trim()) ? selectEmailListRow["LastChanged"].ToString().Trim() : selectEmailListRow["CreatedOn"].ToString().Trim());
                }
                else
                {
                    if (from == null)
                    {
                        QDATE = DateTime.Parse(DateTime.Now.ToString().Trim());
                    }
                    else
                    {
                        QDATE = SqlDateTime.Parse(from.ToString());
                    }
                }
            }
            catch (Exception) { }
            try { QSOURCE = selectEmailListRow["QSOURCE"].ToString().Trim(); }
            catch (Exception) { }
            try { SALES = selectEmailListRow["SALES"].ToString().Trim(); }
            catch (Exception) { }
            try { SUBSCRIPTION = selectEmailListRow["SUBSCRIPTION"].ToString().Trim(); }
            catch (Exception) { }
            try { SUBSRC = selectEmailListRow["SUBSRC"].ToString().Trim(); }
            catch (Exception) { }
            try { VERIFY = selectEmailListRow["VERIFY"].ToString().Trim(); }
            catch (Exception) { }
            try { XACT = selectEmailListRow["XACT"].ToString().Trim(); }
            catch (Exception) { }
            try { ALTERNATE_EMAILADDRESS = selectEmailListRow["ALTERNATE_EMAILADDRESS"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO16 = selectEmailListRow["DEMO16"].ToString().Trim(); }
            catch (Exception) { }
            try { SEC_ADDRESS = selectEmailListRow["SEC_ADDRESS"].ToString().Trim(); }
            catch (Exception) { }
            try { SEC_ADDRESS2 = selectEmailListRow["SEC_ADDRESS2"].ToString().Trim(); }
            catch (Exception) { }
            try { SEC_CITY = selectEmailListRow["SEC_CITY"].ToString().Trim(); }
            catch (Exception) { }
            try { SEC_STATE = selectEmailListRow["SEC_STATE"].ToString().Trim(); }
            catch (Exception) { }
            try { SEC_POSTALCODE = selectEmailListRow["SEC_POSTALCODE"].ToString().Trim(); }
            catch (Exception) { }
            try { MOBILE = selectEmailListRow["MOBILE"].ToString().Trim(); }
            catch (Exception) { }
            try { PAYMENTSTATUS = selectEmailListRow["PAYMENTSTATUS"].ToString().Trim(); }
            catch (Exception) { }
            try { STATE_INT = selectEmailListRow["STATE_INT"].ToString().Trim(); }
            catch (Exception) { }

            try { PA1FNAME = selectEmailListRow["PA1FNAME"].ToString().Trim(); }
            catch (Exception) { }
            try { PA1LNAME = selectEmailListRow["PA1LNAME"].ToString().Trim(); }
            catch (Exception) { }
            try { PA1EMAIL = selectEmailListRow["PA1EMAIL"].ToString().Trim(); }
            catch (Exception) { }
            try { PA1FUNCTION = selectEmailListRow["PA1FUNCTION"].ToString().Trim(); }
            catch (Exception) { }
            try { PA1FUNCTXT = selectEmailListRow["PA1FUNCTXT"].ToString().Trim(); }
            catch (Exception) { }
            try { PA1TITLE = selectEmailListRow["PA1TITLE"].ToString().Trim(); }
            catch (Exception) { }

            try { PA2FNAME = selectEmailListRow["PA2FNAME"].ToString().Trim(); }
            catch (Exception) { }
            try { PA2LNAME = selectEmailListRow["PA2LNAME"].ToString().Trim(); }
            catch (Exception) { }
            try { PA2EMAIL = selectEmailListRow["PA2EMAIL"].ToString().Trim(); }
            catch (Exception) { }
            try { PA2FUNCTION = selectEmailListRow["PA2FUNCTION"].ToString().Trim(); }
            catch (Exception) { }
            try { PA2FUNCTXT = selectEmailListRow["PA2FUNCTXT"].ToString().Trim(); }
            catch (Exception) { }
            try { PA2TITLE = selectEmailListRow["PA2TITLE"].ToString().Trim(); }
            catch (Exception) { }

            try { PA3FNAME = selectEmailListRow["PA3FNAME"].ToString().Trim(); }
            catch (Exception) { }
            try { PA3LNAME = selectEmailListRow["PA3LNAME"].ToString().Trim(); }
            catch (Exception) { }
            try { PA3EMAIL = selectEmailListRow["PA3EMAIL"].ToString().Trim(); }
            catch (Exception) { }
            try { PA3FUNCTION = selectEmailListRow["PA3FUNCTION"].ToString().Trim(); }
            catch (Exception) { }
            try { PA3FUNCTXT = selectEmailListRow["PA3FUNCTXT"].ToString().Trim(); }
            catch (Exception) { }
            try { PA3TITLE = selectEmailListRow["PA3TITLE"].ToString().Trim(); }
            catch (Exception) { }

            try { DEMO35 = selectEmailListRow["DEMO35"].ToString().Trim(); }
            catch (Exception) { }

            try { DEMO36 = selectEmailListRow["DEMO36"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO37 = selectEmailListRow["DEMO37"].ToString().Trim(); }
            catch (Exception) { }

            try { COMPANYTEXT = selectEmailListRow["COMPANYTEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { ADDRESS2 = selectEmailListRow["ADDRESS2"].ToString().Trim(); }
            catch (Exception) { }
            try { MAILSTOP = selectEmailListRow["MAILSTOP"].ToString().Trim(); }
            catch (Exception) { }

            try { JOBT1 = selectEmailListRow["JOBT1"].ToString().Trim(); }
            catch (Exception) { }
            try { JOBT2 = selectEmailListRow["JOBT2"].ToString().Trim(); }
            catch (Exception) { }
            try { JOBT3 = selectEmailListRow["JOBT3"].ToString().Trim(); }
            catch (Exception) { }
            try { TOE1 = selectEmailListRow["TOE1"].ToString().Trim(); }
            catch (Exception) { }
            try { TOE2 = selectEmailListRow["TOE2"].ToString().Trim(); }
            catch (Exception) { }
            try { AOI1 = selectEmailListRow["AOI1"].ToString().Trim(); }
            catch (Exception) { }
            try { AOI2 = selectEmailListRow["AOI2"].ToString().Trim(); }
            catch (Exception) { }
            try { AOI3 = selectEmailListRow["AOI3"].ToString().Trim(); }
            catch (Exception) { }
            try { PROD1 = selectEmailListRow["PROD1"].ToString().Trim(); }
            catch (Exception) { }
            try { MEX_STATE = selectEmailListRow["MEX_STATE"].ToString().Trim(); }
            catch (Exception) { }
            try { BUYAUTH = selectEmailListRow["BUYAUTH"].ToString().Trim(); }
            catch (Exception) { }
            try { JOBT1TEXT = selectEmailListRow["JOBT1TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { PROD1TEXT = selectEmailListRow["PROD1TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { TOE1TEXT = selectEmailListRow["TOE1TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { IND1 = selectEmailListRow["IND1"].ToString().Trim(); }
            catch (Exception) { }
            try { JOBT2TEXT = selectEmailListRow["JOBT2TEXT"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO35A = selectEmailListRow["DEMO35A"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO35B = selectEmailListRow["DEMO35B"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO36A = selectEmailListRow["DEMO36A"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO36B = selectEmailListRow["DEMO36B"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO34A = selectEmailListRow["DEMO34A"].ToString().Trim(); }
            catch (Exception) { }
            try { DEMO34B = selectEmailListRow["DEMO34B"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSNTEXT1 = selectEmailListRow["BUSNTEXT1"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSNTEXT5 = selectEmailListRow["BUSNTEXT5"].ToString().Trim(); }
            catch (Exception) { }
            try { FUNCTEXT6 = selectEmailListRow["FUNCTEXT6"].ToString().Trim(); }
            catch (Exception) { }
            try { FUNCTEXT7 = selectEmailListRow["FUNCTEXT7"].ToString().Trim(); }
            catch (Exception) { }
            try { FUNCTEXT8 = selectEmailListRow["FUNCTEXT8"].ToString().Trim(); }
            catch (Exception) { }
            try { FUNCTEXT9 = selectEmailListRow["FUNCTEXT9"].ToString().Trim(); }
            catch (Exception) { }
            try { SOURCE = selectEmailListRow["SOURCE"].ToString().Trim(); }
            catch (Exception) { }
            try { CAMPAIGN = selectEmailListRow["CAMPAIGN"].ToString().Trim(); }
            catch (Exception) { }
            try { MEDIUM = selectEmailListRow["MEDUIM"].ToString().Trim(); }
            catch (Exception) { }
            try { WEBPGURL = selectEmailListRow["WEBPGURL"].ToString().Trim(); }
            catch (Exception) { }
            try { FORMTYPE = selectEmailListRow["FORMTYPE"].ToString().Trim(); }
            catch (Exception) { }
            try { HEADLINEID = selectEmailListRow["HEADLINEID"].ToString().Trim(); }
            catch (Exception) { }
            try { URLOI = selectEmailListRow["URLOI"].ToString().Trim(); }
            catch (Exception) { }
            try { URLSU = selectEmailListRow["URLSU"].ToString().Trim(); }
            catch (Exception) { }
            try { SALUTATION = selectEmailListRow["SALUTATION"].ToString().Trim(); }
            catch (Exception) { }
            try { MARKETFACL = selectEmailListRow["MARKETFACL"].ToString().Trim(); }
            catch (Exception) { }
            try { MARKETCLN = selectEmailListRow["MARKETCLN"].ToString().Trim(); }
            catch (Exception) { }
            try { MARKETRAIL = selectEmailListRow["MARKETRAIL"].ToString().Trim(); }
            catch (Exception) { }
            try { TWITTER = selectEmailListRow["TWITTER"].ToString().Trim(); }
            catch (Exception) { }
            try { TWITTER_HANDLE = selectEmailListRow["TWITTER_HANDLE"].ToString().Trim(); }
            catch (Exception) { }
            try { BUSINESS1_6 = selectEmailListRow["BUSINESS1_6"].ToString().Trim(); }
            catch (Exception) { }
            
            
        }

        #endregion

        #region Formatted Insert Stmt
        public static string getInsertStatement()
        {
            //Make the InsertStmt
            string insertStmt = string.Format(@"INSERT INTO PublicationSubscriptionData(CUSTOMERID,GROUPID,EMAILID,SUBSCRIBERID,TRANSACTIONTYPE,PUBLICATIONCODE,EMAILADDRESS,FNAME,LNAME,COMPANY,TITLE,
                ADDRESS,MAILSTOP,CITY,STATE,ZIP,PLUS4,COUNTRY,PHONE,FAX,BATCH,BUSINESS,BUSINESSTEXT,BUSINESS3TEXT,BUSINESS1,BUSINESS2,BUSINESS3,BUSINESS4,BUSINESS5,
                BUSINESS6,BUSINESS7,BUSINESS8,BUSINESS9,BUSINESS10,BUSINESS8TEXT,BUSINESS9TEXT,
                CAT,DEMO1,DEMO10,DEMO10TEXT,DEMO11,DEMO11TEXT,DEMO11TEXTHERBS,
                DEMO11TEXTINGR,DEMO11TEXTPCKG,DEMO12,DEMO13,DEMO13TEXT,DEMO14,DEMO14TEXT,DEMO15,DEMO2,DEMO20,DEMO21,DEMO22,DEMO23,DEMO24,DEMO25,DEMO26,DEMO27,DEMO28,DEMO29,DEMO3,DEMO31,DEMO32,
                DEMO33,DEMO34,DEMO38,DEMO38TEXT,DEMO39,DEMO4,DEMO40,DEMO41,DEMO42,DEMO43,DEMO44,DEMO45,DEMO46,DEMO5,DEMO6,DEMO6TEXT,DEMO7,DEMO8,DEMO9,DEMO9TEXT,EMPLOY,FORZIP,
                [FUNCTION],FUNCTIONTEXT,FUNCTION1,FUNCTION1TXT,FUNCTION2,FUNCTION2TXT,FUNCTION3,FUNCTION3TXT,
                FUNCTION4,FUNCTION4TXT,FUNCTION5,FUNCTION5TXT,
                HISTBATCH,PAR3C,PLASTICS,PLASTICSTEXT,PRODUCTS,PRODUCTSTEXT,QDATE,QSOURCE,SALES,SUBSCRIPTION,SUBSRC,VERIFY,XACT,APPDATE,PROCESSED,
                DEMO16,SEC_ADDRESS,SEC_ADDRESS2,SEC_CITY,SEC_STATE,SEC_POSTALCODE,MOBILE,PAYMENTSTATUS,STATE_INT,COMPANYTEXT,ADDRESS2,DEMO35,DEMO36,DEMO37,
                PA1FNAME,PA1LNAME,PA1EMAIL,PA1FUNCTION,PA1FUNCTXT,PA1TITLE,PA2FNAME,PA2LNAME,PA2EMAIL,
                PA2FUNCTION,PA2FUNCTXT,PA2TITLE,PA3FNAME,PA3LNAME,PA3EMAIL,PA3FUNCTION,PA3FUNCTXT,PA3TITLE,
                JOBT1, JOBT2, JOBT3, TOE1, TOE2, AOI1, AOI2, AOI3, PROD1, MEX_STATE, BUYAUTH, JOBT1TEXT, PROD1TEXT, TOE1TEXT,IND1, JOBT2TEXT, DEMO35A, DEMO35B, DEMO36A, DEMO36B,DEMO34A, DEMO34B,
                BUSNTEXT1, BUSNTEXT5, FUNCTEXT6, FUNCTEXT7, FUNCTEXT8, FUNCTEXT9,SOURCE,CAMPAIGN,MEDIUM,WEBPGURL,FORMTYPE,HEADLINEID,URLOI,URLSU,SALUTATION,MARKETFACL,
                MARKETCLN,MARKETRAIL,TWITTER,TWITTER_HANDLE,BUSINESS1_6)
                VALUES                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
                (@CUSTOMERID,@GROUPID,@EMAILID,@SUBSCRIBERID,@TRANSACTIONTYPE,@PUBLICATIONCODE,@EMAILADDRESS,@FNAME,@LNAME,@COMPANY,@TITLE,@ADDRESS,@MAILSTOP,@CITY,
                @STATE,@ZIP,@PLUS4,@COUNTRY,@PHONE,@FAX,@BATCH,@BUSINESS,@BUSINESSTEXT,@BUSINESS3TEXT,@BUSINESS1,@BUSINESS2,@BUSINESS3,@BUSINESS4,@BUSINESS5,
                @BUSINESS6,@BUSINESS7,@BUSINESS8,@BUSINESS9,@BUSINESS10,@BUSINESS8TEXT,@BUSINESS9TEXT,
                @CAT,@DEMO1,@DEMO10,@DEMO10TEXT,@DEMO11,
                @DEMO11TEXT,@DEMO11TEXTHERBS,@DEMO11TEXTINGR,@DEMO11TEXTPCKG,@DEMO12,@DEMO13,@DEMO13TEXT,@DEMO14,@DEMO14TEXT,@DEMO15,@DEMO2,@DEMO20,@DEMO21,@DEMO22,@DEMO23,@DEMO24,
                @DEMO25,@DEMO26,@DEMO27,@DEMO28,@DEMO29,@DEMO3,@DEMO31,@DEMO32,@DEMO33,@DEMO34,@DEMO38,@DEMO38TEXT,@DEMO39,@DEMO4,@DEMO40,@DEMO41,@DEMO42,@DEMO43,@DEMO44,@DEMO45,@DEMO46,
                @DEMO5,@DEMO6,@DEMO6TEXT,@DEMO7,@DEMO8,@DEMO9,@DEMO9TEXT,@EMPLOY,@FORZIP,@FUNCTION,@FUNCTIONTEXT,@FUNCTION1,@FUNCTION1TEXT,@FUNCTION2,@FUNCTION2TEXT,@FUNCTION3,@FUNCTION3TEXT,
                @FUNCTION4,@FUNCTION4TXT,@FUNCTION5,@FUNCTION5TXT,
                @HISTBATCH,@PAR3C,@PLASTICS,@PLASTICSTEXT,@PRODUCTS,@PRODUCTSTEXT,
                @QDATE,@QSOURCE,@SALES,@SUBSCRIPTION,@SUBSRC,@VERIFY,@XACT,@APPDATE,@PROCESSED,@DEMO16,@SEC_ADDRESS,@SEC_ADDRESS2,@SEC_CITY,@SEC_STATE,@SEC_POSTALCODE,@MOBILE,@PAYMENTSTATUS,@STATE_INT,@COMPANYTEXT,
                @ADDRESS2,@DEMO35,@DEMO36,@DEMO37,@PA1FNAME,@PA1LNAME,@PA1EMAIL,@PA1FUNCTION,@PA1FUNCTXT,@PA1TITLE,@PA2FNAME,@PA2LNAME,@PA2EMAIL,@PA2FUNCTION,@PA2FUNCTXT,@PA2TITLE,
                @PA3FNAME,@PA3LNAME,@PA3EMAIL,@PA3FUNCTION,@PA3FUNCTXT,@PA3TITLE,
                @JOBT1,@JOBT2,@JOBT3,@TOE1,@TOE2,@AOI1,@AOI2,@AOI3,@PROD1,@MEX_STATE, @BUYAUTH, @JOBT1TEXT, @PROD1TEXT, @TOE1TEXT, @IND1, @JOBT2TEXT,@DEMO35A, @DEMO35B, @DEMO36A, @DEMO36B,@DEMO34A, @DEMO34B,
                @BUSNTEXT1, @BUSNTEXT5, @FUNCTEXT6, @FUNCTEXT7, @FUNCTEXT8, @FUNCTEXT9,
                @SOURCE,@CAMPAIGN,@MEDIUM,@WEBPGURL,@FORMTYPE,@HEADLINEID,@URLOI,@URLSU,@SALUTATION,@MARKETFACL,
                @MARKETCLN,@MARKETRAIL,@TWITTER, @TWITTER_HANDLE,@BUSINESS1_6)");

            return insertStmt;
        }
        #endregion

        #region Method to Select Records that needs to be Exported
        public static DataTable getSelectedRecordsToExport(string groupID, string customerID)
        {
            DataTable selectedEmailProfilesDT = null;
            try
            {
            //string filter = " AND ((CONVERT(VARCHAR, CreatedOn, 101) = CONVERT(VARCHAR,GETDATE()-1,101) ) OR (CONVERT(VARCHAR, LastChanged, 101) = CONVERT(VARCHAR,GETDATE()-1,101)) ) ";
            //Changed the above filter to the one below 'cos when there's a FP update from Phil, the next day, it exports all the data 
            //in to the Interim DB. we only need the web txns & not all the txns. - ashok 8/28/06
            //string filter =	" AND ( (CONVERT(VARCHAR, CreatedOn, 101) = CONVERT(VARCHAR,GETDATE()-1,101) OR "+
            //					" CONVERT(VARCHAR, LastChanged, 101) = CONVERT(VARCHAR,GETDATE()-1,101) ) "+
            //					" AND (LEN(TRANSACTIONTYPE) <> 0 AND TRANSACTIONTYPE <> '') ) ";

            //Changed the above Filter & reverted to the way it was before. Phil wanted this 'cos he wanted to get the EmailID's associated with each record so he won't bug me
            //to get the whole list from ECN for a Publication each time he does the FP updates.

            SqlConnection dbConn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlConnection dbConn_Activity = new SqlConnection(ConfigurationManager.AppSettings["Activity"]);

            string filter = "";
                if (useDateRange)
                {
                    filter = " AND CAST(ISNULL(LastChanged,CreatedOn) as date) BETWEEN '" + from.ToString("MM/dd/yyyy") + "' AND '" + to.ToString("MM/dd/yyyy") + "' ";
                }
                else
                {
            if (from != null)
            {
                        filter = " AND CAST(ISNULL(LastChanged, CreatedOn) as date) = '" + from.ToString("MM/dd/yyyy") + "' ";
                
            }
            else
            {
                        filter = " AND CAST(ISNULL(LastChanged, CreatedOn) as date) = GETDATE() - 1 ";
                    }
            }
            //removed section where it was calling a stored proc to see if any records get returned.  If it did then it would execute sproc below this, else it would just return a null datatable.  JWelter 02112014
            string emailsFilter = "";

            //SqlCommand selectedEmailsProfileCmd = null;
            //SqlDataAdapter selectedEmailProfilesDA = null;
            //DataSet selectedEmailProfilesDS = null;

            //emailsFilter = " AND ( Emails.EmailID in ( SELECT EmailID FROM ecn5_communicator..EmailGroups WHERE GroupID = " + groupID + " AND ((CONVERT(VARCHAR, CreatedOn, 101) = CONVERT(VARCHAR,GETDATE()-1,101)) OR (CONVERT(VARCHAR, LastChanged, 101) = CONVERT(VARCHAR,GETDATE()-1,101))) )) ";
            emailsFilter = " AND ( Emails.EmailID in ( SELECT EmailID FROM ecn5_communicator..EmailGroups WHERE GroupID = " + groupID + filter + " ))";
            Console.WriteLine(emailsFilter);
                int tryTimes = 2;
                try
                {
                    tryTimes = Convert.ToInt32(ConfigurationManager.AppSettings["TryTimes"].ToString());
                }
                catch { }
                for (int i = 0; i < tryTimes; i++)
                {
                    try
                    {
                        selectedEmailProfilesDT = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.FilterEmailsAllWithSmartSegment(Convert.ToInt32(groupID), Convert.ToInt32(customerID), 0, emailsFilter, 0, "", "", 0, null);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (i + 1 == tryTimes)
                        {
                            Console.WriteLine("Issue getting records from ECN for Group: " + groupID.ToString() + ".  Hit max amount of retries.  Notifying Admin");
                            writeToLog("Issue getting records from ECN for Group: " + groupID.ToString() + ".  Hit max amount of retries.  Notifying Admin");
                            throw ex;
                        }
                        else
                        {
                            Console.WriteLine("Issue getting records from ECN for Group: " + groupID.ToString() + ".  Attempt: " + (i + 1).ToString() + " out of " + tryTimes.ToString());
                            writeToLog("Issue getting records from ECN for Group: " + groupID.ToString() + ".  Attempt: " + (i + 1).ToString() + " out of " + tryTimes.ToString());
                        }
                    }
                }

                //selectedEmailsProfileCmd = new SqlCommand("v_FilterEmails_ALL_with_smartSegment", dbConn_Activity);
                //selectedEmailsProfileCmd.CommandTimeout = 0;

                //selectedEmailsProfileCmd.CommandType = CommandType.StoredProcedure;
                //selectedEmailsProfileCmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
                //selectedEmailsProfileCmd.Parameters["@GroupID"].Value = groupID;
                //selectedEmailsProfileCmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
                //selectedEmailsProfileCmd.Parameters["@CustomerID"].Value = customerID;
                //selectedEmailsProfileCmd.Parameters.Add(new SqlParameter("@FilterID", SqlDbType.Int));
                //selectedEmailsProfileCmd.Parameters["@FilterID"].Value = 0;
                //selectedEmailsProfileCmd.Parameters.Add(new SqlParameter("@Filter", SqlDbType.VarChar, 2500));
                //selectedEmailsProfileCmd.Parameters["@Filter"].Value = emailsFilter;
                //selectedEmailsProfileCmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
                //selectedEmailsProfileCmd.Parameters["@BlastID"].Value = 0;
                //selectedEmailsProfileCmd.Parameters.Add(new SqlParameter("@BlastID_and_BounceDomain", SqlDbType.VarChar));
                //selectedEmailsProfileCmd.Parameters["@BlastID_and_BounceDomain"].Value = "";
                //selectedEmailsProfileCmd.Parameters.Add(new SqlParameter("@ActionType", SqlDbType.VarChar));
                //selectedEmailsProfileCmd.Parameters["@ActionType"].Value = "";
                //selectedEmailsProfileCmd.Parameters.Add(new SqlParameter("@refBlastID", SqlDbType.Int));
                //selectedEmailsProfileCmd.Parameters["@refBlastID"].Value = 0;

                //selectedEmailProfilesDA = new SqlDataAdapter(selectedEmailsProfileCmd);
                //selectedEmailProfilesDS = new DataSet();
                //dbConn.Open();
                //selectedEmailProfilesDA.Fill(selectedEmailProfilesDS, "v_FilterEmails_ALL_with_smartSegment");
                //dbConn.Close();
                //selectedEmailProfilesDT = selectedEmailProfilesDS.Tables[0];
                Console.WriteLine("Total Records:" + selectedEmailProfilesDT.Rows.Count.ToString());
                writeToLog("Record Selection Complete");
            }
            catch (Exception ex)
            {
                writeToLog("ERROR: While selecting Records from Stored Procedure - GroupID: " + groupID + " CustomerID: " + customerID);
                writeToLog("ERROR: " + ex.ToString());
                throw;
            }

            return selectedEmailProfilesDT;
        }
        #endregion

        #region Method to Write into log file.
        public static void writeToLog(string text)
        {
            logFile.AutoFlush = true;
            logFile.WriteLine(DateTime.Now + " >> " + text);
            logFile.Flush();
        }
        #endregion


    }
}
