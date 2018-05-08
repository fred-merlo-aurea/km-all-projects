using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Core.ADMS.Events;
using Core.ADMS;
using FrameworkUAS.Entity;
using System.Configuration;
using Core_AMS.Utilities;

namespace ADMS.Services.FileWatcher
{
    public class FileWatcher : ServiceBase, IFileWatcher
    {
        public event Action<FileDetected> FileDetected;
        public event Action<FileProcessed> FileProcessed;
        public Dictionary<string, FileSystemWatcher> FileSystemWatcherDictionary = new Dictionary<string, FileSystemWatcher>();
        private FolderSetup folderSetup = new FolderSetup();
        private WatcherConfig watchConfig = new WatcherConfig();

        public void Activate(bool isActivated = false)
        {
            try
            {
                folderSetup.InitialSetup();
                FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWorker = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                var caps = capWorker.SetObjects(client.ClientID, false);
                if (BillTurner.ClientAdditionalProperties.ToList().Exists(x => x.Key == client.ClientID))
                    BillTurner.ClientAdditionalProperties.Remove(client.ClientID);
                BillTurner.ClientAdditionalProperties.Add(client.ClientID, caps);
                string directory = Core.ADMS.BaseDirs.createDirectory(Core.ADMS.BaseDirs.getFtpDir(), client.FtpFolder + @"\ADMS");
                folderSetup.SetupClientFolderStructure(directory, client.FtpFolder);
                clientPubCodes = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(client);
                if (!isActivated)
                {
                    //ProcessFilesInDQMQue(client, isActivated);

                    //once done then can start processing new files
                    //AdmsLogCleanUpIncompleteFiles will call a sproc that will clean up any files that previously were stopped mid process because of an engine shutdown.
                    //This code is for updating ADMSLog files that weren't fully completed so they are properly removed from the MVC Dashboard File Status. This will also
                    //provide a message as to why it was incomplete. IE Engine shutoff or Error Threshold reached to display in the MVC Dashboard File History tab
                    AdmsLogCleanUpIncompleteFiles(client, true);
                    //CreateFileSystemWatcher(c, directory);
                    try
                    {
                        var fileTokenSource = new CancellationTokenSource();
                        var fileToken = fileTokenSource.Token;
                        System.Threading.Tasks.Task fileWatcher = new System.Threading.Tasks.Task(() => { CreateFileSystemWatcher(client, directory); }, fileToken);
                        fileWatcher.Start();
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, this.GetType().Name.ToString() + ".FileWatcher");
                    }
                    ProcessFilesInRepository(client, Core.ADMS.BaseDirs.createDirectory(Core.ADMS.BaseDirs.getClientRepoDir(), client.FtpFolder));
                    ProcessFilesInFTP(client, directory);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".FileWatcher");
            }
        }

