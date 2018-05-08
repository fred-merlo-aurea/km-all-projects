using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class PubCode
    {
        #region
        [DataMember]
        public string Pubcode { get; set; }
        [DataMember]
        public string DataType { get; set; }
        #endregion
    }
}
