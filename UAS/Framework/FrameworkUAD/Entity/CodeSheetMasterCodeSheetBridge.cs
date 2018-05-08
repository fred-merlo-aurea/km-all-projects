using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class CodeSheetMasterCodeSheetBridge
    {
        public CodeSheetMasterCodeSheetBridge() { }

        #region Properties
        [DataMember]
        public int CodeSheetID { get; set; }
        [DataMember]
        public int MasterID { get; set; }
        #endregion
    }
}
