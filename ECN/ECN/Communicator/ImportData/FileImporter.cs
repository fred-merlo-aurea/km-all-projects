using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.OleDb;		// for reading Excel file
using System.IO;
using System.Net;
// for reading other type of delimited files.
using System.Text;
using KM.Common.Functions;

namespace ecn.communicator.classes.ImportData
{
    public abstract class FileImporter
    {
        /// 
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
            var csv = ImportHelper.LoadCsv(theFile);
            return csv; 
        }

        /// <param name="physicalDataPath"></param>
        /// <param name="fileName"></param>
        /// <param name="excelSheetName"></param>
        /// <param name="startLine"></param>
        /// <param name="maxRecordsToRetrieve">0 means retrieve all records.</param>
        /// <returns></returns>
        public static DataTable GetDataTableByFileType(string physicalDataPath, string fileType, string fileName, string excelSheetName, int startLine, int maxRecordsToRetrieve, string delimiter)
        {
            string connString = "";

            if (fileType == "X")
            {
                var result = ImportHelper.ProcessXFile(
                    physicalDataPath, fileType, fileName, excelSheetName, startLine, maxRecordsToRetrieve);
                return result;
            }
            else if ((fileType == "C"))
            {
                //create a schema to use before filling the adapter
                ConstructSchema(physicalDataPath, fileName);
                connString = getDatasource(physicalDataPath, fileName, fileType);
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

        public static DataSet getDatafromTXT(
            string physicalDataPath, 
            string fileName, 
            int startLine, 
            int maxRecordsToRetrieve, 
            string delimiterBy)
        {
            var dataSet = ImportHelper.GetDatafromTxt(
                physicalDataPath,
                fileName,
                startLine,
                maxRecordsToRetrieve,
                delimiterBy);
            return dataSet;
        }

        private static string getDatasource(string path, string fileName, string fileType)
        {
            var connStr = ImportHelper.GetDataSource(path, fileName, fileType, false);
            return connStr;
        }
    }
}
