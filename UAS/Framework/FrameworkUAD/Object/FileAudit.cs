using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class FileAudit
    {
        #region
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        #endregion
    }
}
