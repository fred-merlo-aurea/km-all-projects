using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class EmailHistory
    {
        public EmailHistory()
        {
            OldEmailID = -1;
            NewEmailID = -1;
            Action = string.Empty;
            OldGroupID = -1;
            ActionTime = new DateTime();
        }

        public int OldEmailID { get; set; }

        public int NewEmailID { get; set; }

        public string Action { get; set; }

        public int OldGroupID { get; set; }

        public DateTime ActionTime { get; set; }
    }
}
