using System.Collections.Generic;
using ECN_Framework_Entities.Activity.Report;

namespace ecn.communicator.main.blasts.Interfaces
{
    public interface IBlastReport
    {
        IList<BlastReport> Get(int blastID);
    }
}
