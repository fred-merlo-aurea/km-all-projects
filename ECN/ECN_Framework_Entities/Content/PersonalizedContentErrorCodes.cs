using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Content
{
    [Serializable]
    [DataContract]
    public class PersonalizedContentErrorCodes
    {
        public PersonalizedContentErrorCodes()
        {

        }

        [DataMember]
        public int ErrorCode { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
