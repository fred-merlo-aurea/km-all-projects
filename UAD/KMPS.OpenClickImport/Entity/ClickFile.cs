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
    public class ClickFile
    {
        public ClickFile()
        {
            Pubcode = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            EmailAddress = string.Empty;
            ClickTime = System.DateTime.Now;
            ClickURL = string.Empty;
            Alias = string.Empty;
            BlastID = 0;
            Subject = string.Empty;
            SendTime = DateTime.Parse("1/1/1900");

        }
        //PubCode,recipient_first_name,recipient_last_name,recipient_address,recipient_city,recipient_state,recipient_zip,recipient_email,link_date,link_url
        public string Pubcode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string EmailAddress { get; set; }
        public DateTime ClickTime { get; set; }
        public string ClickURL { get; set; }
        public string Alias { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public DateTime SendTime { get; set; }
    }
}
