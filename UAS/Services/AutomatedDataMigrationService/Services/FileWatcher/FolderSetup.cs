using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.ADMS;

namespace ADMS.Services.FileWatcher
{
    public class FolderSetup: ServiceBase
    {
        public Dictionary<string, string> BaseDirectories
        {
            get { return BaseDirs.GetBaseDirectories(); }
        }
        public void InitialSetup()
        {
            foreach (KeyValuePair<string, string> kvp in Core.ADMS.BaseDirs.GetBaseDirectories())
            {
                if (!Directory.Exists(kvp.Value))
                    Directory.CreateDirectory(kvp.Value);
            }
        }
        public void SetupClientFolderStructure(KMPlatform.Entity.Client client)
        {
            foreach (KeyValuePair<string, string> directories in BaseDirectories)
            {
                if (!Directory.Exists(Core.ADMS.BaseDirs.createDirectory(directories.Value, client.FtpFolder)))
                {
                    Directory.CreateDirectory(Core.ADMS.BaseDirs.createDirectory(directories.Value, client.FtpFolder));
                }
            }
        }
        public void SetupClientFolderStructure(string directoryName, string clientName)
        {            
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            foreach (KeyValuePair<string, string> directories in BaseDirectories)
            {
                if (!Directory.Exists(Core.ADMS.BaseDirs.createDirectory(directories.Value, clientName)))
                {
                    Directory.CreateDirectory(Core.ADMS.BaseDirs.createDirectory(directories.Value, clientName));
                }

                //Creates the Processed and Invalid folders for the Client Archive -> Client path
                if (directories.Value.Equals(Core.ADMS.BaseDirs.getClientArchiveDir(), StringComparison.CurrentCultureIgnoreCase))
                {
                    var archiveLocation = Core.ADMS.BaseDirs.getClientArchiveDir();
                    var processedLocation = "Processed";
                    var invalidLocation = "Invalid";
                    var reportsLocation = "Reports";

                    //Create folder if it doesn't exist
                    var processedFileArchiveLocation = archiveLocation + "\\" + clientName + "\\" + processedLocation + "\\";
                    var processedFileArchiveLocationExist = Directory.Exists(processedFileArchiveLocation);
                    if (processedFileArchiveLocationExist == false) Directory.CreateDirectory(processedFileArchiveLocation);

                    //Create folder if it doesn't exist
                    var invalidFileArchiveLocation = archiveLocation + "\\" + clientName + "\\" + invalidLocation + "\\";
                    var doesInvalidFileArchiveLocationExist = Directory.Exists(invalidFileArchiveLocation);
                    if (doesInvalidFileArchiveLocationExist == false) Directory.CreateDirectory(invalidFileArchiveLocation);

                    //Create folder if it doesn't exist
                    var reportsFileArchiveLocation = archiveLocation + "\\" + clientName + "\\" + reportsLocation + "\\";
                    var doesReportsFileArchiveLocationExist = Directory.Exists(reportsFileArchiveLocation);
                    if (doesReportsFileArchiveLocationExist == false) Directory.CreateDirectory(reportsFileArchiveLocation);
                }
            }
        }
    }
}
