using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ADMS.ClientMethods.Common;
using FrameworkUAS.Entity;
using KM.Common;
using ClientEntity = KMPlatform.Entity.Client;
using ClientMethodsBusiness = FrameworkUAS.BusinessLogic.ClientMethods;
using FileFunctions = Core_AMS.Utilities.FileFunctions;

namespace ADMS.ClientMethods
{
    public class Vcast : ClientSpecialCommon
    {
        private const string AutoGenVcastMxBooksFileName = "AutoGen_Vcast_MXBooks.csv";
        private const string VcastAutoGenMxBooksFilePath = "C:\\ADMS\\Client Archive\\VCAST\\Vcast_AutoGen_MXBooks.csv";
        private const string AutoGenVcastMxElanBookFileName = "AutoGen_Vcast_MXElanBook.csv";
        private const string VcastAutoGenMxElanBookFilePath = "C:\\ADMS\\Client Archive\\VCAST\\Vcast_AutoGen_MXElanBook.csv";

        private const string VcastMxBooksCode = "Vcast_MX_BOOKS";
        private const string VcastMxElanBookCode = "Vcast_MX_ELANBOOK";
        private const string VcastProcessFileMxBooks = "Vcast_Process_File_MX_Books";
        private const string VcastProcessFileMxElan = "Vcast_Process_File_MX_Elan";

        public void Vcast_MX_BOOKS(ClientEntity clientEntity, FileInfo fileInfo, SourceFile clientSpecialFile)
        {
            ClientMethodHelpers.DeleteExistingFiles(VcastAutoGenMxBooksFilePath);
            var clientMethodsData = new ClientMethodsBusiness();
            clientMethodsData.Vcast_CreateTempMXBooksTable();

            try
            {
                var fcAccess = ClientMethodHelpers.CreateFileConfiguration(null, string.Empty, Consts.TabColumnDelimiter, Consts.TxtExtension, true);
                Console.WriteLine(Consts.MessageGettingFile, DateTime.Now);

                var dtAccess = ClientMethodHelpers.ProcessImportVesselData(fileInfo, fcAccess);
                Console.WriteLine($"{Consts.ProcessStarted} {VcastProcessFileMxBooks} {0}", DateTime.Now);
                clientMethodsData.Vcast_Process_File_MX_Books(dtAccess);
                Console.WriteLine($"{Consts.ProcessFinished} {VcastProcessFileMxBooks} {0}", DateTime.Now);

                if (dtAccess != null)
                {
                    var distinctValues = clientMethodsData.Vcase_Get_Distinct_MX_Books();
                    CreateCsvForVcast(dtAccess, distinctValues, VcastMxBooksCode, VcastAutoGenMxBooksFilePath);
                }
            }
            // Must take any exception that arises
            catch (Exception ex)
            {
                LogError(ex, clientEntity, $"{GetType().Name}{Consts.DotSepator}{VcastMxBooksCode}");
            }

            clientMethodsData.Vcast_DropTempMXBooksTable();
            ClientMethodHelpers.FinishingUploadToFtp(clientEntity, AutoGenVcastMxBooksFileName, VcastAutoGenMxBooksFilePath);
        }

        public void Vcast_MX_ELANBOOK(ClientEntity clientEntity, FileInfo fileInfo, SourceFile clientSpecialFile)
        {
            ClientMethodHelpers.DeleteExistingFiles(VcastAutoGenMxElanBookFilePath);
            var clientMethodsData = new ClientMethodsBusiness();
            clientMethodsData.Vcast_CreateTempMXElanTable();

            try
            {
                var fcAccess = ClientMethodHelpers.CreateFileConfiguration(null, string.Empty, Consts.TabColumnDelimiter, Consts.TxtExtension, true);
                Console.WriteLine(Consts.MessageGettingFile, DateTime.Now);

                var dtAccess = ClientMethodHelpers.ProcessImportVesselData(fileInfo, fcAccess);
                Console.WriteLine($"{Consts.ProcessStarted} {VcastProcessFileMxElan} {0}", DateTime.Now);
                clientMethodsData.Vcast_Process_File_MX_Elan(dtAccess);
                Console.WriteLine($"{Consts.ProcessFinished} {VcastProcessFileMxElan} {0}", DateTime.Now);

                if (dtAccess != null)
                {
                    var distinctValues = clientMethodsData.Vcase_Get_Distinct_MX_Elan();
                    CreateCsvForVcast(dtAccess, distinctValues, VcastMxElanBookCode, VcastAutoGenMxBooksFilePath);
                }
            }
            // Must take any exception that arises
            catch (Exception ex)
            {
                LogError(ex, clientEntity, $"{GetType().Name}{Consts.DotSepator}{VcastMxElanBookCode}");
            }

            clientMethodsData.Vcast_DropTempMXElanTable();
            ClientMethodHelpers.FinishingUploadToFtp(clientEntity, AutoGenVcastMxElanBookFileName, VcastAutoGenMxElanBookFilePath);
        }

        public void CreateCsvForVcast(DataTable dtAccess, DataTable distinctValues, string processCode, string filePath)
        {
            Guard.NotNull(dtAccess, nameof(dtAccess));
            Guard.NotNull(distinctValues, nameof(distinctValues));

            var accessKeys = new DataColumn[1];
            accessKeys[0] = distinctValues.Columns["SubNum"];
            distinctValues.PrimaryKey = accessKeys;
            distinctValues.AcceptChanges();
            var totalWorkRows = dtAccess.Rows.Count;
            var workProcessed = 0;
            var workTable = dtAccess.Clone();

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

                ConsoleMessage($"{Consts.NewBatchStatus}{batchProcessing}", processCode, false);
                ConsoleMessage($"{Consts.ProcessedStatus}{workProcessed}", processCode, false);

                for (var i = workProcessed; i < batchProcessing; i++)
                {
                    var currentDataRow = dtAccess.Rows[workProcessed];
                    var match = distinctValues.Rows.Find(currentDataRow["SUBSCRBNUM"].ToString());

                    AddNewRows(workTable, newRows, currentDataRow, match);

                    deleteRows.Add(rowIndex, currentDataRow);
                    rowIndex++;
                    workProcessed++;
                }
                foreach (var row in newRows)
                {
                    workTable.Rows.Add(row.Value);
                }
                workTable.AcceptChanges();

                var fileFunctions = new FileFunctions();
                fileFunctions.CreateCSVFromDataTable(workTable, filePath, false);
                GC.Collect();
            }
            dtAccess.Dispose();
        }

        public void AddNewRows(DataTable workTable, IDictionary<int, DataRow> newRows, DataRow currentDataRow, DataRow match)
        {
            Guard.NotNull(workTable, nameof(workTable));
            Guard.NotNull(newRows, nameof(newRows));
            Guard.NotNull(currentDataRow, nameof(currentDataRow));

            if (match != null)
            {
                var so2 = match["SO2"].ToString();
                var so3 = match["SO3"].ToString();

                var newRow = workTable.NewRow();
                newRow.ItemArray = currentDataRow.ItemArray;
                newRow.SetField("SUB02ANS", so2);
                newRow.SetField("SUB03ANS", so3);
                newRows.Add(newRows.Count + 1, newRow);
            }
            else
            {
                var newRow = workTable.NewRow();
                newRow.ItemArray = currentDataRow.ItemArray;
                newRows.Add(newRows.Count + 1, newRow);
            }
        }
    }
}
