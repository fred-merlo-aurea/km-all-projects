using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class RegionMap
    {
        public RegionMap() { }
        #region Properties
        [DataMember]
        public int RegionID { get; set; }
        [DataMember]
        public string RegionDirty { get; set; }
        #endregion
    }
}
