using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Collections.Specialized;
using ServiceStack.Text;
using System.Linq;
using System.Xml.Linq;
using Core_AMS.Utilities;
using KM.Common;
using KM.Common.Import;
using Microsoft.VisualBasic.FileIO;
using StringFunctions = KM.Common.StringFunctions;

namespace FrameworkUAD.BusinessLogic
{
    public class ImportFile
    {
        public string GetCustomerErrorMessage(Object.ImportFile iv)
        {
            System.Text.StringBuilder sbDetail = new StringBuilder();
            foreach (Entity.ImportError error in iv.ImportErrors)
            {
                if (error.RowNumber > 0)
                    sbDetail.AppendLine("Row Number: " + error.RowNumber.ToString() + "<br/>");

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
        public string GetBadData(Object.ImportFile iv)
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
        public string GetCleanOriginalData(Object.ImportFile iv)
        {
            System.Text.StringBuilder sbDetail = new StringBuilder();
            if (iv != null && iv.DataOriginal != null)
            {
                sbDetail.AppendLine(string.Join(",", iv.HeadersOriginal));
                foreach (var key in iv.DataOriginal.Keys)
                {
                    StringDictionary myRow = iv.DataOriginal[key];
                    StringBuilder bd = new StringBuilder();
                    foreach (KeyValuePair<string, string> kvp in myRow)
                    {
                        bd.Append(kvp.Value + ",");
                    }

                    sbDetail.AppendLine(bd.ToString().TrimEnd(','));
                }
            }
            return sbDetail.ToString();
        }
        public string GetTransformedData(Object.ImportFile iv)
        {
            System.Text.StringBuilder sbDetail = new StringBuilder();
            if (iv != null && iv.DataTransformed != null)
            {
                sbDetail.AppendLine(string.Join(",", iv.HeadersTransformed));
                foreach (var key in iv.DataTransformed.Keys)
                {
                    StringDictionary myRow = iv.DataTransformed[key];
                    StringBuilder bd = new StringBuilder();
                    foreach (KeyValuePair<string, string> kvp in myRow)
                    {
                        bd.Append(kvp.Value + ",");
                    }

                    sbDetail.AppendLine(bd.ToString().TrimEnd(','));
                }
            }

            return sbDetail.ToString();
        }

        public FrameworkUAD.Object.ImportFile ConvertImportVesselToImportFile(FrameworkUAD.Object.ImportVessel importVessel)
        {
            FrameworkUAD.Object.ImportFile newIF = new FrameworkUAD.Object.ImportFile();
            newIF.ProcessFile = importVessel.ImportFile;
            newIF.SourceFileId = importVessel.SourceFileID;
            newIF.TotalRowCount = importVessel.TotalRowCount;
            newIF.ImportedRowCount = importVessel.ImportedRowCount;
            newIF.OriginalRowCount = importVessel.OriginalRowCount;
            newIF.ImportErrorCount = importVessel.ImportErrorCount;
            newIF.HasError = importVessel.HasError;
            newIF.ProcessCode = importVessel.ProcessCode;
            newIF.ImportErrors = importVessel.ImportErrors;
            newIF.TransformedRowCount = importVessel.TransformedRowCount;
            newIF.TransformedRowToOriginalRowMap = importVessel.TransformedRowToOriginalRowMap;

            int counter = 0;
            foreach (string h in importVessel.HeadersTransformed)
            {
                newIF.HeadersTransformed.Add(h.Trim(), counter.ToString());
                counter++;
            }
            counter = 0;
            foreach (string h in importVessel.HeadersOriginal)
            {
                newIF.HeadersTransformed.Add(h.Trim(), counter.ToString());
                counter++;
            }

            newIF.DataOriginal = ConvertToDictionary(importVessel.DataOriginal);
            newIF.DataTransformed = ConvertToDictionary(importVessel.DataTransformed);

            return newIF;
        }
        public Dictionary<int, StringDictionary> ConvertToDictionary(DataTable dt)
        {
            Dictionary<int, StringDictionary> dict = new Dictionary<int, StringDictionary>();
            int rowNumber = 1;
            foreach (DataRow dr in dt.Rows)
            {
                //string[] data = Array.ConvertAll(dr.ItemArray, x => x.ToString());
                //List<string> sList = new List<string>(data);
                StringDictionary drData = new StringDictionary();
                foreach (DataColumn dc in dt.Columns)
                {
                    string columnData = dr[dc.ColumnName].ToString().Trim();
                    string columnName = dc.ColumnName.ToUpper().Trim();
                    drData.Add(columnName, columnData);
                }

                dict.Add(rowNumber, drData);
                rowNumber++;
            }
            return dict;
        }
        public Object.ImportFile GetImportFile(FileInfo fileInfo, FileConfiguration fileConfig = null)
        {
            Core_AMS.Utilities.FileWorker fw = new FileWorker();
            if (fw.IsExcelFile(fileInfo))
                return GetImportFileExcel(fileInfo);
            else if (fw.IsDbfFile(fileInfo))
                return GetImportFileDbf(fileInfo);
            else if (fw.IsJsonFile(fileInfo))
                return GetImportFileJson(fileInfo);
            else if (fw.IsXmlFile(fileInfo))
                return GetImportFileXml(fileInfo);
            else return GetImportFileText(fileInfo, fileConfig);
        }
        private Object.ImportFile GetImportFileExcel(FileInfo fileInfo)
        {
            Object.ImportFile newIV = new Object.ImportFile(fileInfo);
            int errorCounter = 0;
            try
            {
                Core_AMS.Utilities.FileWorker fw = new FileWorker();
                DataTable dtExcel = fw.GetData(fileInfo);
                dtExcel.TableName = "Data";

                newIV.DataOriginal = ConvertToDictionary(dtExcel);
                newIV.HeadersOriginal = fw.GetFileHeadersExcel(fileInfo);
                newIV.OriginalRowCount = dtExcel.Rows.Count;
                newIV.TotalRowCount = dtExcel.Rows.Count;
                newIV.ImportedRowCount = dtExcel.Rows.Count;
                newIV.BadDataOriginalHeaders = newIV.HeadersOriginal.DeepClone();
            }
            catch (Exception exception)
            {
                Entity.ImportError ie = new Entity.ImportError();

                var formatError = StringFunctions.FormatExceptionForHTML(exception);
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
        private Object.ImportFile GetImportFileDbf(FileInfo fileInfo)
        {
            Object.ImportFile newIV = new Object.ImportFile(fileInfo);
            int errorCounter = 0;
            Core_AMS.Utilities.FileWorker fw = new FileWorker();
            try
            {

                DataTable dt = new DataTable();
                OleDbConnection yourConnectionHandler = new OleDbConnection(
                    @"Provider=VFPOLEDB.1;Data Source=" + fileInfo.FullName);

                int startRow = 0;
                int totalRows = FileImporter.RowCount(fileInfo);
                int endRow = 0;

                // Open the connection, and if open successfully, you can try to query it
                while (endRow <= totalRows - 1)
                {
                    endRow = startRow + 2500;

                    yourConnectionHandler.Open();

                    if (yourConnectionHandler.State == ConnectionState.Open)
                    {
                        string mySQL = "SELECT * FROM [" + fileInfo.Name + "] WHERE RECNO() between " + startRow.ToString() + " and " + endRow.ToString();

                        OleDbCommand MyQuery = new OleDbCommand(mySQL, yourConnectionHandler);
                        OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);
                        dt = new DataTable();
                        DA.Fill(dt);

                        yourConnectionHandler.Close();
                    }
                    dt = FileImporter.TrimData(dt);

                    if (startRow == 0)
                        startRow = 1;
                    int rowNumber = startRow;
                    foreach (DataRow dr in dt.Rows)
                    {
                        //string[] data = Array.ConvertAll(dr.ItemArray, x => x.ToString());
                        //newIV.DataOriginal.Add(rowNumber, new List<string>(data));
                        //rowNumber++;

                        StringDictionary drData = new StringDictionary();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            string columnData = dr[dc.ColumnName].ToString().Trim();
                            string columnName = dc.ColumnName.ToUpper().Trim();
                            drData.Add(columnName, columnData);
                        }

                        newIV.DataOriginal.Add(rowNumber, drData);
                        rowNumber++;
                    }

                    startRow = endRow + 1;
                }

            }
            catch (Exception exception)
            {
                Entity.ImportError ie = new Entity.ImportError();

                var formatError = StringFunctions.FormatExceptionForHTML(exception);
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


            newIV.HeadersOriginal = fw.GetFileHeadersDbf(fileInfo);
            newIV.OriginalRowCount = newIV.DataOriginal.Count;
            newIV.TotalRowCount = newIV.DataOriginal.Count;
            newIV.ImportedRowCount = newIV.DataOriginal.Count;
            newIV.BadDataOriginalHeaders = newIV.HeadersOriginal.DeepClone();

            return newIV;
        }

        private Object.ImportFile GetImportFileText(FileInfo file, FileConfiguration fileConfig)
        {
            Guard.NotNull(file, nameof(file));

            fileConfig = FileHelpers.EnsureFileConfiguration(fileConfig);

            const string fileEncoding = "iso-8859-1";
            using (var parser = FileHelpers.CreateTextFieldParser(file, fileConfig, Encoding.GetEncoding(fileEncoding)))
            {
                var resultImportFile = new Object.ImportFile(file);

                var parserInfo = new ParseTextFileInfo
                {
                    ErrorCounter = 0,
                    RowImportCounter = 1,
                    ProcessedCounter = 0,
                    ParserRow = 1
                };

                try
                {
                    FillResultImportFile(file, parser, resultImportFile, parserInfo);
                }
                catch (Exception exception)
                {
                    FormatImportException(exception, file, resultImportFile);
                    parserInfo.ErrorCounter++;
                }

                resultImportFile.ImportedRowCount = parserInfo.RowImportCounter - 1;
                resultImportFile.ImportErrorCount = parserInfo.ErrorCounter;
                resultImportFile.OriginalRowCount = parserInfo.RowImportCounter - 1;

                resultImportFile.TotalRowCount = parserInfo.ProcessedCounter;
                resultImportFile.BadDataOriginalHeaders = resultImportFile.HeadersOriginal.DeepClone();

                return resultImportFile;
            }
        }
        
        private void FillResultImportFile(FileInfo fileInfo, TextFieldParser parser, Object.ImportFile resultImportFile, ParseTextFileInfo parserInfo)
        {
            var firstChar = parser.PeekChars(1);
            if (string.IsNullOrEmpty(firstChar))
            {
                return;
            }

            var headerRow = parser.ReadFields();
            if (headerRow == null || headerRow.Length == 0)
            {
                return;
            }

            var columnOrder = 0;
            foreach (var column in headerRow)
            {
                resultImportFile.HeadersOriginal.Add(column.Trim(), columnOrder.ToString());
                columnOrder++;
            }

            parserInfo.ParserRow = 1;
            while (!parser.EndOfData)
            {
                string[] row = null;
                try
                {
                    row = parser.ReadFields();

                    if (row == null || row.Length == 0)
                    {
                        continue;
                    }

                    ParseFileTextRow(row, fileInfo, resultImportFile, parserInfo);
                }
                catch (Exception e)
                {
                    FormatTextFileRowException(row, fileInfo, resultImportFile, parserInfo, e);
                    break;
                }
                finally
                {
                    parserInfo.ProcessedCounter++;
                    parserInfo.ParserRow++;
                }                
            }
        }

        private void ParseFileTextRow(string[] row, FileSystemInfo fileInfo, Object.ImportFile resultImportFile, ParseTextFileInfo parserInfo)
        {
            var headerColumnCount = resultImportFile.HeadersOriginal.Count;
            var rowColumnCount = row.Length;

            if (headerColumnCount == rowColumnCount)
            {
                var rowData = new StringDictionary();
                foreach (string key in resultImportFile.HeadersOriginal.Keys)
                {
                    var columnIndex = Convert.ToInt32(resultImportFile.HeadersOriginal[key]);
                    var columnData = row.GetValue(columnIndex).ToString().Trim();
                    var columnName = key.Trim();
                    rowData.Add(columnName, columnData);
                }

                resultImportFile.DataOriginal.Add(parserInfo.RowImportCounter, rowData);
                parserInfo.RowImportCounter++;
            }
            else
            {
                var rowData = new StringDictionary();
                foreach (string key in resultImportFile.HeadersOriginal.Keys)
                {
                    var columnIndex = Convert.ToInt32(resultImportFile.HeadersOriginal[key]);
                    if (resultImportFile.HeadersOriginal[key] != null && columnIndex < (row.Length - 1))
                    {
                        var columnData = row.GetValue(columnIndex).ToString().Trim();
                        var columnName = key.Trim();
                        rowData.Add(columnName, columnData);
                    }
                    else
                    {
                        var columnData = string.Empty;
                        var columnName = key.Trim();
                        rowData.Add(columnName, columnData);
                    }
                }
                
                var formatError = $"Expected column count is {headerColumnCount} but this row column count is {rowColumnCount}.<br/>This error is due to row {parserInfo.ParserRow} not having the expected number of columns.<br/>";
                FormatTextFileRowException(row, fileInfo, resultImportFile, parserInfo, null, formatError);
            }
        }

        private void FormatTextFileRowException(
            string[] row, 
            FileSystemInfo fileInfo, 
            Object.ImportFile resultImportFile, 
            ParseTextFileInfo parserInfo, 
            Exception exception,
            string additionalInfo = null)
        {
            var sbErrorText = new StringBuilder()
                .AppendLine($"An error has been detected with your import file: {fileInfo.Name}{fileInfo.Extension}<br/>")
                .AppendLine($"This error is due to row {parserInfo.ParserRow} having an issue.<br/>");

            if (!string.IsNullOrEmpty(additionalInfo))
            {
                sbErrorText.AppendLine(additionalInfo);
            }

            sbErrorText.AppendLine("This row of data has been excluded from your import.<br/>");
            sbErrorText.AppendLine("You may correct this row of data, add it to a new file and resubmit for processing.<br/>");

            var importError = new Entity.ImportError
            {
                IsFatalError = false,
                RowNumber = parserInfo.ParserRow,
                FormattedException = StringFunctions.FormatExceptionForHTML(exception),
                ClientMessage = sbErrorText.ToString()
            };

            if (row != null)
            {
                importError.BadDataRow = string.Join(",", row);
            }

            resultImportFile.HasError = true;
            resultImportFile.ImportErrorCount++;
            resultImportFile.ImportErrors.Add(importError);

            parserInfo.ErrorCounter++;
        }

        private Object.ImportFile GetImportFileJson(FileInfo fileInfo)
        {
            Guard.NotNull(fileInfo, nameof(fileInfo));

            var resultImportFile = new Object.ImportFile(fileInfo);
            var rowCount = 0;

            try
            {
                string json;
                using (var streamReader = new StreamReader(fileInfo.FullName, Encoding.ASCII))
                {
                    json = streamReader.ReadToEnd().Replace(Environment.NewLine, string.Empty);
                }
                
                var itemList = JsonArrayObjects.Parse(json);

                var firstRow = itemList.First();
                var columnOrder = 0;
                foreach (var column in firstRow)
                {
                    if (!resultImportFile.HeadersOriginal.ContainsValue(column.Key.ToUpper().Trim()))
                    {
                        resultImportFile.HeadersOriginal.Add(column.Key.Trim(), columnOrder.ToString());
                        columnOrder++;
                    }
                }

                foreach (var rowData in itemList)
                {
                    FileImportProcessRow(fileInfo, rowData, resultImportFile, ref rowCount);
                }
            }
            catch (Exception exception)
            {
                FormatImportException(exception, fileInfo, resultImportFile);
            }

            resultImportFile.ImportedRowCount = resultImportFile.DataOriginal.Count;
            resultImportFile.OriginalRowCount = resultImportFile.DataOriginal.Count;

            resultImportFile.TotalRowCount = rowCount;
            resultImportFile.BadDataOriginalHeaders = resultImportFile.HeadersOriginal.DeepClone();

            return resultImportFile;
        }

        private void FormatImportException(Exception exception, FileSystemInfo fileInfo, Object.ImportFile resultImportFile)
        {
            var sbError = new StringBuilder()
                .AppendLine($"A fatal error has been detected with your import file: {fileInfo.Name}{fileInfo.Extension}<br/>")
                .AppendLine("The file has been rejected.<br/>")
                .AppendLine("You should correct this file and resubmit for processing.<br/>");

            var importError = new Entity.ImportError
            {
                IsFatalError = true,
                BadDataRow = string.Empty,
                RowNumber = -99,
                FormattedException = StringFunctions.FormatExceptionForHTML(exception),
                ClientMessage = sbError.ToString()
            };

            resultImportFile.HasError = true;
            resultImportFile.ImportErrorCount++;
            resultImportFile.ImportErrors.Add(importError);
        }

        private void FileImportProcessRow(FileSystemInfo fileInfo, JsonObject rowData, Object.ImportFile resultImportFile, ref int rowCount)
        {
            var rowStringData = new StringDictionary();

            foreach (var item in rowData)
            {
                try
                {
                    rowStringData.Add(item.Key.ToUpper().Trim(), item.Value.Trim());
                }
                catch (Exception exception)
                {
                    var sbError = new StringBuilder()
                        .AppendLine($"An error has been detected with your import file: {fileInfo.Name}{fileInfo.Extension}<br/>")
                        .AppendLine($"This error is due to row {rowCount + 1} having an issue.<br/>")
                        .AppendLine("This row of data has been excluded from your import.<br/>")
                        .AppendLine("You may correct this row of data, add it to a new file and resubmit for processing.<br/>");

                    var importError = new Entity.ImportError
                    {
                        IsFatalError = false,
                        BadDataRow = string.Join(",", rowData.Values),
                        RowNumber = rowCount,
                        FormattedException = StringFunctions.FormatExceptionForHTML(exception),
                        ClientMessage = sbError.ToString()
                    };

                    resultImportFile.HasError = true;
                    resultImportFile.ImportErrorCount++;
                    resultImportFile.ImportErrors.Add(importError);
                }
            }

            resultImportFile.DataOriginal.Add(rowCount, rowStringData);
            rowCount++;
        }

        private Object.ImportFile GetImportFileXml(FileInfo fileInfo)
        {
            FrameworkUAD.Object.ImportFile newIf = new FrameworkUAD.Object.ImportFile(fileInfo);
            int errorCounter = 0;
            int rowCount = 1;
            try
            {
                Core_AMS.Utilities.FileWorker fw = new Core_AMS.Utilities.FileWorker();

                newIf.HeadersOriginal = fw.GetFileHeadersXml(fileInfo);//fw.GetFileHeadersExcel(fileInfo);
                //newIV.DataOriginal = ConvertToDictionary(Core_AMS.Utilities.XmlFunctions.CreateDataTableFromXmlFile(fi));
                #region Get Data
                XDocument doc = Core_AMS.Utilities.XmlFunctions.CreateFromFile(fileInfo);

                string parentRootName = doc.Root.Name.ToString();
                string recordRootName = doc.Root.Elements().First().Name.ToString();

                try
                {
                    foreach (var record in doc.Root.Elements(recordRootName))
                    {
                        System.Collections.Specialized.StringDictionary sd = new System.Collections.Specialized.StringDictionary();
                        try
                        {
                            foreach (var element in record.Elements())
                            {
                                if (newIf.HeadersOriginal.ContainsValue(element.Name.ToString().ToUpper().Trim()))
                                    sd.Add(element.Name.ToString().ToUpper().Trim(), element.Value.ToString().Trim());

                            }
                            newIf.DataOriginal.Add(rowCount, sd);
                        }
                        catch (Exception exception)
                        {
                            FrameworkUAD.Entity.ImportError newER = new Entity.ImportError();
                            newER.IsFatalError = false;
                            if (record != null)
                                newER.BadDataRow = record.ToString();

                            newER.RowNumber = rowCount;
                            newER.FormattedException = StringFunctions.FormatExceptionForHTML(exception);
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("An error has been detected with your import file: " + fileInfo.Name + fileInfo.Extension + "<br/>");
                            sb.AppendLine("This error is due to row " + rowCount.ToString() + " having an issue.<br/>");
                            sb.AppendLine("This row of data has been excluded from your import.<br/>");
                            sb.AppendLine("You may correct this row of data, add it to a new file and resubmit for processing.<br/>");
                            newER.ClientMessage = sb.ToString();

                            newIf.HasError = true;
                            newIf.ImportErrorCount++;
                            newIf.ImportErrors.Add(newER);
                            errorCounter++;
                        }
                        rowCount++;
                    }
                }
                catch (Exception exception)
                {
                    FrameworkUAD.Entity.ImportError ie = new FrameworkUAD.Entity.ImportError();

                    var formatError = StringFunctions.FormatExceptionForHTML(exception);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("An error has been detected with your import file: " + fileInfo.Name + fileInfo.Extension + "<br/>");
                    sb.AppendLine("Your file has been rejected for processing due to the following:<br/>");
                    errorCounter++;

                    ie.BadDataRow = null;
                    ie.ClientMessage = sb.ToString();
                    ie.FormattedException = formatError;
                    ie.RowNumber = 0;

                    newIf.HasError = true;
                    newIf.ImportErrorCount = errorCounter;
                    newIf.ImportErrors.Add(ie);
                    errorCounter++;
                }

                //newIV.DataOriginal.Add(rowCounter, sd);
                #endregion
                newIf.ImportedRowCount = newIf.DataOriginal.Count;
                newIf.ImportErrorCount = errorCounter;
                newIf.OriginalRowCount = newIf.DataOriginal.Count;

                newIf.TotalRowCount = rowCount - 1;
                newIf.BadDataOriginalHeaders = newIf.HeadersOriginal.DeepClone();
            }
            catch (Exception exception)
            {
                FrameworkUAD.Entity.ImportError ie = new FrameworkUAD.Entity.ImportError();

                var formatError = StringFunctions.FormatExceptionForHTML(exception);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("An error has been detected with your import file: " + fileInfo.Name + fileInfo.Extension + "<br/>");
                sb.AppendLine("Your file has been rejected for processing due to the following:<br/>");
                errorCounter++;

                ie.BadDataRow = null;
                ie.ClientMessage = sb.ToString();
                ie.FormattedException = formatError;
                ie.RowNumber = 0;

                newIf.HasError = true;
                newIf.ImportErrorCount = errorCounter;
                newIf.ImportErrors.Add(ie);
                errorCounter++;
            }

            return newIf;
        }

        public Object.ImportFile DropUnmappedHeaderColumns(Object.ImportFile dataIV, FrameworkUAS.Entity.SourceFile sourceFile)
        {
            List<string> unknownColumns = new List<string>();
            foreach (var h in dataIV.HeadersOriginal.Keys)
            {
                if (sourceFile.FieldMappings.Count(x => x.IncomingField.Equals(h.ToString(), StringComparison.CurrentCultureIgnoreCase)) == 0)
                {
                    unknownColumns.Add(h.ToString());
                }
            }

            foreach (var h in unknownColumns)
            {
                dataIV.HeadersOriginal.Remove(h);
                foreach (var key in dataIV.DataOriginal.Keys)
                {
                    StringDictionary myRow = dataIV.DataOriginal[key];
                    myRow.Remove(h);
                }
            }
            return dataIV;
        }

        private class ParseTextFileInfo
        {
            public int ErrorCounter { get; set; }
            public int RowImportCounter { get; set; }
            public int ProcessedCounter { get; set; }
            public int ParserRow { get; set; }
        }
    }
}
