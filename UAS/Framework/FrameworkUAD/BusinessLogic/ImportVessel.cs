using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Core_AMS.Utilities;
using KM.Common.Import;

namespace FrameworkUAD.BusinessLogic
{
    public class ImportVessel
    {
        public static readonly string TableCounts = "Counts";
        public static readonly string ColumnRowImportCount = "RowImportCount";
        public static readonly string ColumnRowErrorCount = "RowErrorCount";
        public static readonly string ColumnTotalRows = "TotalRows";
        public static readonly string TableData = "Data";
        public static readonly string TableErrors = "Errors";
        public static readonly string ColumnRowNumber = "RowNumber";
        public static readonly string ColumnBadDataRow = "BadDataRow";
        public static readonly string ColumnClientMessage = "ClientMessage";
        public static readonly string ColumnFormattedError = "FormattedError";

        public static string GetCustomerErrorMessage(Object.ImportVessel iv)
        {
            System.Text.StringBuilder sbDetail = new StringBuilder();
            foreach (Entity.ImportError error in iv.ImportErrors)
            {
                if (error.RowNumber > 0)
                    sbDetail.AppendLine("Row Number: " + error.RowNumber.ToString() + "<br/>");
                //if (error.FormattedException != null && error.FormattedException.Length > 0)
                //    sbDetail.AppendLine(error.FormattedException);
                if (error.BadDataRow != null && error.BadDataRow.Length > 0)
                {
                    string badData = string.Join(", ", error.BadDataRow);
                    sbDetail.AppendLine("Data: " + badData + "<br/>");
                }
                if (error.ClientMessage != null && error.ClientMessage.Length > 0)
                    sbDetail.AppendLine(error.ClientMessage + "<br/>");
                sbDetail.AppendLine(string.Empty);
            }

            return sbDetail.ToString();
        }
        public static string GetBadData(Object.ImportVessel iv)
        {
            System.Text.StringBuilder sbDetail = new StringBuilder();

            if (iv != null && iv.ImportErrors != null)
            {
                sbDetail.AppendLine(string.Join(",", iv.HeadersOriginal));
                foreach (Entity.ImportError error in iv.ImportErrors)
                {
                    if (error.BadDataRow != null && error.BadDataRow.Length > 0)
                    {
                        string badData = string.Join(",", error.BadDataRow);
                        sbDetail.AppendLine(badData);
                    }
                }
            }
            return sbDetail.ToString();
        }
        public static string GetCleanOriginalData(Object.ImportVessel iv)
        {
            System.Text.StringBuilder sbDetail = new StringBuilder();
            if (iv != null && iv.DataOriginal != null)
            {
                sbDetail.AppendLine(string.Join(",", iv.HeadersOriginal));
                foreach (DataRow dr in iv.DataOriginal.Rows)
                {
                    string[] rowArray = dr.ItemArray.OfType<string>().ToArray();
                    string badData = string.Join(",", rowArray);
                    sbDetail.AppendLine(badData);
                }
            }
            return sbDetail.ToString();
        }
        public static string GetTransformedData(Object.ImportVessel iv)
        {
            System.Text.StringBuilder sbDetail = new StringBuilder();
            if (iv != null && iv.DataTransformed != null)
            {
                sbDetail.AppendLine(string.Join(",", iv.HeadersTransformed));
                foreach (DataRow dr in iv.DataTransformed.Rows)
                {
                    string[] rowArray = dr.ItemArray.OfType<string>().ToArray();
                    string badData = string.Join(",", rowArray);
                    sbDetail.AppendLine(badData);
                }
            }

            return sbDetail.ToString();
        }

        public Object.ImportVessel GetImportVessel(FileInfo fileInfo, FileConfiguration fileConfig = null)
        {
            Core_AMS.Utilities.FileWorker fw = new FileWorker();
            if (fw.IsExcelFile(fileInfo))
                return GetImportVesselExcel(fileInfo);
            else return GetImportVesselText(fileInfo, fileConfig);
        }
        public Object.ImportVessel GetImportVessel(FileInfo fileInfo, int startRow, int takeRowCount, FileConfiguration fileConfig = null)
        {
            Core_AMS.Utilities.FileWorker fw = new FileWorker();
            if (fw.IsExcelFile(fileInfo))
                return GetImportVesselExcel(fileInfo);
            else if (fw.IsDbfFile(fileInfo))
                return GetImportVesselDbf(fileInfo, startRow, takeRowCount);
            else return GetImportVesselText(fileInfo, startRow, takeRowCount, fileConfig);
        }
        public Object.ImportVessel GetImportVesselExcel(FileInfo fileInfo)
        {
            Object.ImportVessel newIV = new Object.ImportVessel(fileInfo);
            int errorCounter = 0;

            //FileStream stream = System.IO.File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            //IExcelDataReader excelReader = null;
            try
            {
                Core_AMS.Utilities.FileWorker fw = new FileWorker();
                DataTable dtExcel = fw.GetData(fileInfo);//excelReader.AsDataSet().Tables[0].Copy();
                dtExcel.TableName = "Data";

                newIV.DataOriginal = dtExcel;
                newIV.OriginalRowCount = dtExcel.Rows.Count;
                newIV.TotalRowCount = dtExcel.Rows.Count;
            }
            catch (Exception exception)
            {
                Entity.ImportError ie = new Entity.ImportError();

                var formatError = StringFunctions.FormatExceptionForHtml(exception);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("An error has been detected with your import file: " + fileInfo.Name + fileInfo.Extension + "<br/>");
                sb.AppendLine("Your file has been rejected for processing due to the following:<br/>");
                errorCounter++;

                ie.BadDataRow = null;
                ie.ClientMessage = sb.ToString();
                ie.FormattedException = formatError;
                ie.RowNumber = 0;

                newIV.HasError = true;
                newIV.ImportErrorCount = errorCounter;
                newIV.ImportErrors.Add(ie);
            }

            return newIV;
        }

