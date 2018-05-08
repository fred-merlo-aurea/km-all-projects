using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.OleDb;
using System.Collections;
using System.Security;
using System.Text.RegularExpressions;
namespace ECNTools.ECN_Import
{
    public partial class FPImport2 : Form
    {
        public FPImport2()
        {
            Main2.user = new KMPlatform.BusinessLogic.User().LogIn(Guid.Parse(ConfigurationManager.AppSettings["ecnAccessKey"].ToString()),false);
            InitializeComponent();
        }

        private static string Log = string.Empty;
        private static StreamWriter Logfile = null;
        private static DataTable DBIVDataTable = null;
        private static Hashtable hUpdatedRecords = new Hashtable();
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

            DBIVDataTable = CreateDataTableFromDBF(filePath, file);
            WriteToLog("* ColumnNames extraced from File");
            ExtractDataFromFileAndImport(DBIVDataTable);
            WriteToLog("-END-----------------------------------------");
            Logfile.Close();

        }

        //private object ExecuteScalar(string SQL)
        //{
        //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand(SQL, conn);
        //    cmd.CommandTimeout = 6000;
        //    cmd.Connection.Open();
        //    object obj = cmd.ExecuteScalar();
        //    conn.Close();
        //    return obj;
        //}

        public static int GetGroupIDByPubCode(string pubCode)
        {
            int groupID = -1;
            string query = "Select GroupID from FPDataImporter WHERE PublicationName = '" + pubCode + "'";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = ECN_Framework_DataLayer.DataFunctions.GetSqlConnection("KMPSInterim");
            cmd.Connection.Open();
            object id = cmd.ExecuteScalar();

            cmd.Connection.Close();
            cmd.Dispose();

            int.TryParse(id.ToString(), out groupID);

            return groupID;
        }

