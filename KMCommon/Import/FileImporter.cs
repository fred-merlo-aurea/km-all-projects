using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Linq;
using KM.Common.Functions;
using Microsoft.VisualBasic.FileIO;

namespace KM.Common.Import
{
    public class FileImporter
    {
        private const string TableErrors = "Errors";
        private const string TableData = "Data";
        private const string TableCounts = "Counts";

        private const string TableErrorFieldBadRowData = "BadDataRow";
        private const string TableErrorFieldRowNumber = "RowNumber";
        private const string TableErrorFieldFormattedError = "FormattedError";
        private const string TableErrorFieldClientMessage = "ClientMessage";
        private const string TableErrorFielIsFatalError = "IsFatalError";

        private const string TableCountFieldTotalRows = "TotalRows";
        private const string TableCountFieldRowImportCount = "RowImportCount";
        private const string TableCountFieldRowErrorCount = "RowErrorCount";

        #region old methods
        private static void ConstructSchema(string physicalDataPath, string fileName)
        {
            FileInfo theFile = new FileInfo(physicalDataPath + "\\" + fileName);

            StringBuilder schema = new StringBuilder();
            DataTable data = LoadCSV(theFile);
            schema.AppendLine("[" + theFile.Name + "]");
            schema.AppendLine("ColNameHeader=True");
            for (int i = 0; i < data.Columns.Count; i++)
            {
                schema.AppendLine("col" + (i + 1).ToString() + "=" + data.Columns[i].ColumnName.Replace(" ", "") + " Text");
            }
            string schemaFileName = theFile.DirectoryName + @"\Schema.ini";
            TextWriter tw = new StreamWriter(schemaFileName);
            tw.WriteLine(schema.ToString());
            tw.Close();
        }
        private static DataTable LoadCSV(FileInfo theFile)
        {
            string sqlString = "Select * FROM [" + theFile.Name + "];";
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + theFile.DirectoryName + ";" + "Extended Properties='text;HDR=YES;'";
            if (System.Environment.Is64BitOperatingSystem == true)
            {
                try
                {
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + theFile.DirectoryName + "; Extended Properties='text;HDR=Yes;'";
                }
                catch
                {
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + theFile.DirectoryName + "; Extended Properties='text;HDR=Yes;'";
                }
            }
            DataTable theCSV = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(conStr))
            {
                using (OleDbCommand comm = new OleDbCommand(sqlString, conn))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(comm))
                    {
                        adapter.Fill(theCSV);
                    }
                }
            }
            return theCSV;
        }
        public static DataTable GetDataTableByFileType(string physicalDataPath, string fileType, string fileName, string excelSheetName, int startLine, int maxRecordsToRetrieve, string delimiter)
        {
            string connString = "";

            if (fileType == "X")
            {			// load data from Excel file				
                connString = GetDataSource(physicalDataPath, fileName, fileType);
                OleDbDataAdapter oleAdapter = new OleDbDataAdapter("SELECT * FROM [" + excelSheetName + "$]", connString);
                DataSet dataset = new DataSet();
                oleAdapter.Fill(dataset, startLine, maxRecordsToRetrieve, "ExcelTable"); //fill the adapter with rows only from the linenumber specified															
                return dataset.Tables["ExcelTable"];
            }
            else if ((fileType == "C"))
            {
                //create a schema to use before filling the adapter
                ConstructSchema(physicalDataPath, fileName);
                connString = GetDataSource(physicalDataPath, fileName, fileType);
                OleDbDataAdapter oleAdapter = new OleDbDataAdapter("SELECT * FROM " + fileName + " ", connString);
                DataSet dataset = new DataSet(fileName);
                oleAdapter.Fill(dataset, startLine, maxRecordsToRetrieve, fileName); //fill the adapter with rows only from the linenumber specified														
                return dataset.Tables[fileName];
            }
            else if ((fileType == "O"))
            {
                return getDatafromTXT(physicalDataPath, fileName, startLine, maxRecordsToRetrieve, delimiter).Tables[0];
            }
            else if (fileType.ToUpper() == "XML")
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml(System.IO.Path.Combine(physicalDataPath, fileName));
                return dataset.Tables[0];
            }
            throw new ArgumentException(string.Format("Unknown file type '{0}'.", fileType));
        }
        public static DataSet getDatafromTXT(string physicalDataPath, string fileName, int startLine, int maxRecordsToRetrieve, string delimiterBy)
        {
            var dataSet = ImportHelper.GetDatafromTxt(
                physicalDataPath,
                fileName,
                startLine,
                maxRecordsToRetrieve,
                delimiterBy);
            return dataSet;
        }

        public static string GetDataSource(string path, string fileName, string filetype)
        {
            var connStr = ImportHelper.GetDataSource(path, fileName, filetype);
            return connStr;
        }
        #endregion

        public static DataTable LoadFileTopRows(FileInfo file, FileConfiguration fileConfig, int rowsToGet = 1)
        {
            fileConfig = FileHelpers.EnsureFileConfiguration(fileConfig);
            var tfp = FileHelpers.CreateTextFieldParser(file, fileConfig);

            var ds = CreateDataSet(false);

            int rowCounter = 1;
            int errorCounter = 0;
            int rowImportCounter = 0;
            try
            {
                String[] stringRow = tfp.ReadFields();
                foreach (String field in stringRow)
                {
                    ds.Tables[0].Columns.Add(field.Trim(), Type.GetType("System.String"));
                }
                //populate with data:

                while (rowCounter <= rowsToGet)
                {
                    try
                    {
                        stringRow = tfp.ReadFields();
                        ds.Tables[0].Rows.Add(stringRow);
                        rowImportCounter++;
                    }
                    catch (Exception rowError)
                    {
                        var clientMessage = GetErrorRowHasWrongMoreColumns(file, rowCounter);
                        var errorArgs = new ErrorArgs(
                            ds.Tables[TableErrors], rowError, clientMessage, stringRow, rowCounter);
                        AddError(errorArgs);

                        errorCounter++;
                    }
                    rowCounter++;

                    if (tfp.LineNumber == -1)
                    {
                        break;
                    }
                }

                tfp.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally { tfp.Close(); }

            FillCountsTable(ds.Tables[TableCounts], rowCounter, rowImportCounter, errorCounter);

            return ds.Tables[TableData];
        }

        #region FoxPro Dbf files
        public static DataTable TrimData(DataTable dt)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                dc.ColumnName = dc.ColumnName.Trim();
            }
            dt.AcceptChanges();

            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    dr[dc.ColumnName] = dr[dc.ColumnName].ToString().Trim();
                }
            }
            dt.AcceptChanges();
            return dt;
        }
        public static DataTable FoxProDBF(FileInfo file)
        {
            //download for OLEDB FoxPro 9.0
            //http://www.microsoft.com/en-us/download/details.aspx?id=14839


            //http://social.msdn.microsoft.com/Forums/en-US/c69b0b4b-f801-49a0-bb34-75d9a42b6b07/how-to-read-dbf-file-in-visual-cnet?forum=adodotnetdataproviders
            var dt = new DataTable();

            //"Provider=VFPOLEDB.1;Data Source=" + FullPathToDatabase
            OleDbConnection yourConnectionHandler = new OleDbConnection(
                @"Provider=VFPOLEDB.1;Data Source=C:\Users\PC1\Documents\Visual FoxPro Projects\");
            //"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\db\db.dbf;Extended Properties=dBASE IV;User ID=Admin;Password=" 

            // if including the full dbc (database container) reference, just tack that on
            //      OleDbConnection yourConnectionHandler = new OleDbConnection(
            //          "Provider=VFPOLEDB.1;Data Source=C:\\SomePath\\NameOfYour.dbc;" );


            // Open the connection, and if open successfully, you can try to query it
            yourConnectionHandler.Open();

            if (yourConnectionHandler.State == ConnectionState.Open)
            {
                string mySQL = "select * from CLIENTS";  // dbf table name  FileName.dbf

                OleDbCommand MyQuery = new OleDbCommand(mySQL, yourConnectionHandler);
                OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);

                DA.Fill(dt);

                yourConnectionHandler.Close();
            }
            dt = TrimData(dt);
            return dt;
        }
        public static DataTable ConvertFoxProDBFToDataTable(FileInfo file)
        {
            DataTable dt = new DataTable();


            OleDbConnection yourConnectionHandler = new OleDbConnection(
                @"Provider=VFPOLEDB.1;Data Source=" + file.FullName);

            // Open the connection, and if open successfully, you can try to query it
            yourConnectionHandler.Open();

            if (yourConnectionHandler.State == ConnectionState.Open)
            {
                string mySQL = "SELECT * FROM [" + file.Name + "]";

                OleDbCommand MyQuery = new OleDbCommand(mySQL, yourConnectionHandler);
                OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);

                DA.Fill(dt);

                yourConnectionHandler.Close();
            }

            dt = TrimData(dt);
            return dt;
        }
        public static DataTable ConvertFoxProDBFToDataTable(FileInfo file, int startRow, int takeRowCount)
        {
            DataTable dt = new DataTable();
            OleDbConnection yourConnectionHandler = new OleDbConnection(
                @"Provider=VFPOLEDB.1;Data Source=" + file.FullName);
            if (startRow == 1)
                startRow = 0;
            // Open the connection, and if open successfully, you can try to query it
            yourConnectionHandler.Open();
            int endRow = startRow + takeRowCount;
            if (yourConnectionHandler.State == ConnectionState.Open)
            {
                string mySQL = "SELECT * FROM [" + file.Name + "] WHERE RECNO() between " + startRow.ToString() + " and " + endRow.ToString();

                OleDbCommand MyQuery = new OleDbCommand(mySQL, yourConnectionHandler);
                OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);

                DA.Fill(dt);

                yourConnectionHandler.Close();
            }
            dt = TrimData(dt);
            return dt;
        }
        public void ReadDBFUsingOdbc()
        {
        }

        #endregion

        /// <summary>
        /// Send a CSV or TXT file, will return 3 DataTables, in FileConfiguration object define file delimiter and if quote encapsulated.
        /// Assumes first row are Column Headers.
        /// Only required properties on FileConfigruation are FileColumnDelimiter and IsQuoteEncapsulated
        /// DataTables: Data - Errors - Counts
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileConfig"></param>
        /// <returns>ErrorTable</returns>
        public static DataTable LoadFile(FileInfo file, FileConfiguration fileConfig)
        {
            fileConfig = FileHelpers.EnsureFileConfiguration(fileConfig);
            var textFieldParser = FileHelpers.CreateTextFieldParser(file, fileConfig);

            var dataSet = CreateDataSet();

            ParseFile(true, textFieldParser, dataSet, file, 1);

            return dataSet.Tables[TableData];
        }

        public static DataSet GetDataSet(FileInfo file, FileConfiguration fileConfig)
        {
            fileConfig = FileHelpers.EnsureFileConfiguration(fileConfig);
            var textFieldParser = FileHelpers.CreateTextFieldParser(file, fileConfig);

            var dataSet = CreateDataSet();

            ParseFile(true, textFieldParser, dataSet, file, 1);

            return dataSet;
        }
        public static DataSet GetDataSetNoHeader(FileInfo file, FileConfiguration fileConfig)
        {
            fileConfig = FileHelpers.EnsureFileConfiguration(fileConfig);
            var textFieldParser = FileHelpers.CreateTextFieldParser(file, fileConfig);

            var dataSet = CreateDataSet();

            ParseFile(false, textFieldParser, dataSet, file);
            return dataSet;
        }

        /// <summary>
        /// Send a CSV or TXT file, will return 3 DataTables, in FileConfiguration object define file delimiter and if quote encapsulated.
        /// Assumes first row are Column Headers.
        /// Only required properties on FileConfigruation are FileColumnDelimiter and IsQuoteEncapsulated
        /// DataTables: Data - Errors - Counts
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileConfig"></param>
        /// <returns>ErrorTable</returns>
        public static DataSet LoadFileDataSet(FileInfo file, FileConfiguration fileConfig)
        {
            fileConfig = FileHelpers.EnsureFileConfiguration(fileConfig);
            var textFieldParser = FileHelpers.CreateTextFieldParser(file, fileConfig);

            var dataSet = CreateDataSet();

            ParseFile(true, textFieldParser, dataSet, file);

            return dataSet;
        }

        public static DataSet LoadFileDataSet(FileInfo file, int startRow, int takeRowCount, FileConfiguration fileConfig)
        {
            fileConfig = FileHelpers.EnsureFileConfiguration(fileConfig);

            var errorCounter = 0;
            var processedCounter = 0;
            var dataSet = CreateDataSet(addFatalError: true);
    
            try
            {
                processedCounter = ParseColumns(
                    dataSet, 
                    file,
                    fileConfig,
                    startRow, 
                    takeRowCount, 
                    ref errorCounter);
            }
            catch (Exception exception)
            {
                var clientMessage = GetErrorBatchRow(file, startRow, startRow + takeRowCount);
                var errorArgs = new ErrorArgs(dataSet.Tables[TableErrors], exception, clientMessage, fatalError: true);
                AddError(errorArgs);

                errorCounter++;
            }
    
            var newCountRow = dataSet.Tables[TableCounts].NewRow();
            newCountRow[TableCountFieldTotalRows] = processedCounter - startRow;
            newCountRow[TableCountFieldRowImportCount] = dataSet.Tables[0].Rows.Count;
            newCountRow[TableCountFieldRowErrorCount] = errorCounter;
            dataSet.Tables[TableCounts].Rows.Add(newCountRow);
            
            return dataSet;
        }

        private static int ParseColumns(
            DataSet dataSet,
            FileInfo file,
            FileConfiguration fileConfig,
            int startRow, 
            int takeRowCount,  
            ref int errorCounter)
        {
            Guard.NotNull(dataSet, nameof(dataSet));

            var processedCounter = 1;
            using (var textFieldParser = FileHelpers.CreateTextFieldParser(file, fileConfig))
            {                
                var peekChars = textFieldParser.PeekChars(1);
                if (peekChars == null || peekChars.Length <= 0)
                {
                    return processedCounter;
                }

                var rowNumber = 1;
                var stringRow = textFieldParser.ReadFields();
                try
                {
                    if (stringRow != null && stringRow.Length > 0)
                    {
                        foreach (var field in stringRow)
                        {
                            dataSet.Tables[0].Columns.Add(field.Trim(), typeof(string));
                        }

                        rowNumber = 1;
                        while (processedCounter < (takeRowCount + startRow))
                        {
                            processedCounter++;
                            var parsingSuccessfull = ParseRow(
                                dataSet,
                                file,
                                textFieldParser,
                                startRow,
                                rowNumber,
                                ref stringRow,
                                ref errorCounter);

                            if (parsingSuccessfull)
                            {
                                break;
                            }
                            
                            rowNumber++;
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (rowNumber >= startRow)
                    {
                        var clientMessage = GetErrorClientMessage(file);
                        var errorArgs = new ErrorArgs(dataSet.Tables[TableErrors], exception, clientMessage, stringRow);
                        AddError(errorArgs);

                        errorCounter++;
                    }
                }
            }

            return processedCounter;
        }

        private static bool ParseRow(
            DataSet dataSet,
            FileInfo file, 
            TextFieldParser textFieldParser,
            int startRow,
            int rowNumber, 
            ref string[] stringRow, 
            ref int errorCounter)
        {
            Guard.NotNull(dataSet, nameof(dataSet));
            Guard.NotNull(textFieldParser, nameof(textFieldParser));

            try
            {
                stringRow = textFieldParser.ReadFields();
                if (stringRow != null && stringRow.Length > 0)
                {
                    if (rowNumber >= startRow)
                    {
                        ImportRow(dataSet, stringRow, file, rowNumber, ref errorCounter);

                        if (textFieldParser.LineNumber == -1)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if (rowNumber >= startRow)
                {
                    var clientMessage = GetErrorRowIssue(file, rowNumber);
                    var errorArgs = 
                        new ErrorArgs(dataSet.Tables[TableErrors], exception, clientMessage, stringRow, rowNumber, false);
                    AddError(errorArgs);

                    errorCounter++;
                }
            }

            return false;
        }

        private static void ImportRow(
            DataSet dataSet,
            string[] stringRow,
            FileInfo file,
            int rowNumber,
            ref int errorCounter)
        {
            Guard.NotNull(dataSet, nameof(dataSet));
            Guard.NotNull(stringRow, nameof(stringRow));

            try
            {
                var columnCount = dataSet.Tables[0].Columns.Count;
                var rowColumnCount = stringRow.Length;
                if (columnCount == rowColumnCount)
                {
                    dataSet.Tables[0].Rows.Add(stringRow);
                }
                else
                {
                    var formatError = FormatErrorWrongColumn(columnCount, rowColumnCount);
                    var clientMessage = GetErrorRowHasWrongColumnCount(file, formatError, rowNumber);
                    var errorArgs = 
                        new ErrorArgs(dataSet.Tables[TableErrors], formatError, clientMessage, stringRow, rowNumber, false);

                    AddError(errorArgs);

                    errorCounter++;
                }
            }
            catch (Exception rowError)
            {
                var clientMessage = GetErrorRowIssue(file, rowNumber);
                var errorArgs = 
                    new ErrorArgs(dataSet.Tables[TableErrors], rowError, clientMessage, stringRow, rowNumber, false);
                AddError(errorArgs);

                errorCounter++;
            }
        }

        public static List<string> LoadHeaders(FileInfo file, FileConfiguration fileConfig)
        {
            fileConfig = FileHelpers.EnsureFileConfiguration(fileConfig);
            var textFieldParser = FileHelpers.CreateTextFieldParser(file, fileConfig);

            List<string> headers = new List<string>();
            try
            {
                if (textFieldParser != null)
                {
                    String[] stringRow = textFieldParser.ReadFields();
                    if (stringRow != null)
                    {
                        foreach (String field in stringRow)
                        {
                            headers.Add(field.Trim());
                        }

                        textFieldParser.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                textFieldParser.Close();
            }

            return headers;
        }
        public static List<string> LoadDbfHeaders(FileInfo file)
        {
            List<string> headers = new List<string>();
            DataTable dt = new DataTable();
            dt = ConvertFoxProDBFToDataTable(file);
            foreach (DataColumn dc in dt.Columns)
            {
                headers.Add(dc.ColumnName.Trim());
            }

            return headers;
        }
        public static int RowCount(FileInfo file)
        {
            int rowCount = 0;
            if (file.Extension == ".DBF")
            {
                OleDbConnection yourConnectionHandler = new OleDbConnection(
                @"Provider=VFPOLEDB.1;Data Source=" + file.FullName);
                OleDbCommand cmd = new OleDbCommand("SELECT Count(*) FROM [" + file.Name + "]", yourConnectionHandler);
                yourConnectionHandler.Open();
                rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                yourConnectionHandler.Close();
            }
            else
            {
                string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file.DirectoryName + "; Extended Properties='text;HDR=Yes;FMT=Delimited'";
                if (System.Environment.Is64BitOperatingSystem == true)
                {
                    try
                    {
                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file.DirectoryName + "; Extended Properties='text;HDR=Yes;FMT=Delimited'";
                    }
                    catch
                    {
                        conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file.DirectoryName + "; Extended Properties='text;HDR=Yes;FMT=Delimited'";
                    }
                }

                OleDbConnection cn = new OleDbConnection(conString);
                OleDbCommand cmd = new OleDbCommand("SELECT Count(*) FROM [" + file.Name + "]", cn);
                cn.Open();
                rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }

            return rowCount;
        }
        public static DataTable GetBigTable(FileInfo fileInfo, string firstColumn)
        {
            DataTable dt = new DataTable();
            string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileInfo.DirectoryName + "; Extended Properties='text;HDR=Yes;FMT=Delimited'";
            if (System.Environment.Is64BitOperatingSystem == true)
            {
                try
                {
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileInfo.DirectoryName + "; Extended Properties='text;HDR=Yes;FMT=Delimited'";
                }
                catch
                {
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileInfo.DirectoryName + "; Extended Properties='text;HDR=Yes;FMT=Delimited'";
                }
            }
            OleDbConnection cn = new OleDbConnection(conString);
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + fileInfo.Name + "]", cn);
            cn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dt);
            cn.Close();

            return dt;
        }

        private static void ParseFile(
            bool withHeader,
            TextFieldParser textFieldParser,
            DataSet ds,
            FileInfo file,
            int? startRowCounter = null)
        {
            if (textFieldParser == null)
            {
                throw new ArgumentNullException(nameof(textFieldParser));
            }

            if (ds == null)
            {
                throw new ArgumentNullException(nameof(ds));
            }

            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var rowCounter = startRowCounter
                             ?? (withHeader ? 0 : 1);
            var errorCounter = 0;
            var rowImportCounter = 0;

            try
            {
                String[] stringRow = null;
                if (withHeader)
                {
                    stringRow = textFieldParser.ReadFields();
                }

                var columnCount = 0;

                try
                {
                    if (withHeader)
                    {
                        foreach (var field in stringRow)
                        {
                            ds.Tables[0].Columns.Add(field.Trim(), typeof(string));
                        }
                        columnCount = ds.Tables[0].Columns.Count;
                    }

                    while (true)
                    {
                        try
                        {
                            stringRow = textFieldParser.ReadFields();

                            if (!withHeader)
                            {
                                if (ds.Tables[0].Columns.Count == 0)
                                {
                                    foreach (var field in stringRow)
                                    {
                                        ds.Tables[0].Columns.Add(field.Trim(), typeof(string));
                                    }
                                    columnCount = ds.Tables[0].Columns.Count;
                                }
                            }

                            try
                            {
                                var rowColumnCount = stringRow.Count();
                                if (columnCount == rowColumnCount)
                                {
                                    ds.Tables[0].Rows.Add(stringRow);
                                    rowImportCounter++;
                                }
                                else
                                {
                                    var formatError = FormatErrorWrongColumn(columnCount, rowColumnCount);
                                    var clientMessage = GetErrorRowHasWrongColumnCount(
                                        file, formatError, rowCounter);
                                    var errorArgs = new ErrorArgs(
                                        ds.Tables[TableErrors], formatError, clientMessage, stringRow, rowCounter);
                                    AddError(errorArgs);

                                    errorCounter++;
                                }

                                if (textFieldParser.LineNumber == -1)
                                {
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                var clientMessage = GetErrorRowIssue(file, rowCounter);
                                var errorArgs = new ErrorArgs(
                                    ds.Tables[TableErrors], ex, clientMessage, stringRow, rowCounter);
                                AddError(errorArgs);

                                errorCounter++;
                            }
                        }
                        catch (Exception rowError)
                        {
                            var clientMessage = GetErrorRowHasWrongMoreColumns(file, rowCounter);
                            var errorArgs = new ErrorArgs(
                                ds.Tables[TableErrors], rowError, clientMessage, stringRow, rowCounter);
                            AddError(errorArgs);

                            errorCounter++;
                        }
                        rowCounter++;
                    }
                    textFieldParser.Close();
                }
                catch (Exception ex)
                {
                    var clientMessage = GetErrorClientMessage(file);
                    var errorArgs = new ErrorArgs(
                        ds.Tables[TableErrors], ex, clientMessage, stringRow);
                    AddError(errorArgs);

                    errorCounter++;
                }

                textFieldParser.Close();
            }
            catch (Exception ex)
            {
                var clientMessage = GetFatalClientMessage(file);
                var errorArgs = new ErrorArgs(
                    ds.Tables[TableErrors], ex, clientMessage);
                AddError(errorArgs);

                errorCounter++;
            }
            finally { textFieldParser.Close(); }

            FillCountsTable(ds.Tables[TableCounts], rowCounter, rowImportCounter, errorCounter);
        }

        private static string FormatErrorWrongColumn(int columnCount, int rowColumnCount)
        {
            var formatError = string.Format(
                "Expected column count is {0} but this row column count is  {1}",
                columnCount, rowColumnCount);
            return formatError;
        }

        private static string GetErrorBatchRow(FileInfo file, int startRow, int endRow)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var message = new StringBuilder();

            message.AppendFormat(
                "An error has been detected with your import file: {0}{1}<br/>", file.Name, file.Extension);
            message.AppendLine();

            message.AppendFormat(
                "The batch of rows {0} to {1} has been rejected.<br/>", startRow, endRow);
            message.AppendLine();

            message.AppendLine("You should correct this batch of records and resubmit for processing.<br/>");

            return message.ToString();
        }

        private static string GetErrorRowHasWrongMoreColumns(FileInfo file, int rowCounter)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var message = new StringBuilder();

            message.AppendFormat(
                "An error has been detected with your import file: {0}{1}<br/>", file.Name, file.Extension);
            message.AppendLine();
            message.AppendFormat(
                "This error is due to row {0} having more data values than defined columns.<br/>", rowCounter);
            message.AppendLine();
            message.AppendLine("This row of data has been excluded from your import.<br/>");
            message.AppendLine("You may correct this row of data, add it to a new file and resubmit for processing.<br/>");

            return message.ToString();
        }

        private static string GetErrorRowHasWrongColumnCount(FileInfo file, string formattedError, int rowCounter)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var message = new StringBuilder();
            message.AppendFormat(
                "An error has been detected with your import file: {0}{1}<br/>", file.Name, file.Extension);
            message.AppendLine();
            message.AppendFormat(
                "This error is due to row {0} not having the expected number of columns.<br/>", rowCounter);
            message.AppendLine();
            message.AppendFormat("{0}.<br/>{1}", formattedError, Environment.NewLine);
            message.AppendLine("This row of data has been excluded from your import.<br/>");
            message.AppendLine("You may correct this row of data, add it to a new file and resubmit for processing.<br/>");

            return message.ToString();
        }

        private static string GetErrorRowIssue(FileInfo file, int rowCounter)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var message = new StringBuilder();
            message.AppendFormat(
                "An error has been detected with your import file: {0}{1}<br/>", file.Name, file.Extension);
            message.AppendFormat("This error is due to row {0} having an issue.<br/>", rowCounter);
            message.AppendLine();
            message.AppendLine("This row of data has been excluded from your import.<br/>");
            message.AppendLine("You may correct this row of data, add it to a new file and resubmit for processing.<br/>");

            return message.ToString();
        }

        private static string GetErrorClientMessage(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var message = new StringBuilder();
            message.AppendFormat(
                "An error has been detected with your import file: {0}{1}<br/>", file.Name, file.Extension);
            message.AppendLine();
            message.AppendLine("This error while attempting to parse the column headers.<br/>");
            message.AppendLine("Please review your files column headers and upload the file again.<br/>");
            return message.ToString();
        }

        private static string GetFatalClientMessage(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var message = new StringBuilder();
            message.AppendFormat(
                "A fatal error has been detected with your import file: {0}{1}<br/>", file.Name, file.Extension);
            message.AppendLine();
            message.AppendLine("The entire file has been rejected.<br/>");
            message.AppendLine("You should correct this file and resubmit for processing.<br/>");
            return message.ToString();
        }

        private static void AddError(ErrorArgs errorArgs)
        {
            if (errorArgs == null)
            {
                throw new ArgumentNullException(nameof(errorArgs));
            }

            var newEr = errorArgs.ErrorTable.NewRow();

            if (errorArgs.StringRow != null)
            {
                newEr[TableErrorFieldBadRowData] = string.Join(",", errorArgs.StringRow);
            }
            else
            {
                newEr[TableErrorFieldBadRowData] = string.Empty;
            }

            if (errorArgs.FatalError.HasValue)
            {
                newEr[TableErrorFielIsFatalError] = false;
            }

            newEr[TableErrorFieldRowNumber] = errorArgs.RowNumber ?? -99;

            var formattedError = errorArgs.FormattedError ?? StringFunctions.FormatExceptionForHTML(errorArgs.Error);

            newEr[TableErrorFieldFormattedError] = formattedError;

            newEr[TableErrorFieldClientMessage] = errorArgs.ClientMessage;

            errorArgs.ErrorTable.Rows.Add(newEr);
        }

        private static void FillCountsTable(DataTable countsTable, int rowCounter, int rowImportCounter, int errorCounter)
        {
            if (countsTable == null)
            {
                throw new ArgumentNullException(nameof(countsTable));
            }

            var newCountRow = countsTable.NewRow();
            newCountRow[TableCountFieldTotalRows] = rowCounter;
            newCountRow[TableCountFieldRowImportCount] = rowImportCounter;
            newCountRow[TableCountFieldRowErrorCount] = errorCounter;
            countsTable.Rows.Add(newCountRow);
        }

        private static DataSet CreateDataSet(
            bool addBadRowData = true,
            bool addFatalError = false)
        {
            var dataSet = new DataSet();
            dataSet.Tables.Add(TableData);
            dataSet.Tables.Add(TableErrors);
            dataSet.Tables.Add(TableCounts);
            dataSet.Tables[TableErrors].Columns.Add(TableErrorFieldRowNumber, Type.GetType("System.Int32"));
            dataSet.Tables[TableErrors].Columns.Add(TableErrorFieldFormattedError, Type.GetType("System.String"));
            dataSet.Tables[TableErrors].Columns.Add(TableErrorFieldClientMessage, Type.GetType("System.String"));

            if (addBadRowData)
            {
                dataSet.Tables[TableErrors].Columns.Add(TableErrorFieldBadRowData, Type.GetType("System.String"));
            }

            if (addFatalError)
            {
                dataSet.Tables[TableErrors].Columns.Add(TableErrorFielIsFatalError, Type.GetType("System.Boolean"));
            }

            dataSet.Tables[TableCounts].Columns.Add(TableCountFieldTotalRows, Type.GetType("System.Int32"));
            dataSet.Tables[TableCounts].Columns.Add(TableCountFieldRowImportCount, Type.GetType("System.Int32"));
            dataSet.Tables[TableCounts].Columns.Add(TableCountFieldRowErrorCount, Type.GetType("System.Int32"));

            return dataSet;
        }
    }

    
}
