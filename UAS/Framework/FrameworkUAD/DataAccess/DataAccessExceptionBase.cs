using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using FrameworkUAS.Entity;
using BusinessFileLog = FrameworkUAS.BusinessLogic.FileLog;
using StringFunctions = KM.Common.StringFunctions;

namespace FrameworkUAD.DataAccess
{
    internal class DataAccessExceptionBase
    {
        private const string SortedColumnMappings = "_sortedColumnMappings";
        private const string Items = "_items";
        private const string Metadata = "_metadata";
        private const string Column = "column";
        private const string Length = "length";
        private const int Minus99 = -99;

        internal static void LogException(Exception ex, SqlBulkCopy bulkCopy, string methodName, string matchingMessage, string errorPattern)
        {
            var message = StringFunctions.FormatException(ex);
            var fileLog = new BusinessFileLog();

            if (ex.Message.Contains(matchingMessage))
            {
                var match = Regex.Match(ex.Message, errorPattern);
                var index = Convert.ToInt32(match.Value) - 1;

                var fieldInfo = typeof(SqlBulkCopy).GetField(SortedColumnMappings, BindingFlags.NonPublic | BindingFlags.Instance);
                var sortedColumns = fieldInfo?.GetValue(bulkCopy);
                var items = sortedColumns?.GetType().GetField(Items, BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(sortedColumns) as object[];

                var itemdata = items?[index].GetType().GetField(Metadata, BindingFlags.NonPublic | BindingFlags.Instance);
                var metadata = itemdata?.GetValue(items[index]);

                var column = metadata?.GetType().GetField(Column, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(metadata);
                var length = metadata?.GetType().GetField(Length, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(metadata);
                var formatEx = $"Column: {column} contains data with a length greater than: {length}";
                fileLog.Save(new FileLog(Minus99, Minus99, formatEx, methodName));
            }

            fileLog.Save(new FileLog(Minus99, Minus99, message, methodName));
        }
    }
}
