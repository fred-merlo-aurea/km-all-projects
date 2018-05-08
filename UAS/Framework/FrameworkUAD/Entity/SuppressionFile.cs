using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SuppressionFile
    {
        public SuppressionFile() 
        {
            SuppressionFileId = 0;
            FileName = string.Empty;
            FileDateModified = DateTime.Now;
            DateCreated = DateTime.Now;
            DateUpdated = null;
        }
        #region Properties
        [DataMember]
        public int SuppressionFileId { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public DateTime FileDateModified { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        #endregion
    }
}
