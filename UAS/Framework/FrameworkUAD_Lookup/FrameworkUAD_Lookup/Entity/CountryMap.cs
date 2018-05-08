using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class CountryMap
    {
        public CountryMap() { }
        #region Properties
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string CountryDirty { get; set; }
        #endregion
    }
}
