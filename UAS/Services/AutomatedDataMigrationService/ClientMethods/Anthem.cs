using System;
using System.Linq;
using FrameworkUAS.Entity;
using Core_AMS.Utilities;
using System.Data;
using System.Collections.Generic;
using System.IO;

namespace ADMS.ClientMethods
{
    public class Anthem : ClientSpecialCommon
    {
        public void FileKCM_KCB_GHCK(KMPlatform.Entity.Client client, SourceFile sFile, FileInfo file)
        {
            FileWorker fw = new FileWorker();

            DataTable dataIV = new DataTable();
            dataIV = fw.GetData(file, null);

            if (File.Exists("C:\\ADMS\\Client Archive\\Anthem\\KCM-KCB-GHCK_Email3_3_14.xlsx"))
                File.Delete("C:\\ADMS\\Client Archive\\Anthem\\KCM-KCB-GHCK_Email3_3_14.xlsx");

            #region Uncomment For Testing KCB 3RD Party
            //FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
            //FrameworkUAS.BusinessLogic.FieldMapping fmpData = new FrameworkUAS.BusinessLogic.FieldMapping();
            //eventMessage.SourceFile = sfData.Select("Anthem", "KCM-KCB-GHCK_Email");
            //eventMessage.SourceFile.FieldMappings = fmpData.Select(108);
            #endregion
            int batch = 2500;
            int rowsProcessed = 0;
            //DataTable workTable = dataIV.Clone();

            ConsoleMessage("found rows: " + dataIV.Rows.Count.ToString(),"FileKCM_KCB_GHCK", false);
            int columns = dataIV.Columns.Count;
            ConsoleMessage("found columns: " + columns.ToString(), "FileKCM_KCB_GHCK", false);

            //add a PubCode column to datatable
            dataIV.Columns.Add("PubCode");
            dataIV.AcceptChanges();

            List<DataRow> newRows = new List<DataRow>();
            foreach (DataRow dr in dataIV.Rows)
            {
                bool newCheck = false;
                string KCBenews = dr["KCB ENewsletter"].ToString();
                //make sure not null
                string KCenews = dr["KC Enewsletter"].ToString();
                //make sure not null

                if (string.IsNullOrEmpty(KCBenews))
                {
                    if (string.IsNullOrEmpty(KCenews))
                        dr.SetField("PubCode", "KCM");
                }
                if (KCenews.Equals("Y"))
                {
                    DataRow newRow1 = dataIV.NewRow();
                    newRow1.ItemArray = dr.ItemArray;
                    dr.SetField("PubCode", "KCDE");
                    newRow1.SetField("PubCode", "KCSE");
                    newRows.Add(newRow1);
                    newCheck = true;
                }
                if (KCBenews.Equals("Y"))
                {
                    if (newCheck == true)
                    {
                        DataRow newRow = dataIV.NewRow();
                        newRow.ItemArray = dr.ItemArray;
                        newRow.SetField("PubCode", "KCBE");
                        newRows.Add(newRow);
                    }
                    else
                    {
                        dr.SetField("PubCode", "KCBE");
                    }
                }
            }

            int totalRows = newRows.Count;
            while (rowsProcessed < totalRows)
            {
                int batchProcessing = rowsProcessed + batch;
                if (batchProcessing > totalRows)
                    batchProcessing = totalRows;
                ConsoleMessage("New Batch: " + batchProcessing.ToString(),"FileKCM_KCB_GHCK", false);
                ConsoleMessage("Processed: " + rowsProcessed.ToString(),"FileKCM_KCB_GHCK", false);
                for (int i = rowsProcessed; i < batchProcessing; i++)
                {
                    DataRow dr = newRows[rowsProcessed];
                    dataIV.Rows.Add(dr);
                    rowsProcessed++;
                }
            }
            dataIV.AcceptChanges();


            #region SplitRules
            //Multi-Split file for Anthem (they don't send this every month, but they have refreshed it):   
            //Pubcode is based on 2 columns, [KCB ENewsletter] and [KC Enewsletter], both of which can be 'Y' or null
            //[KC Enewsletter]='Y', pubcodes: KCDE, KCSE
            //[KCB ENewsletter]='Y', pubcode: KCBE
            //[Both null], pubcode: KCM
            //(so all records will have a pubcode, if both optins are Y then it will have 3 pubcodes)
            #endregion

            Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
            ff.CreateCSVFromDataTable(dataIV, "C:\\ADMS\\Client Archive\\Anthem\\AnthemMultiSplit01.csv");

            CheckFieldMapping(sFile);

            FrameworkUAS.BusinessLogic.ClientFTP clientFTPData = new FrameworkUAS.BusinessLogic.ClientFTP();
            FrameworkUAS.Entity.ClientFTP clientFTP = clientFTPData.SelectClient(3).FirstOrDefault();
            bool uploaded = false;
            if (clientFTP != null)
            {
                Core_AMS.Utilities.FtpFunctions ftpData = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
                uploaded = ftpData.Upload(clientFTP.Folder + "\\AnthemMultiSplit01.csv", "C:\\ADMS\\Client Archive\\Anthem\\AnthemMultiSplit01.csv");
            }

            ConsoleMessage("Uploaded = " + uploaded.ToString(),"FileKCM_KCB_GHCK");
        }

        public void CheckFieldMapping(SourceFile sFile)
        {
            //(also for this file, [KCB 3RD PARTY] should map to DEMO35)
            HashSet<FrameworkUAS.Entity.FieldMapping> fm = sFile.FieldMappings;
            if (fm.Count(x => x.IncomingField.Equals("KCB 3RD PARTY", System.StringComparison.InvariantCultureIgnoreCase)) > 0) //(fm.Exists(x => x.IncomingField.Equals("KCB 3RD PARTY", System.StringComparison.InvariantCultureIgnoreCase)))
            {
                bool exists = false;
                foreach (FrameworkUAS.Entity.FieldMapping f in fm)
                {
                    if (f.IncomingField.Equals("KCB 3RD PARTY", System.StringComparison.InvariantCultureIgnoreCase) && f.MAFField.Equals("DEMO35"))
                    {
                        exists = true;
                        break;
                    }
                }
                if (exists == false)
                {
                    FieldMapping f = fm.FirstOrDefault(x => x.IncomingField.Equals("KCB 3RD PARTY", System.StringComparison.InvariantCultureIgnoreCase));
                    f.MAFField = "DEMO35";
                }
            }
            else
            {
                FrameworkUAS.BusinessLogic.FieldMapping fmData = new FrameworkUAS.BusinessLogic.FieldMapping();
                FieldMapping newFM = new FieldMapping();
                newFM.FieldMappingTypeID = 2;
                newFM.IsNonFileColumn = false;
                newFM.SourceFileID = 108;
                newFM.IncomingField = "KCB 3rd Party";
                newFM.MAFField = "DEMO35";
                newFM.PubNumber = 0;
                newFM.DataType = "VARCHAR";
                fmData.Save(newFM);
            }
        }
    }
}
