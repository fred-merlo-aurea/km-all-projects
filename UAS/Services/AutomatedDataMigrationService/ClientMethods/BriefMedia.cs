using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ADMS.ClientMethods.Common;
using Core.ADMS.Events;
using Core_AMS.Utilities;
using FrameworkUAS.Entity;
using KM.Common;
using KM.Common.Import;
using KMPlatform.Entity;
using FileFunctions = Core_AMS.Utilities.FileFunctions;
using UasBusinessLogic = FrameworkUAS.BusinessLogic;

namespace ADMS.ClientMethods
{
    public class BriefMedia : ClientSpecialCommon
    {
        private const int RowBatchSize = 2500;
        private const int DatabaseBatchSize = 250000;
        private const int BatchSize10000 = 10000;
        private const int ColumnCount2 = 2;
        private const int ColumnCount5 = 5;
        private const int ColumnCount6 = 6;
        private const int ColumnCount8 = 8;
        private const int AlgorithmsColumnCount = 21;
        private const string AutoGenBmAlgorithmsFileName = "AutoGen_BM_AlgorithmsFile.csv";
        private const string BmRelDataCsvFileName = "BM_RelData.csv";
        private const string AutoGenBmRelDataCsvFileName = "AutoGen_BM_RelationalFile.csv";
        private const string BmRelationalFolderName = "BM_Relational";
        private const string FileColumnDelimeterComma = "comma";
        private const string FileNameAccessCsv = "Access.csv";
        private const string FileNameUsersCsv = "Users.csv";
        private const string FileNameTaxonomyBehaviorCsv = "TaxonomyBehavior.csv";
        private const string FileNamePageBehaviorCsv = "PageBehavior.csv";
        private const string FileNameSearchBehaviorCsv = "SearchBehavior.csv";
        private const string FileNameTopicCodeCsv = "TopicCode.csv";

        private static readonly string FileExtension = ".csv";
        private static readonly string FileColumnDelimiter = "comma";
        private static readonly string VTBDnpPhoneGroupName = "BriefMedia_VtbDnpPhone";
        private static readonly string CustomerIdStandardField = "Customer_ID";
        private static readonly string CustomerIdFieldName = "Customer Id";
        private static readonly string VTBPubCode = "VTB";
        private static readonly string VTBDnpEmailGroupName = "BriefMedia_VtbDnpEmail";
        private static readonly string CBDnpEmailGroupName = "BriefMedia_CbDnpEmail";
        private static readonly string CBPubCode = "CB";
        private static readonly string CbDnpPhoneGroupName = "BriefMedia_CbDnpPhone";
        private static readonly string CbDnpPostalGroupName = "BriefMedia_CbDnpPostal";

        private const string BriefMediaAlgorithmsCode = "BriefMedia_Algorithms";
        private const string BmAutoGenAlgorithmsFilePath = "C:\\ADMS\\Client Archive\\BriefMedia\\BM_AutoGen_Algorithms.csv";

        public enum BMRelationalFileTypes
        {
            Access,
            PageBehavior,
            SearchBehavior,
            TaxonomyBehavior,
            TopicCode,
            Users
        }

