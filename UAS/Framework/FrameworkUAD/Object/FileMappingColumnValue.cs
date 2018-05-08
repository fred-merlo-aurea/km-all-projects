using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class FileMappingColumnValue
    {
        public FileMappingColumnValue() { }
        #region Properties
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ItemText { get; set; }
        [DataMember]
        public string ItemValue { get; set; }
        [DataMember]
        public int ItemOrder { get; set; }
        #endregion
    }
}
