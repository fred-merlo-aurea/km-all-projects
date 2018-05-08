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

namespace KMPS_Tools
{
    public partial class WQTImport : Form
    {
        public WQTImport()
        {
            InitializeComponent();
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ImportDatatoDB();

            backgroundWorker1.ReportProgress(100, "End Time : " + System.DateTime.Now);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if (lstMessage.Items.Count > 0 && e.UserState.ToString().StartsWith("* Import Complete", StringComparison.OrdinalIgnoreCase))
            //{
            //    object li = lstMessage.Items[lstMessage.Items.Count - 1];

            //    if (li.ToString().StartsWith("* Import Complete", StringComparison.OrdinalIgnoreCase))
            //    {
            //        lstMessage.Items.Remove(li);
            //    }
            //    lstMessage.Items.Add(e.UserState);
            //}
            //else
            {
                lstMessage.Items.Add(e.UserState);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnImport.Enabled = true;
            btnCancel.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        #region extract Column Names from From DBIVFile / SQL Table
        //extract Column Names from From DBIVFile / Table
        //public List<Subscriber> GetList(string filePath, string file)
        //{
        //    List<Subscriber> lSubscriber = new List<Subscriber>();

        //    int MagazineID = SQLFunctions.getMagazineID(file.ToLower().Replace("sub.dbf",""));
        //    Dictionary<string, int> dQSource = SQLFunctions.getQSource();
        //    //Hashtable(TransactionID)
        //    //Hashtable(CategoryID)
        //    //Hashtable(QsourceID)

        //    try
        //    {
        //        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filePath + ";Extended Properties=dBase III");
        //        OleDbDataReader dr;

        //        OleDbCommand comm = new OleDbCommand("SELECT * FROM " + file + "", conn);

        //        conn.Open();

        //        dr = comm.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            Subscriber s = new Subscriber();
        //            int index;
        //            string name;

        //            s.MagazineID = MagazineID;

        //            name = "SEQUENCE";
        //            index = dr.GetOrdinal(name);
        //            if (index >= 0 && !dr.IsDBNull(index))
        //                s.SubscriberID = Convert.ToInt32(dr[index]);

        //            s.EMAILADDRESS =getValue(dr, "EMAIL");
        //            s.FNAME = getValue(dr, "FNAME");

        //            s.LNAME = getValue(dr, "LNAME");
        //            s.COMPANY = getValue(dr, "COMPANY");
        //            s.TITLE = getValue(dr, "TITLE");
        //            s.ADDRESS = getValue(dr, "ADDRESS");
        //            s.MAILSTOP = getValue(dr, "MAILSTOP");
        //            s.CITY = getValue(dr, "CITY");
        //            s.STATE = getValue(dr, "STATE");
        //            s.ZIP = getValue(dr, "ZIP");
        //            s.PLUS4 = getValue(dr, "PLUS4");
        //            s.COUNTY = getValue(dr, "COUNTY");
        //            s.COUNTRY = getValue(dr, "COUNTRY");

        //            name = "CTRY";
        //            index = dr.GetOrdinal(name);
        //            if (index >= 0 && !dr.IsDBNull(index))
        //                s.Country_ID = Convert.ToInt32(dr[index]);

        //            s.PHONE = getValue(dr, "PHONE");
        //            s.FAX = getValue(dr, "FAX");

        //            name = "CAT";
        //            index = dr.GetOrdinal(name);
        //            if (index >= 0 && !dr.IsDBNull(index))
        //                s.Category_ID = Convert.ToInt32(dr[index]);

        //            name = "XACT";
        //            index = dr.GetOrdinal(name);
        //            if (index >= 0 && !dr.IsDBNull(index))
        //                s.Transaction_ID = Convert.ToInt32(dr[index]);

        //            name = "XACTDATE";
        //            index = dr.GetOrdinal(name);
        //            if (index >= 0 && !dr.IsDBNull(index))
        //                s.Transaction_Date = Convert.ToDateTime(dr[index]);

        //            name = "QSOURCE";
        //            index = dr.GetOrdinal(name);
        //            if (index >= 0 && !dr.IsDBNull(index))
        //                s.QSource_ID = Convert.ToInt32(dQSource[dr[index].ToString()]);


        //            name = "QDATE";
        //            index = dr.GetOrdinal(name);
        //            if (index >= 0 && !dr.IsDBNull(index))
        //                s.QualificationDate = Convert.ToDateTime(dr[index]);

        //            //s.Subsrc = getValue(dr, "Subsrc");
        //            //s.Promosrc = getValue(dr, "Promosrc");

        //            //s.FORZIP = getValue(dr, "FORZIP");
        //            //s.BUSINESS = getValue(dr, "BUSINESS");
        //            //s.BUSINESS1 = getValue(dr, "BUSINESS1");
        //            //s.FUNCTION = getValue(dr, "FUNCTION");
        //            //s.FUNCTION1 = getValue(dr, "FUNCTION1");
        //            //s.SALES = getValue(dr, "SALES");
        //            //s.EMPLOY = getValue(dr, "EMPLOY");
        //            //s.PAR3C = getValue(dr, "PAR3C");
        //            //s.EXPIRE = getValue(dr, "EXPIRE");
        //            //s.SPECIFY = getValue(dr, "SPECIFY");

        //            name = "COPIES";
        //            index = dr.GetOrdinal(name);
        //            if (index >= 0 && !dr.IsDBNull(index))
        //                s.COPIES = Convert.ToInt32(dr[index]);

        //            //s.WEBSITE = getValue(dr, "WEBSITE");
        //            //s.MBR_CODE = getValue(dr, "MBR_CODE");
        //            //s.MBR_FLAG = getValue(dr, "MBR_FLAG");
        //            //s.MBR_REJECT = getValue(dr, "MBR_REJECT");
        //            //s.Demo1 = getValue(dr, "Demo1");
        //            //s.Demo2 = getValue(dr, "Demo2");
        //            //s.Demo3= getValue(dr, "Demo3");
        //            //s.Demo4= getValue(dr, "Demo4");
        //            //s.Demo5= getValue(dr, "Demo5");
        //            //s.Demo6= getValue(dr, "Demo6");
        //            //s.Demo7= getValue(dr, "Demo7");
        //            //s.Demo8= getValue(dr, "Demo8");
        //            //s.Demo9= getValue(dr, "Demo9");
        //            //s.Demo10= getValue(dr, "Demo10");
        //            //s.Demo11= getValue(dr, "Demo11");
        //            //s.Demo12= getValue(dr, "Demo12");
        //            //s.Demo13= getValue(dr, "Demo13");
        //            //s.Demo14= getValue(dr, "Demo14");
        //            //s.Demo15= getValue(dr, "Demo15");
        //            //s.Demo16= getValue(dr, "Demo16");
        //            //s.Demo31= getValue(dr, "Demo31");
        //            //s.Demo32= getValue(dr, "Demo32");
        //            //s.Demo33= getValue(dr, "Demo33");
        //            //s.Demo34= getValue(dr, "Demo34");
        //            //s.Demo35= getValue(dr, "Demo35");
        //            //s.Demo36= getValue(dr, "Demo36");

        //            lSubscriber.Add(s);
        //        }

        //        lstMessage.Items.Add("* # of Records in DBIV file : " + lSubscriber.Count.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        string exx = ex.ToString();
        //        lstMessage.Items.Add("[ERROR]: Error when Creating DataTable from DBASE File. \n" + exx);
        //    }

        //    return lSubscriber;
        //}


        private string getValue(OleDbDataReader dr, string columnname)
        {
            try
            {
                int index = dr.GetOrdinal(columnname);

                if (index >= 0 && !dr.IsDBNull(index))
                    return System.Text.RegularExpressions.Regex.Replace(Convert.ToString(dr[index]), @"[^\u0000-\u007F]", "");
                ;
            }
            catch
            {
                lstMessage.Items.Add("* " + columnname + " not exists ");
            }

            return string.Empty;
        }
        #endregion

        private void btnChooseDBF_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtDBFFile.Text = Path.GetFileName(openFileDialog1.FileName);
                lstMessage.Items.Add("* Filepath: " + openFileDialog1.FileName);
            }
        }

