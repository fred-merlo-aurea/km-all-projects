using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ADMS.ClientMethods.Common;
using Core.ADMS.Events;
using Core_AMS.Utilities;
using FrameworkUAS.Entity;
using KM.Common;
using KM.Common.Import;
using ClientEntity = KMPlatform.Entity.Client;
using FileFunctions = Core_AMS.Utilities.FileFunctions;
using ImportErrorEntity = FrameworkUAD.Entity.ImportError;
using ImportVesselBusiness = FrameworkUAD.BusinessLogic.ImportVessel;

namespace ADMS.ClientMethods
{
    public class SAETB : ClientSpecialCommon
    {
        private const int Ntb258ColumnCount = 133;
        private const int KnwS0100ColumnCount = 50;
        private const int MysaeColumnCount = 50;

        private const string AutoGenSaetbMembersNtb258ENewsletterFileName = "AutoGen_SAETB_Members_NTB258_ENewsletter.csv";
        private const string MembersNtb258ENewsletterFilePath = "C:\\ADMS\\Client Archive\\SAETB\\members-ntb258_enewsletter.csv";
        private const string SaetbMembersNtb258ENewsletterCode = "SAETB_Members_NTB258_ENewsletter";
        private const string SaetbAutoGenMembersNtb258ENewsletterFilePath = "C:\\ADMS\\Client Archive\\SAETB\\SAETB_AutoGen_Members_NTB258_ENewsletter.csv";

        private const string AutoGenKnws0100Code = "AutoGen_KNWS0100";
        private const string AutoGenSaetbKnws0100FileName = "AutoGen_SAETB_KNWS0100.csv";
        private const string Knws0100FilePath = "C:\\ADMS\\Client Archive\\SAETB\\KNWS0100.csv";
        private const string Knws0100Code = "KNWS0100";
        private const string SaetbKnws0100Code = "SAETB_KNWS0100";
        private const string SaetbAutoGenKnws0100FilePath = "C:\\ADMS\\Client Archive\\SAETB\\SAETB_AutoGen_KNWS0100.csv";

        private const string AutoGenMysaeCode = "AutoGen_MYSAE";
        private const string MysaeCode = "MYSAE";
        private const string MysaeFilePath = "C:\\ADMS\\Client Archive\\SAETB\\MYSAE.csv";
        private const string SaetbMysaeCode = "SAETB_MYSAE";
        private const string SaetbAutoGenMysaeFileName = "SAETB_AutoGen_MYSAE.csv";
        private const string SaetbAutoGenMysaeFilePath = "C:\\ADMS\\Client Archive\\SAETB\\SAETB_AutoGen_MYSAE.csv";

        private const string InitialTransId = "10";
        private const string DefaultTransId = "38";

        public const string NoEmailKey = "noemail";
        public const string NoAdvEmailKey = "noadvemail";
        public const string NoTelemktg = "notelemktg";
        public const string Noroweml = "noroweml";
        public const string Noroweml3 = "noroweml3";

        public void SAETB_Members_NTB258_ENewsletter(
            ClientEntity clientEntity, 
            SourceFile clientSpecialFile, 
            ClientCustomProcedure clientCustomProcedure, 
            FileMoved eventMessage)
        {
            ClientMethodHelpers.DeleteExistingFiles(MembersNtb258ENewsletterFilePath);
            ClientMethodHelpers.DeleteExistingFiles(SaetbAutoGenMembersNtb258ENewsletterFilePath);

            try
            {
                var fcAccess = ClientMethodHelpers.CreateFileConfiguration(
                    Ntb258ColumnCount, Consts.DefaultColumnHeaders, Consts.CommaFileDelimiter, Consts.CsvExtension, true);
                Console.WriteLine(Consts.MessageGettingFile, DateTime.Now);

                var dtAccess = ProcessImportVesselRows(eventMessage, fcAccess, null, string.Empty);
                if (dtAccess != null)
                {
                    CreateCsvFileForNtb258(dtAccess);
                    dtAccess.Dispose();
                }
            }
            // Must take any exception that arises
            catch (Exception ex)
            {
                LogError(ex, clientEntity, $"{GetType().Name}{Consts.DotSepator}{SaetbMembersNtb258ENewsletterCode}");
            }

            ClientMethodHelpers.FinishingUploadToFtp(clientEntity, AutoGenSaetbMembersNtb258ENewsletterFileName, SaetbAutoGenMembersNtb258ENewsletterFilePath);
        }

