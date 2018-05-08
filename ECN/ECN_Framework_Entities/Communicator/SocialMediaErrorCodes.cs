using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public partial class SocialMediaErrorCodes
    {
        public SocialMediaErrorCodes()
        { }

        #region properties  
        [DataMember]
        public int mediaType { get; set; }
        [DataMember]
        public int errorCodeRepost { get; set; }
        [DataMember]
        public int errorCodeNoRepost { get; set; }
        [DataMember]
        public string errorMsg { get; set; }
        [DataMember]
        public string friendlyMsg { get; set; }
        #endregion
    }
}
