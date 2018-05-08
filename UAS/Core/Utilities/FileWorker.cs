using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using Excel;
using System.Xml.Linq;
using ServiceStack.Text;
using System.Collections.Specialized;
using KM.Common.Import;
using ColumnDelimiter = KM.Common.Enums.ColumnDelimiter;

namespace Core_AMS.Utilities
{
    public class FileWorker
    {
        IExcelDataReader GetExcelReader(FileInfo fileInfo, bool isFirstRowColumnNames = true)
        {
            IExcelDataReader excelReader;

            FileStream stream = System.IO.File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            
            if (fileInfo.Extension.Equals(".xls", StringComparison.CurrentCultureIgnoreCase))
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            excelReader.IsFirstRowAsColumnNames = isFirstRowColumnNames;
            return excelReader;
        }

        #region DataTables
        public DataTable GetData(FileInfo fileInfo, FileConfiguration fileConfig = null)
        {
            if (IsExcelFile(fileInfo))
                return GetDataExcel(fileInfo);
            else if (IsXmlFile(fileInfo))
                return GetDataXml(fileInfo);
            else if (IsJsonFile(fileInfo))
                return GetDataJson(fileInfo);
            else if (IsDbfFile(fileInfo))
                return GetDataDbf(fileInfo);
            else return GetDataText(fileInfo, fileConfig);
        }
        public DataTable GetDataTopRows(FileInfo fileInfo, FileConfiguration fileConfig = null, int rowsToGet = -1)
        {
            if (IsExcelFile(fileInfo))
                return GetDataExcel(fileInfo);
            else if (IsDbfFile(fileInfo))
                return GetDataDbfTopRows(fileInfo, rowsToGet);
            else return GetDataTextTopRows(fileInfo, fileConfig, rowsToGet);
        }

        DataTable GetDataExcel(FileInfo fileInfo)
        {
            try
            {
                IExcelDataReader excelReader = GetExcelReader(fileInfo);
                if (excelReader.AsDataSet().Tables != null && excelReader.AsDataSet().Tables.Count >= 1 && excelReader.AsDataSet().Tables[0] != null)
                    return excelReader.AsDataSet().Tables[0];
                else
                    return new DataTable();
            }
            catch(Exception)
            {
                System.Threading.Thread.Sleep(5000);
                IExcelDataReader excelReader = GetExcelReader(fileInfo);
                if(excelReader.AsDataSet().Tables.Count >= 1)
                    return excelReader.AsDataSet().Tables[0];
                else
                    return new DataTable();
            }
        }
        DataTable GetDataText(FileInfo fileInfo, FileConfiguration fileConfig)
        {
            return FileImporter.LoadFile(fileInfo, fileConfig);
        }
        DataTable GetDataDbf(FileInfo fileInfo)
        {
            return FileImporter.ConvertFoxProDBFToDataTable(fileInfo);
        }
        DataTable GetDataXml(FileInfo fileInfo)
        {
            return Core_AMS.Utilities.XmlFunctions.CreateDataTableFromXmlFile(fileInfo);
        }
        DataTable GetDataJson(FileInfo fileInfo)
        {
            JsonFunctions jf = new JsonFunctions();
            return jf.CreateDataTableFromJsonFile(fileInfo);
        }

        DataTable GetDataExcelTopRows(FileInfo fileInfo, int rowsToGet = -1)
        {
            IExcelDataReader excelReader = GetExcelReader(fileInfo);
            return excelReader.AsDataSet().Tables[0];
        }
        DataTable GetDataDbfTopRows(FileInfo fileInfo, int rowsToGet = -1)
        {
            DataTable dt = FileImporter.ConvertFoxProDBFToDataTable(fileInfo);
            return dt;
        }
        DataTable GetDataTextTopRows(FileInfo fileInfo, FileConfiguration fileConfig, int rowsToGet = -1)
        {
            return FileImporter.LoadFileTopRows(fileInfo, fileConfig, rowsToGet);
        }
        #endregion

