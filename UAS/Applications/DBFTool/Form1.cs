using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBFtoUAD_Circ_Migration
{
    public partial class Form1 : Form
    {
        private string clientConn = "";
        private string pubCode = "";
        private int pubID = 0;
        private DataTable demoTable = new DataTable();
        private List<string> profileFields = new List<string>();
        private List<string> mappedProfileFields = new List<string>();
        private List<string> demographics = new List<string>();
        private List<string> adHocs = new List<string>();
        private string processCode = "";
        private string paidProcessCode = "";
        private int sourceFileID = -1;

        public Form1()
        {
            InitializeComponent();

            processCode = "DBF_" + Guid.NewGuid().ToString();
            paidProcessCode = "DBF_" + Guid.NewGuid().ToString();
            sourceFileID = -921;

            demoTable.Columns.Add("PubID", typeof(int));
            demoTable.Columns.Add("SORecordIdentifier", typeof(Guid));
            demoTable.Columns.Add("STRecordIdentifier", typeof(Guid));
            demoTable.Columns.Add("MAFField", typeof(string));
            demoTable.Columns.Add("Value", typeof(string));
            demoTable.Columns.Add("NotExists", typeof(bool));
            demoTable.Columns.Add("NotExistReason", typeof(string));
            demoTable.Columns.Add("DateCreated", typeof(DateTime));
            demoTable.Columns.Add("CreatedByUserID", typeof(int));
            demoTable.Columns.Add("DemographicUpdateCodeId", typeof(int));
            demoTable.Columns.Add("IsAdhoc", typeof(bool));
            demoTable.Columns.Add("ResponseOther", typeof(string));
        }

        private bool handleSelection = true;
        private void Form1_Load(object sender, EventArgs e)
        {            
            cbClient.DataSource = DataFunctions.getDataTable("select clientID, clientname, clientlivedbconnectionstring from client where isnull(clientname,'') <> '' and isams = 1 order by clientname asc", new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["KMPlatform"].ConnectionString));
            handleSelection = false;
        }

        private void cbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbClient.SelectedItem != null && handleSelection == false)
            {
                DataRowView dr = cbClient.SelectedItem as DataRowView;
                clientConn = dr["ClientLiveDBConnectionString"].ToString();
                cbPub.DataSource = DataFunctions.getDataTable("select PubID, PubCode, PubName from Pubs where isactive = 1 order by PubCode asc", new SqlConnection(clientConn));
            }
        }

        private void cbPub_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbPub.SelectedItem != null)
            {
                DataRowView dr = cbPub.SelectedItem as DataRowView;
                pubCode = cbPub.SelectedValue.ToString();
                pubID = int.Parse(dr["PubID"].ToString());
            }
        }

        private void btnChooseLogFile_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtDBFFileName.Text = Path.GetFileName(openFileDialog1.FileName);
                lstMessage.Items.Add("* Filepath: " + openFileDialog1.FileName);

                if (txtDBFFileName.Text == string.Empty || !txtDBFFileName.Text.EndsWith("dbf", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Select DBF File");
                }
            }
        }
      
        private void btnStart_Click(object sender, EventArgs e)
        {
            lstMessage.Items.Clear();

            if (cbClient.SelectedValue == null)
            {
                MessageBox.Show("Select Client");
                return;
            }

            lstMessage.Items.Add("* Client: " + cbClient.Text);

            if (cbPub.SelectedValue == null)
            {
                MessageBox.Show("Select Pub");
                return;
            }
            lstMessage.Items.Add("* Pub: " + cbPub.Text);

            if (txtDBFFileName.Text == string.Empty || !txtDBFFileName.Text.EndsWith("dbf", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Select DBF File");
                return;
            }
            lstMessage.Items.Add("* Filepath: " + openFileDialog1.FileName);

            string pubcode = Path.GetFileNameWithoutExtension(txtDBFFileName.Text).ToLower().Replace("sub", "");
            if (cbPub.SelectedValue == null || !cbPub.SelectedValue.ToString().ToLower().Equals(pubcode.ToLower()))
            {
                MessageBox.Show("File selected not matching the selected pub");
                return;
            }

            var confirmResult = MessageBox.Show("Are you sure you want to clear all records & import the file??",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                btnStart.Enabled = false;

                lstMessage.Items.Add("Start Time : " + System.DateTime.Now);

                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker1.ReportProgress(1, "Cleaning up records from UAD for Pub : " + pubCode);
            DataFunctions.execute_remove_by_pubcode(pubCode, new SqlConnection(clientConn));
            ImporttoDBFSub();

            backgroundWorker1.ReportProgress(100, "End Time : " + System.DateTime.Now);
        }
        
        public void ImporttoDBFSub()
        {
            DataTable dt = new DataTable();

            string fileNameNoExt = Path.GetFileNameWithoutExtension(txtDBFFileName.Text);

            try
            {               
                backgroundWorker1.ReportProgress(2, "Done cleaning UAD for Pub : " + pubCode);

                DataTable cats = DataFunctions.getDataTable("select cc.CategoryCodeValue FROM UAD_Lookup..CategoryCode cc join UAD_Lookup..CategoryCodeType cct on cc.CategoryCodeTypeID = cct.CategoryCodeTypeID where cct.IsFree = 0", new SqlConnection(clientConn));

                OleDbConnection fileConn = new OleDbConnection(@"Provider=VFPOLEDB.1;Data Source=" + openFileDialog1.FileName);
                StringBuilder strInsertXML = new StringBuilder();
                StringBuilder demoInsertXML = new StringBuilder();
                Dictionary<string, int> dQSource = DataFunctions.getQSource(new SqlConnection(clientConn));

                // Open the connection, and if open successfully, you can try to query it
                fileConn.Open();

                if (fileConn.State == ConnectionState.Open)
                {
                    backgroundWorker1.ReportProgress(3, "Reading in File: " + fileNameNoExt);
                    string mySQL = "SELECT * FROM [" + fileNameNoExt + "]";

                    OleDbCommand MyQuery = new OleDbCommand(mySQL, fileConn);
                    OleDbDataAdapter da = new OleDbDataAdapter(MyQuery);
                    OleDbDataReader dr;        

                    da.FillSchema(dt, SchemaType.Mapped);
                    da.Dispose();

                    DataTable others = DataFunctions.getDataTable("select distinct * from (select CustomField FROM subscriptionsextensionmapper where Active = 1 union " +
                    "select CustomField FROM PubSubscriptionsExtensionMapper where Active = 1 AND PubID = " + pubID + ") e"
                    , new SqlConnection(clientConn));
                    DataTable responses = DataFunctions.getDataTable("select ResponseGroupName from ResponseGroups where IsActive = 1 AND PubID = " + pubID, new SqlConnection(clientConn));
                    DataTable fields = DataFunctions.getDataTable("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SubscriberTransformed'", new SqlConnection(clientConn));
                    foreach (DataColumn dc in dt.Columns)
                    {
                        FindField(dc.ColumnName, others, responses, fields);
                    }

                    dr = MyQuery.ExecuteReader();
                    int count = 0;

                    while (dr.Read())
                    {
                        #region Reading File & Sending Records
                        count++;
                        strInsertXML.Append("<SUBSCRIBER>");

                        bool paid = false;
                        string code = "";
                        foreach (DataRow inn in cats.Rows)
                        {
                            if (dr["CAT"] == inn[0])
                            {
                                paid = true;
                                code = paidProcessCode;
                            }
                        }
                        if (!paid)
                            code = processCode;

                        string stRecordID = Guid.NewGuid().ToString();
                        string soRecordID = Guid.NewGuid().ToString();

                        #region Profile
                        foreach (string s in profileFields)
                        {
                            try
                            {   
                                string data = dr[s].ToString().Trim();

                                if (data.Length > 0 && s != "SORecordIdentifier" && s != "STRecordIdentifier" && s != "SourceFileID" && s != "ProcessCode")
                                {
                                    switch (s.ToUpper())
                                    {
                                        case "QSOURCE":
                                        {
                                            strInsertXML.Append("<QSOURCE>" + System.Security.SecurityElement.Escape(dQSource[data].ToString()) + "</QSOURCE>");
                                            break;
                                        }
                                        case "XACTDATE":
                                        case "BIRTHDATE":
                                        case "QDATE":
                                        case "EXPIRE":
                                        {
                                            strInsertXML.Append("<" + s.ToUpper() + ">" + System.Security.SecurityElement.Escape(String.Format("{0:MM/dd/yyyy}", data)) + "</" + s.ToUpper() + ">");
                                            break;
                                        }
                                        default:
                                        {
                                            strInsertXML.Append("<" + s.ToUpper() + ">" + System.Security.SecurityElement.Escape(
                                                System.Text.RegularExpressions.Regex.Replace(Convert.ToString(data), @"[^\u0000-\u007F]", "")) + "</" + s.ToUpper() + ">");
                                            break;
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                        #endregion
                        #region Demos
                        foreach (string s in demographics)
                        {
                            if (dr[s].ToString().Contains(","))
                            {
                                string[] values = dr[s].ToString().Split(',');
                                foreach (string z in values.ToList())
                                {
                                    demoInsertXML.Append("<SUBSCRIBER>");
                                    demoInsertXML.Append("<SORECORDIDENTIFIER>" + System.Security.SecurityElement.Escape(soRecordID) + "</SORECORDIDENTIFIER>");
                                    demoInsertXML.Append("<STRECORDIDENTIFIER>" + System.Security.SecurityElement.Escape(stRecordID) + "</STRECORDIDENTIFIER>");
                                    demoInsertXML.Append("<PUBID>" + pubID.ToString() + "</PUBID>");
                                    demoInsertXML.Append("<MAFFIELD>" + s + "</MAFFIELD>");
                                    demoInsertXML.Append("<VALUE>" + z + "</VALUE>");
                                    demoInsertXML.Append("<NOTEXISTS>" + false + "</NOTEXISTS>");
                                    demoInsertXML.Append("<NOTEXISTREASON>" + "" + "</NOTEXISTREASON>");
                                    demoInsertXML.Append("<DATECREATED>" + DateTime.Now + "</DATECREATED>");
                                    demoInsertXML.Append("<CREATEDBYUSERID>" + 1 + "</CREATEDBYUSERID>");
                                    demoInsertXML.Append("<DEMOGRAPHICUPDATECODEID>" + 0 + "</DEMOGRAPHICUPDATECODEID>");
                                    demoInsertXML.Append("<ISADHOC>" + false + "</ISADHOC>");
                                    demoInsertXML.Append("<RESPONSEOTHER>" + "" + "</RESPONSEOTHER>");
                                    demoInsertXML.Append("</SUBSCRIBER>");
                                }
                            }
                            else if (!string.IsNullOrEmpty(dr[s].ToString()))
                            {
                                demoInsertXML.Append("<SUBSCRIBER>");
                                demoInsertXML.Append("<SORECORDIDENTIFIER>" + System.Security.SecurityElement.Escape(soRecordID) + "</SORECORDIDENTIFIER>");
                                demoInsertXML.Append("<STRECORDIDENTIFIER>" + System.Security.SecurityElement.Escape(stRecordID) + "</STRECORDIDENTIFIER>");
                                demoInsertXML.Append("<PUBID>" + pubID.ToString() + "</PUBID>");
                                demoInsertXML.Append("<MAFFIELD>" + s + "</MAFFIELD>");
                                demoInsertXML.Append("<VALUE>" + dr[s] + "</VALUE>");
                                demoInsertXML.Append("<NOTEXISTS>" + false + "</NOTEXISTS>");
                                demoInsertXML.Append("<NOTEXISTREASON>" + "" + "</NOTEXISTREASON>");
                                demoInsertXML.Append("<DATECREATED>" + DateTime.Now + "</DATECREATED>");
                                demoInsertXML.Append("<CREATEDBYUSERID>" + 1 + "</CREATEDBYUSERID>");
                                demoInsertXML.Append("<DEMOGRAPHICUPDATECODEID>" + 0 + "</DEMOGRAPHICUPDATECODEID>");
                                demoInsertXML.Append("<ISADHOC>" + false + "</ISADHOC>");
                                demoInsertXML.Append("<RESPONSEOTHER>" + "" + "</RESPONSEOTHER>");
                                demoInsertXML.Append("</SUBSCRIBER>");
                            }
                        }
                        #endregion
                        #region AdHocs
                        foreach (string s in adHocs)
                        {
                            if (!string.IsNullOrEmpty(dr[s].ToString()))
                            {
                                demoInsertXML.Append("<SUBSCRIBER>");
                                demoInsertXML.Append("<SORECORDIDENTIFIER>" + System.Security.SecurityElement.Escape(soRecordID) + "</SORECORDIDENTIFIER>");
                                demoInsertXML.Append("<STRECORDIDENTIFIER>" + System.Security.SecurityElement.Escape(stRecordID) + "</STRECORDIDENTIFIER>");
                                demoInsertXML.Append("<PUBID>" + pubID.ToString() + "</PUBID>");
                                demoInsertXML.Append("<MAFFIELD>" + s + "</MAFFIELD>");
                                demoInsertXML.Append("<VALUE>" + dr[s] + "</VALUE>");
                                demoInsertXML.Append("<NOTEXISTS>" + false + "</NOTEXISTS>");
                                demoInsertXML.Append("<NOTEXISTREASON>" + "" + "</NOTEXISTREASON>");
                                demoInsertXML.Append("<DATECREATED>" + DateTime.Now + "</DATECREATED>");
                                demoInsertXML.Append("<CREATEDBYUSERID>" + 1 + "</CREATEDBYUSERID>");
                                demoInsertXML.Append("<DEMOGRAPHICUPDATECODEID>" + 0 + "</DEMOGRAPHICUPDATECODEID>");
                                demoInsertXML.Append("<ISADHOC>" + true + "</ISADHOC>");
                                demoInsertXML.Append("<RESPONSEOTHER>" + "" + "</RESPONSEOTHER>");
                                demoInsertXML.Append("</SUBSCRIBER>");
                            }
                        }
                        #endregion
                        strInsertXML.Append("<SORECORDIDENTIFIER>" + System.Security.SecurityElement.Escape(soRecordID) + "</SORECORDIDENTIFIER>");
                        strInsertXML.Append("<STRECORDIDENTIFIER>" + System.Security.SecurityElement.Escape(stRecordID) + "</STRECORDIDENTIFIER>");
                        strInsertXML.Append("<SOURCEFILEID>" + System.Security.SecurityElement.Escape(sourceFileID.ToString()) + "</SOURCEFILEID>");
                        strInsertXML.Append("<PROCESSCODE>" + System.Security.SecurityElement.Escape(code.ToString()) + "</PROCESSCODE>");
                        strInsertXML.Append("</SUBSCRIBER>");

                        if (count % 1000 == 0)
                        {
                            DateTime beforeImport = DateTime.Now;
                            backgroundWorker1.ReportProgress(4, "* Sending records to DB... " + count + " | " + beforeImport.ToString());

                            DataFunctions.ImportSubscribers("<XML>" + strInsertXML.ToString() + "</XML>", new SqlConnection(clientConn));
                            if (demoInsertXML.ToString().Length > 20)
                            {
                                DataFunctions.ImportSubscriberDemographics("<XML>" + demoInsertXML.ToString() + "</XML>", new SqlConnection(clientConn));
                            }

                            demoInsertXML.Clear();
                            strInsertXML.Clear();
                        }
                        #endregion
                    }
                    if(!string.IsNullOrEmpty(strInsertXML.ToString()))
                    {
                        DateTime beforeImport = DateTime.Now;
                        backgroundWorker1.ReportProgress(4, "* Sending records to DB... " + count + " | " + beforeImport.ToString());

                        DataFunctions.ImportSubscribers("<XML>" + strInsertXML.ToString() + "</XML>", new SqlConnection(clientConn));
                        if (demoInsertXML.ToString().Length > 20)
                        {
                            DataFunctions.ImportSubscriberDemographics("<XML>" + demoInsertXML.ToString() + "</XML>", new SqlConnection(clientConn));
                        }

                        demoInsertXML.Clear();
                        strInsertXML.Clear();
                    }

                    fileConn.Close();
                    backgroundWorker1.ReportProgress(4, "Done reading in File: " + Path.GetFileNameWithoutExtension(txtDBFFileName.Text) + ", Count: " + count);
                }

                backgroundWorker1.ReportProgress(6, "Validating Codesheet");
                DataFunctions.execute_codeSheetValidation(sourceFileID, processCode, new SqlConnection(clientConn));
                DataFunctions.execute_codeSheetValidation(sourceFileID, paidProcessCode, new SqlConnection(clientConn));

                backgroundWorker1.ReportProgress(7, "Moving records to Final");
                DataFunctions.execute_subscriberFinal_save("DBF", processCode, new SqlConnection(clientConn));
                DataFunctions.execute_subscriberFinal_save("DBF", paidProcessCode, new SqlConnection(clientConn));

                backgroundWorker1.ReportProgress(8, "Cleaning data...");
                DataFunctions.execute_data_matching(sourceFileID, processCode, new SqlConnection(clientConn));

                backgroundWorker1.ReportProgress(9, "Import into PubSubscriptions & Subscriptions");
                DataFunctions.execute_ImportFromUAS(processCode, "DBF", new SqlConnection(clientConn));
                DataFunctions.execute_ImportFromUAS(paidProcessCode, "DBF", new SqlConnection(clientConn));

                backgroundWorker1.ReportProgress(10, "Creating Report...");

                DataTable report = DataFunctions.getDataTable("SELECT " + 
                    "( SELECT COUNT(*) FROM SubscriberTransformed st WHERE st.SourceFileID = " + sourceFileID.ToString() + " AND (st.ProcessCode = '" + processCode + "' OR st.ProcessCode = '" + paidProcessCode + "') ) as 'Original Count'," +
                    "( SELECT COUNT(*) FROM SubscriberInvalid si WHERE si.SourceFileID = " + sourceFileID.ToString() + " AND (si.ProcessCode = '" + processCode + "' OR si.ProcessCode = '" + paidProcessCode + "' )) as 'Invalid Count'," +
                    "( SELECT COUNT(*) FROM SubscriberArchive sa WHERE sa.SourceFileID = " + sourceFileID.ToString() + " AND (sa.ProcessCode = '" + processCode + "' OR sa.ProcessCode = '" + paidProcessCode + "' )) as 'Dupe Count'",
                    new SqlConnection(clientConn));

                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = Path.Combine(pathUser, "Downloads");

                DataFunctions.CreateCSVFromDataTable(report, pathDownload + "\\" + pubCode.ToUpper() + "_" + DateTime.Now.ToShortDateString() + "_DupesReport.csv", true);

                backgroundWorker1.ReportProgress(11, "Report saved to: " + pathDownload + "\\" + pubCode.ToUpper() + "_" + DateTime.Now.ToShortDateString() + "_DupesReport");
                backgroundWorker1.ReportProgress(12, "File " + fileNameNoExt + " Loaded");
                backgroundWorker1.ReportProgress(13, " ==================================================");
                backgroundWorker1.ReportProgress(14, " ");
            }
            catch (Exception ex)
            {
                backgroundWorker1.ReportProgress(15, "Error processing file : " + fileNameNoExt + " / " + ex.Message);
                backgroundWorker1.ReportProgress(16, " ");
            }
        }

        private void FindField(string field, DataTable others, DataTable responses, DataTable fields)
        {
            field = field.Trim();
            string find = "ResponseGroupName = '" + field + "'";
            string findProfile = "COLUMN_NAME = '" + field + "'";
            string findAdHoc = "CustomField = '" + field + "'";
            DataRow[] foundRows = responses.Select(find);
            DataRow[] foundProfileRows = fields.Select(findProfile);
            DataRow[] foundAdHocRows = others.Select(findAdHoc);
            if (responses.Rows.Count > 0 && foundRows.Count() > 0)
                demographics.Add(foundRows.FirstOrDefault().ItemArray[0].ToString());
            else if (fields.Rows.Count > 0 && foundProfileRows.Count() > 0)
                profileFields.Add(foundProfileRows.FirstOrDefault().ItemArray[0].ToString());
            else if (others.Rows.Count > 0 && foundAdHocRows.Count() > 0)
                adHocs.Add(field);
            else if (ConfigurationManager.AppSettings.Get(field) != null)
            {
                string name = ConfigurationManager.AppSettings.Get(field);
                findProfile = "COLUMN_NAME = '" + name + "'";
                foundProfileRows = fields.Select(findProfile);
                if(fields.Rows.Count > 0 && foundProfileRows.Count() > 0)
                {
                    mappedProfileFields.Add(field);
                }
            }
        }

        int percent = 0;
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(percent == e.ProgressPercentage)
            {
                lstMessage.Items[lstMessage.Items.Count - 1] = e.UserState + " - " + DateTime.Now.ToShortTimeString().ToString();
            }
            else
                lstMessage.Items.Add(e.UserState + " - " + DateTime.Now.ToShortTimeString().ToString());

            percent = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.Enabled = true;
        }
    }
}
