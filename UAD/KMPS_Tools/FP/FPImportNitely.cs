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
using ecn.communicator.classes;
using ecn.common.classes;
using System.Net.Mail;

namespace KMPS_Tools
{
    public partial class FPImportNitely : Form
    {
        public FPImportNitely()
        {
            InitializeComponent();
            LoadListBox();
        }

        private static string Log = string.Empty;
        private static StreamWriter Logfile = null;
        private static DataTable DBIVDataTable = null;
        private static string Pubs = string.Empty;

        private static Emails _newEmailObj = null;
        public static Emails newEmailObj
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
            DataTable selectedPubsDT = DataFunctions.GetDataTable(selectPubList, ConfigurationManager.AppSettings["KMPS_ConnString"].ToString());

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
            DataTable selectedPubsDT = DataFunctions.GetDataTable(selectPubList, ConfigurationManager.AppSettings["KMPS_ConnString"].ToString());

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
            SqlDateTime sqldatenull = SqlDateTime.Null;
            int custID = 0;
            int linenumber = Convert.ToInt32(ConfigurationManager.AppSettings["LinenumberToStart"].ToString());
            try
            {
                //Add Code to Filter based on the LastDownloadDate. Check for XACTDATE between lastDownloadDate & TODAY
                DataRow[] selectDBIVDataTableRows = dBaseDataTable.Select();

                SqlConnection ecnConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString);
                Groups pubGroup = new Groups(groupID);
                custID = pubGroup.CustomerID();

                Action maxProgressBar = () => progressBar1.Maximum = selectDBIVDataTableRows.Length;
                progressBar1.BeginInvoke(maxProgressBar);
                Action minProgressBar = () => progressBar1.Minimum = 0;
                progressBar1.BeginInvoke(minProgressBar);

