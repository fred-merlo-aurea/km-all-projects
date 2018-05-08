using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
        [Serializable]
    public class BlastSocial : BlastAbstract
    {
        public Blast _Blast;

        public BlastSocial()
            : base()
        {
        }
    }
}
