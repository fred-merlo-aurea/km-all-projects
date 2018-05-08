using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Report
{
    [Serializable]
    [DataContract]
    public class AcsImportCount
    {
        public AcsImportCount() { }
        #region Properties
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string PublicationCode { get; set; }
        [DataMember]
        public int Xact21 { get; set; }
        [DataMember]
        public int Xact31 { get; set; }
        [DataMember]
        public int Xact32 { get; set; }
        #endregion
    }
}
