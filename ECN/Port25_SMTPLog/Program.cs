using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Xml;
using System.Configuration;
using System.Text.RegularExpressions;
using KM.Common;

namespace Port25_SMTPLog
{
    class Program
    {
        private static string userName = ConfigurationManager.AppSettings["ftpusername"];
        private static string password = ConfigurationManager.AppSettings["ftppassword"];
        private static string server = ConfigurationManager.AppSettings["ftpserver"];
        private static int DaytoProcess = Convert.ToInt32(ConfigurationManager.AppSettings["DaytoProcess"]);
        private static string StartsWith = "acct-d-" + DateTime.Now.AddDays(DaytoProcess).Year.ToString() + "-" + DateTime.Now.AddDays(DaytoProcess).Month.ToString("#00") + "-" + DateTime.Now.AddDays(DaytoProcess).ToString("dd");

        static string DownloadPath = Environment.CurrentDirectory + "\\Downloads\\";
        static int numberoffiles = 1;

        static void Main(string[] args)
        {
            string ProcessAllFilesinFolder = ConfigurationManager.AppSettings["ProcessAllFilesinFolder"];
            if (ProcessAllFilesinFolder.ToLower().Equals("false"))
            {
                DeleteOldFiles();
                GetFiles();
                if (numberoffiles > 0)
                {
                    FileFunctions.LogConsoleActivity("Number of files: " + numberoffiles.ToString());
                    DataTable dt = new DataTable();
                    for (int i = 0; i < numberoffiles; i++)
                    {
                        ConstructSchema(new FileInfo(DownloadPath + StartsWith + "-" + i.ToString("#0000") + ".csv"));
                        insertDB(ParseFile(DownloadPath + StartsWith + "-" + i.ToString("#0000") + ".csv"));
                        FileFunctions.LogConsoleActivity("Completed " + StartsWith + "-" + i.ToString("#0000") + ".csv" + " at " + DateTime.Now);
                    }
                    //add call to new proc for bulk update
                    FileFunctions.LogConsoleActivity("Doing bulk update - " + DateTime.Now);
                    try
                    {
                        ECN_Framework_BusinessLayer.Activity.BlastActivitySends.doBulkUpdate_SMTPMessage();
                    }
                    catch(Exception ex)
                    {
                        KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Port25_SMTPLog.doBulkUpdate_SMPTMessage", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                    }
                    FileFunctions.LogConsoleActivity("Done with bulk update - " + DateTime.Now);

                    FileFunctions.LogConsoleActivity("Completed " + DateTime.Now);
                }
                else
                {
                    FileFunctions.LogConsoleActivity("No Files - " + DateTime.Now);
                }
            }
            else
            {
                DirectoryInfo DirInfo = new DirectoryInfo(DownloadPath);
                foreach (FileInfo fi in DirInfo.GetFiles())
                {
                    if (fi.Extension.Contains("csv"))
                    {
                        ConstructSchema(fi);
                        insertDB(ParseFile(DownloadPath + fi.Name));
                        FileFunctions.LogConsoleActivity("Completed " + fi.Name + " at " + DateTime.Now);
                    }
                }
                //add call to new proc for bulk update
                FileFunctions.LogConsoleActivity("Doing bulk update - " + DateTime.Now);
                try
                {
                    ECN_Framework_BusinessLayer.Activity.BlastActivitySends.doBulkUpdate_SMTPMessage();
                }
                catch (Exception ex)
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Port25_SMTPLog.doBulkUpdate_SMPTMessage", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                }
                FileFunctions.LogConsoleActivity("Done with bulk update - " + DateTime.Now);
            }
        }

        public static void GetFiles()
        {
            try
            {
                FileFunctions.LogConsoleActivity("Connecting to FTP: " + DateTime.Now);
                List<string> files = new List<string>();
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(server + "/"));
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(userName, password);
                request.KeepAlive = false;
                using(FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            while (!reader.EndOfStream)
                            {
                                string filename = reader.ReadLine().ToString();
                                if (filename.Length > 4)
                                {
                                    if (!string.IsNullOrEmpty(StartsWith))
                                    {
                                        if (filename.StartsWith(StartsWith, StringComparison.OrdinalIgnoreCase))
                                        {
                                            files.Add(filename);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                numberoffiles = files.Count;
                foreach (string file in files)
                {
                    DownloadFile(file);
                }
                FileFunctions.LogConsoleActivity("FTP Connection Closed: " + DateTime.Now);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Port25_SMTPLog.GetFiles", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            }

        }

        static void DownloadFile(string filename)
        {
            try
            {
                FileFunctions.LogConsoleActivity("Downloading File: " + filename + " " + DateTime.Now);
                if (!Directory.Exists(DownloadPath))
                    Directory.CreateDirectory(DownloadPath);
                FtpWebRequest reqFTP;
                using (FileStream outputStream = new FileStream(DownloadPath + filename, FileMode.Create))
                {
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(server + "/" + filename));
                    reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                    reqFTP.UseBinary = true;
                    reqFTP.Credentials = new NetworkCredential(userName, password);
                    reqFTP.KeepAlive = false;
                    using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                    {
                        using (Stream ftpStream = response.GetResponseStream())
                        {
                            long cl = response.ContentLength;
                            int bufferSize = 2048;
                            int readCount;
                            byte[] buffer = new byte[bufferSize];
                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                            while (readCount > 0)
                            {
                                outputStream.Write(buffer, 0, readCount);
                                readCount = ftpStream.Read(buffer, 0, bufferSize);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileFunctions.LogConsoleActivity("Error Downloading File: " + ex.ToString() + " " + DateTime.Now);
            }
        }

        public static void ConstructSchema(FileInfo theFile)
        {
            try
            {
                FileFunctions.LogConsoleActivity("ConstructSchema File: " + theFile.Name + " " + DateTime.Now);
                StringBuilder schema = new StringBuilder();
                DataTable data = LoadCSV(theFile);
                schema.AppendLine("[" + theFile.Name + "]");
                schema.AppendLine("ColNameHeader=True");
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    schema.AppendLine("col" + (i + 1).ToString() + "=" + data.Columns[i].ColumnName + " Text");
                }
                string schemaFileName = DownloadPath + "Schema.ini";
                TextWriter tw = new StreamWriter(schemaFileName);
                tw.WriteLine(schema.ToString());
                tw.Close();
            }
            catch (Exception ex)
            {
                FileFunctions.LogConsoleActivity("Error creating schema: " + theFile);

                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Port25_SMTPLog.ConstructSchema", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                throw ex;
            }
        }

        public static DataTable LoadCSV(FileInfo theFile)
        {
            string sqlString = "Select top 1 * FROM [" + theFile.Name + "];";
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="
                + theFile.DirectoryName + ";" + "Extended Properties='text;HDR=YES;'";
            DataTable theCSV = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(conStr))
            {
                using (OleDbCommand comm = new OleDbCommand(sqlString, conn))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(comm))
                    {
                        adapter.Fill(theCSV);
                    }
                }
            }
            return theCSV;
        }

        public static DataTable ParseFile(string path)
        {
            FileFunctions.LogConsoleActivity("ParseFile File: " + Path.GetFileName(path) + " " + DateTime.Now);
            string header = "Yes";
            string sql = string.Empty;
            DataTable dataTable = null;
            string pathOnly = string.Empty;
            string fileName = string.Empty;
            try
            {

                pathOnly = Path.GetDirectoryName(path);
                fileName = Path.GetFileName(path);
                sql = @"SELECT dlvDestinationIp +' | '+ dsnDiag  as SMTPMessage, [orig]  as Original , dlvSourceIp as SourceIP " +
                       "FROM [" + fileName + "]  where [type]='d' and [orig] like 'bounce_%'";

               
               //sql = @"SELECT IIF(IsNull(dlvDestinationIp), ' ', dlvDestinationIp) +' | '+ IIF(IsNull(dsnDiag), ' ', dsnDiag)  as 'SMTPMessage', MID([orig], Instr([orig],'_')+1 ,  Instr([orig],'-')-Instr([orig],'_')-1)  as 'EmailID' ," +
               //    " MID([orig], Instr( [orig],'-') + 1,  Instr([orig],'@')-Instr( [orig],'-')-1)  as 'BlastID', dlvSourceIp as 'SourceIP'  FROM [" + fileName + "]  where [type]='d' and [orig] like 'bounce_%'";

                OleDbConnection connection = new OleDbConnection(
                        @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                        ";Extended Properties=\"Text;HDR=" + header + "\"");

                OleDbCommand command = new OleDbCommand(sql, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                FileFunctions.LogConsoleActivity("Error parsing: " + path);
                FileFunctions.LogConsoleActivity("Error Message: " + ex.Message);
                FileFunctions.LogConsoleActivity("Skipping File: " + path);
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Port25_SMTPLog.ParseFile", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            }
            FileFunctions.LogConsoleActivity("End Parsing : " + DateTime.Now);
            return dataTable;
        }              

        static void insertDB(DataTable dt)
        {
            StringBuilder sbxmldata = new StringBuilder();
            int rowcount = 0;
            int batchCount = 1;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    FileFunctions.LogConsoleActivity(String.Format("Total Records to be processed : {0}", dt.Rows.Count));
                    int i = 1;
                    foreach (DataRow currentrow in dt.Rows)
                    {     
                        Regex r = new Regex("@");
                        Array BreakupOriginal = r.Split(currentrow[1].ToString());
                        string data = BreakupOriginal.GetValue(0).ToString();
                        if (data.Contains("_") && data.Contains("-"))
                        {
                            r = new Regex("_");
                            BreakupOriginal = r.Split(data);
                            data = BreakupOriginal.GetValue(1).ToString();
                            r = new Regex("-");
                            BreakupOriginal = r.Split(data);
                            string EmailID = BreakupOriginal.GetValue(0).ToString();
                            string BlastID = BreakupOriginal.GetValue(1).ToString();

                            sbxmldata.Append(string.Format("<Email EmailID=\"{0}\" BlastID=\"{1}\" SMTPMessage=\"{2}\" SourceIP=\"{3}\"/>", EmailID, BlastID, CleanUpMessage(currentrow[0].ToString()), currentrow[2].ToString()));
                            rowcount++;
                            if (rowcount == 2500)
                            {
                                FileFunctions.LogConsoleActivity(String.Format("Processing Batch {0} : {1}", batchCount, (batchCount * 2500).ToString()));
                                batchCount++;

                                rowcount = 0;
                                ECN_Framework_BusinessLayer.Activity.BlastActivitySends.update_SMTPMessage(sbxmldata.ToString());
                                sbxmldata.Clear();
                            }
                        }
                        else
                        {
                            FileFunctions.LogConsoleActivity(String.Format("Skipped : {0}", currentrow[1].ToString()));
                        }
                    }
                    if (sbxmldata.ToString() != String.Empty)
                    {
                        ECN_Framework_BusinessLayer.Activity.BlastActivitySends.update_SMTPMessage(sbxmldata.ToString());
                        sbxmldata.Clear();
                    }
                }
                catch (Exception ex)
                {
                    FileFunctions.LogConsoleActivity(String.Format("Error : {0}", ex.Message));
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Port25_SMTPLog.insertDB", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                }
            }
        }

        public static string CleanUpMessage(string message)
        {
            string cleanedMessage = "";
            if (message.Length > 253)
            {
                cleanedMessage = message.Substring(0, 253);
                cleanedMessage = CleanAndTrimSignature(cleanedMessage);
            }
            else
            {
                cleanedMessage = message.ToString();
                cleanedMessage = CleanAndTrimSignature(cleanedMessage);

                if (cleanedMessage.Length > 1)
                {
                    //cleanedMessage = cleanedMessage.Substring(0, (cleanedMessage.Length - 1));
                }
                else
                {
                    cleanedMessage = "";
                }
            }
            return cleanedMessage;
        }

        private static string CleanAndTrimSignature(string message)
        {
            string cleanedMessage = message;

            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.AppendChild(doc.CreateElement("xml"));
            node.InnerText = cleanedMessage;
            StringWriter writer = new StringWriter();
            XmlTextWriter xml_writer = new XmlTextWriter(writer);
            node.WriteContentTo(xml_writer);
            cleanedMessage = writer.ToString();

            cleanedMessage = cleanedMessage.Replace("\"", "");
            cleanedMessage = cleanedMessage.Replace("'", "");
            cleanedMessage = cleanedMessage.Replace(",", "");
            cleanedMessage = cleanedMessage.Replace("<", "");
            cleanedMessage = cleanedMessage.Replace(">", "");
            cleanedMessage = cleanedMessage.Replace("&", "");
            cleanedMessage = cleanedMessage.Replace("$", "");
            cleanedMessage = cleanedMessage.Replace("(", "");

            if (cleanedMessage.Length > 253)
            {
                cleanedMessage = cleanedMessage.Substring(0, 253);
            }

            return cleanedMessage;
        }

        public static void DeleteOldFiles()
        {
            FileFunctions.LogConsoleActivity("Deleting Old Files: " + DateTime.Now);
            try
            {
                 int daysToKeep = 3;
                try
                {
                    daysToKeep = Convert.ToInt32(ConfigurationManager.AppSettings["DaysToKeep"].ToString());
                }
                catch { }
                DirectoryInfo DirInfo = new DirectoryInfo(DownloadPath);
                foreach (FileInfo fi in DirInfo.GetFiles())
                {
                    if (fi.CreationTime < DateTime.Now.AddDays(daysToKeep))
                    {
                        fi.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                FileFunctions.LogConsoleActivity("Delete Old Files Error: " + ex.ToString() + " " + DateTime.Now);
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Port25_SMTPLog.DeleteOldFiles", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            }
        }

    }
}
