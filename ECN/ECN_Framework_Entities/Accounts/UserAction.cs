using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class UserAction
    {
        public UserAction()
        {
            UserActionID = -1;
            UserID = null;
            ActionID = null;
            Active = string.Empty;
        }

        [DataMember]
        public int UserActionID { get; set; }
        [DataMember]
        public int?  UserID { get; set; }
        [DataMember]
        public int? ActionID { get; set; }
        [DataMember]
        public string Active { get; set; }
    }
}