        private void ImportDatatoDB()
        {

            DateTime dtStartTime = System.DateTime.Now;
            string filePath = Path.GetDirectoryName(openFileDialog1.FileName);
            string file = Path.GetFileName(txtDBFFile.Text);
            int count = 0;
            bool bcancel = false;

            StringBuilder strInsertXML = new StringBuilder();

            int MagazineID = SQLFunctions.getMagazineID(file.ToLower().Replace("sub.dbf", ""));

            if (MagazineID > 0)
            {
                Dictionary<string, int> dQSource = SQLFunctions.getQSource();
                List<string> lFields = SQLFunctions.getMagazineFields(MagazineID);

                List<string> lAddCommas = new List<string>();

                try
                {

                    if (System.Configuration.ConfigurationManager.AppSettings[file.ToLower().Replace("sub.dbf", "").ToUpper() + "_AddCommastoDemo"].ToString() != "")
                    {
                        string[] demofields = System.Configuration.ConfigurationManager.AppSettings[file.ToLower().Replace("sub.dbf", "").ToUpper() + "_AddCommastoDemo"].ToUpper().ToString().Split(',');
                        lAddCommas = new List<string>(demofields);
                    }
                }
                catch
                { }

                try
                {
                    SQLFunctions.UpdateMagazineIssueDate(MagazineID, dtIssueDate.Value);

                    backgroundWorker1.ReportProgress(count, "* Issue Date has been updated"); ;

                    backgroundWorker1.ReportProgress(count, "Starting Deletes (" + DateTime.Now.Subtract(dtStartTime).TotalSeconds.ToString("F2") + " mins)");

                    //Delete Existing records for the Magazine
                    SQLFunctions.DeleteSubscribers(MagazineID);

                    backgroundWorker1.ReportProgress(count, "* Deleting existing Records (" + DateTime.Now.Subtract(dtStartTime).TotalSeconds.ToString("F2") + " secs)");

                    #region OLEBD

                    //OleDbConnection conn = new OleDbConnection(); ;
                    //OleDbDataReader dr;
                    //OleDbCommand comm = new OleDbCommand("SELECT * FROM " + file + "", conn);

                    //try
                    //{
                    //    conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filePath + ";Extended Properties=dBase III";
                    //    conn.Open();
                    //}
                    //catch
                    //{
                    //    try
                    //    {
                    //        conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Extended Properties=dBase III";
                    //        conn.Open();
                    //    }
                    //    catch (Exception ex1)
                    //    {
                    //        MessageBox.Show("[ERROR]: " + ex1);
                    //        throw new Exception("OLEDB driver not found.");
                    //    }
                    //}

                    #endregion

                    string fileNameNoExt = Path.GetFileNameWithoutExtension(file);

                    OleDbConnection conn = new OleDbConnection(@"Provider=VFPOLEDB.1;Data Source=" + filePath + "\\" + file);
                    OleDbDataReader dr;
                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {
                        Console.WriteLine("Reading in File: " + file);
                        string mySQL = "SELECT * FROM [" + fileNameNoExt + "]";

                        OleDbCommand comm = new OleDbCommand(mySQL, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(comm);

                        dr = comm.ExecuteReader();

                        Console.WriteLine("Done reading in File: " + file);

                        while (dr.Read())
                        {
                            if (backgroundWorker1.CancellationPending)
                            {
                                bcancel = true;
                                backgroundWorker1.ReportProgress(count, "Starting CleanUp (" + DateTime.Now.Subtract(dtStartTime).TotalSeconds.ToString("F2") + " mins)");
                                SQLFunctions.DeleteSubscribers(MagazineID);
                                backgroundWorker1.ReportProgress(count, "*CleanUp (" + DateTime.Now.Subtract(dtStartTime).TotalSeconds.ToString("F2") + " mins)");
                                strInsertXML.Clear();
                                break;
                            }

                            strInsertXML.Append("<SUBSCRIBER>");
                            count++;

                            if (count % 1000 == 0)
                                backgroundWorker1.ReportProgress(count, count.ToString() + " / " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            string data = string.Empty;

                            foreach (string s in lFields)
                            {
                                try
                                {
                                    data = dr[s].ToString().Trim();

                                    if (data.Length > 0)
                                    {
                                        switch (s.ToUpper())
                                        {
                                            case "QSOURCE":
                                                {
                                                    strInsertXML.Append("<QSOURCE>" + dQSource[data] + "</QSOURCE>");
                                                    break;
                                                }
                                            case "XACTDATE":
                                            case "QDATE":
                                            case "EXPIRE":
                                                {
                                                    strInsertXML.Append("<" + s + ">" + String.Format("{0:MM/dd/yyyy}", data) + "</" + s + ">");
                                                    break;
                                                }
                                            default:
                                                {
                                                    if (lAddCommas.Count > 0 && lAddCommas.Contains(s))
                                                    {
                                                        strInsertXML.Append("<" + s + ">" + System.Security.SecurityElement.Escape(AddCommas(getValue(dr, s))) + "</" + s + ">");
                                                    }
                                                    else
                                                    {
                                                        strInsertXML.Append("<" + s + ">" + System.Security.SecurityElement.Escape(getValue(dr, s)) + "</" + s + ">");
                                                    }
                                                    break;
                                                }
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }

                            strInsertXML.Append("</SUBSCRIBER>");

                            if (count % 25000 == 0)
                            {
                                DateTime beforeImport = DateTime.Now;
                                backgroundWorker1.ReportProgress(count, "* Sending 25K records to DB... " + beforeImport.ToString());

                                SQLFunctions.ImportSubscribers(MagazineID, "<XML>" + strInsertXML.ToString() + "</XML>");
                                DateTime AfterImport = DateTime.Now;

                                strInsertXML.Clear();

                                backgroundWorker1.ReportProgress(count, "* Import Complete : " + count.ToString() + " / Proc Exec Time : " + AfterImport.Subtract(beforeImport).TotalSeconds.ToString("F2") + " secs" + " / (" + DateTime.Now.Subtract(dtStartTime).TotalSeconds.ToString("F2") + " secs)");
                            }
                        }

                        if (!bcancel && strInsertXML.ToString() != string.Empty)
                        {
                            DateTime beforeImport = DateTime.Now;
                            SQLFunctions.ImportSubscribers(MagazineID, "<XML>" + strInsertXML.ToString() + "</XML>");
                            DateTime AfterImport = DateTime.Now;

                            strInsertXML.Clear();

                            backgroundWorker1.ReportProgress(count, "* Import Complete : " + count.ToString() + " / Proc Exec Time : " + AfterImport.Subtract(beforeImport).TotalSeconds.ToString("F2") + " secs" + " / (" + DateTime.Now.Subtract(dtStartTime).TotalSeconds.ToString("F2") + " secs)");

                        }

                        dr.Dispose();
                        comm.Dispose();
                        conn.Close();

                        if (!bcancel)
                        {
                            backgroundWorker1.ReportProgress(count++, "* Total Records in file : " + count.ToString());

                            backgroundWorker1.ReportProgress(count++, "* Processing Records................................");

                            SQLFunctions.ProcessSubscriberData(MagazineID);

                            backgroundWorker1.ReportProgress(count++, "* Processing Complete : (" + DateTime.Now.Subtract(dtStartTime).TotalSeconds.ToString("F2") + " secs)");

                        }
                    }
                    MessageBox.Show("Import Complete.");
                }
                catch (Exception ex)
                {
                    SQLFunctions.DeleteSubscribers(MagazineID);
                    string exx = ex.ToString();
                    backgroundWorker1.ReportProgress(count++, "[ERROR]: " + exx);
                    MessageBox.Show("[ERROR]: " + exx);
                }
            }
            else
            {
                MessageBox.Show("Invalid Sub File - Sub not configured in the Database");
            }
        }

        private string AddCommas(string value)
        {
            string returnvalue = string.Empty;

            char[] chars = value.ToCharArray();

            foreach (char c in chars)
            {
                returnvalue += (returnvalue == string.Empty ? c.ToString() : "," + c.ToString());
            }

            return returnvalue;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WQTImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main frm = (Main)this.MdiParent;
            frm.ToggleMenus(true);
        }
    }
}
