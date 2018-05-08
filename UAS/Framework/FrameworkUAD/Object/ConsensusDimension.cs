using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class ConsensusDimension
    {
        public ConsensusDimension() 
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Company = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zipcode = string.Empty;
            Country = string.Empty;
            Email = string.Empty;
            ActivityDescription = string.Empty;
            ActivityResponse = string.Empty;
        }
        #region 
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zipcode { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string ActivityDescription { get; set; }
        [DataMember]
        public string ActivityResponse { get; set; }
        #endregion
    }
   
}
