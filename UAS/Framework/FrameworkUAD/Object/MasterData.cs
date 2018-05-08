using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class MasterData
    {
        #region Properties
        [DataMember]
        public int CodeSheetID { get; set; }
        [DataMember]
        public int MasterID { get; set; }
        [DataMember]
        public string MasterValue { get; set; }
        [DataMember]
        public string MasterDesc { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        #endregion
    }
}