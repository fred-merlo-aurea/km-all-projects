using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    [Serializable]
    [DataContract]
    public class Reporting
    {
        public Reporting() 
        {
            PublicationID = -1;
            CategoryIDs = "";
            TransactionIDs = "";
            QSourceIDs = "";
            StateIDs = "";
            Regions = "";
            CountryIDs = "";
            Email = "";
            Fax = "";
            Phone = "";
            FromDate = "";
            ToDate = "";
            Year = "";
            Media = "";
            CategoryCodes = "";
            TransactionCodes = "";
            ResponseIDs = "";
            AdHocXML = "";
        }
        #region Properties
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public string CategoryIDs { get; set; }
        [DataMember]
        public string TransactionIDs { get; set; }
        [DataMember]
        public string QSourceIDs { get; set; }
        [DataMember]
        public string StateIDs { get; set; }
        [DataMember]
        public string Regions { get; set; }
        [DataMember]
        public string CountryIDs { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string Year { get; set; }
        [DataMember]
        public string Media { get; set; }
        [DataMember]
        public string CategoryCodes { get; set; }
        [DataMember]
        public string TransactionCodes { get; set; }
        [DataMember]
        public string ResponseIDs { get; set; }
        [DataMember]
        public string AdHocXML { get; set; }
        #endregion
    }
}
