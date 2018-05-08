using Core.ADMS.Events;
using FrameworkUAS.Entity;
using Core_AMS.Utilities;
using System.Data;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using KM.Common.Import;

namespace ADMS.ClientMethods
{
    public class Watt : ClientSpecialCommon
    {
        private const string TopCompanyDim = "TOPCOMPANY";
        private const string PubCodeMatchFieldName = "Pubcode";
        private const string CompanyMatchFieldName = "COMPANY";
        private const string MarketField            = "Market";
        private const string CompanyIsInUADField = "Companies in UAD";
        private const string TopCompanyCodeField = "TOP_COMPANY_Code to be applied";
        private const string TopCompanyOldDim = "TOP_COMPANY";
        private const string FeedDim = "FEED";
        private const string PoultryDim = "POULTRY";
        private const string PoultryValue = "P";
        private const string FeedValue = "F";
        private const string PigDim = "PIG";
        private const string PigValue = "S";
        private const string PetDim = "PET";
        private const string PetValue = "T";
        private const string EggDim = "EGG";
        private const string EggValue = "E";
        private const string SicalphaDim = "SICALPHA";
        private const string SicalphaValue = "SICALPHA";
        private const string SickalphaMatchField = "BE_Selected_SIC_Code";
        private const string SicStandardField = "SIC";
        private const string MicroDim = "MICRO";
        private const string MicroMacroValue = "CodeSheetValue";
        private const string MacroDim = "MACRO";
        private const string TopCompanyGroupName = "Watt_TOP_COMPANY";
        private const string PubCodeTopCompanyGroupName = "Watt_PubCode_TOP_COMPANY";
        private const string CompanyFeedGroupName = "Watt_Company_Feed";
        private const string CompanyPoultryGroupName = "Watt_Company_POULTRY";
        private const string CompanyPetGroupName = "Watt_Company_PET";
        private const string CompanyPigGroupName = "Watt_Company_PIG";
        private const string CompanyEggGroupName = "Watt_Company_EGG";
        private const string SicSicalphaGroupName = "Watt_SIC_SICALPHA";
        private const string PubCodeMicroGroupName = "Watt_PubCode_MICRO";
        private const string PubCodeMacroGroupName = "Watt_PubCode_MACRO";

