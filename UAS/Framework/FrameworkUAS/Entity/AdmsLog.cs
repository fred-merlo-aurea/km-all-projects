using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


namespace FrameworkUAS.Entity
{
    /// <summary>
    /// Starts once a file picked up until post import to uad
    /// </summary>
    [Serializable]
    [DataContract]
    public class AdmsLog
    {
        public AdmsLog() 
        {
            AdmsLogId = 0;
            ClientId = 0;
            SourceFileId = 0;
            FileNameExact = string.Empty;
            FileStart = DateTime.Now;
            FileEnd = null;
            FileStatusId = 0;
            StatusMessage = string.Empty;
            AdmsStepId = 0;
            ProcessingStatusId = 0;
            ExecutionPointId = 0;
            RecordSource = string.Empty;
            ProcessCode = string.Empty;
            OriginalRecordCount = 0;
            OriginalProfileCount = 0;
            OriginalDemoCount = 0;
            TransformedRecordCount = 0;
            TransformedProfileCount = 0;
            TransformedDemoCount = 0;
            DuplicateRecordCount = 0;
            DuplicateProfileCount = 0;
            DuplicateDemoCount = 0;
            FailedRecordCount = 0;
            FailedProfileCount = 0;
            FailedDemoCount = 0;
            FinalRecordCount = 0;
            FinalProfileCount = 0;
            FinalDemoCount = 0;
            MatchedRecordCount = 0;
            UadConsensusCount = 0;
            FileLogDetails = new List<FileLog>();
            DQM = null;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = 0;
            ImportFile = null;
            ThreadId = 0;
        }
        #region Properties
        [DataMember]
        public int AdmsLogId { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int SourceFileId { get; set; }
        [DataMember]
        public string FileNameExact { get; set; }
        [DataMember]
        public DateTime FileStart { get; set; }
        [DataMember]
        public DateTime? FileEnd { get; set; }
        [DataMember]
        public int FileStatusId { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public int AdmsStepId { get; set; }
        [DataMember]
        public int ProcessingStatusId { get; set; }
        [DataMember]
        public int ExecutionPointId { get; set; }
        [DataMember]
        public string RecordSource { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public int OriginalRecordCount { get; set; }
        [DataMember]
        public int OriginalProfileCount { get; set; }
        [DataMember]
        public int OriginalDemoCount { get; set; }
        [DataMember]
        public int TransformedRecordCount { get; set; }
        [DataMember]
        public int TransformedProfileCount { get; set; }
        [DataMember]
        public int TransformedDemoCount { get; set; }
        [DataMember]
        public int DuplicateRecordCount { get; set; }
        [DataMember]
        public int DuplicateProfileCount { get; set; }
        [DataMember]
        public int DuplicateDemoCount { get; set; }
        [DataMember]
        public int FailedRecordCount { get; set; }
        [DataMember]
        public int FailedProfileCount { get; set; }
        [DataMember]
        public int FailedDemoCount { get; set; }
        [DataMember]
        public int FinalRecordCount { get; set; }
        [DataMember]
        public int FinalProfileCount { get; set; }
        [DataMember]
        public int FinalDemoCount { get; set; }
        [DataMember]
        public int IgnoredRecordCount { get; set; }
        [DataMember]
        public int IgnoredProfileCount { get; set; }
        [DataMember]
        public int IgnoredDemoCount { get; set; }
        [DataMember]
        public int DimensionRecordCount { get; set; }
        [DataMember]
        public int DimensionProfileCount { get; set; }
        [DataMember]
        public int MatchedRecordCount { get; set; }
        [DataMember]
        public int UadConsensusCount { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int UpdatedByUserID { get; set; }

        private List<FileLog> _fileLogDetails;
        [DataMember]
        public List<FileLog> FileLogDetails 
        {
            get
            {
                if (_fileLogDetails == null && !string.IsNullOrEmpty(ProcessCode))
                {
                    var fWrk = new BusinessLogic.FileLog();
                    _fileLogDetails = fWrk.SelectProcessCode(ProcessCode).ToList();
                }
                return _fileLogDetails;
            }
            set { _fileLogDetails = value; }
        }
        private DQMQue _dqmQue;
        [DataMember]
        public DQMQue DQM 
        {
            get
            {
                if(_dqmQue == null && !string.IsNullOrEmpty(ProcessCode))
                {
                    var qWrk = new BusinessLogic.DQMQue();
                    _dqmQue = qWrk.Select(ProcessCode);
                }
                return _dqmQue;
            } 
            set{ _dqmQue = value; }
        }
        [DataMember]
        public System.IO.FileInfo ImportFile { get; set; }
        [DataMember]
        public int ThreadId { get; set; }
        #endregion
    }
}
