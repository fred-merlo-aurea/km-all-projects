using System;
using System.IO;
using System.Net;
using System.Net.Security;
using AlexPilotti.FTPS;
using WinSCP;
using System.Collections.Generic;
using System.Xml;
using Tamir.SharpSsh;

namespace ECN_Framework_Common.Functions
{
    public class FtpFunctions : KM.Common.Functions.FtpFunctions
    {
        private Session currentSFTPSession = null;
        public string HostKey { get; }

        /* Construct Object */
        public FtpFunctions(string hostIP, string userName, string password, string sshHostKey = "") : base(hostIP, userName, password)
        {
            HostKey = sshHostKey;
        }

        /*Validate User Cert*/
        private static bool ValidateUserCert(object sender, System.Security.Cryptography.X509Certificates.X509Certificate cert, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors ==
           SslPolicyErrors.RemoteCertificateChainErrors)
            {
                return false;
            }
            else if (sslPolicyErrors ==
             SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                System.Security.Policy.Zone z =
                   System.Security.Policy.Zone.CreateFromUrl
                   (((HttpWebRequest)sender).RequestUri.ToString());
                if (z.SecurityZone ==
                   System.Security.SecurityZone.Intranet ||
                   z.SecurityZone ==
                   System.Security.SecurityZone.MyComputer)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        /*Validate Credentials*/
        //for both ftps and ftp
        public bool ValidateCredentials(string userName, string passWord, string ftpURL, string remoteFileName, string sourceFilePath)
        {
            bool isValid = false;

            try
            {
                if (ftpURL.Contains("ftps://"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateUserCert);
                    ftpURL = ftpURL.Replace("ftps://", "ftp://");
                    UriBuilder uriBuilder = new UriBuilder(ftpURL + "//" + remoteFileName);
                    uriBuilder.Port = 990;
                    NetworkCredential nc = new NetworkCredential(userName, passWord);
                    using (AlexPilotti.FTPS.Client.FTPSClient ftps = new AlexPilotti.FTPS.Client.FTPSClient())
                    {
                        ftps.Connect(uriBuilder.Uri.Host, 990, nc, AlexPilotti.FTPS.Client.ESSLSupportMode.Implicit, new RemoteCertificateValidationCallback(ValidateUserCert), null, 0, 0, 0, null);
                        isValid = true;

                        ftps.Close();
                        ftps.Dispose();
                    }
                }
                else if (ftpURL.Contains("sftp://"))
                {
                    if (ftpURL.EndsWith("/"))
                        ftpURL = ftpURL.TrimEnd('/');
                    Sftp sftp = new Sftp(ftpURL.Replace("sftp://", "").Trim(), userName.Trim(), passWord.Trim());

                    // Connect Sftp
                    sftp.Connect();
                    isValid = true;

                    // Close the Sftp connection
                    sftp.Close();
                }
                else
                {
                    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpURL);

                    request.KeepAlive = false;
                    request.Credentials = new NetworkCredential(userName, passWord);
                    request.Method = WebRequestMethods.Ftp.ListDirectory;

                    WebResponse response = (FtpWebResponse)request.GetResponse();
                    isValid = true;
                }
            }
            catch (Exception e)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "FtpFunctions.ValidateCredentials", 1, ftpURL);
            }

