using System;
using System.IO;
using System.Threading;
using Core.ADMS.Events;

namespace ADMS.Services.FileMover
{
    public class FileMover : ServiceBase, IFileMover
    {
        public event Action<FileMoved> FileMoved;
        readonly FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
        private FrameworkUAS.Entity.AdmsLog AdmsLog { get; set; }      
        public void HandleFileDetected(FileDetected eventMessage)
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;
            try
            {
                if(eventMessage.IsRepoFile == false)
                    MoveFileFromFTPToClientRepoAsync(eventMessage);
                else
                    PublishFileMoved(eventMessage.ImportFile, eventMessage.Client, eventMessage.SourceFile, eventMessage.AdmsLog);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".HandleFileDetected", eventMessage.AdmsLog);
            }
        }
        public void HandleFileValidated(FileValidated eventMessage)
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;
            try
            {
                CopyFileFromClientRepositoryToArchive(eventMessage.ImportFile.FullName, eventMessage.Client, eventMessage.IsValidFileType, eventMessage.AdmsLog.ProcessCode);
                if (File.Exists(eventMessage.ImportFile.FullName))
                    DeleteFileFromClientRepository(eventMessage.ImportFile.FullName);
                else
                {
                    string repoFile = Core.ADMS.BaseDirs.getClientRepoDir() + "\\" + eventMessage.Client.FtpFolder + "\\" + eventMessage.ImportFile.Name;
                    DeleteFileFromClientRepository(repoFile);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".HandleFileValidated", eventMessage.AdmsLog);
            }
        }
        public void HandleCustomFileProcessed(CustomFileProcessed eventMessage)
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;
            try
            {
                CopyFileFromClientRepositoryToArchive(eventMessage.ImportFile.FullName, eventMessage.Client, eventMessage.IsValid, eventMessage.AdmsLog.ProcessCode);
                if (File.Exists(eventMessage.ImportFile.FullName))
                    DeleteFileFromClientRepository(eventMessage.ImportFile.FullName);
                else
                {
                    string repoFile = Core.ADMS.BaseDirs.getClientRepoDir() + "\\" + eventMessage.Client.FtpFolder + "\\" + eventMessage.ImportFile.Name;
                    DeleteFileFromClientRepository(repoFile);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".HandleCustomFileProcessed", eventMessage.AdmsLog);
            }
        }
        public async void MoveFileFromFTPToClientRepoAsync(FileDetected eventMessage)
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;
            try
            {
                if (File.Exists(eventMessage.ImportFile.FullName))
                {
                    string clientRepo = Core.ADMS.BaseDirs.createDirectory(Core.ADMS.BaseDirs.getClientRepoDir(), eventMessage.Client.FtpFolder);
                    string copyFullFileNameToRepo = Core.ADMS.BaseDirs.createDirectory(clientRepo, eventMessage.ImportFile.Name);
                    FileInfo fileInfo = new FileInfo(copyFullFileNameToRepo);
                    bool locked = true;
                    do
                    {
                        try
                        {
                            using (FileStream sourceStream = File.Open(eventMessage.ImportFile.FullName, FileMode.Open))
                            {
                                using (FileStream destinationStream = File.Create(copyFullFileNameToRepo))
                                {
                                    locked = false;
                                    await sourceStream.CopyToAsync(destinationStream);
                                    sourceStream.Close();
                                    File.Delete(eventMessage.ImportFile.FullName);
                                }
                            }
                            PublishFileMoved(fileInfo, eventMessage.Client, eventMessage.SourceFile, eventMessage.AdmsLog);
                        }
                        catch (IOException)
                        {
                            Thread.Sleep(2000);
                        }
                    } while (locked);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".MoveFileFromFTPToClientRepoAsync", eventMessage.AdmsLog);
            }
        }
        public void PublishFileMoved(FileInfo file, KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.AdmsLog admsLog)
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;
            try
            {
                bool isKnownFile = sourceFile != null ? true : false;
                var fileMoved = new FileMoved(file, client, sourceFile, admsLog, isKnownFile, threadID);
                
                FileMoved(fileMoved);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".PublishFileMoved", admsLog);
            }
        }
        public async void CopyFileFromClientRepositoryToClientStaging(string fileFromRepo, KMPlatform.Entity.Client client)
        {
            //Check if a MoverFile exists for client and if file has specific place to go
            MoverConfig moveConfig = new MoverConfig();
            string result = moveConfig.GetLocationToMoveFile(client.FtpFolder, Path.GetFileName(fileFromRepo));
            string fileName = Path.GetFileName(fileFromRepo);
            string clientRepo = Core.ADMS.BaseDirs.createDirectory(Core.ADMS.BaseDirs.getClientStagingDir(), client.FtpFolder);
            string copyFullFileNameToStaging = "";

            if (result != "Default")
                copyFullFileNameToStaging = Core.ADMS.BaseDirs.createDirectory(result, fileName);
            else
                copyFullFileNameToStaging = Core.ADMS.BaseDirs.createDirectory(clientRepo, fileName);

            bool locked = true;
            do
            {
                try
                {
                    using (FileStream sourceStream = File.Open(fileFromRepo, FileMode.Open))
                    {
                        using (FileStream destinationStream = File.Create(copyFullFileNameToStaging))
                        {
                            locked = false;

                            await sourceStream.CopyToAsync(destinationStream);
                            sourceStream.Close();
                        }
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(2000);
                }
            } while (locked);
        }
        public async void CopyFileFromClientRepositoryToArchive(string localFtpFile, KMPlatform.Entity.Client client, bool status, string processCode)
        {
            //fileFromRepo is meant to be the Client Repository location but the incoming path will be the ftp site

            string fileName = Path.GetFileNameWithoutExtension(localFtpFile);
            string extension = Path.GetExtension(localFtpFile);
            DateTime now = DateTime.Now;
            string clientFilePath = string.Empty;

            var archiveLocation = Core.ADMS.BaseDirs.getClientArchiveDir();
            var processedLocation = "Processed";
            var invalidLocation = "Invalid";

            //Create folder if it doesn't exist
            var processedFileArchiveLocation = archiveLocation + "\\" + client.FtpFolder + "\\" + processedLocation + "\\";
            var processedFileArchiveLocationExist = Directory.Exists(processedFileArchiveLocation);
            if (processedFileArchiveLocationExist == false) Directory.CreateDirectory(processedFileArchiveLocation);

            //Create folder if it doesn't exist
            var invalidFileArchiveLocation = archiveLocation + "\\" + client.FtpFolder + "\\" + invalidLocation + "\\";
            var doesInvalidFileArchiveLocationExist = Directory.Exists(invalidFileArchiveLocation);
            if (doesInvalidFileArchiveLocationExist == false) Directory.CreateDirectory(invalidFileArchiveLocation);

            //Choose 
            if (status)
                clientFilePath = processedFileArchiveLocation;
            else
                clientFilePath = invalidFileArchiveLocation;

            string cleanProcessCode = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(processCode);
            //Append the ProcessCode to filename. This will make it easier to identify the file associated to processcode.
            string copyFullFileNameToArchive = clientFilePath + cleanProcessCode + "_" + fileName + extension;
            string copyFrom = Core.ADMS.BaseDirs.getClientRepoDir() + "\\" + client.FtpFolder + "\\" + fileName + extension;
            bool isFileLocked = true;
            do
            {
                try
                {
                    if (File.Exists(copyFrom))
                    {
                        using (FileStream sourceStream = File.Open(copyFrom, FileMode.Open))
                        {
                            using (FileStream destinationStream = File.Create(copyFullFileNameToArchive))
                            {
                                isFileLocked = false;
                                await sourceStream.CopyToAsync(destinationStream);
                                sourceStream.Close();
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(2000);
                }
            } while (isFileLocked);
        }
        public void DeleteFileFromClientRepository(string fileToDelete)
        {
            bool locked = true;
            do
            {
                try
                {
                    if (File.Exists(fileToDelete) && Core_AMS.Utilities.FileFunctions.IsFileLocked(fileToDelete) == false)
                    {
                        File.Delete(fileToDelete);
                        locked = false;
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(2000);
                }
            } while (locked);
        }
    }
}
