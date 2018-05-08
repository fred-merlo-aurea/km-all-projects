using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace KMPS.MD.Helpers
{
    public static class JsonHelper
    {
        /// <summary>
        /// Builds a json string from a data table
        /// </summary>
        /// <param name="dataTable">data table to convert</param>
        /// <returns>Json string</returns>
        public static string GetJsonStringFromDataTable(DataTable dataTable)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }

            var dataColumns = new string[dataTable.Columns.Count];
            var header = new StringBuilder();

            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                dataColumns[i] = dataTable.Columns[i].Caption;
                header.AppendFormat("\"{0}\":\"{0}{1}¾\",", dataColumns[i], i);
            }

            var headerTemplate = header.Remove(header.Length - 1, 1).ToString();

            var builder = new StringBuilder();
            builder.Append("{\"" + dataTable.TableName + "\":[");

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                var row = headerTemplate;
                builder.Append("{");

                for (var j = 0; j < dataTable.Columns.Count; j++)
                {
                    row = row.Replace(dataTable.Columns[j] + j.ToString() + "¾", dataTable.Rows[i][j].ToString());
                }

                builder.Append(row + "},");
            }

            builder.Remove(builder.Length - 1, 1);
            builder.Append("]}");

            return builder.ToString();
        }
    }
}