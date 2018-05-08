using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ECN_Framework_Common.Functions
{
    public class DataTableFunctions
    {


        // method used for Data Mapper 
        // Datatable as a param (method overload 1)
        public static ArrayList GetDataTableColumns(DataTable dataTable)
        {
            int nColumns = dataTable.Columns.Count;
            ArrayList columnHeadings = new ArrayList();
            DataColumn dataColumn = null;
            for (int i = 0; i < nColumns; i++)
            {
                dataColumn = dataTable.Columns[i];
                columnHeadings.Add(dataColumn.ColumnName.ToString());
            }
            return columnHeadings;
        }

        public static string ToCSV(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => string.Concat("\"", CleanString(column.ColumnName), "\""));
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => string.Concat("\"", CleanString(field.ToString()), "\""));
                sb.AppendLine(string.Join(",", fields));
            }

            return sb.ToString();
        }

        public static string ToTabDelimited(DataTable dt)
        {
            return ToCSV(dt).Replace("\",\"", "\"\t\"");
        }

        private static string CleanString(string text)
        {
            text = text.Replace("\"", "\"\"");
            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            return text;
        }
    }
}