        public DataTable ProcessImportVesselRows(
            FileMoved eventMessage, 
            FileConfiguration fileConfigurationAccess, 
            IList<ImportErrorEntity> importErrors,
            string processCode)
        {
            Guard.NotNull(eventMessage, nameof(eventMessage));
            Guard.NotNull(fileConfigurationAccess, nameof(fileConfigurationAccess));

            var importVesselData = new ImportVesselBusiness();
            var fileWorker = new FileWorker();
            var totalRowCount = fileWorker.GetRowCount(eventMessage.ImportFile);
            var rowProcessedCount = 0;
            DataTable dtAccess = null;
            var firstRun = true;

            if (totalRowCount > 0)
            {
                while (rowProcessedCount < totalRowCount)
                {
                    var endRow = rowProcessedCount + Consts.RowBatch;
                    if (endRow > totalRowCount)
                    {
                        endRow = totalRowCount;
                    }

                    var startRow = rowProcessedCount + 1;
                    var percent = (int)Math.Round((double)(100 * startRow) / totalRowCount);
                    Console.WriteLine($"{Consts.ProcessingStatus}{startRow} to {endRow} {DateTime.Now} {percent}%");

                    var importVessel = importVesselData.GetImportVessel(eventMessage.ImportFile, startRow, Consts.RowBatch, fileConfigurationAccess);
                    if (endRow == totalRowCount)
                    {
                        rowProcessedCount = totalRowCount;
                    }
                    else
                    {
                        rowProcessedCount += importVessel.TotalRowCount;
                    }

                    if (firstRun)
                    {
                        dtAccess = importVessel.DataOriginal.Copy();
                        firstRun = false;
                    }
                    else
                    {
                        dtAccess.Merge(importVessel.DataOriginal);
                    }

                    if (importErrors != null)
                    {
                        ((List<ImportErrorEntity>)importErrors).AddRange(importVessel.ImportErrors);
                        ConsoleMessage($"{Consts.MessageCurrentRowCount}{dtAccess.Rows.Count}", processCode, false);
                    }
                    importVessel.DataOriginal.Dispose();
                }
            }
            return dtAccess;
        }

        public void CreateCsvFileForNtb258(DataTable dtAccess)
        {
            Guard.NotNull(dtAccess, nameof(dtAccess));

            var fileFunctions = new FileFunctions();
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

                ConsoleMessage($"{Consts.NewBatchStatus}{batchProcessing}", SaetbMembersNtb258ENewsletterCode, false);
                ConsoleMessage($"{Consts.ProcessedStatus}{workProcessed}", SaetbMembersNtb258ENewsletterCode, false);

                for (var i = workProcessed; i < batchProcessing; i++)
                {
                    var dataRow = dtAccess.Rows[workProcessed];
                    var pubCodes = new List<string>();
                    AddAllPubCodes(dataRow, pubCodes);

                    if (pubCodes.Count > 0)
                    {
                        foreach (var pub in pubCodes)
                        {
                            var pubCode = pub.Trim();
                            ClientMethodHelpers.AddRow(pubCode, newRows.Count + 1, newRows, workTable, dataRow);
                        }
                    }

                    deleteRows.Add(rowIndex, dataRow);
                    rowIndex++;
                    workProcessed++;
                }
                foreach (var row in newRows)
                {
                    workTable.Rows.Add(row.Value);
                }

                workTable.AcceptChanges();
                fileFunctions.CreateCSVFromDataTable(workTable, SaetbAutoGenMembersNtb258ENewsletterFilePath, false);
                GC.Collect();
            }
        }

