using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN.Communicator.Bounces
{
    public class BounceHolder
    {
        public BounceHolder()
        {

        }

        public int blastID { get; set; }

        public int emailID { get; set; }

        public string fileName { get; set; }

        public int MessageIndex { get; set; }

        public DateTime ReceivedDate { get; set; }

    }
}
