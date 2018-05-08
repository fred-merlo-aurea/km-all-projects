using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Windows.Forms;
using KM.Common.Tools;
using KMPS_Tools.Classes;

namespace KMPS_Tools
{
    public partial class BPALogFix : BPALogFixBase
    {
        public BPALogFix()
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
                txtMessage.Text = txtMessage.Text + e.UserState.ToString().Replace("textbox:","") + Environment.NewLine;
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
                lstMessage.Items.Add("* " + columnname + " not exists " );
            }

            return string.Empty;
        }
        #endregion

        private void btnChooseDBF_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtBPAFileName.Text= Path.GetFileName(openFileDialog1.FileName);
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

            gridBlastReport.AutoGenerateColumns = true;
            gridBlastReport.DataSource = bindingSource1;

        }

        private void ReadDatafromBPAFile()
        {
            var fileName = Path.GetFileName(txtBPAFileName.Text);
            var blastId = 0;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                int.TryParse(fileName.Substring(0, fileName.IndexOf("_")), out blastId);
            }

            var args = new BpaFileArgs();

            if (blastId > 0)
            {
                try
                {
                    var blast = Blast.getBlastDetails(blastId);
                    args.Blast = blast;
                    args.BounceDictionary = blast.getBounces(blastId);
                    args.SendDictionary = blast.getSends(blastId);

                    ReadFileLines(args);

                    var filename = $"{blast.BlastID}_{blast.sendTime.Year}_{blast.sendTime.Month}_{blast.sendTime.Day}.txt";
                    var path = $"{folderBrowserDialog1.SelectedPath}\\{filename}";

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    WriteToFile(args, path);

                }
                catch (Exception ex)
                {
                    var errorMessage = ex.ToString();
                    backgroundWorker1.ReportProgress(args.Count++, $"[ERROR]: {errorMessage}");
                    MessageBox.Show($"[ERROR]: {errorMessage}");
                }
            }
            else
            {
                MessageBox.Show("Invalid BPA File");
            }
        }

        private void WriteToFile(BpaFileArgs args, string path)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            using (var writer = new StreamWriter(path))
            {
                backgroundWorker1.ReportProgress(args.Count, $"Generating NEW BPA File with {args.SendDictionary.Count} records") ;

                foreach (var entry in args.SendDictionary)
                {
                    var entryKey = entry.Key.ToLower();
                    var domain = entryKey.Substring(entry.Key.IndexOf("@") + 1);

                    writer.WriteLine(
                        "MSGID[{0}] ISSUE[{1}] DATE: {2}",
                        entry.Value,
                        args.Blast.IssueID,
                        args.Blast.sendTime.ToString("ddd MMM dd HH:mm:ss.fff yyyy"));
                    writer.WriteLine("MSGID[{0}] ISSUE[{1}] FROM: {2}", entry.Value, args.Blast.IssueID, args.Blast.EmailFrom);
                    writer.WriteLine("MSGID[{0}] ISSUE[{1}] TO: {2}", entry.Value, args.Blast.IssueID, entry.Key);
                    writer.WriteLine("MSGID[{0}] ISSUE[{1}] SUBJECT: {2}", entry.Value, args.Blast.IssueID, args.Blast.Subject);

                    if (args.BounceDictionary.ContainsKey(entryKey))
                    {
                        writer.WriteLine(
                            "MSGID[{0}] ISSUE[{1}] {2}",
                            entry.Value,
                            args.Blast.IssueID,
                            args.BounceDictionary[entryKey].Replace(Environment.NewLine, string.Empty));
                    }
                    else
                    {
                        if (args.IpDictionary.ContainsKey(domain))
                        {
                            writer.WriteLine(
                                "MSGID[{0}] ISSUE[{1}] 250 Email Sent Successfully to server [{2}]",
                                entry.Value,
                                args.Blast.IssueID,
                                args.IpDictionary[domain]);
                        }
                        else
                        {
                            WriteForNewDomain(args, writer, domain, entry.Value);
                        }
                    }
                }
            }
        }

        private void WriteForNewDomain(BpaFileArgs args, StreamWriter writer, string domain, int messageId)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            try
            {
                var ipEntry = Dns.GetHostEntry(domain);
                var addressList = ipEntry.AddressList;

                if (!args.IpDictionary.ContainsKey(domain))
                {
                    args.IpDictionary.Add(domain, addressList[0].ToString());
                }

                writer.WriteLine(
                    "MSGID[{0}] ISSUE[{1}] 250 Email Sent Successfully to server [{2}]",
                    messageId,
                    args.Blast.IssueID,
                    args.IpDictionary[domain]);
            }
            catch
            {
                if (!args.IpDictionary.ContainsKey(domain))
                {
                    args.IpDictionary.Add(domain, $"NO IP ADDRESS {domain}");
                }

                writer.WriteLine(
                    "MSGID[{0}] ISSUE[{1}] 250 Email Sent Successfully to server [{2}]",
                    messageId,
                    args.Blast.IssueID,
                    $"NO IP ADDRESS {domain}");
                backgroundWorker1.ReportProgress(args.Count++, $"textbox:IP address not exists for {domain}");
            }
        }

        private void ReadFileLines(BpaFileArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var uniqueEmails = new List<string>();

            using (var reader = new StreamReader(openFileDialog1.FileName))
            {
                var lineNumber = 0;
                var totalRecords = 0;
                var isFirst = true;

                while ((args.Line = reader.ReadLine()) != null)
                {
                    args.Count++;

                    if (backgroundWorker1.CancellationPending)
                    {
                        backgroundWorker1.ReportProgress(args.Count, " Exiting..");
                        break;
                    }

                    if (isFirst)
                    {
                        //get Blast info
                        isFirst = false;
                        args.Line = args.Line.Substring(args.Line.IndexOf("]") + 1);
                        args.Blast.IssueID = GetIssueId(args.Line);
                    }

                    lineNumber++;

                    switch (lineNumber)
                    {
                        case 3:
                            //get Emailaddress
                            args.Subscriber = args.Line.Substring(args.Line.IndexOf("TO:") + 4);
                            if (!uniqueEmails.Contains(args.Subscriber.ToLower()))
                            {
                                uniqueEmails.Add(args.Subscriber.ToLower());
                            }

                            break;
                        case 5:
                            //get delivery message
                            totalRecords++;
                            SetDeliverMessage(args);
                            lineNumber = 0;
                            break;
                    }
                }

                backgroundWorker1.ReportProgress(args.Count, string.Empty);
                backgroundWorker1.ReportProgress(args.Count, "===== IN BPA FILE =====");
                backgroundWorker1.ReportProgress(args.Count, $" Total  Records = {totalRecords}");
                backgroundWorker1.ReportProgress(args.Count, $" Unique Emails  = {uniqueEmails.Count}");
                backgroundWorker1.ReportProgress(args.Count, "=======================");
                backgroundWorker1.ReportProgress(args.Count, string.Empty);
            }
        }

        private int GetIssueId(string line)
        {
            if (line == null)
            {
                throw new ArgumentNullException(nameof(line));
            }

            int issueId;
            var start = line.IndexOf("[") + 1;
            var length = line.IndexOf("]") - start;

            if (length <= 0)
            {
                throw new InvalidOperationException($"Unable parse issue ID from current line.");
            }

            var text = line.Substring(start, length);

            if (!int.TryParse(text, out issueId))
            {
                throw new InvalidOperationException($"Unable parse issue ID from '{text}'");
            }

            return issueId;
        }

        private void SetDeliverMessage(BpaFileArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var subscriber = args.Subscriber.ToLower();
            if (!args.SendDictionary.ContainsKey(subscriber))
            {
                return;
            }

            args.SendDictionary[subscriber] = GetIssueId(args.Line);

            if (!args.BounceDictionary.ContainsKey(subscriber))
            {
                var tmpLine = args.Line;
                tmpLine = tmpLine.Replace("[", string.Empty);
                tmpLine = tmpLine.Replace("]", string.Empty);
                tmpLine = tmpLine.Replace("MSGID", string.Empty);
                tmpLine = tmpLine.Replace("ISSUE", string.Empty);
                tmpLine = tmpLine.Replace(args.SendDictionary[subscriber].ToString(), string.Empty);
                tmpLine = tmpLine.Replace(args.Blast.IssueID.ToString(), string.Empty);

                if (tmpLine.Trim().StartsWith("250"))
                {
                    tmpLine = tmpLine.Replace("250 Email Sent Successfully to server ", string.Empty);
                    tmpLine = tmpLine.Replace(" ", string.Empty);

                    var ip = tmpLine;
                    var domain = subscriber.Substring(args.Subscriber.IndexOf("@") + 1);
                    if (!args.IpDictionary.ContainsKey(domain))
                    {
                        args.IpDictionary.Add(domain, ip);
                    }
                }
            }
        }
    }
}
