using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class RegionGroup
    {
        public RegionGroup() 
        {
            RegionGroupID = 0;
            RegionGroupName = string.Empty;
            Sortorder = 0;
        }

        #region Properties
        [DataMember]
        public int RegionGroupID { get; set; }
        [DataMember]
        public string RegionGroupName { get; set; }
        [DataMember]
        public int Sortorder { get; set; }
        #endregion
    }
}
