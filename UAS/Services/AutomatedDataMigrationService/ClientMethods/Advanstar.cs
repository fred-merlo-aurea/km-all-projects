using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using Core.ADMS.Events;
using Core_AMS.Utilities;
using FrameworkUAS.Entity;
using KM.Common;
using KM.Common.Import;
using BusinessAdHocDimension = FrameworkUAS.BusinessLogic.AdHocDimension;
using FileFunctions = Core_AMS.Utilities.FileFunctions;

namespace ADMS.ClientMethods
{
    public class Advanstar : ClientSpecialCommon
    {
        private const string FieldCatCode = "CatCode";
        private const string FieldPersonId = "Person_ID";
        private const string FieldRegCode = "RegCode";
        private const string FieldNewTitle = "NEWTITLE";

        private const string AdvanstarPersonIdCatCode = "Advanstar_PersonID_CatCode";
        private const string AdvanstarMobileDemo33 = "Advanstar_Mobile_Demo33";
        private const string StandardFieldSequence = "Sequence";
        private const int OrderOfOperation2 = 2;
        private const string DimensioDemo33 = "DEMO33";
        private const string AdHocDimensionGroupAdvanstarPersonIdIndyCode = "Advanstar_PersonID_IndyCode";
        private const string DimensionIndyCode = "IndyCode";
        private const string DimensionOperatorEquals = "equals";
        private const string DimensionTitle = "Title";
        private const string BadPhoneImportPhoneGroupName = "Advanstar_BadPhone_Import_Phone";
        private const string PhonePermissionsDim = "PhonePermission";
        private const string AdvanstarNewTitleImport = "Advanstar_NewTitle_Import";
        private const string AdvanstarTitleCodeImport = "Advanstar_TitleCode_Import";
        private const string TitleCodeDimName = "Title_Code";
        private const string BadPhoneDimValue = "false";
        private const string DeviceValueFieldName = "Device_Value";
        private const string BadPhoneImportFaxGroupName = "Advanstar_BadPhone_Import_Fax";
        private const string FaxPermissionsDim = "FaxPermission";
        private const string BadPhoneImportMobileGroupName = "Advanstar_BadPhone_Import_Mobile";
        private const string PhoneFieldValue = "PHONE";
        private const string FaxFieldValue = "FAX";
        private const string MobileFieldValue = "MOBILE";
        private const string DeviceFieldName = "Device";
        private const string AdHocDimensionGroupPersonIdGegCode = "Advanstar_PersonID_RegCode";
        private const string AdHocDimensionGroupTitle = "Advanstar_Title_Title";
        private const string AdHocDimensionGroupTitleCode = "Advanstar_Title_TITLE_CODE";

        private FileInfo fi { get; set; }
        private FrameworkUAS.BusinessLogic.ClientMethods cm { get; set; }
        private Core_AMS.Utilities.FileWorker fw { get; set; }

