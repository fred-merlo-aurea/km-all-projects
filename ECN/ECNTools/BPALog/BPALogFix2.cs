using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using ECN_Framework_BusinessLayer.Activity;
using ECN_Framework_BusinessLayer.Communicator;
using KM.Common.Tools;
using CommunicatorBlast = ECN_Framework_Entities.Communicator.Blast;
using EntityBlastActivityBounces = ECN_Framework_Entities.Activity.BlastActivityBounces;

namespace ECNTools.BPALog
{
    public partial class BPALogFix2 : BPALogFixBase
    {
        private const string UnderScore = "_";
        private const int InvalidBlastId = -1;
        private const string InBpaFile = "===== IN BPA FILE =====";
        private const string SeperatorLine = "=======================";
        private const string ErrorMessageInvalidBpaFile = "Invalid BPA File";
        private const string EmailSentSuccessfullyToServer = "250 Email Sent Successfully to server ";
        private const string MessageId = "MSGID";
        private const string Issue = "ISSUE";
        private const string MessageSendPrefix = "250";
        private const string Exiting = " Exiting..";

        public BPALogFix2()
        {
            InitializeComponent();
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            lstMessage.Items.Clear();

            if (txtBPAFileName.Text != string.Empty && txtBPAFileName.Text.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
            {
                if (txtFolderLocation.Text == String.Empty)
                {
                    MessageBox.Show("Select Folder to save the File.");
                    return;
                }

                btnStart.Enabled = false;
                btnCancel.Enabled = true;

                lstMessage.Items.Add("Start Time : " + System.DateTime.Now);

                backgroundWorker1.RunWorkerAsync();

            }
            else
            {
                MessageBox.Show("Select TXT File");
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadDatafromBPAFile();

            backgroundWorker1.ReportProgress(100, "End Time : " + System.DateTime.Now);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().ToLower().StartsWith("textbox:"))
                txtMessage.Text = txtMessage.Text + e.UserState.ToString().Replace("textbox:", "") + Environment.NewLine;
            else
                lstMessage.Items.Add(e.UserState);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.Enabled = true;
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
                txtBPAFileName.Text = Path.GetFileName(openFileDialog1.FileName);
                lstMessage.Items.Add("* Filepath: " + openFileDialog1.FileName);


                if (txtBPAFileName.Text != string.Empty && txtBPAFileName.Text.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        int BlastID = Convert.ToInt32(Path.GetFileName(txtBPAFileName.Text).Substring(0, Path.GetFileName(txtBPAFileName.Text).IndexOf("_")));
                        getBlastReport(BlastID);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Invalid BlastID");
                    }
                }
                else
                {
                    MessageBox.Show("Select TXT File");
                }
            }
        }