                for (int i = 0; i < selectDBIVDataTableRows.Length; i++)
                {
                    int copy = i;
                    Action updateProgressBar = () => progressBar1.Value = copy;
                    progressBar1.BeginInvoke(updateProgressBar); 

                    if ((i + 1) > linenumber)
                    {
                        WriteToLog("--- RECORD " + (i + 1) + " START ---");
                        string rowEmailAddress = "", rowSubscriberID = "", rowEmailID = "", rowPubCde = "", rowAltEMailAddress = "", rowProcessType = "";
                        bool OKToProcess = false;
                        rowPubCde = selectDBIVDataTableRows[i]["PUBCODE"].ToString();
                        rowEmailAddress = selectDBIVDataTableRows[i]["EMAIL"].ToString().Trim();
                        rowSubscriberID = selectDBIVDataTableRows[i]["SEQUENCE"].ToString().Trim();
                        rowEmailID = selectDBIVDataTableRows[i]["EMAILID"].ToString().Trim();
                        try
                        {
                            Convert.ToInt32(rowEmailID.ToString());
                        }
                        catch (Exception ex)
                        {
                            rowEmailID = "0";
                        }
                        int rowXACT = Convert.ToInt32(selectDBIVDataTableRows[i]["XACT"].ToString()); //get the TransactionType Code: 

                        //Is Blank EmailAddress ?
                        if (rowEmailAddress.Length < 5)
                        {
                            rowEmailAddress = getDummyEmailAddress(rowPubCde);
                            WriteToLog("Getting DUMMY EmailAddress: " + rowEmailAddress);
                        }

                        //Create an EmailObject with the EmailAddress for this Group.
                        newEmailObj = pubGroup.WhatEmail(rowEmailAddress);

                        //Check for EmailID & get SubscriberID depending on that.
                        CheckEmailAndSubscriberID(rowEmailID, rowSubscriberID, groupID);
                        WriteToLog("Match on Email / SubID ? : " + ECNEmailID_SubIDMatch);

                        // START PROCESSING FOR XACT = 10 -- NEW Subscriptions
                        if ((rowXACT == 10) || (rowXACT == 13) || (rowXACT == 14))
                        {
                            WriteToLog("XACTCODE = " + rowXACT + ". NEW SUBSCRIPTION");

                            //Check if there was a match on EmailID / SubID in ECN.
                            if (ECNEmailID_SubIDMatch)
                            { //YES MATCH. 
                                //There is atleast a Sub / EmailID Match for sure.
                                //Check to see if the EmailAddress Entered exists in a the Group.
                                if (newEmailObj == null)
                                {
                                    //There are no records with the Entered EmailAddress OKtoProcess is True.
                                    // Do an Update 'cos we have the EmailID already
                                    OKToProcess = true;
                                    rowProcessType = "NOTEXIST";
                                }
                                else
                                {
                                    //There is a record of this EmailAddress.. Might be a different record / the EmailAddress has not been Changed / updated.
                                    //Check to see if the EmailID's are the same !! If they are then its just an Update on other information. 
                                    if (newEmailObj.ID() == Convert.ToInt32(rowEmailID.ToString()))
                                    {
                                        OKToProcess = true;
                                        rowProcessType = "UPDATE";
                                    }
                                    else
                                    {
                                        // Here the EmailID for the newEmailObj belongs to some one else. 
                                        // so Let it go thru by using a Dummy Email Address & store the Duplicate emailAddress entered in the 
                                        // user_ALTERNATE_EMAILADDRESS UDF
                                        rowEmailAddress = getDummyEmailAddress(rowPubCde);
                                        OKToProcess = true;
                                        rowProcessType = "INSERT";
                                        rowAltEMailAddress = rowEmailAddress;
                                    }
                                }
                            }
                            else
                            { //NO MATCH
                                //There is no Match on either SubID / EmailID so its an Insert.. 
                                if (newEmailObj == null)
                                {
                                    //There are no records with the Entered EmailAddress OKtoProcess is True.
                                    // Do an INSERT 'cos we DON'T have the EmailID / SUBID
                                    OKToProcess = true;
                                    rowProcessType = "INSERT";
                                }
                                else
                                {
                                    //There is a record of this EmailAddress.. Might be a different record / the EmailAddress has not been Changed / updated.
                                    // Here the EmailID for the newEmailObj belongs to some one else. 
                                    // so Let it go thru by using a Dummy Email Address & store the Duplicate emailAddress entered in the 
                                    // user_ALTERNATE_EMAILADDRESS UDF
                                    rowEmailAddress = getDummyEmailAddress(rowPubCde);
                                    OKToProcess = true;
                                    rowProcessType = "INSERT";
                                    rowAltEMailAddress = rowEmailAddress;
                                }
                            }//END MATCH ON ECNEmailID_SubIDMatch 

                            //Ready to Process this Record:
                            if (rowProcessType.Equals("INSERT"))
                            { //Insert in to ECN
                                WriteToLog("EmailAddress: " + rowEmailAddress + " DOES NOT Exist in this group. REPORT ON NON EXISTING EMAIL / SUBID");
                                //InsertEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, selectDBIVDataTableRows[i]);
                                ReportNonExistEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, selectDBIVDataTableRows[i]);
                                string subject = "NON EXISTING EMAIL / SUBID - NEED TO BE INSERTED";
                                string body = "<P>PUBLICATION: " + rowPubCde + "<BR>" +
                                    "RECORD #: " + (i + 1) + "<BR>" +
                                    "EMAILADDRESS: " + rowEmailAddress + "<BR>" +
                                    "XACT: " + rowXACT + "<BR>" +
                                    "SEQUENCE ID: " + selectDBIVDataTableRows[i]["SEQUENCE"].ToString().Trim() + "<BR></P>";
                                NotifyAdmin(subject, body);
                                WriteToLog("PUBLICATION: " + rowPubCde + "; CUSTOMERID: " + custID + "; EMAIL: " + rowEmailAddress + "");
                                WriteToLog("--- RECORD " + (i + 1) + " END ---");
                            }
                            else if (rowProcessType.Equals("UPDATE"))
                            { //UPDATE in to ECN.
                                WriteToLog("EmailAddress: " + rowEmailAddress + " DOES Exist in this group. EmailID : " + newEmailObj.ID() + " - UPDATING");
                                UpdateEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, selectDBIVDataTableRows[i]);
                                WriteToLog("PUBLICATION: " + rowPubCde + "; CUSTOMERID: " + custID + "; EMAIL: " + rowEmailAddress + "");
                                WriteToLog("--- RECORD " + (i + 1) + " END ---");
                            }
                            else if (rowProcessType.Equals("NOTEXIST"))
                            {
                                WriteToLog("EmailAddress: " + rowEmailAddress + " DOES Exist in this group. EmailID : " + newEmailObj.ID() + " - UPDATING");
                                ReportNonExistEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, selectDBIVDataTableRows[i]);
                                WriteToLog("PUBLICATION: " + rowPubCde + "; CUSTOMERID: " + custID + "; EMAIL: " + rowEmailAddress + "");
                                WriteToLog("--- RECORD " + (i + 1) + " END ---");
                            }

