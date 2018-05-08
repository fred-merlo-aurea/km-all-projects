using Core.ADMS.Events;
using FrameworkUAS.Entity;
using Core_AMS.Utilities;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using KM.Common.Import;

namespace ADMS.ClientMethods
{
    public class ATHB : ClientSpecialCommon
    {
        //OLD NAME CHANGE TO BE LIKE FILE NAME IS NOW -Jason
        //public void File_ABC_CONF_2(KMPlatform.Entity.Client client, FileInfo fileInfo)
        //public void KMPS_MASTER_conf_data(KMPlatform.Entity.Client client, FileInfo fileInfo, ClientSpecialFile cSpecialFile)
        //eventMessage.Client, cSpecialFile, ccp, eventMessage
        public void KMPS_MASTER_conf_data(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            //fileWorker.GetRowCount(eventMessage.FileName);  50000
            //new DataTable = fileWorker.GetImportVessel(eventMessage.FileName, startRow, fileRowBatch, fileConfig);
            //loop for each row in new DataTable 
            //write to file via ff.CreateCSVFromDataTable(dtEcom, "C:\\ADMS\\Client Archive\\Haymarket\\CMS_Ecom.csv", false);

            FileWorker fileWorker = new FileWorker();
            fileWorker.RemoveTrailingCharactersFromTextFile(eventMessage.ImportFile);

            DataTable dataIV = new DataTable();
            dataIV.CaseSensitive = false;
            FileWorker fw = new FileWorker();
            FileConfiguration fc = new FileConfiguration();
            fc.ColumnCount = eventMessage.SourceFile.FieldMappings.Count;
            fc.ColumnHeaders = "";
            fc.FileColumnDelimiter = eventMessage.SourceFile.Delimiter;
            fc.FileExtension = eventMessage.SourceFile.Extension;
            fc.IsQuoteEncapsulated = true;//eventMessage.SourceFile.IsTextQualifier;

            dataIV = fw.GetData(eventMessage.ImportFile, fc);

            ConsoleMessage("found rows: " + dataIV.Rows.Count.ToString(), "KMPS_MASTER_conf_data", false);
            int columns = dataIV.Columns.Count;
            ConsoleMessage("found columns: " + columns.ToString(), "KMPS_MASTER_conf_data", false);
            
            dataIV.AcceptChanges();

            if (File.Exists("C:\\ADMS\\Client Archive\\ATHB\\MS_AutoGen_abc_conf.csv"))
                File.Delete("C:\\ADMS\\Client Archive\\ATHB\\MS_AutoGen_abc_conf.csv");

            List<string> pubCodeList = new List<string>();
            foreach (DataRow dr in dataIV.Rows)
            {
                string pc = dr["Demo3"].ToString();
                if (!string.IsNullOrEmpty(pc) && !pubCodeList.Contains(pc))
                    pubCodeList.Add(pc);
            }
            
            int totalRows = dataIV.Rows.Count;
            int batch = 2500;
            int rowsProcessed = 0;
            //for (int i = 0; i < dataIV.Rows.Count; i++)
            DataTable workTable = dataIV.Clone();
            while (rowsProcessed < totalRows)
            {
                workTable.Clear();
                Dictionary<int, DataRow> newRows = new Dictionary<int, DataRow>();
                Dictionary<int, DataRow> deleteRows = new Dictionary<int, DataRow>();
                int rowIndex = 0;

                int batchProcessing = rowsProcessed + batch;
                if (batchProcessing > totalRows)
                    batchProcessing = totalRows;

                ConsoleMessage("New Batch: " + batchProcessing.ToString(), "KMPS_MASTER_conf_data", false);
                ConsoleMessage("Processed: " + rowsProcessed.ToString(), "KMPS_MASTER_conf_data", false);
                for (int i = rowsProcessed; i < batchProcessing; i++)
                {
                    DataRow dr = dataIV.Rows[rowsProcessed];
                    List<string> pubCodes = new List<string>();
                    string DEMO3 = dr["DEMO3"].ToString();
                    string DEMO6 = dr["DEMO6"].ToString();
                    string lastPub = "";
                    if (String.IsNullOrEmpty(DEMO3) && String.IsNullOrEmpty(DEMO6))
                        lastPub = "NEEDSDEFAULTVALUE";
                    else
                    {
                        //string[] pubs3 = DEMO3.Split(',');
                        string[] pubs6 = DEMO6.Split(',');
                        foreach (string p in pubs6)
                            if (pubCodes.FirstOrDefault(x => x == p) == null)
                                pubCodes.Add(p);

                        if (pubCodes.FirstOrDefault(x => x == DEMO3) == null)
                            pubCodes.Add(DEMO3);

                        string finalPubs = string.Join(",", pubCodes);
                        lastPub = finalPubs.TrimStart(',').TrimEnd(',');                  
                    }
                    
                    DataRow newRow = workTable.NewRow();
                    newRow.ItemArray = dr.ItemArray;
                    newRow.SetField("pubcode", lastPub);
                    newRows.Add(rowIndex, newRow);
                    deleteRows.Add(rowIndex, dr);
                    rowIndex++;
                    rowsProcessed++;
                }

                //foreach (KeyValuePair<int, DataRow> kvp in deleteRows.OrderByDescending(k => k.Key))
                //{
                //    dataIV.Rows[kvp.Key].Delete();
                //}
                //dataIV.AcceptChanges();
                foreach (KeyValuePair<int, DataRow> kvp in newRows)
                {
                    workTable.Rows.Add(kvp.Value);
                }
                workTable.AcceptChanges();

                //for testing
                Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
                ff.CreateCSVFromDataTable(workTable, "C:\\ADMS\\Client Archive\\ATHB\\MS_AutoGen_abc_conf.csv", false);
                GC.Collect();
            }

            ConsoleMessage("end count rows: " + dataIV.Rows.Count.ToString(), "KMPS_MASTER_conf_data", false);
            ConsoleMessage("Uploading file to Client FTP", "KMPS_MASTER_conf_data", false);
            FrameworkUAS.BusinessLogic.ClientFTP blcftp = new FrameworkUAS.BusinessLogic.ClientFTP();
            ClientFTP clientFTP = new ClientFTP();
            clientFTP = blcftp.SelectClient(client.ClientID).First();
            Core_AMS.Utilities.FtpFunctions ftp = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
            bool uploaded = ftp.Upload(clientFTP.Folder + "\\MS_AutoGen_abc_conf.csv", "C:\\ADMS\\Client Archive\\ATHB\\MS_AutoGen_abc_conf.csv");
            ConsoleMessage("Uploaded = " + uploaded.ToString(), "KMPS_MASTER_conf_data", false);
        }

