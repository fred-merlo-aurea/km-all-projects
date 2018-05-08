using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
//using UAD.API.Models.Reports.Blast;

namespace UAD.API.ExtentionMethods
{
    public static class DataTableExtentionMethods
    {
        public static IEnumerable<Dictionary<string, string>> ToSimpleDictionary(this DataTable dt, IEnumerable<string> fieldNames = null, IEnumerable<string> excludeFieldNames = null)
        {
            if (fieldNames == null)
            {
                fieldNames = (from DataColumn c in dt.Columns select c.ColumnName).ToArray();
            }
            if( null != excludeFieldNames)
            {
                fieldNames = from f in fieldNames where false == excludeFieldNames.Contains(f) select f;
            }
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (string column in fieldNames)
                {
                    dictionary[column] = (dataRow.Field<object>(column) ?? "").ToString();
                }
                yield return dictionary;
            }
        }

        

       
    }
}