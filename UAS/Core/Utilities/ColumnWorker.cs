using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using Excel;

namespace Core_AMS.Utilities
{
    public class ColumnWorker
    {
        private static int rowPreviewNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PreviewRows"]);

        public DataTable ClientIncomingColumnHandler(string FileName)
        {
            string headerLine;
            string rowLine;
            List<string> headerOutput = new List<string>();
            List<string> rowOutput = new List<string>();            
            int i = 0;
            int headerCount = 0;            
            string[] incomingColumns;            
            DataTable fileDataTable = new DataTable();            

            using (StreamReader reader = new StreamReader(FileName))
            {
                //First Row is header, add one to get next 10 rows of data rather than 9
                while (i < (rowPreviewNumber + 1))
                {
                    if (i == 0)
                    {
                        headerOutput.Clear();
                        headerLine = reader.ReadLine();
                        incomingColumns = headerLine.Split(',', '\t');
                        foreach (string col in incomingColumns)
                        {
                            headerOutput.Add(col);
                            fileDataTable.Columns.Add(col);
                        }
                        headerCount = headerOutput.Count;
                    }
                    else
                    {
                        rowOutput.Clear();
                        rowLine = reader.ReadLine();
                        if (rowLine != null)
                        {
                            incomingColumns = rowLine.Split(',', '\t');
                            foreach (string col in incomingColumns)
                            {
                                rowOutput.Add(col);
                            }
                            if (headerCount == rowOutput.Count)
                                fileDataTable.Rows.Add(rowOutput.ToArray());
                            else
                            {
                                fileDataTable.Clear();
                                break;
                            }
                        }
                    }

                    i++;
                }
            }            

            return fileDataTable;
        }

        static DataSet ProcessColumnXLS(FileStream stream)
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            return result;
        }

        public static DataTable ProcessColumnXLS(string file)
        {
            DataTable xlsDataTable = new DataTable();
            DataTable clientDataTable = new DataTable();            
            try
            {
                FileStream stream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read);                 
                DataSet ds = new DataSet();
                DataTable fileDataTable = new DataTable();

                xlsDataTable = ProcessColumnXLS(stream).Tables[0];

                foreach (DataColumn col in xlsDataTable.Columns)
                {
                    clientDataTable.Columns.Add(col.ToString());
                }
                for (int i = 0; i < rowPreviewNumber; i++)
                {
                    if (xlsDataTable.Rows[i] != null)
                        clientDataTable.ImportRow(xlsDataTable.Rows[i]);

                }

                stream.Close();
            }
            catch (Exception)
            {

            }
            return clientDataTable;
        }

        static DataSet ProcessColumnXLSX(FileStream stream)
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            excelReader.Close();
            return result;
        }

        public static DataTable ProcessColumnXLSX(string file)
        {
            DataTable xlsDataTable = new DataTable();
            DataTable clientDataTable = new DataTable();            
            try
            {
                FileStream stream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read);
                DataSet ds = new DataSet();

                xlsDataTable = ProcessColumnXLSX(stream).Tables[0];

                foreach (DataColumn col in xlsDataTable.Columns)
                {
                    string test = col.ToString();
                    clientDataTable.Columns.Add(col.ToString());
                }
                for (int i = 0; i < rowPreviewNumber; i++)
                {
                    if (xlsDataTable.Rows[i] != null)
                        clientDataTable.ImportRow(xlsDataTable.Rows[i]);

                }

                stream.Close();

            }
            catch (Exception)
            {

            }

            return clientDataTable;
        }

        public DataTable ClientIncomingExcelColumnHandler(string FileName)
        {
            if (Path.GetExtension(FileName).ToLower() == ".xls")
                return ProcessColumnXLS(FileName);
            else
                return ProcessColumnXLSX(FileName);

        }
    }
}
