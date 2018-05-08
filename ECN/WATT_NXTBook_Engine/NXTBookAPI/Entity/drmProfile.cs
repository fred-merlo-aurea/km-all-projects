using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WATT_NXTBook_Engine.NXTBookAPI.Entity
{
    [Serializable]
    public class drmProfile
    {
        public string subscriptionid { get; set; }
        public bool? update { get; set; }
        public bool? noupdate { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public string access_type { get; set; }

        
    }
}
