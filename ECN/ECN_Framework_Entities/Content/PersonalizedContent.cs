using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Content
{
    [Serializable]
    [DataContract]
    public class PersonalizedContent
    {
        public PersonalizedContent()
        {
            PersonalizedContentID = 0;
            BlastID = 0;
            EmailAddress = string.Empty;
            EmailSubject = string.Empty;
            HTMLContent = string.Empty;
            TEXTContent = string.Empty;
            IsValid = true;
            IsDeleted = false;
            IsProcessed = false;
            CreatedDate = DateTime.Now;
            CreatedUserID = 0;
            UpdatedUserID = null;
            UpdatedDate = null;
        }

        [DataMember]
        public Int64 PersonalizedContentID { get; set; }
        [DataMember]
        public Int64 BlastID { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string HTMLContent { get; set; }
        [DataMember]
        public string TEXTContent { get; set; }
        [DataMember]
        public bool IsValid { get; set; }
        [DataMember]
        public bool IsProcessed { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; private set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
     
       
    }
}
