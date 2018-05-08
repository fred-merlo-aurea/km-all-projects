using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class AdmsResultCount
    {
        public AdmsResultCount() { }
        #region Properties
        [DataMember]
        public int OriginalProfileCount { get; set; }
        [DataMember]
        public int OriginalDemoCount { get; set; }
        [DataMember]
        public int TransformedProfileCount { get; set; }
        [DataMember]
        public int TransformedDemoCount { get; set; }
        [DataMember]
        public int InvalidProfileCount { get; set; }
        [DataMember]
        public int InvalidDemoCount { get; set; }
        [DataMember]
        public int ArchiveProfileCount { get; set; }
        [DataMember]
        public int ArchiveDemoCount { get; set; }
        [DataMember]
        public int FinalProfileCount { get; set; }
        [DataMember]
        public int FinalDemoCount { get; set; }
        [DataMember]
        public int MatchedRecordCount { get; set; }
        [DataMember]
        public int UadConsensusCount { get; set; }
        #endregion
    }
}
