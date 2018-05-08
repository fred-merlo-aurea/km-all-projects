using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher.Report
{
    [Serializable]
    [DataContract]
    public class ActivityTopClicksDownload
    {
        public ActivityTopClicksDownload() 
        { 
        }

        public int PageNumber { get; set; }
        public string link { get; set; }
        public DateTime Actiondate { get; set; }
        public int emailID { get; set; }
        public string Emailaddress { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string Occupation { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Voice { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Age { get; set; }
        public string Income { get; set; }
        public string Gender { get; set; }
    }
}
