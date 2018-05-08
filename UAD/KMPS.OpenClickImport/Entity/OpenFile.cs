using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.ActivityImport.Entity
{
    [DataContract]
    [Serializable]
    public class OpenFile
    {
        public OpenFile()
        {
            Pubcode = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            EmailAddress = string.Empty;
            OpenTime = System.DateTime.Now;
            BlastID = 0;
            Subject = string.Empty;
            SendTime = DateTime.Parse("1/1/1900");

        }
        //PubCode,recipient_first_name,recipient_last_name,recipient_address,recipient_city,recipient_state,recipient_zip,recipient_email,mr_opened
        [DataMember]
        public string Pubcode { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public DateTime OpenTime { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public DateTime SendTime { get; set; }
    }
}
