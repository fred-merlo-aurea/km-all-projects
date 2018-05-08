using System;
using System.Data;
using System.IO;
using System.Linq;
using ADMS.ClientMethods.Common;
using Core.ADMS;
using Core_AMS.Utilities;
using FrameworkUAS.Entity;
using KM.Common;
using KM.Common.Import;
using BusinessClientMethods = FrameworkUAS.BusinessLogic.ClientMethods;
using FileFunctions = Core_AMS.Utilities.FileFunctions;

namespace ADMS.ClientMethods
{
    public class GLM : ClientSpecialCommon
    {
        private const int Pier92ColumnHeader = 15;

        public void CreateGLMRelationalFiles(KMPlatform.Entity.Client client, FileInfo zipFile, SourceFile cSpecialFile)
        {
            var fileFunctions = new FileFunctions();
            var destinationPath = $"{BaseDirs.getClientArchiveDir()}\\{client.FtpFolder}\\ProcessCMS\\{DateTime.Now:MMddyyyy}";
            fileFunctions.ExtractZipFile(zipFile, destinationPath);

            var filePath = $"{destinationPath}\\GLM_Relational";
            var clientMethods = new BusinessClientMethods();
            clientMethods.GLM_CreateTempCMSTables();

            var checkFiles = new[]
            {
                new { Name = "icff365 drupal leads march 25.csv", UseQuotes = true, Description = "ICFF365" },
                new { Name = "nynow365 drupoal leads march 25.csv", UseQuotes = false, Description = "NYNOW365" },
                new { Name = "sigmix365 drupal leads march 25.csv", UseQuotes = true, Description = "Page Behavior" }
            };

            var fileList = ClientMethodHelpers.GetFilesListWithExistenceCheck(filePath, checkFiles.Select(f => f.Name));
            if (!fileList.Any())
            {
                return;
            }

            try
            {
                foreach (var file in checkFiles)
                {
                    ProcessFile(filePath, clientMethods, file.Name, file.UseQuotes, file.Description);
                }

                Console.WriteLine($"Started: Data Roll Up {DateTime.Now}");
                clientMethods.GLM_Relational_Update();
                Console.WriteLine($"Finished: Data Roll Up {DateTime.Now}");
            }
            catch (Exception ex)
            {
                LogError(ex, client, $"{GetType().Name}.CreateGLMRelationalFiles");
            }

            clientMethods.GLM_DropTempCMSTables();            
        }

        private void ProcessFile(string filePath, BusinessClientMethods clientMethods, string fileName, bool useQuotes, string description)
        {
            var fileConfiguration = new FileConfiguration
            {
                ColumnCount = 8,
                ColumnHeaders = "Company,Username,Name,E-mail,Leads Sent,Likes,Board Follows,Exhibitor Follows",
                FileColumnDelimiter = "comma",
                FileExtension = ".csv",
                FileFolder = filePath,
                IsQuoteEncapsulated = useQuotes
            };

            Console.WriteLine($"Getting {description} {DateTime.Now}");

            var fileInfo = new FileInfo(Path.Combine(filePath, fileName));
            var dataTable = ClientMethodHelpers.ProcessImportVesselData(fileInfo, fileConfiguration);

            InsertGlmRelationalData(clientMethods, dataTable);
        }

        private void InsertGlmRelationalData(BusinessClientMethods clientMethodsData, DataTable dtNyNow)
        {
            Guard.NotNull(clientMethodsData, nameof(clientMethodsData));
            Guard.NotNull(dtNyNow, nameof(dtNyNow));

            Console.WriteLine($"Started: GLM_Relational_InsertData {DateTime.Now}");
            clientMethodsData.GLM_Relational_InsertData(dtNyNow);
            Console.WriteLine($"Finished: GLM_Relational_InsertData {DateTime.Now}");
        }

