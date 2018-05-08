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
    public class CustomerNote
    {
        public CustomerNote()
        {
            NoteID = -1;
            CustomerID = -1;
            Notes = string.Empty;
            IsBillingNotes = null;
            CreatedBy = string.Empty;
            UpdatedBy = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;        
        }

        [DataMember]
        public int NoteID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public bool? IsBillingNotes { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }         
    }
}
