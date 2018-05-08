using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class FilterExportField : FilterBase
    {
        public FilterExportField()
        {
            FilterExportFieldId = 0;
            FilterScheduleId = 0;
            ExportColumn = string.Empty;
        }

        [DataMember]
        public int FilterExportFieldId { get; set; }
        [DataMember]
        public int FilterScheduleId { get; set; }
        [DataMember]
        public string ExportColumn { get; set; }
    }
}
