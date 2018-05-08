using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
        [Serializable]
    public class BlastABMaster
    {
        //public ECN_Framework_Entities.Communicator.Sample Sample;
        public BlastAbstract BlastA;
        public BlastAbstract BlastB;
        //validation
        //public List<ECNError> ErrorList { get; set; }

        public BlastABMaster()
        {
            //Sample = null;
            BlastA = null;
            BlastB = null;
            //ErrorList = new List<ECNError>();
        }
    }
}
