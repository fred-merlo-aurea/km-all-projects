using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Data.OleDb;
using System.Globalization;
using KM.Common;


namespace DQM.Helpers.DataLoader
{
    public class TextParser
    {
        //Grab first row "Header Row" from file
        public static string outputColumns;

        public static void textFileRead(string filePath, string fileName)
        {

            TextFieldParser tfp = new TextFieldParser(filePath + fileName, Encoding.Default);
            tfp.Delimiters = new string[] { "\t" };

            tfp.HasFieldsEnclosedInQuotes = true;

            string[] fields = tfp.ReadFields();

            DataTable dt = new DataTable();

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] == "" || fields[i] == " ")
                {
                    fields[i] = "[blankField]";
                }
                dt.Columns.Add(fields[i]);
            }

            /* If text file was csv as .txt, first row count would only equal to 1 and thus would more than likely be csv and not text tab delimited */
            if (fields.Count() > 1)
            {
                string line;
                string[] columns;

                while ((line = tfp.ReadLine()) != null)
                {
                    columns = line.Split('\t');
                    dt.Rows.Add(columns);
                }
                tfp.Close();
                BulkInsertData.BulkInsertDataTable(fileName, dt, fields.ToArray());

            }
            else
            {
                tfp.Close(); // Close file so GetDataTableFromCsv can read the file
                textAsCSVRead(filePath, fileName, true);
            }

        }

        public static void textAsCSVRead(string filePath, string fileName, bool isFirstRowHeader)
        {

            TextFieldParser tfp = new TextFieldParser(filePath + fileName, Encoding.Default);
            tfp.Delimiters = new string[] { "," };
            tfp.HasFieldsEnclosedInQuotes = true;

            string[] fields = tfp.ReadFields();

            DataTable dt = new DataTable();

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] == "" || fields[i] == " ")
                {
                    fields[i] = "[blankField]";
                }

                dt.Columns.Add(fields[i]);
            }

            tfp.Close();

            if (fields.Count() > 1)
            {

                string header = isFirstRowHeader ? "Yes" : "No";

                string sql = @"SELECT * FROM [" + fileName + "]";

                using (OleDbConnection connection = new OleDbConnection(
                          @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath +
                          ";Extended Properties=\"Text;HDR=" + header + "\""))
                using (OleDbCommand command = new OleDbCommand(sql, connection))

                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Locale = CultureInfo.CurrentCulture;
                    Console.Out.Write(dataTable.ToString());
                    adapter.Fill(dataTable);

                    BulkInsertData.BulkInsertDataTable(fileName, dataTable, fields.ToArray());

                }
            }
            else
            {
                Console.Out.Write("Unknown file type");
            }

        }

        public void ParseFile(FileInfo fileInfo, Enums.ColumnDelimiter delimiter, bool isQuoteEncapsulated)
        {
            DataTable myData = LoadFile(fileInfo, delimiter, isQuoteEncapsulated);
            Dictionary<int, string> myColumns = ColumnMap(myData);

        }
        private Dictionary<int, string> ColumnMap(DataTable dtFile)
        {
            Dictionary<int, string> ColumnIndexList = new Dictionary<int, string>();
            int index = 0;
            foreach (DataColumn c in dtFile.Columns)
            {
                ColumnIndexList.Add(index, c.ColumnName);
                index++;
            }

            return ColumnIndexList;
        }
        private DataTable LoadFile(FileInfo file, Enums.ColumnDelimiter delimiter, bool isQuoteEncapsulated)
        {
            var tfp = new TextFieldParser(file.FullName)
            {
                TextFieldType = FieldType.Delimited
            };

            var delim = Enums.GetDelimiterSymbol(delimiter);
            if (delim.HasValue)
            {
                tfp.SetDelimiters(delim.Value.ToString());
            }

            if (isQuoteEncapsulated == true)
                tfp.HasFieldsEnclosedInQuotes = true;
            else
                tfp.HasFieldsEnclosedInQuotes = false;

            //convert to dataset:
            DataSet ds = new DataSet();
            ds.Tables.Add("Data");

            var stringRow = tfp.ReadFields();
            foreach (String field in stringRow)
            {
                ds.Tables[0].Columns.Add(field, Type.GetType("System.String"));
            }

            //populate with data:
            while (!tfp.EndOfData)
            {
                stringRow = tfp.ReadFields();
                ds.Tables[0].Rows.Add(stringRow);
            }

            tfp.Close();

            return ds.Tables["Data"];
        }
    }
}
