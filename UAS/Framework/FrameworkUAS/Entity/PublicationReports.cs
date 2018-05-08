using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class PublicationReports
    {
        #region Properties
        [DataMember]
        public int ReportID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public string ReportName { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public bool IsCrossTabSupport { get; set; }
        [DataMember]
        public string Row { get; set; }
        [DataMember]
        public string Column { get; set; }
        [DataMember]
        public bool SuppressTotal { get; set; }
        [DataMember]
        public bool Status { get; set; }
        #endregion
    }
}
