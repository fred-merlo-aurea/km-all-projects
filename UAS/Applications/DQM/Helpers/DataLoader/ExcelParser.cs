using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using Excel;

namespace DQM.Helpers.DataLoader
{
    class ExcelParser
    {

        public static void xlsReader(string filePath, string fileName)
        {
            List<string> output = new List<string>();

            FileInfo fi = new FileInfo(fileName);

            DirectoryInfo di = new DirectoryInfo(filePath);
            FileStream stream = File.Open(filePath + fileName, FileMode.Open, FileAccess.Read);
            DataTable xlsDataTable = new DataTable();

            if (fi.Extension.ToLower().Equals(".xls"))
            {

                xlsDataTable = ProcessXLS(stream).Tables[0];

                foreach (DataColumn dc in xlsDataTable.Columns)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                    output.Add(dc.ColumnName);
                }

            }
            else if (fi.Extension.ToLower().Equals(".xlsx"))
            {

                xlsDataTable = ProcessXLSX(stream).Tables[0];

                foreach (DataColumn dc in xlsDataTable.Columns)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                    output.Add(dc.ColumnName);
                }

            }

            BulkInsertData.BulkInsertDataTable(fileName, xlsDataTable, output.ToArray());

        }


        private static DataSet ProcessXLS(FileStream stream)
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            return result;
        }
        private static DataSet ProcessXLSX(FileStream stream)
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            return result;
        }
    }
}
