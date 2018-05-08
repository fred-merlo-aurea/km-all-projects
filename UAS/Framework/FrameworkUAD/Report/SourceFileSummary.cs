﻿using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Report
{
    [Serializable]
    [DataContract]
    public class SourceFileSummary
    {
        public SourceFileSummary() { }
        #region Properties
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public DateTime LastUploaded { get; set; }
        [DataMember]
        public int OriginalCount { get; set; }
        [DataMember]
        public int TransformedCount { get; set; }
        [DataMember]
        public int ArchivedCount { get; set; }
        [DataMember]
        public int InvalidCount { get; set; }
        [DataMember]
        public int FinalCount { get; set; }
        [DataMember]
        public int MasterCount { get; set; }
        [DataMember]
        public int SubordinateCount { get; set; }
        [DataMember]
        public int SuppressedCount { get; set; }

        #endregion
    }
}