        public static void AddAllPubCodes(DataRow dataRow, IList<string> pubCodes)
        {
            Guard.NotNull(dataRow, nameof(dataRow));
            Guard.NotNull(pubCodes, nameof(pubCodes));

            ClientMethodHelpers.AddPubCode(dataRow["IMS_Program01"].ToString(), pubCodes, "TBMIN");
            ClientMethodHelpers.AddPubCode(dataRow["IMS_Program02"].ToString(), pubCodes, "TBDIN");
            ClientMethodHelpers.AddPubCode(dataRow["IMS_Program04"].ToString(), pubCodes, "TBMCIN");
            ClientMethodHelpers.AddPubCode(dataRow["IMS_Program06"].ToString(), pubCodes, "TBMDIN");
            ClientMethodHelpers.AddPubCode(dataRow["IMS_Program07"].ToString(), pubCodes, "TBECIN");
            ClientMethodHelpers.AddPubCode(dataRow["IMS_Program08"].ToString(), pubCodes, "TBPIN");
            ClientMethodHelpers.AddPubCode(dataRow["IMS_Program09"].ToString(), pubCodes, "TBPMIN");
            ClientMethodHelpers.AddPubCode(dataRow["IMS_Program10"].ToString(), pubCodes, "TBAIN");
            ClientMethodHelpers.AddPubCode(dataRow["IMS_Program11"].ToString(), pubCodes, "TBTMIN");
        }

        public void SAETB_KNWS0100(
            ClientEntity clientEntity, 
            SourceFile clientSourceFile, 
            ClientCustomProcedure customProcedure, 
            FileMoved eventMessage)
        {
            ClientMethodHelpers.DeleteExistingFiles(Knws0100FilePath);
            ClientMethodHelpers.DeleteExistingFiles(SaetbAutoGenKnws0100FilePath);

            try
            {
                var fcAccess = ClientMethodHelpers.CreateFileConfiguration(
                    KnwS0100ColumnCount, Consts.AddressColumnHeaders, Consts.CommaFileDelimiter, Consts.CsvExtension, true);
                Console.WriteLine(Consts.MessageGettingFile, DateTime.Now);

                var importErrors = new List<ImportErrorEntity>();
                var dtAccess = ProcessImportVesselRows(eventMessage, fcAccess, null, Knws0100Code);

                if (dtAccess != null)
                {
                    var sbDetail = ClientMethodHelpers.CreateEmailHeader(clientEntity, fcAccess, dtAccess);
                    var reportBody = ClientMethodHelpers.AppendImportErrors(fcAccess, importErrors, sbDetail);
                    ClientMethodHelpers.EmailSender(clientEntity, eventMessage, sbDetail, reportBody, AutoGenKnws0100Code);

                    CreateCsvForKnws0100(dtAccess);
                    dtAccess.Dispose();
                }
            }
            // Must take any exception that arises
            catch (Exception ex)
            {
                LogError(ex, clientEntity, $"{GetType().Name}{Consts.DotSepator}{SaetbKnws0100Code}");
            }

            ClientMethodHelpers.FinishingUploadToFtp(clientEntity, AutoGenSaetbKnws0100FileName, SaetbAutoGenKnws0100FilePath);
        }

        public void CreateCsvForKnws0100(DataTable dtAccess)
        {
            Guard.NotNull(dtAccess, nameof(dtAccess));
            var totalWorkRows = dtAccess.Rows.Count;
            var workProcessed = 0;

            var workTable = dtAccess.Clone();
            workTable.Columns.Add("DEMO31");
            workTable.Columns.Add("DEMO32");
            workTable.Columns.Add("DEMO33");
            workTable.Columns.Add("DEMO34");
            workTable.Columns.Add("DEMO35");
            workTable.Columns.Add("DEMO36");

            while (workProcessed < totalWorkRows)
            {
                workTable.Clear();
                var newRows = new Dictionary<int, DataRow>();
                var batchProcessing = workProcessed + Consts.RowBatch;

                if (batchProcessing > totalWorkRows)
                {
                    batchProcessing = totalWorkRows;
                }

                if (totalWorkRows == 0)
                {
                    throw new DivideByZeroException();
                }

                ConsoleMessage($"{Consts.ProcessedStatus}{workProcessed}{Consts.NextBatchStatus}{batchProcessing}", Knws0100Code, false);
                var percent = (int)Math.Round((double)(100 * workProcessed) / totalWorkRows);
                ConsoleMessage($"{percent}%", Knws0100Code, false);
                workProcessed = AddProcessedRows(dtAccess, workTable, newRows, workProcessed, batchProcessing);

                foreach (var row in newRows)
                {
                    workTable.Rows.Add(row.Value);
                }
                workTable.AcceptChanges();

                var fileFunctions = new FileFunctions();
                fileFunctions.CreateCSVFromDataTable(workTable, SaetbAutoGenKnws0100FilePath, false);
                GC.Collect();
            }
            dtAccess.Dispose();
        }

