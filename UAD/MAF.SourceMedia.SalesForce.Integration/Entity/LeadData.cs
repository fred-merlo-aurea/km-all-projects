using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.SourceMedia.SalesForce.Integration.Entity
{
    [Serializable]
    public class LeadData
    {
        public string acronym { get; set; }
        public string e_mail { get; set; }

        public string f_name { get; set; }
        public string l_name { get; set; }
        public string title { get; set; }
        public string company { get; set; }
        
        public string street { get; set; }
        public string addr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        public string bcode { get; set; }  //BUSINESS DEMO FIELD
        public string btitle { get; set; }
        public string SourceCode { get; set; }
        public double Rate { get; set; }
        public string trackcode { get; set; }
        public string trial_expire_date { get; set; }

    }
}