            return isValid;
        }

        public bool ValidateFtpUrl(string userName, string passWord, string ftpURL, string remoteFileName, string sourceFilePath)
        {
            bool isValid = false;

            if (!ftpURL.EndsWith("/"))
                ftpURL += "/";

            try
            {
                if (ftpURL.Contains("ftps://"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateUserCert);
                    ftpURL = ftpURL.Replace("ftps://", "ftp://");
                    UriBuilder uriBuilder = new UriBuilder(ftpURL + "//" + remoteFileName);
                    uriBuilder.Port = 990;
                    NetworkCredential nc = new NetworkCredential(userName, passWord);
                    using (AlexPilotti.FTPS.Client.FTPSClient ftps = new AlexPilotti.FTPS.Client.FTPSClient())
                    {
                        ftps.Connect(uriBuilder.Uri.Host, 990, nc, AlexPilotti.FTPS.Client.ESSLSupportMode.Implicit, new RemoteCertificateValidationCallback(ValidateUserCert), null, 0, 0, 0, null);
                        isValid = true;

                        ftps.Close();
                        ftps.Dispose();
                    }
                }
                else if (ftpURL.Contains("sftp://"))
                {
                    if (ftpURL.EndsWith("/"))
                        ftpURL = ftpURL.TrimEnd('/');
                    Sftp sftp = new Sftp(ftpURL.Replace("sftp://", "").Trim(), userName.Trim(), passWord.Trim());

                    // Connect Sftp
                    sftp.Connect();
                    isValid = true;

                    // Close the Sftp connection
                    sftp.Close();
                }
                else
                {
                    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpURL);

                    request.KeepAlive = false;
                    request.Credentials = new NetworkCredential(userName, passWord);
                    request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                    WebResponse response = (FtpWebResponse)request.GetResponse();
                    isValid = true;
                }
            }
            catch (Exception e)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "FtpFunctions.ValidateFtpUrl", 1, ftpURL);
            }

            return isValid;
        }

        public List<string> SFTP_DirectoryListSimple(string directoryPath, bool closeSession)
        {
            WinSCP.SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Host,
                UserName = User,
                Password = Pass,
                SshHostKeyFingerprint = HostKey
            };

            try
            {
                if (currentSFTPSession != null)
                {
                    if (!currentSFTPSession.Opened)
                    {
                        currentSFTPSession = new Session();

                        currentSFTPSession.Open(sessionOptions);
                    }
                }
                else
                {
                    currentSFTPSession = new Session();

                    currentSFTPSession.Open(sessionOptions);
                }

                RemoteDirectoryInfo directory = currentSFTPSession.ListDirectory(directoryPath);
                List<string> files = new List<string>();
                foreach (RemoteFileInfo file in directory.Files)
                {
                    files.Add(file.Name);
                }
                if (closeSession)
                    currentSFTPSession.Close();
                return files;
            }
            catch (Exception e)
            {
                if (currentSFTPSession.Opened)
                    currentSFTPSession.Close();
                return new List<string>();
            }


        }

        public string Download_SFTP(string remoteFile, string destination, bool closeSession)
        {
            string retString = "";
            WinSCP.SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Host,
                UserName = User,
                Password = Pass,
                SshHostKeyFingerprint = HostKey
            };

            try
            {
                if (currentSFTPSession != null)
                {
                    if (!currentSFTPSession.Opened)
                    {
                        currentSFTPSession = new Session();

                        currentSFTPSession.Open(sessionOptions);
                    }
                }
                else
                {
                    currentSFTPSession = new Session();

                    currentSFTPSession.Open(sessionOptions);
                }

                TransferOptions to = new TransferOptions();
                to.TransferMode = TransferMode.Binary;

                TransferOperationResult tor;
                tor = currentSFTPSession.GetFiles(remoteFile, destination, true, to);

                foreach (TransferEventArgs tea in tor.Transfers)
                {
                    retString += string.Format("Download of {0} succeded", tea.FileName);
                }

                if (closeSession)
                    currentSFTPSession.Close();
                return retString;
            }
            catch (Exception e)
            {
                if (currentSFTPSession.Opened)
                    currentSFTPSession.Close();
                retString = e.Message;
                return retString;

            }
        }

        public string Delete_SFTP(string remoteFile, bool closeSession)
        {
            string retString = "";
            WinSCP.SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Host,
                UserName = User,
                Password = Pass,
                SshHostKeyFingerprint = HostKey
            };

            try
            {
                if (currentSFTPSession != null)
                {
                    if (!currentSFTPSession.Opened)
                    {
                        currentSFTPSession = new Session();

                        currentSFTPSession.Open(sessionOptions);
                    }
                }
                else
                {
                    currentSFTPSession = new Session();

                    currentSFTPSession.Open(sessionOptions);
                }

                TransferOptions to = new TransferOptions();
                to.TransferMode = TransferMode.Binary;

                RemovalOperationResult ror;
                ror = currentSFTPSession.RemoveFiles(remoteFile);

                foreach (RemovalEventArgs tea in ror.Removals)
                {
                    if (ror.IsSuccess)
                    {
                        retString += string.Format("Removal of {0} succeded", tea.FileName);
                    }
                    else
                    {
                        retString += string.Format("File removal failed: {0}", tea.FileName);
                        retString += string.Format("Error: {0}", tea.Error.Message);
                    }
                }

                if (closeSession)
                    currentSFTPSession.Close();
                return retString;
            }
            catch (Exception e)
            {
                if (currentSFTPSession.Opened)
                    currentSFTPSession.Close();

                return retString;

            }
        }

        public void CloseSFTPSession()
        {
            if (currentSFTPSession != null && currentSFTPSession.Opened)
            {
                currentSFTPSession.Close();
            }
        }
    }
}

