using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Net.Mail;

namespace PersonifyToECNEngine
{
    class Engine
    {
        private string _ReportLogFile = String.Format("{0}\\PersonifyToECNEngine_log_{1}.log", ConfigurationManager.AppSettings["ReportLogFilePath"], DateTime.Now.ToString("MM_dd_yyyy"));
        private string _ReportDirectory = ConfigurationManager.AppSettings["ReportLocation"];
        private int _StartDay = Convert.ToInt32(ConfigurationManager.AppSettings["StartDay"]);

        static void Main(string[] args)
        {
            Console.Title = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            new Engine().RunEngine();
        }

        public void RunEngine()
        {
            WriteStatus(string.Format("Personify Engine({0}) started", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));
            try
            {
                CleanupOldFiles();
                ExportData();
                WriteStatus(string.Format("Personify Engine({0}) completed", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));
            }
            catch (Exception e)
            {
                SendErrorNotification(e.ToString());
                WriteStatus(e.Message);
            }
        }

        private void WriteStatus(string message)
        {
            message = DateTime.Now.ToString() + "   " + message;
            Console.WriteLine(message);
            string temp = _ReportLogFile;
            using (StreamWriter file = new StreamWriter(new FileStream(_ReportLogFile, System.IO.FileMode.Append)))
            {
                file.WriteLine(message);
                file.Close();
            }
        }

        private void CleanupOldFiles()
        {
            WriteStatus("Deleting old reports");
            
            DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings.Get("ReportLocation"));
            FileInfo[] rgFiles = di.GetFiles("*.csv");
            foreach (FileInfo fi in rgFiles)
            {
                fi.Refresh();
                if (fi.CreationTime < DateTime.Now.AddDays(-30))
                {
                    WriteStatus("Deleting file: " + fi.Name);
                    fi.Delete();
                }
            }
        }

        private void ExportData()
        {
            DateTime reportTime = DateTime.Now;
            string reportName = "KMExport_" + reportTime.Month.ToString() + reportTime.Day.ToString() + reportTime.Year.ToString() + ".csv";
            string reportFullLocalPath = _ReportDirectory + "\\" + reportName;

            //get report data
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_km_getdata";
            //cmd.Parameters.AddWithValue("@StartDate", startDate);
            DataTable dtReport = GetDataTable(cmd);

            //create report file
            System.IO.File.WriteAllText(reportFullLocalPath, ToCSV(dtReport));

            //FTP report
            FTPReport(reportName, reportFullLocalPath);
        }

        private DataTable GetDataTable(SqlCommand cmd)
        {
            WriteStatus("Getting data");
            cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Personify"].ToString());
            cmd.CommandTimeout = 0;
            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            dt.Load(dr);
            cmd.Connection.Close();

            return dt;
        }

        private string ToCSV(DataTable table)
        {
            WriteStatus("Creating CSV");

            var result = new StringBuilder();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append(table.Columns[i].ColumnName);
                result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    result.Append(row[i].ToString());
                    result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
                }
            }

            return result.ToString();
        }

        private void SendErrorNotification(string error)
        {
            WriteStatus("Sending error notification");
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["KMEmailFrom"]);
            message.To.Add(ConfigurationManager.AppSettings["KMEmailTo"]);
            message.Subject = "Severity 1 Application Error for Personify Application";
            message.Body = error;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["PersonifySMTPServer"]);
            client.Send(message);
        }

        private void FTPReport(string reportName, string reportLocation)
        {
            WriteStatus("FTPing " + reportName);

            string userName = ConfigurationManager.AppSettings["FTPUserID"];
            string password = ConfigurationManager.AppSettings["FTPPassword"];
            string server = ConfigurationManager.AppSettings["FTPServer"];
            //FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(server + "/" + reportName));
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(server + "/" + reportName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = true;
            request.Credentials = new NetworkCredential(userName, password);

            StreamReader sourceStream = new StreamReader(reportLocation);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            WriteStatus(string.Format("Remote Response: {0}", response.StatusDescription));

            response.Close();
        }

    }

}
