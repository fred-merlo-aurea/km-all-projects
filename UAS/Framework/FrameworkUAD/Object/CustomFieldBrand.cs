using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class CustomFieldBrand
    {
        public CustomFieldBrand() { }
        #region Properties
        [DataMember]
        public int BrandId { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public bool IsBrandDeleted { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        #endregion
    }
}
