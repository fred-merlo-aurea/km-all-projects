using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Common.Objects
{
    public class BlastHoldException : Exception
    {
        public string NewStatus { get; set; }
        public int BlastID { get; set; }
        public int CustomerID { get; set; }

        public BlastHoldException():base()
        {

        }

        public BlastHoldException(string message, Exception ex, string newStatus,int blastID, int customerID)
            : base(message, ex)
        {
            NewStatus = newStatus;
            CustomerID = customerID;
            BlastID = blastID;
        }


    }
}