        public void AdvanstarRelationalFiles(KMPlatform.Entity.Client client, FileMoved eventMessage)
        {
            #region Initialzing Variables & Data
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Welcome to Advanstar Relational files. Despair in its unholy presence.");
            fw = new FileWorker();
            Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
            SourceFile sf = new SourceFile();
            FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
            FrameworkUAS.BusinessLogic.AdHocDimension ahdData = new FrameworkUAS.BusinessLogic.AdHocDimension();
            string destinationPath = Core.ADMS.BaseDirs.getClientArchiveDir() + "\\" + client.FtpFolder + "\\ProcessCMS\\" + DateTime.Now.ToString("MMddyyyy");
            ff.ExtractZipFile(eventMessage.ImportFile, destinationPath);
            string filePath = destinationPath;
            cm = new FrameworkUAS.BusinessLogic.ClientMethods();
            cm.Advanstar_CreateTempTables();
            int totalRowCount = 0;
            int rowProcessedCount = 0;
            int rowBatch = 2500;
            int counter = 0;
            int pubCount = 0;
            //bool firstRun = true;
            string pubFile = "";
            DataTable dt = new DataTable();
            DataTable dtNew = new DataTable();
            FileConfiguration fc = new FileConfiguration();
            FrameworkUAD.Object.ImportVessel iv = new FrameworkUAD.Object.ImportVessel();
            DataTable workTable = new DataTable();
            DataTable ecnData = new DataTable();
            string line = string.Empty;

            #endregion

            #region File exist check
            //Zip File Name = Advanstar_Relational.zip
            //Included files:
            //1.	KM_ADVANSTAR_LOOKUP_BAD_PHONE_FAX_MOBILE.xlsx
            //2.	KM_ADVANSTAR_LOOKUP_GROUPID_PUBCODE_TYPE.xlsx
            //3.	KM_ADVANSTAR_LOOKUP_NEWTITLE.xlsx
            //4.	KM_ADVANSTAR_LOOKUP_REGCODE.xlsx
            //5.	KM_ADVANSTAR_LOOKUP_TITLECDE.xlsx
            //6.	KM_ADVANSTAR_LOOKUP_WEB_AND_WP_PUBCODES_FROM_PRDCDE.xlsx
            //7.    PERSON_ID_SOURCECODE.DBF
            //8.    CBI_REGCODE.DBF
            //9.    CBI_SOURCECODE.DBF
            //10.   CBI_SRCODE_PRICODE.DBF

            //make sure all files exist
            bool filesExist = true;
            string checkFile = string.Empty;
            List<FileInfo> fileList = new List<FileInfo>();
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (di.Exists == false)
                filesExist = false;
            else
            {
                fileList = di.GetFiles().ToList();
                while (filesExist == true)
                {
                    checkFile = "KM_ADVANSTAR_LOOKUP_BAD_PHONE_FAX_MOBILE.xlsx";
                    filesExist = fileList.Exists(x => x.Name.Equals(checkFile, StringComparison.CurrentCultureIgnoreCase));
                    checkFile = "KM_ADVANSTAR_LOOKUP_GROUPID_PUBCODE_TYPE.xlsx";
                    filesExist = fileList.Exists(x => x.Name.Equals(checkFile, StringComparison.CurrentCultureIgnoreCase));
                    checkFile = "KM_ADVANSTAR_LOOKUP_NEWTITLE.xlsx";
                    filesExist = fileList.Exists(x => x.Name.Equals(checkFile, StringComparison.CurrentCultureIgnoreCase));
                    checkFile = "KM_ADVANSTAR_LOOKUP_REGCODE.xlsx";
                    filesExist = fileList.Exists(x => x.Name.Equals(checkFile, StringComparison.CurrentCultureIgnoreCase));
                    checkFile = "KM_ADVANSTAR_LOOKUP_TITLECDE.xlsx";
                    filesExist = fileList.Exists(x => x.Name.Equals(checkFile, StringComparison.CurrentCultureIgnoreCase));
                    checkFile = "KM_ADVANSTAR_LOOKUP_WEB_AND_WP_PUBCODES_FROM_PRDCDE.xlsx";
                    filesExist = fileList.Exists(x => x.Name.Equals(checkFile, StringComparison.CurrentCultureIgnoreCase));
                    checkFile = "PERSON_ID_SOURCECODE.DBF";
                    filesExist = fileList.Exists(x => x.Name.Equals(checkFile, StringComparison.CurrentCultureIgnoreCase));
                    checkFile = "CBI_REGCODE.DBF";
                    filesExist = fileList.Exists(x => x.Name.Equals(checkFile, StringComparison.CurrentCultureIgnoreCase));
                    checkFile = "CBI_SOURCECODE.DBF";
                    filesExist = fileList.Exists(x => x.Name.Equals(checkFile, StringComparison.CurrentCultureIgnoreCase));
                    checkFile = "CBI_SRCODE_PRICODE.DBF";
                    filesExist = fileList.Exists(x => x.Name.Equals(checkFile, StringComparison.CurrentCultureIgnoreCase));
                    break;
                }
            }
            #endregion

            if (filesExist == true)
            {
                try
                {
                    #region PERSON_ID_SOURCECODE
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Starting PERSON_ID_SOURCECODE");
                    fc = new FileConfiguration();
                    fc.FileExtension = ".dbf";
                    fc.FileColumnDelimiter = "tab";
                    fc.ColumnHeaders = "person_id, sourcecode";
                    fi = new FileInfo(filePath + "\\PERSON_ID_SOURCECODE.DBF");
                    dt = fw.GetData(fi, fc);
                    dtNew = new DataTable();
                    Console.WriteLine("Begin inserting data...");
                    cm.Advanstar_Insert_PersonID(dt);
                    cm.Advanstar_Insert_PersonID_Final();
                    Console.WriteLine("Data insert complete.");
                    rowBatch = 10000;
                    pubFile = filePath + "\\Advanstar_PersonIDSourceCode.csv";
                    if (File.Exists(pubFile))
                        File.Delete(pubFile);
                    pubCount = cm.Advanstar_Get_Count("tempAdvanstarPersonIDFinal");
                    totalRowCount = pubCount;
                    Console.WriteLine("File creation started.");
                    while (rowProcessedCount < totalRowCount)
                    {
                        counter++;
                        if (pubCount > rowBatch)
                        {
                            DataTable dtPub = cm.Advanstar_Select_Data_Paging(counter, rowBatch);
                            ff.CreateCSVFromDataTable(dtPub, pubFile, false);
                            rowProcessedCount += dtPub.Rows.Count;
                            pubCount -= dtPub.Rows.Count;
                            string backup = new string('\b', line.Length);
                            Console.Write(backup);
                            line = string.Format("Processed Count: " + rowProcessedCount + "/" + totalRowCount + " (" + (int)Math.Round((double)(rowProcessedCount * 100) / totalRowCount) + "%)");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(line);
                            dtPub.Dispose();
                            dtPub = null;
                        }
                        else
                        {
                            DataTable dtPub = cm.Advanstar_Select_Data_Paging(counter, rowBatch);
                            ff.CreateCSVFromDataTable(dtPub, pubFile, false);
                            rowProcessedCount += dtPub.Rows.Count;
                            string backup = new string('\b', line.Length);
                            Console.Write(backup);
                            line = string.Format("Processed Count: " + rowProcessedCount + "/" + totalRowCount + " (" + (int)Math.Round((double)(rowProcessedCount * 100) / totalRowCount) + "%)");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(line);
                            dtPub.Dispose();
                            dtPub = null;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine("File creation completed.");
                    #region Sending Person_ID data to AdHoc table
                    Console.WriteLine("Sending Person_ID data to AdHocDimension table.");
                    fi = null;
                    fi = new FileInfo(filePath + "\\Advanstar_PersonIDSourceCode.csv");
                    fc = new FileConfiguration
                    {
                        FileExtension = ".csv",
                        FileColumnDelimiter = "comma"
                    };
                    sf = sfData.Select(client.ClientID, "Advanstar_PersonIDSourceCode");
                    if (sf == null)
                        sf = createSourceFile("Advanstar_PersonIDSourceCode", sf, sfData, ".csv");
                    ahdData.Delete(sf.SourceFileID);
                    dt = null;
                    dt = fw.GetData(fi, fc);

                    var fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
                    {
                        Client = client,
                        EventMessage = eventMessage,
                        SourceFileId = sf.SourceFileID,
                        Dt = dt,
                        AhdData = ahdData,
                        StandardField = StandardFieldSequence,
                        AdHocDimensionGroupName = AdHocDimensionGroupAdvanstarPersonIdIndyCode,
                        CreatedDimension = DimensionIndyCode,
                        DimensionOperator = DimensionOperatorEquals,
                        MatchValueField = FieldPersonId,
                        DimensionValueField = DimensionIndyCode
                    };
                    var agWorker = ClientMethodHelpers.FillAgGroupAndTable(fillAgGroupAndTableArgs);


                    var createAdhocGroupCodeArgs = new CreateAdhocGroupCodeArgs(
                        client.ClientID, sf.SourceFileID, eventMessage.SourceFile.SourceFileID, 
                        agWorker,
                        AdvanstarPersonIdCatCode,
                        OrderOfOperation2,
                        StandardFieldSequence, 
                        FieldCatCode);
                    var adgCatCode = ClientMethodHelpers.CreateAdhocGroupCode(createAdhocGroupCodeArgs);

                    ClientMethodHelpers.SaveAdHocDimensions(dt, adgCatCode, ahdData, FieldCatCode, FieldPersonId);

                    #endregion
                    totalRowCount = 0; rowProcessedCount = 0; rowBatch = 0; counter = 0; pubCount = 0; line = string.Empty;
                    dt.Dispose();
                    dt = null;
                    Console.WriteLine("PERSON_ID_SOURCECODE Complete");
                    #endregion

                    #region KM_ADVANSTAR_LOOKUP_REGCODE

                    fi = null;
                    fi = new FileInfo(filePath + "\\KM_ADVANSTAR_LOOKUP_REGCODE.xlsx");
                    dt = fw.GetData(fi);
                    cm.Advanstar_Insert_RegCodeCompare(dt);
                    dt.Dispose();
                    dt = null;
                    #endregion

                    #region CBI_REGCODE
                    Console.WriteLine("Starting CBI_REGCODE");
                    //firstRun = true;
                    fc = new FileConfiguration
                    {
                        FileExtension = ".dbf",
                        FileColumnDelimiter = "tab"
                    };
                    fi = null;
                    fi = new FileInfo(filePath + "\\CBI_REGCODE.DBF");
                    dt = fw.GetData(fi, fc);
                    dtNew = null;
                    dtNew = new DataTable();
                    Console.WriteLine("Inserting RegCode data...");
                    cm.Advanstar_Insert_RegCode(dt);
                    cm.Advanstar_Insert_RegCode_Final();
                    Console.WriteLine("Insert complete.");
                    rowBatch = 10000;
                    pubFile = filePath + "\\Advanstar_RegCode_Relational.csv";
                    if (File.Exists(pubFile))
                        File.Delete(pubFile);
                    pubCount = cm.Advanstar_Get_Count("tempAdvanstarRegCodeFinal");
                    totalRowCount = pubCount;
                    Console.WriteLine("File creation started.");
                    while (rowProcessedCount < totalRowCount)
                    {
                        counter++;
                        if (totalRowCount > rowBatch)
                        {
                            DataTable dtPub = cm.Advanstar_Select_Data_PagingRegCode(counter, rowBatch);
                            ff.CreateCSVFromDataTable(dtPub, pubFile, false);
                            rowProcessedCount += dtPub.Rows.Count;
                            pubCount -= dtPub.Rows.Count;
                            string backup = new string('\b', line.Length);
                            Console.Write(backup);
                            line = string.Format("Processed Count: " + rowProcessedCount + "/" + totalRowCount + " (" + (int)Math.Round((double)(rowProcessedCount * 100) / totalRowCount) + "%)");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(line);
                            dtPub.Dispose();
                            dtPub = null;
                        }
                        else
                        {
                            DataTable dtPub = cm.Advanstar_Select_Data_PagingRegCode(counter, rowBatch);
                            ff.CreateCSVFromDataTable(dtPub, pubFile, false);
                            rowProcessedCount += dtPub.Rows.Count;
                            string backup = new string('\b', line.Length);
                            Console.Write(backup);
                            line = string.Format("Processed Count: " + rowProcessedCount + "/" + totalRowCount + " (" + (int)Math.Round((double)(rowProcessedCount * 100) / totalRowCount) + "%)");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(line);
                            dtPub.Dispose();
                            dtPub = null;
                        }
                    }

                    // Send CBI_RegCode to AdHoc Table
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("File creation completed.");
                    Console.WriteLine("Sending CBI_RegCode data to AdHocDimension table.");
                    fi = null;
                    fi = new FileInfo(filePath + "\\Advanstar_RegCode_Relational.csv");
                    fc = new FileConfiguration
                    {
                        FileExtension = ".csv",
                        FileColumnDelimiter = "comma"
                    };
                    sf = sfData.Select(client.ClientID, "Advanstar_RegCode_Relational");
                    if (sf == null)
                        sf = createSourceFile("Advanstar_RegCode_Relational", sf, sfData, ".csv");
                    ahdData.Delete(sf.SourceFileID);                    
                    dt = fw.GetData(fi, fc);

                    var fillAgGroupAndTableArgsRegCode = new FillAgGroupAndTableArgs()
                    {
                        Client = client,
                        EventMessage = eventMessage,
                        SourceFileId = sf.SourceFileID,
                        AdHocDimensionGroupName = AdHocDimensionGroupPersonIdGegCode,
                        AhdData = ahdData,
                        CreatedDimension = FieldRegCode,
                        StandardField = StandardFieldSequence,
                        DimensionValueField = FieldRegCode,
                        MatchValueField = FieldPersonId,
                        Dt = dt,
                        DimensionOperator = DimensionOperatorEquals,
                        UpdateUAD = true
                    };
                    ClientMethodHelpers.FillAgGroupAndTable(fillAgGroupAndTableArgsRegCode);

                    totalRowCount = 0; rowProcessedCount = 0; rowBatch = 0; counter = 0; pubCount = 0; line = string.Empty;
                    dt.Dispose();
                    dt = null;
                    Console.WriteLine();
                    Console.WriteLine("CBI_REGCODE Complete");
                    #endregion

                    #region KM_ADVANSTAR_LOOKUP_BAD_PHONE_FAX_MOBILE
                    fi = null;
                    fi = new FileInfo(filePath + "\\KM_ADVANSTAR_LOOKUP_BAD_PHONE_FAX_MOBILE.xlsx");
                    sf = sfData.Select(client.ClientID, "KM_ADVANSTAR_LOOKUP_BAD_PHONE_FAX_MOBILE");
                    if (sf == null)
                        sf = createSourceFile("KM_ADVANSTAR_LOOKUP_BAD_PHONE_FAX_MOBILE", sf, sfData, ".xlsx");
                    ahdData.Delete(sf.SourceFileID);
                    dt = fw.GetData(fi);

                    var adgDemo33 = ClientMethodHelpers.CreateAdhocGroupCode(
                        new CreateAdhocGroupCodeArgs(
                            client.ClientID, 
                            sf.SourceFileID, 
                            eventMessage.SourceFile.SourceFileID, 
                            agWorker,
                            AdvanstarMobileDemo33, 
                            4,
                            MobileStandardField,
                            DimensioDemo33));

                    ClientMethodHelpers.SaveAdHocDimensions(dt, adgDemo33, ahdData, "DEMOVALUE", "DEVICE");

                    #endregion

                    #region KM_ADVANSTAR_LOOKUP_GROUPID_PUBCODE_TYPE
                    Console.WriteLine("Starting KM_ADVANSTAR_LOOKUP_GROUPID_PUBCODE_TYPE");
                    fi = new FileInfo(filePath + "\\KM_ADVANSTAR_LOOKUP_GROUPID_PUBCODE_TYPE.xlsx");
                    DataTable dtGroupID = fw.GetData(fi);
                    pubCount = cm.Advanstar_Get_ECN_Count("Advanstar");
                    totalRowCount = pubCount;
                    DataColumn[] accessKeys = new DataColumn[1];
                    accessKeys[0] = dtGroupID.Columns["GROUPID"];
                    dtGroupID.PrimaryKey = accessKeys;
                    dtGroupID.AcceptChanges();
                    rowBatch = 10000;
                    workTable = null;
                    workTable = ecnData.Clone();
                    workTable.Columns.Add("PubCode");
                    if (File.Exists(filePath + "\\Advanstar_GROUPID_LookUp.csv"))
                        File.Delete(filePath + "\\Advanstar_GROUPID_LookUp.csv");
                    Console.WriteLine("ECN data retrieval and file creation started.");
                    while (rowProcessedCount < totalRowCount)
                    {
                        counter++;
                        ecnData.Dispose();
                        ecnData = null;
                        ecnData = cm.Advanstar_Select_ECN_Paging(counter, rowBatch, "Advanstar");
                        workTable = ecnData.Clone();
                        workTable.Columns.Add("PubCode");
                        workTable.Clear();
                        Dictionary<int, DataRow> newRows = new Dictionary<int, DataRow>();
                        Dictionary<int, DataRow> deleteRows = new Dictionary<int, DataRow>();
                        int rowIndex = 0;

                        int batchProcessing = rowProcessedCount + rowBatch;
                        if (batchProcessing > totalRowCount)
                            rowBatch = ecnData.Rows.Count;

                        string backup = new string('\b', line.Length);
                        Console.Write(backup);
                        line = string.Format("Processed Count: " + rowProcessedCount + "/" + totalRowCount + " (" + (int)Math.Round((double)(rowProcessedCount * 100) / totalRowCount) + "%)");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(line);
                        for (int i = 0; i < rowBatch; i++)
                        {
                            DataRow dr = ecnData.Rows[i];
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
                            rowProcessedCount++;
                        }
                        foreach (KeyValuePair<int, DataRow> kvp in newRows)
                        {
                            workTable.Rows.Add(kvp.Value);
                        }
                        workTable.AcceptChanges();

                        //for testing
                        ff.CreateCSVFromDataTable(workTable, filePath + "\\Advanstar_GROUPID_LookUp.csv", false);
                        GC.Collect();
                    }
                    //Console.WriteLine();
                    dtGroupID.Dispose();
                    dtGroupID = null;
                    totalRowCount = 0; rowProcessedCount = 0; rowBatch = 0; counter = 0; pubCount = 0; line = string.Empty;
                    dt.Dispose();
                    dt = null;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("KM_ADVANSTAR_LOOKUP_GROUPID_PUBCODE_TYPE Completed.");
                    #endregion

                    fi = null;
                    fi = new FileInfo(filePath + "\\KM_ADVANSTAR_LOOKUP_NEWTITLE.xlsx");
                    sf = sfData.Select(client.ClientID, "KM_ADVANSTAR_LOOKUP_NEWTITLE");
                    if (sf == null)
                        sf = createSourceFile("KM_ADVANSTAR_LOOKUP_NEWTITLE", sf, sfData, ".xlsx");
                    ahdData.Delete(sf.SourceFileID);
                    dt = fw.GetData(fi);

                    var fillAgGroupAndTableArgsTitle = new FillAgGroupAndTableArgs()
                    {
                        Client = client,
                        EventMessage = eventMessage,
                        SourceFileId = sf.SourceFileID,
                        AdHocDimensionGroupName = AdHocDimensionGroupTitle,
                        AhdData = ahdData,
                        CreatedDimension = TitleFieldName,
                        StandardField = TitleFieldName,
                        DimensionValueField = FieldNewTitle,
                        MatchValueField = TitleFieldName,
                        Dt = dt,
                        DimensionOperator = EqualOperation,
                        UpdateUAD = true
                    };
                    ClientMethodHelpers.FillAgGroupAndTable(fillAgGroupAndTableArgsTitle);

                    fi = null;
                    fi = new FileInfo(filePath + "\\KM_ADVANSTAR_LOOKUP_TITLECDE.xlsx");
                    sf = sfData.Select(client.ClientID, "KM_ADVANSTAR_LOOKUP_TITLECDE");
                    if (sf == null)
                        sf = createSourceFile("KM_ADVANSTAR_LOOKUP_TITLECDE", sf, sfData, ".xlsx");
                    ahdData.Delete(sf.SourceFileID);
                    dt = fw.GetData(fi);
                    fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
                    {
                        Client = client,
                        EventMessage = eventMessage,
                        SourceFileId = sf.SourceFileID,
                        AdHocDimensionGroupName = AdHocDimensionGroupTitleCode,
                        AhdData = ahdData,
                        CreatedDimension = TitleCodeFieldName,
                        StandardField = TitleFieldName,
                        DimensionValueField = TitleCodeFieldName,
                        MatchValueField = TitleFieldName,
                        Dt = dt,
                        DimensionOperator = ContainsOperation,
                        UpdateUAD = true
                    };
                    ClientMethodHelpers.FillAgGroupAndTable(fillAgGroupAndTableArgs);

                    #region KM_ADVANSTAR_LOOKUP_WEB_AND_WP_PUBCODES_FROM_PRDCDE
                    Console.WriteLine("Starting KM_ADVANSTAR_LOOKUP_WEB_AND_WP_PUBCODES_FROM_PRDCDE");
                    fi = new FileInfo(filePath + "\\KM_ADVANSTAR_LOOKUP_WEB_AND_WP_PUBCODES_FROM_PRDCDE.xlsx");
                    dt = fw.GetData(fi);
                    DataTable prdcdes = cm.Advanstar_Select_PRDCDES();
                    totalRowCount = prdcdes.Rows.Count;
                    rowBatch = 2500;
                    workTable = null;
                    workTable = prdcdes.Clone();
                    accessKeys = new DataColumn[1];
                    accessKeys[0] = dt.Columns["PRDCDE"];
                    dt.PrimaryKey = accessKeys;
                    if (File.Exists(filePath + "\\Advanstar_PRDCDE_LookUp.csv"))
                        File.Delete(filePath + "\\Advanstar_PRDCDE_LookUp.csv");
                    while (rowProcessedCount < totalRowCount)
                    {
                        workTable.Clear();
                        Dictionary<int, DataRow> newRows = new Dictionary<int, DataRow>();
                        Dictionary<int, DataRow> deleteRows = new Dictionary<int, DataRow>();
                        int rowIndex = 0;

                        int batchProcessing = rowProcessedCount + rowBatch;
                        if (batchProcessing > totalRowCount)
                            batchProcessing = totalRowCount;
                        string backup = new string('\b', line.Length);
                        double percent = (int)Math.Round((double)(100 * batchProcessing) / totalRowCount);
                        Console.Write(backup);
                        line = string.Format("Processing: " + rowProcessedCount.ToString() + " to " + batchProcessing.ToString() + " " + DateTime.Now.ToString() + " " + percent.ToString() + "%");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(line);
                        for (int i = rowProcessedCount; i < batchProcessing; i++)
                        {
                            DataRow dr = prdcdes.Rows[rowProcessedCount];
                            //List<string> pubCodes = new List<string>();

                            DataRow match = dt.Rows.Find(dr["PRDCDE"].ToString());

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
                            rowProcessedCount++;
                        }
                        foreach (KeyValuePair<int, DataRow> kvp in newRows)
                        {
                            workTable.Rows.Add(kvp.Value);
                        }
                        workTable.AcceptChanges();

                        //for testing
                        ff.CreateCSVFromDataTable(workTable, filePath + "\\Advanstar_PRDCDE_LookUp.csv", false);
                        GC.Collect();
                    }
                    totalRowCount = 0; rowProcessedCount = 0; rowBatch = 0; counter = 0; pubCount = 0; line = string.Empty;
                    dt.Dispose();
                    dt = null;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("KM_ADVANSTAR_LOOKUP_WEB_AND_WP_PUBCODES_FROM_PRDCDE Completed.");
                    #endregion

                    cm.Advanstar_DropTempTables();

                    #region FTPUploading

                    Console.WriteLine("Uploading to FTP " + DateTime.Now.ToString());
                    FrameworkUAS.BusinessLogic.ClientFTP blcftp = new FrameworkUAS.BusinessLogic.ClientFTP();
                    ClientFTP clientFTP = new ClientFTP();
                    clientFTP = blcftp.SelectClient(client.ClientID).First();
                    Core_AMS.Utilities.FtpFunctions ftp = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
                    Console.WriteLine("Uploading Advanstar_PRDCDE_LookUp");
                    ftp.Upload(clientFTP.Folder + "\\Advanstar_Relational_PRDCDELookUp.csv", filePath + "\\Advanstar_PRDCDE_LookUp.csv");
                    Console.WriteLine("Uploading Advanstar_GROUPID_LookUp.csv");
                    ftp.Upload(clientFTP.Folder + "\\Advanstar_Relational_GROUPID.csv", filePath + "\\Advanstar_GROUPID_LookUp.csv");
                    Console.WriteLine("Finished Uploading to FTP " + DateTime.Now.ToString());

                    #endregion
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".AdvanstarRelationalFiles");
                }
            }
        }


        public void AddRow(string pubCode, int pos, Dictionary<int, DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;
            newRow.SetField("PubCode", pubCode);
            rows.Add(pos, newRow);
        }

        public SourceFile createSourceFile(string fileName, SourceFile sf, FrameworkUAS.BusinessLogic.SourceFile sfData, string extension)
        {
            sf = new SourceFile
            {
                ClientID = 2,
                FileName = fileName,
                Extension = extension,
                IsDQMReady = true,
                IsDeleted = false,
                IsIgnored = false,
                FileSnippetID = -1,
                DateCreated = DateTime.Now,
                DateUpdated = null,
                UpdatedByUserID = null,
                CreatedByUserID = 1,
                Delimiter = "",
                IsTextQualifier = false,
                IsSpecialFile = true,
                ServiceID = 2,
                ServiceFeatureID = 7,
            };
            sfData.Save(sf);

            return sf;
        }

        public void resetVariables(int total, int processed, int batchSize, int counter, int pubCount, DataTable dt, String line)
        {
            total = 0;
            processed = 0;
            batchSize = 2500;
            counter = 0;
            pubCount = 0;
            dt.Dispose();
            dt = null;
            line = string.Empty;
        }

        public void NewTitleImport(FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = AdvanstarNewTitleImport,
                    CreatedDimension = DimensionTitle,
                    DimensionOperator = EqualOperation,
                    DimensionValueField = FieldNewTitle,
                    MatchValueField = TitleFieldName,
                    StandardField = TitleFieldName
                });

