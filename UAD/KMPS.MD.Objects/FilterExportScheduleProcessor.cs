using System;
using System.Collections.Specialized;
using System.Data;

namespace KMPS.MD.Objects
{
    public static class FilterExportScheduleProcessor
    {
        private const string ReferenceKey = "Reference";
        private const string ReferenceId2Key = "ReferenceID2";
        private const string ReferenceId1Key = "ReferenceID1";
        private const string ReferenceNameKey = "ReferenceName";
        private const string FilterExportSchedule = "FILTER EXPORT SCHEDULE";
        private const string FilterExportFormatString = "<a href=\'../main/FilterExport.aspx?FilterScheduleId={0}&FilterID={1}\'>{2}</a>";
        private const string ParameterKeySuffix = " : ";

        public static NameValueCollection ProcessDataReader(IDataReader dataReader)
        {
            var result = new NameValueCollection();
            while (dataReader.Read())
            {
                var provessValue  = !dataReader[ReferenceKey].ToString().Equals(FilterExportSchedule, StringComparison.InvariantCultureIgnoreCase)
                                ? dataReader[ReferenceNameKey].ToString()
                                : string.Format(
                                    FilterExportFormatString,
                                    dataReader[ReferenceId2Key],
                                    dataReader[ReferenceId1Key],
                                    dataReader[ReferenceNameKey]);

                result.Add(dataReader[ReferenceKey] + ParameterKeySuffix, provessValue);
            }

            return result;
        }
    }
}
