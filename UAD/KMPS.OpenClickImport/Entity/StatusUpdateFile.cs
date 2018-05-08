using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using KMPS.MD.Objects;

namespace KMPS.ActivityImport.Entity
{
    [DataContract]
    [Serializable]
    public class StatusUpdateFile
    {
        public StatusUpdateFile()
        {
            EmailAddress = string.Empty;
            EmailStatus = KMPS.MD.Objects.Enums.EmailStatus.Active;
        }

        public string EmailAddress { get; set; }
        public KMPS.MD.Objects.Enums.EmailStatus EmailStatus { get; set; }
    }
}