                            //END PROCESSING for XACT = 10
                            //Start PROCESSING OTHER XACT's
                        }
                        else if ((rowXACT == 16) || (rowXACT == 17) || (rowXACT == 18) || (rowXACT == 19) || (rowXACT == 22) || (rowXACT == 23) ||
                           (rowXACT == 25) || (rowXACT == 27) || (rowXACT == 40) || (rowXACT == 41) || (rowXACT == 42) || (rowXACT == 43) ||
                           (rowXACT == 46) || (rowXACT == 47) || (rowXACT == 48) || (rowXACT == 49) || //RENEWALS
                           (rowXACT == 31) || (rowXACT == 32) || (rowXACT == 33) || (rowXACT == 34) || (rowXACT == 38) || (rowXACT == 39) ||
                           ((rowXACT > 59) && (rowXACT < 70)) || //CANCELS
                           (rowXACT == 99) || //ORIG CONVERSIONS
                           (rowXACT == 21) //ADDCHANGE ONLY
                           )
                        {
                            // OTHER TRANSACTIONS Other than NEW Subscriptions [RENEW, ADDCHANGE etc.,]
                            WriteToLog("XACTCODE = " + rowXACT + ". RENEW / ADDCHG / CANCEL SUBSCRIPTION");

                            //Check if there was a match on EmailID / SubID in ECN.
                            if (ECNEmailID_SubIDMatch)
                            { //YES MATCH. 
                                //There is atleast a Sub / EmailID Match for sure.
                                //Check to see if the EmailAddress Entered exists in a the Group.
                                if (newEmailObj == null)
                                {
                                    //There are no records with the Entered EmailAddress OKtoProcess is True.
                                    // Do an Update 'cos we have the EmailID already
                                    OKToProcess = true;
                                    rowProcessType = "NOTEXIST";
                                }
                                else
                                {
                                    //There is a record of this EmailAddress.. Might be a different record / the EmailAddress has not been Changed / updated.
                                    //Check to see if the EmailID's are the same !! If they are then its just an Update on other information. 
                                    if (newEmailObj.ID() > 0)
                                    { //== Convert.ToInt32(rowEmailID.ToString())){
                                        OKToProcess = true;
                                        rowProcessType = "UPDATE";
                                    }
                                    else
                                    {
                                        // Here the EmailID for the newEmailObj belongs to some one else. 
                                        // so Let it go thru by using a Dummy Email Address & store the Duplicate emailAddress entered in the 
                                        // user_ALTERNATE_EMAILADDRESS UDF
                                        rowEmailAddress = getDummyEmailAddress(rowPubCde);
                                        OKToProcess = true;
                                        rowProcessType = "INSERT";
                                        rowAltEMailAddress = rowEmailAddress;
                                    }
                                }

                                //Ready to Process this Record:
                                if (rowProcessType.Equals("INSERT"))
                                { //Insert in to ECN
                                    WriteToLog("EmailAddress: " + rowEmailAddress + " DOES NOT Exist in this group. REPORT ON NON EXISTING EMAIL / SUBID");
                                    //InsertEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, selectDBIVDataTableRows[i]);
                                    ReportNonExistEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, selectDBIVDataTableRows[i]);
                                    string subject = "NON EXISTING EMAIL / SUBID - NEED TO BE INSERTED";
                                    string body = "<P>PUBLICATION: " + rowPubCde + "<BR>" +
                                        "RECORD #: " + (i + 1) + "<BR>" +
                                        "EMAILADDRESS: " + rowEmailAddress + "<BR>" +
                                        "XACT: " + rowXACT + "<BR>" +
                                        "SEQUENCE ID: " + selectDBIVDataTableRows[i]["SEQUENCE"].ToString().Trim() + "<BR></P>";
                                    NotifyAdmin(subject, body);
                                    WriteToLog("PUBLICATION: " + rowPubCde + "; CUSTOMERID: " + custID + "; EMAIL: " + rowEmailAddress + "");
                                    WriteToLog("--- RECORD " + (i + 1) + " END ---");
                                }
                                else if (rowProcessType.Equals("UPDATE"))
                                { //UPDATE in to ECN.
                                    WriteToLog("EmailAddress: " + rowEmailAddress + " DOES Exist in this group. EmailID : " + newEmailObj.ID() + " - UPDATING");
                                    UpdateEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, selectDBIVDataTableRows[i]);
                                    WriteToLog("PUBLICATION: " + rowPubCde + "; CUSTOMERID: " + custID + "; EMAIL: " + rowEmailAddress + "");
                                    WriteToLog("--- RECORD " + (i + 1) + " END ---");
                                }
                                else if (rowProcessType.Equals("NOTEXIST"))
                                {
                                    WriteToLog("EmailAddress: " + rowEmailAddress + " DOES Exist in this group. EmailID : " + newEmailObj.ID() + " - UPDATING");
                                    ReportNonExistEmailProfile(rowEmailAddress, pubGroup, custID, rowAltEMailAddress, selectDBIVDataTableRows[i]);
                                    WriteToLog("PUBLICATION: " + rowPubCde + "; CUSTOMERID: " + custID + "; EMAIL: " + rowEmailAddress + "");
                                    WriteToLog("--- RECORD " + (i + 1) + " END ---");
                                }
                            }
                            else
                            { //NO MATCH 
                                //Notify Admin about an UNRECOGNIZED XACT Code.
                                WriteToLog(">>>>> NO MATCH on EMAIL / SUBSCRIBERID for this Record. Email Sent to Admins <<<<< ");
                                string subject = "NO MATCH on EMAIL / SUBSCRIBERID";
                                string body = "<P>PUBLICATION: " + rowPubCde + "<BR>" +
                                    "RECORD #: " + (i + 1) + "<BR>" +
                                    "EMAILADDRESS: " + rowEmailAddress + "<BR>" +
                                    "XACT: " + rowXACT + "<BR>" +
                                    "SEQUENCE ID: " + selectDBIVDataTableRows[i]["SEQUENCE"].ToString().Trim() + "<BR></P>";
                                NotifyAdmin(subject, body);
                            }

                            //END PROCESSING OTHER XACT's
                        }
                        else
                        {
                            //Notify Admin about an UNRECOGNIZED XACT Code.
                            WriteToLog(">>>>> UNRECOGNIZED XACT Code for this Record. Email Sent to Admins <<<<< ");
                            string subject = "UNRECOGNIZED XACT Code";
                            string body = "<P>PUBLICATION: " + rowPubCde + "<BR>" +
                                "RECORD #: " + (i + 1) + "<BR>" +
                                "EMAILADDRESS: " + rowEmailAddress + "<BR>" +
                                "XACT: " + rowXACT + "<BR>" +
                                "SEQUENCE ID: " + selectDBIVDataTableRows[i]["SEQUENCE"].ToString().Trim() + "<BR></P>";
                            NotifyAdmin(subject, body);
                        }
                    }
                    

                } //end for Loop... go to the next record now.
            }
            catch (Exception ex)
            {
                WriteToLog("* [ERROR]:Error occured while fetching data from the File" + ex.ToString());
            }
        }

        private void UpdateEmailProfile(string EmailAddress, Groups pubGroup, int custID, string AlternateEmail, DataRow selectDBIVDataTableRows)
        {
            int emailID = 0;
            try
            {
                Convert.ToInt32(selectDBIVDataTableRows["EMAILID"].ToString().Trim());
            }
            catch (Exception ex)
            {
                emailID = 0;
            }
            if (newEmailObj.ID() > 0)
            {
                string zip = selectDBIVDataTableRows["ZIP"].ToString().Trim();
                string zipPlus = selectDBIVDataTableRows["PLUS4"].ToString().Trim();
                if (zipPlus.Length > 0)
                {
                    zip = zip + "-" + zipPlus;
                }
                SqlConnection ecnConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString);
                SqlCommand emailsUpdateCommand = new SqlCommand(null, ecnConnection);
                emailsUpdateCommand.CommandText = "UPDATE Emails SET " +
                    " EmailAddress=@emailAddress, CustomerID=@CustomerID, Title = @title, FirstName = @FirstName, LastName = @LastName, Company = @Company," +
                    " Occupation = @Occupation, Address = @Address, Address2 = @Address2, City = @City, State = @State, Zip = @Zip, Country = @Country, " +
                    " Voice = @Voice, Fax = @Fax, Website = @Website " +
                    " WHERE EmailID = @EmailID;";

                emailsUpdateCommand.Parameters.Add("@EmailID", SqlDbType.Int).Value = newEmailObj.ID();
                emailsUpdateCommand.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 250).Value = EmailAddress;
                emailsUpdateCommand.Parameters.Add("@CustomerID", SqlDbType.Int, 4).Value = custID;
                emailsUpdateCommand.Parameters.Add("@Title", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows["TITLE"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows["FNAME"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows["LNAME"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@Company", SqlDbType.VarChar, 100).Value = selectDBIVDataTableRows["COMPANY"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@Occupation", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows["TITLE"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@Address", SqlDbType.VarChar, 255).Value = selectDBIVDataTableRows["ADDRESS"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@Address2", SqlDbType.VarChar, 255).Value = selectDBIVDataTableRows["MAILSTOP"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@City", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows["CITY"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@State", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows["STATE"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@Zip", SqlDbType.VarChar, 50).Value = zip;
                emailsUpdateCommand.Parameters.Add("@Country", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows["COUNTRY"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@Voice", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows["PHONE"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows["FAX"].ToString().Trim();
                emailsUpdateCommand.Parameters.Add("@Website", SqlDbType.VarChar).Value = selectDBIVDataTableRows["WEBSITE"].ToString().Trim();

                ecnConnection.Open();
                emailsUpdateCommand.ExecuteNonQuery();
                ecnConnection.Close();
                Emails newEmail = newEmailObj;
                WriteToLog(EmailAddress + " UPDATED. ");
                pubGroup.AttachEmail(newEmail, "html", "S");

                // The UDF's Insert & Update.. 
                ArrayList _keyArrayList = new ArrayList();
                ArrayList _UDFData = new ArrayList();
                SortedList UDFHash = pubGroup.UDFHash;

                if (UDFHash.Count > 0)
                {
                    IDictionaryEnumerator UDFHashEnumerator = UDFHash.GetEnumerator();
                    while (UDFHashEnumerator.MoveNext())
                    {
                        string UDFData = "";
                        string _value = "user_" + UDFHashEnumerator.Value.ToString();
                        string _key = UDFHashEnumerator.Key.ToString();
                        string user_UDFValue = "";
                        try
                        {
                            if (UDFHashEnumerator.Value.ToString().ToUpper().Equals("SUBSCRIBERID"))
                            {
                                user_UDFValue = selectDBIVDataTableRows["SEQUENCE"].ToString().Trim();
                            }
                            else if (UDFHashEnumerator.Value.ToString().ToUpper().Equals("PUBLICATIONCODE"))
                            {
                                user_UDFValue = selectDBIVDataTableRows["PUBCODE"].ToString().Trim();
                            }
                            else if (UDFHashEnumerator.Value.ToString().ToUpper().Equals("ALTERNATE_EMAILADDRESS"))
                            {
                                user_UDFValue = AlternateEmail;
                            }
                            else
                            {
                                user_UDFValue = selectDBIVDataTableRows[UDFHashEnumerator.Value.ToString()].ToString().Trim();
                            }
                            if (user_UDFValue.Length > 0)
                            {
                                UDFData = user_UDFValue;
                            }
                            else
                            {
                                UDFData = "";
                            }
                            WriteToLog(UDFHashEnumerator.Value.ToString().ToUpper() + ": " + UDFData);
                        }
                        catch (Exception ex)
                        {
                            // do Nothing - the field might not be in the DBF file.
                            WriteToLog(UDFHashEnumerator.Value.ToString() + ": >>>>> NOT IN DBASE FILE <<<<< ");
                            UDFData = "";
                        }
                        if (!(UDFHashEnumerator.Value.ToString().Trim().ToUpper().Equals("DEMO39")))
                        {
                            _keyArrayList.Add(_key);
                            _UDFData.Add(UDFData);
                        }
                        else
                        {
                            WriteToLog("::" + UDFHashEnumerator.Value.ToString().Trim().ToUpper() + ":: >>>>> IS IGNORED <<<<<");
                        }
                    }

                    string GUID = "";
                    GUID = DataFunctions.ExecuteScalar("SELECT GUID = NewID()").ToString();

                    string dfsID = null;
                    for (int j = 0; j < _UDFData.Count; j++)
                    {
                        dfsID = null;
                        try
                        {
                            dfsID = DataFunctions.ExecuteScalar("SELECT DataFieldSetID FROM GroupDataFields WHERE GroupDataFieldsID = " + _keyArrayList[j].ToString()).ToString();
                        }
                        catch (Exception) { }
                        if (dfsID == null || dfsID.Length == 0)
                        {
                            pubGroup.AttachUDFToEmail(newEmail, _keyArrayList[j].ToString(), _UDFData[j].ToString());
                        }
                        else
                        {
                            pubGroup.AttachUDFToEmail(newEmail, _keyArrayList[j].ToString(), _UDFData[j].ToString(), GUID);
                        }
                    }
                }
            }
            else
            {
                WriteToLog(" >>>>>> EMAILID = 0 in Update <<<<<<<");
                WriteToLog("PUBCODE: " + selectDBIVDataTableRows["PUBCODE"].ToString());
                WriteToLog("EMAIL: " + selectDBIVDataTableRows["EMAIL"].ToString().Trim());
                WriteToLog("SEQUENCE: " + selectDBIVDataTableRows["SEQUENCE"].ToString().Trim());
                WriteToLog("EMAILID: " + selectDBIVDataTableRows["EMAILID"].ToString().Trim());
                WriteToLog(" >>>>>>>>>>>>>>><<<<<<<<<<<<<<<<");
            }
        }

        private void ReportNonExistEmailProfile(string EmailAddress, Groups pubGroup, int custID, string AlternateEmail, DataRow selectDBIVDataTableRows)
        {
            WriteToLog(" >>>>>> REPORT NON EXISTANT EMAIL PROFILE <<<<<<<");
            WriteToLog("PUBCODE: " + selectDBIVDataTableRows["PUBCODE"].ToString());
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
            else if (Convert.ToInt32(rowSubscriberID.ToString()) > 0)
            {
                //Get SubscriptionID from the current Row Using a subscriberID from the ECNDB.
                getECNSubscriberID(rowSubscriberID, groupID, "SubIDSearch");
            }
        }

        private string getECNSubscriberID(string ID, string groupID, string searchType)
        {
            string ecnSubscriberID = "", subIDSelectSQL = "";
            if (searchType.Equals("EmailSearch"))
            {
                subIDSelectSQL = " SELECT edv.EmailID, edv.DataValue FROM " +
                    " GroupDataFields gdf JOIN Groups g on gdf.GroupID = g.GroupID JOIN " +
                    " EmailDataValues edv ON gdf.GroupDatafieldsID = edv.GroupDatafieldsID " +
                    " WHERE g.GroupID = " + groupID + " AND " +
                    " gdf.ShortName like 'SUBSCRIBERID' AND " +
                    " edv.EmailID = " + ID;
            }
            else if (searchType.Equals("SubIDSearch"))
            {
                subIDSelectSQL = " SELECT edv.EmailID, edv.DataValue FROM " +
                    " GroupDataFields gdf JOIN Groups g on gdf.GroupID = g.GroupID JOIN " +
                    " EmailDataValues edv ON gdf.GroupDatafieldsID = edv.GroupDatafieldsID " +
                    " WHERE g.GroupID = " + groupID + " AND " +
                    " gdf.ShortName like 'SUBSCRIBERID' AND " +
                    " edv.DataValue = '" + ID + "'";
            }
            try
            {
                DataTable dt = DataFunctions.GetDataTable(subIDSelectSQL);
                if (dt.Rows.Count > 0)
                {
                    ECNEmailID_SubIDMatch = true;
                    foreach (DataRow dr in dt.Rows)
                    {
                        ECNEmailID = dr["EmailID"].ToString();
                        ECNSubscriberID = dr["DataValue"].ToString();
                        break;
                    }
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
                newEmailObj = new Emails(ECNEmailID);
            }

            return ecnSubscriberID;
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
            MailMessage msg = new MailMessage(ConfigurationManager.AppSettings["Admin_FromEmail"].ToString(), ConfigurationManager.AppSettings["Admin_ToEmail"].ToString());
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(msg);
        }

        private DataTable CreateDataTableFromDBF(string filePath, string file)
        {
            WriteToLog("* Filepath: " + filePath + file);
            string connString = "";
            DataTable dt = null;
            try
            {
                DataSet dataset;

                try
                {
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=dBASE IV;Data Source=" + filePath + ";";
                    OleDbDataAdapter oleAdapter = new OleDbDataAdapter("SELECT * FROM " + file + "", connString);
                    dataset = new DataSet(file);
                    oleAdapter.Fill(dataset); //fill the adapter with rows only from the linenumber specified				
                    oleAdapter.Dispose();
                }
                catch
                {
                    try
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties=dBASE IV;Data Source=" + filePath + ";";
                        OleDbDataAdapter oleAdapter = new OleDbDataAdapter("SELECT * FROM " + file + "", connString);
                        dataset = new DataSet(file);
                        oleAdapter.Fill(dataset); //fill the adapter with rows only from the linenumber specified				
                        oleAdapter.Dispose();
                    }
                    catch (Exception ex1)
                    {
                        MessageBox.Show("[ERROR]: " + ex1);
                        throw new Exception("OLEDB driver not found.");
                    }
                }

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

        private void FPImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main frm = (Main)this.MdiParent;
            frm.ToggleMenus(true);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ImportDataToDB();

            backgroundWorker1.ReportProgress(100, "End Time : " + System.DateTime.Now);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
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

            if(comboBox1.SelectedItem.ToString() != "")
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
            Main frm = (Main)this.MdiParent;
            frm.ToggleMenus(true);
        }
    }
}
