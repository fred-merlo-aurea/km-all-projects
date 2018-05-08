using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Report
{
    [Serializable]
    [DataContract]
    public class FileCount
    {
        public FileCount() { }
        #region Properties
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int OriginalCount { get; set; }
        [DataMember]
        public int InvalidCount { get; set; }
        [DataMember]
        public int TransformedCount { get; set; }
        [DataMember]
        public int ArchivedCount { get; set; }
        [DataMember]
        public int FinalCount { get; set; }
        #endregion
    }
}