        #region DataSets
        public DataSet GetDataSet(FileInfo fileInfo, FileConfiguration fileConfig = null)
        {
            if (IsExcelFile(fileInfo))
                return GetDataSetExcel(fileInfo);
            else return GetDataSetText(fileInfo, fileConfig);
        }
        public DataSet GetDataSet_NoHeader(FileInfo fileInfo, FileConfiguration fileConfig = null)
        {
            if (IsExcelFile(fileInfo))
                return GetDataSetExcel(fileInfo);
            else return GetDataSetText_NoHeader(fileInfo, fileConfig);
        }
        DataSet GetDataSetExcel(FileInfo fileInfo)
        {
            //IExcelDataReader excelReader = GetExcelReader(fileInfo);
            DataSet ds = new DataSet();
            DataTable dtErrors = new DataTable();
            dtErrors.TableName = "Errors";
            DataTable dtCounts = new DataTable();
            dtCounts.TableName = "Counts";

            dtErrors.Columns.Add("RowNumber", Type.GetType("System.Int32"));
            dtErrors.Columns.Add("FormattedError", Type.GetType("System.String"));
            dtErrors.Columns.Add("ClientMessage", Type.GetType("System.String"));

            dtCounts.Columns.Add("TotalRows", Type.GetType("System.Int32"));
            dtCounts.Columns.Add("RowImportCount", Type.GetType("System.Int32"));
            dtCounts.Columns.Add("RowErrorCount", Type.GetType("System.Int32"));

            int errorCounter = 0;

            FileStream stream = System.IO.File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = null;
            try
            {
                if (fileInfo.Extension.Equals(".xls", StringComparison.CurrentCultureIgnoreCase))
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                else
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                excelReader.IsFirstRowAsColumnNames = true;
            }
            catch (Exception exception)
            {
                DataRow newER = dtErrors.NewRow();
                var formatError = StringFunctions.FormatExceptionForHtml(exception);
                newER["FormattedError"] = formatError;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("An error has been detected with your import file: " + fileInfo.Name + fileInfo.Extension + "<br/>");
                //sb.AppendLine("This error is due to row " + rowCounter.ToString() + " having more data values than defined columns.");
                //sb.AppendLine("This row of data has been excluded from your import.");
                //sb.AppendLine("You may correct this row of data, add it to a new file and resubmit for processing.");
                sb.AppendLine("Your file has been rejected for processing due to the following:<br/>");
                sb.AppendLine(formatError);
                newER["ClientMessage"] = sb.ToString();

                dtErrors.Rows.Add(newER);
                errorCounter++;
            }

            DataTable dtExcel = excelReader.AsDataSet().Tables[0].Copy();
            dtExcel.TableName = "Data";

            DataRow newCountRow = dtCounts.NewRow();
            newCountRow["TotalRows"] = dtExcel.Rows.Count;
            newCountRow["RowImportCount"] = dtExcel.Rows.Count;
            newCountRow["RowErrorCount"] = errorCounter;
            dtCounts.Rows.Add(newCountRow);


            ds.Tables.Add(dtExcel);
            ds.Tables.Add(dtErrors);
            ds.Tables.Add(dtCounts);

            return ds;
        }
        DataSet GetDataSetText(FileInfo fileInfo, FileConfiguration fileConfig)
        {
            return FileImporter.GetDataSet(fileInfo, fileConfig);
        }

        DataSet GetDataSetText_NoHeader(FileInfo fileInfo, FileConfiguration fileConfig)
        {
            return FileImporter.GetDataSetNoHeader(fileInfo, fileConfig);
        }
        #endregion