        #region ATHB Conference file splitting pubcode
        //public void KMPS_MASTER_conf_data(Client client, ClientSpecialFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        //{
        //    //fileWorker.GetRowCount(eventMessage.FileName);  50000
        //    //new DataTable = fileWorker.GetImportVessel(eventMessage.FileName, startRow, fileRowBatch, fileConfig);
        //    //loop for each row in new DataTable 
        //    //write to file via ff.CreateCSVFromDataTable(dtEcom, "C:\\ADMS\\Client Archive\\Haymarket\\CMS_Ecom.csv", false);
                        
        //    FileWorker fileWorker = new FileWorker();
        //    fileWorker.RemoveTrailingCharactersFromTextFile(eventMessage.ImportFile);

        //    DataTable dataIV = new DataTable();
        //    dataIV.CaseSensitive = false;
        //    FileWorker fw = new FileWorker();
        //    FileConfiguration fc = new FileConfiguration();
        //    fc.ColumnCount = eventMessage.SourceFile.FieldMappings.Count;
        //    fc.ColumnHeaders = "";
        //    fc.FileColumnDelimiter = eventMessage.SourceFile.Delimiter;
        //    fc.FileExtension = eventMessage.SourceFile.Extension;
        //    fc.IsQuoteEncapsulated = eventMessage.SourceFile.IsTextQualifier;
            
        //    dataIV = fw.GetData(eventMessage.ImportFile, fc);

        //    ConsoleMessage("found rows: " + dataIV.Rows.Count.ToString(),"KMPS_MASTER_conf_data", false);
        //    int columns = dataIV.Columns.Count;
        //    ConsoleMessage("found columns: " + columns.ToString(), "KMPS_MASTER_conf_data", false);

        //    //dataIV.Columns.Add("PubCode");
        //    dataIV.AcceptChanges();

        //    //List<DataRow> newRows = new List<DataRow>();

        //    if (File.Exists("C:\\ADMS\\Client Archive\\ATHB\\MS_AutoGen_abc_conf.csv"))
        //        File.Delete("C:\\ADMS\\Client Archive\\ATHB\\MS_AutoGen_abc_conf.csv");

