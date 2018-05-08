using System;
using System.Data;
using System.IO;
using System.Linq;
using ADMS.ClientMethods.Common;
using Core.ADMS;
using Core_AMS.Utilities;
using FrameworkUAD.BusinessLogic;
using FrameworkUAS.Entity;
using KM.Common;
using KM.Common.Import;
using BusinessClientMethods = FrameworkUAS.BusinessLogic.ClientMethods;
using FileFunctions = Core_AMS.Utilities.FileFunctions;

namespace ADMS.ClientMethods
{
    public class Northstar : ClientSpecialCommon
    {
        private const int WebPersonColumnHeader = 30;
        private const string NorthstarRelationalAddGroupCommand = "Northstar_Relational_AddGroup";
        private const string NorthstarRelationalInsertPersonCommand = "Northstar_Relational_InsertPerson";

        public void CreateNorthstarWebFiles(KMPlatform.Entity.Client client, FileInfo zipFile, SourceFile cSpecialFile)
        {
            var ivWorker = new ImportVessel();
            var fileWorker = new FileWorker();
            var fileFunctions = new FileFunctions();
            var destinationPath = $"{BaseDirs.getClientArchiveDir()}\\{client.FtpFolder}\\ProcessCMS\\{DateTime.Now:MMddyyyy}";
            fileFunctions.ExtractZipFile(zipFile, destinationPath);
            var filePath = $"{destinationPath}\\Northstar_Web_Files";
            var cmData = new BusinessClientMethods();
            cmData.Northstar_CreateTempCMSTables();

            var checkFiles = new[]
            {
                "WEB_Group.txt",
                "WEB_Person.txt"
            };
            var fileList = ClientMethodHelpers.GetFilesListWithExistenceCheck(filePath, checkFiles);

            if (!fileList.Any())
            {
                return;
            }

            #region Process Zip
            int totalRowCount = 0;
            int rowProcessedCount = 0;
            int rowBatch = 2500;
            bool firstRun = true;
            try
            {
                FrameworkUAD.Object.ImportVessel iv = null;

                #region WEB_Person.txt Data
                var fcPerson = ClientMethodHelpers.CreateFileConfiguration(
                    WebPersonColumnHeader, Consts.PubCodesColumnHeaders, Consts.TabColumnDelimiter, Consts.TxtExtension, true);
                fcPerson.FileFolder = filePath;

                Console.WriteLine($"Getting WEB_Person {DateTime.Now}");

                var fileInfoWebPerson = new FileInfo(Path.Combine(filePath, "WEB_Person.txt"));
                var dtPerson = ClientMethodHelpers.ProcessImportVesselData(fileInfoWebPerson, fcPerson);

                LogNorthstarRelationalMessage(cmData, dtPerson, NorthstarRelationalInsertPersonCommand);
                #endregion

                #region WEB_Group.txt Data
                //1) Read in WEB_Group.txt
                //a) gives us PubCode,GlobalUserKey,GroupId,IsRecent,AddDate,DropDate
                FileConfiguration fcGroup = new FileConfiguration();
                fcGroup.ColumnCount = 6;
                fcGroup.ColumnHeaders = "PubCode,GlobalUserKey,GroupId,IsRecent,AddDate,DropDate";
                fcGroup.FileColumnDelimiter = "tab";
                fcGroup.FileExtension = ".txt";
                fcGroup.FileFolder = filePath;
                fcGroup.IsQuoteEncapsulated = false;
                Console.WriteLine("Getting WEB_Group " + DateTime.Now.ToString());

                var fileInfoWebGroup = new FileInfo(Path.Combine(filePath, "WEB_Group.txt"));
                var dtGroup = ClientMethodHelpers.ProcessImportVesselData(fileInfoWebGroup, fcGroup);

                LogNorthstarRelationalMessage(cmData, dtGroup, NorthstarRelationalAddGroupCommand);
                #endregion

                #region Create File
                if (File.Exists("C:\\ADMS\\Client Archive\\Northstar\\Northstar_WebRelData.csv"))
                    File.Delete("C:\\ADMS\\Client Archive\\Northstar\\Northstar_WebRelData.csv");

                #region BM_RelData
                int BatchSize = 10000;
                int total = 0;
                int counter = 0;
                int processedCount = 0;
                //string pubFile = filePath + "\\Northstar_WebRelData.csv";
                int pubCount = cmData.Northstar_Get_WEB_Person_Group_Get_Count();
                if (pubCount < BatchSize)
                {
                    //will have a RowID column                    
                    DataTable workTable = cmData.Northstar_Get_WEB_Person_Group_Data();
                    workTable.Columns.Add("RowNum");
                    workTable.AcceptChanges();
                    int rowID = 1;
                    foreach (DataRow dr in workTable.Rows)
                    {
                        dr["RowNum"] = rowID.ToString();
                        rowID++;
                    }
                    workTable.AcceptChanges();
                    fileFunctions.CreateCSVFromDataTable(workTable, "C:\\ADMS\\Client Archive\\Northstar\\Northstar_WebRelData.csv", false);
                    workTable.Dispose();
                    workTable = null;
                }
                else
                {
                    total = pubCount;
                    counter = 1;
                    processedCount = 0;
                    while (processedCount < total)
                    {
                        DataTable workTable = cmData.Northstar_Get_WEB_Person_Group_Data_Paging(counter, BatchSize);
                        fileFunctions.CreateCSVFromDataTable(workTable, "C:\\ADMS\\Client Archive\\Northstar\\Northstar_WebRelData.csv", false);
                        processedCount += workTable.Rows.Count;
                        workTable.Dispose();
                        workTable = null;
                        counter++;
                    }
                }
                #endregion

                cmData.Northstar_DropTempCMSTables();
                #endregion

            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".CreateNorthstarWebFiles");
            }

            #endregion

            Console.WriteLine("Uploading to FTP " + DateTime.Now.ToString());
            FrameworkUAS.BusinessLogic.ClientFTP blcftp = new FrameworkUAS.BusinessLogic.ClientFTP();
            ClientFTP clientFTP = new ClientFTP();
            clientFTP = blcftp.SelectClient(client.ClientID).First();
            Core_AMS.Utilities.FtpFunctions ftp = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
            ftp.Upload(clientFTP.Folder + "\\AutoGen_Northstar_WEB_Files.csv", "C:\\ADMS\\Client Archive\\Northstar\\Northstar_WebRelData.csv");
            Console.WriteLine("Finished Uploading to FTP " + DateTime.Now.ToString());
        }

        private void LogNorthstarRelationalMessage(BusinessClientMethods businessClientMethods, DataTable dtGroup, string command)
        {
            Guard.NotNull(businessClientMethods, nameof(businessClientMethods));
            Guard.NotNull(dtGroup, nameof(dtGroup));
            Console.WriteLine($"Started: {command} {DateTime.Now}");

            if (command.Equals(NorthstarRelationalInsertPersonCommand))
            {
                businessClientMethods.Northstar_Relational_InsertPerson(dtGroup);
            }
            else
            {
                businessClientMethods.Northstar_Relational_AddGroup(dtGroup);
            }
            
            Console.WriteLine($"Finished: {command} {DateTime.Now}");
        }
    }
}
