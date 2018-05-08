using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    /// <summary>
    /// should only be used for File processing errors or step notes
    /// </summary>
    [Serializable]
    [DataContract]
    public class FileLog
    {
        public FileLog() 
        {
            SourceFileID = -1;
            FileStatusTypeID = -1;
            Message = string.Empty;
            LogDate = DateTime.Now; 
            LogTime = DateTime.Now.TimeOfDay; 
        }
        public FileLog(int sourceFileID, int fileStatusTypeID, string message,string processCode)
        {
            SourceFileID = sourceFileID;
            FileStatusTypeID = fileStatusTypeID;
            Message = message;
            LogDate = DateTime.Now;
            LogTime = DateTime.Now.TimeOfDay;
            ProcessCode = processCode;
        }
        #region Properties
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public int FileStatusTypeID { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime LogDate { get; set; }
        [DataMember]
        public TimeSpan LogTime { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        #endregion
    }
}
