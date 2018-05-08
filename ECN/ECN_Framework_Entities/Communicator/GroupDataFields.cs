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
    public class GroupDataFields
    {
        public GroupDataFields()
        {
            GroupDataFieldsID = -1;
            GroupID = -1;
            ShortName = string.Empty;
            LongName = string.Empty;
            SurveyID = null;
            DatafieldSetID = null;
            IsPublic = string.Empty;
            IsPrimaryKey = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
            DataValuesList = null;
        }

        [DataMember]
        public int GroupDataFieldsID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string LongName { get; set; }
        [DataMember]
        public int? SurveyID { get; set; }
        [DataMember]
        public int? DatafieldSetID { get; set; }
        [DataMember]
        public string IsPublic { get; set; }
        [DataMember]
        public bool? IsPrimaryKey { get; set; }
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
        //optional
        public List<EmailDataValues> DataValuesList { get; set; }

        public GroupDataFieldsDefault DefaultValue { get; set; }
    }
}
