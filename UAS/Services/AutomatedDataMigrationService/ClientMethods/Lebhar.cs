using System;
using System.Linq;
using Core.ADMS.Events;
using FrameworkUAS.Entity;
using Core_AMS.Utilities;
using System.Data;
using System.Collections.Generic;
using System.IO;
using KM.Common.Import;

namespace ADMS.ClientMethods
{
    public class Lebhar : ClientSpecialCommon
    {
        private const string TitleCodeGroupName = "Lebhar_TITLE_TITLE_CODE";
        private const string TitleCodeDim = "TITLE_CODE";
        private const string CompanyGroupName = "Lebhar_Company_Top100";
        private const string Top100Dim = "TOP_100_RANKING";
        private const string RankFieldName = "RANK";
        private const string CompanyFieldName = "COMPANY";

        #region Multi-Split File
        private bool newCheck = false;

        public void FileKMNM(KMPlatform.Entity.Client client, FileInfo file)
        {
            FileWorker fw = new FileWorker();
            FileConfiguration fileConfig = new FileConfiguration()
            {
                FileColumnDelimiter = "comma",
                IsQuoteEncapsulated = true
            };
            DataTable dataIV = fw.GetData(file, fileConfig);
            int totalRows = dataIV.Rows.Count;
            int batch = 2500;
            int rowsProcessed = 0;
            //ImportVessel dataIV = new ImportVessel();
            //dataIV = fw.GetImportVessel(file, 0, 2500, fileConfig);

            ConsoleMessage("found rows: " + dataIV.Rows.Count.ToString(),"FileKMNM", false);
            int columns = dataIV.Columns.Count;
            ConsoleMessage("found columns: " + columns.ToString(), "FileKMNM", false);

            dataIV.Columns.Add("PubCode");
            dataIV.AcceptChanges();

            if (File.Exists("C:\\ADMS\\Client Archive\\Lebhar\\KMNM_20140531_071428.csv"))
                File.Delete("C:\\ADMS\\Client Archive\\Lebhar\\KMNM_20140531_071428.csv");

            DataTable workTable = dataIV.Clone();
            while (rowsProcessed < totalRows)
            {
                workTable.Clear();
                List<DataRow> newRows = new List<DataRow>();

                int batchProcessing = rowsProcessed + batch;
                if (batchProcessing > totalRows)
                    batchProcessing = totalRows;

                ConsoleMessage("New Batch: " + batchProcessing.ToString(), "FileKMNM", false);
                ConsoleMessage("Processed: " + rowsProcessed.ToString(), "FileKMNM", false);

                for (int i = rowsProcessed; i < batchProcessing; i++)
                {
                    DataRow dr = dataIV.Rows[rowsProcessed];
                    newCheck = false;
                    string[] pubCodes;
                    string UniqueKey = dr["UNIQUE_KEY"].ToString();
                    string LessonUAN = dr["LESSON_UAN"].ToString();

                    if (string.IsNullOrEmpty(LessonUAN))
                    {
                        AddRow("CCPRINT", newRows, workTable, dr);
                    }
                    else
                    {
                        LessonUAN = LessonUAN.Replace("-", "");
                        pubCodes = LessonUAN.Split(',');

                        foreach (string pub in pubCodes)
                        {
                            string pubCode = pub.Substring(pub.Length - 9, 9);
                            pubCode = "WB" + pubCode;
                            AddRow(pubCode, newRows, workTable, dr);
                        }
                        if ((UniqueKey.Substring(UniqueKey.Length - 7, 7)).Equals("0000000"))
                            AddRow("CCPRINT", newRows, workTable, dr);
                    }
                    rowsProcessed++;
                }

                foreach (DataRow dr in newRows)
                    workTable.Rows.Add(dr);

                workTable.AcceptChanges();

                //for testing
                Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
                ff.CreateCSVFromDataTable(workTable, "C:\\ADMS\\Client Archive\\Lebhar\\LebharMultiSplit01.csv", false);
                GC.Collect();
            }

            ConsoleMessage("end count rows: " + workTable.Rows.Count.ToString(), "FileKMNM", false);
            ConsoleMessage("Uploading file to Client FTP", "FileKMNM", false);
            FrameworkUAS.BusinessLogic.ClientFTP clientFTPData = new FrameworkUAS.BusinessLogic.ClientFTP();
            FrameworkUAS.Entity.ClientFTP clientFTP = clientFTPData.SelectClient(10).FirstOrDefault();
            bool uploaded = false;
            if (clientFTP != null)
            {
                Core_AMS.Utilities.FtpFunctions ftpData = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
                uploaded = ftpData.Upload(clientFTP.Folder + "\\LebharMultiSplit01.csv", "C:\\ADMS\\Client Archive\\Lebhar\\LebharMultiSplit01.csv");
            }
            ConsoleMessage("Uploaded = " + uploaded.ToString(), "FileKMNM", false);
        }



        public void AddRow(string pubCode, List<DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;
            newRow.SetField("PubCode", pubCode);
            rows.Add(newRow);
        }
        #endregion

        #region Split Rules
        //For file KMNM_* 
        //1) Read into datatable
        //2) For all rows where (Lesson_UAN != null) and (RIGHT([Unique_Key],7)!="0000000"), append ",CCPRINT"
        //2) Split into rows based on column [LESSON_UAN], comma delimited
        //3) For [LESSON_UAN], remove dashes
        //4) Pubcode will be "WB" + RIGHT([LESSON_UAN],9), except "CCPRINT" which is left alone
        //5) Fill all nulls in [LESSON_UAN] with pubcode "CCPRINT"

        //So, for Lesson_UAN = "0401-0000-10-100-H03-G", pubcode is "WB10100H03G" (after split)
        #endregion

        #region Look-Up File
        public string TitleCodeLookup()
        {
            string titleCode = string.Empty;

            return titleCode;
        }
        public void TitleAdHocImport(KMPlatform.Entity.Client client, FileMoved eventMessage)
        {
            var fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
            {
                Client = client,
                EventMessage = eventMessage,
                AdHocDimensionGroupName = TitleCodeGroupName,
                CreatedDimension = TitleCodeDim,
                StandardField = TitleStandardField,
                DimensionValueField = TitleCodeFieldName,
                MatchValueField = TitleFieldName,
                DimensionOperator = ContainsOperation
            };
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(fillAgGroupAndTableArgs);
        }

        public void CompanyAdHocImport(KMPlatform.Entity.Client client, FileMoved eventMessage)
        {
            var fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
            {
                Client = client,
                EventMessage = eventMessage,
                AdHocDimensionGroupName = CompanyGroupName,
                CreatedDimension = Top100Dim,
                StandardField = CompanyStandardField,
                DimensionValueField = RankFieldName,
                MatchValueField = CompanyFieldName,
                DimensionOperator = ContainsOperation
            };
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(fillAgGroupAndTableArgs);
        }
        #endregion
    }
}