        public Object.ImportVessel GetImportVesselDbf(FileInfo fileInfo)
        {
            var result = GetImportVesselDbfData(fileInfo);
            return result;
        }

        public Object.ImportVessel GetImportVesselDbf(FileInfo fileInfo, int startRow, int takeRowCount)
        {
            var result = GetImportVesselDbfData(fileInfo, startRow, takeRowCount);
            return result;
        }

        public Object.ImportVessel GetImportVesselDbfData(FileInfo fileInfo, int? startRow = null, int? takeRowCount = null)
        {
            var newImportVessel = new Object.ImportVessel(fileInfo);

            var table = new DataTable();
            try
            {
                table = startRow.HasValue && takeRowCount.HasValue ?
                    FileImporter.ConvertFoxProDBFToDataTable(fileInfo, startRow.Value, takeRowCount.Value) :
                    FileImporter.ConvertFoxProDBFToDataTable(fileInfo);
            }
            catch (Exception exception)
            {
                var formatError = StringFunctions.FormatExceptionForHtml(exception);
                var builder = new StringBuilder();
                builder.AppendLine(
                    $"An error has been detected with your import file: {fileInfo.Name}{fileInfo.Extension}<br/>");
                builder.AppendLine("Your file has been rejected for processing due to the following:<br/>");

                var error = new Entity.ImportError
                {
                    BadDataRow = null,
                    ClientMessage = builder.ToString(),
                    FormattedException = formatError,
                    RowNumber = 0
                };

                newImportVessel.HasError = true;
                newImportVessel.ImportErrorCount = 1;
                newImportVessel.ImportErrors.Add(error);
            }

            var tableDbf = table;
            tableDbf.TableName = TableData;

            newImportVessel.DataOriginal = tableDbf;
            newImportVessel.OriginalRowCount = tableDbf.Rows.Count;
            newImportVessel.TotalRowCount = tableDbf.Rows.Count;

            return newImportVessel;
        }

        public Object.ImportVessel GetImportVesselText(FileInfo fileInfo, FileConfiguration fileConfig)
        {
            return LoadFileImportVessel(fileInfo, fileConfig);
        }
        public Object.ImportVessel GetImportVesselText(FileInfo fileInfo, int startRow, int takeRowCount, FileConfiguration fileConfig)
        {
            return LoadFileImportVessel(fileInfo, startRow, takeRowCount, fileConfig);
        }

        public Object.ImportVessel LoadFileImportVessel(FileInfo file, FileConfiguration fileConfig)
        {
            var result = LoadFileImportVesselData(file, fileConfig);
            return result;
        }

        public Object.ImportVessel LoadFileImportVessel(FileInfo file, int startRow, int takeRowCount, FileConfiguration fileConfig)
        {
            var result = LoadFileImportVesselData(file, fileConfig, startRow, takeRowCount);
            return result;
        }

        public Object.ImportVessel LoadFileImportVesselData(
            FileInfo file,
            FileConfiguration fileConfig,
            int? startRow = null,
            int? takeRowCount = null)
        {
            var newImportVessel = new Object.ImportVessel(file);

            var dataSet = startRow.HasValue && takeRowCount.HasValue ?
                FileImporter.LoadFileDataSet(file, startRow.Value, takeRowCount.Value, fileConfig) :
                FileImporter.LoadFileDataSet(file, fileConfig);

            int importRowCount;
            int.TryParse(dataSet.Tables[TableCounts].Rows[0][ColumnRowImportCount].ToString(), out importRowCount);
            int errorCounter;
            int.TryParse(dataSet.Tables[TableCounts].Rows[0][ColumnRowErrorCount].ToString(), out errorCounter);
            int rowCounter;
            int.TryParse(dataSet.Tables[TableCounts].Rows[0][ColumnTotalRows].ToString(), out rowCounter);

            newImportVessel.DataOriginal = dataSet.Tables[TableData];
            newImportVessel.OriginalRowCount = importRowCount;
            newImportVessel.ImportErrorCount = errorCounter;
            newImportVessel.TotalRowCount = rowCounter;
            newImportVessel.ImportFile = file;

            if (errorCounter > 0)
            {
                foreach (DataRow row in dataSet.Tables[TableErrors].Rows)
                {
                    int errorRow;
                    int.TryParse(row[ColumnRowNumber].ToString(), out errorRow);
                    var importError = new Entity.ImportError
                    {
                        BadDataRow = row[ColumnBadDataRow].ToString(),
                        ClientMessage = row[ColumnClientMessage].ToString(),
                        FormattedException = row[ColumnFormattedError].ToString(),
                        RowNumber = errorRow
                    };

                    newImportVessel.HasError = true;
                    newImportVessel.ImportErrors.Add(importError);
                }
            }
            return newImportVessel;
        }
    }
}
