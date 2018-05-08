using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.DigitalEdition
{
    [Serializable]
    [DataContract]
    public class DigitalSession
    {
        public DigitalSession()
        {
            PublicationCode = string.Empty;
            EditionID = -1;
            EmailID = -1;
            BlastID = -1;
            Totalpages = 0;
            IsLoginRequired = false;
            IsAuthenticated = false;
            Publicationtype = string.Empty;
            ImagePath = string.Empty;
        }
        [DataMember]
        public string PublicationCode { get; set; }
        [DataMember]
        public int EditionID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int Totalpages { get; set; }
        [DataMember]
        public bool? IsLoginRequired { get; set; }
        [DataMember]
        public bool IsAuthenticated { get; set; }
        [DataMember]
        public string Publicationtype { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
    }
}
