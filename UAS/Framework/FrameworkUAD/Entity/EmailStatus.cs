using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class EmailStatus
    {
        public EmailStatus() { }
        #region Properties
        [DataMember]
        public int EmailStatusID { get; set; }
        [DataMember]
        public string Status { get; set; }
        #endregion
    }
}
