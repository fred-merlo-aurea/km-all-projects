using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.IO;
using System.Data;
using System.Data.OleDb;

namespace GreenArrow_SMTPLog
{
    public static class Helper
    {
        public static void NotifyAdmin(string pager_subject, string errorMsg)
        {
            MailMessage message = new MailMessage(ConfigurationManager.AppSettings["FromEmail"], ConfigurationManager.AppSettings["ToEmail"], pager_subject, errorMsg);
            if (ConfigurationManager.AppSettings["CCEmail"].Trim().Length > 0)
            {
                message.CC.Add(ConfigurationManager.AppSettings["CCEmail"]);
            }
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"]);
            client.Send(message);
        }

        public static StreamWriter SetupBPAAudit(string customerName, Models.Blasts blast)
        {
            StreamWriter BPAFile = null;
            try
            {                
                if (!Directory.Exists("Output\\" + CleanCustomerName(customerName)))
                {
                    Directory.CreateDirectory("Output\\" + CleanCustomerName(customerName));
                }
                string year = Convert.ToDateTime(blast.SendTime).Year.ToString();
                string month = Convert.ToDateTime(blast.SendTime).Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                string BPALog = "Output\\" + CleanCustomerName(customerName) + "\\" + Convert.ToInt32(blast.BlastID).ToString() + "_" + year + "_" + month + ".txt";
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

        public static string CleanCustomerName(string customerName)
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

        public static void LogRecord(ref StreamWriter BPAFile, Models.EmailActivityLog eal, DataRow row, Models.Blasts blast)
        {
            string year = Convert.ToDateTime(blast.SendTime).Year.ToString();
            string month = Convert.ToDateTime(blast.SendTime).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string output = "MSGID[" + eal.EAID.ToString() + "] ISSUE[" + year + month + "1] ";
            BPAFile.WriteLine(output + "DATE: " + Convert.ToDateTime(blast.SendTime).ToString("ddd MMM dd HH:mm:ss.fff yyyy"));
            BPAFile.WriteLine(output + "FROM: " + blast.EmailFrom);
            BPAFile.WriteLine(output + "TO: " + eal.EmailName + "@" + eal.EmailDomain);
            BPAFile.WriteLine(output + "SUBJECT: " + blast.EmailSubject);
            if (row["Status"].ToString() == "accepted")
            {
                string comments = row["Comments"].ToString();
                comments = comments.Substring(0, comments.IndexOf(" "));
                BPAFile.WriteLine(output + "250 Email Sent Successfully to server [" + comments + "].");
            }
            else
            {
                BPAFile.WriteLine(output + "Error: " + row["ErrorMsg"].ToString());
            }
        }

        public static void LogRecordUsingEAL(ref StreamWriter BPAFile, Models.EmailActivityLog eal, DataRow row, Models.Blasts blast)
        {
            string year = Convert.ToDateTime(blast.SendTime).Year.ToString();
            string month = Convert.ToDateTime(blast.SendTime).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string output = "MSGID[" + eal.EAID.ToString() + "] ISSUE[" + year + month + "1] ";
            BPAFile.WriteLine(output + "DATE: " + Convert.ToDateTime(blast.SendTime).ToString("ddd MMM dd HH:mm:ss.fff yyyy"));
            BPAFile.WriteLine(output + "FROM: " + blast.EmailFrom);
            BPAFile.WriteLine(output + "TO: " + eal.EmailName + "@" + eal.EmailDomain);
            BPAFile.WriteLine(output + "SUBJECT: " + blast.EmailSubject);
            BPAFile.WriteLine(output + "Error: " + eal.ActionNotes);
        }

        public static string GetGALogFile(int blastID, ref DateTime startTime, ref DateTime endTime)
        {
            string Url = ConfigurationManager.AppSettings["GAURL"] + "sendid=" + blastID.ToString() + "&limit=0&format=csv";
            string gaFile = ConfigurationManager.AppSettings["GAFile_"] + blastID.ToString() + "_" + DateTime.Now.Date.ToString("d") + ".csv";
            gaFile = gaFile.Replace(@"/", "");
            if (File.Exists(gaFile))
            {
                File.Delete(gaFile);
            }

            // *** Establish the request 
            HttpWebRequest Http = (HttpWebRequest)WebRequest.Create(Url);

            // *** Set properties
            Http.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["GATimeout"].ToString());     
            Http.Method = "POST";

            Http.PreAuthenticate = false;
            byte[] credentialsAuth = new UTF8Encoding().GetBytes(ConfigurationManager.AppSettings["GAUserID"] + ":" + ConfigurationManager.AppSettings["GAPassword"]);

            Http.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialsAuth);

            // *** Retrieve request info headers

            startTime = DateTime.Now;
            HttpWebResponse WebResponse = (HttpWebResponse)Http.GetResponse();

            Encoding enc = Encoding.GetEncoding(1252);  // Windows default Code Page

            StreamReader ResponseStream = new StreamReader(WebResponse.GetResponseStream(), enc);

            string data = ResponseStream.ReadToEnd();

            WebResponse.Close();
            ResponseStream.Close();
            endTime = DateTime.Now;

            using (StreamWriter outfile =
            new StreamWriter(gaFile))
            {
                outfile.Write(data);
            }

            return gaFile;
        }

        public static DataTable GetGALogTable(string gaFile)
        {
            string path = System.IO.Path.GetDirectoryName(gaFile);
            DataTable xlsDataTable = new DataTable("Logs");
            string file = gaFile;
            try
            {
                
                DataColumn dc = new DataColumn();
                dc.ColumnName = "LogDate";
                xlsDataTable.Columns.Add(dc);
                dc = new DataColumn();
                dc.ColumnName = "EmailName";
                xlsDataTable.Columns.Add(dc);
                dc = new DataColumn();
                dc.ColumnName = "EmailDomain";
                xlsDataTable.Columns.Add(dc);
                dc = new DataColumn();
                dc.ColumnName = "MTA";
                xlsDataTable.Columns.Add(dc);
                dc = new DataColumn();
                dc.ColumnName = "Status";
                xlsDataTable.Columns.Add(dc);
                dc = new DataColumn();
                dc.ColumnName = "LogDate2";
                xlsDataTable.Columns.Add(dc);
                dc = new DataColumn();
                dc.ColumnName = "ErrorMsg";
                xlsDataTable.Columns.Add(dc);
                dc = new DataColumn();
                dc.ColumnName = "Domain";
                xlsDataTable.Columns.Add(dc);
                dc = new DataColumn();
                dc.ColumnName = "Comments";
                xlsDataTable.Columns.Add(dc);


                StreamReader sr = new StreamReader(path + file);
                string line;
                List<string> listS = new List<string>();
                int linecount = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace("\",\"", "^");
                    line = line.Replace("\"", "");

                    string[] fields = line.ToString().Split('^');

                    try
                    {
                        DataRow dr = xlsDataTable.NewRow();
                        for (int i = 0; i < fields.Length; i++)
                        {
                            dr[i] = fields[i].ToString();
                        }
                        xlsDataTable.Rows.Add(dr);
                    }
                    catch (Exception ex)
                    {

                    }

                    linecount++;
                }
                sr.Close();

                xlsDataTable.AcceptChanges();
                
            }
            catch (Exception ex)
            {
            }

            if (File.Exists(path + file))
            {
                File.Delete(path + file);
            }
            return xlsDataTable;

        }

    }
}
