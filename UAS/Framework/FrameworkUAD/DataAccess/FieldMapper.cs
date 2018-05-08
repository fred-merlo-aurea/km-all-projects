using System;
using System.Collections.Generic;

namespace FrameworkUAD.DataAccess
{
    public static class FieldMapper
    {
        private static readonly Dictionary<string, string> _exportProductDisplayNameToColumnOrder = 
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["ADDRESS1"] = "Address",
                ["REGIONCODE"] = "State",
                ["ZIPCODE"] = "Zip",
                ["PUBTRANSACTIONDATE"] = "TransactionDate",
                ["QUALIFICATIONDATE"] = "QDate"
            };

        private static readonly Dictionary<string, string> _exportDefaultDisplayNameToColumnOrder =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["FNAME"] = "FirstName",
                ["LNAME"] = "LastName",
                ["ISLATLONVALID"] = "GeoLocated"
            };

        public static string GetColumnOrderByProductExportFieldDisplayName(string displayName)
        {
            string columnOrder;
            if (_exportProductDisplayNameToColumnOrder.TryGetValue(displayName, out columnOrder))
            {
                return columnOrder;
            }
            return displayName;
        }

        public static string GetColumnOrderByDefaultExportFieldDisplayName(string displayName)
        {
            string columnOrder;
            if (_exportDefaultDisplayNameToColumnOrder.TryGetValue(displayName, out columnOrder))
            {
                return columnOrder;
            }
            return displayName;
        }
    }
}