        //Semi Stubbed out needs to be revised or can take code from maybe?//FIXTHIS
        public void SwipeDataFile(KMPlatform.Entity.Client client, FileInfo swipeFile, SourceFile cSpecialFile)
        {
            var fileWorker = new FileWorker();
            var fileFunctions = new FileFunctions();
            var destinationPath = $"{BaseDirs.getClientArchiveDir()}\\{client.FtpFolder}\\ProcessCMS\\{DateTime.Now:MMddyyyy}";
            var filePath = $"{destinationPath}\\GLM_Relational";

            var checkFiles = new[]
            {
                "Pier92RawET version 2.xlsx",
                "icff365 drupal leads march 25.csv",
                "nynow365 drupoal leads march 25.csv",
                "sigmix365 drupal leads march 25.csv",
            };
            var fileList = ClientMethodHelpers.GetFilesListWithExistenceCheck(filePath, checkFiles);

            if (!fileList.Any())
            {
                return;
            }

            try
            {
                //FrameworkUAD.Object.ImportVessel iv = null;

                #region Pier92RawET version 2.xlsx Data
                var fcAccess = ClientMethodHelpers.CreateFileConfiguration(Pier92ColumnHeader, Consts.ScanTimeColumnHeaders, string.Empty, Consts.XlsxExtension, false);
                fcAccess.FileFolder = filePath;

                Console.WriteLine($"Getting Pier92RawET {DateTime.Now}");

                DataTable dtAccess = fileWorker.GetData(fileList.Single(x => x.Name.Equals("Pier92RawET version 2.xlsx", StringComparison.CurrentCultureIgnoreCase)), fcAccess);
                //ImportVessel iv = null;
                //totalRowCount = fw.GetRowCount(new FileInfo(filePath + "\\Access.csv"));
                //rowProcessedCount = 0;
                //DataTable dtAccess = null;
                //firstRun = true;
                //if (totalRowCount > 0)
                //{
                //    while (rowProcessedCount < totalRowCount)
                //    {
                //        int endRow = rowProcessedCount + rowBatch;
                //        if (endRow > totalRowCount)
                //            endRow = totalRowCount;
                //        int startRow = rowProcessedCount + 1;
                //        double percent = (int)Math.Round((double)(100 * startRow) / totalRowCount);
                //        Console.WriteLine("Processing: " + startRow + " to " + endRow + " " + DateTime.Now.ToString() + " " + percent.ToString() + "%");
                //        iv = fw.GetImportVessel(new FileInfo(filePath + "\\Access.csv"), startRow, rowBatch, fcAccess);
                //        rowProcessedCount += iv.TotalRowCount;
                //        if (firstRun)
                //        {
                //            dtAccess = iv.DataOriginal.Copy();
                //            firstRun = false;
                //        }
                //        else
                //            dtAccess.Merge(iv.DataOriginal);

                //        iv.DataOriginal.Dispose();
                //    }
                //}

                //DataColumn[] accessKeys = new DataColumn[1];
                //accessKeys[0] = dtAccess.Columns["AccessID"];
                //dtAccess.PrimaryKey = accessKeys;
                //dtAccess.AcceptChanges();

                Console.WriteLine("Started: GLM_Relational_Insert_Pier92 " + DateTime.Now.ToString());
                //FIXTHIScmData.GLM_Relational_Insert_Pier92(dtAccess);
                Console.WriteLine("Finished: GLM_Relational_Insert_Pier92 " + DateTime.Now.ToString());
                dtAccess.Dispose();
                dtAccess = null;
                #endregion

                Console.WriteLine("Started: Data Roll Up " + DateTime.Now.ToString());
                //FIXTHIScmData.GLM_Relational_CleanUpData();
                Console.WriteLine("Finished: Data Roll Up " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".SwipeDataFile");
            }


            #region Create csv Files for Mapping
            //so now have one big DataTable - need a sourceFile and Mapping for this so validations can be performed
            //for testing lets write this dataTable out to a csv file to look at it
            int BatchSize = 10000;
            int total = 0;
            int counter = 0;
            int processedCount = 0;

            #region BM_RelData
            string pubFile = filePath + "\\BM_RelData.csv";
            int pubCount = 0;//FIXTHIS cmData.GLM_Relational_Get_Count();
            if (pubCount > BatchSize)
            {
                //will have a RowID column                    
                DataTable dtPub = new DataTable();//FIXTHIS cmData.GLM_Select_Data_Paging(counter, BatchSize);
                dtPub.Columns.Add("RowNum");
                dtPub.AcceptChanges();
                int rowID = 1;
                foreach (DataRow dr in dtPub.Rows)
                {
                    dr["RowRowNumID"] = rowID.ToString();
                    rowID++;
                }
                dtPub.AcceptChanges();
                fileFunctions.CreateCSVFromDataTable(dtPub, pubFile, true);
                dtPub.Dispose();
                dtPub = null;
            }
            else
            {
                total = pubCount;
                counter = 1;
                processedCount = 0;
                while (processedCount < total)
                {
                    DataTable dtPub = new DataTable();//FIXTHIScmData.GLM_Select_Data();
                    fileFunctions.CreateCSVFromDataTable(dtPub, pubFile, false);
                    processedCount += dtPub.Rows.Count;
                    dtPub.Dispose();
                    dtPub = null;
                    counter++;
                }
            }
            #endregion

            #endregion

            //FIXTHIScmData.GLM_DropTempCMSTables();

            #region Upload to FTP
            Console.WriteLine("Uploading to FTP " + DateTime.Now.ToString());
            FrameworkUAS.BusinessLogic.ClientFTP blcftp = new FrameworkUAS.BusinessLogic.ClientFTP();
            ClientFTP clientFTP = new ClientFTP();
            clientFTP = blcftp.SelectClient(client.ClientID).First();
            Core_AMS.Utilities.FtpFunctions ftp = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
            ftp.Upload(clientFTP.Folder + "\\AutoGen_GLM_RelationalFile.csv", filePath + "\\GLM_RelData.csv");
            Console.WriteLine("Finished Uploading to FTP " + DateTime.Now.ToString());
            #endregion
        }
    }
}
