using System;
using System.Linq;
using System.Runtime.Serialization;

namespace KMPlatform.Object
{
    [Serializable]
    [DataContract]
    public class FileLog
    {
        #region
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        #endregion
    }
}