using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    public class BlastSetupInfo
    {
        public BlastSetupInfo()
        {
            BlastScheduleID = null;
            ScheduleType = string.Empty;
            IsTestBlast = null;
            SendNowIsAmount = null;
            SendNowAmount = null;
            SendTime = null;
            BlastFrequency = string.Empty;
            BlastType = string.Empty;
            SendTextTestBlast = false;
        }
        public int? BlastScheduleID { get; set; }
        public string ScheduleType { get; set; }
        public bool? IsTestBlast { get; set; }
        public bool? SendNowIsAmount { get; set; }
        public int? SendNowAmount { get; set; }
        public DateTime? SendTime { get; set; }
        public string BlastFrequency { get; set; }

        public string BlastType { get; set; }

        public bool? SendTextTestBlast { get; set; }
    }
}