        //    List<string> pubCodeList = new List<string>();
        //    foreach(DataRow dr in dataIV.Rows)
        //    {
        //        string pc = dr["Demo3"].ToString();
        //        if (!string.IsNullOrEmpty(pc) && !pubCodeList.Contains(pc))
        //            pubCodeList.Add(pc);
        //    }

            
        //    //foreach (DataRow dr in dataIV.Rows)
        //    int totalRows = dataIV.Rows.Count;
        //    int batch = 2500;
        //    int rowsProcessed = 0;
        //    //for (int i = 0; i < dataIV.Rows.Count; i++)
        //    DataTable workTable = dataIV.Clone();            
        //    while (rowsProcessed < totalRows)
        //    {
        //        workTable.Clear();
        //        Dictionary<int, DataRow> newRows = new Dictionary<int, DataRow>();
        //        Dictionary<int, DataRow> deleteRows = new Dictionary<int, DataRow>();
        //        int rowIndex = 0;

        //        int batchProcessing = rowsProcessed + batch;
        //        if (batchProcessing > totalRows)
        //            batchProcessing = totalRows;

        //        ConsoleMessage("New Batch: " + batchProcessing.ToString(),"KMPS_MASTER_conf_data", false);
        //        ConsoleMessage("Processed: " + rowsProcessed.ToString(),"KMPS_MASTER_conf_data", false);
        //        for (int i = rowsProcessed; i < batchProcessing; i++)
        //        {
        //            DataRow dr = dataIV.Rows[rowsProcessed];
        //            List<string> pubCodes = new List<string>();
        //            //string DEMO3 = dr["DEMO3"].ToString();
        //            string DEMO6 = dr["DEMO6"].ToString();

        //            //if (!string.IsNullOrEmpty(DEMO3))
        //            //{
        //            //    string[] pubs = DEMO3.Split(',');
        //            //    foreach (string p in pubs)
        //            //        if (!pubCodes.Contains(p))
        //            //            pubCodes.Add(p);

        //            //}

        //            if (!string.IsNullOrEmpty(DEMO6))
        //            {
        //                string[] pubs = DEMO6.Split(',');
        //                foreach (string p in pubs)
        //                    if (!pubCodes.Contains(p))
        //                        pubCodes.Add(p);

        //            }

        //            if (!pubCodes.Contains(pubCodeList.FirstOrDefault()))
        //                pubCodes.Add(pubCodeList.FirstOrDefault());

        //            foreach (string pub in pubCodes)
        //            {
        //                string pubCode = pub.Trim();
        //                if (!String.IsNullOrEmpty(pubCode))
        //                    AddRow(pubCode, newRows.Count + 1, newRows, workTable, dr);

        //            }

        //            deleteRows.Add(rowIndex, dr);
        //            rowIndex++;
        //            rowsProcessed++;
        //        }

        //        //foreach (KeyValuePair<int, DataRow> kvp in deleteRows.OrderByDescending(k => k.Key))
        //        //{
        //        //    dataIV.Rows[kvp.Key].Delete();
        //        //}
        //        //dataIV.AcceptChanges();
        //        foreach (KeyValuePair<int, DataRow> kvp in newRows)
        //        {
        //            workTable.Rows.Add(kvp.Value);
        //        }
        //        workTable.AcceptChanges();

        //        //for testing
        //        Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
        //        ff.CreateCSVFromDataTable(workTable, "C:\\ADMS\\Client Archive\\ATHB\\MS_AutoGen_abc_conf.csv", false);
        //        GC.Collect();
        //    }

            

        //    ConsoleMessage("end count rows: " + dataIV.Rows.Count.ToString(),"KMPS_MASTER_conf_data", false);
        //    ConsoleMessage("Uploading file to Client FTP","KMPS_MASTER_conf_data", false);
        //    FrameworkUAS.BusinessLogic.ClientFTP blcftp = new FrameworkUAS.BusinessLogic.ClientFTP();
        //    ClientFTP clientFTP = new ClientFTP();
        //    clientFTP = blcftp.SelectClient(client.ClientID).First();
        //    Core_AMS.Utilities.FtpFunctions ftp = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
        //    bool uploaded = ftp.Upload(clientFTP.Folder + "\\MS_AutoGen_abc_conf.csv", "C:\\ADMS\\Client Archive\\ATHB\\MS_AutoGen_abc_conf.csv");
        //    ConsoleMessage("Uploaded = " + uploaded.ToString(),"KMPS_MASTER_conf_data", false);
        //}
        #endregion
        
        public void AddRow(string pubCode, int pos, Dictionary<int, DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;
            newRow.SetField("PubCode", pubCode);
            rows.Add(pos, newRow);
        }
    }
}