        public void ProcessFilesInFTP(KMPlatform.Entity.Client client, string directory)
        {
            //Initial Check for File Existing Before Service started.
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            FileInfo[] files = dirInfo.GetFiles();
            if (BillTurner.ClientAdditionalProperties == null || !BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
            {
                FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                cap = capWork.SetObjects(client.ClientID, false);

                if (BillTurner.ClientAdditionalProperties == null)
                {
                    BillTurner.ClientAdditionalProperties = new Dictionary<int, FrameworkUAS.Object.ClientAdditionalProperties>();
                    BillTurner.ClientAdditionalProperties.Add(client.ClientID, cap);
                }
                else
                {
                    BillTurner.ClientAdditionalProperties.Add(client.ClientID, cap);
                }
            }

            foreach (FileInfo file in files)
            {
                FrameworkUAS.Entity.SourceFile sf = new SourceFile();
                string fileNoExtension = System.IO.Path.GetFileNameWithoutExtension(file.Name).ToLower();

                if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.StartsWith(x.FileName.ToLower()) && x.IsDeleted == false))
                {
                    if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.Equals(x.FileName.ToLower())))
                        sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Single(x => fileNoExtension.Equals(x.FileName.ToLower()));
                    else
                        sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.FirstOrDefault(x => fileNoExtension.StartsWith(x.FileName.ToLower()));
                }
                else
                {
                    FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
                    BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList = sfData.Select(client.ClientID, true).Where(x => x.IsDeleted == false).ToList();
                    if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.StartsWith(x.FileName.ToLower())))
                    {
                        if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.Equals(x.FileName.ToLower())))
                            sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Single(x => fileNoExtension.Equals(x.FileName.ToLower()));
                        else
                            sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.FirstOrDefault(x => fileNoExtension.StartsWith(x.FileName.ToLower()));
                    }
                }

                if (sf != null && sf.SourceFileID > 0)
                {
                    AddFileToDQMQue(client, file);
                    var fileDetected = new FileDetected(file, client, sf, true, false);
                    SetStaticProperties(fileDetected);

                    FileDetected(fileDetected);
                }
                else
                {
                    Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.FileNotDefined);
                    em.FileNotDefined(client, file);
                    //delete the file
                    File.Delete(file.FullName);
                    SetPropertiesNull();
                }
            }
        }
        public void ProcessFilesInRepository(KMPlatform.Entity.Client client, string directory)
        {
            //Initial Check for File Existing Before Service started.
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            FileInfo[] files = dirInfo.GetFiles();
            if (BillTurner.ClientAdditionalProperties == null || !BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
            {
                FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                cap = capWork.SetObjects(client.ClientID, false);

                if (BillTurner.ClientAdditionalProperties == null)
                {
                    BillTurner.ClientAdditionalProperties = new Dictionary<int, FrameworkUAS.Object.ClientAdditionalProperties>();
                    BillTurner.ClientAdditionalProperties.Add(client.ClientID, cap);
                }
                else
                {
                    BillTurner.ClientAdditionalProperties.Add(client.ClientID, cap);
                }
            }
            foreach (FileInfo file in files)
            {
                FrameworkUAS.Entity.SourceFile sf = new SourceFile();
                string fileNoExtension = System.IO.Path.GetFileNameWithoutExtension(file.Name).ToLower();

                if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.StartsWith(x.FileName.ToLower()) && x.IsDeleted == false))
                {
                    if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.Equals(x.FileName.ToLower())))
                        sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Single(x => fileNoExtension.Equals(x.FileName.ToLower()));
                    else
                        sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.FirstOrDefault(x => fileNoExtension.StartsWith(x.FileName.ToLower()));
                }
                else
                {
                    FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
                    BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList = sfData.Select(client.ClientID, true).Where(x => x.IsDeleted == false).ToList();
                    if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.StartsWith(x.FileName.ToLower())))
                    {
                        if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.Equals(x.FileName.ToLower())))
                            sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Single(x => fileNoExtension.Equals(x.FileName.ToLower()));
                        else
                            sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.FirstOrDefault(x => fileNoExtension.StartsWith(x.FileName.ToLower()));
                    }
                }

                if (sf != null && sf.SourceFileID > 0)
                {
                    AddFileToDQMQue(client, file);
                    var fileDetected = new FileDetected(file, client, sf, true, true);
                    SetStaticProperties(fileDetected);
                    FileDetected(fileDetected);
                }
                else
                {
                    Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.FileNotDefined);
                    em.FileNotDefined(client, file);
                    //delete the file
                    File.Delete(file.FullName);
                    SetPropertiesNull();
                }
            }
        }
        public void AdmsLogCleanUpIncompleteFiles(KMPlatform.Entity.Client client, bool isADMS)
        {
            FrameworkUAS.BusinessLogic.AdmsLog admsWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
            admsWrk.AdmsLogCleanUp(client.ClientID, isADMS);
        }
        /// <summary>
        /// This returns the parent directory name for the file
        /// </summary>
        /// <param name="file"></param>
        /// <returns>file.Directory.Name</returns>
        public string returnClientName(FileInfo file)
        {
            //Internal grab client based on folder
            if (file.FullName.StartsWith(BaseDirs.getFtpDir()))
            {
                return file.Directory.Name;
            }
            else          //Outside source need to check path and get client name
            {
                string clientDirectoryName = watchConfig.GetClientForOutsideDirectory(Path.GetDirectoryName(file.FullName));
                return clientDirectoryName;
            }
        }

        private void isFileUploaded(KMPlatform.Entity.Client client, FileInfo file)
        {
            if (!checkFullPathDoesntExceedLimit(file))
            {
                #region Check File Locked
                bool locked;
                locked = true;
                do
                {
                    if (File.Exists(file.FullName))
                    {
                        //Try to open file if not locked.
                        try
                        {
                            using (File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                locked = false;
                            }
                        }
                        catch
                        {
                            Thread.Sleep(5000);
                        }
                    }
                    else
                        return;
                } while (locked);
                #endregion
                PublishFileDetected(client, file);
            }
        }
        private void AddFileToDQMQue(KMPlatform.Entity.Client client, FileInfo file)
        {
            #region Add Client File to Processing List Else Pause While Waiting to Free
            //bool wait;
            //wait = false;
            //do
            //{
            //    //wait = ADMSProcessingQue.AddClientFile(client, file);
            //    if (!wait)
            //        Thread.Sleep(60000);

            //} while (!wait);
            #endregion
        }
        public void PublishFileDetected(KMPlatform.Entity.Client client, FileInfo fileName)
        {
            FrameworkUAD_Lookup.BusinessLogic.Code fst = new FrameworkUAD_Lookup.BusinessLogic.Code();
            int fileStatusTypeID = fst.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.File_Status, FrameworkUAD_Lookup.Enums.FileStatusType.Detected.ToString()).CodeId;
            //what if FileName is xxxxFileNamexxxx  or FileNamexxxxx or xxxxFileName
            //IncomingFileName.ToLower().Contains(ServerFileName.ToLower())
            string fileNoExtension = System.IO.Path.GetFileNameWithoutExtension(fileName.Name).ToLower();
            if (BillTurner.ClientAdditionalProperties == null || !BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
            {
                FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                cap = capWork.SetObjects(client.ClientID, false);

                if (BillTurner.ClientAdditionalProperties == null)
                {
                    BillTurner.ClientAdditionalProperties = new Dictionary<int, FrameworkUAS.Object.ClientAdditionalProperties>();
                    BillTurner.ClientAdditionalProperties.Add(client.ClientID, cap);
                }
                else if (!BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
                {
                    BillTurner.ClientAdditionalProperties.Add(client.ClientID, cap);
                }
            }
            try
            {
                if (File.Exists(fileName.FullName))
                {
                    #region Process client file
                    if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.StartsWith(x.FileName.ToLower()) && x.IsDeleted == false))
                    {
                        FrameworkUAS.Entity.SourceFile sf = new SourceFile();
                        if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.Equals(x.FileName.ToLower())))
                            sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Single(x => fileNoExtension.Equals(x.FileName.ToLower())).DeepClone();
                        else
                            sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.FirstOrDefault(x => fileNoExtension.StartsWith(x.FileName.ToLower())).DeepClone();

                        AddFileToDQMQue(client, fileName);
                        var fileDetected = new FileDetected(fileName, client, sf, true, false);
                        SetStaticProperties(fileDetected);
                        ConsoleMessage("Detected File: " + fileName.FullName + " Client: " + client.FtpFolder);
                        FileDetected(fileDetected);
                    }
                    else
                    {
                        FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
                        int previousCount = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Count;
                        BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList = sfData.Select(client.ClientID, true).Where(x => x.IsDeleted == false).ToList();
                        if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.StartsWith(x.FileName.ToLower())))
                        {
                            FrameworkUAS.Entity.SourceFile sf = new SourceFile();
                            if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => fileNoExtension.Equals(x.FileName.ToLower())))
                                sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Single(x => fileNoExtension.Equals(x.FileName.ToLower())).DeepClone();
                            else
                                sf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.FirstOrDefault(x => fileNoExtension.StartsWith(x.FileName.ToLower())).DeepClone();

                            AddFileToDQMQue(client, fileName);
                            var fileDetected = new FileDetected(fileName, client, sf, true, false);
                            SetStaticProperties(fileDetected);
                            ConsoleMessage("Detected File: " + fileName.FullName + " Client: " + client.FtpFolder);
                            FileDetected(fileDetected);
                        }
                        else
                        {                            
                            if (BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID) && (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList == null || BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Count() == 0))
                            {
                                string message = "SourceFileList Count is zero.";
                                if (previousCount > 0)
                                    message = "SourceFileList Count is zero when it previously had values. Potential database call failed.";

                                Exception ex = new Exception(message);
                                LogError(ex, client, this.GetType().Name.ToString() + ".PublishFileDetected");
                            }

                            fileStatusTypeID = fst.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.File_Status, FrameworkUAD_Lookup.Enums.FileStatusType.Invalid.ToString()).CodeId;
                            ConsoleMessage("File name does not exist for client. Client: " + client.FtpFolder + " ClientID: " + client.ClientID.ToString() + " FileName: " + fileName.FullName);
                            Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.FileNotDefined);
                            em.FileNotDefined(client, fileName);
                            File.Delete(fileName.FullName);
                            SetPropertiesNull();
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".PublishFileDetected");
            }
        }

        public bool checkFullPathDoesntExceedLimit(FileInfo fullFilePath)
        {
            string client = returnClientName(fullFilePath);
            StringBuilder biggestString = new StringBuilder();
            List<string> directories = new List<string>();
            directories.Add(BaseDirs.getFtpDir() + client + "\\");
            directories.Add(BaseDirs.getFtpDir() + client + "\\");
            directories.Add(BaseDirs.getFtpDir() + client + "\\");
            directories.Add(BaseDirs.getFtpDir() + client + "\\");

            biggestString.Append(directories.Aggregate(string.Empty, (max, cur) => max.Length > cur.Length ? max : cur));
            biggestString.Append(fullFilePath.Name);

            if (biggestString.Length > 242)
                return true;
            else
                return false;
        }
        public void CheckForFiles()
        {
            DirectoryInfo di = new DirectoryInfo(Core.ADMS.BaseDirs.createDirectory(Core.ADMS.BaseDirs.getFtpDir(), client.FtpFolder + @"\ADMS"));
            foreach (var fi in di.GetFiles())
            {
                if (!checkFullPathDoesntExceedLimit(fi))
                    isFileUploaded(client, fi);
            }
        }
        public FileSystemWatcher CreateFileSystemWatcher(KMPlatform.Entity.Client client, string directory)
        {
            /* Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories. */
            // Watch all files.
            /*
             *          The internal buffer size in bytes. The default is 8192 (8 KB). 
             * You can set the buffer to 4 KB or larger, but it must not exceed 64 KB. I
             * f you try to set the InternalBufferSize property to less than 4096 bytes,
             * your value is discarded and the InternalBufferSize property is set to 4096 bytes. For best performance, 
             * use a multiple of 4 KB on Intel-based computers.
             * 
             * http://stackoverflow.com/questions/6000856/filesystemwatcher-issues/6001055#6001055
             * https://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.internalbuffersize.aspx?f=255&MSPPError=-2147217396
             * 
             */

            var fileSystemWatcher = new FileSystemWatcher
            {
                Path = directory,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.FileName,
                Filter = "*.*",
                IncludeSubdirectories = false,
                EnableRaisingEvents = true,
                InternalBufferSize = 65536
            };

            // Add event handlers.
            fileSystemWatcher.Created += (s, e) => OnCreated(client, e);
            fileSystemWatcher.Renamed += (s, e) => OnRenamed(client, e);
            fileSystemWatcher.Changed += (s, e) => OnChanged(client, e);
            //error detection
            fileSystemWatcher.Error += new ErrorEventHandler(WatcherError);

            // Begin watching.
            if (!FileSystemWatcherDictionary.ContainsKey(directory))
                FileSystemWatcherDictionary[directory] = fileSystemWatcher;

            return fileSystemWatcher;
        }

        // Define the event handlers.
        private void OnCreated(KMPlatform.Entity.Client client, FileSystemEventArgs e)
        {
            try
            {
                if (!e.FullPath.EndsWith("DataCompare", StringComparison.CurrentCultureIgnoreCase))
                {
                    FileInfo file = new FileInfo(e.FullPath);
                    // Specify what is done when a file is changed, created, or deleted.
                    string msg = "FILEWATCHER: Creation Detected, File: " + e.FullPath + " " + e.ChangeType;
                    SaveEngineLog(msg);
                    if (!checkFullPathDoesntExceedLimit(file))
                        isFileUploaded(client, file);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".OnCreated");
            }
        }
        private void OnChanged(KMPlatform.Entity.Client client, FileSystemEventArgs e)
        {
            try
            {
                if (!e.FullPath.EndsWith("DataCompare", StringComparison.CurrentCultureIgnoreCase))
                {
                    FileInfo file = new FileInfo(e.FullPath);
                    // Specify what is done when a file is changed, created, or deleted.
                    string msg = "FILEWATCHER: Creation Detected, File: " + e.FullPath + " " + e.ChangeType;
                    SaveEngineLog(msg);
                    if (!checkFullPathDoesntExceedLimit(file))
                        isFileUploaded(client, file);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".OnCreated");
            }
        }
        private void OnRenamed(KMPlatform.Entity.Client client, FileSystemEventArgs e)
        {
            try
            {
                if (!e.FullPath.EndsWith("DataCompare", StringComparison.CurrentCultureIgnoreCase))
                {
                    FileInfo file = new FileInfo(e.FullPath);
                    // Specify what is done when a file is changed, created, or deleted.
                    string msg = "FILEWATCHER: Creation Detected, File: " + e.FullPath + " " + e.ChangeType;
                    SaveEngineLog(msg);
                    if (!checkFullPathDoesntExceedLimit(file))
                        isFileUploaded(client, file);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".OnCreated");
            }
        }
        // The error event handler
        private void WatcherError(object source, ErrorEventArgs e)
        {
            Exception watchException = e.GetException();
            if (!watchException.Message.StartsWith("Insufficient system resources"))
            {
                LogError(watchException, client, this.GetType().Name.ToString() + ".WatcherError");

                FileSystemWatcher fsw = (FileSystemWatcher) source;
                fsw.Dispose();
                fsw = null;
                //Lets reactivate our Watchers
                Activate();
            }
        }
    }
}