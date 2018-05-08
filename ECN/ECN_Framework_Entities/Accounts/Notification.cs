using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class Notification
    {
        public Notification()
        {
            NotificationID = -1;
            NotificationName = string.Empty;
            NotificationText = string.Empty;
            StartDate = null;
            StartTime = null;
            EndDate = null;
            EndTime = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            BackGroundColor = null;
            CloseButtonColor = null;
        }

        [DataMember]
        public string BackGroundColor { get; set; }
        [DataMember]
        public string CloseButtonColor { get; set; }
        [DataMember]
        public int NotificationID { get; set; }
        [DataMember]
        public string NotificationName { get; set; }
        [DataMember]
        public string NotificationText { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public string EndTime { get; set; }
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
    }
}