            ArchiveFile(eventMessage);
        }

        public void TitleCodeImport(FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = AdvanstarTitleCodeImport,
                    CreatedDimension = TitleCodeDimName,
                    DimensionOperator = EqualOperation,
                    DimensionValueField = TitleCodeFieldName,
                    MatchValueField = TitleFieldName,
                    StandardField = DimensionTitle
                });

            ArchiveFile(eventMessage);
        }

        public void BadPhoneImport(FileMoved eventMessage)
        {
            var ahdData = new BusinessAdHocDimension();
            ahdData.Delete(eventMessage.SourceFile.SourceFileID);

            var fileWorker = new FileWorker();
            var dataTable = fileWorker.GetData(eventMessage.ImportFile);
            var list = new List<AdHocDimension>();

            var agWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            FillAgGroupAndTableArgs phoneGroupConfig, faxGroupConfig, mobileGroupConfig;
            CreateGroupConfigElements(eventMessage, out phoneGroupConfig, out faxGroupConfig, out mobileGroupConfig);

            var adgPhone = ClientMethodHelpers.CreateDimensionGroup(phoneGroupConfig, agWorker);
            var adgFax = ClientMethodHelpers.CreateDimensionGroup(faxGroupConfig, agWorker);
            var adgMobile = ClientMethodHelpers.CreateDimensionGroup(mobileGroupConfig, agWorker);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow[DeviceFieldName].ToString().Equals(PhoneFieldValue, StringComparison.CurrentCultureIgnoreCase))
                {
                    ClientMethodHelpers.CreateDimensionValue(phoneGroupConfig, adgPhone, list, dataRow);
                }
                else if (dataRow[DeviceFieldName].ToString().Equals(FaxFieldValue, StringComparison.CurrentCultureIgnoreCase))
                {
                    ClientMethodHelpers.CreateDimensionValue(faxGroupConfig, adgFax, list, dataRow);
                }
                else if (dataRow[DeviceFieldName].ToString().Equals(MobileFieldValue, StringComparison.CurrentCultureIgnoreCase))
                {
                    ClientMethodHelpers.CreateDimensionValue(mobileGroupConfig, adgMobile, list, dataRow);
                }
            }

            ahdData.SaveBulkSqlInsert(list);
            ArchiveFile(eventMessage);
        }

        private static void CreateGroupConfigElements(
            FileMoved eventMessage, 
            out FillAgGroupAndTableArgs phoneGroupConfig, 
            out FillAgGroupAndTableArgs faxGroupConfig, 
            out FillAgGroupAndTableArgs mobileGroupConfig)
        {
            Guard.NotNull(eventMessage, nameof(eventMessage));

            phoneGroupConfig = new FillAgGroupAndTableArgs()
            {
                Client = eventMessage.Client,
                EventMessage = eventMessage,
                AdHocDimensionGroupName = BadPhoneImportPhoneGroupName,
                StandardField = PhoneStandardField,
                CreatedDimension = PhonePermissionsDim,
                IsActive = false,
                DimensionValue = BadPhoneDimValue,
                MatchValueField = DeviceValueFieldName,
                DimensionOperator = EqualOperation
            };
            faxGroupConfig = new FillAgGroupAndTableArgs()
            {
                Client = eventMessage.Client,
                EventMessage = eventMessage,
                AdHocDimensionGroupName = BadPhoneImportFaxGroupName,
                StandardField = FaxStandardField,
                CreatedDimension = FaxPermissionsDim,
                IsActive = false,
                DimensionValue = BadPhoneDimValue,
                MatchValueField = DeviceValueFieldName,
                DimensionOperator = EqualOperation
            };
            mobileGroupConfig = new FillAgGroupAndTableArgs()
            {
                Client = eventMessage.Client,
                EventMessage = eventMessage,
                AdHocDimensionGroupName = BadPhoneImportMobileGroupName,
                StandardField = MobileStandardField,
                CreatedDimension = PhonePermissionsDim,
                IsActive = false,
                DimensionValue = BadPhoneDimValue,
                MatchValueField = DeviceValueFieldName,
                DimensionOperator = EqualOperation
            };
        }

        public FrameworkUAD.Object.ImportFile ApplyConditionalAdHocs(FrameworkUAD.Object.ImportFile dataIV)
        {
            FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = ahdgWorker.Select(dataIV.ClientId, true).OrderBy(y => y.OrderOfOperation).ToList();
            FrameworkUAS.BusinessLogic.AdHocDimension adWorker = new FrameworkUAS.BusinessLogic.AdHocDimension();
            FrameworkUAS.Entity.AdHocDimensionGroup newTitleADG = null;
            FrameworkUAS.Entity.AdHocDimensionGroup badPhoneADG = null;
            FrameworkUAS.Entity.AdHocDimensionGroup badFaxADG = null;
            FrameworkUAS.Entity.AdHocDimensionGroup badMobileADG = null;

            if (ahdGroups.Exists(x => x.AdHocDimensionGroupName.Equals(AdvanstarNewTitleImport)))
                newTitleADG = ahdGroups.Single(x => x.AdHocDimensionGroupName.Equals(AdvanstarNewTitleImport));
            if (ahdGroups.Exists(x => x.AdHocDimensionGroupName.Equals("Advanstar_BadPhone_Import_Phone")))
                badPhoneADG = ahdGroups.Single(x => x.AdHocDimensionGroupName.Equals("Advanstar_BadPhone_Import_Phone"));
            if (ahdGroups.Exists(x => x.AdHocDimensionGroupName.Equals("Advanstar_BadPhone_Import_Fax")))
                badFaxADG = ahdGroups.Single(x => x.AdHocDimensionGroupName.Equals("Advanstar_BadPhone_Import_Fax"));
            if (ahdGroups.Exists(x => x.AdHocDimensionGroupName.Equals("Advanstar_BadPhone_Import_Mobile")))
                badMobileADG = ahdGroups.Single(x => x.AdHocDimensionGroupName.Equals("Advanstar_BadPhone_Import_Mobile"));

            List<FrameworkUAS.Entity.AdHocDimension> newTitleList = new List<AdHocDimension>();
            List<FrameworkUAS.Entity.AdHocDimension> badPhoneList = new List<AdHocDimension>();
            List<FrameworkUAS.Entity.AdHocDimension> badFaxList = new List<AdHocDimension>();
            List<FrameworkUAS.Entity.AdHocDimension> badMobileList = new List<AdHocDimension>();
            if (newTitleADG != null)
                newTitleList = adWorker.Select(newTitleADG.AdHocDimensionGroupId);
            if (badPhoneADG != null)
                badPhoneList = adWorker.Select(badPhoneADG.AdHocDimensionGroupId);
            if (badFaxADG != null)
                badFaxList = adWorker.Select(badFaxADG.AdHocDimensionGroupId);
            if (badMobileADG != null)
                badMobileList = adWorker.Select(badMobileADG.AdHocDimensionGroupId);            

            //loop through each record and switch title to newtitle and demo33 for badphone
            foreach (var key in dataIV.DataTransformed.Keys)
            {
                StringDictionary myRow = dataIV.DataTransformed[key];

                #region TITLE->NEWTITLE Logic
                string titleField = string.Empty;                

                if (newTitleADG != null)
                {
                    if (myRow.ContainsKey(newTitleADG.StandardField))
                        titleField = myRow[newTitleADG.StandardField].ToString();

                    if (!string.IsNullOrEmpty(titleField))
                    {
                        //check starts and contains list - if found then set to DimensionValue else leave default
                        string fieldToChange = newTitleADG.CreatedDimension.ToUpper();
                        foreach (FrameworkUAS.Entity.AdHocDimension c in newTitleList)
                        {
                            if (titleField.Equals(c.MatchValue, StringComparison.CurrentCultureIgnoreCase))
                            {
                                myRow[fieldToChange] = c.DimensionValue;
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region BAD PHONE|FAX|MOBILE Logic
                string phoneField = string.Empty;                
                string faxField = string.Empty;                
                string mobileField = string.Empty;                

                #region Phone
                if (badPhoneADG != null)
                {
                    if (myRow.ContainsKey(badPhoneADG.StandardField))
                        phoneField = myRow[badPhoneADG.StandardField].ToString();

                    if (!string.IsNullOrEmpty(phoneField))
                    {
                        foreach (FrameworkUAS.Entity.AdHocDimension ad in badPhoneList)
                        {
                            if (phoneField.Equals(ad.MatchValue, StringComparison.CurrentCultureIgnoreCase))
                            {
                                string generatedValue = ad.DimensionValue;
                                if (!myRow.ContainsKey(badPhoneADG.CreatedDimension.ToUpper()))
                                    myRow.Add(badPhoneADG.CreatedDimension.ToUpper(), generatedValue);
                                else
                                    myRow[badPhoneADG.CreatedDimension.ToUpper()] = generatedValue;
                            }
                        }
                    }
                }
                #endregion
                #region Fax
                if (badFaxADG != null)
                {
                    if (myRow.ContainsKey(badFaxADG.StandardField))
                        faxField = myRow[badFaxADG.StandardField].ToString();

                    if (!string.IsNullOrEmpty(faxField))
                    {
                        foreach (FrameworkUAS.Entity.AdHocDimension ad in badFaxList)
                        {
                            if (faxField.Equals(ad.MatchValue, StringComparison.CurrentCultureIgnoreCase))
                            {
                                string generatedValue = ad.DimensionValue;
                                if (!myRow.ContainsKey(badFaxADG.CreatedDimension.ToUpper()))
                                    myRow.Add(badFaxADG.CreatedDimension.ToUpper(), generatedValue);
                                else
                                    myRow[badFaxADG.CreatedDimension.ToUpper()] = generatedValue;
                            }
                        }
                    }
                }
                #endregion
                #region Mobile
                //Only if phone was empty will bad mobile set demo field 
                if (badMobileADG != null)
                {
                    if (myRow.ContainsKey(badMobileADG.StandardField))
                        mobileField = myRow[badMobileADG.StandardField].ToString();

                    if (string.IsNullOrEmpty(phoneField) && !string.IsNullOrEmpty(mobileField))
                    {
                        foreach (FrameworkUAS.Entity.AdHocDimension ad in badMobileList)
                        {
                            if (mobileField.Equals(ad.MatchValue, StringComparison.CurrentCultureIgnoreCase))
                            {
                                string generatedValue = ad.DimensionValue;
                                if (!myRow.ContainsKey(badMobileADG.CreatedDimension.ToUpper()))
                                    myRow.Add(badMobileADG.CreatedDimension.ToUpper(), generatedValue);
                                else
                                    myRow[badMobileADG.CreatedDimension.ToUpper()] = generatedValue;
                            }
                        }
                    }
                }
                #endregion
                #endregion
            }

            return dataIV;
        }

        private static void ArchiveFile(FileMoved eventMessage)
        {
            if (File.Exists(eventMessage.ImportFile.FullName))
            {
                File.Move(eventMessage.ImportFile.FullName,
                    string.Format(
                        "{0}\\{1}\\Processed\\{2}",
                        Core.ADMS.BaseDirs.getClientArchiveDir(),
                        eventMessage.Client.FtpFolder,
                        eventMessage.ImportFile.Name));
            }
        }
    }
}