﻿using System;
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
    
        public string BounceTime{ get; set; }
        public string EmailAddress{ get; set; }
        public string BounceType{ get; set; }
        public string BounceSignature{ get; set; }
        #endregion
    }
}