        public char GetFirstCharacter(FileInfo fileInfo)
        {
            char first;
            StreamReader reader;
            reader = new StreamReader(fileInfo.FullName);
            first = (char)reader.Peek();
            reader.Close();
            reader.Dispose();
            return first;
        }
        public int GetRowCount(FileInfo fileInfo)
        {
            int numberOfRows = 0;
            if (IsExcelFile(fileInfo))
                numberOfRows = GetDataExcel(fileInfo).Rows.Count;
            else if (IsDbfFile(fileInfo))
                numberOfRows = DbfRowCount(fileInfo);
            else if (IsJsonFile(fileInfo))
                numberOfRows = JsonRowCount(fileInfo);
            else if (IsXmlFile(fileInfo))
                numberOfRows = XmlRowCount(fileInfo);
            else
                numberOfRows = RowCount(fileInfo);
            return numberOfRows;
        }
        private int RowCount(FileInfo file)
        {
            int rowCount = 0;

            using (StreamReader sr = File.OpenText(file.FullName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    rowCount++;
            }

            //remove header
            return rowCount - 1;
        }
        private int JsonRowCount(FileInfo file)
        {
            int rowCount = 0;
            try
            {
                JsonFunctions jf = new JsonFunctions();
                rowCount = jf.GetRecordCount(file);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rowCount;
        }
        private int XmlRowCount(FileInfo file)
        {
            int rowCount = 0;
            try
            {
                rowCount = XmlFunctions.GetRecordCount(file);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rowCount;
        }
        public void RemoveTrailingCharactersFromTextFile(FileInfo file, bool quoteEncapsulate = false, ColumnDelimiter delimiter = ColumnDelimiter.comma)
        {
            //remove trailing tabs, returns, escapes, commas and whitespace.
            if (IsExcelFile(file) == false && IsDbfFile(file) == false)
            {
                List<string> list = new List<string>();
                string line;
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    while((line = sr.ReadLine()) != null)
                    { 
                        string cleanLine = line.TrimEnd().TrimEnd('\t').TrimEnd(',');
                        if(!string.IsNullOrEmpty(cleanLine))
                            list.Add(cleanLine);
                    }
                }
                File.Delete(file.FullName);

                WriteList(file.FullName, list, quoteEncapsulate, delimiter);
            }
        }

        public void QuoteEncapsulateFile(FileInfo file, ColumnDelimiter delimiter = ColumnDelimiter.comma)
        {
            if (IsExcelFile(file) == false && IsDbfFile(file) == false)
            {
                List<string> list = new List<string>();
                string line;
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string cleanLine = line.TrimEnd().TrimEnd('\t').TrimEnd(',');
                        if (!string.IsNullOrEmpty(cleanLine))
                            list.Add(cleanLine);
                    }
                }
                File.Delete(file.FullName);

                WriteList(file.FullName, list, true, delimiter);
            }
        }
        private int DbfRowCount(FileInfo file)
        {
            int rowCount = 0;
            try
            {
                //string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file.DirectoryName + "; Extended Properties='text;HDR=Yes;FMT=Delimited'";
                OleDbConnection cn = new OleDbConnection(@"Provider=VFPOLEDB.1;Data Source=" + file.FullName);
                OleDbCommand cmd = new OleDbCommand("SELECT Count(*) FROM [" + file.Name + "]", cn);
                cn.Open();
                rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            catch(Exception ex) 
            {
                throw ex;
            }

            return rowCount;
        }

        #region Dictionaries
        public StringDictionary GetFileHeaders(FileInfo fileInfo, FileConfiguration fileConfig = null, bool isFirstRowColumnNames = true)
        {
            StringDictionary headers = new StringDictionary();

            if (IsExcelFile(fileInfo))
                headers = GetFileHeadersExcel(fileInfo, isFirstRowColumnNames);
            else if (IsXmlFile(fileInfo))
                headers = GetFileHeadersXml(fileInfo);
            else if (IsJsonFile(fileInfo))
                headers = GetFileHeadersJson(fileInfo);
            else if (IsDbfFile(fileInfo))
                headers = GetFileHeadersDbf(fileInfo);
            else
                headers = GetFileHeadersText(fileInfo, fileConfig);
            return headers;
        }

        public StringDictionary GetFileHeadersExcel(FileInfo fileInfo, bool isFirstRowColumnNames = true)
        {
            StringDictionary headers = new StringDictionary();
            IExcelDataReader excelReader = GetExcelReader(fileInfo, isFirstRowColumnNames);

            DataSet excelData = new DataSet();
            try
            {
                excelData = excelReader.AsDataSet();
            }
            catch { }
            int i = 0;
            if (excelData != null && excelData.Tables != null && excelData.Tables.Count >= 1)
            {
                if (excelData.Tables[0].Columns.Count > 0 && (!excelData.Tables[0].Columns.Contains("Column1") && !excelData.Tables[0].Columns.Contains("Column2")))
                {
                    foreach (object item in excelData.Tables[0].Columns)
                    {
                        if (!string.IsNullOrEmpty(item.ToString().Trim()))
                        {
                            if (!headers.ContainsKey(item.ToString().Trim()))
                            {
                                headers.Add(item.ToString().Trim(), i.ToString());
                                i++;
                            }
                        }
                    }
                }
                else
                {
                    foreach (object item in excelData.Tables[0].Rows[0].ItemArray)
                    {
                        if (!string.IsNullOrEmpty(item.ToString().Trim()))
                        {
                            if (!headers.ContainsKey(item.ToString().Trim()))
                            {
                                headers.Add(item.ToString().Trim(), i.ToString());
                                i++;
                            }
                        }
                    }
                }
            }

            return headers;
        }

        public StringDictionary GetFileHeadersXml(FileInfo fileInfo)
        {
            XDocument doc = Core_AMS.Utilities.XmlFunctions.CreateFromFile(fileInfo);
            StringDictionary headers = new StringDictionary();
            int columnOrder = 0;            

            string parentRootName = doc.Root.Name.ToString();
            string recordRootName = doc.Root.Elements().First().Name.ToString();

            foreach (var headerName in doc.Root.Elements(recordRootName).Elements().Select(x => x.Name).Distinct())
            {
                if (!string.IsNullOrEmpty(headerName.ToString().Trim()))
                {
                    if (!headers.ContainsKey(headerName.ToString().Trim()))
                    {
                        headers.Add(headerName.ToString().Trim(), columnOrder.ToString());
                        columnOrder++;
                    }
                }
            }

            return headers;
        }

        public StringDictionary GetFileHeadersJson(FileInfo fileInfo)
        {
            StringDictionary headers = new StringDictionary();
            string json = "";
            StreamReader streamReader = new StreamReader(fileInfo.FullName, System.Text.Encoding.ASCII);
            json = streamReader.ReadToEnd().Replace(Environment.NewLine, "");
            streamReader.Close();

            JsonArrayObjects aObj = JsonArrayObjects.Parse(json);
            var firstRow = aObj.First();
            int columnOrder = 0;    
            foreach (var i in firstRow)
            {
                if (!string.IsNullOrEmpty(i.Key.Trim()))
                {
                    if (!headers.ContainsKey(i.Key.ToString().Trim()))
                    {
                        headers.Add(i.Key.ToString().Trim(), columnOrder.ToString());
                        columnOrder++;
                    }
                }
            }

            return headers;
        }

        public StringDictionary GetFileHeadersDbf(FileInfo fileInfo)
        {
            StringDictionary headers = new StringDictionary();
            List<string> headerList = FileImporter.LoadDbfHeaders(fileInfo);
            int i = 0;
            foreach (string dc in headerList)
            {
                if (!string.IsNullOrEmpty(dc.Trim()))
                {
                    if (!headers.ContainsKey(dc.ToString().Trim()))
                    {
                        headers.Add(dc.ToString().Trim(), i.ToString());
                        i++;
                    }
                }
            }

            return headers;
        }

        public StringDictionary GetFileHeadersText(FileInfo fileInfo, FileConfiguration fileConfig)
        {
            StringDictionary headers = new StringDictionary();
            List<string> headerList = FileImporter.LoadHeaders(fileInfo, fileConfig);
            int i = 0;
            foreach (string dc in headerList)
            {
                if (!string.IsNullOrEmpty(dc.Trim()))
                {
                    if (!headers.ContainsKey(dc.ToString().Trim()))
                    {
                        headers.Add(dc.ToString().Trim(), i.ToString());
                        i++;
                    }
                }
            }
            return headers;
        }

        public Dictionary<string, bool> CheckHeadersForUnicodeChars(FileInfo fileInfo, FileConfiguration fileConfig = null, bool isFirstRowColumnNames = true)
        {
            StringDictionary headers = new StringDictionary();

            if (IsExcelFile(fileInfo))
                headers = GetFileHeadersExcel(fileInfo, isFirstRowColumnNames);
            else if (IsXmlFile(fileInfo))
                headers = GetFileHeadersXml(fileInfo);
            else if (IsJsonFile(fileInfo))
                headers = GetFileHeadersJson(fileInfo);
            else if (IsDbfFile(fileInfo))
                headers = GetFileHeadersDbf(fileInfo);
            else
                headers = GetFileHeadersText(fileInfo, fileConfig);

            
            List<string> myHeaders = new List<string>();
            foreach(var kvp in headers.Keys)
            {
                if (!myHeaders.Contains(kvp.ToString().Trim()))
                    myHeaders.Add(kvp.ToString().Trim());
            }
            Dictionary<string, bool> checkedHeaders = CheckHeadersForUnicodeChars(myHeaders);
            return checkedHeaders;
        }
        public Dictionary<string, bool> CheckHeadersForUnicodeChars(List<string> dirtyHeaders)
        {
            Dictionary<string, bool> headers = new Dictionary<string, bool>();
            FileFunctions ff = new FileFunctions();
            foreach (string field in dirtyHeaders)
            {
                bool hasUnicode = ff.HasUnicodeChars(field.Trim());
                if (!headers.ContainsKey(field.Trim()))
                    headers.Add(field.Trim(), hasUnicode);
            }

            return headers;
        }
        public Dictionary<string, bool> CheckTextFileHeadersForUnicodeChars(FileInfo file, FileConfiguration fileConfig)
        {
            Dictionary<string, bool> headers = new Dictionary<string, bool>();

            fileConfig = FileHelpers.EnsureFileConfiguration(fileConfig);
            var tfp = FileHelpers.CreateTextFieldParser(file, fileConfig);

            try
            {
                var stringRow = tfp.ReadFields();
                FileFunctions ff = new FileFunctions();
                foreach (String field in stringRow)
                {
                    bool hasUnicode = ff.HasUnicodeChars(field.Trim());
                    headers.Add(field.Trim(), hasUnicode);
                }

                tfp.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { tfp.Close(); }

            return headers;
        }
        #endregion

        #region Duplicate Column Checks
        private List<string> DuplicateColumnCheck(FileInfo fileInfo, FileConfiguration fileConfig)
        {
            List<string> duplicates = new List<string>();
            
            if (IsExcelFile(fileInfo))
                duplicates = DuplicateColumnCheckExcel(fileInfo, true);
            else if (IsXmlFile(fileInfo))
                duplicates = DuplicateColumnCheckXml(fileInfo);
            else if (IsJsonFile(fileInfo))
                duplicates = DuplicateColumnCheckJson(fileInfo);
            else if (IsDbfFile(fileInfo))
                duplicates = DuplicateColumnCheckDbf(fileInfo);
            else
                duplicates = DuplicateColumnCheckText(fileInfo, fileConfig);
            return duplicates;
        }
        public List<string> DuplicateColumnCheckExcel(FileInfo fileInfo, bool isFirstRowColumnNames = true)
        {
            StringDictionary headers = new StringDictionary();
            List<string> dupes = new List<string>();
            IExcelDataReader excelReader = GetExcelReader(fileInfo, isFirstRowColumnNames);
            DataSet excelData = excelReader.AsDataSet();
            int i = 0;
            if (excelData != null)
            {
                if (excelData.Tables[0].Columns.Count > 0 && (!excelData.Tables[0].Columns.Contains("Column1") && !excelData.Tables[0].Columns.Contains("Column2")))
                {
                    foreach (object item in excelData.Tables[0].Columns)
                    {
                        if (headers.ContainsKey(item.ToString().Trim()))
                            dupes.Add(item.ToString().Trim());
                        else
                        {
                            headers.Add(item.ToString().Trim(), i.ToString());
                            i++;
                        }
                    }
                }
                else
                {
                    foreach (object item in excelData.Tables[0].Rows[0].ItemArray)
                    {
                        if (headers.ContainsKey(item.ToString().Trim()))
                            dupes.Add(item.ToString().Trim());
                        else
                        {
                            headers.Add(item.ToString().Trim(), i.ToString());
                            i++;
                        }
                    }
                }
            }

            return dupes;
        }
        public List<string> DuplicateColumnCheckXml(FileInfo fileInfo)
        {
            StringDictionary headers = new StringDictionary();
            XDocument doc = Core_AMS.Utilities.XmlFunctions.CreateFromFile(fileInfo);
            List<string> dupes = new List<string>();

            string parentRootName = doc.Root.Name.ToString();
            string recordRootName = doc.Root.Elements().First().Name.ToString();

            int i = 0;

            foreach (var headerName in doc.Root.Elements(recordRootName).Elements().Select(x => x.Name).Distinct())
            {
                if (headers.ContainsKey(headerName.ToString().Trim()))
                    dupes.Add(headerName.ToString().Trim());
                else
                {
                    headers.Add(headerName.ToString().Trim(), i.ToString());
                    i++;
                }
            }

            return dupes;
        }
        public List<string> DuplicateColumnCheckJson(FileInfo fileInfo)
        {
            StringDictionary headers = new StringDictionary();
            List<string> dupes = new List<string>();
            string json = "";
            int order = 0;
            StreamReader streamReader = new StreamReader(fileInfo.FullName, System.Text.Encoding.ASCII);
            json = streamReader.ReadToEnd().Replace(Environment.NewLine, "");
            streamReader.Close();

            JsonArrayObjects aObj = JsonArrayObjects.Parse(json);
            var firstRow = aObj.First();
            foreach (var i in firstRow)
            {
                if (headers.ContainsKey(i.Key.ToString().Trim()))
                    dupes.Add(i.Key.ToString().Trim());
                else
                {
                    headers.Add(i.Key.ToString().Trim(), i.ToString());
                    order++;
                }

            }

            return dupes;
        }
        public List<string> DuplicateColumnCheckDbf(FileInfo fileInfo)
        {
            StringDictionary headers = new StringDictionary();
            List<string> dupes = new List<string>();
            List<string> headerList = FileImporter.LoadDbfHeaders(fileInfo);
            int i = 0;
            foreach (string dc in headerList)
            {
                if (headers.ContainsKey(dc.ToString().Trim()))
                    dupes.Add(dc.ToString().Trim());
                else
                {
                    headers.Add(dc.ToString().Trim(), i.ToString());
                    i++;
                }
            }

            return dupes;
        }
        public List<string> DuplicateColumnCheckText(FileInfo fileInfo, FileConfiguration fileConfig)
        {
            StringDictionary headers = new StringDictionary();
            List<string> dupes = new List<string>();
            List<string> headerList = FileImporter.LoadHeaders(fileInfo, fileConfig);
            int i = 0;
            foreach (string dc in headerList)
            {
                if (headers.ContainsKey(dc.ToString().Trim()))
                    dupes.Add(dc.ToString().Trim());
                else
                {
                    headers.Add(dc.ToString().Trim(), i.ToString());
                    i++;
                }
            }
            return dupes;
        }
        public List<string> GetDuplicateColumns(FileInfo fileInfo, FileConfiguration fileConfig)
        {
            List<string> duplicates = DuplicateColumnCheck(fileInfo, fileConfig);
            
            return duplicates;
        }
        #endregion

        #region Unmapped Column Check
        public List<string> GetUnmappedHeaderColumns(List<string> fileHeaders, List<string> mappedHeaders)
        {
            List<string> unknownColumns = new List<string>();
            foreach (var h in fileHeaders)
            {
                if (!mappedHeaders.Exists(x => x.Equals(h, StringComparison.CurrentCultureIgnoreCase)))
                {
                    unknownColumns.Add(h.ToString());
                }
            }
            return unknownColumns;
        }
        public List<string> GetSimilarHeaderColumns(List<string> fileHeaders)
        {
            List<string> list1 = fileHeaders;
            List<string> possibleDupes = new List<string>();
            foreach (var h in fileHeaders)
            {
                foreach (var i in list1)
                {
                    if (i.StartsWith(h))
                        possibleDupes.Add(i);
                }
            }
            return possibleDupes;
        }
        #endregion
        public bool IsJsonFile(FileInfo file)
        {
            string extensions = ".json";
            if (extensions.Equals(file.Extension, StringComparison.CurrentCultureIgnoreCase)) return true;
            else return false;
        }
        public bool IsXmlFile(FileInfo file)
        {
            string extensions = ".xml";
            if (extensions.Equals(file.Extension, StringComparison.CurrentCultureIgnoreCase)) return true;
            else return false;
        }
        public bool IsExcelFile(FileInfo file)
        {
            string[] extensions = new string[] { ".xlsx", ".xls" };
            if (extensions.Contains(file.Extension.ToLower())) return true;
            else return false;
        }
        public bool IsDbfFile(FileInfo file)
        {
            string extensions = ".dbf";
            if (extensions.Equals(file.Extension, StringComparison.CurrentCultureIgnoreCase)) return true;
            else return false;
        }
        public bool IsZipFile(FileInfo file)
        {
            string extensions = ".zip";
            if (extensions.Equals(file.Extension, StringComparison.CurrentCultureIgnoreCase)) return true;
            else return false;
        }
        public bool AcceptableFileType(FileInfo file)
        {
            string[] extensions = new string[] { ".xlsx", ".xls", ".csv", ".txt", ".dbf", ".zip", ".xml", ".json" };
            if (extensions.Contains(file.Extension.ToLower())) return true;
            else return false;
        }

        private static void WriteList(
            string fileFullName,
            List<string> list,
            bool quoteEncapsulate = false,
            ColumnDelimiter delimiter = ColumnDelimiter.comma)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (quoteEncapsulate)
            {
                var del = GetDelimiterString(delimiter);

                var quoteList = new List<string>();
                foreach (var splitLine in list)
                {
                    var quotedDel = Delimiters.Quotes + del + Delimiters.Quotes;
                    var replaced = splitLine.Replace(del, quotedDel);
                    quoteList.Add(Delimiters.Quotes + replaced + Delimiters.Quotes);
                }

                list.Clear();
                list = quoteList;
            }

            foreach (var item in list)
            {
                using (var writer = new StreamWriter(fileFullName, true))
                {
                    writer.Write(item + Environment.NewLine);
                    writer.Flush();
                }
            }
        }

        private static string GetDelimiterString(ColumnDelimiter delimiter)
        {
            var del = Delimiters.Comma;

            switch (delimiter)
            {
                case ColumnDelimiter.comma:
                    del = Delimiters.Comma;
                    break;
                case ColumnDelimiter.semicolon:
                    del = Delimiters.Semicolon;
                    break;
                case ColumnDelimiter.tab:
                    del = Delimiters.Tab;
                    break;
                case ColumnDelimiter.colon:
                    del = Delimiters.Colon;
                    break;
                case ColumnDelimiter.tild:
                    del = Delimiters.Tild;
                    break;
                case ColumnDelimiter.pipe:
                    del = Delimiters.Pipe;
                    break;
            }

            return del;
        }
    }
}
