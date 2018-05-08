using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class Counts
    {
        public Counts()
        {
            this.FreeCopies = 0;
            this.FreeRecords = 0;
            this.PaidCopies = 0;
            this.PaidRecords = 0;
        }

        #region Properties
        [DataMember]
        public int FreeRecords { get; set; }
        [DataMember]
        public int PaidRecords{ get; set; }
        [DataMember]
        public int FreeCopies { get; set; }
        [DataMember]
        public int PaidCopies { get; set; }
        #endregion
    }
}
