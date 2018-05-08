using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    public class BlastRegular : BlastAbstract
    {
        public Blast _Blast;

        public BlastRegular()
            : base()
        {
            
        }
    }
}
