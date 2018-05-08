using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace KM.Common.Functions
{
    public static class ImportHelper
    {
        private const string XlsExtention = ".xls";
        private const string XlsxExtension = ".xlsx";
        private const string ExcelTable = "ExcelTable";
        private const string MaxScanRows = "MaxScanRows=10000;";

        public static DataSet GetDatafromTxt(
            string physicalDataPath, 
            string fileName, 
            int startLine, 
            int maxRecordsToRetrieve, 
            string delimiterBy)
        {
            var delimiter = string.Empty;

            if (string.Equals(delimiterBy, "TABDELIMITED", StringComparison.OrdinalIgnoreCase))
            {
                delimiter = "\t";
            }
            else
            {
                delimiter = ",";
            }

            //The DataSet to Return
            var dataSet = new DataSet();

            //Open the file in a stream reader.
            var streamReader = new StreamReader(Path.Combine(physicalDataPath, fileName));

            //Split the first line into the columns       
            var columns = streamReader
                            .ReadLine()
                            .Split(delimiter.ToCharArray());
            //Add the new DataTable to the RecordSet
            dataSet.Tables.Add(fileName);

            //Cycle the colums, adding those that don't exist yet and sequencing the one that do.
            for (var i = 0; i < columns.Length; i++)
            {
                var columnName = string.Format("col{0}", i);
                dataSet.Tables[fileName].Columns.Add(columnName);
            }

            streamReader.DiscardBufferedData();
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            streamReader.BaseStream.Position = 0;

            //Read the rest of the data in the file.        
            var allData = streamReader.ReadToEnd();
            var rows = allData.Split("\r\n".ToCharArray());

            var rowcount = 0;

            foreach (var row in rows)
            {
                if (rowcount < maxRecordsToRetrieve || maxRecordsToRetrieve == 0)
                {
                    if (row != string.Empty)
                    {
                        try
                        {
                            //Split the row at the delimiter.
                            var items = row.Split(delimiter.ToCharArray());

                            dataSet.Tables[fileName].Rows.Add(items);
                        }
                        catch (Exception) {}

                        rowcount++;
                    }
                }
            }

            streamReader.Dispose();

            return dataSet;
        }

        public static string GetDataSource(
            string path, string fileName, string filetype, bool consider64Bit = true, bool addCMaxScanRows = false)
        {
            var connStr = string.Empty;
            switch (filetype)
            {
                case "X":
                    connStr = GetConnStrForXFile(path, fileName, connStr);
                    break;
                case "C":
                    connStr = string.Format(
                        "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Text;HDR=NO;{1}'", 
                        path,
                        addCMaxScanRows ? 
                            MaxScanRows : 
                            string.Empty);
                    if (consider64Bit && Environment.Is64BitOperatingSystem)
                    {
                        try
                        {
                            connStr = string.Format(
                                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties='text;HDR=NO;'",
                                path);
                        }
                        catch
                        {
                            connStr = string.Format(
                                "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}; Extended Properties='text;HDR=NO;'",
                                path);
                        }
                    }
                    break;
                case "O":
                    connStr = string.Format(
                        "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Text;HDR=NO;FMT=TabDelimited';",
                        path);

                    if (consider64Bit && Environment.Is64BitOperatingSystem)
                    {
                        try
                        {
                            connStr = string.Format(
                                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties='text;HDR=NO;FMT=TabDelimited;'",
                                path);
                        }
                        catch
                        {
                            connStr = string.Format(
                                "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}; Extended Properties='text;HDR=NO;FMT=TabDelimited;'",
                                path);
                        }
                    }
                    break;
            }
            return connStr;
        }

        public static string GetConnStrForXFile(string path, string fileName, string connStr)
        {
            var fullPath = Path.Combine(path, fileName);

            if (fileName.EndsWith(XlsExtention, StringComparison.OrdinalIgnoreCase))
            {
                connStr = string.Format(
                    "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=NO;IMEX=1;'", 
                    fullPath);
            }
            else
            if (fileName.EndsWith(XlsxExtension, StringComparison.OrdinalIgnoreCase))
            {
                connStr = string.Format(
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=NO;IMEX=1;'", 
                    fullPath);
            }

            return connStr;
        }

        public static DataTable LoadCsv(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            var sqlString = string.Format("Select * FROM [{0}];", fileInfo.Name);
            var conStr = string.Format(
                "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='text;HDR=YES;'", 
                fileInfo.DirectoryName);
            var theCsv = new DataTable();

            using (var conn = new OleDbConnection(conStr))
            {
                using (var comm = new OleDbCommand(sqlString, conn))
                {
                    using (var adapter = new OleDbDataAdapter(comm))
                    {
                        adapter.Fill(theCsv);
                    }
                }
            }
            return theCsv;
        }

        public static DataTable ProcessXFile(
            string physicalDataPath, 
            string fileType, 
            string fileName, 
            string excelSheetName, 
            int startLine, 
            int maxRecordsToRetrieve)
        {
            var connString = ImportHelper.GetDataSource(physicalDataPath, fileName, fileType, false, true);

            var oleAdapter = new OleDbDataAdapter(
                string.Format("SELECT * FROM [{0}$]", excelSheetName), 
                connString);

            var dataset = new DataSet();
            //fill the adapter with rows only from the linenumber specified
            oleAdapter.Fill(dataset, startLine, maxRecordsToRetrieve, ExcelTable);

            return dataset.Tables[ExcelTable];
        }
    }
}
