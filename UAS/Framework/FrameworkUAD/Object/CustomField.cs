using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class CustomField
    {
        public CustomField() { }
        #region Properties
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        [DataMember]
        public bool IsMultipleValue { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsAdHoc { get; set; }
        #endregion
    }
}
