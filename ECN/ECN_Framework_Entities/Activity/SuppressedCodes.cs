using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity
{
    [Serializable]
    [DataContract]
    public class SuppressedCodes
    {
        public SuppressedCodes() 
        { 
        }
        
        [DataMember]
        public int SuppressedCodeID { get; set; }
        [DataMember]
        public string SupressedCode { get; set; }
    }
}
