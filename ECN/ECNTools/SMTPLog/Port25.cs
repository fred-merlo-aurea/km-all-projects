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
using ECN_Framework_Entities.Activity;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using BusinessAccounts = ECN_Framework_BusinessLayer.Accounts;

namespace ECNTools.SMTPLog
{
    public partial class Port25 : Form
    {
        private const int HundredPercent = 100;
        private const string OutFileErrorPrefix = "[ERROR]:";
        private const string AppSettingAppLog = "OutLog";
        private const string DelimSlash = @"/";
        private const string FileTypeC = "C";
        private const int Maxrecords = 100001;
        private const string MessageNoBlast = "There no blasts sent to this group during the dates specified";
        private const int Hours23 = 23;
        private const int Minutes59 = 59;
        private const string ErrorParse = "{0} couldn't be parsed into int.";
        private const int BlastThreshold10 = 10;
        private const char DelimComma = ',';
        private static KMPlatform.Entity.User _User = null;
        string ecnCommunicatorConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString;
        public static string outLog = "";

        public static string BPALog = "";
        public static StreamWriter BPAFile = null;
        public static Random r1 = new Random();

        public Port25()
        {
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
            System.Data.OleDb.OleDbDataAdapter oleAdapter = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM " + filename + " ", connString);
            DataSet dataset = new DataSet(filename);
            oleAdapter.Fill(dataset, 1, maxrecords, filename); //fill the adapter with rows only from the linenumber specified														
            return dataset.Tables[filename];
        }

        private void ExportDataByBlast()
        {
            using (var outFile = CreateOutFileForExport())
            {
                List<int> lBlastIds;

                try
                {
                    lBlastIds = txtBlastID.Text.Split(DelimComma).Select(int.Parse).ToList();
                }
                catch
                {
                    var formattedError = $"{OutFileErrorPrefix} Incorrect BlastIDs.";
                    outFile.WriteLine(formattedError);
                    MessageBox.Show(formattedError);
                    outFile.Close();
                    return;
                }

                if (lBlastIds.Count > BlastThreshold10)
                {
                    var formattedError = $"{OutFileErrorPrefix} Cannot process more than {BlastThreshold10} Blasts.";
                    outFile.WriteLine(formattedError);
                    MessageBox.Show(formattedError);
                    outFile.Close();
                    return;
                }

                ExportProcessBlasts(outFile, lBlastIds, blast =>
                {
                    var cust = BusinessAccounts.Customer.GetByCustomerID(blast.CustomerID.Value, false);
                    return SetupBPAAudit(cust.CustomerName, blast);
                });
            }
        }

        private void ExportDataByGroup()
        {
            Group groupEntity;
            try
            {
                int groupId;
                if (!int.TryParse(txtGroupID.Text, out groupId))
                {
                    throw new InvalidOperationException(string.Format(ErrorParse, txtGroupID.Text));
                }

                groupEntity = BusinessCommunicator.Group.GetByGroupID_NoAccessCheck(groupId);
            }
            catch
            {
                MessageBox.Show($"{OutFileErrorPrefix} Incorrect GroupID.");
                return;
            }
            var outFile = CreateOutFileForExport();
            var blastList = BusinessCommunicator.Blast.GetByGroupID_NoAccessCheck(groupEntity.GroupID, false);
            var lBlastIds = (from src in blastList
                                where src.SendTime > dtFrom.Value
                                                            .AddHours(-dtFrom.Value.Hour)
                                                            .AddMinutes(-dtFrom.Value.Minute) && 
                                      src.SendTime < dtTo.Value
                                                            .AddHours(-dtTo.Value.Hour)
                                                            .AddMinutes(-dtTo.Value.Minute)
                                                            .AddHours(Hours23)
                                                            .AddMinutes(Minutes59)
                                orderby src.BlastID
                                select src.BlastID).ToList();
            if (lBlastIds.Count == 0)
            {
                MessageBox.Show(MessageNoBlast);
                return;
            }

            ExportProcessBlasts(outFile, lBlastIds, _ => SetupBPAAuditByGroup(groupEntity));
            outFile.Close();
        }