        public void getBlastReport(int BlastID)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString);

            SqlCommand cmd = new SqlCommand("sp_getBlastReportData", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@blastID", BlastID);
            cmd.Parameters.AddWithValue("@UDFname", "");
            cmd.Parameters.AddWithValue("@UDFdata", "");


            cmd.Connection.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);

            // Populate a new data table and bind it to the BindingSource.
            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);

            DataView dsView = table.DefaultView;

            dsView.RowFilter = "ActionTypecode='send' or ActionTypecode='bounce'";
            dsView.Sort = "ActionTypecode desc";

            bindingSource1.DataSource = dsView;

            // Resize the DataGridView columns to fit the newly loaded content.
            gridBlastReport.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCells);

            conn.Close();

            gridBlastReport.ReadOnly = true;
            gridBlastReport.AutoGenerateColumns = true;
            gridBlastReport.DataSource = bindingSource1;
            

        }

        private void ReadDatafromBPAFile()
        {
            var blastId = GetBlastIdFromFileName(txtBPAFileName.Text);

            var count = 0;

            if (blastId > 0)
            {
                try
                {
                    var issueId = -1;
                    var blastAbstract = Blast.GetByBlastID_NoAccessCheck(blastId, false);

                    var activityBounceses = BlastActivityBounces.GetByBlastID(blastId);
                    var activitySendses = BlastActivitySends.GetByBlastID(blastId);
                    var domainIps = new Dictionary<string, string>();

                    var bounces = CreateBounces(activityBounceses);

                    var sends = activitySendses.ToDictionary(
                        activitySends => activitySends.EmailAddress.ToLower(),
                        activitySends => activitySends.SendID);

                    var uniqueEmails = new List<string>();

                    if (!ProcessInputFile(uniqueEmails, sends, bounces, domainIps, ref count, ref issueId))
                    {
                        return;
                    }

                    var filename = $"{blastAbstract.BlastID}_{blastAbstract.SendTime.Value.Year}_{blastAbstract.SendTime.Value.Month}_{blastAbstract.SendTime.Value.Day}.txt";
                    var fullPath = Path.Combine(folderBrowserDialog1.SelectedPath, filename);
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }

                    count = GenerateBpaFile(fullPath, sends, issueId, blastAbstract, bounces, domainIps, count);
                }
                catch (Exception exception)
                {
                    var errorMessage = $"[ERROR]: {exception}";
                    backgroundWorker1.ReportProgress(count, errorMessage);
                    MessageBox.Show(errorMessage);
                }
            }
            else
            {
                MessageBox.Show(ErrorMessageInvalidBpaFile);
            }
        }

        private bool ProcessInputFile(
            ICollection<string> uniqueEmails,
            IDictionary<string, int> sends,
            IReadOnlyDictionary<string, string> bounces,
            IDictionary<string, string> domainIps,
            ref int count,
            ref int issueId)
        {
            using (var streamReader = new StreamReader(openFileDialog1.FileName))
            {
                var lineNumber = 0;
                var subscriber = string.Empty;
                string line;
                var records = 0;
                var bfirst = true;

                while ((line = streamReader.ReadLine()) != null)
                {
                    count++;

                    if (backgroundWorker1.CancellationPending)
                    {
                        backgroundWorker1.ReportProgress(100, Exiting);
                        return false;
                    }

                    if (bfirst)
                    {
                        bfirst = false;

                        line = line.Substring(line.IndexOf("]", StringComparison.Ordinal) + 1);
                        issueId = Convert.ToInt32(
                            line.Substring(
                                line.IndexOf("[", StringComparison.Ordinal) + 1,
                                line.IndexOf("]", StringComparison.Ordinal) - (line.IndexOf("[", StringComparison.Ordinal) + 1)));
                    }

                    lineNumber++;

                    switch (lineNumber)
                    {
                        case 3:
                            subscriber = line.Substring(line.IndexOf("TO:", StringComparison.Ordinal) + 4);
                            if (!uniqueEmails.Contains(subscriber.ToLower()))
                            {
                                uniqueEmails.Add(subscriber.ToLower());
                            }

                            break;
                        case 5:
                            records++;

                            ProcessLine5(sends, subscriber, line, bounces, issueId, domainIps);
                            lineNumber = 0;
                            break;
                    }
                }

                ReportProgress(count, records, uniqueEmails.Count);
            }

            return true;
        }

        private static void ProcessLine5(
            IDictionary<string, int> sends,
            string subscriber,
            string line,
            IReadOnlyDictionary<string, string> bounces,
            int issueId,
            IDictionary<string, string> domainIps)
        {
            if (!sends.ContainsKey(subscriber.ToLower()))
            {
                return;
            }

            sends[subscriber.ToLower()] = Convert.ToInt32(
                line.Substring(
                    line.IndexOf("[", StringComparison.Ordinal) + 1,
                    line.IndexOf("]", StringComparison.Ordinal) - (line.IndexOf("[", StringComparison.Ordinal) + 1)));

            if (bounces.ContainsKey(subscriber.ToLower()))
            {
                return;
            }

            var tmpline = new StringBuilder(line);
            tmpline = tmpline.Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Replace(MessageId, string.Empty)
                .Replace(Issue, string.Empty)
                .Replace(sends[subscriber.ToLower()].ToString(), string.Empty)
                .Replace(issueId.ToString(), string.Empty);

            if (!tmpline.ToString()
                    .Trim()
                    .StartsWith(MessageSendPrefix))
            {
                return;
            }

            tmpline = tmpline
                .Replace(EmailSentSuccessfullyToServer, string.Empty)
                .Replace(" ", string.Empty);

            var ipBuilder = tmpline;

            var domainIp = subscriber
                .Substring(subscriber.IndexOf("@", StringComparison.Ordinal) + 1)
                .ToLower();

            if (!domainIps.ContainsKey(domainIp))
            {
                domainIps.Add(domainIp, ipBuilder.ToString());
            }
        }

        private static Dictionary<string, string> CreateBounces(IEnumerable<EntityBlastActivityBounces> activityBounceses)
        {
            var bounces = new Dictionary<string, string>();
            foreach (var activityBounces in activityBounceses)
            {
                var emailIdNoAccessCheck = Email.GetByEmailID_NoAccessCheck(activityBounces.EmailID);
                if (!bounces.ContainsKey(emailIdNoAccessCheck.EmailAddress.ToLower()))
                {
                    bounces.Add(emailIdNoAccessCheck.EmailAddress.ToLower(), activityBounces.BounceMessage);
                }
            }

            return bounces;
        }

        private int GenerateBpaFile(
            string fullPath,
            Dictionary<string, int> sends,
            int issueId,
            CommunicatorBlast blast,
            IReadOnlyDictionary<string, string> bounces,
            IDictionary<string, string> domainIps,
            int count)
        {
            var newCount = count;
            using (var streamWriter = new StreamWriter(fullPath))
            {
                backgroundWorker1.ReportProgress(newCount, $"Generating NEW BPA File with {sends.Count} records");

                foreach (var entry in sends)
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        backgroundWorker1.ReportProgress(100, " Exiting..");
                        return newCount;
                    }

                    newCount = ProcessSend(issueId, blast, bounces, domainIps, entry, streamWriter, newCount);
                }

                streamWriter.Close();
            }

            return newCount;
        }

        private int ProcessSend(
            int issueId,
            CommunicatorBlast blast,
            IReadOnlyDictionary<string, string> bounces,
            IDictionary<string, string> domainIps,
            KeyValuePair<string, int> entry,
            TextWriter streamWriter,
            int newCount)
        {
            var sdomain = entry.Key.ToLower()
                .Substring(entry.Key.IndexOf("@", StringComparison.Ordinal) + 1);

            var messageIdIssued = $"MSGID[{entry.Value}] ISSUE[{issueId}]";
            streamWriter.WriteLine($"{messageIdIssued} DATE: {blast.SendTime.Value:ddd MMM dd HH:mm:ss.fff yyyy}");
            streamWriter.WriteLine($"{messageIdIssued} FROM: {blast.EmailFrom}");
            streamWriter.WriteLine($"{messageIdIssued} TO: {entry.Key}");
            streamWriter.WriteLine($"{messageIdIssued} SUBJECT: {blast.EmailSubject}");

            if (bounces.ContainsKey(entry.Key.ToLower()))
            {
                streamWriter.WriteLine(
                    $"{messageIdIssued} {bounces[entry.Key.ToLower()] .Replace(Environment.NewLine, string.Empty)}");
            }
            else
            {
                if (domainIps.ContainsKey(sdomain.ToLower()))
                {
                    streamWriter.WriteLine(
                        $"{messageIdIssued} 250 Email Sent Successfully to server [{domainIps[sdomain.ToLower()]}]");
                }
                else
                {
                    try
                    {
                        var ipEntry = Dns.GetHostEntry(sdomain);
                        var addr = ipEntry.AddressList;

                        if (!domainIps.ContainsKey(sdomain.ToLower()))
                        {
                            domainIps.Add(
                                sdomain.ToLower(),
                                addr[0].ToString());
                        }

                        streamWriter.WriteLine($"{messageIdIssued} 250 Email Sent Successfully to server [{domainIps[sdomain.ToLower()]}]");
                    }
                    catch
                    {
                        if (!domainIps.ContainsKey(sdomain.ToLower()))
                        {
                            domainIps.Add(sdomain.ToLower(), $"NO IP ADDRESS {sdomain}");
                        }

                        streamWriter.WriteLine($"{messageIdIssued} 250 Email Sent Successfully to server [{"NO IP ADDRESS " + sdomain}]");
                        backgroundWorker1.ReportProgress(newCount++, $"textbox:IP address not exists for {sdomain}");
                    }
                }
            }

            return newCount;
        }

        private void ReportProgress(int count, int records, int uniqueEmailsCount)
        {
            backgroundWorker1.ReportProgress(count, string.Empty);
            backgroundWorker1.ReportProgress(count, InBpaFile);
            backgroundWorker1.ReportProgress(count, $" Total  Records = {records}");
            backgroundWorker1.ReportProgress(count, $" Unique Emails  = {uniqueEmailsCount}");
            backgroundWorker1.ReportProgress(count, SeperatorLine);
            backgroundWorker1.ReportProgress(count, string.Empty);
        }

        private static int GetBlastIdFromFileName(string fileName)
        {
            try
            {
                int blastId;
                var file = Path.GetFileName(fileName);
                var numericFileNamePart = file?.Substring(0, file.IndexOf(UnderScore, StringComparison.Ordinal));

                if (int.TryParse(numericFileNamePart, out blastId))
                {
                    return blastId;
                }
            }
            catch (Exception exception) when (exception is ArgumentException)
            {
                // TODO : log exception
            }
           
            return InvalidBlastId;
        }
    }
}
