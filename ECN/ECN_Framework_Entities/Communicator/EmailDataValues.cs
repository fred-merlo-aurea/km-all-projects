using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class EmailDataValues
    {
        public EmailDataValues()
        {
            EmailDataValuesID = -1;
            EmailID = -1;
            GroupDataFieldsID = -1;
            DataValue = string.Empty;
            SurveyGridID = null;
            EntryID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            //IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public long EmailDataValuesID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public int GroupDataFieldsID { get; set; }
        [DataMember]
        public string DataValue { get; set; }
        [DataMember]
        public int? SurveyGridID { get; set; }
        [DataMember]
        public Guid? EntryID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        //[DataMember]
        //public bool? IsDeleted { get; set; }

        [DataMember]
        public int? CustomerID { get; set; }
    }
}