        private void ExtractDataFromFileAndImport(DataTable dBastDataTable)
        {
            WriteToLog("Ready to Import data in to ECN: ");
            StringBuilder xmlProfile = new StringBuilder();
            StringBuilder xmlUDF = new StringBuilder();
            SqlDateTime sqldatenull = SqlDateTime.Null;

            DateTime dtStartTime = System.DateTime.Now;

            hUpdatedRecords = new Hashtable();
            ECN_Framework_Entities.Communicator.Group pubGroup = null;
            int batchCount = 0;
            int EmailID = 0;
            string EmailAddress = "";
            int groupID = -1;
            string zip = "", zipPlus = "";
            int custID = 0;
            int linenumber = Convert.ToInt32(ConfigurationManager.AppSettings["LinenumberToStart"].ToString());
            try
            {
                //DataRow[] selectDBIVDataTableRows = dBastDataTable.Select("", "PUBCODE");
                DataRow[] selectDBIVDataTableRows = dBastDataTable.Select();

                //SqlConnection ecnConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString);

                //Action maxProgressBar = () => progressBar1.Maximum = selectDBIVDataTableRows.Length;
                //progressBar1.BeginInvoke(maxProgressBar);
                //Action minProgressBar = () => progressBar1.Minimum = 0;
                //progressBar1.BeginInvoke(minProgressBar);

                bool doneWithRecord = false;
                int dbLength = selectDBIVDataTableRows.Length;
                Regex lineFeed = new Regex(@"\\n|\\r\\n");
                for (int i = 0; i < selectDBIVDataTableRows.Length; i++)
                {
                    int copy = i;

                    if (backgroundWorker1.CancellationPending)
                    {
                        backgroundWorker1.ReportProgress(100, " Exiting..");
                        return;
                    }

                    double progress = (double)i / dbLength;
                    progress = progress * 100;
                    copy = Convert.ToInt32(progress);

                    //backgroundWorker1.ReportProgress(i / selectDBIVDataTableRows.Length, "Importing row " + i.ToString() + " of " + selectDBIVDataTableRows.Length.ToString());
                    backgroundWorker1.ReportProgress(copy, "");
                    string pubCde = selectDBIVDataTableRows[i]["PUBCODE"].ToString().Trim();
                    if (i == 0)
                    {
                        groupID = GetGroupIDByPubCode(pubCde);
                        pubGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(Convert.ToInt32(groupID));
                        custID = pubGroup.CustomerID;
                    }

                    if ((i + 1) > linenumber)
                    {
                        EmailAddress = selectDBIVDataTableRows[i]["EMAIL"].ToString().Trim();
                        EmailAddress = EmailAddress.Replace("'", "");
                        zip = selectDBIVDataTableRows[i]["ZIP"].ToString().Trim();
                        zipPlus = selectDBIVDataTableRows[i]["PLUS4"].ToString().Trim();
                        string Country = "";
                        Country = selectDBIVDataTableRows[i]["COUNTRY"].ToString().Trim();
                        if (Country.ToLower().Equals("ca") || Country.ToLower().Equals("canada"))
                        {
                            zip = zip + " " + zipPlus;
                        }
                        else if (string.IsNullOrEmpty(Country))
                        {
                            if (zipPlus.Length > 0)
                            {
                                zip = zip + "-" + zipPlus;
                            }
                        }
                        else
                        {
                            if (zipPlus.Length > 0)
                            {
                                zip = zip + zipPlus;
                            }
                        }

                        //Random rnd =new Random();
                        string GUID = "";
                        GUID = Guid.NewGuid().ToString();// DataFunctions.ExecuteScalar("SELECT GUID = NewID()").ToString();

                        if (EmailAddress.Length < 5)
                        {
                            EmailAddress = DateTime.Now.ToString("yyyyMMdd-HHmmss.fff") + "-" + GUID.Substring(0, 6) + "@" + pubCde + ".kmpsgroup.com";
                        }

                        for (; ; )
                        {
                            if (backgroundWorker1.CancellationPending)
                            {
                                backgroundWorker1.ReportProgress(100, " Exiting..");
                                return;
                            }
                            //ECN_Framework_Entities.Communicator.Email existingEmail = null;
                            ECN_Framework_Entities.Communicator.EmailGroup eg = null;
                            try
                            {

                                eg = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(EmailAddress, pubGroup.GroupID);

                                //existingEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(EmailAddress, pubGroup.CustomerID);
                                if (!(null == eg))
                                {
                                    WriteToLog(EmailAddress + " already Exists in this group. Creating a new Email.");
                                    GUID = Guid.NewGuid().ToString();// DataFunctions.ExecuteScalar("SELECT GUID = NewID()").ToString();
                                    EmailAddress = DateTime.Now.ToString("yyyyMMdd-HHmmss.fff") + "-" + GUID.Substring(0, 6) + "@" + pubCde + ".kmpsgroup.com";
                                }
                                else
                                {
                                    WriteToLog((i + 1) + ". PUBLICATION: " + pubCde + "; CUSTOMERID: " + custID + "; EMAIL: " + EmailAddress + "-----");
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteToLog("*Error looking up email address: " + EmailAddress + ". \n  Trying again.");
                                WriteToLog("* [ERROR]:Error occured while fetching data from the File. \n" + ex.ToString());

                            }
                        }

                        xmlProfile.Append("<Emails>");
                        xmlProfile.Append("<emailaddress>" + SecurityElement.Escape(EmailAddress) + "</emailaddress>");
                        xmlProfile.Append("<firstname>" + SecurityElement.Escape(selectDBIVDataTableRows[i]["FNAME"].ToString().Trim()) + "</firstname>");
                        xmlProfile.Append("<lastname>" + SecurityElement.Escape(selectDBIVDataTableRows[i]["LNAME"].ToString().Trim()) + "</lastname>");
                        xmlProfile.Append("<company>" + SecurityElement.Escape(selectDBIVDataTableRows[i]["COMPANY"].ToString().Trim()) + "</company>");
                        xmlProfile.Append("<occupation>" + SecurityElement.Escape(selectDBIVDataTableRows[i]["TITLE"].ToString().Trim()) + "</occupation>");
                        xmlProfile.Append("<address>" + lineFeed.Replace(SecurityElement.Escape(selectDBIVDataTableRows[i]["ADDRESS"].ToString().Trim()), " ") + "</address>");
                        xmlProfile.Append("<address2>" + lineFeed.Replace(SecurityElement.Escape(selectDBIVDataTableRows[i]["MAILSTOP"].ToString().Trim()), " ") + "</address2>");
                        xmlProfile.Append("<city>" + SecurityElement.Escape(selectDBIVDataTableRows[i]["CITY"].ToString().Trim()) + "</city>");
                        xmlProfile.Append("<state>" + SecurityElement.Escape(selectDBIVDataTableRows[i]["STATE"].ToString().Trim()) + "</state>");
                        xmlProfile.Append("<zip>" + zip + "</zip>");
                        xmlProfile.Append("<country>" + SecurityElement.Escape(selectDBIVDataTableRows[i]["COUNTRY"].ToString().Trim()) + "</country>");
                        xmlProfile.Append("<voice>" + SecurityElement.Escape(selectDBIVDataTableRows[i]["PHONE"].ToString().Trim()) + "</voice>");
                        xmlProfile.Append("<fax>" + SecurityElement.Escape(selectDBIVDataTableRows[i]["FAX"].ToString().Trim()) + "</fax>");
                        xmlProfile.Append("<website>" + SecurityElement.Escape(selectDBIVDataTableRows[i]["WEBSITE"].ToString().Trim()) + "</website>");
                        xmlProfile.Append("</Emails>");


                        WriteToLog(EmailAddress + " Inserted. ");

                        #region Old email import
                        //SqlCommand emailsInsertCommand = new SqlCommand();
                        //emailsInsertCommand.CommandText = "INSERT INTO Emails "
                        //    + "(EmailAddress,CustomerID,FirstName,LastName,Company,Occupation,Voice,Fax, Address,Address2,City,State,Zip,Country, Website)"
                        //    + " VALUES "
                        //    + "(@EmailAddress,@CustomerID,@FirstName,@LastName,@Company,@Occupation,@Voice,@Fax,@Address,@Address2,@City,@State,@Zip,@Country,@Website) "
                        //    + "SELECT @@IDENTITY ";
                        //emailsInsertCommand.CommandType = CommandType.Text;
                        //emailsInsertCommand.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 250).Value = EmailAddress;
                        //emailsInsertCommand.Parameters.Add("@CustomerID", SqlDbType.Int, 4).Value = custID;
                        //emailsInsertCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["FNAME"].ToString().Trim(); ;
                        //emailsInsertCommand.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["LNAME"].ToString().Trim();
                        //emailsInsertCommand.Parameters.Add("@Company", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["COMPANY"].ToString().Trim();
                        //emailsInsertCommand.Parameters.Add("@Occupation", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["TITLE"].ToString().Trim();
                        //emailsInsertCommand.Parameters.Add("@Address", SqlDbType.VarChar, 255).Value = selectDBIVDataTableRows[i]["ADDRESS"].ToString().Trim();
                        //emailsInsertCommand.Parameters.Add("@Address2", SqlDbType.VarChar, 255).Value = selectDBIVDataTableRows[i]["MAILSTOP"].ToString().Trim();
                        //emailsInsertCommand.Parameters.Add("@City", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["CITY"].ToString().Trim();
                        //emailsInsertCommand.Parameters.Add("@State", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["STATE"].ToString().Trim();
                        //emailsInsertCommand.Parameters.Add("@Zip", SqlDbType.VarChar, 50).Value = zip;
                        //emailsInsertCommand.Parameters.Add("@Country", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["COUNTRY"].ToString().Trim();
                        //emailsInsertCommand.Parameters.Add("@Voice", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["PHONE"].ToString().Trim();
                        //emailsInsertCommand.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["FAX"].ToString().Trim();
                        //emailsInsertCommand.Parameters.Add("@Website", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["WEBSITE"].ToString().Trim();

                        ////emailsInsertCommand.Connection.Open();
                        //EmailID = Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar(emailsInsertCommand, "ECNCommunicator").ToString());
                        ////emailsInsertCommand.Connection.Close();
                        //Emails newEmail = new Emails(EmailID);

                        //pubGroup.AttachEmail(newEmail, "html", "S");
                        #endregion

                        // The UDF's Insert & Update.. 

                        Hashtable UDFHash = GetGroupUDFHash(pubGroup.GroupID);

                        if (UDFHash.Count > 0)
                        {
                            IDictionaryEnumerator UDFHashEnumerator = UDFHash.GetEnumerator();
                            while (UDFHashEnumerator.MoveNext())
                            {
                                if (backgroundWorker1.CancellationPending)
                                {
                                    backgroundWorker1.ReportProgress(100, " Exiting..");
                                    return;
                                }
                                string UDFData = "";
                                string _value = "user_" + UDFHashEnumerator.Value.ToString();
                                string _key = UDFHashEnumerator.Key.ToString();
                                string user_UDFValue = "";
                                try
                                {
                                    if (UDFHashEnumerator.Value.ToString().ToUpper().Equals("SUBSCRIBERID"))
                                    {
                                        user_UDFValue = selectDBIVDataTableRows[i]["SEQUENCE"].ToString().Trim();
                                    }
                                    else if (UDFHashEnumerator.Value.ToString().ToUpper().Equals("PUBLICATIONCODE"))
                                    {
                                        user_UDFValue = selectDBIVDataTableRows[i]["PUBCODE"].ToString().Trim();
                                    }
                                    else
                                    {
                                        user_UDFValue = selectDBIVDataTableRows[i][UDFHashEnumerator.Value.ToString()].ToString().Trim();
                                    }
                                    if (user_UDFValue.Length > 0)
                                    {
                                        UDFData = user_UDFValue;
                                    }
                                    else
                                    {
                                        UDFData = "";
                                    }

                                    xmlUDF.Append("<row>");
                                    xmlUDF.Append("<ea>" + SecurityElement.Escape(EmailAddress)+ "</ea>");
                                    xmlUDF.Append("<udf id=\"" + _key + "\">");
                                    xmlUDF.Append("<v>" + SecurityElement.Escape(UDFData) + "</v>");
                                    xmlUDF.Append("</udf>");
                                    xmlUDF.Append("</row>");

                                    WriteToLog(UDFHashEnumerator.Value.ToString().ToUpper() + ": " + UDFData);
                                }
                                catch (Exception ex)
                                {
                                    // do Nothing - the field might not be in the DBF file.
                                    WriteToLog(UDFHashEnumerator.Value.ToString() + ": >>>>> NOT IN DBASE FILE <<<<< ");
                                    UDFData = "";
                                }
                            }
                            #region old UDF import
                            ////string GUID = "";
                            ////GUID = DataFunctions.ExecuteScalar("SELECT GUID = NewID()").ToString(); 

                            //string dfsID = null;
                            //for (int j = 0; j < _UDFData.Count; j++)
                            //{
                            //    dfsID = null;
                            //    //try{
                            //    //	dfsID = DataFunctions.ExecuteScalar("SELECT DataFieldSetID FROM GroupDataFields WHERE GroupDataFieldsID = "+_keyArrayList[j].ToString()).ToString();
                            //    //}catch(Exception){}
                            //    if (dfsID == null || dfsID.Length == 0)
                            //    {
                            //        pubGroup.AttachUDFToEmail(newEmail, _keyArrayList[j].ToString(), _UDFData[j].ToString());
                            //    }//else{
                            //    //	pubGroup.AttachUDFToEmail(newEmail,_keyArrayList[j].ToString(), _UDFData[j].ToString(),GUID);	
                            //    //}
                            //}
                            #endregion
                        }


                        //batch the import using importEmails

                        if ((batchCount != 0) && (batchCount % 100 == 0) || (batchCount == selectDBIVDataTableRows.Length - linenumber - 1))
                        {
                            try
                            {
                                backgroundWorker1.ReportProgress(copy, "Importing row - " + i.ToString() + " of " + selectDBIVDataTableRows.Length.ToString() + "(" + DateTime.Now.Subtract(dtStartTime).TotalSeconds.ToString("F2") + " secs)");
                                UpdateToDB(pubGroup, xmlProfile.ToString(), xmlUDF.ToString());
                                backgroundWorker1.ReportProgress(copy, "Importing complete - " + i.ToString() + " of " + selectDBIVDataTableRows.Length.ToString() + "(" + DateTime.Now.Subtract(dtStartTime).TotalSeconds.ToString("F2") + " secs)");

                                dtStartTime = System.DateTime.Now;

                                xmlProfile = new StringBuilder();
                                xmlUDF = new StringBuilder();
                            }
                            catch (Exception ex)
                            {
                                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "ECNTools.FPImport2.ExtractDataFromFileAndImport", -1);
                                WriteToLog("* [ERROR]:Error occured while writing to the DB. \n" + ex.ToString());
                                return;
                            }
                        }
                        batchCount++;

                } //end for Loop... go to the next record now.
                WriteToLog("* Import Results");
                foreach (DictionaryEntry de in hUpdatedRecords)
                {
                    WriteToLog(de.Key.ToString() + ": " + de.Value.ToString());
                }
                WriteToLog("* End Results");
            }
            }
            catch (Exception ex)
            {
                backgroundWorker1.ReportProgress(100, "***Error occured");
                WriteToLog("* [ERROR]:Error occured while fetching data from the File. \n" + ex.ToString());
            }
        }

