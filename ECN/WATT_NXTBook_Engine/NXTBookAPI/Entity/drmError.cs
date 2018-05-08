using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WATT_NXTBook_Engine.NXTBookAPI.Entity
{
    [Serializable]
    public class drmError
    {
        public string faultString { get; set; }

        public drmProfile faultData { get; set; }
    }
}
