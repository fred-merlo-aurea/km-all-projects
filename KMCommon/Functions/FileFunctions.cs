using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace KM.Common
{
    public class FileFunctions
    {
        private const string DownloadingFromFTPErrorSubject = "Issue Downloading From FTP";
        private const string DownloadingFromFTPErrorMessagePrefix = "Request URI: ";
        private const string DownloadingFromFTPErrorMessageMiddlePart = " Error: ";

        public static bool DownloadFileFromFTP(string userid, string password, string fullFileName, string requestURI)
        {
            bool success = false;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(requestURI);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                string user = userid;
                string pass = password;
                request.Credentials = new NetworkCredential(user, pass);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                if (response.StatusCode == FtpStatusCode.FileActionOK)
                {
                    StreamReader reader = new StreamReader(responseStream);
                    string filePath = fullFileName;
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    StreamWriter sw = new StreamWriter(new FileStream(filePath, System.IO.FileMode.Append));

                    while (reader.EndOfStream == false)
                    {
                        sw.WriteLine(reader.ReadLine());
                    }
                    sw.Flush();
                    sw.Close();

                    reader.Close();
                    response.Close();

                    //delete file from FTP Site
                    FtpWebRequest rDelete = (FtpWebRequest)WebRequest.Create(requestURI);
                    rDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                    rDelete.Credentials = new NetworkCredential(user, pass);
                    FtpWebResponse responseDelete = (FtpWebResponse)rDelete.GetResponse();

                    Stream streamDelete = responseDelete.GetResponseStream();
                    if (responseDelete.StatusCode != FtpStatusCode.FileActionOK)
                    {
                        EmailFunctions.NotifyAdmin("Issue Deleting File From FTP", "Request URI: " + requestURI);
                    }

                    responseDelete.Close();
                    success = true;
                }
                else
                {
                    EmailFunctions.NotifyAdmin("Issue Getting File From FTP", "Request URI: " + requestURI);
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = BuildDownloadExceptionMessage(ex);
                EmailFunctions.NotifyAdmin(
                    DownloadingFromFTPErrorSubject,
                    string.Format("{0}{1}{2}{3}", 
                    DownloadingFromFTPErrorMessagePrefix,
                    requestURI,
                    DownloadingFromFTPErrorMessageMiddlePart,
                    exceptionMessage));
            }
            return success;
        }

        public static string BuildDownloadExceptionMessage(Exception ex)
        {
            var sbLog = new StringBuilder();
            sbLog.AppendLine("**********************");
            sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
            sbLog.AppendLine("-- Message --");
            if (ex.Message != null)
            {
                sbLog.AppendLine(ex.Message);
            }
            sbLog.AppendLine("-- InnerException --");
            if (ex.InnerException != null)
            {
                sbLog.AppendLine(ex.InnerException.ToString());
            }
            sbLog.AppendLine("-- Stack Trace --");
            if (ex.StackTrace != null)
            {
                sbLog.AppendLine(ex.StackTrace);
            }
            sbLog.AppendLine("**********************");

            return sbLog.ToString();
        }

        static void UnZipFile(string filePath)
        {
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(filePath)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    string directoryName = filePath.Replace(".zip", "") + @"\";
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        if (!Directory.Exists(directoryName))
                            Directory.CreateDirectory(directoryName);
                    }

                    if (File.Exists(directoryName + fileName))
                        File.Delete(directoryName + fileName);

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(directoryName + fileName))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            streamWriter.Flush(true);

                        }
                    }
                }
            }
        }

        public static void LogActivity(bool outputToConsole, string activity, string logSuffix = "")
        {
            try
            {
                string logPath = Environment.CurrentDirectory + "\\Log\\";

                logSuffix = "_" + logSuffix;
                string logFile = DateTime.Now.ToShortDateString().Replace("/", "-").ToString() + logSuffix + "_Log.txt";

                activity = DateTime.Now.ToString() + " " + activity;

                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);

                StreamWriter sw;
                if (!File.Exists(logPath + logFile))
                {
                    sw = File.CreateText(logPath + logFile);
                    sw.WriteLine("********** CREATE LOG FILE - " + DateTime.Now.ToString() + " **********");
                    if (outputToConsole)
                        Console.WriteLine("********** CREATE LOG FILE - " + DateTime.Now.ToString() + " **********");

                    CleanupConsoleActivity(logPath, sw);
                }
                else
                {
                    sw = File.AppendText(logPath + logFile);
                }

                sw.WriteLine(activity);
                sw.Close();
                if (outputToConsole)
                    Console.WriteLine(activity);
            }
            catch (Exception){}
        }

        public static void LogConsoleActivity(string activity, string logSuffix = "")
        {
            string logPath = Environment.CurrentDirectory + "\\Log\\";

            logSuffix = "_" + logSuffix;
            string logFile = DateTime.Now.ToShortDateString().Replace("/", "-").ToString() + logSuffix + "_Log.txt";

            activity = DateTime.Now.ToString() + " " + activity;

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            StreamWriter sw;
            if (!File.Exists(logPath + logFile))
            {
                sw = File.CreateText(logPath + logFile);
                sw.WriteLine("********** CREATE LOG FILE - " + DateTime.Now.ToString() + " **********");
                Console.WriteLine("********** CREATE LOG FILE - " + DateTime.Now.ToString() + " **********");

                CleanupConsoleActivity(logPath, sw);
            }
            else
            {
                sw = File.AppendText(logPath + logFile);
            }

            sw.WriteLine(activity);
            sw.Close();
            Console.WriteLine(activity);            
        }

        public static void LogConsoleActivity(StringBuilder activity, string logSuffix = "")
        {
            string logPath = Environment.CurrentDirectory + "\\Log\\";

            logSuffix = "_" + logSuffix;
            string logFile = DateTime.Now.ToShortDateString().Replace("/", "-").ToString() + logSuffix + "_Log.txt";        

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            StreamWriter sw;
            if (!File.Exists(logPath + logFile))
            {
                sw = File.CreateText(logPath + logFile);
                sw.WriteLine("********** CREATE LOG FILE - " + DateTime.Now.ToString() + " **********");
                Console.WriteLine("********** CREATE LOG FILE - " + DateTime.Now.ToString() + " **********");

                CleanupConsoleActivity(logPath, sw);
            }
            else
            {
                sw = File.AppendText(logPath + logFile);
            }

            sw.Write(activity.ToString());
            sw.Close();
            Console.Write(activity.ToString());
        }

        private static void CleanupConsoleActivity(string logPath, StreamWriter sw)
        {
            int daysToKeep = 0;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("KMCommon_LoggingDaysToKeep") == true)
                int.TryParse(ConfigurationManager.AppSettings["KMCommon_LoggingDaysToKeep"].ToString(), out daysToKeep);

            if (daysToKeep != 0)
            {
                sw.WriteLine("********** DELETING OLD FILES - " + DateTime.Now.ToString() + " *******");
                Console.WriteLine("********** DELETING OLD FILES - " + DateTime.Now.ToString() + " *******");

                DirectoryInfo DirInfo = new DirectoryInfo(logPath);
                foreach (FileInfo fi in DirInfo.GetFiles())
                {
                    string fileName = fi.Name.Substring(0, fi.Name.IndexOf("_"));
                    if ((DateTime.Now - Convert.ToDateTime(fileName.Replace("-", "/"))).TotalDays > daysToKeep)
                    {
                        fi.Delete();
                    }
                }
            }
        }
    }
}
