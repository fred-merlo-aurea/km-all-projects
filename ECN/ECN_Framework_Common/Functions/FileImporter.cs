using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;					
using System.Text;
using KM.Common.Functions;
using LumenWorks.Framework.IO;

namespace ECN_Framework_Common.Functions
{
    public abstract class FileImporter
    {       
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
             
        public static DataTable GetDataTableByFileType(string physicalDataPath, string fileType, string fileName, string excelSheetName, int startLine, int maxRecordsToRetrieve, string delimiter)
        {
            string connString = "";

            if (fileType == "X")
            {
                // load data from Excel file				
                var result = ImportHelper.ProcessXFile(
                    physicalDataPath, fileType, fileName, excelSheetName, startLine, maxRecordsToRetrieve);
                return result;
            }
            else if ((fileType == "C"))
            {
                //create a schema to use before filling the adapter
                //DataTable dtReturn = new DataTable();
                //using (LumenWorks.Framework.IO.Csv.CachedCsvReader csv = new LumenWorks.Framework.IO.Csv.CachedCsvReader(new StreamReader(System.IO.Path.Combine(physicalDataPath, fileName)), true))
                //{
                //    string[] headers = csv.GetFieldHeaders();
                //    foreach(string s in headers)
                //    {
                //        dtReturn.Columns.Add(s);
                //    }

                //    while(csv.ReadNextRecord())
                //    {
                //        DataRow dr = dtReturn.NewRow();

                //        for(int i = 0; i < dtReturn.Columns.Count; i++)
                //        {
                //            dr[dtReturn.Columns[i].ColumnName] = csv[i];                             
                //        }
                //        dtReturn.Rows.Add(dr);
                //    }
                //}

                return getDatafromTXT(physicalDataPath, fileName, startLine, maxRecordsToRetrieve, ",").Tables[0];
                
                //ConstructSchema(physicalDataPath, fileName);
                //connString = getDatasource(physicalDataPath, fileName, fileType);
                //OleDbDataAdapter oleAdapter = new OleDbDataAdapter("SELECT * FROM " + fileName + " ", connString);
                //DataSet dataset = new DataSet(fileName);
                //oleAdapter.Fill(dataset, startLine, maxRecordsToRetrieve, fileName); //fill the adapter with rows only from the linenumber specified														
                //return dataset.Tables[fileName];
                //return dtReturn;
            }
            else if ((fileType == "O"))
            {
                return getDatafromTXT(physicalDataPath, fileName, startLine, maxRecordsToRetrieve, delimiter).Tables[0];
            }
            else if (fileType.ToUpper() == "XML")
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml(System.IO.Path.Combine(physicalDataPath, fileName));
                List<string> columns = new List<string>();
                foreach(DataColumn dc in dataset.Tables[0].Columns)
                {
                    if(!columns.Contains(dc.ColumnName.ToLower()))
                    {
                        columns.Add(dc.ColumnName.ToLower());
                    }
                    else
                    {
                        List<ECN_Framework_Common.Objects.ECNError> errorList = new List<Objects.ECNError>();
                        errorList.Add(new Objects.ECNError(Objects.Enums.Entity.Email, Objects.Enums.Method.Validate, "Duplicate column names"));
                        throw new ECN_Framework_Common.Objects.ECNException(errorList);
                    }
                }
                return dataset.Tables[0];
            }
            throw new ArgumentException(string.Format("Unknown file type '{0}'.", fileType));
        }

        private static DataSet getDatafromTXT(string physicalDataPath, string fileName, int startLine, int maxRecordsToRetrieve, string delimiterBy)
        {
            string delimiter = string.Empty;

            if (delimiterBy.ToUpper() == "TABDELIMITED")
                delimiter = "\t";
            else
                delimiter = ",";

            //The DataSet to Return
            DataSet ds = new DataSet();

            //Open the file in a stream reader.
            using (StreamReader s = new StreamReader(physicalDataPath + "\\" + fileName))
            {

                //Split the first line into the columns       
                string[] columns = s.ReadLine().Split(delimiter.ToCharArray());
                //Add the new DataTable to the RecordSet
                ds.Tables.Add(fileName);

                //Cycle the colums, adding those that don't exist yet and sequencing the one that do.
                try
                {
                    for (int i = 0; i < columns.Length; i++)
                    {
                        string columnName = columns[i];
                        if (columnName.StartsWith("\"") && columnName.EndsWith("\""))
                            columnName = columnName.TrimStart('\"').TrimEnd('\"');
                        ds.Tables[fileName].Columns.Add(columnName);
                    }
                }
                catch (Exception ex)
                {
                    List<ECN_Framework_Common.Objects.ECNError> errorList = new List<Objects.ECNError>();
                    errorList.Add(new Objects.ECNError(Objects.Enums.Entity.Email, Objects.Enums.Method.Validate, "Duplicate column names"));
                    throw new ECN_Framework_Common.Objects.ECNException(errorList);
                }

                s.DiscardBufferedData();
                s.BaseStream.Seek(0, SeekOrigin.Begin);
                s.BaseStream.Position = 0;


                //Read the rest of the data in the file.        
                string AllData = s.ReadToEnd();
                string[] rows = AllData.Split("\r\n".ToCharArray());

                int rowcount = 0;

                foreach (string r in rows)
                {
                    if (rowcount < maxRecordsToRetrieve || maxRecordsToRetrieve == 0)
                    {
                        if (r != string.Empty)
                        {
                            try
                            {
                                //Split the row at the delimiter.
                                string[] items = r.Split(delimiter.ToCharArray());
                                for (int i = 0; i < items.Length; i++)
                                {
                                    string sQ = items[i];
                                    if (sQ.StartsWith("\"") && sQ.EndsWith("\""))
                                    {
                                        items[i] = sQ.TrimStart('"').TrimEnd('"');
                                    }
                                }
                                ds.Tables[fileName].Rows.Add(items);
                            }
                            catch (Exception ex)
                            {
                                string error = ex.Message;
                            }
                            rowcount++;

                        }
                    }
                }

                s.Dispose();
            }

            return ds;
        }

        private static string getDatasource(string path, string fileName, string filetype)
        {
            var connStr = ImportHelper.GetDataSource(path, fileName, filetype, false, true);
            return connStr;
        }
    }
}
