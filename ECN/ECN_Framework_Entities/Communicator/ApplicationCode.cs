using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class ApplicationCode
    {
        public ApplicationCode()
        {
            AppCodeID = -1;
            CodeType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.UNKNOWN;
            CodeValue = string.Empty;
            IsDeleted = false;
        }

        [DataMember]
        public int AppCodeID { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode CodeType { get; set; }
        [DataMember]
        public string CodeValue { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }    
}
