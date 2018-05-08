using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class UserDataMask
    {
        public UserDataMask() { }
        #region Properties
        [DataMember]
        public int UserDataMaskID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string MaskField { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        #endregion
    }
}