        private Hashtable GetGroupUDFHash(int groupID)
        {
            Hashtable retHash = new Hashtable();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdf = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
            try
            {
                gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);
            }
            catch (Exception ex)
            {

            }

            int tries = 1;
            while ((gdf == null) && tries < 10)
            {
                try
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError("Error Getting Group Data Fields for : " + groupID, "ECN_Tools.FPImport.GetGroupUDFHash", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                    gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);
                }
                catch (Exception ex)
                {

                }
            }

            foreach (ECN_Framework_Entities.Communicator.GroupDataFields g in gdf)
            {
                retHash.Add(g.GroupDataFieldsID, g.ShortName);
            }
            return retHash;
        }

        private void UpdateToDB(ECN_Framework_Entities.Communicator.Group group, string xmlProfile, string xmlUDF)
        {
            DataTable dtResults = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(Main2.user, group.CustomerID, group.GroupID, 
                "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>",
                "HTML", "S", false, "", "ECNTools.ECN_Import.FPImport2.UpdateToDB");

            if (dtResults.Rows.Count > 0)
            {
                foreach (DataRow dr in dtResults.Rows)
                {
                    if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                        hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                    else
                    {
                        int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                        hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                    }
                }

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
                //connString = "Provider=Microsoft.JET.OLEDB.4.0;Extended Properties=dBASE IV;Data Source=" + filePath + ";";

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

        private void FPImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main2 frm = (Main2)this.MdiParent;
            frm.ToggleMenus(true);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ImportDataToDB();

            backgroundWorker1.ReportProgress(100, "End Time : " + System.DateTime.Now);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (e.UserState.ToString().Length > 0)
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