        public void TopCompanyAdHocImport(FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = TopCompanyGroupName,
                    CreatedDimension = TopCompanyDim,
                    DimensionOperator = EqualOperation,
                    DimensionValueField = CompanyIsInUADField,
                    MatchValueField = TopCompanyCodeField,
                    StandardField = StandardFieldCompany
                });
        }

        public List<FrameworkUAD.Entity.SubscriberTransformed> TopCompany(List<FrameworkUAD.Entity.SubscriberTransformed> data, Dictionary<int, string> clientPubCodes, int clientId)
        {
            List<FrameworkUAD_Lookup.Entity.Code> demoUpdates = new List<FrameworkUAD_Lookup.Entity.Code>();
            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            demoUpdates = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);
            FrameworkUAD_Lookup.Entity.Code demoUpdate = demoUpdates.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace.ToString()));

            //execute in Validator.ValidateData after TransformedDedupe - 
            //at this point the incoing ST list has had pubs validated so we know everything exists and matches
            FrameworkUAS.BusinessLogic.AdHocDimensionGroup adgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            List<AdHocDimensionGroup> adgList = adgWorker.Select(clientId, true);
            List<AdHocDimension> companyList = new List<AdHocDimension>();
            int adgCompanyId = 0;

            if (adgList.Any(x => x.AdHocDimensionGroupName.Equals(TopCompanyGroupName)) == true)
                adgCompanyId = adgList.First(x => x.AdHocDimensionGroupName.Equals(TopCompanyGroupName)).AdHocDimensionGroupId;

            if (adgCompanyId > 0)
                companyList = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).AdHocDimensions;

            foreach (FrameworkUAD.Entity.SubscriberTransformed st in data)
            {
                int pubId = clientPubCodes.Single(x => x.Value.ToLower().Equals(st.PubCode.ToLower())).Key;
                if (companyList.Count > 0 && !string.IsNullOrEmpty(st.Company))
                {
                    AdHocDimension ad = companyList.FirstOrDefault(x => x.DimensionValue.Equals(st.Company, StringComparison.CurrentCultureIgnoreCase));
                    if (ad != null)
                    {
                        if (ad.MatchValue.Contains(","))
                        {
                            HashSet<string> codes = new HashSet<string>();
                            string[] values = ad.MatchValue.Split(',').ToArray();
                            foreach (string s in values)
                            {
                                FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                                sdo.CreatedByUserID = st.CreatedByUserID;
                                sdo.DateCreated = DateTime.Now;
                                sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).CreatedDimension;
                                sdo.NotExists = false;
                                sdo.PubID = pubId;
                                sdo.SORecordIdentifier = st.SORecordIdentifier;
                                sdo.STRecordIdentifier = st.STRecordIdentifier;
                                sdo.Value = s.Trim();
                                sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                                sdo.IsAdhoc = true;

                                st.DemographicTransformedList.Add(sdo);
                            }
                        }
                        else
                        {
                            FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                            sdo.CreatedByUserID = st.CreatedByUserID;
                            sdo.DateCreated = DateTime.Now;
                            sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).CreatedDimension;
                            sdo.NotExists = false;
                            sdo.PubID = pubId;
                            sdo.SORecordIdentifier = st.SORecordIdentifier;
                            sdo.STRecordIdentifier = st.STRecordIdentifier;
                            sdo.Value = ad.MatchValue;
                            sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                            sdo.IsAdhoc = true;

                            st.DemographicTransformedList.Add(sdo);
                        }
                    }
                }
            }

            return data;
        }

        // Old logic
        //KM_WATT_LOOKUP_Market_Pubcode.xlsx
        //KM_WATT_LOOKUP_TOP100_Feed Top Cos 140218_UNIQUE.xlsx
        //KM_WATT_LOOKUP_TOP100_Poultry Top Cos 140219_UNIQUE.xlsx
        //KM_WATT_LOOKUP_TOP100_unique_Pet_Top_Companies_2.xlsx

        //Watt Top Company
        //1.       If pubcode matches pubcode column in KM_WATT_LOOKUP_Market_Pubcode.xlsx, assign value in Market column to temp field Market.
        //2.       If market = 'FEED', then fuzzy match on company column from KM_WATT_LOOKUP_TOP100_Feed Top Cos 140218_UNIQUE.xlsx.  If there is a match, assign ‘F’ to TOP_COMPANY.
        //3.       If market = 'POULTRY', then fuzzy match on company column from KM_WATT_LOOKUP_TOP100_Poultry Top Cos 140219_UNIQUE.xlsx.  If there is a match, assign ‘P’ to TOP_COMPANY.
        //4.       If market = 'PET', then fuzzy match on company column from KM_WATT_LOOKUP_TOP100_unique_Pet_Top_Companies_2.xlsx.  If there is a match, assign ‘T’ to TOP_COMPANY.
        //5.       If market = 'PIG', then fuzzy match on company column from KM_WATT_LOOKUP_TOP100_Pig_Topcompany.xlsx.  If there is a match, assign ‘S’ to TOP_COMPANY.
        //6.       Note: a pubcode can belong to more than one Market.  As a result a record can have comma separated values such as F,P,T in TOP_COMPANY.
        public void TopCompanyAdHocImportOld(FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = PubCodeTopCompanyGroupName,
                    CreatedDimension = TopCompanyOldDim,
                    DimensionOperator = EqualOperation,
                    DimensionValueField = MarketField,
                    MatchValueField = PubCodeMatchFieldName,
                    StandardField = PubCodeFieldName
                });
        }
        public void FeedAdHocImport(FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = CompanyFeedGroupName,
                    CreatedDimension = FeedDim,
                    DimensionOperator = ContainsOperation,
                    DimensionValue = FeedValue,
                    MatchValueField = CompanyMatchFieldName,
                    StandardField = StandardFieldCompany
                });
        }
        public void PoultryAdHocImport(FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = CompanyPoultryGroupName,
                    CreatedDimension = PoultryDim,
                    DimensionOperator = ContainsOperation,
                    DimensionValue = PoultryValue,
                    MatchValueField = CompanyMatchFieldName,
                    StandardField = StandardFieldCompany
                });
        }
        public void PetAdHocImport(FileMoved eventMessage)
        {
            AdHockDimensionsImport(eventMessage, CompanyPetGroupName, PetDim, PetValue, StandardFieldCompany);
        }

        public void PigAdHocImport(FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = CompanyPigGroupName,
                    CreatedDimension = PigDim,
                    DimensionOperator = ContainsOperation,
                    DimensionValue = PigValue,
                    MatchValueField = CompanyMatchFieldName,
                    StandardField = StandardFieldCompany
                });
        }
        public void EggAdHocImport(FileMoved eventMessage)
        {
            AdHockDimensionsImport(eventMessage, CompanyEggGroupName, EggDim, EggValue, StandardFieldCompany);
        }

        public void CreateWattRelationalFiles(KMPlatform.Entity.Client client, FileInfo zipFile, SourceFile cSpecialFile)
        {
            var fileWorker = new FileWorker();
            var fileFunctions = new FileFunctions();
            var destinationPath = $"{Core.ADMS.BaseDirs.getClientArchiveDir()}\\{client.FtpFolder}\\ProcessCMS\\{DateTime.Now:MMddyyyy}";
            fileFunctions.ExtractZipFile(zipFile, destinationPath);
            var filePath = $"{destinationPath}\\WATT_Relational";
            var cmData = new FrameworkUAS.BusinessLogic.ClientMethods();
            cmData.WATT_CreateTempCMSTables();

            var checkFiles = new[]
            {
                "KM_WATT_LOOKUP_ECN_GROUPID_PUBCODE.xlsx",
                "KM_WATT_LOOKUP_MAC_MIC.xlsx",
                "KM_WATT_LOOKUP_Market_Pubcode.xlsx",
                "KM_WATT_LOOKUP_unique_SICs_for_WATT.xlsx"
            };
            var fileList = ClientMethodHelpers.GetFilesListWithExistenceCheck(filePath, checkFiles);
            if (!fileList.Any())
            {
                return;
            }

            #region Process Zip            
            try
            {
                //FrameworkUAD.Object.ImportVessel iv = null;

                #region KM_WATT_LOOKUP_ECN_GROUPID_PUBCODE.xlsx Data
                //1) Read in KM_WATT_LOOKUP_ECN_GROUPID_PUBCODE.xlsx
                //a) gives us GROUPID,PUBCODE
                FileConfiguration fcGroupID = new FileConfiguration();
                fcGroupID.ColumnCount = 2;
                fcGroupID.ColumnHeaders = "GROUPID,PUBCODE";
                //fcGroupID.FileColumnDelimiter = "";
                fcGroupID.FileExtension = ".xlsx";
                fcGroupID.FileFolder = filePath;
                //fcGroupID.IsQuoteEncapsulated = true;
                Console.WriteLine("Getting KM_WATT_LOOKUP_ECN_GROUPID_PUBCODE " + DateTime.Now.ToString());
                DataTable dtGroupID = fileWorker.GetData(fileList.Single(x => x.Name.Equals("KM_WATT_LOOKUP_ECN_GROUPID_PUBCODE.xlsx", StringComparison.CurrentCultureIgnoreCase)), fcGroupID);

                Console.WriteLine("Started: WATT_Relational_Insert_ECN_GROUPID_PUBCODE " + DateTime.Now.ToString());
                Console.WriteLine("Skip: WATT_Relational_Insert_ECN_GROUPID_PUBCODE " + DateTime.Now.ToString());
                //cmData.WATT_Relational_Process_ECN_GROUPID_PUBCODE(dtGroupID);
                DataTable ecnData = cmData.WATT_Get_ECN_Data(client.FtpFolder);
                Console.WriteLine("Finished: WATT_Relational_Insert_ECN_GROUPID_PUBCODE " + DateTime.Now.ToString());
                DataColumn[] accessKeys = new DataColumn[1];
                accessKeys[0] = dtGroupID.Columns["GROUPID"];
                dtGroupID.PrimaryKey = accessKeys;
                dtGroupID.AcceptChanges();

                int totalWorkRows = ecnData.Rows.Count;
                int workProcessed = 0;
                int batch = 2500;
                DataTable workTable = ecnData.Clone();
                workTable.Columns.Add(PubCodeFieldName);
                while (workProcessed < totalWorkRows)
                {
                    workTable.Clear();
                    Dictionary<int, DataRow> newRows = new Dictionary<int, DataRow>();
                    Dictionary<int, DataRow> deleteRows = new Dictionary<int, DataRow>();
                    int rowIndex = 0;

                    int batchProcessing = workProcessed + batch;
                    if (batchProcessing > totalWorkRows)
                        batchProcessing = totalWorkRows;

                    ConsoleMessage("New Batch: " + batchProcessing.ToString(), "CreateWattRelationalFiles", false);
                    ConsoleMessage("Processed: " + workProcessed.ToString(), "CreateWattRelationalFiles", false);
                    for (int i = workProcessed; i < batchProcessing; i++)
                    {
                        DataRow dr = ecnData.Rows[workProcessed];
                        //List<string> pubCodes = new List<string>();

                        DataRow match = dtGroupID.Rows.Find(dr["GROUPID"].ToString());

                        if (match != null)
                        {
                            AddRow(match["PUBCODE"].ToString(), newRows.Count + 1, newRows, workTable, dr);
                        }
                        else
                        {
                            AddRow("", newRows.Count + 1, newRows, workTable, dr);
                        }

                        deleteRows.Add(rowIndex, dr);
                        rowIndex++;
                        workProcessed++;
                    }
                    foreach (KeyValuePair<int, DataRow> kvp in newRows)
                    {
                        workTable.Rows.Add(kvp.Value);
                    }
                    workTable.AcceptChanges();

                    //for testing
                    //Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
                    fileFunctions.CreateCSVFromDataTable(workTable, "C:\\ADMS\\Client Archive\\Watt\\WATT_RelData.csv", false);
                    GC.Collect();
                }
                dtGroupID.Dispose();
                dtGroupID = null;
                #endregion

                #region KM_WATT_LOOKUP_MAC_MIC.xlsx Data
                //1) Read in KM_WATT_LOOKUP_MAC_MIC.xlsx
                //a) gives us ProductID,GroupID,ProductCode,ProductName,FOXColumnName,CodeSheetValue,CodeSheetDesc,MasterGroup,MasterGroupID,MasterCodeSheetValue,MasterCodeSheetDesc
                FileConfiguration fcMacMic = new FileConfiguration();
                fcMacMic.ColumnCount = 11;
                fcMacMic.ColumnHeaders = "ProductID,GroupID,ProductCode,ProductName,FOXColumnName,CodeSheetValue,CodeSheetDesc,MasterGroup,MasterGroupID,MasterCodeSheetValue,MasterCodeSheetDesc";
                fcMacMic.FileExtension = ".xlsx";
                fcMacMic.FileFolder = filePath;

                Console.WriteLine("Getting KM_WATT_LOOKUP_MAC_MIC.xlsx " + DateTime.Now.ToString());
                DataTable dtMacMic = fileWorker.GetData(fileList.Single(x => x.Name.Equals("KM_WATT_LOOKUP_MAC_MIC.xlsx", StringComparison.CurrentCultureIgnoreCase)), fcMacMic);

                Console.WriteLine("Started: WATT_Relational_Insert_MacMic " + DateTime.Now.ToString());
                cmData.WATT_Relational_Process_MacMic(dtMacMic);

                //Get the Mic and Mac table data
                FrameworkUAS.BusinessLogic.SourceFile blsf = new FrameworkUAS.BusinessLogic.SourceFile();
                SourceFile sf = blsf.Select(client.ClientID, "KM_WATT_LOOKUP_MAC_MIC");
                MicAdHocImportClear(client, sf);

                DataTable dtMic = cmData.WATT_Get_Mic_Data();
                this.MicAdHocImport(client, sf, dtMic);
                DataTable dtMac = cmData.WATT_Get_Mac_Data();
                this.MacAdHocImport(client, sf, dtMac);

                Console.WriteLine("Finished: WATT_Relational_Insert_MacMic " + DateTime.Now.ToString());
                dtMacMic.Dispose();
                dtMacMic = null;
                #endregion

                #region KM_WATT_LOOKUP_unique_SICs_for_WATT.xlsx Data
                //1) Read in KM_WATT_LOOKUP_unique_SICs_for_WATT.xlsx
                //a) gives us BE_Selected_SIC_Description,BE_Selected_SIC_Code,SICALPHA
                FileConfiguration fcSics = new FileConfiguration();
                fcSics.ColumnCount = 3;
                fcSics.ColumnHeaders = "BE_Selected_SIC_Description,BE_Selected_SIC_Code,SICALPHA";
                fcSics.FileExtension = ".xlsx";
                fcSics.FileFolder = filePath;

                Console.WriteLine("Getting KM_WATT_LOOKUP_unique_SICs_for_WATT " + DateTime.Now.ToString());
                DataTable dtSics = fileWorker.GetData(fileList.Single(x => x.Name.Equals("KM_WATT_LOOKUP_unique_SICs_for_WATT.xlsx", StringComparison.CurrentCultureIgnoreCase)), fcSics);

                Console.WriteLine("Started: WATT_Relational_Insert_Sics " + DateTime.Now.ToString());

                FrameworkUAS.BusinessLogic.SourceFile blsf2 = new FrameworkUAS.BusinessLogic.SourceFile();
                SourceFile sf2 = blsf2.Select(client.ClientID, "KM_WATT_LOOKUP_unique_SICs_for_WATT");

                this.RelationalSicAdHocImport(client, sf2, dtSics);

                Console.WriteLine("Finished: WATT_Relational_Insert_Sics " + DateTime.Now.ToString());

                dtSics.Dispose();
                #endregion
            }
            catch (Exception ex)
            {
                //Core_AMS.ADMS.Logging logger = new Core_AMS.ADMS.Logging();
                //logger.LogIssue(ex);

                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                    app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".CreateWattRelationalFiles", app, string.Empty);
            }

            #endregion

            cmData.WATT_DropTempCMSTables();

            Console.WriteLine("Uploading to FTP " + DateTime.Now.ToString());
            FrameworkUAS.BusinessLogic.ClientFTP blcftp = new FrameworkUAS.BusinessLogic.ClientFTP();
            ClientFTP clientFTP = new ClientFTP();
            clientFTP = blcftp.SelectClient(client.ClientID).First();
            Core_AMS.Utilities.FtpFunctions ftp = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
            ftp.Upload(clientFTP.Folder + "\\AutoGen_WATT_RelationalFile.csv", "C:\\ADMS\\Client Archive\\Watt\\WATT_RelData.csv");
            Console.WriteLine("Finished Uploading to FTP " + DateTime.Now.ToString());
        }

        public void AddRow(string pubCode, int pos, Dictionary<int, DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;
            newRow.SetField(PubCodeFieldName, pubCode);
            rows.Add(pos, newRow);
        }

        public void SicAdHocImport(FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = SicSicalphaGroupName,
                    CreatedDimension = SicalphaDim,
                    DimensionOperator = ContainsOperation,
                    DimensionValueField = SicalphaValue,
                    MatchValueField = SickalphaMatchField,
                    StandardField = SicStandardField
                });
        }

        public void RelationalSicAdHocImport(KMPlatform.Entity.Client client, SourceFile sf, DataTable dtSics)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    Client = client,
                    SourceFileId = sf.SourceFileID,
                    Dt = dtSics,
                    AdHocDimensionGroupName = SicSicalphaGroupName,
                    CreatedDimension = SicalphaDim,
                    DimensionOperator = ContainsOperation,
                    DimensionValueField = SicalphaValue,
                    MatchValueField = SickalphaMatchField,
                    StandardField = SicStandardField
                });
        }

        public void MicAdHocImportClear(KMPlatform.Entity.Client client, SourceFile sf)
        {
            FrameworkUAS.BusinessLogic.AdHocDimension ahdData = new FrameworkUAS.BusinessLogic.AdHocDimension();
            ahdData.Delete(sf.SourceFileID);
        }

        public void MicAdHocImport(KMPlatform.Entity.Client client, SourceFile sf, DataTable dtMic)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    Client = client,
                    SourceFileId = sf.SourceFileID,
                    Dt = dtMic,
                    AdHocDimensionGroupName = PubCodeMicroGroupName,
                    CreatedDimension = MicroDim,
                    DimensionOperator = ContainsOperation,
                    DimensionValueField = MicroMacroValue,
                    MatchValueField = PubCodeFieldName,
                    StandardField = PubCodeFieldName
                });
        }

        public void MacAdHocImport(KMPlatform.Entity.Client client, SourceFile sf, DataTable dtMac)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    Client = client,
                    SourceFileId = sf.SourceFileID,
                    Dt = dtMac,
                    AdHocDimensionGroupName = PubCodeMacroGroupName,
                    CreatedDimension = MacroDim,
                    DimensionOperator = ContainsOperation,
                    DimensionValueField = MicroMacroValue,
                    MatchValueField = PubCodeFieldName,
                    StandardField = PubCodeFieldName
                });
        }
    }
}
