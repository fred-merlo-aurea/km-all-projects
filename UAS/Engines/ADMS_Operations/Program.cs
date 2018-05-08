using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ADMS_Operations
{
    class Program
    {
        static void Main(string[] args)
        {
            clientArchive();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
        public static void clientArchive(string archive = "C:\\ADMS\\Client Archive",  string historicalBackup = "C:\\ADMS\\HistoricalBackups")
        {
            bool fileArchiveExists = Directory.Exists(archive);
            bool backupExists = Directory.Exists(historicalBackup);

            DateTime time = DateTime.Now;

            string[] fileEntries;
            string [] filePath;
            string[] innerFolders;
            string [] oldFiles; 

            string fileName;
            string newFile;
            string format = "M-d-yyyy";
            string backupDate =(time.ToString(format));

            int filesDeleted;
            try
            {
                //Ends program execution if the file archive to be backed up doesn't exist
                if (!fileArchiveExists)
                {
                    Console.WriteLine("Files to backup not found, execution canceled");
                    return;
                }
                else if(!backupExists)
                {
                    Console.WriteLine("Backup location does not exist, execution canceled");
                    return;
                }
                else
                {
                    fileEntries = Directory.GetDirectories(archive);
                    foreach (string f in fileEntries)
                    {
                        if (f.Substring(f.Length - 4, 4) != "ADMS")
                        {
                            filePath = f.Split('\\');
                            fileName = filePath[filePath.Length - 1];

                            newFile = historicalBackup + "\\" + fileName + backupDate + ".zip";
                            newFile.Replace("/", "-");

                            //Checks if the backup has been run the same day if it has skips the file
                            if (File.Exists(newFile))
                            {
                                Console.WriteLine(newFile + " already exists, contiuning to next file");
                                break;
                            }
                            else
                            {
                                ZipFile.CreateFromDirectory(f, newFile);

                                #region Removeing old files
                                innerFolders = Directory.GetDirectories(f);
                                filesDeleted = 0;

                                foreach (String fo in innerFolders)
                                {
                                    oldFiles = Directory.GetFiles(fo);
                                    foreach (String of in oldFiles)
                                    {
                                        
                                        File.Delete(of);
                                        filesDeleted++;
                                    }
                                }
                                //Remove backup zip file if it would be empty
                                if (filesDeleted == 0)
                                {
                                    Console.WriteLine(newFile + "was empty so was not saved");
                                    File.Delete(newFile);
                                }
                                #endregion
                            }
                        }
                    }
                    Console.WriteLine("Process complete");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

        }
    }
}
