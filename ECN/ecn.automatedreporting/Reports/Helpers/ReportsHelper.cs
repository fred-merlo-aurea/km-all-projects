using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using AlexPilotti.FTPS.Client;
using KM.Common.Entity;
using WinSCP;

namespace ecn.automatedreporting.Reports.Helpers
{
    public static class ReportsHelper
    {
        public static StreamWriter LogFile { get; set; }
        public static Assembly Assembly { get; set; }

        public static DateTime GetPreviousWeekStartDate(DateTime masterStartDate)
        {
            var retDate = masterStartDate.Date.AddDays(-7);
            switch (retDate.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return retDate;
                case DayOfWeek.Monday:
                    return retDate.AddDays(-1);
                case DayOfWeek.Tuesday:
                    return retDate.AddDays(-2);
                case DayOfWeek.Wednesday:
                    return retDate.AddDays(-3);
                case DayOfWeek.Thursday:
                    return retDate.AddDays(-4);
                case DayOfWeek.Friday:
                    return retDate.AddDays(-5);
                case DayOfWeek.Saturday:
                    return retDate.AddDays(-6);
                default:
                    return retDate;
            }
        }

        public static void WriteToLog(string text)
        {
            LogFile.AutoFlush = true;
            LogFile.WriteLine($"{DateTime.Now} >> {text}");
            LogFile.Flush();
        }

        public static string GetFilePath(int customerId)
        {
            var filepath = $"{ConfigurationManager.AppSettings["FilePath"]}\\customers\\{customerId}\\downloads\\";

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            else
            {
                try
                {
                    Directory.GetFiles(filepath).ToList().ForEach(File.Delete);
                }
                catch (Exception ex)
                {
                    ApplicationLog.LogNonCriticalError(ex, "ecn.automatedReporting.GetFilePath.FileDelete", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                }
            }

            return filepath;
        }

        private static bool ValidateUserCert(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
            {
                return false;
            }

            if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                var z = Zone.CreateFromUrl(((HttpWebRequest)sender).RequestUri.ToString());
                if (z.SecurityZone == SecurityZone.Intranet ||
                    z.SecurityZone == SecurityZone.MyComputer)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        public static bool MoveFileToFtp(string ftpUrl, string remoteFileName, string userName, string passWord, string sourceFilePath)
        {
            try
            {
                WriteToLog("Moving " + remoteFileName + " to FTP site " + ftpUrl);
                for (var i = 1; i <= 10; i++)
                {
                    WriteToLog("Try " + i + " out of 10");
                    try
                    {
                        if (ftpUrl.Contains("ftps://"))
                        {
                            ServicePointManager.ServerCertificateValidationCallback = ValidateUserCert;
                            var tempURL = ftpUrl.Replace("ftps://", "ftp://");
                            var uriBuilder = new UriBuilder($"{tempURL}/{remoteFileName}");
                            uriBuilder.Port = 990;
                            var nc = new NetworkCredential(userName, passWord);
                            WriteToLog("Beginning FTP export");
                            using (var ftps = new FTPSClient())
                            {

                                ftps.Connect(uriBuilder.Uri.Host, 990, nc, ESSLSupportMode.Implicit, ValidateUserCert, null, 0, 0, 0, null);
                                ftps.PutFile(sourceFilePath, uriBuilder.Uri.AbsolutePath);
                                ftps.Close();
                                ftps.Dispose();
                            }

                            WriteToLog("Finished FTP export");
                            break;
                        }

                        if (ftpUrl.Contains("sftp://"))
                        {
                            if (ftpUrl.EndsWith("/"))
                            {
                                ftpUrl = ftpUrl.TrimEnd('/');
                            }

                            var tempUrl = ftpUrl.Replace("sftp://", "");
                            var uriBuilder = new UriBuilder($"{tempUrl}/{remoteFileName}");

                            var sessionOptions = new SessionOptions
                            {
                                Protocol = Protocol.Sftp,
                                HostName = tempUrl,
                                UserName = userName,
                                Password = passWord
                            };

                            string fingerprint = null;

                            using (var session = new Session())
                            {
                                fingerprint = session.ScanFingerprint(sessionOptions);
                            }

                            // Now we have the fingerprint
                            sessionOptions.SshHostKeyFingerprint = fingerprint;

                            WriteToLog("Beginning SFTP export");
                            using (var session = new Session())
                            {
                                // Connect
                                session.Open(sessionOptions);

                                // Upload files
                                var transferOptions = new TransferOptions {TransferMode = TransferMode.Binary};

                                var transferResult = session.PutFiles(sourceFilePath, uriBuilder.Uri.AbsolutePath, false, transferOptions);

                                // Throw on any error
                                transferResult.Check();
                            }

                            WriteToLog("Finished SFTP export");
                            break;
                        }

                        var request = (FtpWebRequest)WebRequest.Create($"{ftpUrl}/{remoteFileName}");
                        request.Method = WebRequestMethods.Ftp.UploadFile;
                        request.KeepAlive = false;
                        request.Credentials = new NetworkCredential(userName, passWord);


                        // Copy the contents of the file to the request stream.

                        var fileContents = File.ReadAllBytes(sourceFilePath);
                        request.ContentLength = fileContents.Length;

                        using (var requestStream = request.GetRequestStream())
                        {
                            WriteToLog("Beginning FTP export");
                            requestStream.Write(fileContents, 0, fileContents.Length);

                            requestStream.Close();
                            requestStream.Dispose();
                            using (var response = (FtpWebResponse)request.GetResponse())
                            {
                                WriteToLog($"Upload File Complete, status {response.StatusDescription}");

                                try
                                {
                                    response.Close();
                                }
                                catch { }
                            }
                        }

                        WriteToLog("Finished FTP export");
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (i == 10)
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog("error during FTP Export");
                ApplicationLog.LogCriticalError(ex, "ECN_Automated_Reporting.MoveFileToFTP", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                return false;
            }
            File.Delete(sourceFilePath);
            return true;
        }

        public static ArrayList GetDataTableColumns(DataTable dataTable)
        {
            var nColumns = dataTable.Columns.Count;
            var columnHeadings = new ArrayList();
            for (var i = 0; i < nColumns; i++)
            {
                var dataColumn = dataTable.Columns[i];
                columnHeadings.Add(dataColumn.ColumnName);
            }
            return columnHeadings;
        }
    }
}