        public FrameworkUAD.Object.ImportFile RelationalFileBundle(SourceFile cSpecialFile, FileValidated eventMessage)
        {
            FrameworkUAD.Object.ImportFile dataIV = new FrameworkUAD.Object.ImportFile();
            //JW - notes
            //Below is a situation where 6 data files are processed at once – this is obviously not natively supported since the FileWatcher processes on a per file basis.  
            //The only way to get this type of process automated is to require that the files be bundled together into one zip (.zip) file.  
            //Then File Watcher can pickup that single zip file, unpack the contents and process all files in zip container as a single batch.  
            

            //6 files: Relational Data

            //Access.csv
            //PageBehavior.csv
            //SearchBehavior.csv
            //TaxonomyBehavior.csv
            //Users.csv
            //TopicCode.csv


            //What we did the first time to create relational data:
            //1)    Read all files into separate Tables (Users, Access, TaxBehavior, TopicCode, PageBehavior, SearchBehavior)

            //2)    Join Users and Access on Drupal_ID, read into a fixed relational table (BMWU)
            //        a.     This gives us name, email, and Drupal_ID to match into UAD, and accessID to join in relational data
            //        b.     However, there will be multiple AccessID’s per DrupalID that we need to STUFF later.

            //3)    With TaxBehavior, select distinct AccessID and STUFF all topiccodes for that ID into #tmptable
            //        65177701	23407
            //        65177701	103
            //        65177701	105
            //          Becomes
            //        65177701	23407, 103, 105
		
            //        a)    Then update BMWU by joining to #tmptable on acessID to get consensus listing of topiccodes

            //4)    BMWU, join PageBehavior on accessID to set pageID (adhoc field)

            //5)    BMWU, join SearchBehavior on accessID to set search_term (adhoc field)

            //6)    Drop records from BMWU that do not have valid email addresses (i.e. delete where emails like “Province of China”)

            //7)    Roll multiple Drupal_ID’s into distinct with (Stuff TopicCode, Stuff Search_Term, Stuff Page_ID, Stuff AccessID)
            //        a.     This takes a while, but we need distinct records and customer wants consensus of those fields

            //8)      With TopicCode, select distinct email and (STUFF topiccodes) and (STUFF TopicCodeText as search_term) for it into #tmp2
            //        a.       Full Join BMWU to #tmp2, append new topiccode’s and new search_term’s (keep orphan contacts from both sides)
            //        // We haven’t run this file yet, it was included with the current refresh

            //9)      At this point, the relational data could be inserted as a source, since all contacts will have name and/or email.  
                    //The matching process should pick them up and match them to any other contacts that have the same name/email.  
                    //We originally ran Users.csv as a sourcefile, then matched the relational table to it.

            //See proc:  [216.17.41.196].[dbo].[BRIEFMEDIA_SP_SPECIFIC_TAXID_TO_WORKING], Taxonomy_ID became TopicCode

            return dataIV;
        }

        public void CreateBMRelationalFiles(Client kmClient, SourceFile cSpecialFile, ClientCustomProcedure ccp,
            FileMoved zipFile)
        {
            var ff = new FileFunctions();
            var destinationPath = $"{Core.ADMS.BaseDirs.getClientArchiveDir()}\\" +
                                  $"{kmClient.FtpFolder}\\ProcessCMS\\{DateTime.Now.ToString("MMddyyyy")}";
            ff.ExtractZipFile(zipFile.ImportFile, destinationPath);
            var filePath = $"{destinationPath}\\{BmRelationalFolderName}";
            var cmData = new UasBusinessLogic.ClientMethods();
            cmData.BriefMedia_CreateTempCMSTables(kmClient);

            if (FileExistsCheck(filePath))
            {
                try
                {
                    //Access Data
                    BMRelationalFilesBatching(BMRelationalFileTypes.Access, filePath, kmClient);

                    //Users Data
                    BMRelationalFilesBatching(BMRelationalFileTypes.Users, filePath, kmClient);

                    //TaxonomyBehavior Data
                    BMRelationalFilesBatching(BMRelationalFileTypes.TaxonomyBehavior, filePath, kmClient);

                    //PageBehavior Data
                    BMRelationalFilesBatching(BMRelationalFileTypes.PageBehavior, filePath, kmClient);

                   //SearchBehavior Data
                    BMRelationalFilesBatching(BMRelationalFileTypes.SearchBehavior, filePath, kmClient);

                   //TopicCode Data
                    BMRelationalFilesBatching(BMRelationalFileTypes.TopicCode, filePath, kmClient);

                    Console.WriteLine($"Started: Data Roll Up {DateTime.Now.ToString()}");
                    cmData.BriefMedia_Relational_CleanUpData(kmClient);
                    Console.WriteLine($"Finished: Data Roll Up {DateTime.Now.ToString()}");
                }
                catch (Exception ex)
                {
                    LogError(ex, kmClient, $"{this.GetType().Name.ToString()}.CreateBMRelationalFiles");
                }

                /* Create csv Files for Mapping:
                 * so now have one big DataTable - need a sourceFile and Mapping for this so validations can be 
                 * performed for testing lets write this dataTable out to a csv file to look at it */
                CreateCsvFilesForMapping(filePath, kmClient);

                cmData.BriefMedia_DropTempCMSTables(kmClient);

                Console.WriteLine($"Uploading to FTP {DateTime.Now.ToString()}");
                var blcftp = new UasBusinessLogic.ClientFTP();
                var clientFtp = blcftp.SelectClient(kmClient.ClientID).First();
                var ftp = new FtpFunctions(clientFtp.Server, clientFtp.UserName, clientFtp.Password);
                ftp.Upload(
                    $"{clientFtp.Folder}\\{AutoGenBmRelDataCsvFileName}", 
                    $"{filePath}\\{BmRelDataCsvFileName}");
                Console.WriteLine($"Finished Uploading to FTP {DateTime.Now.ToString()}");
            }
        }

