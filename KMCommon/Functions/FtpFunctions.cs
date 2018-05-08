using System;
using System.IO;
using System.Net;

namespace KM.Common.Functions
{
    public class FtpFunctions
    {
        private FtpWebRequest _ftpRequest = null;
        private FtpWebResponse _ftpResponse = null;
        private Stream _ftpStream = null;
        private int bufferSize = 2048;
        private int MaxCacheSize = 2097152;
        public string Host { get; }
        public string User { get; }
        public string Pass { get; }

        /* Construct Object */
        public FtpFunctions(string hostIP, string userName, string password)
        {
            Host = hostIP;
            User = userName;
            Pass = password;
        }

        /* Download File */
        public void Download(string remoteFile, string localFile, bool enableSSL = false)
        {
            FileStream localFileStream = null;
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)WebRequest.Create(Host + "/" + remoteFile);
                _ftpRequest.EnableSsl = enableSSL;
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(User, Pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                _ftpStream = _ftpResponse.GetResponseStream();
                /* Open a File Stream to Write the Downloaded File */
                localFileStream = new FileStream(localFile, FileMode.Create);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesRead = _ftpStream?.Read(byteBuffer, 0, bufferSize) ?? 0;
                /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = _ftpStream?.Read(byteBuffer, 0, bufferSize) ?? 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                /* Resource Cleanup */
                localFileStream?.Close();
                _ftpStream?.Close();
                _ftpResponse?.Close();
                _ftpRequest = null;
            }
            return;
        }

