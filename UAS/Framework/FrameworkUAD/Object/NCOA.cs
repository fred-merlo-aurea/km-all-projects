using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class NCOA
    {
        public NCOA() 
        {
            SequenceID = 0;
            Address1 = string.Empty;
            Address2 = string.Empty;
            City = string.Empty;
            RegionCode = string.Empty;
            ZipCode = string.Empty;
            Plus4 = string.Empty;
            ProductCode = string.Empty;
            PublisherID = 0;
            PublicationID = 0;
            ProcessCode = string.Empty;
        }
        #region Properties
        [DataMember]
        public int SequenceID { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string RegionCode { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string Plus4 { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public int PublisherID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        #endregion
    }
}
