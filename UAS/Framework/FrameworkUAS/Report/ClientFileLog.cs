using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Report
{
    [Serializable]
    [DataContract]
    public class ClientFileLog
    {
        public ClientFileLog() { }
        #region Properties
        [DataMember]
        public int ClientID {get;set;}
        [DataMember]
        public string ClientName {get;set;}
        [DataMember]
		public int SourceFileID {get;set;}
        [DataMember]
        public string FileName{get;set;}
        [DataMember]
		public string FileStatusName {get;set;}
        [DataMember]
        public string FileStatusCode {get;set;} 
        [DataMember]
        public string FileStatusDescription {get;set;}
		[DataMember]
        public DateTime LogDate {get;set;}
        [DataMember]
        public TimeSpan LogTime {get;set;}
        [DataMember]
        public string Message { get; set; }
        #endregion
    }
}
