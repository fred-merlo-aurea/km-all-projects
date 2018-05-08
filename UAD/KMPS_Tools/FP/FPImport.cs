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

namespace KMPS_Tools
{
    public partial class FPImport : Form
    {
        public FPImport()
        {
            InitializeComponent();
        }

        private static string Log = string.Empty;
        private static StreamWriter Logfile = null;
        private static DataTable DBIVDataTable = null;

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

        public static string GetGroupIDByPubCode(string pubCode)
        {
            string grpID = ConfigurationManager.AppSettings[pubCode].ToString();
            return grpID;
        }

        private void ExtractDataFromFileAndImport(DataTable dBastDataTable)
        {
            WriteToLog("Ready to Import data in to ECN: ");
            SqlDateTime sqldatenull = SqlDateTime.Null;
            Groups pubGroup = null;
            int EmailID = 0;
            string EmailAddress = "";
            string groupID = "", zip = "", zipPlus = "";
            int custID = 0;
            int linenumber = Convert.ToInt32(ConfigurationManager.AppSettings["LinenumberToStart"].ToString());
            try
            {
                //DataRow[] selectDBIVDataTableRows = dBastDataTable.Select("", "PUBCODE");
                DataRow[] selectDBIVDataTableRows = dBastDataTable.Select();

                //SqlConnection ecnConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString);

                Action maxProgressBar = () => progressBar1.Maximum = selectDBIVDataTableRows.Length;
                progressBar1.BeginInvoke(maxProgressBar);
                Action minProgressBar = () => progressBar1.Minimum = 0;
                progressBar1.BeginInvoke(minProgressBar);

                for (int i = 0; i < selectDBIVDataTableRows.Length; i++)
                {
                    int copy = i;
                    Action updateProgressBar = () => progressBar1.Value = copy;
                    progressBar1.BeginInvoke(updateProgressBar); 

                    string pubCde = selectDBIVDataTableRows[i]["PUBCODE"].ToString();
                    if (i == 0)
                    {
                        groupID = GetGroupIDByPubCode(pubCde);
                        pubGroup = new Groups(groupID);
                        custID = pubGroup.CustomerID();
                    }

                    if ((i + 1) > linenumber)
                    {
                        EmailAddress = selectDBIVDataTableRows[i]["EMAIL"].ToString().Trim();
                        EmailAddress = EmailAddress.Replace("'", "");
                        zip = selectDBIVDataTableRows[i]["ZIP"].ToString().Trim();
                        zipPlus = selectDBIVDataTableRows[i]["PLUS4"].ToString().Trim();
                        if (zipPlus.Length > 0)
                        {
                            zip = zip + "-" + zipPlus;
                        }

                        //Random rnd =new Random();
                        string GUID = "";
                        GUID = DataFunctions.ExecuteScalar("SELECT GUID = NewID()").ToString();

                        if (EmailAddress.Length < 5)
                        {
                            EmailAddress = DateTime.Now.ToString("yyyyMMdd-HHmmss.fff") + "-" + GUID.Substring(0, 6) + "@" + pubCde + ".kmpsgroup.com";
                        }

                        for (; ; )
                        {
                            Emails existingEmail = null;
                            existingEmail = pubGroup.WhatEmail(EmailAddress);
                            if (!(null == existingEmail))
                            {
                                WriteToLog(EmailAddress + " already Exists in this group. Creating a new Email.");
                                GUID = DataFunctions.ExecuteScalar("SELECT GUID = NewID()").ToString();
                                EmailAddress = DateTime.Now.ToString("yyyyMMdd-HHmmss.fff") + "-" + GUID.Substring(0, 6) + "@" + pubCde + ".kmpsgroup.com";
                            }
                            else
                            {
                                WriteToLog((i + 1) + ". PUBLICATION: " + pubCde + "; CUSTOMERID: " + custID + "; EMAIL: " + EmailAddress + "-----");
                                break;
                            }
                        }

                        SqlCommand emailsInsertCommand = new SqlCommand();
                        emailsInsertCommand.CommandText = "INSERT INTO Emails "
                            + "(EmailAddress,CustomerID,FirstName,LastName,Company,Occupation,Voice,Fax, Address,Address2,City,State,Zip,Country, Website)"
                            + " VALUES "
                            + "(@EmailAddress,@CustomerID,@FirstName,@LastName,@Company,@Occupation,@Voice,@Fax,@Address,@Address2,@City,@State,@Zip,@Country,@Website) "
                            + "SELECT @@IDENTITY ";
                        emailsInsertCommand.CommandType = CommandType.Text;
                        emailsInsertCommand.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 250).Value = EmailAddress;
                        emailsInsertCommand.Parameters.Add("@CustomerID", SqlDbType.Int, 4).Value = custID;
                        emailsInsertCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["FNAME"].ToString().Trim(); ;
                        emailsInsertCommand.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["LNAME"].ToString().Trim();
                        emailsInsertCommand.Parameters.Add("@Company", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["COMPANY"].ToString().Trim();
                        emailsInsertCommand.Parameters.Add("@Occupation", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["TITLE"].ToString().Trim();
                        emailsInsertCommand.Parameters.Add("@Address", SqlDbType.VarChar, 255).Value = selectDBIVDataTableRows[i]["ADDRESS"].ToString().Trim();
                        emailsInsertCommand.Parameters.Add("@Address2", SqlDbType.VarChar, 255).Value = selectDBIVDataTableRows[i]["MAILSTOP"].ToString().Trim();
                        emailsInsertCommand.Parameters.Add("@City", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["CITY"].ToString().Trim();
                        emailsInsertCommand.Parameters.Add("@State", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["STATE"].ToString().Trim();
                        emailsInsertCommand.Parameters.Add("@Zip", SqlDbType.VarChar, 50).Value = zip;
                        emailsInsertCommand.Parameters.Add("@Country", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["COUNTRY"].ToString().Trim();
                        emailsInsertCommand.Parameters.Add("@Voice", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["PHONE"].ToString().Trim();
                        emailsInsertCommand.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["FAX"].ToString().Trim();
                        emailsInsertCommand.Parameters.Add("@Website", SqlDbType.VarChar, 50).Value = selectDBIVDataTableRows[i]["WEBSITE"].ToString().Trim();

                        //emailsInsertCommand.Connection.Open();
                        EmailID = Convert.ToInt32(DataFunctions.ExecuteScalar(emailsInsertCommand).ToString());
                        //emailsInsertCommand.Connection.Close();
                        Emails newEmail = new Emails(EmailID);
                        WriteToLog(EmailAddress + " Inserted. ");
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
                                    WriteToLog(UDFHashEnumerator.Value.ToString().ToUpper() + ": " + UDFData);
                                }
                                catch (Exception ex)
                                {
                                    // do Nothing - the field might not be in the DBF file.
                                    WriteToLog(UDFHashEnumerator.Value.ToString() + ": >>>>> NOT IN DBASE FILE <<<<< ");
                                    UDFData = "";
                                }
                                _keyArrayList.Add(_key);
                                _UDFData.Add(UDFData);
                            }

                            //string GUID = "";
                            //GUID = DataFunctions.ExecuteScalar("SELECT GUID = NewID()").ToString(); 

                            string dfsID = null;
                            for (int j = 0; j < _UDFData.Count; j++)
                            {
                                dfsID = null;
                                //try{
                                //	dfsID = DataFunctions.ExecuteScalar("SELECT DataFieldSetID FROM GroupDataFields WHERE GroupDataFieldsID = "+_keyArrayList[j].ToString()).ToString();
                                //}catch(Exception){}
                                if (dfsID == null || dfsID.Length == 0)
                                {
                                    pubGroup.AttachUDFToEmail(newEmail, _keyArrayList[j].ToString(), _UDFData[j].ToString());
                                }//else{
                                //	pubGroup.AttachUDFToEmail(newEmail,_keyArrayList[j].ToString(), _UDFData[j].ToString(),GUID);	
                                //}
                            }
                        }
                    }
                } //end for Loop... go to the next record now.
            }
            catch (Exception ex)
            {
                WriteToLog("* [ERROR]:Error occured while fetching data from the File. \n" + ex.ToString());
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
    }
}
