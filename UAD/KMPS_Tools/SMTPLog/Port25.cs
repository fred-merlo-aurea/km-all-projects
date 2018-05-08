using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using ecn.communicator.classes.ImportData;

namespace KMPS_Tools
{
    public partial class Port25 : Form
    {
        private static ECN_Framework_Entities.Accounts.User _User = null;
        string ecnCommunicatorConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString;
        public static string outLog = "";
        
        public static string BPALog = "";
        public static StreamWriter BPAFile = null;
        public static Random r1 = new Random();
        //

        public Port25()
        {
            _User = ECN_Framework_BusinessLayer.Accounts.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);
            InitializeComponent();
        }

        private void btnFileLocation_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtFolderLocation.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private DataTable GetDigitalSplit(string path, string filetype, string filename, int maxrecords)
        {
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Text;HDR=NO;'";

            try
            {
                System.Data.OleDb.OleDbDataAdapter oleAdapter = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM " + filename + " ", connString);
                DataSet dataset = new DataSet(filename);
                oleAdapter.Fill(dataset, 1, maxrecords, filename); //fill the adapter with rows only from the linenumber specified														
                return dataset.Tables[filename];
            }
            catch
            {
                try
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Text;HDR=NO;'";
                    System.Data.OleDb.OleDbDataAdapter oleAdapter = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM " + filename + " ", connString);
                    DataSet dataset = new DataSet(filename);
                    oleAdapter.Fill(dataset, 1, maxrecords, filename); //fill the adapter with rows only from the linenumber specified														
                    return dataset.Tables[filename];
                }
                catch (Exception ex1)
                {
                    MessageBox.Show("[ERROR]: " + ex1);
                    throw new Exception("OLEDB driver not found.");
                }
            }


        }

        private void ExportDataByBlast()
        {
            StreamWriter outFile = null;
            outLog = ConfigurationManager.AppSettings["OutLog"] + DateTime.Now.Date.ToString("d") + ".log";
            outLog = outLog.Replace(@"/", "");
            outFile = new StreamWriter(new FileStream(folderBrowserDialog1.SelectedPath + "\\" + outLog, System.IO.FileMode.Append));
            outFile.WriteLine("Start Time : " + System.DateTime.Now);

            List<int> lBlastIDs;

            try
            {
                lBlastIDs = txtBlastID.Text.Split(',').Select(int.Parse).ToList();
            }
            catch
            {
                string exx = "Incorrect BlastIDs.";
                outFile.WriteLine("[ERROR]: " + exx);
                MessageBox.Show("[ERROR]: " + exx);
                outFile.Close();
                return;
            }

            if (lBlastIDs.Count > 10)
            {
                string exx = "Cannot process more than 10 Blasts.";
                outFile.WriteLine("[ERROR]: " + exx);
                MessageBox.Show("[ERROR]: " + exx);
                outFile.Close();
                return;
            }

            foreach (int BlastID in lBlastIDs)
            {

                int count = 0;
                try
                {
                    DataTable digitalSplitDT = null;
                    if (txtDigitalSplit.Text.Length > 0)
                    {
                        digitalSplitDT = GetDigitalSplit(Path.GetDirectoryName(txtDigitalSplit.Text), "C", openFileDialog1.SafeFileName, 100001);
                    }

                    outFile.WriteLine(DateTime.Now.ToString() + " Getting blast info for blast: " + BlastID);
                    backgroundWorker1.ReportProgress(count, "* Getting blast info for blast: " + BlastID);
                    Models.Blasts blast = DAL.Blasts.GetBlastsInfo(BlastID);
                    if (blast != null)
                    {
                        outFile.WriteLine(DateTime.Now.ToString() + " Setting up audit file");
                        backgroundWorker1.ReportProgress(count, "* Setting up audit file");

                        BPAFile = SetupBPAAudit(blast.CustomerName, blast);
                        if (BPAFile != null)
                        {
                            outFile.WriteLine(DateTime.Now.ToString() + " Getting sends for blast: " + BlastID);
                            backgroundWorker1.ReportProgress(count, "* Getting sends for blast: " + BlastID);
                            List<ECN_Framework.Activity.Entity.BlastActivitySends> sendsList = ECN_Framework.Activity.Entity.BlastActivitySends.GetByBlastID(BlastID);
                            List<ECN_Framework.Activity.Entity.BlastActivityBounces> bouncesList = ECN_Framework.Activity.Entity.BlastActivityBounces.GetByBlastID(BlastID);
                            if (sendsList != null && sendsList.Count > 0)
                            {
                                foreach (ECN_Framework.Activity.Entity.BlastActivitySends send in sendsList)
                                {
                                    count++;
                                    if (count % 100 == 0)
                                    {
                                        backgroundWorker1.ReportProgress(count, "* Writing " + count.ToString() + " records");
                                    }

                                    List<ECN_Framework.Activity.Entity.BlastActivityBounces> bouncesListFiltered = bouncesList.FindAll(x => x.EmailID == send.EmailID);
                                    if (bouncesListFiltered != null && bouncesListFiltered.Count > 0)
                                    {
                                        //bounce found, use the bounce info
                                        LogRecord(ref BPAFile, send, bouncesListFiltered[0], blast);
                                    }
                                    else
                                    {
                                        //bounce not found, use send info
                                        LogRecord(ref BPAFile, send, null, blast);
                                    }
                                }
                                if (digitalSplitDT != null && digitalSplitDT.Rows.Count > 0)
                                {
                                    outFile.WriteLine(DateTime.Now.ToString() + " Processing digital split for blast: " + BlastID);
                                    backgroundWorker1.ReportProgress(count, "* Processing digital split for blast: " + BlastID);

                                    count = 0;
                                    int msgid = sendsList[0].SendID;
                                    foreach (DataRow row in digitalSplitDT.Rows)
                                    {
                                        count++;
                                        if (count % 100 == 0)
                                        {
                                            backgroundWorker1.ReportProgress(count, "* Checking " + count.ToString() + " records");
                                        }
                                        List<ECN_Framework.Activity.Entity.BlastActivitySends> sendsListFiltered = sendsList.FindAll(x => x.EmailAddress.ToUpper() == row[0].ToString().ToUpper());
                                        if (sendsListFiltered == null || sendsListFiltered.Count <= 0)
                                        {
                                            LogDigitalSplitRecord(ref BPAFile, row[0].ToString(), blast, ref msgid);
                                        }
                                    }
                                }

                                outFile.WriteLine("Export Completed");
                                backgroundWorker1.ReportProgress(count, "* Export Completed");
                                BPAFile.Close();
                                //MessageBox.Show("Export Complete.");
                            }
                            else
                            {
                                outFile.WriteLine("No sends found to process");
                                backgroundWorker1.ReportProgress(count, "* No sends found to process");
                                //MessageBox.Show("Export Complete.");
                            }
                        }
                        else
                        {
                            backgroundWorker1.ReportProgress(count, "* Error creating BPALog file for blast: " + BlastID);
                            outFile.WriteLine("Error creating BPALog file for blast: " + BlastID);
                           // MessageBox.Show(DateTime.Now.ToString() + " Export Complete.");
                        }
                    }
                    else
                    {
                        outFile.WriteLine("No blast found to process");
                        backgroundWorker1.ReportProgress(count, "* No blast found to process");
                        //MessageBox.Show("Export Complete.");
                    }
                }
                catch (Exception Ex)
                {
                    string exx = Ex.ToString();
                    outFile.WriteLine("[ERROR]: " + exx);
                    backgroundWorker1.ReportProgress(count++, "[ERROR]: " + exx);
                    MessageBox.Show("[ERROR]: " + exx);
                }
            }
            outFile.Close();
            MessageBox.Show("Export Complete.");

        }

        private void ExportDataByGroup()
        {
            ECN_Framework_Entities.Communicator.Group group = null;
            try
            {
               group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(Convert.ToInt32(txtGroupID.Text), _User);
            }
            catch 
            {
                string exx = "Incorrect GroupID.";
                MessageBox.Show("[ERROR]: " + exx);
                return;
            }
            StreamWriter outFile = null;
            outLog = ConfigurationManager.AppSettings["OutLog"] + DateTime.Now.Date.ToString("d") + ".log";
            outLog = outLog.Replace(@"/", "");
            outFile = new StreamWriter(new FileStream(folderBrowserDialog1.SelectedPath + "\\" + outLog, System.IO.FileMode.Append));
            outFile.WriteLine("Start Time : " + System.DateTime.Now); 
            List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList= ECN_Framework_BusinessLayer.Communicator.Blast.GetByGroupID(group.GroupID, _User, false);
            List<int> lBlastIDs = (from src in blastList
                                  where src.SendTime > dtFrom.Value && src.SendTime < dtTo.Value
                                  orderby src.BlastID
                                  select src.BlastID).ToList();
            if (lBlastIDs != null && lBlastIDs.Count == 0)
            {
                MessageBox.Show("There no blasts sent to this group during the dates specified");
                return;
            }

            BPAFile = SetupBPAAuditByGroup(group);
            foreach (int BlastID in lBlastIDs)
            {

                int count = 0;
                try
                {
                    DataTable digitalSplitDT = null;
                    if (txtDigitalSplit.Text.Length > 0)
                    {
                        digitalSplitDT = GetDigitalSplit(Path.GetDirectoryName(txtDigitalSplit.Text), "C", openFileDialog1.SafeFileName, 100001);
                    }

                    outFile.WriteLine(DateTime.Now.ToString() + " Getting blast info for blast: " + BlastID);
                    backgroundWorker1.ReportProgress(count, "* Getting blast info for blast: " + BlastID);
                    Models.Blasts blast = DAL.Blasts.GetBlastsInfo(BlastID);
                    if (blast != null)
                    {
                        outFile.WriteLine(DateTime.Now.ToString() + " Setting up audit file");
                        backgroundWorker1.ReportProgress(count, "* Setting up audit file");

                        
                        if (BPAFile != null)
                        {
                            outFile.WriteLine(DateTime.Now.ToString() + " Getting sends for blast: " + BlastID);
                            backgroundWorker1.ReportProgress(count, "* Getting sends for blast: " + BlastID);
                            List<ECN_Framework.Activity.Entity.BlastActivitySends> sendsList = ECN_Framework.Activity.Entity.BlastActivitySends.GetByBlastID(BlastID);
                            List<ECN_Framework.Activity.Entity.BlastActivityBounces> bouncesList = ECN_Framework.Activity.Entity.BlastActivityBounces.GetByBlastID(BlastID);
                            if (sendsList != null && sendsList.Count > 0)
                            {
                                foreach (ECN_Framework.Activity.Entity.BlastActivitySends send in sendsList)
                                {
                                    count++;
                                    if (count % 100 == 0)
                                    {
                                        backgroundWorker1.ReportProgress(count, "* Writing " + count.ToString() + " records");
                                    }

                                    List<ECN_Framework.Activity.Entity.BlastActivityBounces> bouncesListFiltered = bouncesList.FindAll(x => x.EmailID == send.EmailID);
                                    if (bouncesListFiltered != null && bouncesListFiltered.Count > 0)
                                    {
                                        //bounce found, use the bounce info
                                        LogRecord(ref BPAFile, send, bouncesListFiltered[0], blast);
                                    }
                                    else
                                    {
                                        //bounce not found, use send info
                                        LogRecord(ref BPAFile, send, null, blast);
                                    }
                                }
                                if (digitalSplitDT != null && digitalSplitDT.Rows.Count > 0)
                                {
                                    outFile.WriteLine(DateTime.Now.ToString() + " Processing digital split for blast: " + BlastID);
                                    backgroundWorker1.ReportProgress(count, "* Processing digital split for blast: " + BlastID);

                                    count = 0;
                                    int msgid = sendsList[0].SendID;
                                    foreach (DataRow row in digitalSplitDT.Rows)
                                    {
                                        count++;
                                        if (count % 100 == 0)
                                        {
                                            backgroundWorker1.ReportProgress(count, "* Checking " + count.ToString() + " records");
                                        }
                                        List<ECN_Framework.Activity.Entity.BlastActivitySends> sendsListFiltered = sendsList.FindAll(x => x.EmailAddress.ToUpper() == row[0].ToString().ToUpper());
                                        if (sendsListFiltered == null || sendsListFiltered.Count <= 0)
                                        {
                                            LogDigitalSplitRecord(ref BPAFile, row[0].ToString(), blast, ref msgid);
                                        }
                                    }
                                }

                                outFile.WriteLine("Export Completed");
                                backgroundWorker1.ReportProgress(count, "* Export Completed");
                                //MessageBox.Show("Export Complete.");
                            }
                            else
                            {
                                outFile.WriteLine("No sends found to process");
                                backgroundWorker1.ReportProgress(count, "* No sends found to process");
                                //MessageBox.Show("Export Complete.");
                            }
                        }
                        else
                        {
                            backgroundWorker1.ReportProgress(count, "* Error creating BPALog file for blast: " + BlastID);
                            outFile.WriteLine("Error creating BPALog file for blast: " + BlastID);
                            // MessageBox.Show(DateTime.Now.ToString() + " Export Complete.");
                        }
                    }
                    else
                    {
                        outFile.WriteLine("No blast found to process");
                        backgroundWorker1.ReportProgress(count, "* No blast found to process");
                        //MessageBox.Show("Export Complete.");
                    }
                }
                catch (Exception Ex)
                {
                    string exx = Ex.ToString();
                    outFile.WriteLine("[ERROR]: " + exx);
                    backgroundWorker1.ReportProgress(count++, "[ERROR]: " + exx);
                    MessageBox.Show("[ERROR]: " + exx);
                }
            }
            BPAFile.Close();
            outFile.Close();
            MessageBox.Show("Export Complete.");

        }

        private int getNewMessageID(int msgID)
        {
            msgID = msgID + r1.Next(100, 999);
            return msgID;
        }

        public void LogDigitalSplitRecord(ref StreamWriter BPAFile, string EmailAddress, Models.Blasts blast, ref int msgid)
        {
            string year = Convert.ToDateTime(blast.SendTime).Year.ToString();
            string month = Convert.ToDateTime(blast.SendTime).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            msgid = getNewMessageID(msgid);
            string output = "MSGID[" + msgid.ToString() + "] ISSUE[" + year + month + "1] ";
            if(rbGroup.Checked)
                BPAFile.WriteLine(output + "BLASTID: " + blast.BlastID.ToString());
            BPAFile.WriteLine(output + "DATE: " + Convert.ToDateTime(blast.SendTime).ToString("ddd MMM dd HH:mm:ss.fff yyyy"));
            BPAFile.WriteLine(output + "FROM: " + blast.EmailFrom);
            BPAFile.WriteLine(output + "TO: " + EmailAddress);
            BPAFile.WriteLine(output + "SUBJECT: " + blast.EmailSubject);
            BPAFile.WriteLine(output + "Error: - Delivery expire (message too old) timeout. ");            
        }

        public  void LogRecord(ref StreamWriter BPAFile, ECN_Framework.Activity.Entity.BlastActivitySends send, ECN_Framework.Activity.Entity.BlastActivityBounces bounce, Models.Blasts blast)
        {
            string year = Convert.ToDateTime(blast.SendTime).Year.ToString();
            string month = Convert.ToDateTime(blast.SendTime).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string output = "MSGID[" + send.SendID.ToString() + "] ISSUE[" + year + month + "1] ";
            if (rbGroup.Checked)                
                BPAFile.WriteLine(output + "BLASTID: " + blast.BlastID.ToString());
            BPAFile.WriteLine(output + "DATE: " + Convert.ToDateTime(blast.SendTime).ToString("ddd MMM dd HH:mm:ss.fff yyyy"));
            BPAFile.WriteLine(output + "FROM: " + blast.EmailFrom);
            BPAFile.WriteLine(output + "TO: " + send.EmailAddress);
            BPAFile.WriteLine(output + "SUBJECT: " + blast.EmailSubject);
            if (bounce == null)
            {
                
                if (send.SMTPMessage != null && send.SMTPMessage.Trim().Length > 0)
                {
                    string smtpMessage = string.Empty;
                    try
                    {
                        smtpMessage = send.SMTPMessage.Split('|')[0].ToString();
                    }
                    catch (Exception)
                    {
                        smtpMessage = "IP NOT FOUND, HERE IS ORIGINAL SMTPMessage: " + send.SMTPMessage;
                    }
                    smtpMessage = smtpMessage.Replace("\n", " ");
                    BPAFile.WriteLine(output + "250 Email Sent Successfully to server [" + smtpMessage + "].");
                }
                else
                {
                    BPAFile.WriteLine(output + "250 Email Sent Successfully to server [NO SMTP MESSAGE RECEIVED].");
                }
                
            }
            else
            {
                if (send.SendID == -1237911168)
                {
                    string temp = "stop";
                }
                string bounceMessage = string.Empty;
                if (bounce.BounceMessage == null || bounce.BounceMessage.Length <= 0)
                {
                    bounceMessage = "NO BOUNCE MESSAGE RECEIVED";
                }
                else
                {
                    bounceMessage = bounce.BounceMessage;
                }
                bounceMessage = bounceMessage.Replace("\n", " ");
                BPAFile.WriteLine(output + "Error: " + bounceMessage);
            }
        }

        private string CleanCustomerName(string customerName)
        {
            customerName = customerName.Replace(" ", "");
            customerName = customerName.Replace("/", "");
            customerName = customerName.Replace(@"\", "");
            customerName = customerName.Replace(@"""", "");
            customerName = customerName.Replace("'", "");
            customerName = customerName.Replace("|", "");
            customerName = customerName.Replace(">", "");
            customerName = customerName.Replace("<", "");
            customerName = customerName.Replace("?", "");
            customerName = customerName.Replace("`", "");
            customerName = customerName.Replace("~", "");
            customerName = customerName.Replace("!", "");
            customerName = customerName.Replace("@", "");
            customerName = customerName.Replace("#", "");
            customerName = customerName.Replace("$", "");
            customerName = customerName.Replace("%", "");
            customerName = customerName.Replace("^", "");
            customerName = customerName.Replace("&", "");
            customerName = customerName.Replace("*", "");
            customerName = customerName.Replace("(", "");
            customerName = customerName.Replace(")", "");
            customerName = customerName.Replace(">", "");
            customerName = customerName.Replace(">", "");

            return customerName;
        }

        private StreamWriter SetupBPAAuditByGroup(ECN_Framework_Entities.Communicator.Group group)
        {
            ECN_Framework_Entities.Accounts.Customer customer= ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(group.CustomerID, false);
            StreamWriter BPAFile = null;
            try
            {
                if (!Directory.Exists(folderBrowserDialog1.SelectedPath + "\\" + CleanCustomerName(customer.CustomerName)))
                {
                    Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "\\" + CleanCustomerName(customer.CustomerName));
                }
                string BPALog = folderBrowserDialog1.SelectedPath + "\\" + CleanCustomerName(customer.CustomerName) + "\\" + group.GroupID.ToString() + "_" + dtFrom.Value.ToLongDateString() + "_" + dtTo.Value.ToLongDateString() + ".txt";
                BPALog = BPALog.Replace(@"/", "");
                if (File.Exists(BPALog))
                {
                    File.Delete(BPALog);
                }
                BPAFile = new StreamWriter(new FileStream(BPALog, System.IO.FileMode.Append));
            }
            catch (Exception)
            {
            }
            return BPAFile;
        }

        private StreamWriter SetupBPAAudit(string customerName, Models.Blasts blast)
        {
            StreamWriter BPAFile = null;
            try
            {
                if (!Directory.Exists(folderBrowserDialog1.SelectedPath + "\\" + CleanCustomerName(customerName)))
                {
                    Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "\\" + CleanCustomerName(customerName));
                }
                string year = Convert.ToDateTime(blast.SendTime).Year.ToString();
                string month = Convert.ToDateTime(blast.SendTime).Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                string BPALog = folderBrowserDialog1.SelectedPath + "\\" + CleanCustomerName(customerName) + "\\" + blast.BlastID.ToString() + "_" + year + "_" + month + ".txt";
                BPALog = BPALog.Replace(@"/", "");
                if (File.Exists(BPALog))
                {
                    File.Delete(BPALog);
                }
                BPAFile = new StreamWriter(new FileStream(BPALog, System.IO.FileMode.Append));
            }
            catch (Exception)
            {
            }
            return BPAFile;
        }

        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (rbBlastID.Checked)
                ExportDataByBlast();
            else
                ExportDataByGroup();

            backgroundWorker1.ReportProgress(100, "End Time : " + System.DateTime.Now);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lstMessage.Items.Add(e.UserState);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnCreateLogFile.Enabled = true;
            btnCancel.Enabled = false;
        }

        private void btnCreateLogFile_Click(object sender, EventArgs e)
        {
            lstMessage.Items.Clear();
            

            string errorMessage = ValidateOptions();
            if (errorMessage.Length == 0)
            {
                btnCreateLogFile.Enabled = false;
                btnCancel.Enabled = true;

                lstMessage.Items.Add("Start Time : " + System.DateTime.Now);
                lstMessage.Items.Add(" ");

                backgroundWorker1.RunWorkerAsync();

            }
            else
            {
                MessageBox.Show(errorMessage);
            }
        }

        private string ValidateOptions()
        {
            string errorMessage = string.Empty;
            if (rbBlastID.Checked)
            {
                if (txtBlastID.Text.Length <= 0 || txtFolderLocation.Text.Length <= 0 || (cbxDigitalSplit.Checked && btnDigitalSplit.Text.Length <= 0))
                {
                    errorMessage += "Select options to export. ";
                }
            }
            else
            {
                if (txtGroupID.Text.Length <= 0 || txtFolderLocation.Text.Length <= 0 || (cbxDigitalSplit.Checked && btnDigitalSplit.Text.Length <= 0))
                {
                    errorMessage += "Select options to export. ";
                }
            }
            return errorMessage;
        }

        private void cbxDigitalSplit_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxDigitalSplit.Checked)
            {
                btnDigitalSplit.Enabled = true;
            }
            else
            {
                btnDigitalSplit.Enabled = false;
            }
        }

        private void btnDigitalSplit_Click(object sender, EventArgs e)
        {

            DialogResult result = openFileDialog1.ShowDialog();//folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtDigitalSplit.Text = openFileDialog1.FileName;
            }
        }

        private void rbBlastID_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBlastID.Checked)
            {
                rbGroup.Checked= false;
                pnlGroupSelect.Enabled = false;
                txtBlastID.Enabled = true;
            }
        }

        private void rbGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGroup.Checked)
            {
                rbBlastID.Checked = false;
                pnlGroupSelect.Enabled = true;
                txtBlastID.Enabled = false;
            }
        }

        
    }
}