        public void BriefMedia_Algorithms(Client clientEntity, FileInfo fileInfo, SourceFile clientSpecialFile)
        {
            ClientMethodHelpers.DeleteExistingFiles(BmAutoGenAlgorithmsFilePath);

            try
            {
                var fcAccess = ClientMethodHelpers.CreateFileConfiguration(
                    AlgorithmsColumnCount, Consts.BriefMediaColumnHeaders, Consts.CommaFileDelimiter, Consts.CsvExtension, false);
                Console.WriteLine(Consts.MessageGettingAlgorithms, DateTime.Now);
                
                var dtAccess = ClientMethodHelpers.ProcessImportVesselData(fileInfo, fcAccess);
                if (dtAccess != null)
                {
                    CreateCsvForBmAutoGenAlgorithms(dtAccess);
                }
            }
            // Must take any exception that arises
            catch (Exception ex)
            {
                LogError(ex, clientEntity, $"{GetType().Name}{Consts.DotSepator}{BriefMediaAlgorithmsCode}");
            }                                     
                
            ClientMethodHelpers.FinishingUploadToFtp(clientEntity, AutoGenBmAlgorithmsFileName, BmAutoGenAlgorithmsFilePath);
        }

        public void CreateCsvForBmAutoGenAlgorithms(DataTable dtAccess)
        {
            Guard.NotNull(dtAccess, nameof(dtAccess));
            var totalWorkRows = dtAccess.Rows.Count;
            var workProcessed = 0;
            var workTable = dtAccess.Clone();
            workTable.Columns.Add(Consts.PubCodeKey);

            while (workProcessed < totalWorkRows)
            {
                workTable.Clear();
                var newRows = new Dictionary<int, DataRow>();
                var deleteRows = new Dictionary<int, DataRow>();
                var rowIndex = 0;

                var batchProcessing = workProcessed + Consts.RowBatch;
                if (batchProcessing > totalWorkRows)
                {
                    batchProcessing = totalWorkRows;
                }

                ConsoleMessage($"{Consts.NewBatchStatus}{batchProcessing}", BriefMediaAlgorithmsCode, false);
                ConsoleMessage($"{Consts.ProcessedStatus}{workProcessed}", BriefMediaAlgorithmsCode, false);

                for (var i = workProcessed; i < batchProcessing; i++)
                {
                    var dataRowAccess = dtAccess.Rows[workProcessed];
                    var pubCodes = new List<string>();

                    ClientMethodHelpers.AddPubCode(dataRowAccess["2014_Update"].ToString(), pubCodes, "ALGUP14");
                    ClientMethodHelpers.AddPubCode(dataRowAccess["2013_Update"].ToString(), pubCodes, "ALGUP13");
                    ClientMethodHelpers.AddPubCode(dataRowAccess["2012_Update"].ToString(), pubCodes, "ALGUP12");
                    ClientMethodHelpers.AddPubCode(dataRowAccess["2011_Update"].ToString(), pubCodes, "ALGUP11");
                    ClientMethodHelpers.AddPubCode(dataRowAccess["2010_Update"].ToString(), pubCodes, "ALGUP10");
                    ClientMethodHelpers.AddPubCode(dataRowAccess["Binder"].ToString(), pubCodes, "ALGB");
                    ClientMethodHelpers.AddPubCode(dataRowAccess["Spiral"].ToString(), pubCodes, "ALGS");

                    foreach (var pub in pubCodes)
                    {
                        var pubCode = pub.Trim();
                        ClientMethodHelpers.AddRow(pubCode, newRows.Count + 1, newRows, workTable, dataRowAccess);
                    }

                    deleteRows.Add(rowIndex, dataRowAccess);
                    rowIndex++;
                    workProcessed++;
                }
                foreach (var row in newRows)
                {
                    workTable.Rows.Add(row.Value);
                }
                workTable.AcceptChanges();

                var fileFunctions = new FileFunctions();
                fileFunctions.CreateCSVFromDataTable(workTable, BmAutoGenAlgorithmsFilePath, false);
                GC.Collect();
            }
        }

