using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [DataContract]
    public class ActionBackUp
    {
        [DataMember]
        public int ActionBackUpID { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public int PubSubscriptionID { get; set; }
        [DataMember]
        public int PubCategoryID { get; set; }
        [DataMember]
        public int PubTransactionID { get; set; }
    }
}
