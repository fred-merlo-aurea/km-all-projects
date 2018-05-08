using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    public class UserGroup
    {
        public UserGroup()
        {
             UGID = -1;
             UserID = -1;
             GroupID = -1;
             CustomerID = null;
             CreatedUserID = null;
             CreatedDate = null;
             UpdatedUserID = null;
             UpdatedDate = null;
             IsDeleted = null;
        }

        [DataMember]
        public int UGID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
    }
}