        public void VtbDnpPhone(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    Client = client,
                    FileExtension = FileExtension,
                    FileColumnDelimiter = FileColumnDelimiter,
                    AdHocDimensionGroupName = VTBDnpPhoneGroupName,
                    CreatedDimension = string.Empty,
                    StandardField = CustomerIdStandardField,
                    IsPubcodeSpecific = true,
                    DimensionValue = string.Empty,
                    MatchValueField = CustomerIdFieldName,
                    DimensionOperator = EqualOperation,
                    UpdateUAD = false,
                    AdditionalInitFunction = InitializePubCode(VTBPubCode)
                });
        }

        public void VtbDnpEmail(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    Client = client,
                    FileExtension = FileExtension,
                    FileColumnDelimiter = FileColumnDelimiter,
                    AdHocDimensionGroupName = VTBDnpEmailGroupName,
                    CreatedDimension = string.Empty,
                    StandardField = CustomerIdStandardField,
                    IsPubcodeSpecific = true,
                    DimensionValue = string.Empty,
                    MatchValueField = CustomerIdFieldName,
                    DimensionOperator = EqualOperation,
                    UpdateUAD = false,
                    AdditionalInitFunction = InitializePubCode(VTBPubCode)
                });
        }

        public void CbDnpEmail(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    Client = client,
                    FileExtension = FileExtension,
                    FileColumnDelimiter = FileColumnDelimiter,
                    AdHocDimensionGroupName = CBDnpEmailGroupName,
                    CreatedDimension = string.Empty,
                    StandardField = CustomerIdStandardField,
                    IsPubcodeSpecific = true,
                    DimensionValue = string.Empty,
                    MatchValueField = CustomerIdFieldName,
                    DimensionOperator = EqualOperation,
                    UpdateUAD = false,
                    AdditionalInitFunction = InitializePubCode(CBPubCode)
                });
        }

        public void CbDnpPhone(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    Client = client,
                    FileExtension = FileExtension,
                    FileColumnDelimiter = FileColumnDelimiter,
                    AdHocDimensionGroupName = CbDnpPhoneGroupName,
                    CreatedDimension = string.Empty,
                    StandardField = CustomerIdStandardField,
                    IsPubcodeSpecific = true,
                    DimensionValue = string.Empty,
                    MatchValueField = CustomerIdFieldName,
                    DimensionOperator = EqualOperation,
                    UpdateUAD = false,
                    AdditionalInitFunction = InitializePubCode(CBPubCode)
                });
        }

        public void CbDnpPostal(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    Client = client,
                    FileExtension = FileExtension,
                    FileColumnDelimiter = FileColumnDelimiter,
                    AdHocDimensionGroupName = CbDnpPostalGroupName,
                    CreatedDimension = string.Empty,
                    StandardField = CustomerIdStandardField,
                    IsPubcodeSpecific = true,
                    DimensionValue = string.Empty,
                    MatchValueField = CustomerIdFieldName,
                    DimensionOperator = EqualOperation,
                    UpdateUAD = false,
                    AdditionalInitFunction = InitializePubCode(CBPubCode)
                });
        }

        private static Action<AdHocDimensionGroup> InitializePubCode(string code)
        {
            return adg =>
            {
                var pubCodeMapGroup = new AdHocDimensionGroupPubcodeMap
                {
                    AdHocDimensionGroupId = adg.AdHocDimensionGroupId,
                    CreatedByUserID = 1,
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    Pubcode = code,
                    UpdatedByUserID = 1
                };

                var mWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroupPubcodeMap();
                mWorker.Save(pubCodeMapGroup);
            };
        }

        private void BMRelationalFilesBatching(BMRelationalFileTypes bmRelationalFileTypes, string filePath,
            Client kmClient)
        {
            var totalBatched = 0;
            var rowProcessedCount = 0;
            var firstRun = true;
            var totalRowCount = 0;
            var isRegularBatching = false;
            var fileWorker = new FileWorker();
            var ivData = new FrameworkUAD.BusinessLogic.ImportVessel();

            var fileConfiguration = GetFileConfigForBMRelationalFiles(bmRelationalFileTypes, filePath);

            Console.WriteLine($"Getting Access {DateTime.Now.ToString()}");

            totalRowCount =
                fileWorker.GetRowCount(
                    new FileInfo($"{filePath}\\{GetCsvFileName(bmRelationalFileTypes)}"));

            if (totalRowCount > 0)
            {
                totalBatched += DatabaseBatchSize;
                DataTable dataTable = null;
                if (totalRowCount < DatabaseBatchSize)
                {
                    isRegularBatching = true;
                }

                while (rowProcessedCount < totalRowCount)
                {
                    var endRow = rowProcessedCount + RowBatchSize;
                    if (endRow > totalRowCount)
                    {
                        endRow = totalRowCount;
                    }

                    var startRow = rowProcessedCount + 1;
                    var percent = (int) Math.Round((double) (100 * startRow) / totalRowCount);

                    Console.WriteLine(
                        $"Processing: {startRow} to {endRow} {DateTime.Now.ToString()} {percent.ToString()}%");

                    var importVessel = ivData.GetImportVessel(
                        new FileInfo(
                            $"{filePath}\\{GetCsvFileName(bmRelationalFileTypes)}"),
                        startRow, 
                        RowBatchSize, 
                        fileConfiguration);

                    rowProcessedCount += importVessel.TotalRowCount;
                    if (firstRun)
                    {
                        dataTable = importVessel.DataOriginal.Copy();
                        firstRun = false;
                    }
                    else
                    {
                        dataTable.Merge(importVessel.DataOriginal);
                    }

                    importVessel.DataOriginal.Dispose();

                    //DatabaseBatching
                    if (!isRegularBatching)
                    {
                        if (rowProcessedCount == totalBatched)
                        {
                            SetDataTablePrimaryKey(bmRelationalFileTypes, ref dataTable);
                            InsertUpdateRelationalData(bmRelationalFileTypes, kmClient, dataTable);

                            firstRun = true;
                            totalBatched += DatabaseBatchSize;
                            if (totalBatched > totalRowCount)
                            {
                                totalBatched = totalRowCount;
                            }

                            Console.WriteLine("Database Batch Processed.");
                            GC.Collect();
                        }
                    }
                }

                //RegularBatching
                if (isRegularBatching)
                {
                    SetDataTablePrimaryKey(bmRelationalFileTypes, ref dataTable);
                    InsertUpdateRelationalData(bmRelationalFileTypes, kmClient, dataTable);
                }
            }
        }

        private FileConfiguration GetFileConfigForBMRelationalFiles(BMRelationalFileTypes bmRelationalFileTypes,
            string filePath)
        {
            var fileConfiguration = new FileConfiguration()
            {
                FileColumnDelimiter = FileColumnDelimeterComma,
                FileExtension = FileExtension,
                FileFolder = filePath,
            };

            switch (bmRelationalFileTypes)
            {
                case BMRelationalFileTypes.Access:
                    fileConfiguration.ColumnCount = ColumnCount5;
                    fileConfiguration.ColumnHeaders = "AccessID,UserID,DateTime,IP,SessionID";
                    fileConfiguration.IsQuoteEncapsulated = true;
                    break;
                case BMRelationalFileTypes.Users:
                    fileConfiguration.ColumnCount = ColumnCount8;
                    fileConfiguration.ColumnHeaders =
                        "Drupal_User_id,First_Name,Last_Name,Title,Suffix,Country,Email,Registration_Date";
                    fileConfiguration.IsQuoteEncapsulated = true;
                    break;
                case BMRelationalFileTypes.TaxonomyBehavior:
                    fileConfiguration.ColumnCount = ColumnCount2;
                    fileConfiguration.ColumnHeaders = "Access_ID,Taxonomy_ID";
                    fileConfiguration.IsQuoteEncapsulated = false;
                    break;
                case BMRelationalFileTypes.PageBehavior:
                    fileConfiguration.ColumnCount = ColumnCount2;
                    fileConfiguration.ColumnHeaders = "Access_ID,Page_ID";
                    fileConfiguration.IsQuoteEncapsulated = false;
                    break;
                case BMRelationalFileTypes.SearchBehavior:
                    fileConfiguration.ColumnCount = ColumnCount2;
                    fileConfiguration.ColumnHeaders = "Access_ID,Search_Term";
                    fileConfiguration.IsQuoteEncapsulated = true;
                    break;
                case BMRelationalFileTypes.TopicCode:
                    fileConfiguration.ColumnCount = ColumnCount6;
                    fileConfiguration.ColumnHeaders =
                        "FirstName,LastName,EmailAddress,ActivityDatetime,TopicCode,TopicCodeText";
                    fileConfiguration.IsQuoteEncapsulated = true;
                    break;
            }

            return fileConfiguration;
        }

        private static bool FileExistsCheck(string filePath)
        {
            var checkFiles = new[]
            {
                FileNameAccessCsv,
                FileNamePageBehaviorCsv,
                FileNameSearchBehaviorCsv,
                FileNameTaxonomyBehaviorCsv,
                FileNameTopicCodeCsv,
                FileNameUsersCsv,
            };
            var fileList = ClientMethodHelpers.GetFilesListWithExistenceCheck(filePath, checkFiles);
            return fileList.Any();
        }

        private string GetCsvFileName(BMRelationalFileTypes bmRelationalFileTypes)
        {
            switch (bmRelationalFileTypes)
            {
                case BMRelationalFileTypes.Access:
                    return FileNameAccessCsv;
                case BMRelationalFileTypes.Users:
                    return FileNameUsersCsv;
                case BMRelationalFileTypes.TaxonomyBehavior:
                    return FileNameTaxonomyBehaviorCsv;
                case BMRelationalFileTypes.PageBehavior:
                    return FileNamePageBehaviorCsv;
                case BMRelationalFileTypes.SearchBehavior:
                    return FileNameSearchBehaviorCsv;
                case BMRelationalFileTypes.TopicCode:
                    return FileNameTopicCodeCsv;
                default:
                    return string.Empty;
            }
        }

        private void CreateCsvFilesForMapping(string filePath, Client kmClient)
        {
            var counter = 0;
            var cmData = new UasBusinessLogic.ClientMethods();
            var fileFunctions = new FileFunctions();
            var pubFile = $"{filePath}\\{BmRelDataCsvFileName}";
            var pubCount = cmData.BriefMedia_Relational_Get_Count(kmClient);

            if (pubCount > BatchSize10000)
            {
                //will have a RowID column                    
                var dtPub = cmData.BriefMedia_Select_Data_Paging(kmClient, counter, BatchSize10000);
                dtPub.Columns.Add("RowNum");
                dtPub.AcceptChanges();
                var rowId = 1;
                foreach (DataRow row in dtPub.Rows)
                {
                    row["RowRowNumID"] = rowId.ToString();
                    rowId++;
                }
                dtPub.AcceptChanges();
                fileFunctions.CreateCSVFromDataTable(dtPub, pubFile, true);
                dtPub.Dispose();
                dtPub = null;
            }
            else
            {
                var processedCount = 0;
                counter = 1;
                while (processedCount < pubCount)
                {
                    var dtPub = cmData.BriefMedia_Select_Data(kmClient);
                    fileFunctions.CreateCSVFromDataTable(dtPub, pubFile, false);
                    processedCount += dtPub.Rows.Count;
                    dtPub.Dispose();
                    dtPub = null;
                    counter++;
                }
            }
        }

        private void InsertUpdateRelationalData(BMRelationalFileTypes bmRelationalFileTypes, Client kmClient,
            DataTable dataTable)
        {
            var cmData = new UasBusinessLogic.ClientMethods();
            switch (bmRelationalFileTypes)
            {
                case BMRelationalFileTypes.Access:
                    Console.WriteLine($"Started: BriefMedia_Relational_BulkInsert {DateTime.Now.ToString()}");
                    
                    cmData.BriefMedia_Relational_Insert_Access(kmClient, dataTable);

                    Console.WriteLine($"Finished: BriefMedia_Relational_BulkInsert {DateTime.Now.ToString()}");
                    break;
                case BMRelationalFileTypes.Users:
                    Console.WriteLine($"Started: BriefMedia_Relational_Update_Users {DateTime.Now.ToString()}");

                    cmData.BriefMedia_Relational_Update_Users(kmClient, dataTable);

                    Console.WriteLine($"Finished: BriefMedia_Relational_Update_Users {DateTime.Now.ToString()}");
                    break;
                case BMRelationalFileTypes.TaxonomyBehavior:
                    Console.WriteLine($"Started: BriefMedia_Relational_Update_TaxBehavior {DateTime.Now.ToString()}");

                    cmData.BriefMedia_Relational_Update_TaxBehavior(kmClient, dataTable);

                    Console.WriteLine($"Finished: BriefMedia_Relational_Update_TaxBehavior {DateTime.Now.ToString()}");
                    break;
                case BMRelationalFileTypes.PageBehavior:
                    Console.WriteLine($"Started: BriefMedia_Relational_Update_PageBehavior {DateTime.Now.ToString()}");

                    cmData.BriefMedia_Relational_Update_PageBehavior(kmClient, dataTable);

                    Console.WriteLine($"Finished: BriefMedia_Relational_Update_PageBehavior {DateTime.Now.ToString()}");
                    break;
                case BMRelationalFileTypes.SearchBehavior:
                    Console.WriteLine(
                        $"Started: BriefMedia_Relational_Update_SearchBehavior {DateTime.Now.ToString()}");

                    cmData.BriefMedia_Relational_Update_SearchBehavior(kmClient, dataTable);

                    Console.WriteLine(
                        $"Finished: BriefMedia_Relational_Update_SearchBehavior {DateTime.Now.ToString()}");
                    break;
                case BMRelationalFileTypes.TopicCode:
                    Console.WriteLine($"Started: BriefMedia_Relational_Update_TopicCode {DateTime.Now.ToString()}");

                    cmData.BriefMedia_Relational_Update_TopicCode(kmClient, dataTable);

                    Console.WriteLine($"Finished: BriefMedia_Relational_Update_TopicCode {DateTime.Now.ToString()}");
                    break;
            }

            dataTable.Dispose();
            dataTable = null;
        }

        private void SetDataTablePrimaryKey(BMRelationalFileTypes bmRelationalFileTypes, ref DataTable dataTable)
        {
            var columnName = string.Empty;
            switch (bmRelationalFileTypes)
            {
                case BMRelationalFileTypes.Access:
                    columnName = "AccessID";
                    break;
                case BMRelationalFileTypes.Users:
                    columnName = "Drupal_User_id";
                    break;
                case BMRelationalFileTypes.PageBehavior:
                    columnName = "Access_ID";
                    break;
                case BMRelationalFileTypes.SearchBehavior:
                    columnName = "Access_ID";
                    break;
            }

            if (!string.IsNullOrWhiteSpace(columnName))
            {
                var accessKeys = new DataColumn[1];
                accessKeys[0] = dataTable.Columns[columnName];
                dataTable.PrimaryKey = accessKeys;
                dataTable.AcceptChanges();
            }
        }
    }
}
