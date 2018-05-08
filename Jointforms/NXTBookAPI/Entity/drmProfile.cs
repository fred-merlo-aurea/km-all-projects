using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NXTBookAPI.Entity
{
    [Serializable]
    public class drmProfile
    {
    
        public string subscriptionid { get; set; }
        public bool update { get; set; }
        public bool noupdate { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string changepassword { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string organization { get; set; }
        public string extraparams { get; set; }
        public string access_type { get; set; }

        public string access_nbissues { get; set; }
        public string access_firstissue { get; set; }
        public string access_limitdate { get; set; }
        public string access_startdate { get; set; }
        public string access_enddate { get; set; }

        public List<drmBook> access_issues { get; set; }
    }
}
