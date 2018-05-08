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
    public class VisitFile
    {
        public VisitFile()
        {
            EmailAddress = string.Empty;
            VisitTime = System.DateTime.Now;
            URL = string.Empty;
        }

        public string EmailAddress { get; set; }
        public DateTime VisitTime { get; set; }
        public string URL { get; set; }
    }
}
