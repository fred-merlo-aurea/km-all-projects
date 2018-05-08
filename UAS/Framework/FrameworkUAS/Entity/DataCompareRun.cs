using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareRun
    {
        public DataCompareRun()
        {
            DcRunId = 0;
            ClientId = 0;
            SourceFileId = 0;
            FileRecordCount = 0;
            MatchedRecordCount = 0;
            UadConsensusCount = 0;
            ProcessCode = string.Empty;
            DateCreated = DateTime.Now;
            IsBillable = true;
            Notes = string.Empty;
            DcViews = new List<DataCompareView>();
        }
        public DataCompareRun(int clientId, int sourceFileId, string processCode)
        {
            DcRunId = 0;
            ClientId = clientId;
            SourceFileId = sourceFileId;
            FileRecordCount = 0;
            MatchedRecordCount = 0;
            UadConsensusCount = 0;
            ProcessCode = processCode;
            DateCreated = DateTime.Now;
            IsBillable = true;
            Notes = string.Empty;
            DcViews = new List<DataCompareView>();
        }
        #region Properties
        [DataMember]
        public int DcRunId { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int SourceFileId { get; set; }
        [DataMember]
        public int FileRecordCount { get; set; }
        [DataMember]
        public int MatchedRecordCount { get; set; }
        [DataMember]
        public int UadConsensusCount { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public bool IsBillable { get; set; }
        [DataMember]
        public string Notes { get; set; }
        #endregion

        public List<DataCompareView> DcViews { get; set; }
    }
}