        /* Download File Using Cache Technique */
        private void WriteCacheToFile(MemoryStream downloadCache, string downloadPath, int cachedSize)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Append))
            {
                byte[] cacheContent = new byte[cachedSize];
                downloadCache.Seek(0, SeekOrigin.Begin);
                downloadCache.Read(cacheContent, 0, cachedSize);
                fileStream.Write(cacheContent, 0, cachedSize);
            }
        }

        public void DownloadCached(string file, string downloadPath, Action<string> handleWebException = null, string errorMethodName = "")
        {
            _ftpRequest = WebRequest.Create(string.Format("{0}/{1}", Host, file)) as FtpWebRequest;
            _ftpRequest.Credentials = new NetworkCredential(User, Pass);
            _ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            MemoryStream downloadCache = null;
            try
            {
                _ftpResponse = _ftpRequest.GetResponse() as FtpWebResponse;
                _ftpStream = _ftpResponse.GetResponseStream();

                downloadCache = new MemoryStream(MaxCacheSize);
                var downloadBuffer = new byte[bufferSize];
                var bytesSize = 0;
                var cachedSize = 0;

                while (true)
                {
                    bytesSize = _ftpStream.Read(downloadBuffer, 0, downloadBuffer.Length);

                    // If the cache is full, or the download is completed, write the data in cache to local file.
                    if (bytesSize == 0
                        || MaxCacheSize < cachedSize + bytesSize)
                    {
                        try
                        {
                            var destPath = downloadPath + file;
                            WriteCacheToFile(downloadCache, destPath, cachedSize);

                            // Stop downloading the file if the download is paused, canceled or completed. 
                            if (bytesSize == 0)
                            {
                                break;
                            }

                            // Reset cache
                            downloadCache.Seek(0, SeekOrigin.Begin);
                            cachedSize = 0;
                        }
                        catch (Exception ex)
                        {
                            var msg = string.Format(
                                "There is an error while downloading {0}{1}.  See InnerException for detailed error. ",
                                Host,
                                file);

                            new ApplicationException(msg, ex);
                            return;
                        }
                    }

                    downloadCache.Write(downloadBuffer, 0, bytesSize);
                    cachedSize += bytesSize;
                }
            }
            catch (WebException ex)
            {
                handleWebException?.Invoke(string.Format(
                    "{0}{1}{2}",
                    DateTime.Now.ToString(),
                    errorMethodName,
                    ((FtpWebResponse)ex.Response).StatusDescription));
                throw;
            }
            finally
            {
                _ftpStream?.Close();
                _ftpResponse?.Close();
                downloadCache?.Close();
                _ftpRequest = null;
            }
        }
 
        /* Upload File */
        public bool Upload(string remoteFile, string localFile, bool enableSSL = false)
        {
            FileStream localFileStream = null;
            bool uploaded = true;
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(User, Pass);
                _ftpRequest.EnableSsl = enableSSL;
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                localFileStream = new FileStream(localFile, FileMode.Open);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                while (bytesSent != 0)
                {
                    _ftpStream.Write(byteBuffer, 0, bytesSent);
                    bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                }
            }
            catch (Exception ex)
            {
                uploaded = false;
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                /* Resource Cleanup */
                localFileStream?.Close();
                _ftpStream?.Close();
                _ftpRequest = null;
            }
            return uploaded;
        }

        /* Delete File */
        public void Delete(string deleteFile)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)WebRequest.Create(Host + "/" + deleteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(User, Pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                /* Resource Cleanup */
                _ftpResponse?.Close();
                _ftpRequest = null;
            }
            return;
        }

        /* Rename File */
        public void Rename(string currentFileNameAndPath, string newFileName)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)WebRequest.Create(Host + "/" + currentFileNameAndPath);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(User, Pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                /* Rename the File */
                _ftpRequest.RenameTo = newFileName;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                /* Resource Cleanup */
                _ftpResponse?.Close();
                _ftpRequest = null;
            }
            return;
        }

        /* Create a New Directory on the FTP Server */
        public void CreateDirectory(string newDirectory)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)WebRequest.Create(Host + "/" + newDirectory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(User, Pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                /* Resource Cleanup */
                _ftpResponse?.Close();
                _ftpRequest = null;
            }
            return;
        }

        /* Get the Date/Time a File was Created */
        public string GetFileCreatedDateTime(string fileName)
        {
            StreamReader ftpReader = null;
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(User, Pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                ftpReader = new StreamReader(_ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                fileInfo = ftpReader.ReadToEnd();                 
                /* Return File Created Date Time */
                return fileInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                /* Resource Cleanup */
                ftpReader?.Close();
                _ftpStream?.Close();
                _ftpResponse?.Close();
                _ftpRequest = null;
            }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* Get the Size of a File */
        public string GetFileSize(string fileName)
        {
            StreamReader ftpReader = null;
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(User, Pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                ftpReader = new StreamReader(_ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                while (ftpReader.Peek() != -1)
                {
                    fileInfo = ftpReader.ReadToEnd();
                } 
                /* Return File Size */
                return fileInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                /* Resource Cleanup */
                ftpReader?.Close();
                _ftpStream?.Close();
                _ftpResponse?.Close();
                _ftpRequest = null;
            }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* List Directory Contents File/Folder Name Only */
        public string[] DirectoryListSimple(string directory)
        {
            StreamReader ftpReader = null;
            try
            {
                /* Create an FTP Request */
                if (!string.IsNullOrEmpty(directory))
                   _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Host + "/" + directory);
                else
                    _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Host + "/");
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(User, Pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                ftpReader = new StreamReader(_ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                while (ftpReader.Peek() != -1)
                {
                    directoryRaw += ftpReader.ReadLine() + "|";
                }                             
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                if (!string.IsNullOrEmpty(directoryRaw))
                {
                    string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList;
                }                           
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                /* Resource Cleanup */
                ftpReader?.Close();
                _ftpStream?.Close();
                _ftpResponse?.Close();
                _ftpRequest = null;
            }

            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }

        /* List Directory Contents in Detail (Name, Size, Created, etc.) */
        public string[] DirectoryListDetailed(string directory)
        {
            StreamReader ftpReader = null;
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(User, Pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                ftpReader = new StreamReader(_ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                while (ftpReader.Peek() != -1)
                {
                    directoryRaw += ftpReader.ReadLine() + "|";
                }                
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                if (!string.IsNullOrEmpty(directoryRaw))
                {
                    string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList;
                }                              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                /* Resource Cleanup */
                ftpReader?.Close();
                _ftpStream?.Close();
                _ftpResponse?.Close();
                _ftpRequest = null;
            }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }
        public bool TestUpload()
        {
            bool success = false;
            //create file
            // string path = "km_ftp_UploadVerification.txt";
            string directory = @"C:\TempTextFile";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = Path.Combine(directory, "km_ftp_UploadVerification.txt");

            if (!File.Exists(path))
            {
                // Create a file to write to.
                File.Create(path);
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Testing FTP Upload");
                }
            }

            //call upload
            success = Upload(path, path);
            //delete file
            Delete(path);

            return success;
        }
    }
}
