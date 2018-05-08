using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.Reports.Blast
{
    [Serializable]
    [DataContract]
    [XmlRoot("BounceReport")]
    public class BounceReport
    {
        public BounceReport()
        {
            BounceTime = string.Empty;
        	EmailAddress = string.Empty;
        	BounceType = string.Empty;
        	BounceSignature = string.Empty;
        }
        #region Properties
    		[DataMember]		[XmlElement]
        public string BounceTime{ get; set; }		[DataMember]		[XmlElement]
        public string EmailAddress{ get; set; }		[DataMember]		[XmlElement]
        public string BounceType{ get; set; }		[DataMember]		[XmlElement]
        public string BounceSignature{ get; set; }
        #endregion
    }
}