        private void ExportProcessBlasts(
            StreamWriter outFile, 
            IEnumerable<int> lBlastIds, 
            Func<Blast, StreamWriter> createBpaFile)
        {
            Guard.NotNull(lBlastIds, nameof(lBlastIds));

            foreach (var blastId in lBlastIds)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    backgroundWorker1.ReportProgress(100, " Exiting..");
                    return;
                }
                var count = 0;
                try
                {
                    DataTable digitalSplitDt = null;
                    if (txtDigitalSplit.Text.Length > 0)
                    {
                        digitalSplitDt = GetDigitalSplit(
                            Path.GetDirectoryName(txtDigitalSplit.Text),
                            FileTypeC, 
                            openFileDialog1.SafeFileName,
                            Maxrecords);
                    }

                    outFile.WriteLine($"{DateTime.Now} Getting blast info for blast: {blastId}");
                    backgroundWorker1.ReportProgress(count, $"* Getting blast info for blast: {blastId}");
                    var blast = BusinessCommunicator.Blast.GetByBlastID_NoAccessCheck(blastId, false);

                    if (blast != null)
                    {
                        outFile.WriteLine($"{DateTime.Now} Setting up audit file");
                        backgroundWorker1.ReportProgress(count, "* Setting up audit file");

                        BPAFile = createBpaFile(blast);
                        if (FillBpaFile(outFile, blastId, blast, digitalSplitDt, ref count))
                        {
                            return;
                        }
                    }
                    else
                    {
                        outFile.WriteLine("No blast found to process");
                        backgroundWorker1.ReportProgress(count, "* No blast found to process");
                    }
                }
                catch (Exception exception)
                {
                    HandleGenereicExportException(exception, outFile, count);
                }
            }
        }

        private StreamWriter CreateOutFileForExport()
        {
            outLog = $"{ConfigurationManager.AppSettings[AppSettingAppLog]}{DateTime.Now.Date:d}.log";
            outLog = outLog.Replace(DelimSlash, string.Empty);
            var outFile = new StreamWriter(
                new FileStream(
                    Path.Combine(folderBrowserDialog1.SelectedPath, outLog), 
                    FileMode.Append));
            outFile.WriteLine($"Start Time : {DateTime.Now}");
            return outFile;
        }

        private void HandleGenereicExportException(Exception exception, StreamWriter outFile, int count)
        {
            Guard.NotNull(exception, nameof(exception));
            Guard.NotNull(outFile, nameof(outFile));

            var exx = exception.ToString();
            var errMsg = $"{OutFileErrorPrefix} {exx}";
            outFile.WriteLine(errMsg);
            backgroundWorker1.ReportProgress(count, errMsg);
            MessageBox.Show(errMsg);
        }

        private bool FillBpaFile(TextWriter outFile, int blastId, Blast blast, DataTable digitalSplitDt, ref int count)
        {
            if (BPAFile != null)
            {
                outFile.WriteLine($"{DateTime.Now} Getting sends for blast: {blastId}");
                backgroundWorker1.ReportProgress(count, $"* Getting sends for blast: {blastId}");
                var sendsList = ECN_Framework_BusinessLayer.Activity.BlastActivitySends.GetByBlastID(blastId);
                var bouncesList = ECN_Framework_BusinessLayer.Activity.BlastActivityBounces.GetByBlastID(blastId);
                if (sendsList != null && sendsList.Count > 0)
                {
                    if (ProcessExportSendList(blast, sendsList, bouncesList, ref count))
                    {
                        return true;
                    }

                    if (digitalSplitDt?.Rows.Count > 0)
                    {
                        outFile.WriteLine($"{DateTime.Now} Processing digital split for blast: {blastId}");
                        backgroundWorker1.ReportProgress(count, $"* Processing digital split for blast: {blastId}");

                        count = 0;
                        if (ProcessExportDigitalSplitTable(blast, digitalSplitDt, sendsList, ref count))
                        {
                            return true;
                        }
                    }

                    outFile.WriteLine("Export Completed");
                    backgroundWorker1.ReportProgress(count, "* Export Completed");
                    BPAFile.Close();
                }
                else
                {
                    outFile.WriteLine("No sends found to process");
                    backgroundWorker1.ReportProgress(count, "* No sends found to process");
                }
            }
            else
            {
                backgroundWorker1.ReportProgress(count, $"* Error creating BPALog file for blast: {blastId}");
                outFile.WriteLine($"Error creating BPALog file for blast: {blastId}");
            }

            return false;
        }

        private bool ProcessExportDigitalSplitTable(
            Blast blast,
            DataTable digitalSplitDt, 
            List<BlastActivitySends> sendsList, 
            ref int count)
        {
            Guard.NotNull(sendsList, nameof(sendsList));
            Guard.NotNull(digitalSplitDt, nameof(digitalSplitDt));

            long msgid = sendsList[0].SendID;
            foreach (DataRow row in digitalSplitDt.Rows)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    backgroundWorker1.ReportProgress(HundredPercent, " Exiting..");
                    return true;
                }

                count++;
                if (count % HundredPercent == 0)
                {
                    backgroundWorker1.ReportProgress(count, $"* Checking {count} records");
                }

                var sendsListFiltered = sendsList.FindAll(x => string.Equals(
                    x.EmailAddress,
                    row[0].ToString(),
                    StringComparison.OrdinalIgnoreCase));
                if (sendsListFiltered.Count <= 0)
                {
                    LogDigitalSplitRecord(BPAFile, row[0].ToString(), blast, ref msgid);
                }
            }

            return false;
        }

        private bool ProcessExportSendList(
            Blast blast,
            IEnumerable<BlastActivitySends> sendsList,
            List<BlastActivityBounces> bouncesList,
            ref int count)
        {
            Guard.NotNull(sendsList, nameof(sendsList));

            foreach (var send in sendsList)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    backgroundWorker1.ReportProgress(100, " Exiting..");
                    return true;
                }

                count++;
                if (count % HundredPercent == 0)
                {
                    backgroundWorker1.ReportProgress(count, $"* Writing {count} records");
                }

                var bouncesListFiltered = bouncesList.FindAll(x => x.EmailID == send.EmailID);
                if (bouncesListFiltered.Count > 0)
                {
                    LogRecord(BPAFile, send, bouncesListFiltered[0], blast);
                }
                else
                {
                    LogRecord(BPAFile, send, null, blast);
                }
            }

            return false;
        }

        private long getNewMessageID(long msgID)
        {
            msgID = msgID + r1.Next(100, 999);
            return msgID;
        }

        public void LogDigitalSplitRecord(StreamWriter BPAFile, string EmailAddress, ECN_Framework_Entities.Communicator.Blast blast, ref long msgid)
        {
            string year = Convert.ToDateTime(blast.SendTime).Year.ToString();
            string month = Convert.ToDateTime(blast.SendTime).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            msgid = getNewMessageID(msgid);
            string output = "MSGID[" + msgid.ToString() + "] ISSUE[" + year + month + "1] ";
            if (rbGroup.Checked)
                BPAFile.WriteLine(output + "BLASTID: " + blast.BlastID.ToString());
            BPAFile.WriteLine(output + "DATE: " + Convert.ToDateTime(blast.SendTime).ToString("ddd MMM dd HH:mm:ss.fff yyyy"));
            BPAFile.WriteLine(output + "FROM: " + blast.EmailFrom);
            BPAFile.WriteLine(output + "TO: " + EmailAddress);
            BPAFile.WriteLine(output + "SUBJECT: " + blast.EmailSubject);
            BPAFile.WriteLine(output + "Error: - Delivery expire (message too old) timeout. ");
        }

        public void LogRecord(StreamWriter BPAFile, ECN_Framework_Entities.Activity.BlastActivitySends send, ECN_Framework_Entities.Activity.BlastActivityBounces bounce, ECN_Framework_Entities.Communicator.Blast blast)
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
            customerName = customerName.Replace(":", " ");

            return customerName;
        }

        private StreamWriter SetupBPAAuditByGroup(ECN_Framework_Entities.Communicator.Group group)
        {
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(group.CustomerID, false);
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

        private StreamWriter SetupBPAAudit(string customerName, ECN_Framework_Entities.Communicator.Blast blast)
        {
            StreamWriter BPAFile = null;
            try
            {
                if (!Directory.Exists(folderBrowserDialog1.SelectedPath + "\\" + CleanCustomerName(customerName)))
                {
                    Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "\\" + CleanCustomerName(customerName));
                }
                string year = blast.SendTime.Value.Year.ToString();
                string month = blast.SendTime.Value.Month.ToString();
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

        private void Port25_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main2 frm = (Main2)this.MdiParent;
            frm.ToggleMenus(true);
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
                rbGroup.Checked = false;
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