        public int AddProcessedRows(DataTable dtAccess, DataTable workTable, IDictionary<int, DataRow> newRows, int workProcessed, int batchProcessing)
        {
            Guard.NotNull(dtAccess, nameof(dtAccess));
            Guard.NotNull(workTable, nameof(workTable));
            Guard.NotNull(newRows, nameof(newRows));

            for (var i = workProcessed; i < batchProcessing; i++)
            {
                var dataRow = dtAccess.Rows[i];
                var demo31 = string.Empty;
                var demo32 = string.Empty;
                var demo33 = string.Empty;
                var demo34 = string.Empty;
                var demo35 = string.Empty;
                var demo36 = string.Empty;

                if (dataRow[NoEmailKey].ToString().Trim().Equals(Consts.YesCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    demo31 = bool.FalseString; 
                    demo32 = bool.FalseString; 
                    demo33 = bool.FalseString; 
                    demo34 = bool.FalseString; 
                    demo35 = bool.FalseString; 
                    demo36 = bool.FalseString; 
                }
                else if (dataRow[NoEmailKey].ToString().Trim().Equals(Consts.NoCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    demo31 = bool.TrueString; 
                    demo32 = bool.TrueString; 
                    SetConditionsForNoEmail(dataRow, ref demo33, out demo34, ref demo35, out demo36);
                }

                if (!string.IsNullOrWhiteSpace(dataRow[Consts.PubCodeUpperKey]?.ToString()))
                {
                    var pubCodes = new List<string>();
                    pubCodes = dataRow[Consts.PubCodeKey].ToString().Split(Consts.CommaSeparator).ToList();
                    foreach (var pub in pubCodes)
                    {
                        AddRow(pub.Trim(), demo31, demo32, demo33, demo34, demo35, demo36, newRows.Count + 1,
                            newRows as Dictionary<int, DataRow>, workTable, dataRow);
                    }
                }
                workProcessed++;
            }
            return workProcessed;
        }

        public static void SetConditionsForNoEmail(DataRow dataRow, ref string demo33, out string demo34, ref string demo35, out string demo36)
        {
            Guard.NotNull(dataRow, nameof(dataRow));

            if (string.IsNullOrWhiteSpace(dataRow[NoTelemktg].ToString().Trim()))
            {
                demo33 = bool.TrueString; 
            }
            else
            {
                if (dataRow[NoTelemktg].ToString().Trim().Equals(Consts.NoCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    demo33 = bool.TrueString; 
                }
                else if (dataRow[NoTelemktg].ToString().Trim().Equals(Consts.YesCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    demo33 = bool.FalseString;
                }
            }

            demo34 = true.ToString();
            if (string.IsNullOrWhiteSpace(dataRow[NoAdvEmailKey].ToString().Trim()))
            {
                demo35 = bool.TrueString; 
            }
            else
            {
                if (dataRow[NoAdvEmailKey].ToString().Trim().Equals(Consts.NoCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    demo35 = bool.TrueString; 
                }
                else if (dataRow[NoAdvEmailKey].ToString().Trim().Equals(Consts.YesCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    demo35 = bool.FalseString;
                }
            }
            demo36 = bool.TrueString; 
        }

        public void SAETB_MYSAE(ClientEntity clientEntity, SourceFile clientSpecialFile, ClientCustomProcedure clientCustomProcedure, FileMoved eventMessage)
        {
            ClientMethodHelpers.DeleteExistingFiles(MysaeFilePath);
            ClientMethodHelpers.DeleteExistingFiles(SaetbAutoGenMysaeFilePath);
            var pubCodeAppendList = GetPubCodeList();

            try
            {
                var fcAccess = ClientMethodHelpers.CreateFileConfiguration(
                    MysaeColumnCount, Consts.MysaeColumnHeaders, Consts.CommaFileDelimiter, Consts.CsvExtension, true);
                Console.WriteLine(Consts.MessageGettingFile, DateTime.Now);

                var importErrors = new List<ImportErrorEntity>();
                var dataTableMysae = ProcessImportVesselRows(eventMessage, fcAccess, importErrors, MysaeCode);

                if (dataTableMysae != null)
                {
                    var sbDetail = ClientMethodHelpers.CreateEmailHeader(clientEntity, fcAccess, dataTableMysae);
                    var reportBody = ClientMethodHelpers.AppendImportErrors(fcAccess, importErrors, sbDetail);
                    ClientMethodHelpers.EmailSender(clientEntity, eventMessage, sbDetail, reportBody, AutoGenMysaeCode);

                    CreateCsvForMysae(pubCodeAppendList, dataTableMysae);
                    dataTableMysae.Dispose();
                }
            }
            // Must take any exception that arises
            catch (Exception ex)
            {
                LogError(ex, clientEntity, $"{GetType().Name}{Consts.DotSepator}{SaetbMysaeCode}");
            }

            ClientMethodHelpers.FinishingUploadToFtp(clientEntity, SaetbAutoGenMysaeFileName, SaetbAutoGenMysaeFilePath);
        }

        public DataTable CreateCsvForMysae(IList<string> pubCodeAppendList, DataTable dataTableMysae)
        {
            Guard.NotNull(pubCodeAppendList, nameof(pubCodeAppendList));
            Guard.NotNull(dataTableMysae, nameof(dataTableMysae));

            var workProcessed = 0;
            var totalWorkRows = dataTableMysae.Rows.Count;
            var workTable = SetWorkTableColumns(dataTableMysae);
            var fileFunctions = new FileFunctions();

            while (workProcessed < totalWorkRows)
            {
                workTable.Clear();

                var newRows = new Dictionary<int, DataRow>();
                var batchProcessing = workProcessed + Consts.RowBatch;
                if (batchProcessing > totalWorkRows)
                {
                    batchProcessing = totalWorkRows;
                }

                ConsoleMessage($"{Consts.ProcessedStatus}{workProcessed}{Consts.NextBatchStatus}{batchProcessing}", MysaeCode, false);
                var percent = (int)Math.Round((double)(100 * workProcessed) / totalWorkRows);
                ConsoleMessage($"{percent}%", MysaeCode, false);

                for (var i = workProcessed; i < batchProcessing; i++)
                {
                    AddProcessedRowsForMysae(pubCodeAppendList, dataTableMysae, workTable, newRows, i);
                    workProcessed++;
                }
                foreach (var row in newRows)
                {
                    workTable.Rows.Add(row.Value);
                }

                workTable.AcceptChanges();
                fileFunctions.CreateCSVFromDataTable(workTable, SaetbAutoGenMysaeFilePath, false);
                GC.Collect();
            }
            return dataTableMysae;
        }

        private static DataTable SetWorkTableColumns(DataTable dataTableMysae)
        {
            Guard.NotNull(dataTableMysae, nameof(dataTableMysae));

            var workTable = dataTableMysae.Clone();
            workTable.Columns.Add("DEMO31");
            workTable.Columns.Add("DEMO32");
            workTable.Columns.Add("DEMO33");
            workTable.Columns.Add("DEMO34");
            workTable.Columns.Add("DEMO35");
            workTable.Columns.Add("DEMO36");
            workTable.Columns.Add("TRANSACTIONID");
            return workTable;
        }

        public void AddProcessedRowsForMysae(
            IList<string> pubCodeAppendList, 
            DataTable dataTableMysae, 
            DataTable workTable, 
            IDictionary<int, DataRow> newRows, 
            int countPosition)
        {
            Guard.NotNull(pubCodeAppendList, nameof(pubCodeAppendList));
            Guard.NotNull(dataTableMysae, nameof(dataTableMysae));
            Guard.NotNull(workTable, nameof(workTable));
            Guard.NotNull(newRows, nameof(newRows));

            var dataRow = dataTableMysae.Rows[countPosition];
            var demo31 = string.Empty;
            var demo32 = string.Empty;
            var demo33 = string.Empty;
            var demo34 = string.Empty;
            var demo35 = string.Empty;
            var demo36 = string.Empty;

            if (dataRow[NoEmailKey].ToString().Trim().Equals(Consts.YesCode, StringComparison.CurrentCultureIgnoreCase))
            {
                demo31 = bool.FalseString;
                demo32 = bool.FalseString;
                demo33 = bool.FalseString;
                demo34 = bool.FalseString;
                demo35 = bool.FalseString;
                demo36 = bool.FalseString;
            }
            else if (dataRow[NoEmailKey].ToString().Trim().Equals(Consts.NoCode, StringComparison.CurrentCultureIgnoreCase))
            {
                demo31 = bool.TrueString;
                demo32 = bool.TrueString;
                demo33 = SetSecondaryMysaeFields(dataRow, demo33, out demo34, out demo35);
                demo36 = bool.TrueString;
            }

            if (!string.IsNullOrWhiteSpace(dataRow[Consts.PubCodeUpperKey].ToString()))
            {
                var pubCodes = new List<string>();
                pubCodes = dataRow[Consts.PubCodeKey].ToString().Split(Consts.CommaSeparator).ToList();
                pubCodes.AddRange(pubCodeAppendList);

                foreach (var pub in pubCodes.Distinct())
                {
                    var pubCode = pub.Trim();
                    var transId = GetTransId(dataRow, pubCode);
                    AddRow(pubCode, transId, demo31, demo32, demo33, demo34, demo35, demo36, newRows.Count + 1,
                        newRows as Dictionary<int, DataRow>, workTable, dataRow);
                }
            }
        }

        public static string SetSecondaryMysaeFields(DataRow dataRow, string demo33, out string demo34, out string demo35)
        {
            Guard.NotNull(dataRow, nameof(dataRow));

            if (string.IsNullOrWhiteSpace(dataRow[NoTelemktg].ToString().Trim()))
            {
                demo33 = bool.TrueString;
            }
            else
            {
                if (dataRow[NoTelemktg].ToString().Trim().Equals(Consts.NoCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    demo33 = bool.TrueString;
                }
                else if (dataRow[NoTelemktg].ToString().Trim().Equals(Consts.YesCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    demo33 = bool.FalseString;
                }
            }

            if (dataRow[Noroweml].ToString().Trim().Equals(Consts.YesCode, StringComparison.CurrentCultureIgnoreCase))
            {
                demo34 = bool.FalseString;
            }
            else if (dataRow[Noroweml].ToString().Trim().Equals(Consts.NoCode, StringComparison.CurrentCultureIgnoreCase))
            {
                demo34 = bool.TrueString;
            }
            else
            {
                demo34 = bool.FalseString;
            }

            if (dataRow[Noroweml3].ToString().Trim().Equals(Consts.YesCode, StringComparison.CurrentCultureIgnoreCase))
            {
                demo35 = bool.FalseString;
            }
            else if (dataRow[Noroweml3].ToString().Trim().Equals(Consts.NoCode, StringComparison.CurrentCultureIgnoreCase))
            {
                demo35 = bool.TrueString;
            }
            else
            {
                demo35 = bool.FalseString;
            }
            return demo33;
        }

        public static string GetTransId(DataRow dataRow, string pubCode)
        {
            Guard.NotNull(dataRow, nameof(dataRow));
            var transId = InitialTransId;

            switch (pubCode)
            {
                case "SNWAE":
                    return SetTransId(dataRow[Consts.SnwaeCode]?.ToString());
                case "SNWAU":
                    return SetTransId(dataRow[Consts.SnwauCode]?.ToString());
                case "SNWOH":
                    return SetTransId(dataRow[Consts.SnwohCode]?.ToString());
                case "SNWTNAE":
                    return SetTransId(dataRow[Consts.SnwtnaeCode]?.ToString());
                case "SNWTNALL":
                    return SetTransId(dataRow[Consts.SnwtnallCode]?.ToString());
                case "SNWTNAU":
                    return SetTransId(dataRow[Consts.SnwtnauCode]?.ToString());
                case "SNWTNHD":
                    return SetTransId(dataRow[Consts.SnwthdCode]?.ToString());
                case "SNWTNOF":
                    return SetTransId(dataRow[Consts.SnwtnofCode]?.ToString());
                case "SAERO":
                    return SetTransId(dataRow["MAG_SAERO"]?.ToString());
                case "SAUTO":
                    return SetTransId(dataRow["MAG_SAUTO"]?.ToString());
                case "SDIGAUTO":
                    return SetTransId(dataRow["MAG_SDIGAUTO"]?.ToString());
                case "SOFH":
                    return SetTransId(dataRow["MAG_SOFH"]?.ToString());
                case "SSAEUP-DOM":
                    return SetTransId(dataRow["MAG_SSAEUP_DOM"]?.ToString());
            }

            if (pubCode.Equals(Consts.SnwtnelCode, StringComparison.CurrentCultureIgnoreCase))
            {
                if (dataRow[Consts.SnwauCode] != null &&
                    dataRow[Consts.SnwtnelCode].ToString().Equals(Consts.NoCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    transId = DefaultTransId;
                }
            }

            return transId;
        }

        private static string SetTransId(string dataRowValue)
        {
            return dataRowValue.Equals(Consts.NoCode, StringComparison.CurrentCultureIgnoreCase) 
                ? DefaultTransId 
                : InitialTransId;
        }

        public static IList<string> GetPubCodeList()
        {
            return new List<string>
            {
                Consts.SnwaeCode,
                Consts.SnwauCode,
                Consts.SnwohCode,
                Consts.SnwtnaeCode,
                Consts.SnwtnallCode,
                Consts.SnwtnauCode,
                Consts.SnwtnelCode,
                Consts.SnwthdCode,
                Consts.SnwtnofCode,
                Consts.SaeroCode,
                Consts.SautoCode,
                Consts.SdigautoCode,
                Consts.SofhCode,
                Consts.SsaeupDomCode
            };
        }

        public void AddSpecificColumnRow(string column, string value, int pos, Dictionary<int, DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;
            newRow.SetField(column, value);
            rows.Add(pos, newRow);
        }
        public void AddRow(string pubCode, int pos, Dictionary<int, DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;
            newRow.SetField("PubCode", pubCode);
            rows.Add(pos, newRow);
        }
        public void AddRow(string pubCode, string tID, string D31, string D32, string D33, string D34, string D35, string D36, int pos, Dictionary<int, DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;
            newRow.SetField("PubCode", pubCode);
            newRow.SetField("TRANSACTIONID", tID);
            newRow.SetField("DEMO31", D31);
            newRow.SetField("DEMO32", D32);
            newRow.SetField("DEMO33", D33);
            newRow.SetField("DEMO34", D34);
            newRow.SetField("DEMO35", D35);
            newRow.SetField("DEMO36", D36);
            rows.Add(pos, newRow);
        }
        public void AddRow(string pubCode, string D31, string D32, string D33, string D34, string D35, string D36, int pos, Dictionary<int, DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;
            newRow.SetField("PubCode", pubCode);
            newRow.SetField("DEMO31", D31);
            newRow.SetField("DEMO32", D32);
            newRow.SetField("DEMO33", D33);
            newRow.SetField("DEMO34", D34);
            newRow.SetField("DEMO35", D35);
            newRow.SetField("DEMO36", D36);
            rows.Add(pos, newRow);
        }
        public void AddRow(int pos, Dictionary<int, DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;            
            rows.Add(pos, newRow);
        }
    }
}
