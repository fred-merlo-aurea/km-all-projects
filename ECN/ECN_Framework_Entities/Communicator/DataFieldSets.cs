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
    public class DataFieldSets
    {
        public DataFieldSets()
        {
            DataFieldSetID = -1;
            GroupID = -1;
            MultivaluedYN = string.Empty;
            Name = string.Empty;
            ErrorList = new List<ECNError>();
        }

        [DataMember]
        public int DataFieldSetID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public string MultivaluedYN { get; set; }
        [DataMember]
        public string Name { get; set; }
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
